/*
 * LLCryptoLib - Advanced .NET Encryption and Hashing Library
 * v.$id$
 * 
 * The contents of this file are subject to the license distributed with
 * the package (the License). This file cannot be distributed without the 
 * original LittleLite Software license file. The distribution of this
 * file is subject to the agreement between the licensee and LittleLite
 * Software.
 * 
 * Customer that has purchased Source Code License may alter this
 * file and distribute the modified binary redistributables with applications. 
 * Except as expressly authorized in the License, customer shall not rent,
 * lease, distribute, sell, make available for download of this file. 
 * 
 * This software is not Open Source, nor Free. Its usage must adhere
 * with the License obtained from LittleLite Software.
 * 
 * The source code in this file may be derived, all or in part, from existing
 * other source code, where the original license permit to do so.
 * 
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */


using System;
using System.Collections;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes the CRC hash for the input data using the managed library.</summary>
    public class CRC : System.Security.Cryptography.HashAlgorithm
    {
        static private Hashtable lookupTables;

        private CRCParameters parameters;
        private long[] lookup;
        private long checksum;
        private long registerMask;


        /// <summary>Initializes a new instance of the CRC class.</summary>
        /// <param name="param">The parameters to utilize in the CRC calculation.</param>
        /// <exception cref="ArgumentNullException" />
        public CRC(CRCParameters param)
            : base()
        {
            lock (this)
            {
                if (param == null) { throw new ArgumentNullException("param", "The CRCParameters cannot be null."); }
                parameters = param;
                HashSizeValue = param.Order;

                CRC.BuildLookup(param);
                lookup = (long[])lookupTables[param];
                if (param.Order == 64)
                {
                    registerMask = 0x00FFFFFFFFFFFFFF;
                }
                else
                {
                    registerMask = (long)(Math.Pow(2, (param.Order - 8)) - 1);
                }

                Initialize();
            }
        }


        // Pre-build the more popular lookup tables.
        static CRC()
        {
            lookupTables = new Hashtable();
            BuildLookup(CRCParameters.GetParameters(CRCStandard.CRC32));
        }


        /// <summary>Build the CRC lookup table for a given polynomial.</summary>
        static private void BuildLookup(CRCParameters param)
        {
            if (lookupTables.Contains(param))
            {
                // No sense in creating the table twice.
                return;
            }

            long[] table = new long[256];
            long topBit = (long)1 << (param.Order - 1);
            long widthMask = (((1 << (param.Order - 1)) - 1) << 1) | 1;

            // Build the table.
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = i;

                if (param.ReflectInput) { table[i] = Reflect((long)i, 8); }

                table[i] = table[i] << (param.Order - 8);

                for (int j = 0; j < 8; j++)
                {
                    if ((table[i] & topBit) != 0)
                    {
                        table[i] = (table[i] << 1) ^ param.Polynomial;
                    }
                    else
                    {
                        table[i] <<= 1;
                    }
                }

                if (param.ReflectInput) { table[i] = Reflect(table[i], param.Order); }

                table[i] &= widthMask;
            }

            // Add the new lookup table.
            lookupTables.Add(param, table);
        }


        /// <summary>Initializes the algorithm.</summary>
        override public void Initialize()
        {
            lock (this)
            {
                State = 0;
                checksum = parameters.InitialValue;
                if (parameters.ReflectInput)
                {
                    checksum = Reflect(checksum, parameters.Order);
                }
            }
        }


        /// <summary>Drives the hashing function.</summary>
        /// <param name="array">The array containing the data.</param>
        /// <param name="ibStart">The position in the array to begin reading from.</param>
        /// <param name="cbSize">How many bytes in the array to read.</param>
        override protected void HashCore(byte[] array, int ibStart, int cbSize)
        {
            lock (this)
            {
                for (int i = ibStart; i < (cbSize - ibStart); i++)
                {
                    if (parameters.ReflectInput)
                    {
                        checksum = ((checksum >> 8) & registerMask) ^ lookup[(checksum ^ array[i]) & 0xFF];
                    }
                    else
                    {
                        checksum = (checksum << 8) ^ lookup[((checksum >> (parameters.Order - 8)) ^ array[i]) & 0xFF];
                    }
                }
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        override protected byte[] HashFinal()
        {
            lock (this)
            {
                int i, shift, numBytes;
                byte[] temp;

                checksum ^= parameters.FinalXORValue;

                numBytes = (int)parameters.Order / 8;
                if (((int)parameters.Order - (numBytes * 8)) > 0) { numBytes++; }
                temp = new byte[numBytes];
                for (i = (numBytes - 1), shift = 0; i >= 0; i--, shift += 8)
                {
                    temp[i] = (byte)((checksum >> shift) & 0xFF);
                }

                return temp;
            }
        }


        /// <summary>Reflects the lower bits of the value provided.</summary>
        /// <param name="data">The value to reflect.</param>
        /// <param name="numBits">The number of bits to reflect.</param>
        /// <returns>The reflected value.</returns>
        static private long Reflect(long data, int numBits)
        {
            long temp = data;

            for (int i = 0; i < numBits; i++)
            {
                long bitMask = (long)1 << ((numBits - 1) - i);

                if ((temp & (long)1) != 0)
                {
                    data |= bitMask;
                }
                else
                {
                    data &= ~bitMask;
                }

                temp >>= 1;
            }

            return data;
        }
    }
}

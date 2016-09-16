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

namespace LLCryptoLib.Hash
{
    /// <summary>Computes the FNV hash for the input data using the managed library.</summary>
    public class FNV : System.Security.Cryptography.HashAlgorithm
    {
        private FNVParameters parameters;
        private ulong hash;


        /// <summary>Initializes a new instance of the FNV class.</summary>
        /// <param name="param">The parameters to utilize in the FNV calculation.</param>
        public FNV(FNVParameters param)
            : base()
        {
            lock (this)
            {
                if (param == null) { throw new ArgumentNullException("param", "The FNVParameters cannot be null."); }
                parameters = param;
                HashSizeValue = (int)param.Order;
                Initialize();
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        override public void Initialize()
        {
            lock (this)
            {
                State = 0;
                hash = (ulong)parameters.OffsetBasis;
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
                for (int i = ibStart; i < (ibStart + cbSize); i++)
                {
                    if (parameters.Variation == FNVAlgorithmType.FNV1)
                    {
                        hash *= (ulong)parameters.Prime;
                        hash ^= array[i];
                    }
                    else if (parameters.Variation == FNVAlgorithmType.FNV1A)
                    {
                        hash ^= array[i];
                        hash = hash * (ulong)parameters.Prime;
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
                byte[] temp = Utilities.ULongToByte(hash, EndianType.BigEndian);
                byte[] final = new byte[parameters.Order / 8];

                for (int i = (final.Length - 1); i >= 0; i--)
                {
                    final[i] = temp[temp.Length - ((parameters.Order / 8) - i)];
                }

                return final;
            }
        }
    }
}

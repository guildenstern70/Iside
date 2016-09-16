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
    /// <summary>Computes the BSD-style checksum for the input data using the managed library.</summary>
    public class SumBSD : System.Security.Cryptography.HashAlgorithm
    {
        private ushort checksum;


        /// <summary>Initializes a new instance of the SumBSD class.</summary>
        public SumBSD()
            : base()
        {
            lock (this)
            {
                HashSizeValue = 16;
                Initialize();
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        override public void Initialize()
        {
            lock (this)
            {
                State = 0;
                checksum = 0;
            }
        }


        /// <summary>Performs the hash algorithm on the data provided.</summary>
        /// <param name="array">The array containing the data.</param>
        /// <param name="ibStart">The position in the array to begin reading from.</param>
        /// <param name="cbSize">How many bytes in the array to read.</param>
        override protected void HashCore(byte[] array, int ibStart, int cbSize)
        {
            lock (this)
            {
                for (int i = ibStart; i < (ibStart + cbSize); i++)
                {
                    checksum = (ushort)((checksum >> 1) + ((checksum & 1) << 15));
                    checksum += (ushort)(array[i] & 0xFF);
                    checksum &= 0xFFFF;
                }
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        override protected byte[] HashFinal()
        {
            lock (this)
            {
                return Utilities.UShortToByte(checksum, EndianType.BigEndian);
            }
        }
    }
}

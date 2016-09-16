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
using System.Collections.Generic;
using System.Security.Cryptography;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes Parallel hashes for the input data using the managed libraries.</summary>
    public class ParallelHash : System.Security.Cryptography.HashAlgorithm
    {
        private List<HashAlgorithm> hashers;

        /// <summary>Initializes an instance of ParellelHash.</summary>
        /// <param name="hashers">The list of HashAlgorithms to use in the calculations.</param>
        public ParallelHash(params HashAlgorithm[] hashers)
            : base()
        {
            this.hashers = new List<HashAlgorithm>();
            for (int i = 0; i < hashers.Length; i++)
            {
                this.hashers.Add(hashers[i]);
                this.HashSizeValue += hashers[i].HashSize;
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        override public void Initialize()
        {
            lock (this)
            {
                State = 0;
                for (int i = 0; i < hashers.Count; i++)
                {
                    hashers[i].Initialize();
                }
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
                byte[] temp = new byte[array.Length];

                for (int i = 0; i < hashers.Count; i++)
                {
                    hashers[i].TransformBlock(array, ibStart, cbSize, temp, 0);
                }
            }
        }

        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1817:DoNotCallPropertiesThatCloneValuesInLoops")]
        override protected byte[] HashFinal()
        {
            lock (this)
            {
                byte[] hash = new byte[HashSize / 8];
                byte[] dummy = new byte[1];
                byte[] temp;
                int position = 0;

                for (int i = 0; i < hashers.Count; i++)
                {
                    hashers[i].TransformFinalBlock(dummy, 0, 0);
                    temp = ((System.Security.Cryptography.HashAlgorithm)hashers[i]).Hash;
                    Array.Copy(temp, 0, hash, position, temp.Length);
                    position += temp.Length;
                }

                return hash;
            }
        }
    }
}

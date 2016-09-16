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
	/// <summary>Computes the Adler-32 hash for the input data using the managed library.</summary>
	public class Adler32 : System.Security.Cryptography.HashAlgorithm 
    {
		private uint checksum;


		/// <summary>Initializes a new instance of the Adler32 class.</summary>
		public Adler32 () : base() 
        {
			lock (this) 
            {
				HashSizeValue = 32;
				Initialize();
			}
		}


		/// <summary>Initializes the algorithm.</summary>
		override public void Initialize() 
        {
			lock (this) 
            {
				State = 0;
				checksum = 1;
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
				int n;
				uint s1 = checksum & 0xFFFF;
				uint s2 = checksum >> 16;

				while (cbSize > 0) 
                {
					n = (3800 > cbSize) ? cbSize : 3800;
					cbSize -= n;

					while (--n >= 0) 
                    {
						s1 = s1 + (uint)(array[ibStart++] & 0xFF);
						s2 = s2 + s1;
					}

					s1 %= 65521;
					s2 %= 65521;
				}

				checksum = (s2 << 16) | s1;
			}
		}


		/// <summary>Performs any final activities required by the hash algorithm.</summary>
		/// <returns>The final hash value.</returns>
		override protected byte[] HashFinal() 
        {
			lock (this) 
            {
				return Utilities.UIntToByte(checksum, EndianType.BigEndian);
			}
		}
	}

    /// <summary>
    ///  LLCryptoLib.Hash library contains the functions to generate
    ///  hash codes (message digests) from any file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}

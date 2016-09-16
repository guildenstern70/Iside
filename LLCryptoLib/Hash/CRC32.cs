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
using System.IO;
using System.Collections;
using System.Security.Cryptography;

namespace LLCryptoLib.Hash
{
	/// <summary>
	/// CRC32
	/// </summary>
	public class CRC32 : HashAlgorithm
	{
		private static uint AllOnes = 0xffffffff;
		private static Hashtable cachedCRC32Tables;
		private static bool autoCache;
	
		private uint[] crc32Table; 
		private uint m_crc;
		
		/// <summary>
		/// Returns the default polynomial (used in WinZip, Ethernet, etc)
		/// </summary>
		public static uint DefaultPolynomial
		{
			get { return 0x04C11DB7; }
		}

		/// <summary>
		/// Gets or sets the auto-cache setting of this class.
		/// </summary>
		public static bool AutoCache
		{
			get { return autoCache; }
			set { autoCache = value; }
		}

		/// <summary>
		/// Initialize the cache
		/// </summary>
		static CRC32()
		{
			cachedCRC32Tables = Hashtable.Synchronized( new Hashtable() );
			autoCache = true;
		}

		/// <summary>
		/// Clear tables cache
		/// </summary>
		public static void ClearCache()
		{
			cachedCRC32Tables.Clear();
		}


		/// <summary>
		/// Builds a crc32 table given a polynomial
		/// </summary>
		/// <param name="ulPolynomial">A polynomial input</param>
		/// <returns>The CRC32 table as an array of integers</returns>
		protected static uint[] BuildCRC32Table( uint ulPolynomial )
		{
			uint dwCrc;
			uint[] table = new uint[256];

			// 256 values representing ASCII character codes. 
			for (int i = 0; i < 256; i++)
			{
				dwCrc = (uint)i;
				for (int j = 8; j > 0; j--)
				{
					if((dwCrc & 1) == 1)
						dwCrc = (dwCrc >> 1) ^ ulPolynomial;
					else
						dwCrc >>= 1;
				}
				table[i] = dwCrc;
			}

			return table;
		}


		/// <summary>
		/// Creates a CRC32 object using the DefaultPolynomial
		/// </summary>
		public CRC32() : this(DefaultPolynomial)
		{
		}

        /// <summary>
        /// Creates a CRC32 object using the specified Creates a CRC32 object
        /// </summary>
        /// <param name="aPolynomial">A polynomial.</param>
		public CRC32(uint aPolynomial) : this(aPolynomial, CRC32.AutoCache)
		{
		}

        /// <summary>
        /// Construct the CRC32 object
        /// </summary>
        /// <param name="aPolynomial">A polynomial.</param>
        /// <param name="cacheTable">if set to <c>true</c> uses cache table.</param>
		public CRC32(uint aPolynomial, bool cacheTable)
		{
			this.HashSizeValue = 32;

			crc32Table = (uint []) cachedCRC32Tables[aPolynomial];
			if ( crc32Table == null )
			{
				crc32Table = CRC32.BuildCRC32Table(aPolynomial);
				if ( cacheTable )
					cachedCRC32Tables.Add( aPolynomial, crc32Table );
			}
			Initialize();
		}
	
		/// <summary>
		/// Initializes an implementation of HashAlgorithm.
		/// </summary>
		public override void Initialize()
		{
			m_crc = AllOnes;
		}
	
		/// <summary>
		/// Updates and continues hash calculus
		/// </summary>
		/// <param name="buffer">Buffer of file to compute hash</param>
		/// <param name="ibStart">Offset start</param>
		/// <param name="count">Count of bytes</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#")]
        protected override void HashCore(byte[] buffer, int ibStart, int count)
		{
			// Save the text in the buffer. 
			for (int i = ibStart; i < count; i++)
			{
				ulong tabPtr = (m_crc & 0xFF) ^ buffer[i];
				m_crc >>= 8;
				m_crc ^= crc32Table[tabPtr];
			}
		}
	
		/// <summary>
		/// Return the CRC32 hash bytes
		/// </summary>
		/// <returns>The CRC32 hash bytes</returns>
		protected override byte[] HashFinal()
		{
			byte [] finalHash = new byte [ 4 ];
			ulong finalCRC = m_crc ^ AllOnes;
		
			finalHash[0] = (byte) ((finalCRC >> 24) & 0xFF);
			finalHash[1] = (byte) ((finalCRC >> 16) & 0xFF);
			finalHash[2] = (byte) ((finalCRC >>  8) & 0xFF);
			finalHash[3] = (byte) ((finalCRC >>  0) & 0xFF);
		
			return finalHash;
		}

        /// <summary>
        /// Computes the hash value for the specified Stream.
        /// </summary>
        /// <param name="inputStream">The input to compute the hash code for.</param>
        /// <returns>The computed hash code.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        new public byte[] ComputeHash(Stream inputStream)
		{
			byte [] buffer = new byte [4096];
			int bytesRead;
			while ( (bytesRead = inputStream.Read(buffer, 0, 4096)) > 0 )
			{
				HashCore(buffer, 0, bytesRead);
			}
			return HashFinal();
		}


        /// <summary>
        /// Overloaded. Computes the hash value for the input data.
        /// </summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <returns>The computed hash code.</returns>
		new public byte[] ComputeHash(byte[] buffer)
		{
			return ComputeHash(buffer, 0, buffer.Length);
		}

        /// <summary>
        /// Overloaded. Computes the hash value for the input data.
        /// </summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <param name="offset">The offset into the byte array from which to begin using data.</param>
        /// <param name="count">The number of bytes in the array to use as data.</param>
        /// <returns>The computed hash code.</returns>
		new public byte[] ComputeHash( byte[] buffer, int offset, int count )
		{
			HashCore(buffer, offset, count);
			return HashFinal();
		}
	}
}

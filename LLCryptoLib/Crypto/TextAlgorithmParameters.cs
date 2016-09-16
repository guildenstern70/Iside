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

using LLCryptoLib.Utils;

namespace LLCryptoLib.Crypto
{

	/// <summary>
	/// Class to share parameters between different crypto methods.
	/// </summary>
    /// <example><code>
    /// 
    ///         // TextROT13 TEXT TRANSFORMATION
    ///        TextAlgorithmParameters parms = new TextAlgorithmParameters(3);
    ///        TextCrypter textEncrypter = TextCrypterFactory.Create(SupportedTextAlgorithms.ROT13,parms);
    ///        string encrypted = textEncrypter.TextEncryptDecrypt(origString, true);
    ///        Console.WriteLine("Encrypted string: " + encrypted);
    ///        string decrypted = textEncrypter.TextEncryptDecrypt(encrypted, false);
    ///        Console.WriteLine("Decrypted string: " + decrypted);
	///		   Console.WriteLine(); 
    /// 
    /// </code></example>
    public class TextAlgorithmParameters
	{

		private string skey;
		private int spostamento;
		private short iBytes;
		private short iBlock;

		/// <summary>
		/// Constructor with shift.
		/// </summary>
		/// <param name="shift">Shift for caesar method (IE: shift=3, A+shift=D)</param>
		public TextAlgorithmParameters(int shift)
		{
			this.spostamento = shift;
		}

		/// <summary>
		/// Constructor with key
		/// </summary>
		/// <param name="key">key-string for single key</param>
        public TextAlgorithmParameters(string key)
		{
			this.skey = key;
		}

		/// <summary>
		/// Constructor with key and shift
		/// </summary>
		/// <param name="shift">Shift</param>
		/// <param name="key">TextVigenere</param>
        public TextAlgorithmParameters(int shift, string key) 
		{
			this.skey = key;
			this.spostamento = shift;
		}


		/// <summary>
		/// Constructor with key and shift
		/// </summary>
		/// <param name="shift">Shift</param>
		/// <param name="key">TextVigenere</param>
		/// <param name="keyBlock">Block of the key in bytes</param>
		/// <param name="keyBytes">Size of the key in bytes</param>
        public TextAlgorithmParameters(int shift, string key, short keyBytes, short keyBlock) 
		{
			this.skey = key;
			this.spostamento = shift;
			this.iBlock = keyBlock;
			this.iBytes = keyBytes;
		}

		/// <summary>
		/// Size of the key in bytes
		/// </summary>
		public short KeySize
		{
			get
			{
				return this.iBytes;
			}
		}

		/// <summary>
		/// Size of the block in bytes
		/// </summary>
		public short BlockSize
		{
			get
			{
				return this.iBlock;
			}
		}

		/// <summary>
		/// Get Shift
		/// </summary>
		public int Shift
		{
			get
			{
				return this.spostamento;
			}
		}

		/// <summary>
		/// Get TextVigenere
		/// </summary>
		public string Key
		{
			get
			{
				return this.skey;
			}
		}

		/// <summary>
		/// Get the shift given the position in the alphabetic sequence
		/// </summary>
		/// <param name="pos">Position in ALFABETO const string</param>
		/// <returns>Shift</returns>
		public int GetShiftAt(int pos)
		{
			int jpos = pos % skey.Length;
			char ch = skey[jpos];
			return Alpha.GetPosOf(ch);
		}

	}
}



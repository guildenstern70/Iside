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
	/// A TextCrypter object is used to encrypt or decrypt strings.
	/// </summary>
    /// <example><code>
    /// 
    ///  // TextROT13 TEXT TRANSFORMATION
    ///  TextAlgorithmParameters parms = new TextAlgorithmParameters(3);
    ///  TextCrypter textEncrypter = TextCrypterFactory.Create(SupportedTextAlgorithms.ROT13,parms);
    ///  string encrypted = textEncrypter.TextEncryptDecrypt(origString, true);
    ///  Console.WriteLine("Encrypted string: " + encrypted);
    ///  string decrypted = textEncrypter.TextEncryptDecrypt(encrypted, false);
    ///  Console.WriteLine("Decrypted string: " + decrypted);
    ///	 Console.WriteLine(); 
    /// </code></example>
	public class TextCrypter
	{

        private TextAlgorithm mtd;                 // Encryption Algorithm

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="algorithm">Encryption or decryption algorithm</param>
        internal TextCrypter(TextAlgorithm algorithm) 
        {
            this.mtd = algorithm;
        }

		/// <summary>
		/// Apply encryption algorithm to text. Output is a text string.
		/// </summary>
		/// <param name="text">Text to encode/decode</param>
		/// <param name="coding">If true, code, else decode</param>
		/// <returns>Encrypted/Decrypted Text</returns>
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
        public string TextEncryptDecrypt(string text, bool coding)
		{
            TextCryptEngine crypter = TextCrypter.Transform(text);
			return crypter.Transform(this.mtd,coding);
		}

		/// <summary>
		/// Apply encryption algorithm to text. Output is a Base64 text string.
		/// </summary>
		/// <param name="text">Text to encode/decode</param>
		/// <param name="coding">If true, code, else decode</param>
		/// <returns>Encrypted/Decrypted Text</returns>
		public string Base64EncryptDecrypt(string text, bool coding)
		{
            TextCryptEngine crypter = TextCrypter.Transform(text);
			return crypter.Transform(this.mtd, coding);
		}

		/// <summary>
		/// Apply encryption algorithm to text. Output is in hex numbers
		/// </summary>
		/// <param name="text">Text to encode/decode</param>
		/// <param name="coding">If true, code, else decode</param>
        /// <param name="style">Hexadecimal style to use</param>
		/// <returns>Encrypted/Decrypted Text</returns>
        public string HexEncryptDecrypt(string text, bool coding, HexEnum style)
		{
            TextCryptEngine crypter = TextCrypter.Transform(text);
			return crypter.HexTransform(this.mtd,coding,style);
		}

		/// <summary>
		/// The description of algorithm and its paramters
		/// </summary>
		/// <returns>The description of algorithm and its paramters</returns>
		public override string ToString()
		{
			return this.mtd.ToString();
		}

        private static TextCryptEngine Transform(string text)
		{
			return new TextCryptEngine(text);
		}
	}
}

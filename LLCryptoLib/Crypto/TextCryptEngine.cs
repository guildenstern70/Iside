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
using LLCryptoLib.Utils;

namespace LLCryptoLib.Crypto
{

	/// <summary>
	/// A TextCryptEngine is a transformation box that takes in input
    /// a string and returns a transformed string in output.
    /// The main method that do this is Transform(algorithm, crypt/decrypt).
    /// The engine applies algorithms by polymorphically apply the
    /// Code and Decode methods of an ITextEncryption object.
	/// </summary>
	internal class TextCryptEngine
	{

		private string pText;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">Text to crypt/decrypt</param>
		internal TextCryptEngine(string text)
		{
			pText = text;
		}

		/// <summary>
		/// Encrypt/decrypt a string, given a crypto method, into a scrambled text string.
		/// If the method is a text method (TextROT13, TextPlayfair, TextVigenere...) the coded
		/// string is text. If the method is a byte method (DES, Rijndael) the coded string
		/// is a Base64 string.
		/// </summary>
		/// <param name="mtd">Crypto method (shift, key, des...)</param>
		/// <param name="codeDecode">If true, code; else, decode</param>
		/// <returns>Transformed string</returns>
        internal string Transform(TextAlgorithm mtd, bool codeDecode)
		{
			string tmp;

			if (codeDecode)
			{
				tmp = mtd.Code(pText);
			}
			else
			{
				tmp = mtd.Decode(pText);
			}

			return tmp;
		}

		/// <summary>
		/// Encrypt or decrypt a string, given a crypto method, into a series of hex numbers
		/// </summary>
		/// <param name="algo">Crypto method (shift, key, des...)</param>
		/// <param name="codeDecode">If true, code; else, decode</param>
		/// <param name="style">Style of hexadecimal output</param>
		/// <returns>Transformed string</returns>
        internal string HexTransform(TextAlgorithm algo, bool codeDecode, HexEnum style)
		{
			string base64String;
			string output = null;

			if (codeDecode)
			{
				// Encode to base64
				base64String = Transform(algo,codeDecode);
				if (algo.AlgorithmType == TextAlgorithmType.BINARY)
				{
					// Transform base64 into bytes
					byte[] bytes = TextEncryptionUtils.Base64StringToBytes(base64String);
					// Hex output
					output = Hexer.BytesToHex(bytes, style);
				}
			}
			else
			{
				// pText is a Hex string: turn into bytes, then into bas64
				if (algo.AlgorithmType == TextAlgorithmType.BINARY)
				{
					byte[] bytes = Hexer.Hex2Bytes(this.pText);
					this.pText = Convert.ToBase64String(bytes);
				}

				output = Transform(algo,codeDecode);
			}
			return output;
		}

	}
}
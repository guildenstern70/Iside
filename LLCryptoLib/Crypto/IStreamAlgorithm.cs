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

using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
	/// <summary>
	/// IStreamAlgorithm. This is the base interface for file based/stream based encryption.
	/// </summary>
    /// <example>
    /// Encryption and decryption using stream algorithms:
    /// <code>
    /// // 1. Set algorithm
    /// IStreamAlgorithm cryptoAlgo = StreamAlgorithmFactory.Create(SupportedStreamAlgorithms.BLOWFISH);
    /// // 2. Encrypt 
    /// StreamCrypter crypter = new StreamCrypter(cryptoAlgo);
    /// crypter.GenerateKeys("littlelitesoftware");
    /// crypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    /// Console.WriteLine("File encrypted into " + encryptedFile);
    /// // 3. Decrypt
    /// StreamCrypter decrypter = new StreamCrypter(cryptoAlgo);
    /// crypter.GenerateKeys("littlelitesoftware");
    /// crypter.EncryptDecrypt(encryptedFile, decryptedFile, false, null);
    /// Console.WriteLine("File decrypted into " + decryptedFile);
    /// </code></example>
	[ComVisible(false)]
	public interface IStreamAlgorithm
	{
        /// <summary>
        /// The algorithm as an enum constant
        /// </summary>
        /// <see cref="SupportedStreamAlgorithms"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
        SupportedStreamAlgorithms SupportedAlgorithmID
		{
			get;
		}

        /// <summary>
        /// The algorithm as defined in System.Security.Cryptography
        /// </summary>
		SymmetricAlgorithm Algorithm
		{
			get;
		}

        /// <summary>
        /// Length of block in bytes
        /// </summary>
		short BlockLen
		{
			get;
		}

        /// <summary>
        /// Length of key in bytes
        /// </summary>
		short KeyLen
		{
			get;
		}
	}
}

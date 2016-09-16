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


namespace LLCryptoLib.Crypto
{
	/// <summary>
	/// Available symmetric encryption algorithms.
	/// </summary>
    /// <example><code>
    /// 
    /// // 1. Set algorithm
    /// IStreamAlgorithm cryptoAlgo = StreamAlgorithmFactory.Create(SupportedStreamAlgorithms.BLOWFISH);
    ///
    /// // 2. Encrypt 
    /// StreamCrypter crypter = new StreamCrypter(cryptoAlgo);
    /// crypter.GenerateKeys("littlelitesoftware");
    /// crypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    /// Console.WriteLine("File encrypted into " + encryptedFile);
    ///
    /// // 3. Decrypt
    /// StreamCrypter decrypter = new StreamCrypter(cryptoAlgo);
    /// crypter.GenerateKeys("littlelitesoftware");
    /// crypter.EncryptDecrypt(encryptedFile, decryptedFile, false, null);
    /// Console.WriteLine("File decrypted into " + decryptedFile);
    /// 
    /// </code></example>
	public enum SupportedStreamAlgorithms
	{
		/// <summary>
		/// DES
		/// </summary>
		DES,

		/// <summary>
		/// Triple DES or 3DES
		/// </summary>
		TRIPLEDES,

		/// <summary>
		/// Rijndael or AES 128 bit
		/// </summary>
        RIJNDAEL,

		/// <summary>
		/// AES or Rijndael 128 bit
		/// </summary>
		AES128,
		
		/// <summary>
		/// AES or Rijndael 192 bit
		/// </summary>
		AES192,

		/// <summary>
		/// AES or Rijndael 256 bit
		/// </summary>
		AES256,

		/// <summary>
		/// ARC4 128 bit
		/// </summary>
		ARC4,

		/// <summary>
		/// ARC4 512 bit
		/// </summary>
		ARC4512,

		/// <summary>
		/// ARC4 1024 bit
		/// </summary>
		ARC41024,

		/// <summary>
		/// ARC4 2048 bit
		/// </summary>
		ARC42048,

        /// <summary>
        /// Twofish 128 bit
        /// </summary>
        TWOFISH,

        /// <summary>
        /// Twofish 256 bit
        /// </summary>
        TWOFISH256,

        /// <summary>
        /// Blowfish 128 bit
        /// </summary>
        BLOWFISH,

        /// <summary>
        /// Blowfish 256 bit
        /// </summary>
        BLOWFISH256,

        /// <summary>
        /// Blowfish 448 bit
        /// </summary>
        BLOWFISH448,

        /// <summary>
        /// XOR
        /// </summary>
        XOR,

        /// <summary>
        /// CAST5 128 bit
        /// </summary>
        CAST5,

        /// <summary>
        /// THREEFISH 256 bit
        /// </summary>
        THREEFISH,

        /// <summary>
        /// THREEFISH 512 bit
        /// </summary>
        THREEFISH512,

        /// <summary>
        /// THREEFISH 1024 bit
        /// </summary>
        THREEFISH1024

	}
}

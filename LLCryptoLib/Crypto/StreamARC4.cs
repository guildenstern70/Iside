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
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{

    /// <summary>
    /// Base class for all ARC4 classes
    /// </summary>
    public abstract class StreamARC4Base : StreamAlgorithm
    {
        /// <summary>
        /// Size of block for all ARC4 algorithms.
        /// This is different in MONO and .NET.
        /// </summary>
#if MONO
        protected const int ARC4BLOCK = 8;
#else
        protected const int ARC4BLOCK = 1;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamARC4Base"/> class.
        /// </summary>
        /// <param name="eaKeyLen">Length of key in bytes</param>
        /// <param name="eaBlockLen">Length of block in bytes</param>
        /// <param name="eaDescription">Syntetic description of algorithm</param>
        protected StreamARC4Base(short eaKeyLen, short eaBlockLen, string eaDescription)
            : base(eaKeyLen, eaBlockLen, eaDescription) { }

        /// <summary>
        /// Returns an ARC4 object implementation
        /// </summary>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
#if MONO
				SymmetricAlgorithm sa = new Mono.Security.Cryptography.ARC4Managed();
#else
                SymmetricAlgorithm sa = new Security.Cryptography.ARCFourManaged();
#endif
                sa.KeySize = (this.KeyLen * 8);
                return sa;
            }
        }

        /// <summary>
        /// The algorithm ID as in SupportedStreamAlgorithms
        /// </summary>
        public override abstract SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get;
        }

    }

	/// <summary>
	/// ARC4 128bit.
	/// ARC4 is short for `Alleged RC4'. The real RC4 algorithm is proprietary to RSA Data Security Inc. 
	/// In September 1994, someone posted C code to both the Cypherpunks mailing list and to the Usenet 
	/// newsgroup @code{sci.crypt}, claiming that it implemented the RC4 algorithm. 
	/// This posted code is what it being called Alleged RC4, or ARC4 for short. 
	/// ARC4 is a private-key cipher; the same key is used to both encrypt and decrypt. 
	/// </summary>
    /// <example>
    /// <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamARC4();
    /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
    /// // Set symmetric password
    /// encrypter.GenerateKeys("littlelitesoftware");
    /// // Encrypt
    /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    /// Console.WriteLine("File encrypted into " + encryptedFile);
    /// </code>
    /// </example>
	[Serializable]
	[ComVisible(false)]
    public class StreamARC4 : StreamARC4Base
	{
		/// <summary>
		/// ARC4 128 bit constructor
		/// </summary>
        public StreamARC4()
            : base(16, ARC4BLOCK, "ARCFour (128bit)")
		{
		}

		/// <summary>
		/// Returns SupportedStreamAlgorithms.ARC4
		/// </summary>
		public override SupportedStreamAlgorithms SupportedAlgorithmID
		{
			get
			{
                return SupportedStreamAlgorithms.ARC4;  
			}
		}


	}

	/// <summary>
	/// ARC4 algorithm at 512bit
	/// </summary>
    /// <example>
    /// <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamARC4512();
    /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
    /// // Set symmetric password
    /// encrypter.GenerateKeys("littlelitesoftware");
    /// // Encrypt
    /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    /// Console.WriteLine("File encrypted into " + encryptedFile);
    /// </code>
    /// </example>
	[Serializable]
	[ComVisible(false)]
    public class StreamARC4512 : StreamARC4Base
	{
		/// <summary>
		/// Arc4 512 bit constructor
		/// </summary>
        public StreamARC4512()
            : base(64, ARC4BLOCK, "ARCFour (512bit)")
		{
		}

		/// <summary>
		/// Returns SupportedStreamAlgorithms.ARC4512
		/// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
		{
			get
			{
                return SupportedStreamAlgorithms.ARC4512;
			}
		}

	}

	/// <summary>
	/// ARC4 1024bit
	/// </summary>
    /// <example>
    /// <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamARC41024();
    /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
    /// // Set symmetric password
    /// encrypter.GenerateKeys("littlelitesoftware");
    /// // Encrypt
    /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    /// Console.WriteLine("File encrypted into " + encryptedFile);
    /// </code>
    /// </example>
	[Serializable]
	[ComVisible(false)]
    public class StreamARC41024 : StreamARC4Base
	{
		/// <summary>
		/// Arc4 1024 bit constructor
		/// </summary>
        public StreamARC41024()
            : base(128, ARC4BLOCK, "ARCFour (1024bit)")
		{
		}

		/// <summary>
		/// Returns SupportedStreamAlgorithms.ARC41024
		/// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
		{
			get
			{
                return SupportedStreamAlgorithms.ARC41024;
			}
		}

	}

	/// <summary>
	/// ARC4 2048bit
	/// </summary>
    /// <example>
    /// <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamARC42048();
    /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
    /// // Set symmetric password
    /// encrypter.GenerateKeys("littlelitesoftware");
    /// // Encrypt
    /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    /// Console.WriteLine("File encrypted into " + encryptedFile);
    /// </code>
    /// </example>
	[Serializable]
	[ComVisible(false)]
    public class StreamARC42048 : StreamARC4Base
	{
		/// <summary>
		/// Arc4 2048 constructor
		/// </summary>
        public StreamARC42048()
            : base(256, ARC4BLOCK, "ARCFour (2048bit)")
		{
		}

		/// <summary>
		/// Returns SupportedStreamAlgorithms.ARC42048
		/// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
		{
			get
			{
                return SupportedStreamAlgorithms.ARC42048;
			}
		}

	}
}

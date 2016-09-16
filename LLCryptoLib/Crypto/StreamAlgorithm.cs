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
	/// Encryption Algorithm.
	/// A StreamAlgorithm class indicates the algorithm to be used in Stream Encryption.
	/// <see cref="StreamCrypter"/>
	/// </summary>
    /// <example>
    /// <code>  
    /// // Set encryption algorithm
    /// StreamAlgorithm encryptAlgo = new StreamAES256();
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
	public abstract class StreamAlgorithm : IStreamAlgorithm
	{
		/// <summary>
		/// Block length
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected short pBlockLen;

		/// <summary>
		/// Key Length
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected short pKeyLen;

		/// <summary>
		/// Algorithm description
		/// </summary>
        private string description;

        /// <summary>
        /// Stream algorithm constructor.
        /// </summary>
        /// <param name="eaKeyLen">Length of key in bytes</param>
        /// <param name="eaBlockLen">Length of block in bytes</param>
        /// <param name="eaDescription">Syntetic description of algorithm</param>
		protected StreamAlgorithm(short eaKeyLen, short eaBlockLen, string eaDescription)
		{
			this.pBlockLen = eaBlockLen;
			this.pKeyLen = eaKeyLen;
			this.description = eaDescription;
		}

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String">string</see> that represents the current <see cref="T:System.Object">StreamAlgorithm</see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
		public override string ToString()
		{
			return this.description;
		}

        /// <summary>
        /// Length of key in bytes
        /// </summary>
		public short KeyLen
		{
			get
			{
				return this.pKeyLen;
			}
		}

        /// <summary>
        /// Length of block in bytes
        /// </summary>
		public short BlockLen
		{
			get
			{
				return this.pBlockLen;
			}
		}

        /// <summary>
        /// The algorithm ID as in SupportedStreamAlgorithms
        /// </summary>
        public abstract SupportedStreamAlgorithms SupportedAlgorithmID
		{
			get;
		}

        /// <summary>
        /// The algorithm as defined in System.Security.Cryptography
        /// </summary>
		public abstract SymmetricAlgorithm Algorithm
		{
			get;
		}
	}
}

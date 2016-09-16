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
    /// StreamBlowfish implements Blowfish for streams.
    /// Key len = 16 byte
    /// Block len = 8 byte
    /// </summary>
    /// <example>
    /// Perform a Blowfish stream encryption.
    /// <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamBlowfish();
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
    public class StreamBlowfish : StreamAlgorithm
    {

        /// <summary>
        /// StreamBlowfish constructor
        /// </summary>
        public StreamBlowfish()
            : base(16, 8, "Blowfish (128 bit)")
        {
        }

        /// <summary>
        /// Return SupportedStreamAlgorithms.BLOWFISH
        /// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get 
            {
                return SupportedStreamAlgorithms.BLOWFISH;
            }
        }

        /// <summary>
        /// Return a newly constructed SymmetricAlgorithm of type BlowfishManaged
        /// </summary>
        public override SymmetricAlgorithm Algorithm
        {
            get 
            {
                SymmetricAlgorithm sa = new BlowfishManaged();
                sa.BlockSize = (this.BlockLen * 8);
                sa.KeySize = (this.KeyLen * 8);
				sa.Padding = PaddingMode.PKCS7;
                return sa;
            }
        }
    }

    /// <summary>
    /// StreamBlowfish 256 bit
    /// Key len = 32 byte
    /// Block len = 8 byte
    /// <example>
    /// Perform a Blowfish stream encryption.
    /// <code>  
    /// Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamBlowfish256();
    /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
    /// // Set symmetric password
    /// encrypter.GenerateKeys("littlelitesoftware");
    /// // Encrypt
    /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    /// Console.WriteLine("File encrypted into " + encryptedFile);
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
	[ComVisible(false)]
    public class StreamBlowfish256 : StreamAlgorithm
    {

        /// <summary>
        /// StreamBlowfish 256 constructor
        /// </summary>
        public StreamBlowfish256()
            : base(32, 8, "Blowfish (256 bit)")
        {
        }

        /// <summary>
        /// Return SupportedStreamAlgorithms.BLOWFISH256
        /// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get
            {
                return SupportedStreamAlgorithms.BLOWFISH256;
            }
        }

        /// <summary>
        /// Return a newly constructed SymmetricAlgorithm of type BlowfishManaged
        /// </summary>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
                SymmetricAlgorithm sa = new BlowfishManaged();
                sa.BlockSize = (this.BlockLen * 8);
                sa.KeySize = (this.KeyLen * 8);
                sa.Padding = PaddingMode.PKCS7;
                return sa;
            }
        }
    }


    /// <summary>
    /// StreamBlowfish 448 bit
    /// Key len = 56 byte
    /// Block len = 8 byte
    /// <example>
    /// Perform a Blowfish stream encryption.
    /// <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamBlowfish448();
    /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
    /// // Set symmetric password
    /// encrypter.GenerateKeys("littlelitesoftware");
    /// // Encrypt
    /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    /// Console.WriteLine("File encrypted into " + encryptedFile);
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
	[ComVisible(false)]
    public class StreamBlowfish448 : StreamAlgorithm
    {

        /// <summary>
        /// StreamBlowfish 448 constructor
        /// </summary>
        public StreamBlowfish448()
            : base(56, 8, "Blowfish (448 bit)")
        {
        }

        /// <summary>
        /// Return SupportedStreamAlgorithms.BLOWFISH448
        /// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get
            {
                return SupportedStreamAlgorithms.BLOWFISH448;
            }
        }

        /// <summary>
        /// Return a newly constructed SymmetricAlgorithm of type BlowfishManaged
        /// </summary>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
                SymmetricAlgorithm sa = new BlowfishManaged();
                sa.BlockSize = (this.BlockLen * 8);
                sa.KeySize = (this.KeyLen * 8);
                sa.Padding = PaddingMode.PKCS7;
                return sa;
            }
        }
    }
}

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

namespace LLCryptoLib.Crypto
{
    /// <summary>
    /// A StreamAlgorithmFactory is a factory class to create
    /// IStreamAlgorithm objects based on their description or IDs.
    /// </summary>
	public sealed class StreamAlgorithmFactory
	{
		private StreamAlgorithmFactory() {}

        /// <summary>
        /// Create an IStreamAlgoritm object from its description.
        /// Description must match the one defined in the StreamAlgorithm
        /// Description field.
        /// </summary>
        /// <param name="description">The description, as in StreamAlgorithm Description field</param>
        /// <returns>A newly created IStreamAlgorithm object. If 'description' is not found, then returns null.</returns>
        /// <see cref="StreamAlgorithm"/>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.Create("DES");
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static IStreamAlgorithm Create(string description)
		{
			IStreamAlgorithm retAlgo = null;

			IStreamAlgorithm[] algos = StreamAlgorithmFactory.GetAlgos();
			foreach (IStreamAlgorithm encAlgo in algos)
			{
				if (encAlgo.ToString() == description)
				{
					retAlgo = encAlgo;
					break;
				}
			}

			return retAlgo;
		}

        /// <summary>
        /// Create an IStreamAlgoritm object from its SupportedStreamAlgorithms id.
        /// </summary>
        /// <param name="ssa">SupportedStreamAlgorithms id</param>
        /// <returns>A newly created IStreamAlgorithm object</returns>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.Create(SupportedStreamAlgorithms.AES128);
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
        public static IStreamAlgorithm Create(SupportedStreamAlgorithms ssa)
        {
            IStreamAlgorithm streamAlgo = null;

            switch (ssa)
            {
                case SupportedStreamAlgorithms.AES128:
                    streamAlgo = new StreamAES();
                    break;

                case SupportedStreamAlgorithms.AES192:
                    streamAlgo = new StreamAES192();
                    break;

                case SupportedStreamAlgorithms.AES256:
                    streamAlgo = new StreamAES256();
                    break;

                case SupportedStreamAlgorithms.ARC4:
                    streamAlgo = new StreamARC4();
                    break;

                case SupportedStreamAlgorithms.ARC41024:
                    streamAlgo = new StreamARC41024();
                    break;

                case SupportedStreamAlgorithms.ARC42048:
                    streamAlgo = new StreamARC42048();
                    break;

                case SupportedStreamAlgorithms.ARC4512:
                    streamAlgo = new StreamARC4512();
                    break;

                /* Future

                case SupportedStreamAlgorithms.TWOFISH256:
                    streamAlgo = new StreamTwofish256();
                    break;
                 */

                case SupportedStreamAlgorithms.DES:
                    streamAlgo = new StreamDES();
                    break;

                case SupportedStreamAlgorithms.RIJNDAEL:
                    streamAlgo = new StreamAES();
                    break;

                case SupportedStreamAlgorithms.TRIPLEDES:
                    streamAlgo = new Stream3DES();
                    break;

                case SupportedStreamAlgorithms.BLOWFISH:
                    streamAlgo = new StreamBlowfish();
                    break;

                case SupportedStreamAlgorithms.BLOWFISH256:
                    streamAlgo = new StreamBlowfish256();
                    break;

                case SupportedStreamAlgorithms.BLOWFISH448:
                    streamAlgo = new StreamBlowfish448();
                    break;

                case SupportedStreamAlgorithms.CAST5:
                    streamAlgo = new StreamCast();
                    break;

                case SupportedStreamAlgorithms.THREEFISH:
                    streamAlgo = new StreamThreeFish();
                    break;

                case SupportedStreamAlgorithms.THREEFISH512:
                    streamAlgo = new StreamThreeFish512();
                    break;

                case SupportedStreamAlgorithms.THREEFISH1024:
                    streamAlgo = new StreamThreeFish1024();
                    break;


                default:
                    throw new LLCryptoLibException("Unknown Stream Encryption Algorithm");
            }

            return streamAlgo;
        }

        /// <summary>
        /// Create a new StreamDES object
        /// </summary>
        /// <value>A DES object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.DES64;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static StreamDES DES64
		{
			get
			{
				return new StreamDES();
			}
		}

        /// <summary>
        /// Create a new Stream3DES object
        /// </summary>
        /// <value>A 3DES object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.TripleDES;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static Stream3DES TripleDES
		{
			get
			{
				return new Stream3DES();
			}
		}

        /// <summary>
        /// Create a new StreamAES object
        /// </summary>
        /// <value>A 128 bit AES-Rijndael object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.AES128;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static StreamAES AES128
		{
			get
			{
				return new StreamAES();
			}
		}

        /// <summary>
        /// Create a new StreamAES192 object
        /// </summary>
        /// <value>A 192 bit AES-Rijndael object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.AES192;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static StreamAES192 AES192
		{
			get
			{
				return new StreamAES192();
			}
		}

        /// <summary>
        /// Create a new StreamAES256 object
        /// </summary>
        /// <value>A 256 bit AES-Rijndael object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.AES256;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static StreamAES256 AES256
		{
			get
			{
				return new StreamAES256();
			}
		}

        /// <summary>
        /// Create a new StreamARC4 object
        /// </summary>
        /// <value>A 128 bit ARC4 object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.ArcFour;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static StreamARC4 ArcFour
		{
			get
			{
				return new StreamARC4();
			}
		}

        /// <summary>
        /// Create a new StreamARC4512 object
        /// </summary>
        /// <value>A 512 bit ARC4 object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.ARC4512;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static StreamARC4512 ArcFour512
		{
			get
			{
				return new StreamARC4512();
			}
		}

        /// <summary>
        /// Create a new StreamARC41024 object
        /// </summary>
        /// <value>A 1024 bit ARC4 object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.ArcFour1024;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static StreamARC41024 ArcFour1024
		{
			get
			{
				return new StreamARC41024();
			}
		}

        /// <summary>
        /// Create a new StreamARC42048 object
        /// </summary>
        /// <value>A 2048 bit ARC4 object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.ArcFour2048;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
		public static StreamARC42048 ArcFour2048
		{
			get
			{
				return new StreamARC42048();
			}
		}

        /// <summary>
        /// Create a new Blowfish object
        /// </summary>
        /// <value>A 128 bit Blowfish object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.Blowfish;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
        public static StreamBlowfish Blowfish
        {
            get
            {
                return new StreamBlowfish();
            }
        }


        /// <summary>
        /// Create a new CAST5 (aka CAST-128) object
        /// </summary>
        /// <value>A CAST5 object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.Blowfish;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
        public static StreamCast Cast5
        {
            get
            {
                return new StreamCast();
            }
        }

        /// <summary>
        /// Create a new Blowfish object
        /// </summary>
        /// <value>A 256 bit Blowfish object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.Blowfish256;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
        public static StreamBlowfish256 Blowfish256
        {
            get
            {
                return new StreamBlowfish256();
            }
        }

        /* Future

        /// <summary>
        /// Gets the Twofish symmetric encryption
        /// </summary>
        /// <value>The Twofish.</value>
        public static StreamTwofish Twofish
        {
            get
            {
                return new StreamTwofish();
            }
        }

        /// <summary>
        /// Gets the Twofish256 symmetric encryption
        /// </summary>
        /// <value>The Twofish256.</value>
        public static StreamTwofish256 Twofish256
        {
            get
            {
                return new StreamTwofish256();
            }
        }
         
         */

        /// <summary>
        /// Create a new Blowfish object
        /// </summary>
        /// <value>A 256 bit Blowfish object</value>
        /// <example>
        /// Stream Encryption with DES
        /// <code>  
        /// // Set encryption algorithm
        /// IStreamAlgorithm encryptAlgo = StreamAlgorithmFactory.Blowfish448;
        /// StreamCrypter encrypter = new StreamCrypter(encryptAlgo);
        /// // Set symmetric password
        /// encrypter.GenerateKeys("littlelitesoftware");
        /// // Encrypt
        /// encrypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
        /// Console.WriteLine("File encrypted into " + encryptedFile);
        /// </code>
        /// </example>
        public static StreamBlowfish448 Blowfish448
        {
            get
            {
                return new StreamBlowfish448();
            }
        }

        /*
        /// <summary>
        /// Create a new Twofish object
        /// </summary>
        public static StreamTwofish Twofish
        {
            get
            {
                return new StreamTwofish();
            }
        }

        /// <summary>
        /// Create a new Twofish256 object
        /// </summary>
        public static StreamTwofish Twofish256
        {
            get
            {
                return new StreamTwofish();
            }
        }
         */

        /// <summary>
        /// Get all available stream algoritms.
        /// </summary>
        /// <returns>An array of all available algorithms</returns>
		public static IStreamAlgorithm[] GetAlgos()
		{
			IStreamAlgorithm[] algos = { 
                                          new StreamDES(), 
										  new Stream3DES(),  
										  new StreamAES(), 
										  new StreamAES192(), 
										  new StreamAES256(),
										  new StreamARC4(), 
										  new StreamARC4512(),
										  new StreamARC41024(),
										  new StreamARC42048(),
                                          new StreamBlowfish(),
                                          new StreamBlowfish256(),
                                          new StreamBlowfish448()
									      };
			return algos;
		}
	}
}

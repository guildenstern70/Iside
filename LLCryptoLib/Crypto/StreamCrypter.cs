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
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
	/// <summary>
	/// This class wraps the operations on encryption streams.
	/// </summary>
    /// <example><code>
    /// 
    ///  // 1. Set algorithm
    ///  IStreamAlgorithm cryptoAlgo = StreamAlgorithmFactory.Create(SupportedStreamAlgorithms.BLOWFISH);
    ///
    ///  // 2. Encrypt 
    ///  StreamCrypter crypter = new StreamCrypter(cryptoAlgo);
    ///  crypter.GenerateKeys("littlelitesoftware");
    ///  crypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    ///  Console.WriteLine("File encrypted into " + encryptedFile);
    ///
    ///  // 3. Decrypt
    ///  StreamCrypter decrypter = new StreamCrypter(cryptoAlgo);
    ///  crypter.GenerateKeys("littlelitesoftware");
    ///  crypter.EncryptDecrypt(encryptedFile, decryptedFile, false, null);
    ///  Console.WriteLine("File decrypted into " + decryptedFile);
    /// 
    /// </code></example>
	public class StreamCrypter
	{

		/// <summary>
		/// The KeyFactory class generates symmetric algorithm keys
		/// according to the choosen algorithm
		/// </summary>
		class KeyFactory : LLCryptoLib.Crypto.TextAlgorithm
		{
			short keySize;
			short blockSize;

			/// <summary>
			/// KeyFactory constructor.
			/// </summary>
			/// <param name="sa">Encryption algorithm</param>
			/// <param name="keyLen">Length in bits of the key</param>
			/// <param name="blockLen">Length in bits of the block</param>
			internal KeyFactory(SymmetricAlgorithm sa, short keyLen, short blockLen) : base(TextAlgorithmType.BINARY)
			{
				this.keySize = keyLen;
				this.blockSize = blockLen;
				this.CheckKeySizes(sa);
			}

			/// <summary>
			/// This method transforms a normal string into a strong password
			/// by applying hash trasformation on that. The strong password is
			/// then used to create the key vector and the block vector to
			/// run the algorithm.
			/// </summary>
			/// <param name="password"></param>
			internal void GenerateKeys(string password)
			{
				this.GenerateKey(password,this.keySize,this.blockSize);
			}

			/// <summary>
			/// Get the computed key in bytes
			/// </summary>
			internal byte[] Key
			{
				get
				{
					return this.maKey;
				}
			}

			/// <summary>
			/// Get the computed block in bytes
			/// </summary>
			internal byte[] Block
			{
				get
				{
					return this.maIV;
				}
			}

			/// <summary>
			/// Returns null
			/// </summary>
			/// <param name="a">A string</param>
			/// <returns>Null</returns>
			public override string Code(string a)
			{
				return null;
			}

			/// <summary>
			/// Returns null
			/// </summary>
			/// <param name="b">A string</param>
			/// <returns>Null</returns>
			public override string Decode(string b)
			{
				return null;
			}

			private void CheckKeySizes(SymmetricAlgorithm sa)
			{

				bool sizeOk = false;
				bool blockOk = false;

				short keyLen = (short)(this.keySize*8);
				short ivLen = (short)(this.blockSize*8);

				// Check key size
				KeySizes[] ks = sa.LegalKeySizes;
				foreach (KeySizes k in ks)
				{
					if ((keyLen>=k.MinSize)&&(keyLen<=k.MaxSize))
					{
						sizeOk = true;
						break;
					}
				}

				// Check block size
				KeySizes[] bs = sa.LegalBlockSizes;
				foreach (KeySizes b in bs)
				{
					if ((ivLen>=b.MinSize)&&(ivLen<=b.MaxSize))
					{
						blockOk = true;
						break;
					}
				}

				if (!sizeOk)
				{
					throw new LLCryptoLibException("Key Size for choosen Algorithm not correct!");
				}

				if (!blockOk)
				{
					throw new LLCryptoLibException("Block Size for choosen Algorithm not correct!");
				}

			}
		}

		/// <summary>
		/// The size of the buffer when reading and writing files.
		/// </summary>
		public const short CACHESIZE = 4096;

		private KeyFactory keys;
		private SymmetricAlgorithm symmetricAlgo;
		private short keySize;
		private short blockSize;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:StreamCrypter"/> class.
        /// </summary>
        /// <param name="algo">an encryption algorithm</param>
		public StreamCrypter(IStreamAlgorithm algo)
		{
			this.SetEncryption(algo);
		}

        /// <summary>
        /// Gets the wrapped symmetric algorithm.
        /// </summary>
        /// <value>the encryption algorithm of this Stream Crypter</value>
        public SymmetricAlgorithm Algorithm
        {
            get
            {
                return this.symmetricAlgo;
            }
        }

		/// <summary>
		/// Generate keys starting from a string password. This method transforms a normal string into a strong password
		/// by applying hash trasformation on that. The strong password is
		/// then used to create the key vector and the block vector to
		/// run the algorithm.
		/// </summary>
		/// <param name="password">a given password</param>
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
		public void GenerateKeys(string password)
		{
			this.keys = new KeyFactory(this.symmetricAlgo, this.keySize, this.blockSize);
			this.keys.GenerateKeys(password);
		}

        /// <summary>
        /// Encrypt or decrypt a file into a MemoryStream
        /// </summary>
        /// <param name="inputFile">Path to input file</param>
        /// <param name="isCrypting">True if encryption, False if decryption</param>
        /// <param name="cbp">A feebback callback handle</param>
        /// <returns>the encrypted file as an array of bytes</returns>
        /// <exception cref="LLCryptoLibException" />
        public byte[] MemoryEncryptDecrypt(string inputFile, bool isCrypting, CallbackPoint cbp)
        {

            if (this.keys == null)
            {
                throw new LLCryptoLibException("Key is null. Set a password before encrypting data.");
            }

            if ((this.keys.Key.Length < 1) || (this.keys.Block.Length < 1))
            {
                throw new LLCryptoLibException("Keys not generated or null");
            }

            FileStream fin = null;
            MemoryStream fout = null;
            byte[] memBytes = null;

            try
            {
                fin = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
                fout = new MemoryStream();
                fout.SetLength(0);

                if (isCrypting)
                {
                    System.Diagnostics.Debug.WriteLine(">>> Memory Encrypting " + fin.Name);
                    memBytes = this.MemoryEncryptStream(fout, fin, cbp);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(">>> Memory Decrypting " + fin.Name);
                    memBytes = this.MemoryDecryptStream(fout, fin, cbp);
                }
            }
            catch (IOException exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                System.Diagnostics.Debug.WriteLine(exc.StackTrace);
            }
            finally
            {
                if (fout != null)
                {
                    fout.Close();
                }
                if (fin != null)
                {
                    fin.Close();
                }
            }

            return memBytes;

        }

		/// <summary>
		/// Encrypt or decrypt a file.
		/// </summary>
		/// <param name="inputFile">File to read. If isCrypting, clear file else encrypted file.</param>
		/// <param name="outputFile">File to write. if isCrypting, encrypted file.</param>
		/// <param name="isCrypting">True if encryption, false if decryption.</param>
		/// <param name="cbp">The feedback will be filled with 1 point each CACHE_SIZE wrote.</param>
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
        /// <exception cref="LLCryptoLibException" />
        /// <exception cref="ArgumentNullException" />
		public void EncryptDecrypt(string inputFile, string outputFile, bool isCrypting, CallbackPoint cbp)
		{

            if (inputFile == null)
            {
                throw new ArgumentNullException("inputFile");
            }

            if (outputFile == null)
            {
                throw new ArgumentNullException("outputFile");
            }

    		FileStream fin = null;
			FileStream fout = null;

			try
			{
				fin = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
				fout = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                this.EncryptDecrypt(fin, fout, isCrypting, cbp);

			}
			catch (IOException exc)
			{
                System.Diagnostics.Debug.WriteLine(">>> StreamCrypter.EncryptDecrypt(string,string) EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exc.Message);
                System.Diagnostics.Debug.WriteLine(exc.StackTrace);
			}
			finally
			{
				if (fout != null)
				{
					fout.Close();
				}
				if (fin != null)
				{
					fin.Close();                   
				}
			}
		}

        /// <summary>
        /// Encrypt or decrypt a file stream.
        /// </summary>
        /// <param name="inStream">File stream to read from. If isCrypting, clear file stream else a encrypted file stream.</param>
        /// <param name="outStream">File stream to write to. if isCrypting, a encrypted file stream.</param>
        /// <param name="isCrypting">True if encryption, false if decryption.</param>
        /// <param name="cbp">The feedback will be filled with 1 point each CACHE_SIZE wrote.</param>
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
        /// <exception cref="LLCryptoLibException" />
        /// <exception cref="ArgumentNullException" />
        public void EncryptDecrypt(FileStream inStream, FileStream outStream, bool isCrypting, CallbackPoint cbp)
        {
            if (inStream == null)
            {
                throw new ArgumentNullException("inStream");
            }

            if (outStream == null)
            {
                throw new ArgumentNullException("outStream");
            }

            if (this.keys == null)
            {
                throw new LLCryptoLibException("Key is null. Set a password before encrypting data.");
            }

            if ((this.keys.Key.Length < 1) || (this.keys.Block.Length < 1))
            {
                throw new LLCryptoLibException("Keys not generated or null");
            }

            try
            {
                if (isCrypting)
                {
                    System.Diagnostics.Debug.WriteLine(">>> Encrypting " + inStream.Name);
                    this.EncryptStream(outStream, inStream, cbp);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(">>> Decrypting " + inStream.Name);
                    this.DecryptStream(outStream, inStream, cbp);
                }
            }
            catch (IOException exc)
            {
                System.Diagnostics.Debug.WriteLine(">>> StreamCrypter.EncryptDecrypt(FileStream, FileStream) EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exc.Message);
                System.Diagnostics.Debug.WriteLine(exc.StackTrace);
            }

        }

        private byte[] MemoryEncryptStream(MemoryStream fout, FileStream fin, CallbackPoint cbp)
        {
            byte[] bin = new byte[CACHESIZE];
            int len = 1;

            long rdlen = 0;
            long totlen = fin.Length;

            System.Diagnostics.Debug.WriteLine("Memory decryption using " + this.symmetricAlgo.ToString());

            CryptoStream encStream = new CryptoStream(fout,
                this.symmetricAlgo.CreateEncryptor(this.keys.Key, this.keys.Block),
                CryptoStreamMode.Write);
            try
            {
                while (rdlen < totlen)
                {
                    len = fin.Read(bin, 0, CACHESIZE);
                    encStream.Write(bin, 0, len);
                    rdlen += len;
                    if (cbp != null)
                    {
                        cbp((int)(rdlen / CACHESIZE), null);
                    }
                }
            }
            catch (IOException exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                System.Diagnostics.Debug.WriteLine(exc.StackTrace);
            }
            finally
            {
                encStream.Close();
            }

            return fout.ToArray();
        }


		private void EncryptStream(FileStream fout, FileStream fin, CallbackPoint cbp)
		{
			byte[] bin = new byte[CACHESIZE];    
			int len = 1;      

			long rdlen = 0;             
			long totlen = fin.Length; 

			CryptoStream encStream = new CryptoStream(fout, 
				this.symmetricAlgo.CreateEncryptor(this.keys.Key, this.keys.Block), 
				CryptoStreamMode.Write);

			try
			{
				while (rdlen < totlen)
				{
					len = fin.Read(bin, 0, CACHESIZE);
					encStream.Write(bin, 0, len);
					rdlen += len;
					if (cbp != null)
					{
						cbp((int)(rdlen/CACHESIZE), null);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("*** Error while encrypting archive: "+ex.Message);
				System.Diagnostics.Debug.WriteLine("*** "+ex.StackTrace);
			}
			finally
			{
                if (encStream != null)
                {
                    encStream.Dispose();
                }
			}
		}

        private void SetEncryption(IStreamAlgorithm ea)
        {
            this.keySize = ea.KeyLen;
            this.blockSize = ea.BlockLen;
            this.symmetricAlgo = ea.Algorithm;
        }

		private void DecryptStream(FileStream fout, FileStream fin, CallbackPoint cbp)
		{

			byte[] bin = new byte[CACHESIZE];    
			int len = 0; 

			CryptoStream encStream = new CryptoStream(fin,this.symmetricAlgo.CreateDecryptor(this.keys.Key,this.keys.Block),
				CryptoStreamMode.Read);

			long finLen = fin.Length;
			long counter = 0;

            try
            {
                while (counter < finLen)
                {
                    len = encStream.Read(bin, 0, CACHESIZE);
                    if (len == 0) break;  // ugly but necessary...
                    fout.Write(bin, 0, len);
                    counter += len;
                    if (cbp != null)
                    {
                        cbp((int)(counter / CACHESIZE), null);
                    }
                }
            }
            catch (ArgumentException aex)
            {
                System.Diagnostics.Debug.WriteLine(">>> Argument Exception: wrong decryption method?");
                System.Diagnostics.Debug.WriteLine("StreamCrypter.DecryptStream()");
                System.Diagnostics.Debug.WriteLine(aex.Message);
                System.Diagnostics.Debug.WriteLine(aex.StackTrace);
            }
			finally
			{
				encStream.Close();
			}

		}

        private byte[] MemoryDecryptStream(MemoryStream fout, FileStream fin, CallbackPoint cbp)
        {

            byte[] bin = new byte[CACHESIZE];
            int len = 0;

            CryptoStream encStream = new CryptoStream(fin, this.symmetricAlgo.CreateDecryptor(this.keys.Key, this.keys.Block),
                CryptoStreamMode.Read);

            long finLen = fin.Length;
            long counter = 0;

            try
            {

                while (counter < finLen)
                {
                    len = encStream.Read(bin, 0, CACHESIZE);
                    if (len == 0) break;

                    fout.Write(bin, 0, len);
                    counter += len;
                    if (cbp != null)
                    {
                        cbp((int)(counter / CACHESIZE), null);
                    }
                }
            }
            finally
            {
                encStream.Close();
            }

            return fout.ToArray();

        }


	}


}

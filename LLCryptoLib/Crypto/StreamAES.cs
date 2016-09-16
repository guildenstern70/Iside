using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     AES 128bit. The American Encyption Standard recogninized by NIST (National Institute of Standards and Technology)
    ///     The AES, aka Rijndael, is a cipher by two Belgian cryptographers, Joan Daemen and Vincent Rijmen.
    ///     Rijndael follows the tradition of square ciphers (it is based on ideas similar to the Square cipher).
    ///     NIST gave as its reasons for selecting Rijndael that it performs very well in hardware and software
    ///     across a wide range of environments in all possible modes. It has excellent key setup time and has
    ///     low memory requirements, in addition its operations are easy to defend against power and timing attacks.
    /// </summary>
    /// <example>
    ///     <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamAES();
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
    public class StreamAES : StreamAlgorithm
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="keylen">Length of key in bytes</param>
        /// <param name="blocksize">Length of block in bytes</param>
        /// <param name="description">Description of algorithm</param>
        public StreamAES(short keylen, short blocksize, string description) : base(keylen, blocksize, description)
        {
        }

        /// <summary>
        ///     Constructor.
        ///     Defaults to 16 bytes keylen and blocklen (128 bits).
        /// </summary>
        public StreamAES() : base(16, 16, "AES-Rijndael (128bit)")
        {
        }

        /// <summary>
        ///     Returns SupportedStreamAlgorithms.AES128
        /// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get { return SupportedStreamAlgorithms.AES128; }
        }

        /// <summary>
        ///     Return the corresponding SymmetricAlgorithm
        /// </summary>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
                SymmetricAlgorithm sa = new RijndaelManaged();
                sa.BlockSize = this.pBlockLen*8;
                sa.KeySize = this.pKeyLen*8;
                return sa;
            }
        }
    }

    /// <summary>
    ///     AES 192bit.
    /// </summary>
    /// <example>
    ///     <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamAES192();
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
    public class StreamAES192 : StreamAES
    {
        /// <summary>
        ///     Constructor.
        ///     Defaults to 24 bytes keylen and blocklen (192 bits).
        /// </summary>
        public StreamAES192() : base(24, 24, "AES-Rijndael (192bit)")
        {
        }

        /// <summary>
        ///     Returns SupportedStreamAlgorithms.AES192
        /// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get { return SupportedStreamAlgorithms.AES192; }
        }
    }

    /// <summary>
    ///     AES 256bit
    /// </summary>
    /// <example>
    ///     <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamAES256();
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
    public class StreamAES256 : StreamAES
    {
        /// <summary>
        ///     Constructor.
        ///     Defaults to 32 bytes keylen and blocklen (256 bits)
        /// </summary>
        public StreamAES256() : base(32, 32, "AES-Rijndael (256bit)")
        {
        }

        /// <summary>
        ///     Returns SupportedStreamAlgorithms.AES256
        /// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get { return SupportedStreamAlgorithms.AES256; }
        }
    }
}
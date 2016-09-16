using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
#if MONO
using Mono.Security.Cryptography;
#endif

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     DES 64bit.
    ///     DES is an algorithm developed in the 1970s. It was made a standard by the US government, and has also
    ///     been adopted by several other governments worldwide. It is widely used, especially in the financial industry.
    ///     DES is a block cipher with 64-bit block size. It uses 56-bit keys. This makes it fairly easy to break with modern
    ///     computers or special-purpose hardware. DES is still strong enough to keep most random hackers and individuals out,
    ///     but it is easily breakable with special hardware by government, criminal organizations, or major corporations.
    ///     In large volumes, the cost of beaking DES keys is on the order of tens of dollars. DES is getting too weak,
    ///     and should not be used in new designs.
    /// </summary>
    /// <example>
    ///     <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamDES();
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
    public class StreamDES : StreamAlgorithm
    {
        /// <summary>
        ///     DES 64 bit constructor
        /// </summary>
        public StreamDES() : base(8, 8, "DES (64bit)")
        {
        }

        /// <summary>
        ///     Returns SupportedStreamAlgorithms.DES
        /// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get { return SupportedStreamAlgorithms.DES; }
        }

        /// <summary>
        ///     Returns a DES 64 bit implementation
        /// </summary>
        public override SymmetricAlgorithm Algorithm
        {
            get { return new DESCryptoServiceProvider(); }
        }
    }

    /// <summary>
    ///     3DES 128bit
    ///     A variant of DES, Triple-DES or 3DES is based on using DES three times (an encrypt-decrypt-encrypt sequence with
    ///     three different,
    ///     unrelated keys). Many people consider Triple-DES to be much safer than plain DES.
    /// </summary>
    /// <example>
    ///     <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new Stream3DES();
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
    public class Stream3DES : StreamAlgorithm
    {
        /// <summary>
        ///     Triple DES 128 bit constructor
        /// </summary>
        public Stream3DES() : base(16, 8, "Triple DES (128bit)")
        {
        }

        /// <summary>
        ///     Returns SupportedStreamAlgorithms.TRIPLEDES
        /// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get { return SupportedStreamAlgorithms.TRIPLEDES; }
        }

        /// <summary>
        ///     Returns a 3DES implementation
        /// </summary>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
                SymmetricAlgorithm sa = new TripleDESCryptoServiceProvider();
                sa.KeySize = this.KeyLen*8;
                sa.BlockSize = this.BlockLen*8;
                return sa;
            }
        }
    }
}
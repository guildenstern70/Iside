using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     StreamThreeFish implements Threefish for streams.
    ///     Key len = 32 byte
    ///     Block len = 32 byte
    /// </summary>
    /// <example>
    ///     Perform a ThreeFish stream encryption.
    ///     <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamThreeFish();
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
    public class StreamThreeFish : StreamAlgorithm
    {
        /// <summary>
        ///     StreamThreeFish 256 bit constructor
        /// </summary>
        public StreamThreeFish()
            : base(32, 32, "ThreeFish (256bit)")
        {
        }

        /// <summary>
        ///     The algorithm ID as in SupportedStreamAlgorithms
        /// </summary>
        /// <value></value>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get { return SupportedStreamAlgorithms.THREEFISH; }
        }

        /// <summary>
        ///     Return a newly constructed SymmetricAlgorithm of type ThreeFish
        /// </summary>
        /// <value></value>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
                SymmetricAlgorithm sa = new Threefish();
                sa.KeySize = this.KeyLen*8;
                sa.BlockSize = this.BlockLen*8;
                sa.Padding = PaddingMode.PKCS7;
                return sa;
            }
        }
    }

    /// <summary>
    ///     StreamThreeFish implements Threefish for streams.
    ///     Key len = 64 byte
    ///     Block len = 64 byte
    /// </summary>
    /// <example>
    ///     Perform a ThreeFish512 stream encryption.
    ///     <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamThreeFish512();
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
    public class StreamThreeFish512 : StreamAlgorithm
    {
        /// <summary>
        ///     StreamThreeFish 512 bit constructor
        /// </summary>
        public StreamThreeFish512()
            : base(64, 64, "ThreeFish (512bit)")
        {
        }

        /// <summary>
        ///     The algorithm ID as in SupportedStreamAlgorithms
        /// </summary>
        /// <value></value>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get { return SupportedStreamAlgorithms.THREEFISH512; }
        }

        /// <summary>
        ///     Return a newly constructed SymmetricAlgorithm of type ThreeFish512
        /// </summary>
        /// <value></value>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
                SymmetricAlgorithm sa = new Threefish();
                sa.KeySize = this.KeyLen*8;
                sa.BlockSize = this.BlockLen*8;
                sa.Padding = PaddingMode.PKCS7;
                return sa;
            }
        }
    }


    /// <summary>
    ///     StreamThreeFish implements Threefish for streams.
    ///     Key len = 128 byte
    ///     Block len = 128 byte
    /// </summary>
    /// <example>
    ///     Perform a ThreeFish1024 stream encryption.
    ///     <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamThreeFish512();
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
    public class StreamThreeFish1024 : StreamAlgorithm
    {
        /// <summary>
        ///     StreamThreeFish 1024 bit constructor
        /// </summary>
        public StreamThreeFish1024()
            : base(128, 128, "ThreeFish (1024bit)")
        {
        }

        /// <summary>
        ///     The algorithm ID as in SupportedStreamAlgorithms
        /// </summary>
        /// <value></value>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get { return SupportedStreamAlgorithms.THREEFISH1024; }
        }

        /// <summary>
        ///     Return a newly constructed SymmetricAlgorithm of type ThreeFish1024
        /// </summary>
        /// <value></value>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
                SymmetricAlgorithm sa = new Threefish();
                sa.KeySize = this.KeyLen*8;
                sa.BlockSize = this.BlockLen*8;
                sa.Padding = PaddingMode.PKCS7;
                return sa;
            }
        }
    }
}
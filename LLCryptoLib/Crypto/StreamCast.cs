using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     StreamCast implements CAST5 (aka CAST-128) for streams.
    ///     Key len = 8 byte
    ///     Block len = 8 byte
    /// </summary>
    /// <example>
    ///     Perform a CAST-128 stream encryption.
    ///     <code>  
    /// // Set encryption algorithm
    /// IStreamAlgorithm encryptAlgo = new StreamCast();
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
    public class StreamCast : StreamAlgorithm
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StreamCast" /> class.
        /// </summary>
        public StreamCast() : base(8, 8, "CAST5 (128 bit)")
        {
        }

        /// <summary>
        ///     The algorithm ID as in SupportedStreamAlgorithms
        /// </summary>
        /// <value></value>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get { return SupportedStreamAlgorithms.CAST5; }
        }

        /// <summary>
        ///     The algorithm as defined in System.Security.Cryptography
        /// </summary>
        /// <value></value>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
                SymmetricAlgorithm sa = new Cast5Managed();
                sa.BlockSize = this.BlockLen*8;
                sa.KeySize = this.KeyLen*8;
                return sa;
            }
        }
    }
}
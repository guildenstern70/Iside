using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     Encryption Algorithm.
    ///     A StreamAlgorithm class indicates the algorithm to be used in Stream Encryption.
    ///     <see cref="StreamCrypter" />
    /// </summary>
    /// <example>
    ///     <code>  
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
        ///     Algorithm description
        /// </summary>
        private readonly string description;

        /// <summary>
        ///     Block length
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")
        ] protected short pBlockLen;

        /// <summary>
        ///     Key Length
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")
        ] protected short pKeyLen;

        /// <summary>
        ///     Stream algorithm constructor.
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
        ///     Description
        /// </summary>
        public string Description
        {
            get { return this.description; }
        }

        /// <summary>
        ///     Length of key in bytes
        /// </summary>
        public short KeyLen
        {
            get { return this.pKeyLen; }
        }

        /// <summary>
        ///     Length of block in bytes
        /// </summary>
        public short BlockLen
        {
            get { return this.pBlockLen; }
        }

        /// <summary>
        ///     The algorithm ID as in SupportedStreamAlgorithms
        /// </summary>
        public abstract SupportedStreamAlgorithms SupportedAlgorithmID { get; }

        /// <summary>
        ///     The algorithm as defined in System.Security.Cryptography
        /// </summary>
        public abstract SymmetricAlgorithm Algorithm { get; }

        /// <summary>
        ///     Returns a <see cref="T:System.String">string</see> that represents the current
        ///     <see cref="T:System.Object">StreamAlgorithm</see>.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return this.description;
        }
    }
}
using System;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Represents the base class from which all implementations of the RC4 symmetric stream cipher must inherit.
    /// </summary>
    /// <remarks>
    ///     RC4 is a trademark of RSA Data Security Inc.
    /// </remarks>
    public abstract class RC4 : SymmetricAlgorithm
    {
        private RNGCryptoServiceProvider m_RNG;

        /// <summary>
        ///     Initializes a new instance of the RC4 class.
        /// </summary>
        /// <remarks>
        ///     The default keysize is 128 bits.
        /// </remarks>
        protected RC4()
        {
            this.KeySizeValue = 128;
        }

        /// <summary>
        ///     Gets or sets the block size of the cryptographic operation in bits.
        /// </summary>
        /// <value>The block size of RC4 is always 8 bits.</value>
        /// <exception cref="CryptographicException">The block size is invalid.</exception>
        public override int BlockSize
        {
            get { return 8; }
            set
            {
                if ((value != 8) && (value != 0))
                    throw new CryptographicException(ResourceController.GetString("Error_InvalidIVSize"));
            }
        }

        /// <summary>
        ///     Gets or sets the feedback size of the cryptographic operation in bits.
        /// </summary>
        /// <value>This property always throws a <see cref="CryptographicException" />.</value>
        /// <exception cref="NotSupportedException">This exception is always thrown.</exception>
        /// <remarks>RC4 doesn't use the FeedbackSize property.</remarks>
        public override int FeedbackSize
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        ///     Gets or sets the initialization vector (IV) for the symmetric algorithm.
        /// </summary>
        /// <value>
        ///     This property always returns a byte array of length one. The value of the byte in the array is always set to
        ///     zero.
        /// </value>
        /// <exception cref="CryptographicException">An attempt is made to set the IV to an invalid instance.</exception>
        /// <remarks>
        ///     RC4 doesn't use the IV property, however the property accepts IV's of up to one byte (RC4's
        ///     <see cref="BlockSize" />) in order to interoperate with software that has been written with the use of block
        ///     ciphers in mind.
        /// </remarks>
        public override byte[] IV
        {
            get { return new byte[1]; }
            set
            {
                if ((value != null) && (value.Length > 1))
                    throw new CryptographicException(ResourceController.GetString("Error_InvalidIVSize"));
            }
        }

        /// <summary>
        ///     Gets the block sizes that are supported by the symmetric algorithm.
        /// </summary>
        /// <value>An array containing the block sizes supported by the algorithm.</value>
        /// <remarks>Only a block size of one byte is supported by the RC4 algorithm.</remarks>
        public override KeySizes[] LegalBlockSizes
        {
            get { return new[] {new KeySizes(8, 8, 0)}; }
        }

        /// <summary>
        ///     Gets the key sizes that are supported by the symmetric algorithm.
        /// </summary>
        /// <value>An array containing the key sizes supported by the algorithm.</value>
        /// <remarks>Only key sizes that match an entry in this array are supported by the symmetric algorithm.</remarks>
        public override KeySizes[] LegalKeySizes
        {
            get { return new[] {new KeySizes(8, 2048, 8)}; }
        }

        /// <summary>
        ///     Gets or sets the mode for operation of the symmetric algorithm.
        /// </summary>
        /// <value>The mode for operation of the symmetric algorithm.</value>
        /// <remarks>RC4 only supports the OFB cipher mode. See <see cref="CipherMode" /> for a description of this mode.</remarks>
        /// <exception cref="CryptographicException">The cipher mode is not OFB.</exception>
        public override CipherMode Mode
        {
            get { return CipherMode.OFB; }
            set
            {
                if (value != CipherMode.OFB)
                    throw new CryptographicException(ResourceController.GetString("Error_InvalidCipherMode"));
            }
        }

        /// <summary>
        ///     Gets or sets the padding mode used in the symmetric algorithm.
        /// </summary>
        /// <value>The padding mode used in the symmetric algorithm. This property always returns PaddingMode.None.</value>
        /// <exception cref="CryptographicException">The padding mode is set to a padding mode other than PaddingMode.None.</exception>
        public override PaddingMode Padding
        {
            get { return PaddingMode.None; }
            set
            {
                if (value != PaddingMode.None)
                    throw new CryptographicException(ResourceController.GetString("Error_InvalidPaddingMode"));
            }
        }

        /// <summary>
        ///     Returns an <see cref="RNGCryptoServiceProvider" /> instance.
        /// </summary>
        /// <returns>An RNGCryptoServiceProvider instance.</returns>
        protected RNGCryptoServiceProvider RngCsp
        {
            get
            {
                if (this.m_RNG == null)
                    this.m_RNG = new RNGCryptoServiceProvider();
                return this.m_RNG;
            }
        }

        /// <summary>
        ///     This is a stub method.
        /// </summary>
        /// <remarks>Since the RC4 cipher doesn't use an Initialization Vector, this method will not do anything.</remarks>
        public override void GenerateIV()
        {
            // do nothing
        }

        /// <summary>
        ///     Generates a random Key to be used for the algorithm.
        /// </summary>
        /// <remarks>Use this method to generate a random key when none is specified.</remarks>
        public override void GenerateKey()
        {
            byte[] key = new byte[this.KeySize/8];
            this.RngCsp.GetBytes(key);
            this.Key = key;
        }

        /// <summary>
        ///     Creates an instance of the default cryptographic object used to perform the RC4 transformation.
        /// </summary>
        /// <returns>The instance of a cryptographic object used to perform the RC4 transformation.</returns>
        /// <exception cref="CryptographicException">An error occurs while acquiring the CSP.</exception>
        /// <exception cref="InvalidOperationException">
        ///     FIPS compliance is required, but the current implementation isn't FIPS
        ///     verified.
        /// </exception>
        public new static RC4 Create()
        {
            return Create("ARCFOUR");
        }

        /// <summary>
        ///     Creates an instance of the specified cryptographic object used to perform the RC4 transformation.
        /// </summary>
        /// <param name="algName">The name of the specific implementation of <see cref="RC4" /> to create.</param>
        /// <returns>A cryptographic object.</returns>
        /// <exception cref="CryptographicException">An error occurs while acquiring the CSP.</exception>
        /// <exception cref="InvalidOperationException">
        ///     FIPS compliance is required, but the current implementation isn't FIPS
        ///     verified.
        /// </exception>
        public new static RC4 Create(string algName)
        {
            if (algName == null)
                throw new ArgumentNullException("algName", ResourceController.GetString("Error_ParamNull"));
            if (string.Equals(algName, "RC4", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(algName, "LLCryptoLib.Security.cryptography.rc4cryptoserviceprovider",
                    StringComparison.InvariantCultureIgnoreCase))
                return new RC4CryptoServiceProvider();
            if (string.Equals(algName, "ARCFOUR", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(algName, "LLCryptoLib.Security.cryptography.arcfourmanaged",
                    StringComparison.InvariantCultureIgnoreCase))
                return new ARCFourManaged();
            return null;
        }
    }
}
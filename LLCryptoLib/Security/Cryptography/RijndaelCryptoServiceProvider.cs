using System;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;
using LLCryptoLib.Security.Win32;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Accesses the unmanaged version of the <see cref="Rijndael" /> algorithm. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    ///     This class will use the unmanaged implementation of the Rijndael algorithm, when possible. If the unmanaged
    ///     Rijndael algorithm is not available, it will fall back to the <see cref="RijndaelManaged" /> implementation.
    /// </remarks>
    /// <exception cref="CryptographicException">The AES cryptographic service provider could not be acquired.</exception>
    public sealed class RijndaelCryptoServiceProvider : Rijndael
    {
        private bool m_Disposed;

        private RijndaelManaged m_Managed;
        private readonly IntPtr m_Provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RijndaelCryptoServiceProvider" /> class.
        /// </summary>
        public RijndaelCryptoServiceProvider() : this(false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RijndaelCryptoServiceProvider" /> class.
        /// </summary>
        /// <param name="forceUnmanaged">
        ///     Forces the class to use the unmanaged AES implementation. An exception will be thrown if
        ///     this is impossible.
        /// </param>
        /// <exception cref="CryptographicException">An error occurs while acquiring the AES CSP.</exception>
        public RijndaelCryptoServiceProvider(bool forceUnmanaged)
        {
            // acquire an AES context
            try
            {
                this.m_Provider = CryptoHandle.Handle;
                if (CryptoHandle.HandleProviderType != NativeMethods.PROV_RSA_AES)
                    this.m_Provider = IntPtr.Zero;
            }
            catch (CryptographicException)
            {
                this.m_Provider = IntPtr.Zero;
            }
            if ((this.m_Provider == IntPtr.Zero) && forceUnmanaged)
                throw new CryptographicException(ResourceController.GetString("Error_AcquireCSP"));
            this.m_Managed = new RijndaelManaged();
            this.ForceUnmanaged = forceUnmanaged;
        }

        /// <summary>
        ///     Gets or sets the block size of the cryptographic operation in bits.
        /// </summary>
        /// <value>The block size in bits.</value>
        /// <exception cref="CryptographicException">The block size is invalid.</exception>
        /// <remarks>
        ///     The block size is the basic unit of data that can be encrypted or decrypted in one operation. Messages longer
        ///     than the block size are handled as successive blocks; messages shorter than the block size must be padded with
        ///     extra bits to reach the size of a block. Valid block sizes are determined by the symmetric algorithm used.
        /// </remarks>
        public override int BlockSize
        {
            get { return this.m_Managed.BlockSize; }
            set { this.m_Managed.BlockSize = value; }
        }

        /// <summary>
        ///     Gets or sets the feedback size of the cryptographic operation in bits.
        /// </summary>
        /// <value>The feedback size in bits.</value>
        /// <exception cref="CryptographicException">The feedback size is larger than the block size.</exception>
        /// <remarks>
        ///     The feedback size determines the amount of data that is fed back to successive encryption or decryption
        ///     operations. The feedback size cannot be greater than the block size.
        /// </remarks>
        public override int FeedbackSize
        {
            get { return this.m_Managed.FeedbackSize; }
            set { this.m_Managed.FeedbackSize = value; }
        }

        /// <summary>
        ///     Gets or sets the initialization vector (IV) for the symmetric algorithm.
        /// </summary>
        /// <value>The initialization vector.</value>
        /// <exception cref="ArgumentNullException">
        ///     An attempt is made to set the IV to a null reference (<b>Nothing</b> in Visual
        ///     Basic).
        /// </exception>
        /// <exception cref="CryptographicException">An attempt is made to set the IV to an invalid size.</exception>
        /// <remarks>
        ///     If this property is a null reference (<b>Nothing</b> in Visual Basic) when it is used,
        ///     <see cref="GenerateIV" /> is called to create a new random value.
        /// </remarks>
        public override byte[] IV
        {
            get { return this.m_Managed.IV; }
            set { this.m_Managed.IV = value; }
        }

        /// <summary>
        ///     Gets or sets the secret key for the symmetric algorithm.
        /// </summary>
        /// <value>The secret key to be used for the symmetric algorithm.</value>
        /// <exception cref="ArgumentNullException">
        ///     An attempt is made to set the key to a null reference (<b>Nothing</b> in Visual
        ///     Basic).
        /// </exception>
        /// <remarks>
        ///     <p>
        ///         The secret key is used both for encryption and for decryption. For a symmetric algorithm to be secure, the
        ///         secret key must be known only to the sender and the receiver. The valid key sizes are specified by the
        ///         particular symmetric algorithm implementation and are listed in <see cref="LegalKeySizes" />.
        ///     </p>
        ///     <p>
        ///         If this property is a null reference (<b>Nothing</b> in Visual Basic) when it is used,
        ///         <see cref="GenerateKey" /> is called to create a new random value.
        ///     </p>
        /// </remarks>
        public override byte[] Key
        {
            get { return this.m_Managed.Key; }
            set { this.m_Managed.Key = value; }
        }

        /// <summary>
        ///     Gets or sets the size of the secret key used by the symmetric algorithm in bits.
        /// </summary>
        /// <value>The size of the secret key used by the symmetric algorithm.</value>
        /// <exception cref="CryptographicException">The key size is not valid.</exception>
        /// <remarks>
        ///     The valid key sizes are specified by the particular symmetric algorithm implementation and are listed in
        ///     <see cref="LegalKeySizes" />.
        /// </remarks>
        public override int KeySize
        {
            get { return this.m_Managed.KeySize; }
            set { this.m_Managed.KeySize = value; }
        }

        /// <summary>
        ///     Gets the block sizes that are supported by the symmetric algorithm.
        /// </summary>
        /// <value>An array containing the block sizes supported by the algorithm.</value>
        /// <remarks>Only block sizes that match an entry in this array are supported by the symmetric algorithm.</remarks>
        public override KeySizes[] LegalBlockSizes
        {
            get { return this.m_Managed.LegalBlockSizes; }
        }

        /// <summary>
        ///     Gets the key sizes that are supported by the symmetric algorithm.
        /// </summary>
        /// <value>An array containing the key sizes supported by the algorithm.</value>
        /// <remarks>Only key sizes that match an entry in this array are supported by the symmetric algorithm.</remarks>
        public override KeySizes[] LegalKeySizes
        {
            get { return this.m_Managed.LegalKeySizes; }
        }

        /// <summary>
        ///     Gets or sets the mode for operation of the symmetric algorithm.
        /// </summary>
        /// <value>The mode for operation of the symmetric algorithm.</value>
        /// <exception cref="CryptographicException">The cipher mode is not one of the CipherMode values.</exception>
        /// <remarks>See CipherMode for a description of specific modes.</remarks>
        public override CipherMode Mode
        {
            get { return this.m_Managed.Mode; }
            set { this.m_Managed.Mode = value; }
        }

        /// <summary>
        ///     Gets or sets the padding mode used in the symmetric algorithm.
        /// </summary>
        /// <value>The padding mode used in the symmetric algorithm.</value>
        /// <exception cref="CryptographicException">The padding mode is not one of the PaddingMode values.</exception>
        /// <remarks>
        ///     Most plain text messages do not consist of a number of bytes that completely fill blocks. Often, there are not
        ///     enough bytes to fill the last block. When this happens, a padding string is added to the text. For example, if the
        ///     block length is 64 bits and the last block contains only 40 bits, 24 bits of padding are added. See
        ///     <see cref="PaddingMode" /> for a description of specific modes.
        /// </remarks>
        public override PaddingMode Padding
        {
            get { return this.m_Managed.Padding; }
            set { this.m_Managed.Padding = value; }
        }

        private bool ForceUnmanaged { get; }

        /// <summary>
        ///     Releases all unmanaged resources.
        /// </summary>
        ~RijndaelCryptoServiceProvider()
        {
            this.Dispose(true);
        }

        /// <summary>
        ///     Releases all unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        ///     <b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only
        ///     unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (this.m_Managed != null)
            {
                this.m_Managed.Clear();
                ((IDisposable) this.m_Managed).Dispose();
                this.m_Managed = null;
            }
            base.Dispose(disposing);
            GC.SuppressFinalize(this);
            this.m_Disposed = true;
        }

        /// <summary>
        ///     Generates a random initialization vector (IV) to be used for the algorithm.
        /// </summary>
        /// <remarks>Use this method to generate a random IV when none is specified.</remarks>
        public override void GenerateIV()
        {
            this.m_Managed.GenerateIV();
        }

        /// <summary>
        ///     Generates a random Key to be used for the algorithm.
        /// </summary>
        /// <remarks>Use this method to generate a random key when none is specified.</remarks>
        public override void GenerateKey()
        {
            this.m_Managed.GenerateKey();
        }

        /// <summary>
        ///     Creates a symmetric <see cref="Rijndael" /> decryptor object with the specified <see cref="Key" /> and
        ///     initialization vector (<see cref="IV" />).
        /// </summary>
        /// <param name="rgbKey">The secret key to be used for the symmetric algorithm.</param>
        /// <param name="rgbIV">The IV to be used for the symmetric algorithm.</param>
        /// <returns>A symmetric Rijndael decryptor object.</returns>
        /// <remarks>
        ///     This method decrypts an encrypted message created using the <see cref="CreateEncryptor" /> overload with the
        ///     same signature.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
        /// <exception cref="CryptographicException">An error occurs while creating the decryptor.</exception>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            if (this.m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            if (rgbKey == null)
                throw new ArgumentNullException("rgbKey", ResourceController.GetString("Error_ParamNull"));
            if (rgbIV == null)
                throw new ArgumentNullException("rgbIV", ResourceController.GetString("Error_ParamNull"));
            if ((this.Mode == CipherMode.CTS) || (this.Mode == CipherMode.OFB) || (this.Mode == CipherMode.CFB))
                throw new CryptographicException(ResourceController.GetString("Error_InvalidCipherMode"));
            if (this.CanUseUnmanaged(rgbKey.Length*8, rgbIV.Length*8, this.Padding))
            {
                try
                {
                    return new RijndaelUnmanagedTransform(GetKeyType(rgbKey.Length*8), CryptoMethod.Decrypt, rgbKey,
                        rgbIV, this.Mode, this.FeedbackSize, this.Padding);
                }
                catch (CryptographicException)
                {
                }
            }
            if (this.ForceUnmanaged)
                throw new CryptographicException(ResourceController.GetString("Error_AcquireCSP"));
            return this.m_Managed.CreateDecryptor(rgbKey, rgbIV);
        }

        /// <summary>
        ///     Creates a symmetric <see cref="Rijndael" /> encryptor object with the specified <see cref="Key" /> and
        ///     initialization vector (<see cref="IV" />).
        /// </summary>
        /// <param name="rgbKey">The secret key to be used for the symmetric algorithm.</param>
        /// <param name="rgbIV">The IV to be used for the symmetric algorithm.</param>
        /// <returns>A symmetric Rijndael encryptor object.</returns>
        /// <remarks>Use the <see cref="CreateDecryptor" /> overload with the same signature to decrypt the result of this method.</remarks>
        /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
        /// <exception cref="CryptographicException">An error occurs while creating the encryptor.</exception>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            if (this.m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            if (rgbKey == null)
                throw new ArgumentNullException("rgbKey", ResourceController.GetString("Error_ParamNull"));
            if (rgbIV == null)
                throw new ArgumentNullException("rgbIV", ResourceController.GetString("Error_ParamNull"));
            if ((this.Mode == CipherMode.CTS) || (this.Mode == CipherMode.OFB) || (this.Mode == CipherMode.CFB))
                throw new CryptographicException(ResourceController.GetString("Error_InvalidCipherMode"));
            if (this.CanUseUnmanaged(rgbKey.Length*8, rgbIV.Length*8, this.Padding))
            {
                try
                {
                    return new RijndaelUnmanagedTransform(GetKeyType(rgbKey.Length*8), CryptoMethod.Encrypt, rgbKey,
                        rgbIV, this.Mode, this.FeedbackSize, this.Padding);
                }
                catch (CryptographicException)
                {
                }
            }
            if (this.ForceUnmanaged)
                throw new CryptographicException(ResourceController.GetString("Error_AcquireCSP"));
            return this.m_Managed.CreateEncryptor(rgbKey, rgbIV);
        }

        private static CryptoAlgorithm GetKeyType(int size)
        {
            if (size == 128)
                return CryptoAlgorithm.Rijndael128;
            if (size == 192)
                return CryptoAlgorithm.Rijndael192;
            if (size == 256)
                return CryptoAlgorithm.Rijndael256;
            throw new CryptographicException(ResourceController.GetString("Error_InvalidKeySize"));
        }

        private bool CanUseUnmanaged(int keySize, int blockSize, PaddingMode padding)
        {
            return (this.m_Provider != IntPtr.Zero) && // make sure the unmanaged AES CSP is available
                   (blockSize == 128) && // although Rijndael supports block sizes of 128, 192 and 256 bits,
                   // the AES only specifies a blocksize of 128 bits [and only this block size is supported by the unmanaged implementation]
                   ((padding == PaddingMode.PKCS7) || (padding == PaddingMode.None)) &&
                   // only PKCS7 padding is supported
                   ((keySize == 128) || (keySize == 192) || (keySize == 256));
                // make sure we use one of the supported key lengths
        }
    }
}
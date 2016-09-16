using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;
using LLCryptoLib.Security.Win32;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Defines a wrapper object to access the cryptographic service provider (CSP) version of the RC4 algorithm. This
    ///     class cannot be inherited.
    /// </summary>
    public sealed class RC4CryptoServiceProvider : RC4
    {
        private bool m_Disposed;
        private ARCFourManaged m_Managed;
        private readonly int m_MaxLen;
        private readonly int m_MinLen;
        private readonly IntPtr m_Provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RC4CryptoServiceProvider" /> class.
        /// </summary>
        /// <exception cref="CryptographicException">An error occurs while acquiring the CSP.</exception>
        public RC4CryptoServiceProvider()
        {
            // acquire an RC4 context
            this.m_Provider = CryptoHandle.Handle;
            if (this.m_Provider != IntPtr.Zero)
            {
                int dwFlags = NativeMethods.CRYPT_FIRST;
                bool found = false;
                IntPtr provEnum = Marshal.AllocHGlobal(100);
                int dwSize;
                do
                {
                    dwSize = 100;
                    if (
                        NativeMethods.CryptGetProvParam(this.m_Provider, NativeMethods.PP_ENUMALGS_EX, provEnum,
                            ref dwSize, dwFlags) == 0)
                        break;
                    dwFlags = 0;
                    PROV_ENUMALGS_EX eax = (PROV_ENUMALGS_EX) Marshal.PtrToStructure(provEnum, typeof(PROV_ENUMALGS_EX));
                    if (eax.aiAlgid == NativeMethods.CALG_RC4)
                    {
                        found = true;
                        this.m_MinLen = eax.dwMinLen;
                        this.m_MaxLen = eax.dwMaxLen;
                    }
                } while (!found);
                Marshal.FreeHGlobal(provEnum);
            }
            this.m_Managed = new ARCFourManaged();
        }

        /// <summary>
        ///     Gets or sets the block size of the cryptographic operation in bits.
        /// </summary>
        /// <value>The block size of RC4 is always 8 bits.</value>
        /// <exception cref="CryptographicException">The block size is invalid.</exception>
        public override int BlockSize
        {
            get { return this.m_Managed.BlockSize; }
            set { this.m_Managed.BlockSize = value; }
        }

        /// <summary>
        ///     Gets or sets the feedback size of the cryptographic operation in bits.
        /// </summary>
        /// <value>This property always throws a <see cref="CryptographicException" />.</value>
        /// <exception cref="CryptographicException">This exception is always thrown.</exception>
        /// <remarks>RC4 doesn't use the FeedbackSize property.</remarks>
        public override int FeedbackSize
        {
            get { return this.m_Managed.FeedbackSize; }
            set { this.m_Managed.FeedbackSize = value; }
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
        public override int KeySize
        {
            get { return this.m_Managed.KeySize; }
            set { this.m_Managed.KeySize = value; }
        }

        /// <summary>
        ///     Gets the block sizes that are supported by the symmetric algorithm.
        /// </summary>
        /// <value>An array containing the block sizes supported by the algorithm.</value>
        /// <remarks>Only a block size of one byte is supported by the RC4 algorithm.</remarks>
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
        /// <remarks>RC4 only supports the OFB cipher mode. See <see cref="CipherMode" /> for a description of this mode.</remarks>
        /// <exception cref="CryptographicException">The cipher mode is not OFB.</exception>
        public override CipherMode Mode
        {
            get { return this.m_Managed.Mode; }
            set { this.m_Managed.Mode = value; }
        }

        /// <summary>
        ///     Gets or sets the padding mode used in the symmetric algorithm.
        /// </summary>
        /// <value>The padding mode used in the symmetric algorithm. This property always returns PaddingMode.None.</value>
        /// <exception cref="CryptographicException">The padding mode is set to a padding mode other than PaddingMode.None.</exception>
        public override PaddingMode Padding
        {
            get { return this.m_Managed.Padding; }
            set { this.m_Managed.Padding = value; }
        }

        /// <summary>
        ///     This is a stub method.
        /// </summary>
        /// <remarks>Since the RC4 cipher doesn't use an Initialization Vector, this method will not do anything.</remarks>
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
        ///     Creates a symmetric decryptor object with the specified Key.
        /// </summary>
        /// <param name="rgbKey">The secret key to be used for the symmetric algorithm. </param>
        /// <param name="rgbIV">Not used in RC4. It can be a null reference or a byte array with a length less than 2.</param>
        /// <returns>A symmetric decryptor object.</returns>
        /// <remarks>
        ///     This method decrypts an encrypted message created using the <see cref="CreateEncryptor" /> overload with the
        ///     same parameters.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="rgbKey" /> is a null reference (<b>Nothing</b> in Visual
        ///     Basic).
        /// </exception>
        /// <exception cref="CryptographicException"></exception>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            if (this.m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            if (rgbKey == null)
                throw new ArgumentNullException("rgbKey", ResourceController.GetString("Error_ParamNull"));
            if ((rgbKey.Length == 0) || (rgbKey.Length > 256))
                throw new CryptographicException(ResourceController.GetString("Error_InvalidKeySize"));
            if ((rgbIV != null) && (rgbIV.Length > 1))
                throw new CryptographicException(ResourceController.GetString("Error_InvalidIVSize"));
            try
            {
                if (this.CanUseUnmanaged(rgbKey.Length*8))
                    return new RC4UnmanagedTransform(rgbKey);
            }
            catch (CryptographicException)
            {
            }
            return this.m_Managed.CreateDecryptor(rgbKey, rgbIV);
        }

        /// <summary>
        ///     Creates a symmetric encryptor object with the specified Key.
        /// </summary>
        /// <param name="rgbKey">The secret key to be used for the symmetric algorithm. </param>
        /// <param name="rgbIV">Not used in RC4. It can be a null reference or a byte array with a length less than 2.</param>
        /// <returns>A symmetric encryptor object.</returns>
        /// <remarks>Use the <see cref="CreateDecryptor" /> overload with the same parameters to decrypt the result of this method.</remarks>
        /// <exception cref="ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="rgbKey" /> is a null reference (<b>Nothing</b> in Visual
        ///     Basic).
        /// </exception>
        /// <exception cref="CryptographicException"></exception>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return this.CreateDecryptor(rgbKey, rgbIV);
        }

        /// <summary>
        ///     Returns a boolean that indicates whether the unmanaged CSP can be used or not.
        /// </summary>
        /// <param name="keySize">The size of the required key (in bits).</param>
        /// <returns><b>true</b> if the unmanaged CSP can be used to encrypt and decrypt data, <b>false</b> otherwise.</returns>
        private bool CanUseUnmanaged(int keySize)
        {
            return (this.m_Provider != IntPtr.Zero) && // make sure the unmanaged CSP is available
                   (keySize >= this.m_MinLen) && // keysize is a value between the minimum
                   (keySize <= this.m_MaxLen); // and the maximum size the CSP supports
        }

        private new void Dispose()
        {
            if (!this.m_Disposed)
            {
                this.m_Disposed = true;
                if (this.m_Managed != null)
                {
                    this.m_Managed.Clear();
                    this.m_Managed = null;
                }
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        ///     Finalizes the RC4CryptoServiceProvider.
        /// </summary>
        ~RC4CryptoServiceProvider()
        {
            this.Dispose();
        }
    }
}
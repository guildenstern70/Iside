using System;
using System.Security;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Accesses the managed version of the ARCFour algorithm. This class cannot be inherited.
    ///     ARCFour is fully compatible with the RC4<sup>TM</sup> algorithm.
    /// </summary>
    /// <remarks>
    ///     RC4 is a trademark of RSA Data Security Inc.
    /// </remarks>
    public sealed class ARCFourManaged : RC4
    {
        private bool m_IsDisposed;

        /// <summary>
        ///     Initializes a new instance of the ARCFourManaged class.
        /// </summary>
        /// <remarks>
        ///     The default keysize is 128 bits.
        /// </remarks>
        /// <exception cref="InvalidOperationException" />
        [SecurityCritical]
        public ARCFourManaged()
        {
            if (CryptoHandle.PolicyRequiresFips)
                throw new InvalidOperationException(ResourceController.GetString("Error_RequiresFIPS"));
        }

        /// <summary>
        ///     Creates a symmetric <see cref="RC4" /> decryptor object with the specified Key.
        /// </summary>
        /// <param name="rgbKey">The secret key to be used for the symmetric algorithm.</param>
        /// <param name="rgbIV">
        ///     This parameter is not used an should be set to a null reference, or to an array with zero or one
        ///     bytes.
        /// </param>
        /// <returns>A symmetric ARCFour decryptor object.</returns>
        /// <remarks>
        ///     This method decrypts an encrypted message created using the <see cref="CreateEncryptor" /> overload with the
        ///     same signature.
        /// </remarks>
        /// <exception cref="ObjectDisposedException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="CryptographicException" />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            if (this.m_IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            if (rgbKey == null)
            {
                throw new ArgumentNullException("rgbKey", ResourceController.GetString("Error_ParamNull"));
            }
            if ((rgbKey.Length == 0) || (rgbKey.Length > 256))
            {
                throw new CryptographicException(ResourceController.GetString("Error_InvalidKeySize"));
            }
            if ((rgbIV != null) && (rgbIV.Length > 1))
            {
                throw new CryptographicException(ResourceController.GetString("Error_InvalidIVSize"));
            }

            return new ARCFourManagedTransform(rgbKey);
        }

        /// <summary>
        ///     Creates a symmetric <see cref="RC4" /> encryptor object with the specified Key.
        /// </summary>
        /// <param name="rgbKey">The secret key to be used for the symmetric algorithm.</param>
        /// <param name="rgbIV">
        ///     This parameter is not used an should be set to a null reference, or to an array with zero or one
        ///     bytes.
        /// </param>
        /// <returns>A symmetric ARCFour encryptor object.</returns>
        /// <remarks>Use the <see cref="CreateDecryptor" /> overload with the same signature to decrypt the result of this method.</remarks>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return this.CreateDecryptor(rgbKey, rgbIV);
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the <see cref="ARCFourManaged" /> and optionally releases the managed
        ///     resources.
        /// </summary>
        /// <param name="disposing">
        ///     <b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only
        ///     unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.m_IsDisposed = true;
        }
    }

    /// <summary>
    ///     LLCryptoLib.Security.Cryptography is the place for encryption utility
    ///     classes directly related to digital certificates management.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}
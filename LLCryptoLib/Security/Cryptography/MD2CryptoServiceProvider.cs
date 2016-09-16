using System;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;
using LLCryptoLib.Security.Win32;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Computes the <see cref="MD2" /> hash for the input data using the implementation provided by the cryptographic
    ///     service provider (CSP).
    /// </summary>
    public sealed class MD2CryptoServiceProvider : MD2
    {
        private bool m_Disposed;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability",
             "CA2006:UseSafeHandleToEncapsulateNativeResources")] private IntPtr m_Hash;

        private readonly IntPtr m_Provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MD2CryptoServiceProvider" /> class. This class cannot be inherited.
        /// </summary>
        public MD2CryptoServiceProvider()
        {
            // acquire an MD2 context
            this.m_Provider = CryptoHandle.Handle;
            this.Initialize();
        }

        /// <summary>
        ///     Initializes an instance of <see cref="MD2CryptoServiceProvider" />.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The MD2CryptoServiceProvider instance has been disposed.</exception>
        public override void Initialize()
        {
            if (this.m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            if (this.m_Hash != IntPtr.Zero)
            {
                NativeMethods.CryptDestroyHash(this.m_Hash);
            }
            NativeMethods.CryptCreateHash(this.m_Provider, NativeMethods.CALG_MD2, IntPtr.Zero, 0, out this.m_Hash);
        }

        /// <summary>
        ///     Routes data written to the object into the <see cref="MD2" /> hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The array of data bytes.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the array to use as data.</param>
        /// <exception cref="ObjectDisposedException">The MD2CryptoServiceProvider instance has been disposed.</exception>
        /// <exception cref="CryptographicException">The data could not be hashed.</exception>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (this.m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            byte[] copy = new byte[cbSize];
            Array.Copy(array, ibStart, copy, 0, cbSize);
            if (NativeMethods.CryptHashData(this.m_Hash, copy, copy.Length, 0) == 0)
                throw new CryptographicException(ResourceController.GetString("Error_HashData"));
        }

        /// <summary>
        ///     Returns the computed <see cref="MD2CryptoServiceProvider" /> hash as an array of bytes after all data has been
        ///     written to the object.
        /// </summary>
        /// <returns>The computed hash value.</returns>
        /// <exception cref="ObjectDisposedException">The MD2CryptoServiceProvider instance has been disposed.</exception>
        /// <exception cref="CryptographicException">The data could not be hashed.</exception>
        protected override byte[] HashFinal()
        {
            if (this.m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            byte[] buffer = new byte[16];
            int length = buffer.Length;
            if (NativeMethods.CryptGetHashParam(this.m_Hash, NativeMethods.HP_HASHVAL, buffer, ref length, 0) == 0)
                throw new CryptographicException(ResourceController.GetString("Error_HashRead"));
            return buffer;
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the <see cref="MD2CryptoServiceProvider" /> and optionally releases the
        ///     managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only
        ///     unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!this.m_Disposed)
            {
                if (this.m_Hash != IntPtr.Zero)
                {
                    NativeMethods.CryptDestroyHash(this.m_Hash);
                    this.m_Hash = IntPtr.Zero;
                }
                base.Dispose(disposing);
                GC.SuppressFinalize(this);
                this.m_Disposed = true;
            }
        }

        /// <summary>
        ///     Finalizes the MD2CryptoServiceProvider.
        /// </summary>
        ~MD2CryptoServiceProvider()
        {
            this.Clear();
        }
    }
}
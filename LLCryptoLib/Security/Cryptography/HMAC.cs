using System;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    // http://www.ietf.org/rfc/rfc2104.txt
    /// <summary>
    ///     Implements the HMAC keyed message authentication code algorithm.
    /// </summary>
    public sealed class HMAC : KeyedHashAlgorithm
    {
        private readonly HashAlgorithm m_HashAlgorithm;
        private bool m_IsDisposed;
        private bool m_IsHashing;
        private readonly byte[] m_KeyBuffer;
        private readonly byte[] m_Padded;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HMAC" /> class. This class cannot be inherited.
        /// </summary>
        /// <param name="hash">The underlying hash algorithm to use.</param>
        /// <remarks>A random key will be generated and used by the HMAC.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="hash" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        public HMAC(HashAlgorithm hash) : this(hash, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HMAC" /> class.
        /// </summary>
        /// <param name="hash">The underlying hash algorithm to use.</param>
        /// <param name="rgbKey">The key to use for the HMAC -or- a null reference (<b>Nothing</b> in Visual Basic).</param>
        /// <remarks>If <paramref name="rgbKey" /> is a null reference, the HMAC class will generate a random key.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="hash" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        public HMAC(HashAlgorithm hash, byte[] rgbKey)
        {
            if (hash == null)
                throw new ArgumentNullException("hash", ResourceController.GetString("Error_ParamNull"));
            if (rgbKey == null)
            {
                rgbKey = new byte[hash.HashSize/8];
                new RNGCryptoServiceProvider().GetBytes(rgbKey);
            }
            this.m_HashAlgorithm = hash;
            this.Key = (byte[]) rgbKey.Clone();
            this.m_KeyBuffer = new byte[64];
            this.m_Padded = new byte[64];
            this.Initialize();
        }

        /// <summary>
        ///     Gets the size of the computed hash code in bits.
        /// </summary>
        /// <value>The size of the computed hash code in bits.</value>
        public override int HashSize
        {
            get { return this.m_HashAlgorithm.HashSize; }
        }

        /// <summary>
        ///     Initializes the HMAC.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The HMAC instance has been disposed.</exception>
        public override void Initialize()
        {
            if (this.m_IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            this.m_HashAlgorithm.Initialize();
            this.m_IsHashing = false;
            this.State = 0;
            Array.Clear(this.m_KeyBuffer, 0, this.m_KeyBuffer.Length);
        }

        /// <summary>
        ///     Routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input for which to compute the hash code. </param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data. </param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data. </param>
        /// <exception cref="ObjectDisposedException">The HMAC instance has been disposed.</exception>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (this.m_IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            if (!this.m_IsHashing)
            {
                byte[] key;
                if (this.Key.Length > 64)
                    key = this.m_HashAlgorithm.ComputeHash(this.Key);
                else
                    key = this.Key;
                Array.Copy(key, 0, this.m_KeyBuffer, 0, key.Length);
                for (int i = 0; i < 64; i++)
                    this.m_Padded[i] = (byte) (this.m_KeyBuffer[i] ^ 0x36);
                this.m_HashAlgorithm.TransformBlock(this.m_Padded, 0, this.m_Padded.Length, this.m_Padded, 0);
                this.m_IsHashing = true;
            }
            this.m_HashAlgorithm.TransformBlock(array, ibStart, cbSize, array, ibStart);
        }

        /// <summary>
        ///     Finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        /// <exception cref="ObjectDisposedException">The HMAC instance has been disposed.</exception>
        protected override byte[] HashFinal()
        {
            if (this.m_IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            this.m_HashAlgorithm.TransformFinalBlock(new byte[0], 0, 0);
            byte[] dataHash = this.m_HashAlgorithm.Hash;
            for (int i = 0; i < 64; i++)
                this.m_Padded[i] = (byte) (this.m_KeyBuffer[i] ^ 0x5C);
            this.m_HashAlgorithm.Initialize();
            this.m_HashAlgorithm.TransformBlock(this.m_Padded, 0, this.m_Padded.Length, this.m_Padded, 0);
            this.m_HashAlgorithm.TransformFinalBlock(dataHash, 0, dataHash.Length);
            dataHash = this.m_HashAlgorithm.Hash;
            Array.Clear(this.m_KeyBuffer, 0, this.m_KeyBuffer.Length);
            this.m_IsHashing = false;
            return dataHash;
        }

        /// <summary>
        ///     Releases the resources used by the HMAC.
        /// </summary>
        /// <param name="disposing">
        ///     <b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only
        ///     unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            this.m_IsDisposed = true;
            base.Dispose(true);
            this.m_HashAlgorithm.Clear();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Finalizes the HMAC.
        /// </summary>
        ~HMAC()
        {
            this.m_HashAlgorithm.Clear();
        }
    }
}
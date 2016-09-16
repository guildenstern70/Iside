/*
 * LLCryptoLib - Advanced .NET Encryption and Hashing Library
 * v.$id$
 * 
 * The contents of this file are subject to the license distributed with
 * the package (the License). This file cannot be distributed without the 
 * original LittleLite Software license file. The distribution of this
 * file is subject to the agreement between the licensee and LittleLite
 * Software.
 * 
 * Customer that has purchased Source Code License may alter this
 * file and distribute the modified binary redistributables with applications. 
 * Except as expressly authorized in the License, customer shall not rent,
 * lease, distribute, sell, make available for download of this file. 
 * 
 * This software is not Open Source, nor Free. Its usage must adhere
 * with the License obtained from LittleLite Software.
 * 
 * The source code in this file may be derived, all or in part, from existing
 * other source code, where the original license permit to do so.
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */

using System;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    // http://www.ietf.org/rfc/rfc2104.txt
    /// <summary>
    /// Implements the HMAC keyed message authentication code algorithm.
    /// </summary>
    public sealed class HMAC : KeyedHashAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HMAC"/> class. This class cannot be inherited.
        /// </summary>
        /// <param name="hash">The underlying hash algorithm to use.</param>
        /// <remarks>A random key will be generated and used by the HMAC.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        public HMAC(HashAlgorithm hash) : this(hash, null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="HMAC"/> class.
        /// </summary>
        /// <param name="hash">The underlying hash algorithm to use.</param>
        /// <param name="rgbKey">The key to use for the HMAC -or- a null reference (<b>Nothing</b> in Visual Basic).</param>
        /// <remarks>If <paramref name="rgbKey"/> is a null reference, the HMAC class will generate a random key.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        public HMAC(HashAlgorithm hash, byte[] rgbKey)
        {
            if (hash == null)
                throw new ArgumentNullException("hash", ResourceController.GetString("Error_ParamNull"));
            if (rgbKey == null)
            {
                rgbKey = new byte[hash.HashSize / 8];
                new RNGCryptoServiceProvider().GetBytes(rgbKey);
            }
            m_HashAlgorithm = hash;
            this.Key = (byte[])rgbKey.Clone();
            m_KeyBuffer = new byte[64];
            m_Padded = new byte[64];
            Initialize();
        }
        /// <summary>
        /// Initializes the HMAC.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The HMAC instance has been disposed.</exception>
        public override void Initialize()
        {
            if (m_IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName, ResourceController.GetString("Error_Disposed"));
            m_HashAlgorithm.Initialize();
            m_IsHashing = false;
            this.State = 0;
            Array.Clear(m_KeyBuffer, 0, m_KeyBuffer.Length);
        }
        /// <summary>
        /// Routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input for which to compute the hash code. </param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data. </param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data. </param>
        /// <exception cref="ObjectDisposedException">The HMAC instance has been disposed.</exception>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (m_IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName, ResourceController.GetString("Error_Disposed"));
            if (!m_IsHashing)
            {
                byte[] key;
                if (this.Key.Length > 64)
                    key = m_HashAlgorithm.ComputeHash(this.Key);
                else
                    key = this.Key;
                Array.Copy(key, 0, m_KeyBuffer, 0, key.Length);
                for (int i = 0; i < 64; i++)
                    m_Padded[i] = (byte)(m_KeyBuffer[i] ^ 0x36);
                m_HashAlgorithm.TransformBlock(m_Padded, 0, m_Padded.Length, m_Padded, 0);
                m_IsHashing = true;
            }
            m_HashAlgorithm.TransformBlock(array, ibStart, cbSize, array, ibStart);
        }
        /// <summary>
        /// Finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        /// <exception cref="ObjectDisposedException">The HMAC instance has been disposed.</exception>
        protected override byte[] HashFinal()
        {
            if (m_IsDisposed)
                throw new ObjectDisposedException(this.GetType().FullName, ResourceController.GetString("Error_Disposed"));
            m_HashAlgorithm.TransformFinalBlock(new byte[0], 0, 0);
            byte[] dataHash = m_HashAlgorithm.Hash;
            for (int i = 0; i < 64; i++)
                m_Padded[i] = (byte)(m_KeyBuffer[i] ^ 0x5C);
            m_HashAlgorithm.Initialize();
            m_HashAlgorithm.TransformBlock(m_Padded, 0, m_Padded.Length, m_Padded, 0);
            m_HashAlgorithm.TransformFinalBlock(dataHash, 0, dataHash.Length);
            dataHash = m_HashAlgorithm.Hash;
            Array.Clear(m_KeyBuffer, 0, m_KeyBuffer.Length);
            m_IsHashing = false;
            return dataHash;
        }
        /// <summary>
        /// Gets the size of the computed hash code in bits.
        /// </summary>
        /// <value>The size of the computed hash code in bits.</value>
        public override int HashSize
        {
            get
            {
                return m_HashAlgorithm.HashSize;
            }
        }
        /// <summary>
        /// Releases the resources used by the HMAC.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            m_IsDisposed = true;
            base.Dispose(true);
            m_HashAlgorithm.Clear();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finalizes the HMAC.
        /// </summary>
        ~HMAC()
        {
            m_HashAlgorithm.Clear();
        }

        private HashAlgorithm m_HashAlgorithm;
        private byte[] m_KeyBuffer;
        private bool m_IsHashing;
        private bool m_IsDisposed;
        private byte[] m_Padded;
    }
}
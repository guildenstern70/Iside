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
using System.Runtime.InteropServices;
using LLCryptoLib.Security.Resources;
using LLCryptoLib.Security.Win32;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    /// Computes the <see cref="MD4"/> hash for the input data using the implementation provided by the cryptographic service provider (CSP).
    /// </summary>
    /// <remarks>Warning: The MD4 algorithm is a broken algorithm. It should <i>only</i> be used for compatibility with older systems.</remarks>
    public sealed class MD4CryptoServiceProvider : MD4
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MD4CryptoServiceProvider"/> class. This class cannot be inherited.
        /// </summary>
        public MD4CryptoServiceProvider()
        {
            // acquire an MD4 context
            m_Provider = CryptoHandle.Handle;
            Initialize();
        }
        /// <summary>
        /// Initializes an instance of <see cref="MD4CryptoServiceProvider"/>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The MD4CryptoServiceProvider instance has been disposed.</exception>
        public override void Initialize()
        {
            if (m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName, ResourceController.GetString("Error_Disposed"));
            if (m_Hash != IntPtr.Zero)
            {
                NativeMethods.CryptDestroyHash(m_Hash);
            }
            NativeMethods.CryptCreateHash(m_Provider, NativeMethods.CALG_MD4, IntPtr.Zero, 0, out m_Hash);
        }
        /// <summary>
        /// Routes data written to the object into the <see cref="MD4"/> hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The array of data bytes.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the array to use as data.</param>
        /// <exception cref="ObjectDisposedException">The MD4CryptoServiceProvider instance has been disposed.</exception>
        /// <exception cref="CryptographicException">The data could not be hashed.</exception>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName, ResourceController.GetString("Error_Disposed"));
            byte[] copy = new byte[cbSize];
            Array.Copy(array, ibStart, copy, 0, cbSize);
            if (NativeMethods.CryptHashData(m_Hash, copy, copy.Length, 0) == 0)
                throw new CryptographicException(ResourceController.GetString("Error_HashData"));
        }
        /// <summary>
        /// Returns the computed <see cref="MD4CryptoServiceProvider"/> hash as an array of bytes after all data has been written to the object.
        /// </summary>
        /// <returns>The computed hash value.</returns>
        /// <exception cref="ObjectDisposedException">The MD4CryptoServiceProvider instance has been disposed.</exception>
        /// <exception cref="CryptographicException">The data could not be hashed.</exception>
        protected override byte[] HashFinal()
        {
            if (m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName, ResourceController.GetString("Error_Disposed"));
            byte[] buffer = new byte[16];
            int length = buffer.Length;
            if (NativeMethods.CryptGetHashParam(m_Hash, NativeMethods.HP_HASHVAL, buffer, ref length, 0) == 0)
                throw new CryptographicException(ResourceController.GetString("Error_HashRead"));
            return buffer;
        }
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="MD4CryptoServiceProvider"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (m_Hash != IntPtr.Zero)
                {
                    NativeMethods.CryptDestroyHash(m_Hash);
                    m_Hash = IntPtr.Zero;
                }
                base.Dispose(disposing);
                GC.SuppressFinalize(this);
                m_Disposed = true;
            }
        }
        /// <summary>
        /// Finalizes the MD4CryptoServiceProvider.
        /// </summary>
        ~MD4CryptoServiceProvider()
        {
            Clear();
        }

        private IntPtr m_Provider;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        private IntPtr m_Hash;
        private bool m_Disposed;
    }
}
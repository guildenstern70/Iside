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
using LLCryptoLib.Security.Win32;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    /// Represents an ARCFour managed ICryptoTransform.
    /// </summary>
    internal class RC4UnmanagedTransform : ICryptoTransform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RC4UnmanagedTransform"/> class.
        /// </summary>
        /// <param name="key">The key used to initialize the RC4 state.</param>
        public RC4UnmanagedTransform(byte[] key)
        {
            m_Key = new SymmetricKey(CryptoAlgorithm.RC4, key);
        }
        /// <summary>
        /// Gets a value indicating whether the current transform can be reused.
        /// </summary>
        /// <value>This property returns <b>true</b>.</value>
        public bool CanReuseTransform
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Gets a value indicating whether multiple blocks can be transformed.
        /// </summary>
        /// <value>This property returns <b>true</b>.</value>
        public bool CanTransformMultipleBlocks
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Gets the input block size.
        /// </summary>
        /// <value>This property returns 1.</value>
        public int InputBlockSize
        {
            get
            {
                return 1;
            }
        }
        /// <summary>
        /// Gets the output block size.
        /// </summary>
        /// <value>This property returns 1.</value>
        public int OutputBlockSize
        {
            get
            {
                return 1;
            }
        }
        /// <summary>
        /// Transforms the specified region of the input byte array and copies the resulting transform to the specified region of the output byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform.</param>
        /// <param name="inputOffset">The offset into the input byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the input byte array to use as data.</param>
        /// <param name="outputBuffer">The output to which to write the transform.</param>
        /// <param name="outputOffset">The offset into the output byte array from which to begin writing data.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="inputBuffer"/> or <paramref name="outputBuffer"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="inputOffset"/>, <paramref name="inputCount"/> or <paramref name="outputOffset"/> is invalid.</exception>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            if (m_Key == null)
                throw new ObjectDisposedException(this.GetType().FullName, ResourceController.GetString("Error_Disposed"));
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer", ResourceController.GetString("Error_ParamNull"));
            if (outputBuffer == null)
                throw new ArgumentNullException("outputBuffer", ResourceController.GetString("Error_ParamNull"));
            if (inputCount < 0 || inputOffset < 0 || outputOffset < 0 || inputOffset + inputCount > inputBuffer.Length || outputBuffer.Length - outputOffset < inputCount)
                throw new ArgumentOutOfRangeException(ResourceController.GetString("Error_ParamOutOfRange"));
            byte[] buffer = new byte[inputCount];
            int length = buffer.Length;
            Array.Copy(inputBuffer, inputOffset, buffer, 0, length);
            if (NativeMethods.CryptEncrypt(m_Key.Handle, IntPtr.Zero, 0, 0, buffer, ref length, length) == 0)
                throw new CryptographicException(ResourceController.GetString("Error_Transform"));
            Array.Copy(buffer, 0, outputBuffer, outputOffset, length);
            Array.Clear(buffer, 0, buffer.Length);
            return length;
        }
        /// <summary>
        /// Transforms the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <returns>The computed transform.</returns>
        /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="inputBuffer"/> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="inputOffset"/> or <paramref name="inputCount"/> is invalid.</exception>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (m_Key == null)
                throw new ObjectDisposedException(this.GetType().FullName, ResourceController.GetString("Error_Disposed"));
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer", ResourceController.GetString("Error_ParamNull"));
            if (inputCount < 0 || inputOffset < 0 || inputOffset + inputCount > inputBuffer.Length)
                throw new ArgumentOutOfRangeException(ResourceController.GetString("Error_ParamOutOfRange"));
            byte[] buffer = new byte[inputCount];
            int length = buffer.Length;
            Array.Copy(inputBuffer, inputOffset, buffer, 0, length);
            if (NativeMethods.CryptEncrypt(m_Key.Handle, IntPtr.Zero, 1, 0, buffer, ref length, length) == 0)
                throw new CryptographicException(ResourceController.GetString("Error_Transform"));
            return buffer;
        }
        /// <summary>
        /// Disposes of the cryptographic parameters.
        /// </summary>
        public void Dispose()
        {
            if (m_Key != null)
            {
                m_Key.Dispose();
                m_Key = null;
            }
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finalizes the object.
        /// </summary>
        ~RC4UnmanagedTransform()
        {
            Dispose();
        }
        private SymmetricKey m_Key;
    }
}
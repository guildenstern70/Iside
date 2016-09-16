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
using LLCryptoLib.Security;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    /// Represents an ARCFour managed ICryptoTransform.
    /// </summary>
    internal unsafe sealed class ARCFourManagedTransform : ICryptoTransform
    {
        /// <summary>
        /// Initializes a new instance of the ARCFourManagedTransform class.
        /// </summary>
        /// <param name="key">The key used to initialize the ARCFour state.</param>
        public ARCFourManagedTransform(byte[] key)
        {
            m_Key = (byte[])key.Clone();
            m_KeyLen = key.Length;
            m_Permutation = new byte[256];
            Init();
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
        /// <value>The size of the input data blocks in bytes.</value>
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
        /// <value>The size of the input data blocks in bytes.</value>
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
            if (m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer", ResourceController.GetString("Error_ParamNull"));
            if (outputBuffer == null)
                throw new ArgumentNullException("outputBuffer", ResourceController.GetString("Error_ParamNull"));
            if (inputOffset < 0 || outputOffset < 0 || inputOffset + inputCount > inputBuffer.Length || outputOffset + inputCount > outputBuffer.Length)
                throw new ArgumentOutOfRangeException(ResourceController.GetString("Error_ParamOutOfRange"));
            byte j, temp;
            int length = inputOffset + inputCount;
            fixed (byte* permutation = m_Permutation, output = outputBuffer, input = inputBuffer)
            {
                for (; inputOffset < length; inputOffset++, outputOffset++)
                {
                    // update indices
                    m_Index1 = (byte)((m_Index1 + 1) % 256);
                    m_Index2 = (byte)((m_Index2 + permutation[m_Index1]) % 256);
                    // swap m_State.permutation[m_State.index1] and m_State.permutation[m_State.index2]
                    temp = permutation[m_Index1];
                    permutation[m_Index1] = permutation[m_Index2];
                    permutation[m_Index2] = temp;
                    // transform byte
                    j = (byte)((permutation[m_Index1] + permutation[m_Index2]) % 256);
                    output[outputOffset] = (byte)(input[inputOffset] ^ permutation[j]);
                }
            }
            return inputCount;
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
            if (m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            byte[] ret = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, ret, 0);
            Init();
            return ret;
        }
        /// <summary>
        /// This method (re)initializes the cipher.
        /// </summary>
        private void Init()
        {
            byte temp;
            // init state variable
            for (int i = 0; i < m_Permutation.Length; i++)
            {
                m_Permutation[i] = (byte)i;
            }
            m_Index1 = 0;
            m_Index2 = 0;
            // randomize, using key
            for (int j = 0, i = 0; i < m_Permutation.Length; i++)
            {
                j = (j + m_Permutation[i] + m_Key[i % m_KeyLen]) % 256;
                // swap m_State.permutation[i] and m_State.permutation[j]
                temp = m_Permutation[i];
                m_Permutation[i] = m_Permutation[j];
                m_Permutation[j] = temp;
            }
        }
        /// <summary>
        /// Disposes of the cryptographic parameters.
        /// </summary>
        public void Dispose()
        {
            if (!m_Disposed)
            {
                Array.Clear(m_Key, 0, m_Key.Length);
                Array.Clear(m_Permutation, 0, m_Permutation.Length);
                m_Index1 = 0;
                m_Index2 = 0;
                m_Disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        /// <summary>
        /// Finalizes the object.
        /// </summary>
        ~ARCFourManagedTransform()
        {
            Dispose();
        }

        private byte[] m_Key;
        private int m_KeyLen;
        private byte[] m_Permutation;
        private byte m_Index1;
        private byte m_Index2;
        private bool m_Disposed;
    }
}
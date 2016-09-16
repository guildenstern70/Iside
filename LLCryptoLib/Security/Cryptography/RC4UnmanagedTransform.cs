using System;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;
using LLCryptoLib.Security.Win32;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Represents an ARCFour managed ICryptoTransform.
    /// </summary>
    internal class RC4UnmanagedTransform : ICryptoTransform
    {
        private SymmetricKey m_Key;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RC4UnmanagedTransform" /> class.
        /// </summary>
        /// <param name="key">The key used to initialize the RC4 state.</param>
        public RC4UnmanagedTransform(byte[] key)
        {
            this.m_Key = new SymmetricKey(CryptoAlgorithm.RC4, key);
        }

        /// <summary>
        ///     Gets a value indicating whether the current transform can be reused.
        /// </summary>
        /// <value>This property returns <b>true</b>.</value>
        public bool CanReuseTransform
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets a value indicating whether multiple blocks can be transformed.
        /// </summary>
        /// <value>This property returns <b>true</b>.</value>
        public bool CanTransformMultipleBlocks
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets the input block size.
        /// </summary>
        /// <value>This property returns 1.</value>
        public int InputBlockSize
        {
            get { return 1; }
        }

        /// <summary>
        ///     Gets the output block size.
        /// </summary>
        /// <value>This property returns 1.</value>
        public int OutputBlockSize
        {
            get { return 1; }
        }

        /// <summary>
        ///     Transforms the specified region of the input byte array and copies the resulting transform to the specified region
        ///     of the output byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform.</param>
        /// <param name="inputOffset">The offset into the input byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the input byte array to use as data.</param>
        /// <param name="outputBuffer">The output to which to write the transform.</param>
        /// <param name="outputOffset">The offset into the output byte array from which to begin writing data.</param>
        /// <returns>The number of bytes written.</returns>
        /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="inputBuffer" /> or <paramref name="outputBuffer" /> is a null
        ///     reference (<b>Nothing</b> in Visual Basic).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="inputOffset" />, <paramref name="inputCount" /> or
        ///     <paramref name="outputOffset" /> is invalid.
        /// </exception>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer,
            int outputOffset)
        {
            if (this.m_Key == null)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer", ResourceController.GetString("Error_ParamNull"));
            if (outputBuffer == null)
                throw new ArgumentNullException("outputBuffer", ResourceController.GetString("Error_ParamNull"));
            if ((inputCount < 0) || (inputOffset < 0) || (outputOffset < 0) ||
                (inputOffset + inputCount > inputBuffer.Length) || (outputBuffer.Length - outputOffset < inputCount))
                throw new ArgumentOutOfRangeException(ResourceController.GetString("Error_ParamOutOfRange"));
            byte[] buffer = new byte[inputCount];
            int length = buffer.Length;
            Array.Copy(inputBuffer, inputOffset, buffer, 0, length);
            if (NativeMethods.CryptEncrypt(this.m_Key.Handle, IntPtr.Zero, 0, 0, buffer, ref length, length) == 0)
                throw new CryptographicException(ResourceController.GetString("Error_Transform"));
            Array.Copy(buffer, 0, outputBuffer, outputOffset, length);
            Array.Clear(buffer, 0, buffer.Length);
            return length;
        }

        /// <summary>
        ///     Transforms the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <returns>The computed transform.</returns>
        /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="inputBuffer" /> is a null reference (<b>Nothing</b> in Visual
        ///     Basic).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="inputOffset" /> or <paramref name="inputCount" /> is
        ///     invalid.
        /// </exception>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (this.m_Key == null)
                throw new ObjectDisposedException(this.GetType().FullName,
                    ResourceController.GetString("Error_Disposed"));
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer", ResourceController.GetString("Error_ParamNull"));
            if ((inputCount < 0) || (inputOffset < 0) || (inputOffset + inputCount > inputBuffer.Length))
                throw new ArgumentOutOfRangeException(ResourceController.GetString("Error_ParamOutOfRange"));
            byte[] buffer = new byte[inputCount];
            int length = buffer.Length;
            Array.Copy(inputBuffer, inputOffset, buffer, 0, length);
            if (NativeMethods.CryptEncrypt(this.m_Key.Handle, IntPtr.Zero, 1, 0, buffer, ref length, length) == 0)
                throw new CryptographicException(ResourceController.GetString("Error_Transform"));
            return buffer;
        }

        /// <summary>
        ///     Disposes of the cryptographic parameters.
        /// </summary>
        public void Dispose()
        {
            if (this.m_Key != null)
            {
                this.m_Key.Dispose();
                this.m_Key = null;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Finalizes the object.
        /// </summary>
        ~RC4UnmanagedTransform()
        {
            this.Dispose();
        }
    }
}
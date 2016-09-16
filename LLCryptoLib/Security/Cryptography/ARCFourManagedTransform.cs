using System;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Represents an ARCFour managed ICryptoTransform.
    /// </summary>
    internal sealed unsafe class ARCFourManagedTransform : ICryptoTransform
    {
        private bool m_Disposed;
        private byte m_Index1;
        private byte m_Index2;

        private readonly byte[] m_Key;
        private readonly int m_KeyLen;
        private readonly byte[] m_Permutation;

        /// <summary>
        ///     Initializes a new instance of the ARCFourManagedTransform class.
        /// </summary>
        /// <param name="key">The key used to initialize the ARCFour state.</param>
        public ARCFourManagedTransform(byte[] key)
        {
            this.m_Key = (byte[]) key.Clone();
            this.m_KeyLen = key.Length;
            this.m_Permutation = new byte[256];
            this.Init();
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
        /// <value>The size of the input data blocks in bytes.</value>
        public int InputBlockSize
        {
            get { return 1; }
        }

        /// <summary>
        ///     Gets the output block size.
        /// </summary>
        /// <value>The size of the input data blocks in bytes.</value>
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
            if (this.m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer", ResourceController.GetString("Error_ParamNull"));
            if (outputBuffer == null)
                throw new ArgumentNullException("outputBuffer", ResourceController.GetString("Error_ParamNull"));
            if ((inputOffset < 0) || (outputOffset < 0) || (inputOffset + inputCount > inputBuffer.Length) ||
                (outputOffset + inputCount > outputBuffer.Length))
                throw new ArgumentOutOfRangeException(ResourceController.GetString("Error_ParamOutOfRange"));
            byte j, temp;
            int length = inputOffset + inputCount;
            fixed (byte* permutation = this.m_Permutation, output = outputBuffer, input = inputBuffer)
            {
                for (; inputOffset < length; inputOffset++, outputOffset++)
                {
                    // update indices
                    this.m_Index1 = (byte) ((this.m_Index1 + 1)%256);
                    this.m_Index2 = (byte) ((this.m_Index2 + permutation[this.m_Index1])%256);
                    // swap m_State.permutation[m_State.index1] and m_State.permutation[m_State.index2]
                    temp = permutation[this.m_Index1];
                    permutation[this.m_Index1] = permutation[this.m_Index2];
                    permutation[this.m_Index2] = temp;
                    // transform byte
                    j = (byte) ((permutation[this.m_Index1] + permutation[this.m_Index2])%256);
                    output[outputOffset] = (byte) (input[inputOffset] ^ permutation[j]);
                }
            }
            return inputCount;
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
            if (this.m_Disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            byte[] ret = new byte[inputCount];
            this.TransformBlock(inputBuffer, inputOffset, inputCount, ret, 0);
            this.Init();
            return ret;
        }

        /// <summary>
        ///     Disposes of the cryptographic parameters.
        /// </summary>
        public void Dispose()
        {
            if (!this.m_Disposed)
            {
                Array.Clear(this.m_Key, 0, this.m_Key.Length);
                Array.Clear(this.m_Permutation, 0, this.m_Permutation.Length);
                this.m_Index1 = 0;
                this.m_Index2 = 0;
                this.m_Disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        ///     This method (re)initializes the cipher.
        /// </summary>
        private void Init()
        {
            byte temp;
            // init state variable
            for (int i = 0; i < this.m_Permutation.Length; i++)
            {
                this.m_Permutation[i] = (byte) i;
            }
            this.m_Index1 = 0;
            this.m_Index2 = 0;
            // randomize, using key
            for (int j = 0, i = 0; i < this.m_Permutation.Length; i++)
            {
                j = (j + this.m_Permutation[i] + this.m_Key[i%this.m_KeyLen])%256;
                // swap m_State.permutation[i] and m_State.permutation[j]
                temp = this.m_Permutation[i];
                this.m_Permutation[i] = this.m_Permutation[j];
                this.m_Permutation[j] = temp;
            }
        }

        /// <summary>
        ///     Finalizes the object.
        /// </summary>
        ~ARCFourManagedTransform()
        {
            this.Dispose();
        }
    }
}
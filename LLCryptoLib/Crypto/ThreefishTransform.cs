using System;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    /// </summary>
    public enum ThreefishTransformType
    {
        /// <summary>
        ///     {35A90EBF-F421-44A3-BE3A-47C72AFE47FE}
        /// </summary>
        Encrypt,

        /// <summary>
        /// </summary>
        Decrypt
    }

    /// <summary>
    ///     Transformation for ThreeFish encryption algorithm
    /// </summary>
    public class ThreefishTransform : ICryptoTransform
    {
        private readonly ulong[] _block;

        private readonly ThreefishCipher _cipher;

        private readonly int _cipherBytes;

        private readonly CipherMode _cipherMode;
        private readonly int _cipherWords;
        private readonly ulong[] _iv;
        private readonly PaddingMode _paddingMode;

        // Used when in a stream ciphering mode
        private readonly byte[] _streamBytes;
        private readonly ulong[] _tempBlock;
        private readonly TransformFunc _transformFunc;
        private int _usedStreamBytes;

        private readonly bool isEncrypting;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThreefishTransform" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <param name="type">The type.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="padding">The padding.</param>
        public ThreefishTransform(
            byte[] key, byte[] iv, ThreefishTransformType type, CipherMode mode, PaddingMode padding
        )
        {
            this._cipherMode = mode;
            this._paddingMode = padding;

            this._cipherBytes = key.Length;
            this._cipherWords = key.Length/8;
            this.OutputBlockSize = key.Length*8;

            // Allocate working blocks now so that we don't
            // have to allocate them each time 
            // Transform(Final)Block is called
            this._block = new ulong[this._cipherWords];
            this._tempBlock = new ulong[this._cipherWords];
            this._streamBytes = new byte[this._cipherBytes];

            // Allocate IV and set value
            this._iv = new ulong[this._cipherWords];
            GetBytes(iv, 0, this._iv, this._cipherBytes);

            // Figure out which cipher we need based on
            // the cipher bit size
            switch (this.OutputBlockSize)
            {
                case 256:
                    this._cipher = new Threefish256();
                    break;
                case 512:
                    this._cipher = new Threefish512();
                    break;
                case 1024:
                    this._cipher = new Threefish1024();
                    break;

                default:
                    throw new CryptographicException("Unsupported key/block size.");
            }

            this.isEncrypting = type == ThreefishTransformType.Encrypt;

            switch (this._cipherMode)
            {
                case CipherMode.ECB:
                    this._transformFunc = this.isEncrypting ? this.EcbEncrypt : new TransformFunc(this.EcbDecrypt);
                    break;
                case CipherMode.CBC:
                    this._transformFunc = this.isEncrypting ? this.CbcEncrypt : new TransformFunc(this.CbcDecrypt);
                    break;
                case CipherMode.OFB:
                    this._transformFunc = this.OfbApplyStream;
                    break;
                case CipherMode.CFB:
                    this._transformFunc = this.isEncrypting ? this.CfbEncrypt : new TransformFunc(this.CfbDecrypt);
                    break;
                case CipherMode.CTS:
                    throw new CryptographicException("CTS mode not supported.");
            }

            // Set the key
            var keyWords = new ulong[this._cipherWords];
            GetBytes(key, 0, keyWords, this._cipherBytes);
            this._cipher.SetKey(keyWords);

            this.InitializeBlocks();
        }

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // nothing to dispose
        }

        #endregion

        // (Re)initializes the blocks for encryption
        private void InitializeBlocks()
        {
            switch (this._cipherMode)
            {
                case CipherMode.ECB:
                case CipherMode.CBC:
                    // Clear the working block
                    for (int i = 0; i < this._cipherWords; i++)
                        this._block[i] = 0;
                    break;

                case CipherMode.OFB:
                    // Copy the IV to the working block
                    for (int i = 0; i < this._cipherWords; i++)
                        this._block[i] = this._iv[i];

                    break;

                case CipherMode.CFB:
                    // Copy IV to cipher stream bytes
                    PutBytes(this._iv, this._streamBytes, 0, this._cipherBytes);
                    break;
            }

            this._usedStreamBytes = this._cipherBytes;
        }

        private delegate int TransformFunc(
            byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset);

        #region Utils

        private static ulong GetUInt64(byte[] buf, int offset)
        {
            ulong v = buf[offset];
            v |= (ulong) buf[offset + 1] << 8;
            v |= (ulong) buf[offset + 2] << 16;
            v |= (ulong) buf[offset + 3] << 24;
            v |= (ulong) buf[offset + 4] << 32;
            v |= (ulong) buf[offset + 5] << 40;
            v |= (ulong) buf[offset + 6] << 48;
            v |= (ulong) buf[offset + 7] << 56;
            return v;
        }

        private static void PutUInt64(byte[] buf, int offset, ulong v)
        {
            buf[offset] = (byte) (v & 0xff);
            buf[offset + 1] = (byte) ((v >> 8) & 0xff);
            buf[offset + 2] = (byte) ((v >> 16) & 0xff);
            buf[offset + 3] = (byte) ((v >> 24) & 0xff);
            buf[offset + 4] = (byte) ((v >> 32) & 0xff);
            buf[offset + 5] = (byte) ((v >> 40) & 0xff);
            buf[offset + 6] = (byte) ((v >> 48) & 0xff);
            buf[offset + 7] = (byte) (v >> 56);
        }

        private static void GetBytes(byte[] input, int offset, ulong[] output, int byteCount)
        {
            for (int i = 0; i < byteCount; i += 8)
            {
                output[i/8] = GetUInt64(input, i + offset);
            }
        }

        private static void PutBytes(ulong[] input, byte[] output, int offset, int byteCount)
        {
            for (int i = 0; i < byteCount; i += 8)
            {
                PutUInt64(output, i + offset, input[i/8]);
            }
        }

        #endregion

        #region ModeTransformFunctions

        // ECB mode encryption
        private int EcbEncrypt(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset)
        {
            if (inputCount >= this._cipherBytes)
            {
                GetBytes(input, inputOffset, this._block, this._cipherBytes);
                this._cipher.Encrypt(this._block, this._block);
                PutBytes(this._block, output, outputOffset, this._cipherBytes);

                return this._cipherBytes;
            }

            return 0;
        }

        // ECB mode decryption
        private int EcbDecrypt(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset)
        {
            if (inputCount >= this._cipherBytes)
            {
                GetBytes(input, inputOffset, this._block, this._cipherBytes);
                this._cipher.Decrypt(this._block, this._block);
                PutBytes(this._block, output, outputOffset, this._cipherBytes);

                return this._cipherBytes;
            }

            return 0;
        }

        // CBC mode encryption
        private int CbcEncrypt(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset)
        {
            if (inputCount >= this._cipherBytes)
            {
                int i;

                GetBytes(input, inputOffset, this._block, this._cipherBytes);

                // Apply the IV
                for (i = 0; i < this._cipherWords; i++)
                    this._block[i] ^= this._iv[i];

                this._cipher.Encrypt(this._block, this._block);

                // Copy the output to the IV
                for (i = 0; i < this._cipherWords; i++)
                    this._iv[i] = this._block[i];

                PutBytes(this._block, output, outputOffset, this._cipherBytes);

                return this._cipherBytes;
            }

            return 0;
        }

        // CBC mode encryption
        private int CbcDecrypt(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset)
        {
            if (inputCount >= this._cipherBytes)
            {
                int i;

                GetBytes(input, inputOffset, this._block, this._cipherBytes);

                // Copy the block to the temp block for later (wink wink)
                for (i = 0; i < this._cipherWords; i++)
                    this._tempBlock[i] = this._block[i];

                this._cipher.Decrypt(this._block, this._block);

                // Apply the IV and copy temp block
                // to IV
                for (i = 0; i < this._cipherWords; i++)
                {
                    this._block[i] ^= this._iv[i];
                    this._iv[i] = this._tempBlock[i];
                }

                PutBytes(this._block, output, outputOffset, this._cipherBytes);

                return this._cipherBytes;
            }

            return 0;
        }

        // OFB mode encryption/decryption
        private int OfbApplyStream(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset)
        {
            int i;

            // Input length doesn't matter in OFB, just encrypt
            // as much as we can
            for (i = 0; i < inputCount; i++)
            {
                // Generate new stream bytes if we've used
                // them all up
                if (this._usedStreamBytes >= this._cipherBytes)
                {
                    this._cipher.Encrypt(this._block, this._block);
                    PutBytes(this._block, this._streamBytes, 0, this._cipherBytes);
                    this._usedStreamBytes = 0;
                }

                // XOR input byte with stream byte, output it
                output[outputOffset + i] = (byte) (input[inputOffset + i] ^ this._streamBytes[this._usedStreamBytes]);
                this._usedStreamBytes++;
            }

            // Return bytes done
            return i;
        }

        // CFB mode encryption
        private int CfbEncrypt(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset)
        {
            int i;

            for (i = 0; i < inputCount; i++)
            {
                // Generate new stream bytes if we've used
                // them all up
                if (this._usedStreamBytes >= this._cipherBytes)
                {
                    // Copy cipher stream bytes to working block
                    // (this is the feedback)
                    GetBytes(this._streamBytes, 0, this._block, this._cipherBytes);
                    // Process
                    this._cipher.Encrypt(this._block, this._block);
                    // Put back
                    PutBytes(this._block, this._streamBytes, 0, this._cipherBytes);
                    // Reset for next time
                    this._usedStreamBytes = 0;
                }

                // XOR input byte with stream byte
                var b = (byte) (input[inputOffset + i] ^ this._streamBytes[this._usedStreamBytes]);
                // Output cipher byte
                output[outputOffset + i] = b;
                // Put cipher byte into stream bytes for the feedback
                this._streamBytes[this._usedStreamBytes] = b;

                this._usedStreamBytes++;
            }

            // Return bytes done
            return i;
        }

        // CFB mode decryption
        private int CfbDecrypt(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset)
        {
            int i;

            for (i = 0; i < inputCount; i++)
            {
                // Generate new stream bytes if we've used
                // them all up
                if (this._usedStreamBytes >= this._cipherBytes)
                {
                    // Copy cipher stream bytes to working block
                    // (this is the feedback)
                    GetBytes(this._streamBytes, 0, this._block, this._cipherBytes);
                    // Process
                    this._cipher.Encrypt(this._block, this._block);
                    // Put back
                    PutBytes(this._block, this._streamBytes, 0, this._cipherBytes);
                    // Reset for next time
                    this._usedStreamBytes = 0;
                }

                // Get ciphertext byte
                byte b = input[inputOffset + i];
                // XOR input byte with stream byte, output plaintext
                output[outputOffset + i] = (byte) (b ^ this._streamBytes[this._usedStreamBytes]);
                // Put ciphertext byte into stream bytes for the feedback
                this._streamBytes[this._usedStreamBytes] = b;

                this._usedStreamBytes++;
            }

            // Return bytes done
            return i;
        }

        #endregion

        #region ICryptoTransform Members

        /// <summary>
        ///     Gets a value indicating whether the current transform can be reused.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///     true if the current transform can be reused; otherwise, false.
        /// </returns>
        public bool CanReuseTransform
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets a value indicating whether multiple blocks can be transformed.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///     true if multiple blocks can be transformed; otherwise, false.
        /// </returns>
        public bool CanTransformMultipleBlocks
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets the input block size.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///     The size of the input data blocks in bytes.
        /// </returns>
        public int InputBlockSize
        {
            get { return this.OutputBlockSize; }
        }

        /// <summary>
        ///     Gets the output block size.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///     The size of the output data blocks in bytes.
        /// </returns>
        public int OutputBlockSize { get; }


        private void PadBlock(byte[] input, int inputOffset, int alreadyFilled)
        {
            // Apply the type of padding we're using
            switch (this._paddingMode)
            {
                case PaddingMode.None:
                    break;
                case PaddingMode.Zeros:
                    // Fill with zeros
                    for (int i = alreadyFilled; i < this._cipherBytes; i++)
                        input[i + inputOffset] = 0;

                    break;

                case PaddingMode.PKCS7:
                    // Fill each byte value with the number of
                    // bytes padded
                    for (int i = alreadyFilled; i < this._cipherBytes; i++)
                        input[i + inputOffset] = (byte) (this._cipherBytes - alreadyFilled);

                    break;

                case PaddingMode.ANSIX923:
                    // fill with zeros, set last byte
                    // to number of bytes padded
                    for (int i = alreadyFilled; i < this._cipherBytes; i++)
                    {
                        input[i + inputOffset] = 0;
                        // If its the last byte, set to number of bytes padded
                        if (i == this._cipherBytes - 1)
                            input[i + inputOffset] = (byte) (this._cipherBytes - alreadyFilled);
                    }

                    break;

                case PaddingMode.ISO10126:
                    // Fill remaining bytes with random values
                    if (alreadyFilled < this._cipherBytes)
                    {
                        var randBytes = new byte[this._cipherBytes - alreadyFilled];
                        new RNGCryptoServiceProvider().GetBytes(randBytes);

                        for (int i = alreadyFilled; i < this._cipherBytes; i++)
                            input[i + inputOffset] = randBytes[i - alreadyFilled];
                    }

                    break;
            }
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
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer,
            int outputOffset)
        {
            // Make sure the input count is evenly
            // divisible by the block size
            if ((inputCount & (this._cipherBytes - 1)) != 0)
                throw new CryptographicException("inputCount must be divisible by the block size.");

            int totalDone = 0;
            int done;
            // Apply as much of the transform as we can
            do
            {
                done = this._transformFunc(
                    inputBuffer,
                    inputOffset + totalDone,
                    inputCount - totalDone,
                    outputBuffer,
                    outputOffset + totalDone
                );

                totalDone += done;
            } while (done == this._cipherBytes);

            return totalDone;
        }

        /// <summary>
        ///     Transforms the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <returns>The computed transform.</returns>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            var output = new byte[inputCount];

            int totalDone = 0;
            int done;
            // Apply as much of the transform as we can
            do
            {
                done = this._transformFunc(
                    inputBuffer,
                    inputOffset + totalDone,
                    inputCount - totalDone,
                    output,
                    totalDone
                );

                totalDone += done;
            } while (done == this._cipherBytes);

            int remaining = inputCount - totalDone;

            // Do the padding and the final transform if
            // there's any data left
            if (totalDone < inputCount)
            {
                // Resize output buffer to be evenly
                // divisible by the block size
                if (inputCount%this._cipherBytes != 0)
                {
                    int outputSize = inputCount + (this._cipherBytes - inputCount%this._cipherBytes);
                    Array.Resize(ref output, outputSize);
                }

                // Copy remaining bytes over to the output
                for (int i = 0; i < remaining; i++)
                    output[i + totalDone] = inputBuffer[inputOffset + totalDone + i];

                // Pad the block
                this.PadBlock(output, totalDone, remaining);

                // Encrypt the block
                this._transformFunc(output, totalDone, this._cipherBytes, output, totalDone);
            }

            // Reinitialize the cipher
            this.InitializeBlocks();

            return output;
        }

        #endregion
    }
}
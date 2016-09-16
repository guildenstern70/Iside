using System.Diagnostics;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     XOR simple encryption algorithm.
    ///     XOR, also know as Exclusive OR, is a bitwise operator from binary mathematics.
    ///     The XOR operator returns a 1 when the value of either the first bit or the second bit is a 1.
    ///     The XOR operator returns a 0 when neither or both of the bits is 1.
    /// </summary>
    public sealed class XOR : SymmetricAlgorithm, ICryptoTransform
    {
        private readonly bool canReuseTransform = true;
        private readonly bool canTransformMultipleBlocks = false;
        private readonly int inputBlockSize = 16;
        private readonly int outputBlockSize = 16;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XOR" /> class.
        /// </summary>
        public XOR()
        {
            Trace.Write("XOR::XOR\n");

            this.LegalKeySizesValue = new[] {new KeySizes(128, 128, 0)}; // this is in bits - typical of MS
            this.KeySize = 128; // also in bits

            this.LegalBlockSizesValue = new[] {new KeySizes(128, 128, 0)}; // this is in bits - typical of MS
            this.BlockSize = 128; // also in bits
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources
        /// </summary>
        public new void Dispose()
        {
            Trace.Write("XORTransform::Dispose\n");
        }

        /// <summary>
        ///     Transforms the block.
        /// </summary>
        /// <param name="inputBuffer">The input buffer.</param>
        /// <param name="inputOffset">The input offset.</param>
        /// <param name="inputCount">The input count.</param>
        /// <param name="outputBuffer">The output buffer.</param>
        /// <param name="outputOffset">The output offset.</param>
        /// <returns></returns>
        public int TransformBlock(
            byte[] inputBuffer,
            int inputOffset,
            int inputCount,
            byte[] outputBuffer,
            int outputOffset
        )
        {
            Trace.Write(string.Format("XORTransform::TransformBlock {0} {1} {2}\n", inputOffset, inputCount,
                outputOffset));

            for (int i = 0; i < inputCount; i++)
            {
                outputBuffer[i + outputOffset] = (byte) (inputBuffer[i + inputOffset] ^ this.Key[i]);
            }

            return inputCount;
        }

        /// <summary>
        ///     Transforms the final block.
        /// </summary>
        /// <param name="inputBuffer">The input buffer.</param>
        /// <param name="inputOffset">The input offset.</param>
        /// <param name="inputCount">The input count.</param>
        /// <returns></returns>
        public byte[] TransformFinalBlock(
            byte[] inputBuffer,
            int inputOffset,
            int inputCount
        )
        {
            Trace.Write(string.Format("XORTransform::TransformFinalBlock {0} {1}\n", inputOffset, inputCount));
            byte[] outputBuffer = new byte[inputCount];
            for (int i = 0; i < inputCount; i++)
            {
                outputBuffer[i] = (byte) (inputBuffer[i + inputOffset] ^ this.Key[i]);
            }
            return outputBuffer;
        }

        /// <summary>
        ///     Gets a value indicating whether this instance can reuse transform.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can reuse transform; otherwise, <c>false</c>.
        /// </value>
        public bool CanReuseTransform
        {
            get
            {
                Trace.Write("XORTransform::CanReuseTransform get\n");
                return this.canReuseTransform;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance can transform multiple blocks.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can transform multiple blocks; otherwise, <c>false</c>.
        /// </value>
        public bool CanTransformMultipleBlocks
        {
            get
            {
                Trace.Write("XORTransform::CanTransformMultipleBlocks get\n");
                return this.canTransformMultipleBlocks;
            }
        }

        /// <summary>
        ///     Gets the size of the input block.
        /// </summary>
        /// <value>The size of the input block.</value>
        public int InputBlockSize
        {
            get
            {
                Trace.Write("XORTransform::InputBlockSize get\n");
                return this.inputBlockSize;
            }
        }

        /// <summary>
        ///     Gets the size of the output block.
        /// </summary>
        /// <value>The size of the output block.</value>
        public int OutputBlockSize
        {
            get
            {
                Trace.Write("XORTransform::OutputBlockSize get\n");
                return this.outputBlockSize;
            }
        }

        /// <summary>
        ///     Creates the encryptor.
        /// </summary>
        /// <param name="rgbKey">The key.</param>
        /// <param name="rgbIV">The iv.</param>
        /// <returns></returns>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            Trace.Write("XOR::CreateEncryptor2\n");
            rgbKey.CopyTo(this.Key, 0);
            rgbIV.CopyTo(this.IV, 0);
            return this;
        }

        /// <summary>
        ///     Creates the decryptor.
        /// </summary>
        /// <param name="rgbKey">The key.</param>
        /// <param name="rgbIV">The iv.</param>
        /// <returns></returns>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            Trace.Write("XOR::CreateDecryptor2\n");
            rgbKey.CopyTo(this.Key, 0);
            rgbIV.CopyTo(this.IV, 0);
            return this;
        }

        /// <summary>
        ///     Generates the IV.
        /// </summary>
        public override void GenerateIV()
        {
            Trace.Write("XOR::GenerateIV\n"); // we are delaing with bytes not bits so we need to /8
            this.IV = new byte[16] {0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf};
        }

        /// <summary>
        ///     Generates the key.
        /// </summary>
        public override void GenerateKey()
        {
            Trace.Write("XOR::GenerateKey\n"); // we are delaing with bytes not bits so we need to /8
            this.Key = new byte[16] {0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf};
        }
    }
}
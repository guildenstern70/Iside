namespace LLCryptoLib.Hash
{
    /// <summary>Computes the Jenkins Hash for the input data using the managed library.</summary>
    public class JHash : BlockHashAlgorithm
    {
        private readonly uint[] accumulator;
        private uint length;


        /// <summary>Initializes a new instance of the JHash class.</summary>
        public JHash() : base(12)
        {
            lock (this)
            {
                this.HashSizeValue = 32;
                this.accumulator = new uint[3];
                this.Initialize();
            }
        }


        /// <summary>Initializes an implementation of System.Security.Cryptography.HashAlgorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.accumulator[0] = 0x9E3779B9;
                this.accumulator[1] = 0x9E3779B9;
                this.accumulator[2] = 0;
                this.length = 0;
                base.Initialize();
            }
        }


        /// <summary>Process a block of data.</summary>
        /// <param name="inputBuffer">The block of data to process.</param>
        /// <param name="inputOffset">Where to start in the block.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+9")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+8")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+7")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+6")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+5")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+4")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+3")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+2")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+11")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+10")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+1")]
        protected override void ProcessBlock(byte[] inputBuffer, int inputOffset)
        {
            lock (this)
            {
                this.accumulator[0] += inputBuffer[inputOffset + 0] + ((uint) inputBuffer[inputOffset + 1] << 8) +
                                       ((uint) inputBuffer[inputOffset + 2] << 16) +
                                       ((uint) inputBuffer[inputOffset + 3] << 24);
                this.accumulator[1] += inputBuffer[inputOffset + 4] + ((uint) inputBuffer[inputOffset + 5] << 8) +
                                       ((uint) inputBuffer[inputOffset + 6] << 16) +
                                       ((uint) inputBuffer[inputOffset + 7] << 24);
                this.accumulator[2] += inputBuffer[inputOffset + 8] + ((uint) inputBuffer[inputOffset + 9] << 8) +
                                       ((uint) inputBuffer[inputOffset + 10] << 16) +
                                       ((uint) inputBuffer[inputOffset + 11] << 24);
                this.length += 12;

                // Mix it up.
                this.accumulator[0] -= this.accumulator[1];
                this.accumulator[0] -= this.accumulator[2];
                this.accumulator[0] ^= this.accumulator[2] >> 13;
                this.accumulator[1] -= this.accumulator[2];
                this.accumulator[1] -= this.accumulator[0];
                this.accumulator[1] ^= this.accumulator[0] << 8;
                this.accumulator[2] -= this.accumulator[0];
                this.accumulator[2] -= this.accumulator[1];
                this.accumulator[2] ^= this.accumulator[1] >> 13;
                this.accumulator[0] -= this.accumulator[1];
                this.accumulator[0] -= this.accumulator[2];
                this.accumulator[0] ^= this.accumulator[2] >> 12;
                this.accumulator[1] -= this.accumulator[2];
                this.accumulator[1] -= this.accumulator[0];
                this.accumulator[1] ^= this.accumulator[0] << 16;
                this.accumulator[2] -= this.accumulator[0];
                this.accumulator[2] -= this.accumulator[1];
                this.accumulator[2] ^= this.accumulator[1] >> 5;
                this.accumulator[0] -= this.accumulator[1];
                this.accumulator[0] -= this.accumulator[2];
                this.accumulator[0] ^= this.accumulator[2] >> 3;
                this.accumulator[1] -= this.accumulator[2];
                this.accumulator[1] -= this.accumulator[0];
                this.accumulator[1] ^= this.accumulator[0] << 10;
                this.accumulator[2] -= this.accumulator[0];
                this.accumulator[2] -= this.accumulator[1];
                this.accumulator[2] ^= this.accumulator[1] >> 15;
            }
        }


        /// <summary>Process the last block of data.</summary>
        /// <param name="inputBuffer">The block of data to process.</param>
        /// <param name="inputOffset">Where to start in the block.</param>
        /// <param name="inputCount">How many bytes need to be processed.</param>
        /// <returns>The hash code as an array of bytes</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+9")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+8")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+7")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+6")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+5")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+4")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+3")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+2")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+10")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
             MessageId = "inputOffset+1")]
        protected override byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            lock (this)
            {
                this.accumulator[2] += this.length + (uint) inputCount;

                switch (inputCount)
                {
                    case 11:
                        this.accumulator[2] += (uint) inputBuffer[inputOffset + 10] << 24;
                        goto case 10;
                    case 10:
                        this.accumulator[2] += (uint) inputBuffer[inputOffset + 9] << 16;
                        goto case 9;
                    case 9:
                        this.accumulator[2] += (uint) inputBuffer[inputOffset + 8] << 8;
                        goto case 8;
                    case 8:
                        this.accumulator[1] += (uint) inputBuffer[inputOffset + 7] << 24;
                        goto case 7;
                    case 7:
                        this.accumulator[1] += (uint) inputBuffer[inputOffset + 6] << 16;
                        goto case 6;
                    case 6:
                        this.accumulator[1] += (uint) inputBuffer[inputOffset + 5] << 8;
                        goto case 5;
                    case 5:
                        this.accumulator[1] += inputBuffer[inputOffset + 4];
                        goto case 4;
                    case 4:
                        this.accumulator[0] += (uint) inputBuffer[inputOffset + 3] << 24;
                        goto case 3;
                    case 3:
                        this.accumulator[0] += (uint) inputBuffer[inputOffset + 2] << 16;
                        goto case 2;
                    case 2:
                        this.accumulator[0] += (uint) inputBuffer[inputOffset + 1] << 8;
                        goto case 1;
                    case 1:
                        this.accumulator[0] += inputBuffer[inputOffset + 0];
                        break;
                }

                // Mix it up.
                this.accumulator[0] -= this.accumulator[1];
                this.accumulator[0] -= this.accumulator[2];
                this.accumulator[0] ^= this.accumulator[2] >> 13;
                this.accumulator[1] -= this.accumulator[2];
                this.accumulator[1] -= this.accumulator[0];
                this.accumulator[1] ^= this.accumulator[0] << 8;
                this.accumulator[2] -= this.accumulator[0];
                this.accumulator[2] -= this.accumulator[1];
                this.accumulator[2] ^= this.accumulator[1] >> 13;
                this.accumulator[0] -= this.accumulator[1];
                this.accumulator[0] -= this.accumulator[2];
                this.accumulator[0] ^= this.accumulator[2] >> 12;
                this.accumulator[1] -= this.accumulator[2];
                this.accumulator[1] -= this.accumulator[0];
                this.accumulator[1] ^= this.accumulator[0] << 16;
                this.accumulator[2] -= this.accumulator[0];
                this.accumulator[2] -= this.accumulator[1];
                this.accumulator[2] ^= this.accumulator[1] >> 5;
                this.accumulator[0] -= this.accumulator[1];
                this.accumulator[0] -= this.accumulator[2];
                this.accumulator[0] ^= this.accumulator[2] >> 3;
                this.accumulator[1] -= this.accumulator[2];
                this.accumulator[1] -= this.accumulator[0];
                this.accumulator[1] ^= this.accumulator[0] << 10;
                this.accumulator[2] -= this.accumulator[0];
                this.accumulator[2] -= this.accumulator[1];
                this.accumulator[2] ^= this.accumulator[1] >> 15;

                return Utilities.UIntToByte(this.accumulator[2], EndianType.BigEndian);
            }
        }
    }
}
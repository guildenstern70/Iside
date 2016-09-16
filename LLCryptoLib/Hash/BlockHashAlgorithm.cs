using System;

namespace LLCryptoLib.Hash
{
    /// <summary>
    ///     Represents the abstract class from which all implementations of the Classless.Hasher.BlockHashAlgorithm
    ///     inherit.
    /// </summary>
    public abstract class BlockHashAlgorithm : System.Security.Cryptography.HashAlgorithm
    {
        private byte[] buffer;


        /// <summary>Initializes a new instance of the BlockHashAlgorithm class.</summary>
        /// <param name="blockSize">The size in bytes of an individual block.</param>
        protected BlockHashAlgorithm(int blockSize)
        {
            this.BlockSize = blockSize;
        }


        /// <summary>The size in bytes of an individual block.</summary>
        public int BlockSize { get; }

        /// <summary>The number of bytes currently in the buffer waiting to be processed.</summary>
        public int BufferCount { get; private set; }

        /// <summary>The number of bytes that have been processed.</summary>
        /// <remarks>This number does NOT include the bytes that are waiting in the buffer.</remarks>
        public long Count { get; private set; }

        /// <summary>Initializes the algorithm.</summary>
        /// <remarks>
        ///     If this function is overriden in a derived class, the new function should call back to
        ///     this function or you could risk garbage being carried over from one calculation to the next.
        /// </remarks>
        public override void Initialize()
        {
            lock (this)
            {
                this.Count = 0;
                this.BufferCount = 0;
                this.State = 0;
                this.buffer = new byte[this.BlockSize];
            }
        }

        /// <summary>Performs the hash algorithm on the data provided.</summary>
        /// <param name="array">The array containing the data.</param>
        /// <param name="ibStart">The position in the array to begin reading from.</param>
        /// <param name="cbSize">How many bytes in the array to read.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            lock (this)
            {
                int i;

                // Use what may already be in the buffer.
                if (this.BufferCount > 0)
                {
                    if (cbSize < this.BlockSize - this.BufferCount)
                    {
                        // Still don't have enough for a full block, just store it.
                        Array.Copy(array, ibStart, this.buffer, this.BufferCount, cbSize);
                        this.BufferCount += cbSize;
                        return;
                    }
                    // Fill out the buffer to make a full block, and then process it.
                    i = this.BlockSize - this.BufferCount;
                    Array.Copy(array, ibStart, this.buffer, this.BufferCount, i);
                    this.ProcessBlock(this.buffer, 0);
                    this.Count += this.BlockSize;
                    this.BufferCount = 0;
                    ibStart += i;
                    cbSize -= i;
                }

                // For as long as we have full blocks, process them.
                for (i = 0; i < cbSize - cbSize%this.BlockSize; i += this.BlockSize)
                {
                    this.ProcessBlock(array, ibStart + i);
                    this.Count += this.BlockSize;
                }

                // If we still have some bytes left, store them for later.
                int bytesLeft = cbSize%this.BlockSize;
                if (bytesLeft != 0)
                {
                    Array.Copy(array, cbSize - bytesLeft + ibStart, this.buffer, 0, bytesLeft);
                    this.BufferCount = bytesLeft;
                }
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        protected override byte[] HashFinal()
        {
            lock (this)
            {
                return this.ProcessFinalBlock(this.buffer, 0, this.BufferCount);
            }
        }


        /// <summary>Process a block of data.</summary>
        /// <param name="inputBuffer">The block of data to process.</param>
        /// <param name="inputOffset">Where to start in the block.</param>
        protected abstract void ProcessBlock(byte[] inputBuffer, int inputOffset);


        /// <summary>Process the last block of data.</summary>
        /// <param name="inputBuffer">The block of data to process.</param>
        /// <param name="inputOffset">Where to start in the block.</param>
        /// <param name="inputCount">How many bytes need to be processed.</param>
        /// <returns>The hash code in bytes</returns>
        protected abstract byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);
    }
}
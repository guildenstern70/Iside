using System;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes the MD4 hash for the input data using the managed library.</summary>
    public class MD4 : BlockHashAlgorithm
    {
        internal uint[] accumulator;


        /// <summary>Initializes a new instance of the MD4 class.</summary>
        public MD4() : base(64)
        {
            lock (this)
            {
                this.HashSizeValue = 128;
                this.accumulator = new uint[4];
                this.Initialize();
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.accumulator[0] = 0x67452301;
                this.accumulator[1] = 0xEFCDAB89;
                this.accumulator[2] = 0x98BADCFE;
                this.accumulator[3] = 0x10325476;
                base.Initialize();
            }
        }


        /// <summary>Process a block of data.</summary>
        /// <param name="inputBuffer">The block of data to process.</param>
        /// <param name="inputOffset">Where to start in the block.</param>
        protected override void ProcessBlock(byte[] inputBuffer, int inputOffset)
        {
            lock (this)
            {
                uint[] workBuffer;
                uint A = this.accumulator[0];
                uint B = this.accumulator[1];
                uint C = this.accumulator[2];
                uint D = this.accumulator[3];

                workBuffer = Utilities.ByteToUInt(inputBuffer, inputOffset, this.BlockSize);

                #region Round 1

                A = this.FF(A, B, C, D, workBuffer[0], 3);
                D = this.FF(D, A, B, C, workBuffer[1], 7);
                C = this.FF(C, D, A, B, workBuffer[2], 11);
                B = this.FF(B, C, D, A, workBuffer[3], 19);
                A = this.FF(A, B, C, D, workBuffer[4], 3);
                D = this.FF(D, A, B, C, workBuffer[5], 7);
                C = this.FF(C, D, A, B, workBuffer[6], 11);
                B = this.FF(B, C, D, A, workBuffer[7], 19);
                A = this.FF(A, B, C, D, workBuffer[8], 3);
                D = this.FF(D, A, B, C, workBuffer[9], 7);
                C = this.FF(C, D, A, B, workBuffer[10], 11);
                B = this.FF(B, C, D, A, workBuffer[11], 19);
                A = this.FF(A, B, C, D, workBuffer[12], 3);
                D = this.FF(D, A, B, C, workBuffer[13], 7);
                C = this.FF(C, D, A, B, workBuffer[14], 11);
                B = this.FF(B, C, D, A, workBuffer[15], 19);

                #endregion

                #region Round 2

                A = this.GG(A, B, C, D, workBuffer[0], 3);
                D = this.GG(D, A, B, C, workBuffer[4], 5);
                C = this.GG(C, D, A, B, workBuffer[8], 9);
                B = this.GG(B, C, D, A, workBuffer[12], 13);
                A = this.GG(A, B, C, D, workBuffer[1], 3);
                D = this.GG(D, A, B, C, workBuffer[5], 5);
                C = this.GG(C, D, A, B, workBuffer[9], 9);
                B = this.GG(B, C, D, A, workBuffer[13], 13);
                A = this.GG(A, B, C, D, workBuffer[2], 3);
                D = this.GG(D, A, B, C, workBuffer[6], 5);
                C = this.GG(C, D, A, B, workBuffer[10], 9);
                B = this.GG(B, C, D, A, workBuffer[14], 13);
                A = this.GG(A, B, C, D, workBuffer[3], 3);
                D = this.GG(D, A, B, C, workBuffer[7], 5);
                C = this.GG(C, D, A, B, workBuffer[11], 9);
                B = this.GG(B, C, D, A, workBuffer[15], 13);

                #endregion

                #region Round 3

                A = this.HH(A, B, C, D, workBuffer[0], 3);
                D = this.HH(D, A, B, C, workBuffer[8], 9);
                C = this.HH(C, D, A, B, workBuffer[4], 11);
                B = this.HH(B, C, D, A, workBuffer[12], 15);
                A = this.HH(A, B, C, D, workBuffer[2], 3);
                D = this.HH(D, A, B, C, workBuffer[10], 9);
                C = this.HH(C, D, A, B, workBuffer[6], 11);
                B = this.HH(B, C, D, A, workBuffer[14], 15);
                A = this.HH(A, B, C, D, workBuffer[1], 3);
                D = this.HH(D, A, B, C, workBuffer[9], 9);
                C = this.HH(C, D, A, B, workBuffer[5], 11);
                B = this.HH(B, C, D, A, workBuffer[13], 15);
                A = this.HH(A, B, C, D, workBuffer[3], 3);
                D = this.HH(D, A, B, C, workBuffer[11], 9);
                C = this.HH(C, D, A, B, workBuffer[7], 11);
                B = this.HH(B, C, D, A, workBuffer[15], 15);

                #endregion

                this.accumulator[0] += A;
                this.accumulator[1] += B;
                this.accumulator[2] += C;
                this.accumulator[3] += D;
            }
        }


        /// <summary>Process the last block of data.</summary>
        /// <param name="inputBuffer">The block of data to process.</param>
        /// <param name="inputOffset">Where to start in the block.</param>
        /// <param name="inputCount">How many bytes should be processed.</param>
        /// <returns>The hash code as an array of bytes</returns>
        protected override byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            lock (this)
            {
                byte[] temp;
                int paddingSize;
                ulong size;

                // Figure out how much padding is needed between the last byte and the size.
                paddingSize = (int) (((ulong) inputCount + (ulong) this.Count)%(ulong) this.BlockSize);
                paddingSize = this.BlockSize - 8 - paddingSize;
                if (paddingSize < 1)
                {
                    paddingSize += this.BlockSize;
                }

                // Create the final, padded block(s).
                temp = new byte[inputCount + paddingSize + 8];
                Array.Copy(inputBuffer, inputOffset, temp, 0, inputCount);
                temp[inputCount] = 0x80;
                size = ((ulong) this.Count + (ulong) inputCount)*8;
                Array.Copy(Utilities.ULongToByte(size), 0, temp, inputCount + paddingSize, 8);

                // Push the final block(s) into the calculation.
                this.ProcessBlock(temp, 0);
                if (temp.Length == this.BlockSize*2)
                {
                    this.ProcessBlock(temp, this.BlockSize);
                }

                return Utilities.UIntToByte(this.accumulator);
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint FF(uint a, uint b, uint c, uint d, uint x, int s)
        {
            uint t = a + ((b & c) | (~b & d)) + x;
            return Utilities.RotateLeft(t, s);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint GG(uint a, uint b, uint c, uint d, uint x, int s)
        {
            uint t = a + ((b & c) | (b & d) | (c & d)) + x + 0x5A827999;
            return Utilities.RotateLeft(t, s);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint HH(uint a, uint b, uint c, uint d, uint x, int s)
        {
            uint t = a + (b ^ c ^ d) + x + 0x6ED9EBA1;
            return Utilities.RotateLeft(t, s);
        }
    }
}
using System;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes the GOSTHash hash for the input data using the managed library.</summary>
    public class GOSTHash : BlockHashAlgorithm
    {
        private static readonly uint[] SBox1;
        private static readonly uint[] SBox2;
        private static readonly uint[] SBox3;
        private static readonly uint[] SBox4;
        private uint[] accumulator;
        private uint[] sum;

        /// <summary>Initializes the substitution boxes.</summary>
        static GOSTHash()
        {
            int a, b, i;
            uint ax, bx, cx, dx;

            SBox1 = new uint[256];
            SBox2 = new uint[256];
            SBox3 = new uint[256];
            SBox4 = new uint[256];

            uint[,] sbox = new uint[8, 16]
            {
                {4, 10, 9, 2, 13, 8, 0, 14, 6, 11, 1, 12, 7, 15, 5, 3},
                {14, 11, 4, 12, 6, 13, 15, 10, 2, 3, 8, 1, 0, 7, 5, 9},
                {5, 8, 1, 13, 10, 3, 4, 2, 14, 15, 12, 7, 6, 0, 9, 11},
                {7, 13, 10, 1, 0, 8, 9, 15, 14, 4, 6, 12, 11, 2, 5, 3},
                {6, 12, 7, 1, 5, 15, 13, 8, 4, 10, 9, 14, 0, 3, 11, 2},
                {4, 11, 10, 0, 7, 2, 1, 13, 3, 6, 8, 5, 9, 12, 15, 14},
                {13, 11, 4, 1, 3, 15, 5, 9, 0, 10, 14, 7, 6, 8, 2, 12},
                {1, 15, 13, 0, 5, 7, 10, 4, 9, 2, 3, 14, 6, 11, 8, 12}
            };

            for (a = 0, i = 0; a < 16; a++)
            {
                ax = sbox[1, a] << 15;
                bx = sbox[3, a] << 23;
                cx = sbox[5, a];
                cx = (cx >> 1) | (cx << 31);
                dx = sbox[7, a] << 7;

                for (b = 0; b < 16; b++)
                {
                    SBox1[i] = ax | (sbox[0, b] << 11);
                    SBox2[i] = bx | (sbox[2, b] << 19);
                    SBox3[i] = cx | (sbox[4, b] << 27);
                    SBox4[i++] = dx | (sbox[6, b] << 3);
                }
            }
        }


        /// <summary>Initializes a new instance of the GOSTHash class.</summary>
        public GOSTHash()
            : base(32)
        {
            lock (this)
            {
                this.HashSizeValue = 256;
                this.Initialize();
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.accumulator = new uint[8];
                this.sum = new uint[8];
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
                uint[] temp;
                bool c = false;

                temp = Utilities.ByteToUInt(inputBuffer, inputOffset, this.BlockSize);

                for (int i = 0; i < 8; i++)
                {
                    if (c)
                    {
                        this.sum[i] += temp[i] + 1;
                        c = this.sum[i] <= temp[i];
                    }
                    else
                    {
                        this.sum[i] += temp[i];
                        c = this.sum[i] < temp[i];
                    }
                }

                this.Compress(temp);
            }
        }

        /// <summary>Process the last block of data.</summary>
        /// <param name="inputBuffer">The block of data to process.</param>
        /// <param name="inputOffset">Where to start in the block.</param>
        /// <param name="inputCount">How many bytes need to be processed.</param>
        /// <returns>The hash code as an array of bytes</returns>
        protected override byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            lock (this)
            {
                int i;
                ulong size;
                uint[] sizeArray = new uint[8];

                // Process remaining bytes.
                if (inputCount > 0)
                {
                    // Pad the unused bytes with 0.
                    for (i = inputOffset + inputCount; i < inputBuffer.Length; i++)
                    {
                        inputBuffer[i] = 0;
                    }

                    this.ProcessBlock(inputBuffer, inputOffset);
                }

                // Add on the length and the sum.
                size = ((ulong) this.Count + (ulong) inputCount)*8;
                sizeArray[0] = (uint) (size & 0xFFFFFFFF);
                sizeArray[1] = (uint) ((size >> 32) & 0xFFFFFFFF);
                this.Compress(sizeArray);
                this.Compress(this.sum);

                return Utilities.UIntToByte(this.accumulator);
            }
        }


        private void Compress(uint[] m)
        {
            uint l, r, t;
            uint[] key = new uint[8];
            uint[] u = new uint[8];
            uint[] v = new uint[8];
            uint[] w = new uint[8];
            uint[] s = new uint[8];

            Array.Copy(this.accumulator, 0, u, 0, 8);
            Array.Copy(m, 0, v, 0, 8);

            for (int i = 0; i < 8; i += 2)
            {
                w[0] = u[0] ^ v[0];
                w[1] = u[1] ^ v[1];
                w[2] = u[2] ^ v[2];
                w[3] = u[3] ^ v[3];
                w[4] = u[4] ^ v[4];
                w[5] = u[5] ^ v[5];
                w[6] = u[6] ^ v[6];
                w[7] = u[7] ^ v[7];

                key[0] = (w[0] & 0x000000FF) | ((w[2] & 0x000000FF) << 8) | ((w[4] & 0x000000FF) << 16) |
                         ((w[6] & 0x000000FF) << 24);
                key[1] = ((w[0] & 0x0000FF00) >> 8) | (w[2] & 0x0000FF00) | ((w[4] & 0x0000FF00) << 8) |
                         ((w[6] & 0x0000FF00) << 16);
                key[2] = ((w[0] & 0x00FF0000) >> 16) | ((w[2] & 0x00FF0000) >> 8) | (w[4] & 0x00FF0000) |
                         ((w[6] & 0x00FF0000) << 8);
                key[3] = ((w[0] & 0xFF000000) >> 24) | ((w[2] & 0xFF000000) >> 16) | ((w[4] & 0xFF000000) >> 8) |
                         (w[6] & 0xFF000000);
                key[4] = (w[1] & 0x000000FF) | ((w[3] & 0x000000FF) << 8) | ((w[5] & 0x000000FF) << 16) |
                         ((w[7] & 0x000000FF) << 24);
                key[5] = ((w[1] & 0x0000FF00) >> 8) | (w[3] & 0x0000FF00) | ((w[5] & 0x0000FF00) << 8) |
                         ((w[7] & 0x0000FF00) << 16);
                key[6] = ((w[1] & 0x00FF0000) >> 16) | ((w[3] & 0x00FF0000) >> 8) | (w[5] & 0x00FF0000) |
                         ((w[7] & 0x00FF0000) << 8);
                key[7] = ((w[1] & 0xFF000000) >> 24) | ((w[3] & 0xFF000000) >> 16) | ((w[5] & 0xFF000000) >> 8) |
                         (w[7] & 0xFF000000);

                r = this.accumulator[i];
                l = this.accumulator[i + 1];

                t = key[0] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[1] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[2] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[3] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[4] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[5] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[6] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[7] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[0] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[1] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[2] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[3] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[4] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[5] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[6] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[7] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[0] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[1] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[2] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[3] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[4] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[5] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[6] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[7] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[7] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[6] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[5] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[4] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[3] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[2] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = key[1] + r;
                l ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];
                t = key[0] + l;
                r ^= SBox1[t & 0xFF] ^ SBox2[(t >> 8) & 0xFF] ^ SBox3[(t >> 16) & 0xFF] ^ SBox4[t >> 24];

                t = r;
                r = l;
                l = t;
                s[i] = r;
                s[i + 1] = l;

                if (i == 6)
                {
                    break;
                }

                l = u[0] ^ u[2];
                r = u[1] ^ u[3];
                u[0] = u[2];
                u[1] = u[3];
                u[2] = u[4];
                u[3] = u[5];
                u[4] = u[6];
                u[5] = u[7];
                u[6] = l;
                u[7] = r;

                if (i == 2)
                {
                    u[0] ^= 0xFF00FF00;
                    u[1] ^= 0xFF00FF00;
                    u[2] ^= 0x00FF00FF;
                    u[3] ^= 0x00FF00FF;
                    u[4] ^= 0x00FFFF00;
                    u[5] ^= 0xFF0000FF;
                    u[6] ^= 0x000000FF;
                    u[7] ^= 0xFF00FFFF;
                }

                l = v[0];
                r = v[2];
                v[0] = v[4];
                v[2] = v[6];
                v[4] = l ^ r;
                v[6] = v[0] ^ r;
                l = v[1];
                r = v[3];
                v[1] = v[5];
                v[3] = v[7];
                v[5] = l ^ r;
                v[7] = v[1] ^ r;
            }

            u[0] = m[0] ^ s[6];
            u[1] = m[1] ^ s[7];
            u[2] = m[2] ^ (s[0] << 16) ^ (s[0] >> 16) ^ (s[0] & 0xFFFF) ^
                   (s[1] & 0xFFFF) ^ (s[1] >> 16) ^ (s[2] << 16) ^ s[6] ^ (s[6] << 16) ^
                   (s[7] & 0xFFFF0000) ^ (s[7] >> 16);
            u[3] = m[3] ^ (s[0] & 0xFFFF) ^ (s[0] << 16) ^ (s[1] & 0xFFFF) ^
                   (s[1] << 16) ^ (s[1] >> 16) ^ (s[2] << 16) ^ (s[2] >> 16) ^
                   (s[3] << 16) ^ s[6] ^ (s[6] << 16) ^ (s[6] >> 16) ^ (s[7] & 0xFFFF) ^
                   (s[7] << 16) ^ (s[7] >> 16);
            u[4] = m[4] ^
                   (s[0] & 0xFFFF0000) ^ (s[0] << 16) ^ (s[0] >> 16) ^
                   (s[1] & 0xFFFF0000) ^ (s[1] >> 16) ^ (s[2] << 16) ^ (s[2] >> 16) ^
                   (s[3] << 16) ^ (s[3] >> 16) ^ (s[4] << 16) ^ (s[6] << 16) ^
                   (s[6] >> 16) ^ (s[7] & 0xFFFF) ^ (s[7] << 16) ^ (s[7] >> 16);
            u[5] = m[5] ^ (s[0] << 16) ^ (s[0] >> 16) ^ (s[0] & 0xFFFF0000) ^
                   (s[1] & 0xFFFF) ^ s[2] ^ (s[2] >> 16) ^ (s[3] << 16) ^ (s[3] >> 16) ^
                   (s[4] << 16) ^ (s[4] >> 16) ^ (s[5] << 16) ^ (s[6] << 16) ^
                   (s[6] >> 16) ^ (s[7] & 0xFFFF0000) ^ (s[7] << 16) ^ (s[7] >> 16);
            u[6] = m[6] ^ s[0] ^ (s[1] >> 16) ^ (s[2] << 16) ^ s[3] ^ (s[3] >> 16) ^
                   (s[4] << 16) ^ (s[4] >> 16) ^ (s[5] << 16) ^ (s[5] >> 16) ^ s[6] ^
                   (s[6] << 16) ^ (s[6] >> 16) ^ (s[7] << 16);
            u[7] = m[7] ^ (s[0] & 0xFFFF0000) ^ (s[0] << 16) ^ (s[1] & 0xFFFF) ^
                   (s[1] << 16) ^ (s[2] >> 16) ^ (s[3] << 16) ^ s[4] ^ (s[4] >> 16) ^
                   (s[5] << 16) ^ (s[5] >> 16) ^ (s[6] >> 16) ^ (s[7] & 0xFFFF) ^
                   (s[7] << 16) ^ (s[7] >> 16);

            v[0] = this.accumulator[0] ^ (u[1] << 16) ^ (u[0] >> 16);
            v[1] = this.accumulator[1] ^ (u[2] << 16) ^ (u[1] >> 16);
            v[2] = this.accumulator[2] ^ (u[3] << 16) ^ (u[2] >> 16);
            v[3] = this.accumulator[3] ^ (u[4] << 16) ^ (u[3] >> 16);
            v[4] = this.accumulator[4] ^ (u[5] << 16) ^ (u[4] >> 16);
            v[5] = this.accumulator[5] ^ (u[6] << 16) ^ (u[5] >> 16);
            v[6] = this.accumulator[6] ^ (u[7] << 16) ^ (u[6] >> 16);
            v[7] = this.accumulator[7] ^ (u[0] & 0xFFFF0000) ^ (u[0] << 16) ^ (u[7] >> 16) ^
                   (u[1] & 0xFFFF0000) ^ (u[1] << 16) ^ (u[6] << 16) ^ (u[7] & 0xFFFF0000);

            this.accumulator[0] = (v[0] & 0xFFFF0000) ^ (v[0] << 16) ^ (v[0] >> 16) ^ (v[1] >> 16) ^
                                  (v[1] & 0xFFFF0000) ^ (v[2] << 16) ^ (v[3] >> 16) ^ (v[4] << 16) ^
                                  (v[5] >> 16) ^ v[5] ^ (v[6] >> 16) ^ (v[7] << 16) ^ (v[7] >> 16) ^
                                  (v[7] & 0xFFFF);
            this.accumulator[1] = (v[0] << 16) ^ (v[0] >> 16) ^ (v[0] & 0xFFFF0000) ^ (v[1] & 0xFFFF) ^
                                  v[2] ^ (v[2] >> 16) ^ (v[3] << 16) ^ (v[4] >> 16) ^ (v[5] << 16) ^
                                  (v[6] << 16) ^ v[6] ^ (v[7] & 0xFFFF0000) ^ (v[7] >> 16);
            this.accumulator[2] = (v[0] & 0xFFFF) ^ (v[0] << 16) ^ (v[1] << 16) ^ (v[1] >> 16) ^
                                  (v[1] & 0xFFFF0000) ^ (v[2] << 16) ^ (v[3] >> 16) ^ v[3] ^ (v[4] << 16) ^
                                  (v[5] >> 16) ^ v[6] ^ (v[6] >> 16) ^ (v[7] & 0xFFFF) ^ (v[7] << 16) ^
                                  (v[7] >> 16);
            this.accumulator[3] = (v[0] << 16) ^ (v[0] >> 16) ^ (v[0] & 0xFFFF0000) ^
                                  (v[1] & 0xFFFF0000) ^ (v[1] >> 16) ^ (v[2] << 16) ^ (v[2] >> 16) ^ v[2] ^
                                  (v[3] << 16) ^ (v[4] >> 16) ^ v[4] ^ (v[5] << 16) ^ (v[6] << 16) ^
                                  (v[7] & 0xFFFF) ^ (v[7] >> 16);
            this.accumulator[4] = (v[0] >> 16) ^ (v[1] << 16) ^ v[1] ^ (v[2] >> 16) ^ v[2] ^
                                  (v[3] << 16) ^ (v[3] >> 16) ^ v[3] ^ (v[4] << 16) ^ (v[5] >> 16) ^
                                  v[5] ^ (v[6] << 16) ^ (v[6] >> 16) ^ (v[7] << 16);
            this.accumulator[5] = (v[0] << 16) ^ (v[0] & 0xFFFF0000) ^ (v[1] << 16) ^ (v[1] >> 16) ^
                                  (v[1] & 0xFFFF0000) ^ (v[2] << 16) ^ v[2] ^ (v[3] >> 16) ^ v[3] ^
                                  (v[4] << 16) ^ (v[4] >> 16) ^ v[4] ^ (v[5] << 16) ^ (v[6] << 16) ^
                                  (v[6] >> 16) ^ v[6] ^ (v[7] << 16) ^ (v[7] >> 16) ^ (v[7] & 0xFFFF0000);
            this.accumulator[6] = v[0] ^ v[2] ^ (v[2] >> 16) ^ v[3] ^ (v[3] << 16) ^ v[4] ^
                                  (v[4] >> 16) ^ (v[5] << 16) ^ (v[5] >> 16) ^ v[5] ^ (v[6] << 16) ^
                                  (v[6] >> 16) ^ v[6] ^ (v[7] << 16) ^ v[7];
            this.accumulator[7] = v[0] ^ (v[0] >> 16) ^ (v[1] << 16) ^ (v[1] >> 16) ^ (v[2] << 16) ^
                                  (v[3] >> 16) ^ v[3] ^ (v[4] << 16) ^ v[4] ^ (v[5] >> 16) ^ v[5] ^
                                  (v[6] << 16) ^ (v[6] >> 16) ^ (v[7] << 16) ^ v[7];
        }
    }
}
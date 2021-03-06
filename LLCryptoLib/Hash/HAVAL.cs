using System;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes the HAVAL hash for the input data using the managed library.</summary>
    public class HAVAL : BlockHashAlgorithm
    {
        private readonly uint[] accumulator;
        private readonly HAVALParameters parameters;


        /// <summary>Initializes a new instance of the HAVAL class.</summary>
        /// <param name="param">The parameters to utilize in the HAVAL calculation.</param>
        public HAVAL(HAVALParameters param) : base(128)
        {
            lock (this)
            {
                if (param == null)
                {
                    throw new ArgumentNullException("param", "The HAVALParameters cannot be null.");
                }
                this.HashSizeValue = param.Length;
                this.parameters = param;
                this.accumulator = new uint[8];
                this.Initialize();
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.accumulator[0] = 0x243F6A88;
                this.accumulator[1] = 0x85A308D3;
                this.accumulator[2] = 0x13198A2E;
                this.accumulator[3] = 0x03707344;
                this.accumulator[4] = 0xA4093822;
                this.accumulator[5] = 0x299F31D0;
                this.accumulator[6] = 0x082EFA98;
                this.accumulator[7] = 0xEC4E6C89;
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
                this.Transform(Utilities.ByteToUInt(inputBuffer, inputOffset, this.BlockSize));
            }
        }


        /// <summary>Process the last block of data.</summary>
        /// <param name="inputBuffer">The block of data to process.</param>
        /// <param name="inputOffset">Where to start in the block.</param>
        /// <param name="inputCount">How many bytes need to be processed.</param>
        /// <returns>The hash as an array of bytes</returns>
        protected override byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            lock (this)
            {
                byte[] temp;
                int paddingSize;
                ulong size = (ulong) this.Count + (ulong) inputCount;

                // Figure out how much padding is needed between the last byte and the size.
                paddingSize = (int) (size%(ulong) this.BlockSize);
                paddingSize = this.BlockSize - 10 - paddingSize;
                if (paddingSize < 1)
                {
                    paddingSize += this.BlockSize;
                }

                // Create the final, padded block(s).
                temp = new byte[inputCount + paddingSize + 10];
                Array.Copy(inputBuffer, inputOffset, temp, 0, inputCount);
                temp[inputCount] = 0x01;
                temp[inputCount + paddingSize] =
                    (byte)
                    (((this.parameters.Length & 0x03) << 6) | ((this.parameters.Passes & 0x07) << 3) | (1 & 0x07));
                temp[inputCount + paddingSize + 1] = (byte) ((this.parameters.Length >> 2) & 0xFF);
                size *= 8;
                Array.Copy(Utilities.ULongToByte(size), 0, temp, inputCount + paddingSize + 2, 8);

                // Push the final block(s) into the calculation.
                this.ProcessBlock(temp, 0);
                if (temp.Length == this.BlockSize*2)
                {
                    this.ProcessBlock(temp, this.BlockSize);
                }

                this.FoldHash();
                return Utilities.UIntToByte(this.accumulator, 0, this.parameters.Length/32);
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint F1(uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0)
        {
            return (x1 & (x0 ^ x4)) ^ (x2 & x5) ^ (x3 & x6) ^ x0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint F2(uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0)
        {
            return (x2 & ((x1 & ~x3) ^ (x4 & x5) ^ x6 ^ x0)) ^ (x4 & (x1 ^ x5)) ^ (x3 & x5) ^ x0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint F3(uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0)
        {
            return (x3 & ((x1 & x2) ^ x6 ^ x0)) ^ (x1 & x4) ^ (x2 & x5) ^ x0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint F4(uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0)
        {
            return (x4 & ((x5 & ~x2) ^ (x3 & ~x6) ^ x1 ^ x6 ^ x0)) ^ (x3 & ((x1 & x2) ^ x5 ^ x6)) ^ (x2 & x6) ^ x0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint F5(uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0)
        {
            return (x0 & ((x1 & x2 & x3) ^ ~x5)) ^ (x1 & x4) ^ (x2 & x5) ^ (x3 & x6);
        }

        private uint FF1(uint x7, uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0, uint w)
        {
            uint temp;

            if (this.parameters.Passes == 3)
            {
                temp = this.F1(x1, x0, x3, x5, x6, x2, x4);
            }
            else if (this.parameters.Passes == 4)
            {
                temp = this.F1(x2, x6, x1, x4, x5, x3, x0);
            }
            else
            {
                temp = this.F1(x3, x4, x1, x0, x5, x2, x6);
            }

            return Utilities.RotateRight(temp, 7) + Utilities.RotateRight(x7, 11) + w;
        }

        private uint FF2(uint x7, uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0, uint w, uint c)
        {
            uint temp;

            if (this.parameters.Passes == 3)
            {
                temp = this.F2(x4, x2, x1, x0, x5, x3, x6);
            }
            else if (this.parameters.Passes == 4)
            {
                temp = this.F2(x3, x5, x2, x0, x1, x6, x4);
            }
            else
            {
                temp = this.F2(x6, x2, x1, x0, x3, x4, x5);
            }

            return Utilities.RotateRight(temp, 7) + Utilities.RotateRight(x7, 11) + w + c;
        }

        private uint FF3(uint x7, uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0, uint w, uint c)
        {
            uint temp;

            if (this.parameters.Passes == 3)
            {
                temp = this.F3(x6, x1, x2, x3, x4, x5, x0);
            }
            else if (this.parameters.Passes == 4)
            {
                temp = this.F3(x1, x4, x3, x6, x0, x2, x5);
            }
            else
            {
                temp = this.F3(x2, x6, x0, x4, x3, x1, x5);
            }

            return Utilities.RotateRight(temp, 7) + Utilities.RotateRight(x7, 11) + w + c;
        }

        private uint FF4(uint x7, uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0, uint w, uint c)
        {
            uint temp;

            if (this.parameters.Passes == 4)
            {
                temp = this.F4(x6, x4, x0, x5, x2, x1, x3);
            }
            else
            {
                temp = this.F4(x1, x5, x3, x2, x0, x4, x6);
            }

            return Utilities.RotateRight(temp, 7) + Utilities.RotateRight(x7, 11) + w + c;
        }

        private uint FF5(uint x7, uint x6, uint x5, uint x4, uint x3, uint x2, uint x1, uint x0, uint w, uint c)
        {
            uint temp = this.F5(x2, x5, x0, x6, x4, x3, x1);
            return Utilities.RotateRight(temp, 7) + Utilities.RotateRight(x7, 11) + w + c;
        }


        private void Transform(uint[] inputBuffer)
        {
            uint[] temp = new uint[this.accumulator.Length];
            Array.Copy(this.accumulator, 0, temp, 0, this.accumulator.Length);

            #region Pass 1

            temp[7] = this.FF1(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[0]);
            temp[6] = this.FF1(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[1]);
            temp[5] = this.FF1(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[2]);
            temp[4] = this.FF1(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[3]);
            temp[3] = this.FF1(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[4]);
            temp[2] = this.FF1(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[5]);
            temp[1] = this.FF1(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[6]);
            temp[0] = this.FF1(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[7]);

            temp[7] = this.FF1(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[8]);
            temp[6] = this.FF1(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[9]);
            temp[5] = this.FF1(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[10]);
            temp[4] = this.FF1(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[11]);
            temp[3] = this.FF1(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[12]);
            temp[2] = this.FF1(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[13]);
            temp[1] = this.FF1(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[14]);
            temp[0] = this.FF1(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[15]);

            temp[7] = this.FF1(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[16]);
            temp[6] = this.FF1(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[17]);
            temp[5] = this.FF1(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[18]);
            temp[4] = this.FF1(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[19]);
            temp[3] = this.FF1(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[20]);
            temp[2] = this.FF1(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[21]);
            temp[1] = this.FF1(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[22]);
            temp[0] = this.FF1(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[23]);

            temp[7] = this.FF1(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[24]);
            temp[6] = this.FF1(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[25]);
            temp[5] = this.FF1(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[26]);
            temp[4] = this.FF1(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[27]);
            temp[3] = this.FF1(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[28]);
            temp[2] = this.FF1(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[29]);
            temp[1] = this.FF1(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[30]);
            temp[0] = this.FF1(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[31]);

            #endregion

            #region Pass 2

            temp[7] = this.FF2(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[5],
                0x452821E6);
            temp[6] = this.FF2(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[14],
                0x38D01377);
            temp[5] = this.FF2(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[26],
                0xBE5466CF);
            temp[4] = this.FF2(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[18],
                0x34E90C6C);
            temp[3] = this.FF2(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[11],
                0xC0AC29B7);
            temp[2] = this.FF2(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[28],
                0xC97C50DD);
            temp[1] = this.FF2(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[7],
                0x3F84D5B5);
            temp[0] = this.FF2(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[16],
                0xB5470917);

            temp[7] = this.FF2(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[0],
                0x9216D5D9);
            temp[6] = this.FF2(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[23],
                0x8979FB1B);
            temp[5] = this.FF2(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[20],
                0xD1310BA6);
            temp[4] = this.FF2(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[22],
                0x98DFB5AC);
            temp[3] = this.FF2(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[1],
                0x2FFD72DB);
            temp[2] = this.FF2(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[10],
                0xD01ADFB7);
            temp[1] = this.FF2(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[4],
                0xB8E1AFED);
            temp[0] = this.FF2(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[8],
                0x6A267E96);

            temp[7] = this.FF2(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[30],
                0xBA7C9045);
            temp[6] = this.FF2(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[3],
                0xF12C7F99);
            temp[5] = this.FF2(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[21],
                0x24A19947);
            temp[4] = this.FF2(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[9],
                0xB3916CF7);
            temp[3] = this.FF2(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[17],
                0x0801F2E2);
            temp[2] = this.FF2(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[24],
                0x858EFC16);
            temp[1] = this.FF2(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[29],
                0x636920D8);
            temp[0] = this.FF2(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[6],
                0x71574E69);

            temp[7] = this.FF2(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[19],
                0xA458FEA3);
            temp[6] = this.FF2(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[12],
                0xF4933D7E);
            temp[5] = this.FF2(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[15],
                0x0D95748F);
            temp[4] = this.FF2(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[13],
                0x728EB658);
            temp[3] = this.FF2(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[2],
                0x718BCD58);
            temp[2] = this.FF2(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[25],
                0x82154AEE);
            temp[1] = this.FF2(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[31],
                0x7B54A41D);
            temp[0] = this.FF2(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[27],
                0xC25A59B5);

            #endregion

            #region Pass 3

            temp[7] = this.FF3(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[19],
                0x9C30D539);
            temp[6] = this.FF3(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[9],
                0x2AF26013);
            temp[5] = this.FF3(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[4],
                0xC5D1B023);
            temp[4] = this.FF3(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[20],
                0x286085F0);
            temp[3] = this.FF3(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[28],
                0xCA417918);
            temp[2] = this.FF3(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[17],
                0xB8DB38EF);
            temp[1] = this.FF3(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[8],
                0x8E79DCB0);
            temp[0] = this.FF3(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[22],
                0x603A180E);

            temp[7] = this.FF3(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[29],
                0x6C9E0E8B);
            temp[6] = this.FF3(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[14],
                0xB01E8A3E);
            temp[5] = this.FF3(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[25],
                0xD71577C1);
            temp[4] = this.FF3(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[12],
                0xBD314B27);
            temp[3] = this.FF3(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[24],
                0x78AF2FDA);
            temp[2] = this.FF3(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[30],
                0x55605C60);
            temp[1] = this.FF3(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[16],
                0xE65525F3);
            temp[0] = this.FF3(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[26],
                0xAA55AB94);

            temp[7] = this.FF3(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[31],
                0x57489862);
            temp[6] = this.FF3(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[15],
                0x63E81440);
            temp[5] = this.FF3(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[7],
                0x55CA396A);
            temp[4] = this.FF3(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[3],
                0x2AAB10B6);
            temp[3] = this.FF3(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[1],
                0xB4CC5C34);
            temp[2] = this.FF3(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[0],
                0x1141E8CE);
            temp[1] = this.FF3(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[18],
                0xA15486AF);
            temp[0] = this.FF3(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[27],
                0x7C72E993);

            temp[7] = this.FF3(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], inputBuffer[13],
                0xB3EE1411);
            temp[6] = this.FF3(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], inputBuffer[6],
                0x636FBC2A);
            temp[5] = this.FF3(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], inputBuffer[21],
                0x2BA9C55D);
            temp[4] = this.FF3(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], inputBuffer[10],
                0x741831F6);
            temp[3] = this.FF3(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], inputBuffer[23],
                0xCE5C3E16);
            temp[2] = this.FF3(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], inputBuffer[11],
                0x9B87931E);
            temp[1] = this.FF3(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], inputBuffer[5],
                0xAFD6BA33);
            temp[0] = this.FF3(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], inputBuffer[2],
                0x6C24CF5C);

            #endregion

            #region Pass 4

            if (this.parameters.Passes >= 4)
            {
                temp[7] = this.FF4(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0],
                    inputBuffer[24], 0x7A325381);
                temp[6] = this.FF4(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7],
                    inputBuffer[4], 0x28958677);
                temp[5] = this.FF4(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6],
                    inputBuffer[0], 0x3B8F4898);
                temp[4] = this.FF4(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5],
                    inputBuffer[14], 0x6B4BB9AF);
                temp[3] = this.FF4(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4],
                    inputBuffer[2], 0xC4BFE81B);
                temp[2] = this.FF4(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3],
                    inputBuffer[7], 0x66282193);
                temp[1] = this.FF4(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2],
                    inputBuffer[28], 0x61D809CC);
                temp[0] = this.FF4(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1],
                    inputBuffer[23], 0xFB21A991);

                temp[7] = this.FF4(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0],
                    inputBuffer[26], 0x487CAC60);
                temp[6] = this.FF4(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7],
                    inputBuffer[6], 0x5DEC8032);
                temp[5] = this.FF4(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6],
                    inputBuffer[30], 0xEF845D5D);
                temp[4] = this.FF4(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5],
                    inputBuffer[20], 0xE98575B1);
                temp[3] = this.FF4(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4],
                    inputBuffer[18], 0xDC262302);
                temp[2] = this.FF4(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3],
                    inputBuffer[25], 0xEB651B88);
                temp[1] = this.FF4(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2],
                    inputBuffer[19], 0x23893E81);
                temp[0] = this.FF4(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1],
                    inputBuffer[3], 0xD396ACC5);

                temp[7] = this.FF4(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0],
                    inputBuffer[22], 0x0F6D6FF3);
                temp[6] = this.FF4(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7],
                    inputBuffer[11], 0x83F44239);
                temp[5] = this.FF4(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6],
                    inputBuffer[31], 0x2E0B4482);
                temp[4] = this.FF4(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5],
                    inputBuffer[21], 0xA4842004);
                temp[3] = this.FF4(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4],
                    inputBuffer[8], 0x69C8F04A);
                temp[2] = this.FF4(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3],
                    inputBuffer[27], 0x9E1F9B5E);
                temp[1] = this.FF4(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2],
                    inputBuffer[12], 0x21C66842);
                temp[0] = this.FF4(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1],
                    inputBuffer[9], 0xF6E96C9A);

                temp[7] = this.FF4(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0],
                    inputBuffer[1], 0x670C9C61);
                temp[6] = this.FF4(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7],
                    inputBuffer[29], 0xABD388F0);
                temp[5] = this.FF4(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6],
                    inputBuffer[5], 0x6A51A0D2);
                temp[4] = this.FF4(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5],
                    inputBuffer[15], 0xD8542F68);
                temp[3] = this.FF4(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4],
                    inputBuffer[17], 0x960FA728);
                temp[2] = this.FF4(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3],
                    inputBuffer[10], 0xAB5133A3);
                temp[1] = this.FF4(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2],
                    inputBuffer[16], 0x6EEF0B6C);
                temp[0] = this.FF4(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1],
                    inputBuffer[13], 0x137A3BE4);
            }

            #endregion

            #region Pass 5

            if (this.parameters.Passes == 5)
            {
                temp[7] = this.FF5(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0],
                    inputBuffer[27], 0xBA3BF050);
                temp[6] = this.FF5(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7],
                    inputBuffer[3], 0x7EFB2A98);
                temp[5] = this.FF5(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6],
                    inputBuffer[21], 0xA1F1651D);
                temp[4] = this.FF5(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5],
                    inputBuffer[26], 0x39AF0176);
                temp[3] = this.FF5(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4],
                    inputBuffer[17], 0x66CA593E);
                temp[2] = this.FF5(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3],
                    inputBuffer[11], 0x82430E88);
                temp[1] = this.FF5(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2],
                    inputBuffer[20], 0x8CEE8619);
                temp[0] = this.FF5(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1],
                    inputBuffer[29], 0x456F9FB4);

                temp[7] = this.FF5(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0],
                    inputBuffer[19], 0x7D84A5C3);
                temp[6] = this.FF5(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7],
                    inputBuffer[0], 0x3B8B5EBE);
                temp[5] = this.FF5(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6],
                    inputBuffer[12], 0xE06F75D8);
                temp[4] = this.FF5(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5],
                    inputBuffer[7], 0x85C12073);
                temp[3] = this.FF5(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4],
                    inputBuffer[13], 0x401A449F);
                temp[2] = this.FF5(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3],
                    inputBuffer[8], 0x56C16AA6);
                temp[1] = this.FF5(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2],
                    inputBuffer[31], 0x4ED3AA62);
                temp[0] = this.FF5(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1],
                    inputBuffer[10], 0x363F7706);

                temp[7] = this.FF5(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0],
                    inputBuffer[5], 0x1BFEDF72);
                temp[6] = this.FF5(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7],
                    inputBuffer[9], 0x429B023D);
                temp[5] = this.FF5(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6],
                    inputBuffer[14], 0x37D0D724);
                temp[4] = this.FF5(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5],
                    inputBuffer[30], 0xD00A1248);
                temp[3] = this.FF5(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4],
                    inputBuffer[18], 0xDB0FEAD3);
                temp[2] = this.FF5(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3],
                    inputBuffer[6], 0x49F1C09B);
                temp[1] = this.FF5(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2],
                    inputBuffer[28], 0x075372C9);
                temp[0] = this.FF5(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1],
                    inputBuffer[24], 0x80991B7B);

                temp[7] = this.FF5(temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0],
                    inputBuffer[2], 0x25D479D8);
                temp[6] = this.FF5(temp[6], temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7],
                    inputBuffer[23], 0xF6E8DEF7);
                temp[5] = this.FF5(temp[5], temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6],
                    inputBuffer[16], 0xE3FE501A);
                temp[4] = this.FF5(temp[4], temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5],
                    inputBuffer[22], 0xB6794C3B);
                temp[3] = this.FF5(temp[3], temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4],
                    inputBuffer[4], 0x976CE0BD);
                temp[2] = this.FF5(temp[2], temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3],
                    inputBuffer[1], 0x04C006BA);
                temp[1] = this.FF5(temp[1], temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2],
                    inputBuffer[25], 0xC1A94FB6);
                temp[0] = this.FF5(temp[0], temp[7], temp[6], temp[5], temp[4], temp[3], temp[2], temp[1],
                    inputBuffer[15], 0x409F60C4);
            }

            #endregion

            for (int i = 0; i < this.accumulator.Length; i++)
            {
                this.accumulator[i] += temp[i];
            }
        }


        private void FoldHash()
        {
            uint temp;

            if (this.parameters.Length == 128)
            {
                temp = (this.accumulator[7] & 0x000000FF) | (this.accumulator[6] & 0xFF000000) |
                       (this.accumulator[5] & 0x00FF0000) | (this.accumulator[4] & 0x0000FF00);
                this.accumulator[0] += Utilities.RotateRight(temp, 8);

                temp = (this.accumulator[7] & 0x0000FF00) | (this.accumulator[6] & 0x000000FF) |
                       (this.accumulator[5] & 0xFF000000) | (this.accumulator[4] & 0x00FF0000);
                this.accumulator[1] += Utilities.RotateRight(temp, 16);

                temp = (this.accumulator[7] & 0x00FF0000) | (this.accumulator[6] & 0x0000FF00) |
                       (this.accumulator[5] & 0x000000FF) | (this.accumulator[4] & 0xFF000000);
                this.accumulator[2] += Utilities.RotateRight(temp, 24);

                temp = (this.accumulator[7] & 0xFF000000) | (this.accumulator[6] & 0x00FF0000) |
                       (this.accumulator[5] & 0x0000FF00) | (this.accumulator[4] & 0x000000FF);
                this.accumulator[3] += temp;
            }
            else if (this.parameters.Length == 160)
            {
                temp =
                    (uint)
                    ((this.accumulator[7] & 0x3F) | (this.accumulator[6] & (0x7F << 25)) |
                     (this.accumulator[5] & (0x3F << 19)));
                this.accumulator[0] += Utilities.RotateRight(temp, 19);

                temp =
                    (uint)
                    ((this.accumulator[7] & (0x3F << 6)) | (this.accumulator[6] & 0x3F) |
                     (this.accumulator[5] & (0x7F << 25)));
                this.accumulator[1] += Utilities.RotateRight(temp, 25);

                temp = (this.accumulator[7] & (0x7F << 12)) | (this.accumulator[6] & (0x3F << 6)) |
                       (this.accumulator[5] & 0x3F);
                this.accumulator[2] += temp;

                temp = (this.accumulator[7] & (0x3F << 19)) | (this.accumulator[6] & (0x7F << 12)) |
                       (this.accumulator[5] & (0x3F << 6));
                this.accumulator[3] += temp >> 6;

                temp =
                    (uint)
                    ((this.accumulator[7] & (0x7F << 25)) | (this.accumulator[6] & (0x3F << 19)) |
                     (this.accumulator[5] & (0x7F << 12)));
                this.accumulator[4] += temp >> 12;
            }
            else if (this.parameters.Length == 192)
            {
                temp = (uint) ((this.accumulator[7] & 0x1F) | (this.accumulator[6] & (0x3F << 26)));
                this.accumulator[0] += Utilities.RotateRight(temp, 26);

                temp = (this.accumulator[7] & (0x1F << 5)) | (this.accumulator[6] & 0x1F);
                this.accumulator[1] += temp;

                temp = (this.accumulator[7] & (0x3F << 10)) | (this.accumulator[6] & (0x1F << 5));
                this.accumulator[2] += temp >> 5;

                temp = (this.accumulator[7] & (0x1F << 16)) | (this.accumulator[6] & (0x3F << 10));
                this.accumulator[3] += temp >> 10;

                temp = (this.accumulator[7] & (0x1F << 21)) | (this.accumulator[6] & (0x1F << 16));
                this.accumulator[4] += temp >> 16;

                temp = (uint) ((this.accumulator[7] & (0x3F << 26)) | (this.accumulator[6] & (0x1F << 21)));
                this.accumulator[5] += temp >> 21;
            }
            else if (this.parameters.Length == 224)
            {
                this.accumulator[0] += (this.accumulator[7] >> 27) & 0x1F;
                this.accumulator[1] += (this.accumulator[7] >> 22) & 0x1F;
                this.accumulator[2] += (this.accumulator[7] >> 18) & 0x0F;
                this.accumulator[3] += (this.accumulator[7] >> 13) & 0x1F;
                this.accumulator[4] += (this.accumulator[7] >> 9) & 0x0F;
                this.accumulator[5] += (this.accumulator[7] >> 4) & 0x1F;
                this.accumulator[6] += this.accumulator[7] & 0x0F;
            }
        }
    }
}
using System;
using System.Security.Cryptography;

namespace LLCryptoLib.Hash
{
    /**
	* Implementation of WhirlpoolDigest, based on Java source published by Barreto
	* and Rijmen.
	*
	*/

    public sealed class WhirlpoolDigest
    {
        private const int BYTE_LENGTH = 64;

        private const int DIGEST_LENGTH_BYTES = 512/8;
        private const int ROUNDS = 10;
        private const int REDUCTION_POLYNOMIAL = 0x011d; // 2^8 + 2^4 + 2^3 + 2 + 1;

        // --------------------------------------------------------------------------------------//

        // -- buffer information --
        private const int BITCOUNT_ARRAY_SIZE = 32;

        private static readonly int[] SBOX =
        {
            0x18, 0x23, 0xc6, 0xe8, 0x87, 0xb8, 0x01, 0x4f, 0x36, 0xa6, 0xd2, 0xf5, 0x79, 0x6f, 0x91, 0x52,
            0x60, 0xbc, 0x9b, 0x8e, 0xa3, 0x0c, 0x7b, 0x35, 0x1d, 0xe0, 0xd7, 0xc2, 0x2e, 0x4b, 0xfe, 0x57,
            0x15, 0x77, 0x37, 0xe5, 0x9f, 0xf0, 0x4a, 0xda, 0x58, 0xc9, 0x29, 0x0a, 0xb1, 0xa0, 0x6b, 0x85,
            0xbd, 0x5d, 0x10, 0xf4, 0xcb, 0x3e, 0x05, 0x67, 0xe4, 0x27, 0x41, 0x8b, 0xa7, 0x7d, 0x95, 0xd8,
            0xfb, 0xee, 0x7c, 0x66, 0xdd, 0x17, 0x47, 0x9e, 0xca, 0x2d, 0xbf, 0x07, 0xad, 0x5a, 0x83, 0x33,
            0x63, 0x02, 0xaa, 0x71, 0xc8, 0x19, 0x49, 0xd9, 0xf2, 0xe3, 0x5b, 0x88, 0x9a, 0x26, 0x32, 0xb0,
            0xe9, 0x0f, 0xd5, 0x80, 0xbe, 0xcd, 0x34, 0x48, 0xff, 0x7a, 0x90, 0x5f, 0x20, 0x68, 0x1a, 0xae,
            0xb4, 0x54, 0x93, 0x22, 0x64, 0xf1, 0x73, 0x12, 0x40, 0x08, 0xc3, 0xec, 0xdb, 0xa1, 0x8d, 0x3d,
            0x97, 0x00, 0xcf, 0x2b, 0x76, 0x82, 0xd6, 0x1b, 0xb5, 0xaf, 0x6a, 0x50, 0x45, 0xf3, 0x30, 0xef,
            0x3f, 0x55, 0xa2, 0xea, 0x65, 0xba, 0x2f, 0xc0, 0xde, 0x1c, 0xfd, 0x4d, 0x92, 0x75, 0x06, 0x8a,
            0xb2, 0xe6, 0x0e, 0x1f, 0x62, 0xd4, 0xa8, 0x96, 0xf9, 0xc5, 0x25, 0x59, 0x84, 0x72, 0x39, 0x4c,
            0x5e, 0x78, 0x38, 0x8c, 0xd1, 0xa5, 0xe2, 0x61, 0xb3, 0x21, 0x9c, 0x1e, 0x43, 0xc7, 0xfc, 0x04,
            0x51, 0x99, 0x6d, 0x0d, 0xfa, 0xdf, 0x7e, 0x24, 0x3b, 0xab, 0xce, 0x11, 0x8f, 0x4e, 0xb7, 0xeb,
            0x3c, 0x81, 0x94, 0xf7, 0xb9, 0x13, 0x2c, 0xd3, 0xe7, 0x6e, 0xc4, 0x03, 0x56, 0x44, 0x7f, 0xa9,
            0x2a, 0xbb, 0xc1, 0x53, 0xdc, 0x0b, 0x9d, 0x6c, 0x31, 0x74, 0xf6, 0x46, 0xac, 0x89, 0x14, 0xe1,
            0x16, 0x3a, 0x69, 0x09, 0x70, 0xb6, 0xd0, 0xed, 0xcc, 0x42, 0x98, 0xa4, 0x28, 0x5c, 0xf8, 0x86
        };

        private static readonly long[] C0 = new long[256];
        private static readonly long[] C1 = new long[256];
        private static readonly long[] C2 = new long[256];
        private static readonly long[] C3 = new long[256];
        private static readonly long[] C4 = new long[256];
        private static readonly long[] C5 = new long[256];
        private static readonly long[] C6 = new long[256];
        private static readonly long[] C7 = new long[256];

        /*
            * increment() can be implemented in this way using 2 arrays or
            * by having some temporary variables that are used to set the
            * value provided by EIGHT[i] and carry within the loop.
            *
            * not having done any timing, this seems likely to be faster
            * at the slight expense of 32*(sizeof short) bytes
            */
        private static readonly short[] EIGHT = new short[BITCOUNT_ARRAY_SIZE];

        private readonly long[] _rc = new long[ROUNDS + 1];
        private readonly short[] _bitCount = new short[BITCOUNT_ARRAY_SIZE];
        private readonly long[] _block = new long[8]; // mu (buffer)
        private readonly byte[] _buffer = new byte[64];
        private int _bufferPos;

        // -- internal hash state --
        private readonly long[] _hash = new long[8];
        private readonly long[] _K = new long[8]; // the round key
        private readonly long[] _L = new long[8];
        private readonly long[] _state = new long[8]; // the current "cipher" state

        static WhirlpoolDigest()
        {
            EIGHT[BITCOUNT_ARRAY_SIZE - 1] = 8;

            for (int i = 0; i < 256; i++)
            {
                int v1 = SBOX[i];
                int v2 = maskWithReductionPolynomial(v1 << 1);
                int v4 = maskWithReductionPolynomial(v2 << 1);
                int v5 = v4 ^ v1;
                int v8 = maskWithReductionPolynomial(v4 << 1);
                int v9 = v8 ^ v1;

                C0[i] = packIntoLong(v1, v1, v4, v1, v8, v5, v2, v9);
                C1[i] = packIntoLong(v9, v1, v1, v4, v1, v8, v5, v2);
                C2[i] = packIntoLong(v2, v9, v1, v1, v4, v1, v8, v5);
                C3[i] = packIntoLong(v5, v2, v9, v1, v1, v4, v1, v8);
                C4[i] = packIntoLong(v8, v5, v2, v9, v1, v1, v4, v1);
                C5[i] = packIntoLong(v1, v8, v5, v2, v9, v1, v1, v4);
                C6[i] = packIntoLong(v4, v1, v8, v5, v2, v9, v1, v1);
                C7[i] = packIntoLong(v1, v4, v1, v8, v5, v2, v9, v1);
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WhirlpoolDigest" /> class.
        /// </summary>
        public WhirlpoolDigest()
        {
            this._rc[0] = 0L;
            for (int r = 1; r <= ROUNDS; r++)
            {
                int i = 8*(r - 1);
                this._rc[r] = (long) ((ulong) C0[i] & 0xff00000000000000L) ^
                              (C1[i + 1] & 0x00ff000000000000L) ^
                              (C2[i + 2] & 0x0000ff0000000000L) ^
                              (C3[i + 3] & 0x000000ff00000000L) ^
                              (C4[i + 4] & 0x00000000ff000000L) ^
                              (C5[i + 5] & 0x0000000000ff0000L) ^
                              (C6[i + 6] & 0x000000000000ff00L) ^
                              (C7[i + 7] & 0x00000000000000ffL);
            }
        }


        /**
            * Copy constructor. This will copy the state of the provided message
            * digest.
            */

        public WhirlpoolDigest(WhirlpoolDigest originalDigest)
        {
            Array.Copy(originalDigest._rc, 0, this._rc, 0, this._rc.Length);

            Array.Copy(originalDigest._buffer, 0, this._buffer, 0, this._buffer.Length);

            this._bufferPos = originalDigest._bufferPos;
            Array.Copy(originalDigest._bitCount, 0, this._bitCount, 0, this._bitCount.Length);

            // -- internal hash state --
            Array.Copy(originalDigest._hash, 0, this._hash, 0, this._hash.Length);
            Array.Copy(originalDigest._K, 0, this._K, 0, this._K.Length);
            Array.Copy(originalDigest._L, 0, this._L, 0, this._L.Length);
            Array.Copy(originalDigest._block, 0, this._block, 0, this._block.Length);
            Array.Copy(originalDigest._state, 0, this._state, 0, this._state.Length);
        }

        /// <summary>
        ///     The algorithm name
        /// </summary>
        /// <value></value>
        /// return the algorithm name
        /// @return the algorithm name
        public string AlgorithmName
        {
            get { return "Whirlpool"; }
        }

        private static long packIntoLong(int b7, int b6, int b5, int b4, int b3, int b2, int b1, int b0)
        {
            return
                ((long) b7 << 56) ^
                ((long) b6 << 48) ^
                ((long) b5 << 40) ^
                ((long) b4 << 32) ^
                ((long) b3 << 24) ^
                ((long) b2 << 16) ^
                ((long) b1 << 8) ^
                b0;
        }

        /*
            * int's are used to prevent sign extension.  The values that are really being used are
            * actually just 0..255
            */

        private static int maskWithReductionPolynomial(int input)
        {
            int rv = input;
            if (rv >= 0x100L) // high bit set
            {
                rv ^= REDUCTION_POLYNOMIAL; // reduced by the polynomial
            }
            return rv;
        }

        /// <summary>
        ///     The size, in bytes, of the digest produced by this message digest
        /// </summary>
        /// <returns></returns>
        /// return the size, in bytes, of the digest produced by this message digest.
        /// @return the size, in bytes, of the digest produced by this message digest.
        public int GetDigestSize()
        {
            return DIGEST_LENGTH_BYTES;
        }

        /// <summary>
        ///     Close the digest, producing the final digest value. The doFinal
        ///     call leaves the digest reset.
        /// </summary>
        /// <param name="output">the array the digest is to be copied into.</param>
        /// <param name="outOff">the offset into the out array the digest is to start at.</param>
        /// <returns>The digest size</returns>
        public int DoFinal(byte[] output, int outOff)
        {
            // sets output[outOff] .. output[outOff+DIGEST_LENGTH_BYTES]
            this.finish();

            for (int i = 0; i < 8; i++)
            {
                convertLongToByteArray(this._hash[i], output, outOff + i*8);
            }

            this.Reset();

            return this.GetDigestSize();
        }

        /**
            * Reset the chaining variables
            */

        public void Reset()
        {
            // set variables to null, blank, whatever
            this._bufferPos = 0;
            Array.Clear(this._bitCount, 0, this._bitCount.Length);
            Array.Clear(this._buffer, 0, this._buffer.Length);
            Array.Clear(this._hash, 0, this._hash.Length);
            Array.Clear(this._K, 0, this._K.Length);
            Array.Clear(this._L, 0, this._L.Length);
            Array.Clear(this._block, 0, this._block.Length);
            Array.Clear(this._state, 0, this._state.Length);
        }

        // this takes a buffer of information and fills the block
        private void processFilledBuffer()
        {
            // copies into the block...
            for (int i = 0; i < this._state.Length; i++)
            {
                this._block[i] = bytesToLongFromBuffer(this._buffer, i*8);
            }
            this.processBlock();
            this._bufferPos = 0;
            Array.Clear(this._buffer, 0, this._buffer.Length);
        }

        private static long bytesToLongFromBuffer(byte[] buffer, int startPos)
        {
            long rv = ((buffer[startPos + 0] & 0xffL) << 56) |
                      ((buffer[startPos + 1] & 0xffL) << 48) |
                      ((buffer[startPos + 2] & 0xffL) << 40) |
                      ((buffer[startPos + 3] & 0xffL) << 32) |
                      ((buffer[startPos + 4] & 0xffL) << 24) |
                      ((buffer[startPos + 5] & 0xffL) << 16) |
                      ((buffer[startPos + 6] & 0xffL) << 8) |
                      (buffer[startPos + 7] & 0xffL);

            return rv;
        }

        private static void convertLongToByteArray(long inputLong, byte[] outputArray, int offSet)
        {
            for (int i = 0; i < 8; i++)
            {
                outputArray[offSet + i] = (byte) ((inputLong >> (56 - i*8)) & 0xff);
            }
        }

        private void processBlock()
        {
            // buffer contents have been transferred to the _block[] array via
            // processFilledBuffer

            // compute and apply K^0
            for (int i = 0; i < 8; i++)
            {
                this._state[i] = this._block[i] ^ (this._K[i] = this._hash[i]);
            }

            // iterate over the rounds
            for (int round = 1; round <= ROUNDS; round++)
            {
                for (int i = 0; i < 8; i++)
                {
                    this._L[i] = 0;
                    this._L[i] ^= C0[(int) (this._K[(i - 0) & 7] >> 56) & 0xff];
                    this._L[i] ^= C1[(int) (this._K[(i - 1) & 7] >> 48) & 0xff];
                    this._L[i] ^= C2[(int) (this._K[(i - 2) & 7] >> 40) & 0xff];
                    this._L[i] ^= C3[(int) (this._K[(i - 3) & 7] >> 32) & 0xff];
                    this._L[i] ^= C4[(int) (this._K[(i - 4) & 7] >> 24) & 0xff];
                    this._L[i] ^= C5[(int) (this._K[(i - 5) & 7] >> 16) & 0xff];
                    this._L[i] ^= C6[(int) (this._K[(i - 6) & 7] >> 8) & 0xff];
                    this._L[i] ^= C7[(int) this._K[(i - 7) & 7] & 0xff];
                }

                Array.Copy(this._L, 0, this._K, 0, this._K.Length);

                this._K[0] ^= this._rc[round];

                // apply the round transformation
                for (int i = 0; i < 8; i++)
                {
                    this._L[i] = this._K[i];

                    this._L[i] ^= C0[(int) (this._state[(i - 0) & 7] >> 56) & 0xff];
                    this._L[i] ^= C1[(int) (this._state[(i - 1) & 7] >> 48) & 0xff];
                    this._L[i] ^= C2[(int) (this._state[(i - 2) & 7] >> 40) & 0xff];
                    this._L[i] ^= C3[(int) (this._state[(i - 3) & 7] >> 32) & 0xff];
                    this._L[i] ^= C4[(int) (this._state[(i - 4) & 7] >> 24) & 0xff];
                    this._L[i] ^= C5[(int) (this._state[(i - 5) & 7] >> 16) & 0xff];
                    this._L[i] ^= C6[(int) (this._state[(i - 6) & 7] >> 8) & 0xff];
                    this._L[i] ^= C7[(int) this._state[(i - 7) & 7] & 0xff];
                }

                // save the current state
                Array.Copy(this._L, 0, this._state, 0, this._state.Length);
            }

            // apply Miuaguchi-Preneel compression
            for (int i = 0; i < 8; i++)
            {
                this._hash[i] ^= this._state[i] ^ this._block[i];
            }
        }

        /// <summary>
        ///     Update the message digest with a single byte.
        /// </summary>
        /// <param name="input">the input byte to be entered.</param>
        public void Update(byte input)
        {
            this._buffer[this._bufferPos] = input;

            //Console.WriteLine("adding to buffer = "+_buffer[_bufferPos]);

            ++this._bufferPos;

            if (this._bufferPos == this._buffer.Length)
            {
                this.processFilledBuffer();
            }

            this.increment();
        }

        private void increment()
        {
            int carry = 0;
            for (int i = this._bitCount.Length - 1; i >= 0; i--)
            {
                int sum = (this._bitCount[i] & 0xff) + EIGHT[i] + carry;

                carry = sum >> 8;
                this._bitCount[i] = (short) (sum & 0xff);
            }
        }

        /// <summary>
        ///     Update the message digest with a block of bytes.
        /// </summary>
        /// <param name="input">the byte array containing the data.</param>
        /// <param name="inOff">the offset into the byte array where the data starts.</param>
        /// <param name="length">the length of the data.</param>
        public void BlockUpdate(byte[] input, int inOff, int length)
        {
            while (length > 0)
            {
                this.Update(input[inOff]);
                ++inOff;
                --length;
            }
        }

        private void finish()
        {
            /*
                * this makes a copy of the current bit length. at the expense of an
                * object creation of 32 bytes rather than providing a _stopCounting
                * boolean which was the alternative I could think of.
                */
            byte[] bitLength = this.copyBitLength();

            this._buffer[this._bufferPos++] |= 0x80;

            if (this._bufferPos == this._buffer.Length)
            {
                this.processFilledBuffer();
            }

            /*
                * Final block contains
                * [ ... data .... ][0][0][0][ length ]
                *
                * if [ length ] cannot fit.  Need to create a new block.
                */
            if (this._bufferPos > 32)
            {
                while (this._bufferPos != 0)
                {
                    this.Update(0);
                }
            }

            while (this._bufferPos <= 32)
            {
                this.Update(0);
            }

            // copy the length information to the final 32 bytes of the
            // 64 byte block....
            Array.Copy(bitLength, 0, this._buffer, 32, bitLength.Length);

            this.processFilledBuffer();
        }

        private byte[] copyBitLength()
        {
            byte[] rv = new byte[BITCOUNT_ARRAY_SIZE];
            for (int i = 0; i < rv.Length; i++)
            {
                rv[i] = (byte) (this._bitCount[i] & 0xff);
            }
            return rv;
        }

        /// <summary>
        ///     Return the size, in bytes, of the internal buffer used by this digest.
        /// </summary>
        /// <returns>the size, in bytes, of the internal buffer used by this digest.</returns>
        public int GetByteLength()
        {
            return BYTE_LENGTH;
        }
    }


    /// <summary>Computes the Whirlpool hash for the input data using the managed library.</summary>
    public class Whirlpool : HashAlgorithm
    {
        private readonly byte[] hash;
        private WhirlpoolDigest whirlpoolDigest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Whirlpool" /> class.
        /// </summary>
        public Whirlpool()
        {
            this.whirlpoolDigest = new WhirlpoolDigest();
            this.hash = new byte[this.whirlpoolDigest.GetDigestSize()];
        }

        /// <summary>
        ///     Gets the size, in bits, of the computed hash code.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///     The size, in bits, of the computed hash code.
        /// </returns>
        public override int HashSize
        {
            get { return this.whirlpoolDigest.GetDigestSize()*8; }
        }

        /// <summary>
        ///     Gets the value of the computed hash code.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///     The current value of the computed hash code.
        /// </returns>
        /// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">
        ///     <see cref="F:System.Security.Cryptography.HashAlgorithm.HashValue" /> is null.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The object has already been disposed.
        /// </exception>
        public override byte[] Hash
        {
            get { return this.hash; }
        }

        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            this.whirlpoolDigest = new WhirlpoolDigest();
        }

        /// <summary>
        ///     When overridden in a derived class, routes data written to the object into the hash algorithm for computing the
        ///     hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            this.whirlpoolDigest.BlockUpdate(array, ibStart, cbSize);
        }

        /// <summary>
        ///     When overridden in a derived class, finalizes the hash computation after the last data is processed by the
        ///     cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            this.whirlpoolDigest.DoFinal(this.hash, 0);
            return this.hash;
        }
    }
}
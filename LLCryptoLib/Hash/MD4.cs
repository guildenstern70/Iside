/*
 * LLCryptoLib - Advanced .NET Encryption and Hashing Library
 * v.$id$
 * 
 * The contents of this file are subject to the license distributed with
 * the package (the License). This file cannot be distributed without the 
 * original LittleLite Software license file. The distribution of this
 * file is subject to the agreement between the licensee and LittleLite
 * Software.
 * 
 * Customer that has purchased Source Code License may alter this
 * file and distribute the modified binary redistributables with applications. 
 * Except as expressly authorized in the License, customer shall not rent,
 * lease, distribute, sell, make available for download of this file. 
 * 
 * This software is not Open Source, nor Free. Its usage must adhere
 * with the License obtained from LittleLite Software.
 * 
 * The source code in this file may be derived, all or in part, from existing
 * other source code, where the original license permit to do so.
 * 
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */


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
				HashSizeValue = 128;
				accumulator = new uint[4];
				Initialize();
			}
		}


		/// <summary>Initializes the algorithm.</summary>
		override public void Initialize() 
        {
			lock (this) 
            {
				accumulator[0] = 0x67452301;
				accumulator[1] = 0xEFCDAB89;
				accumulator[2] = 0x98BADCFE;
				accumulator[3] = 0x10325476;
				base.Initialize();
			}
		}


		/// <summary>Process a block of data.</summary>
		/// <param name="inputBuffer">The block of data to process.</param>
		/// <param name="inputOffset">Where to start in the block.</param>
		override protected void ProcessBlock(byte[] inputBuffer, int inputOffset) 
        {
			lock (this) 
            {
				uint[] workBuffer;
				uint A = accumulator[0];
				uint B = accumulator[1];
				uint C = accumulator[2];
				uint D = accumulator[3];

				workBuffer = Utilities.ByteToUInt(inputBuffer, inputOffset, BlockSize);

				#region Round 1
				A = FF(A, B, C, D, workBuffer[ 0],  3);
				D = FF(D, A, B, C, workBuffer[ 1],  7);
				C = FF(C, D, A, B, workBuffer[ 2], 11);
				B = FF(B, C, D, A, workBuffer[ 3], 19);
				A = FF(A, B, C, D, workBuffer[ 4],  3);
				D = FF(D, A, B, C, workBuffer[ 5],  7);
				C = FF(C, D, A, B, workBuffer[ 6], 11);
				B = FF(B, C, D, A, workBuffer[ 7], 19);
				A = FF(A, B, C, D, workBuffer[ 8],  3);
				D = FF(D, A, B, C, workBuffer[ 9],  7);
				C = FF(C, D, A, B, workBuffer[10], 11);
				B = FF(B, C, D, A, workBuffer[11], 19);
				A = FF(A, B, C, D, workBuffer[12],  3);
				D = FF(D, A, B, C, workBuffer[13],  7);
				C = FF(C, D, A, B, workBuffer[14], 11);
				B = FF(B, C, D, A, workBuffer[15], 19);
				#endregion

				#region Round 2
				A = GG(A, B, C, D, workBuffer[ 0],  3);
				D = GG(D, A, B, C, workBuffer[ 4],  5);
				C = GG(C, D, A, B, workBuffer[ 8],  9);
				B = GG(B, C, D, A, workBuffer[12], 13);
				A = GG(A, B, C, D, workBuffer[ 1],  3);
				D = GG(D, A, B, C, workBuffer[ 5],  5);
				C = GG(C, D, A, B, workBuffer[ 9],  9);
				B = GG(B, C, D, A, workBuffer[13], 13);
				A = GG(A, B, C, D, workBuffer[ 2],  3);
				D = GG(D, A, B, C, workBuffer[ 6],  5);
				C = GG(C, D, A, B, workBuffer[10],  9);
				B = GG(B, C, D, A, workBuffer[14], 13);
				A = GG(A, B, C, D, workBuffer[ 3],  3);
				D = GG(D, A, B, C, workBuffer[ 7],  5);
				C = GG(C, D, A, B, workBuffer[11],  9);
				B = GG(B, C, D, A, workBuffer[15], 13);
				#endregion

				#region Round 3
				A = HH(A, B, C, D, workBuffer[ 0],  3);
				D = HH(D, A, B, C, workBuffer[ 8],  9);
				C = HH(C, D, A, B, workBuffer[ 4], 11);
				B = HH(B, C, D, A, workBuffer[12], 15);
				A = HH(A, B, C, D, workBuffer[ 2],  3);
				D = HH(D, A, B, C, workBuffer[10],  9);
				C = HH(C, D, A, B, workBuffer[ 6], 11);
				B = HH(B, C, D, A, workBuffer[14], 15);
				A = HH(A, B, C, D, workBuffer[ 1],  3);
				D = HH(D, A, B, C, workBuffer[ 9],  9);
				C = HH(C, D, A, B, workBuffer[ 5], 11);
				B = HH(B, C, D, A, workBuffer[13], 15);
				A = HH(A, B, C, D, workBuffer[ 3],  3);
				D = HH(D, A, B, C, workBuffer[11],  9);
				C = HH(C, D, A, B, workBuffer[ 7], 11);
				B = HH(B, C, D, A, workBuffer[15], 15);
				#endregion

				accumulator[0] += A;
				accumulator[1] += B;
				accumulator[2] += C;
				accumulator[3] += D;
			}
		}


		/// <summary>Process the last block of data.</summary>
		/// <param name="inputBuffer">The block of data to process.</param>
		/// <param name="inputOffset">Where to start in the block.</param>
		/// <param name="inputCount">How many bytes should be processed.</param>
        /// <returns>The hash code as an array of bytes</returns>
		override protected byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) 
        {
			lock (this) 
            {
				byte[] temp;
				int paddingSize;
				ulong size;

				// Figure out how much padding is needed between the last byte and the size.
				paddingSize = (int)(((ulong)inputCount + (ulong)Count) % (ulong)BlockSize);
				paddingSize = (BlockSize - 8) - paddingSize;
				if (paddingSize < 1) { paddingSize += BlockSize; }

				// Create the final, padded block(s).
				temp = new byte[inputCount + paddingSize + 8];
				Array.Copy(inputBuffer, inputOffset, temp, 0, inputCount);
				temp[inputCount] = 0x80;
				size = ((ulong)Count + (ulong)inputCount) * 8;
				Array.Copy(Utilities.ULongToByte(size), 0, temp, (inputCount + paddingSize), 8);

				// Push the final block(s) into the calculation.
				ProcessBlock(temp, 0);
				if (temp.Length == (BlockSize * 2)) {
					ProcessBlock(temp, BlockSize);
				}

				return Utilities.UIntToByte(accumulator);
			}
		}


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint FF(uint a, uint b, uint c, uint d, uint x, int s) {
			uint t = a + ((b & c) | (~b & d)) + x;
			return Utilities.RotateLeft(t, s);
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint GG(uint a, uint b, uint c, uint d, uint x, int s) {
			uint t = a + ((b & c) | (b & d) | (c & d)) + x + 0x5A827999;
			return Utilities.RotateLeft(t, s);
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private uint HH(uint a, uint b, uint c, uint d, uint x, int s) {
			uint t = a + (b ^ c ^ d) + x + 0x6ED9EBA1;
			return Utilities.RotateLeft(t, s);
		}
	}
}

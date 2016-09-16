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
	/// <summary>Computes the MD2 hash for the input data using the managed library.</summary>
	public class MD2 : BlockHashAlgorithm 
    {
		private byte[] state;
		private byte[] checksum;


		#region Table
		static private readonly byte[] PI_SUBST = {
			41,  46,  67,  201, 162, 216, 124, 1,   61,  54,  84,  161, 236, 240, 6,   19,
			98,  167, 5,   243, 192, 199, 115, 140, 152, 147, 43,  217, 188, 76,  130, 202,
			30,  155, 87,  60,  253, 212, 224, 22,  103, 66,  111, 24,  138, 23,  229, 18,
			190, 78,  196, 214, 218, 158, 222, 73,  160, 251, 245, 142, 187, 47,  238, 122,
			169, 104, 121, 145, 21,  178, 7,   63,  148, 194, 16,  137, 11,  34,  95,  33,
			128, 127, 93,  154, 90,  144, 50,  39,  53,  62,  204, 231, 191, 247, 151, 3,
			255, 25,  48,  179, 72,  165, 181, 209, 215, 94,  146, 42,  172, 86,  170, 198,
			79,  184, 56,  210, 150, 164, 125, 182, 118, 252, 107, 226, 156, 116, 4,   241,
			69,  157, 112, 89,  100, 113, 135, 32,  134, 91,  207, 101, 230, 45,  168, 2,
			27,  96,  37,  173, 174, 176, 185, 246, 28,  70,  97,  105, 52,  64,  126, 15,
			85,  71,  163, 35,  221, 81,  175, 58,  195, 92,  249, 206, 186, 197, 234, 38,
			44,  83,  13,  110, 133, 40,  132, 9,   211, 223, 205, 244, 65,  129, 77,  82,
			106, 220, 55,  200, 108, 193, 171, 250, 36,  225, 123, 8,   12,  189, 177, 74,
			120, 136, 149, 139, 227, 99,  232, 109, 233, 203, 213, 254, 59,  0,   29,  57,
			242, 239, 183, 14,  102, 88,  208, 228, 166, 119, 114, 248, 235, 117, 75,  10,
			49,  68,  80,  180, 143, 237, 31,  26,  219, 153, 141, 51,  159, 17,  131, 20
		};
		#endregion


		/// <summary>Initializes a new instance of the MD2 class.</summary>
		public MD2() : base(16) 
        {
			lock (this) 
            {
				HashSizeValue = 128;
				Initialize();
			}
		}


		/// <summary>Initializes the algorithm.</summary>
		override public void Initialize() 
        {
			lock (this) 
            {
				state = new byte[16];
				checksum = new byte[16];
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
				byte[] temp = new byte[48];
				int i, t;

				Array.Copy(state, 0, temp, 0, 16);
				Array.Copy(inputBuffer, inputOffset, temp, 16, 16);

				for (i = 0; i < 16; i++) {
					temp[i + 32] = (byte)((byte)state[i] ^ (byte)inputBuffer[inputOffset + i]);
				}

				for (i = 0, t = 0; i < 18; i++) {
					for (int j = 0; j < 48; j++ ) {
						t = temp[j] ^= PI_SUBST[t];
					}
					t = (t + i) & 0xFF;
				}

				Array.Copy(temp, 0, state, 0, 16);

				t = checksum[15];
				for (i = 0; i < 16; i++) {
					t = checksum[i] ^= PI_SUBST[inputBuffer[inputOffset + i] ^ t];
				}
			}
		}


		/// <summary>Process the last block of data.</summary>
		/// <param name="inputBuffer">The final block of data to process.</param>
		/// <param name="inputOffset">Where to start in the array.</param>
		/// <param name="inputCount">How many bytes should be processed.</param>
        /// <returns>The hash code as an array of bytes</returns>
		override protected byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {
			lock (this) {
				byte[] temp;
				int paddingSize;

				paddingSize = BlockSize - inputCount;
				if (paddingSize > 0) {
					temp = new byte[BlockSize];
					Array.Copy(inputBuffer, inputOffset, temp, 0, inputCount);
					for (int i = inputCount; i < BlockSize; i++) {
						temp[i] = (byte)paddingSize;
					}
					ProcessBlock(temp, 0);
				}

				ProcessBlock(checksum, 0);

				return state;
			}
		}
	}
}

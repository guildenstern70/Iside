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

namespace LLCryptoLib.Hash {
	/// <summary>Computes the Jenkins Hash for the input data using the managed library.</summary>
	public class JHash : BlockHashAlgorithm 
    {
		private uint[] accumulator;
		private uint length;


		/// <summary>Initializes a new instance of the JHash class.</summary>
		public JHash() : base(12) 
        {
			lock (this) 
            {
				HashSizeValue = 32;
				accumulator = new uint[3];
				Initialize();
			}
		}


		/// <summary>Initializes an implementation of System.Security.Cryptography.HashAlgorithm.</summary>
		override public void Initialize() 
        {
			lock (this) 
            {
				accumulator[0] = 0x9E3779B9;
				accumulator[1] = 0x9E3779B9;
				accumulator[2] = 0;
				length = 0;
				base.Initialize();
			}
		}


		/// <summary>Process a block of data.</summary>
		/// <param name="inputBuffer">The block of data to process.</param>
		/// <param name="inputOffset">Where to start in the block.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+9"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+8"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+7"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+6"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+5"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+4"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+3"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+2"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+11"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+10"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+1")]
        override protected void ProcessBlock(byte[] inputBuffer, int inputOffset) 
        {
			lock (this) 
            {
				accumulator[0] += (inputBuffer[inputOffset + 0] + (((uint)inputBuffer[inputOffset + 1]) << 8) + (((uint)inputBuffer[inputOffset + 2]) << 16) +  (((uint)inputBuffer[inputOffset + 3]) << 24));
				accumulator[1] += (inputBuffer[inputOffset + 4] + (((uint)inputBuffer[inputOffset + 5]) << 8) + (((uint)inputBuffer[inputOffset + 6]) << 16) +  (((uint)inputBuffer[inputOffset + 7]) << 24));
				accumulator[2] += (inputBuffer[inputOffset + 8] + (((uint)inputBuffer[inputOffset + 9]) << 8) + (((uint)inputBuffer[inputOffset + 10]) << 16) + (((uint)inputBuffer[inputOffset + 11]) << 24));
				length += 12;

				// Mix it up.
				accumulator[0] -= accumulator[1]; accumulator[0] -= accumulator[2]; accumulator[0] ^= (accumulator[2] >> 13);
				accumulator[1] -= accumulator[2]; accumulator[1] -= accumulator[0]; accumulator[1] ^= (accumulator[0] << 8);
				accumulator[2] -= accumulator[0]; accumulator[2] -= accumulator[1]; accumulator[2] ^= (accumulator[1] >> 13);
				accumulator[0] -= accumulator[1]; accumulator[0] -= accumulator[2]; accumulator[0] ^= (accumulator[2] >> 12);
				accumulator[1] -= accumulator[2]; accumulator[1] -= accumulator[0]; accumulator[1] ^= (accumulator[0] << 16);
				accumulator[2] -= accumulator[0]; accumulator[2] -= accumulator[1]; accumulator[2] ^= (accumulator[1] >> 5);
				accumulator[0] -= accumulator[1]; accumulator[0] -= accumulator[2]; accumulator[0] ^= (accumulator[2] >> 3);
				accumulator[1] -= accumulator[2]; accumulator[1] -= accumulator[0]; accumulator[1] ^= (accumulator[0] << 10);
				accumulator[2] -= accumulator[0]; accumulator[2] -= accumulator[1]; accumulator[2] ^= (accumulator[1] >> 15);
			}
		}


		/// <summary>Process the last block of data.</summary>
		/// <param name="inputBuffer">The block of data to process.</param>
		/// <param name="inputOffset">Where to start in the block.</param>
		/// <param name="inputCount">How many bytes need to be processed.</param>
        /// <returns>The hash code as an array of bytes</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+9"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+8"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+7"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+6"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+5"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+4"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+3"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+2"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+10"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "inputOffset+1")]
        override protected byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {
			lock (this) {
				accumulator[2] += length + (uint)inputCount;

				switch (inputCount) {
					case 11: accumulator[2] += ((uint)inputBuffer[inputOffset + 10] << 24);	goto case 10;
					case 10: accumulator[2] += ((uint)inputBuffer[inputOffset + 9] << 16);	goto case 9;
					case 9:  accumulator[2] += ((uint)inputBuffer[inputOffset + 8] << 8);	goto case 8;
					case 8:  accumulator[1] += ((uint)inputBuffer[inputOffset + 7] << 24);	goto case 7;
					case 7:  accumulator[1] += ((uint)inputBuffer[inputOffset + 6] << 16);	goto case 6;
					case 6:  accumulator[1] += ((uint)inputBuffer[inputOffset + 5] << 8);	goto case 5;
					case 5:  accumulator[1] += ((uint)inputBuffer[inputOffset + 4]);		goto case 4;
					case 4:  accumulator[0] += ((uint)inputBuffer[inputOffset + 3] << 24);	goto case 3;
					case 3:  accumulator[0] += ((uint)inputBuffer[inputOffset + 2] << 16);	goto case 2;
					case 2:  accumulator[0] += ((uint)inputBuffer[inputOffset + 1] << 8);	goto case 1;
					case 1:  accumulator[0] += ((uint)inputBuffer[inputOffset + 0]);		break;
				}

				// Mix it up.
				accumulator[0] -= accumulator[1]; accumulator[0] -= accumulator[2]; accumulator[0] ^= (accumulator[2] >> 13);
				accumulator[1] -= accumulator[2]; accumulator[1] -= accumulator[0]; accumulator[1] ^= (accumulator[0] << 8);
				accumulator[2] -= accumulator[0]; accumulator[2] -= accumulator[1]; accumulator[2] ^= (accumulator[1] >> 13);
				accumulator[0] -= accumulator[1]; accumulator[0] -= accumulator[2]; accumulator[0] ^= (accumulator[2] >> 12);
				accumulator[1] -= accumulator[2]; accumulator[1] -= accumulator[0]; accumulator[1] ^= (accumulator[0] << 16);
				accumulator[2] -= accumulator[0]; accumulator[2] -= accumulator[1]; accumulator[2] ^= (accumulator[1] >> 5);
				accumulator[0] -= accumulator[1]; accumulator[0] -= accumulator[2]; accumulator[0] ^= (accumulator[2] >> 3);
				accumulator[1] -= accumulator[2]; accumulator[1] -= accumulator[0]; accumulator[1] ^= (accumulator[0] << 10);
				accumulator[2] -= accumulator[0]; accumulator[2] -= accumulator[1]; accumulator[2] ^= (accumulator[1] >> 15);

				return Utilities.UIntToByte(accumulator[2], EndianType.BigEndian);
			}
		}
	}
}

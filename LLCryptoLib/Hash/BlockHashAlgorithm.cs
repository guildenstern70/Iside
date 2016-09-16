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
	/// <summary>Represents the abstract class from which all implementations of the Classless.Hasher.BlockHashAlgorithm inherit.</summary>
	abstract public class BlockHashAlgorithm : System.Security.Cryptography.HashAlgorithm 
    {
		private int blockSize;
		private byte[] buffer;
		private int bufferCount;
		private long count;


		/// <summary>The size in bytes of an individual block.</summary>
		public int BlockSize 
        {
			get { return blockSize; }
		}

		/// <summary>The number of bytes currently in the buffer waiting to be processed.</summary>
		public int BufferCount 
        {
			get { return bufferCount; }
		}

		/// <summary>The number of bytes that have been processed.</summary>
		/// <remarks>This number does NOT include the bytes that are waiting in the buffer.</remarks>
		public long Count 
        {
			get { return count; }
		}


		/// <summary>Initializes a new instance of the BlockHashAlgorithm class.</summary>
		/// <param name="blockSize">The size in bytes of an individual block.</param>
		protected BlockHashAlgorithm(int blockSize) : base() 
        {
			this.blockSize = blockSize;
		}

		/// <summary>Initializes the algorithm.</summary>
		/// <remarks>If this function is overriden in a derived class, the new function should call back to
		/// this function or you could risk garbage being carried over from one calculation to the next.</remarks>
		override public void Initialize() 
        {
			lock (this) 
            {
				count = 0;
				bufferCount = 0;
				State = 0;
				buffer = new byte[BlockSize];
			}
		}

		/// <summary>Performs the hash algorithm on the data provided.</summary>
		/// <param name="array">The array containing the data.</param>
		/// <param name="ibStart">The position in the array to begin reading from.</param>
		/// <param name="cbSize">How many bytes in the array to read.</param>
		override protected void HashCore(byte[] array, int ibStart, int cbSize) 
        {
			lock (this) 
            {
				int i;

				// Use what may already be in the buffer.
				if (BufferCount > 0) 
                {
					if (cbSize < (BlockSize - BufferCount)) 
                    {
						// Still don't have enough for a full block, just store it.
						Array.Copy(array, ibStart, buffer, BufferCount, cbSize);
						bufferCount += cbSize;
						return;
					}
                    else 
                    {
						// Fill out the buffer to make a full block, and then process it.
						i = BlockSize - BufferCount;
						Array.Copy(array, ibStart, buffer, BufferCount, i);
						ProcessBlock(buffer, 0);
						count += (long)BlockSize;
						bufferCount = 0;
						ibStart += i;
						cbSize -= i;
					}
				}

				// For as long as we have full blocks, process them.
				for (i = 0; i < (cbSize - (cbSize % BlockSize)); i += BlockSize) 
                {
					ProcessBlock(array, ibStart + i);
					count += (long)BlockSize;
				}

				// If we still have some bytes left, store them for later.
				int bytesLeft = cbSize % BlockSize;
				if (bytesLeft != 0) 
                {
					Array.Copy(array, ((cbSize - bytesLeft) + ibStart), buffer, 0, bytesLeft);
					bufferCount = bytesLeft;
				}
			}
		}


		/// <summary>Performs any final activities required by the hash algorithm.</summary>
		/// <returns>The final hash value.</returns>
		override protected byte[] HashFinal() 
        {
			lock (this) 
            {
				return ProcessFinalBlock(buffer, 0, bufferCount);
			}
		}


		/// <summary>Process a block of data.</summary>
		/// <param name="inputBuffer">The block of data to process.</param>
		/// <param name="inputOffset">Where to start in the block.</param>
		abstract protected void ProcessBlock(byte[] inputBuffer, int inputOffset);


		/// <summary>Process the last block of data.</summary>
		/// <param name="inputBuffer">The block of data to process.</param>
		/// <param name="inputOffset">Where to start in the block.</param>
		/// <param name="inputCount">How many bytes need to be processed.</param>
        /// <returns>The hash code in bytes</returns>
		abstract protected byte[] ProcessFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);
	}
}

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
	/// <summary>A class that contains the parameters necessary to initialize a FNV algorithm.</summary>
	public class FNVParameters : HashAlgorithmParameters 
    {
		private int order;
		private long prime;
		private long offsetBasis;
		private FNVAlgorithmType type;


		/// <summary>Gets or sets the order of the FNV (e.g., how many bits).</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public int Order 
        {
			get { return order; }
			set 
            {
				if ((value != 32) && (value != 64)) {
					throw new ArgumentException("The FNV order can only be 32 or 64 bits long.", "Order");
				} else {
					order = value;
				}
			}
		}

		/// <summary>Gets or sets the prime number to use in the FNV calculations.</summary>
		public long Prime 
        {
			get { return prime; }
			set { prime = value; }
		}

		/// <summary>Gets or sets the offset basis of the FNV.</summary>
		public long OffsetBasis 
        {
			get { return offsetBasis; }
			set { offsetBasis = value; }
		}

		/// <summary>Gets or sets the FNV algorithm variation.</summary>
		public FNVAlgorithmType Variation 
        {
			get { return type; }
			set { type = value; }
		}


		/// <summary>Initializes a new instance of the FNVParamters class.</summary>
		/// <param name="order">The order of the FNV (e.g., how many bits).</param>
		/// <param name="prime">The prime number to use in the FNV calculations.</param>
		/// <param name="offsetBasis">The offset basis of the FNV.</param>
		/// <param name="type">The FNV algorithm variation.</param>
		public FNVParameters(int order, long prime, long offsetBasis, FNVAlgorithmType type) 
        {
			this.Order = order;
			this.Prime = prime;
			this.OffsetBasis = offsetBasis;
			this.Variation = type;
		}


		/// <summary>Retrieves a standard set of FNV parameters.</summary>
		/// <param name="standard">The name of the standard parameter set to retrieve.</param>
		/// <returns>The FNV Parameters for the given standard.</returns>
		public static FNVParameters GetParameters(FNVStandard standard) 
        {
			FNVParameters temp;

			switch (standard) 
            {
				case FNVStandard.FNV0_32:	temp = new FNVParameters(32, 0x01000193,     0x00000000, FNVAlgorithmType.FNV1); break;
				case FNVStandard.FNV0_64:	temp = new FNVParameters(64, 0x0100000001B3, 0x00000000, FNVAlgorithmType.FNV1); break;
				case FNVStandard.FNV1_32:	temp = new FNVParameters(32, 0x01000193,     0x811C9DC5, FNVAlgorithmType.FNV1); break;
				case FNVStandard.FNV1_64:	temp = new FNVParameters(64, 0x0100000001B3, unchecked((long)0xCBF29CE484222325), FNVAlgorithmType.FNV1); break;
				case FNVStandard.FNV1A_32:	temp = new FNVParameters(32, 0x01000193,     0x811C9DC5, FNVAlgorithmType.FNV1A); break;
				case FNVStandard.FNV1A_64:	temp = new FNVParameters(64, 0x0100000001B3, unchecked((long)0xCBF29CE484222325), FNVAlgorithmType.FNV1A); break;
				default:					temp = new FNVParameters(32, 0x01000193,     0x811C9DC5, FNVAlgorithmType.FNV1); break;
			}

			return temp;
		}
	}
}

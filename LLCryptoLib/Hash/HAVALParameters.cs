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
	/// <summary>A class that contains the parameters necessary to initialize a HAVAL algorithm.</summary>
	public class HAVALParameters : HashAlgorithmParameters 
    {
		private short passes;
		private short length;


		/// <summary>Gets or sets the number of passes.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public short Passes 
        {
			get { return passes; }
			set 
            {
				if ((value != 3) && (value != 4) && (value != 5)) 
                {
					throw new ArgumentException("The number of passes can only be 3, 4, or 5.", "Passes");
				} 
                else 
                {
					passes = value;
				}
			}
		}

		/// <summary>Gets or sets the bit length.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public short Length 
        {
			get { return length; }
			set 
            {
				if ((value != 128) && (value != 160) && (value != 192) && (value != 224) && (value != 256)) 
                {
					throw new ArgumentException("The HAVAL bit length can only be 128, 160, 192, 224, or 256 bits long.", "Length");
				} 
                else 
                {
					length = value;
				}
			}
		}


		/// <summary>Initializes a new instance of the HAVALParamters class.</summary>
		/// <param name="passes">How many transformation passes to do.</param>
		/// <param name="length">The bit length of the final hash.</param>
		public HAVALParameters(short passes, short length) 
        {
			this.Passes = passes;
			this.Length = length;
		}


		/// <summary>Retrieves a standard set of HAVAL parameters.</summary>
		/// <param name="standard">The name of the standard parameter set to retrieve.</param>
		/// <returns>The HAVAL Parameters for the given standard.</returns>
		public static HAVALParameters GetParameters(HAVALStandard standard) 
        {
			HAVALParameters temp;

			switch (standard) 
            {
				case HAVALStandard.HAVAL_3_128:	temp = new HAVALParameters(3, 128);	break;
				case HAVALStandard.HAVAL_3_160:	temp = new HAVALParameters(3, 160);	break;
				case HAVALStandard.HAVAL_3_192:	temp = new HAVALParameters(3, 192);	break;
				case HAVALStandard.HAVAL_3_224:	temp = new HAVALParameters(3, 224);	break;
				case HAVALStandard.HAVAL_3_256:	temp = new HAVALParameters(3, 256);	break;
				case HAVALStandard.HAVAL_4_128:	temp = new HAVALParameters(4, 128);	break;
				case HAVALStandard.HAVAL_4_160:	temp = new HAVALParameters(4, 160);	break;
				case HAVALStandard.HAVAL_4_192:	temp = new HAVALParameters(4, 192);	break;
				case HAVALStandard.HAVAL_4_224:	temp = new HAVALParameters(4, 224);	break;
				case HAVALStandard.HAVAL_4_256:	temp = new HAVALParameters(4, 256);	break;
				case HAVALStandard.HAVAL_5_128:	temp = new HAVALParameters(5, 128);	break;
				case HAVALStandard.HAVAL_5_160:	temp = new HAVALParameters(5, 160);	break;
				case HAVALStandard.HAVAL_5_192:	temp = new HAVALParameters(5, 192);	break;
				case HAVALStandard.HAVAL_5_224:	temp = new HAVALParameters(5, 224);	break;
				case HAVALStandard.HAVAL_5_256:	temp = new HAVALParameters(5, 256);	break;
				default:						temp = new HAVALParameters(5, 256);	break;
			}

			return temp;
		}
	}
}

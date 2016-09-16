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




namespace LLCryptoLib.Crypto
{

	/// <summary>
	/// PseudoDES text encoding class.
	/// PseudoDES is a TextVigenere type of encoding, remade
	/// for a number of times equal to TextAlgorithmParameters.Shift.
	/// If TextAlgorithmParameters.Shift is zero, then a value of 7
	/// is taken.
	/// </summary>
	public class TextPseudoDes : TextVigenere
	{

		private int howManyTimes;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p">Parametri must have a valid key</param>
		internal TextPseudoDes(TextAlgorithmParameters p) : base(p)
		{
			if (p.Shift==0)
			{
				this.howManyTimes=7;
			}
			else
			{
				this.howManyTimes=p.Shift;
			}
		}

		/// <summary>
		/// Encoding algorithm
		/// </summary>
		/// <param name="txt">Plain text</param>
		/// <returns>Encoded text</returns>
		public override string Code(string txt)
		{
			string sequenceTmp = txt;

			for (short j=0; j<(short)howManyTimes; j++)
			{
				sequenceTmp = this.Algo(sequenceTmp,true);
			}

			return sequenceTmp;
		}

		/// <summary>
		/// Decoding algorithm
		/// </summary>
		/// <param name="txt">Encoded text</param>
		/// <returns>Decoded text</returns>
		public override string Decode(string txt)
		{
			string sequenceTmp = txt;

			for (short j=0; j<(short)howManyTimes; j++)
			{
				sequenceTmp = this.Algo(sequenceTmp,false);
			}

			return sequenceTmp;
		}


        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
		public override string ToString()
		{
			return "Polyalphabetic with key ="+this.key+" and shift ="+this.shift;
		}


	}
}

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

namespace LLCryptoLib.Utils
{
	/// <summary>
    /// PasswordComplexityIndex class can evaluate a string to return an index
	/// between 0 (minimum) and 40 (maximum) that indicates
	/// the password strength.
	/// </summary>
	public class PasswordComplexityIndex
	{
		private const int ALPHASET = 26;
		private const int NUMBERSET = 10;
		private const int SPECIALSET = 32;
		private const ulong HIDESKTOPTRIALRATE = 17179869184; // 2*2^33

		private ulong _complexity;
		private string _cryptoPassword;
		private int _space;
		private int _totalChars;

		/// <summary>
		/// The password complexity index.
		/// </summary>
		/// <see href="http://www.mandylionlabs.com/index15.htm"/>
		public PasswordComplexityIndex()
		{
			this._cryptoPassword = String.Empty;
		}

		/// <summary>
		/// Evaluate the complexity index of the given password/passphrase
		/// </summary>
		/// <param name="passphrase">a given password string</param>
		public void Evaluate(string passphrase)
		{
			this._cryptoPassword = passphrase;
			this.ComputeComplexity();
		}

		/// <summary>
		/// The complexity of this password as total requested workload in floating points
		/// </summary>
		public ulong Complexity
		{
			get
			{
				return this._complexity;
			}
		}

		/// <summary>
		/// The estimated hours to crack the password on a computer capable of 2*2^33 trials per hour
		/// </summary>
		public float HoursToCrack
		{
			get
			{
				float hoursToCrack = 0.0F;
				hoursToCrack = this._complexity / HIDESKTOPTRIALRATE;
				return hoursToCrack;
			}
		}

		/// <summary>
		/// It is ln(Complexity), with a value between 0 (minimum) and 40 (maximum)
		/// </summary>
		public uint ComplexityProxy
		{
			get
			{
				uint percentage = (uint)(Math.Log(this._complexity));
				return Math.Min(40, percentage);
			}
		}

		private void ComputeComplexity()
		{
			ulong complexity;
			this.ScanCharacters();


			complexity = PasswordComplexityIndex.Lambda(this._space, this._totalChars);


			this._complexity = (complexity/2);
		}

		private static ulong Lambda(int setNumber, int number)
		{
			double nPow = Math.Pow(setNumber, number);
			if ((nPow)>Int64.MaxValue)
			{
				nPow = Int64.MaxValue;
			}
			ulong lambda = (number > 0 ? (ulong)(nPow) : 1);
			return lambda;
		}

		private void ScanSpace(int lw, int up, int nb, int sp)
		{
			int setSpace = 0;

			if (lw > 0)
			{
				setSpace += ALPHASET;
			}

			if (up > 0)
			{
				setSpace += ALPHASET;
			}

			if (nb > 0)
			{
				setSpace += NUMBERSET;
			}

			if (sp > 0)
			{
				setSpace += SPECIALSET;
			}

			this._space = setSpace;
			this._totalChars = lw + up + nb + sp;
		}

		private void ScanCharacters()
		{
			int lowerAlfaChars = 0;
			int upperAlfaChars = 0;
			int numberChars = 0;
			int specialChars = 0;

			foreach (Char character in this._cryptoPassword)
			{
				if (Char.IsDigit(character))
				{
					++numberChars;
				}
				else if (Char.IsLower(character))
				{
					++lowerAlfaChars;
				}
				else if (Char.IsUpper(character))
				{
					++upperAlfaChars;
				}
				else
				{
					++specialChars;
				}
			}

			this.ScanSpace(lowerAlfaChars, upperAlfaChars, numberChars, specialChars);
		}
	}
}

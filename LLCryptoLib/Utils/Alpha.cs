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
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */

namespace LLCryptoLib.Utils
{

	/// <summary>
	/// Utility class to handle alphabetic shifts.
    /// It defines a standard set of characters, the "Characters" string, which is
    /// <code>
    /// <![CDATA[ABCDEFGHIJKLMNOPQRSTUVWXYZ{} []()<>,.?;:'+-=@!#$%^&*~`_|/abcdefghijklmnopqrstuvwxyz0123456789]]>
    /// </code>
    /// and then performs method on the "Characters" string.
	/// </summary>
	public static class Alpha
	{
		private static string Beto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ{} []()<>,.?;:'+-=@!#$%^&*~`_|/abcdefghijklmnopqrstuvwxyz0123456789";

		/// <summary>
		/// Get a string with ASCII alphanumeric characters.
        /// The character string is defined as follows:
        /// <code>
        /// <![CDATA[ABCDEFGHIJKLMNOPQRSTUVWXYZ{} []()<>,.?;:'+-=@!#$%^&*~`_|/abcdefghijklmnopqrstuvwxyz0123456789]]>
        /// </code>
		/// </summary>
		public static string Characters
		{
			get
			{
				return Beto;
			}

            set
            {
                Alpha.Beto = value;
            }
		}

		/// <summary>
		/// Return the number of characters in Characters string
		/// </summary>
        /// <returns>The number of characters in Characters string</returns>
		public static short CharactersLen
		{
			get
			{
				return (short)Alpha.Beto.Length;
			}
		}

		/// <summary>
        /// Return true if character is in Characters string
		/// </summary>
		/// <param name="car">Character to be checked</param>
        /// <returns>True if car is inside the Characters string</returns>
		public static bool IsInAlfa(char car)
		{
			int dove;

			dove=Beto.IndexOf(car);

			if (dove==-1)
			{
				return false;
			}

			return true; 
		}

		/// <summary>
        /// Return the char in Characters at position pos
		/// </summary>
		/// <param name="pos">Position</param>
        /// <returns>Corresponding char in Characters string</returns>
		internal static char GetCharAt(int pos)
		{
			int lenB = Beto.Length;

			//charAt va da 0 a lenB-1
			if ( pos>lenB-1 )
			{
				pos -= lenB;
			}
			else if ( pos<0 )
			{
				pos += lenB;
			}

			return Beto[pos];
		}

		/// <summary>
        /// Get position of char 'car' in Characters string
		/// </summary>
		/// <param name="car">Given Character</param>
		/// <returns>Position</returns>
		internal static int GetPosOf(char car)
		{
			return Beto.IndexOf(car);
		}

	}

    /// <summary>
    ///  LLCryptoLib.Utils library contains the utilities
    ///  used throughout the code, such as Input/Output, Password meter,
    ///  Windows registry and so on.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }

}

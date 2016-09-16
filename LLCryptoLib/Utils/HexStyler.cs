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

using System;
using System.Text;
using System.Globalization;


namespace LLCryptoLib.Utils
{
	/// <summary>
	/// HexStyler.
	/// This class holds a string representation of a series of bytes[].
	/// The available styles are:
	/// <list type="bullet">
	/// <item>UNIX</item><description>Example: ff12ab4d</description>
	/// <item>SPACE</item><description>Example: FF 12 AB 4D</description>
    /// <item>CLASSIC</item><description>Example: FF12AB4D</description>
    /// <item>NETSCAPE</item><description>Example: FF:12:AB:4D</description>
	/// </list>
	/// </summary>
	public class HexStyler
	{
		/// <summary>
		/// Unix style: ff12ab4d
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static readonly HexStyler UNIX = new HexStyler(HexEnum.UNIX);

		/// <summary>
		/// Space style: FF 12 AB 4D
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static readonly HexStyler SPACE = new HexStyler(HexEnum.SPACE);

		/// <summary>
		/// Classic style: FF12AB4D
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static readonly HexStyler CLASSIC = new HexStyler(HexEnum.CLASSIC);

		/// <summary>
		/// Netscape(TM) style: FF:12:AB:4D
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static readonly HexStyler NETSCAPE = new HexStyler(HexEnum.NETSCAPE);

        /// <summary>
        /// Netscape(TM) style: FF:12:AB:4D
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static readonly HexStyler MODERN = new HexStyler(HexEnum.MODERN);

		private HexEnum style;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="i">Identifier of style.
		/// </param>
		public HexStyler(HexEnum i)
		{
			this.style = i;
		}

        /// <summary>
        /// Availables hex styles
        /// </summary>
        /// <returns></returns>
        public static string[] AvailableStyles()
        {
            return new string[] { "CLASSIC", "MODERN", "UNIX", "SPACE", "NETSCAPE(tm)" };
        }

		/// <summary>
		/// Get/Set the style ID for this Style.
		/// </summary>
		public HexEnum Style
		{
			get
			{
				return this.style;
			}
		}

		/// <summary>
		/// Return the hex style format of an hexstring
		/// </summary>
		/// <param name="hexstring">an existing string with hexadecimal numbers</param>
		/// <returns>The recognized HexStyle, or HexStyle.UNKNOWN if not recognized</returns>
        /// <exception cref="ArgumentNullException" />
		public static HexEnum Recognize(string hexstring)
		{
            if (hexstring == null)
            {
                throw new ArgumentNullException("hexstring");
            }

			HexEnum hs = HexEnum.UNKNOWN;

			// Is hexadecimal?
			string firstTwo = hexstring.Substring(0,2);
			try
			{
				Int32.Parse(firstTwo,NumberStyles.HexNumber);
			}
			catch (FormatException)
			{
				return hs;
			}

			char[] lowerAlpha = { 'a', 'b', 'c', 'd', 'e', 'f' };
			
			// Determine hexstyle
			if (hexstring.IndexOf(':')>0)
			{
				hs = HexEnum.NETSCAPE;
			}
			else if (hexstring.IndexOf(' ')>0)
			{
                if (hexstring.IndexOfAny(lowerAlpha) > 0)
                {
                    hs = HexEnum.MODERN;
                }
                else
                {
                    hs = HexEnum.SPACE;
                }
			}
			else if (hexstring.IndexOfAny(lowerAlpha)>0)
			{
				hs = HexEnum.UNIX;
			}
			else
			{
				hs = HexEnum.CLASSIC;
			}

			return hs;
		}

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object">HexStyler</see>.
        /// </returns>
		public override int GetHashCode()
		{
			return (int)this.style*343;
		}


        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
		public override bool Equals(object obj)
		{ 
			if(!base.Equals(obj))
				return false;

			if(obj == null)
				return false; 

			if(this.GetType() != obj.GetType())
				return false;

			HexStyler toCfr = (HexStyler)obj;
			if (this.style==toCfr.style)
			{
				return true;
			}

			return false;
		}


        /// <summary>
        /// Operator == for the HexStyler object
        /// </summary>
        /// <param name="lhs">An HexStyler object</param>
        /// <param name="rhs">An HexStyler object</param>
        /// <returns>true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false</returns>
        /// <exception cref="ArgumentNullException" />
		public static bool operator==(HexStyler lhs, HexStyler rhs)
		{

            if (lhs == null)
            {
                return false;
            }

            if (rhs == null)
            {
                return false;
            }

			// If both are null, or both are same instance, return true.
			if (Object.ReferenceEquals(lhs, rhs))
			{
				return true;
			}
				// If one is null, but not both, return false.
			else if (((object)lhs == null) || ((object)rhs == null))
			{
				return false;
			}
			// Otherwise, compare values and return:
			else return lhs.Style == rhs.Style;
		}

		/// <summary>
		/// If this style is NOT equal to obj
		/// </summary>
        /// <param name="lhs">An HexStyler object</param>
        /// <param name="rhs">An HexStyler object</param>
        /// <returns>false if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, true</returns>
		public static bool operator!=(HexStyler lhs, HexStyler rhs)
		{ 
			return !(lhs == rhs);
		}

		/// <summary>
		/// Transform an HexStyler style to another style
		/// </summary>
		/// <param name="toStyle">The new style to apply to 'styledText'</param>
		/// <param name="styledText">The string in 'this' style form</param>
		/// <returns>The string transformed from this style, to style 'toStyle'</returns>
		public string Transform(HexEnum toStyle, string styledText)
		{
			string unix = this.ToPlain(styledText);
			return HexStyler.Format(unix, toStyle);
		}

		/// <summary>
		/// Transform the given string in this representation style into a string without style
		/// (the same to be given in input to Format method)
		/// </summary>
        /// <param name="styledText">Original string with style. The style of the string must be 'this'</param>
		/// <returns>The given string with style without style (in Unix style)</returns>
        /// <exception cref="ArgumentNullException" />
		public string ToPlain(string styledText)
		{
            if (styledText == null)
            {
                throw new ArgumentNullException("styledText");
            }

			string plain = null;

			switch (this.style)
			{
				case HexEnum.UNIX: // UNIX 
					plain = styledText; // Nothing to do
					break;

				case HexEnum.SPACE: // SPACE
                case HexEnum.MODERN: // MODERN
					plain = styledText.Replace(" ","");
					break;

				case HexEnum.NETSCAPE: // NETSCAPE
					plain = styledText.Replace(":","");
					break;

				case HexEnum.CLASSIC: // CLASSIC
				default:
					plain = styledText.ToLower();
					break;
			}

			return plain;
		}


		
		/// <summary>
		/// Given a stringbuilder of bytes values in hexadecimal format
		/// return a string in the wanted style.
		/// </summary>
        /// <param name="hexRepr">Is a StringBuilder containing a plain bytes representation (in the UNIX form) in lowercase or uppercase</param>
		/// <param name="newFormat">The wanted style</param>
		/// <returns>A string with the wanted style</returns>
		public static string Format(string hexRepr, HexEnum newFormat)
		{
			string output = hexRepr;

			switch (newFormat)
			{
				case HexEnum.UNIX: // UNIX
					output = output.ToLower();
					break;

				case HexEnum.SPACE: // SPACE
                    output = HexStyler.InsertChar(' ', output);
                    output = output.ToUpper();
					break;

                case HexEnum.MODERN: // SPACE
                    output = HexStyler.InsertChar(' ', output);
                    output = output.ToLower();
                    break;

				case HexEnum.NETSCAPE: // NETSCAPE
                    output = HexStyler.InsertChar(':', output);
					output = output.Substring(0,output.Length-1);
                    output = output.ToUpper();
					break;

				case HexEnum.CLASSIC: // CLASSIC
				default:
					output = output.ToUpper();
					break;
			}

			return output.Trim();

		}

		/// <summary>
		/// Given a stringbuilder of bytes values in hexadecimal format
		/// return a string in the wanted style.
		/// </summary>
        /// <param name="hexBytesRepr">Is a StringBuilder containing a plain bytes representation (in the UNIX form) in lowercase or uppercase</param>
		/// <returns>A string with the wanted style</returns>
		public string Format(string hexBytesRepr)
		{
			return HexStyler.Format(hexBytesRepr, this.Style);
		}

        private static string InsertChar(char c, string s)
        {
            StringBuilder newText = new StringBuilder();
            int counter = 0;

            foreach (char cx in s)
            {
                newText.Append(cx);
                counter++;
                if (counter % 2 == 0)
                {
                    newText.Append(c);
                }
            }

            return newText.ToString();
        }

	}
}

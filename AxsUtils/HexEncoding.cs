/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved
 * 
 * AxsUtils.HexEncoding.cs
 * 
 */
using System;
using System.Text;

namespace AxsUtils
{
	/// <summary>
	/// HexEncoding.
	/// </summary>
	public static class HexEncoding
	{

        /// <summary>
        /// Gets the byte count.
        /// </summary>
        /// <param name="hexNumber">The hex number.</param>
        /// <returns></returns>
		public static int GetByteCount(string hexNumber)
		{

            if (hexNumber == null)
            {
                throw new ArgumentNullException("hexNumber");
            }

			int numHexChars = 0;
			char c;
			// remove all none A-F, 0-9, characters
			for (int i=0; i<hexNumber.Length; i++)
			{
				c = hexNumber[i];
				if (IsHexDigit(c))
					numHexChars++;
			}
			// if odd number of characters, discard last character
			if (numHexChars % 2 != 0)
			{
				numHexChars--;
			}
			return numHexChars / 2; // 2 characters per byte
		}


		/// <summary>
		/// Creates a byte array from the hexadecimal string. Each two characters are combined
		/// to create one byte. First two hexadecimal characters become first byte in returned array.
		/// Non-hexadecimal characters are ignored. 
		/// </summary>
		/// <param name="hexString">string to convert to byte array</param>
		/// <param name="discarded">number of characters in string ignored</param>
		/// <returns>byte array, in the same left-to-right order as the hexString</returns>
		public static byte[] GetBytes(string hexNumber, out int discarded)
		{
            if (hexNumber == null)
            {
                throw new ArgumentNullException("hexNumber");
            }

			discarded = 0;
			StringBuilder sb = new StringBuilder();
			char c;
			// remove all none A-F, 0-9, characters
			for (int i=0; i<hexNumber.Length; i++)
			{
				c = hexNumber[i];
				if (IsHexDigit(c))
				{
					sb.Append(c);
				}
				else
				{
					discarded++;
				}
			}

			string newString = sb.ToString();

			// if odd number of characters, discard last character
			if (newString.Length % 2 != 0)
			{
				discarded++;
				newString = newString.Substring(0, newString.Length-1);
			}

			int byteLength = newString.Length / 2;
			byte[] bytes = new byte[byteLength];
			string hex;
			int j = 0;
			for (int i=0; i<bytes.Length; i++)
			{
				hex = new String(new Char[] {newString[j], newString[j+1]});
				bytes[i] = HexToByte(hex);
				j = j+2;
			}
			return bytes;
		}

        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
		public static string ToString(byte[] message)
		{
			StringBuilder hexString = new StringBuilder();
			for (int i=0; i<message.Length; i++)
			{
				hexString.Append(message[i].ToString("X2"));
			}
			return hexString.ToString();
		}

		/// <summary>
		/// Determines if given string is in proper hexadecimal string format
		/// </summary>
		/// <param name="hexString"></param>
		/// <returns></returns>
		public static bool IsHexFormat(string hexString)
		{
            if (hexString == null)
            {
                throw new ArgumentNullException("hexString");
            }

			bool hexFormat = true;

			foreach (char digit in hexString)
			{
				if (!IsHexDigit(digit))
				{
					hexFormat = false;
					break;
				}
			}
			return hexFormat;
		}

		/// <summary>
		/// Returns true is c is a hexadecimal digit (A-F, a-f, 0-9)
		/// </summary>
		/// <param name="c">Character to test</param>
		/// <returns>true if hex digit, false if not</returns>
		public static bool IsHexDigit(Char c)
		{
			int numChar;
			int numA = Convert.ToInt32('A');
			int num1 = Convert.ToInt32('0');
			c = Char.ToUpper(c);
			numChar = Convert.ToInt32(c);
			if (numChar >= numA && numChar < (numA + 6))
				return true;
			if (numChar >= num1 && numChar < (num1 + 10))
				return true;
			return false;
		}

		/// <summary>
		/// Converts 1 or 2 character string into equivalant byte value
		/// </summary>
		/// <param name="hex">1 or 2 character string</param>
		/// <returns>byte</returns>
		private static byte HexToByte(string hex)
		{
			if (hex.Length > 2 || hex.Length <= 0)
				throw new ArgumentException("hex must be 1 or 2 characters in length");
			byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
			return newByte;
		}


	}
}

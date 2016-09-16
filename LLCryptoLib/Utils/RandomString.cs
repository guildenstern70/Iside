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

namespace LLCryptoLib.Utils
{
    /// <summary>
    /// This class generates random strings in various formats.
    /// </summary>
    public static class RandomString
    {

        private static Random seed = new Random();
        private const string ALPHA = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string ALPHA_SPACE = "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890";
        private const string EXTENDED_ALPHA = "abcdefghijklmnopqrstuvwxyz!@#$%^&*()è+òàìùç©¡£¿\r\nABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;':[]{}_=";
        private const string EXTENDED_ALPHA_SPACE = "abcdefghijklmnopqrstuvwxyz !@#$%^&*()è+òàìùç©¡£¿\r\nABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;':[]{}_=";

        /// <summary>
        /// Get a random string made of 10 alphanumeric characters, both uppercase and lowercase, with numbers.
        /// </summary>
        /// <returns>A random string of 10 alphanumeric characters.</returns>
        public static string Get()
        {
            return RandomString.Get(10);
        }

        /// <summary>
        /// Get a random string made of 10 alphanumeric characters, both uppercase and lowercase,
        /// with symbols and numbers.
        /// </summary>
        /// <returns>A random string 10 alphanumeric characters, both uppercase and lowercase, with symbols and numbers.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
        public static string GetEx()
        {
            return RandomString.GetEx(10);
        }

        /// <summary>
        /// Get a random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers.
        /// </summary>
        /// <param name="len">The length of the random generated string.</param>
        /// <returns>A random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers.</returns>
        public static string Get(int len)
        {
            return RandomString.GetEx(len, false);
        }

        /// <summary>
        /// Get a random string made of 'len' alphanumeric characters, both uppercase and lowercase,
        /// with symbols and numbers.
        /// </summary>
        /// <param name="len">A random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers and symbols.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
        public static string GetEx(int len)
        {
            return RandomString.GetEx(len, true);
        }

        /// <summary>
        /// Determines whether the character c between alphanumeric characters, both uppercase and lowercase,
        /// with numbers.
        /// </summary>
        /// <param name="c">The character to be found</param>
        /// <returns>
        /// 	<c>true</c> if c in the specified character set; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInAlpha(char c)
        {
            bool found = false;

            if (ALPHA.IndexOf(c) > 0)
            {
                found = true;
            }

            return found;
        }

        /// <summary>
        /// Determines whether the character c between alphanumeric characters, both uppercase and lowercase,
        /// with numbers.
        /// </summary>
        /// <param name="c">The character to be found</param>
        /// <returns>
        /// 	<c>true</c> if c in the specified character set; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
        public static bool IsInAlphaEx(char c)
        {
            bool found = false;

            if (EXTENDED_ALPHA.IndexOf(c) > 0)
            {
                found = true;
            }

            return found;
        }

        /// <summary>
        /// Get a random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers.
        /// If 'withSpaces' then the resulting string may contain one or more spaces.
        /// </summary>
        /// <param name="len">The length of the random generated string.</param>
        /// <param name="withSpaces">if set to <c>true</c> the generated string will contain one or more spaces.</param>
        /// <returns>A random generated string.</returns>
        public static string Get(int len, bool withSpaces)
        {

            StringBuilder sb = new StringBuilder(len);
            string setOfChars = ALPHA;
            if (withSpaces)
            {
                setOfChars = ALPHA_SPACE;
            }

            for (int j = 0; j < len; j++)
            {
                char c = setOfChars[seed.Next(setOfChars.Length)];
                sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get a random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers and symbols.
        /// If 'withSpaces' then the resulting string may contain one or more spaces.
        /// </summary>
        /// <param name="len">The length of the random generated string.</param>
        /// <param name="withSpaces">if set to <c>true</c> the generated string will contain one or more spaces.</param>
        /// <returns>A random generated string.</returns>
        public static string GetEx(int len, bool withSpaces)
        {

            StringBuilder sb = new StringBuilder(len);
            string setOfChars = EXTENDED_ALPHA_SPACE;
            if (!withSpaces)
            {
                setOfChars = EXTENDED_ALPHA;
            }

            for (int j = 0; j < len; j++)
            {
                char c = setOfChars[seed.Next(setOfChars.Length)];
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}


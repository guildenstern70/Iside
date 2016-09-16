using System;
using System.Text;

namespace LLCryptoLib.Utils
{
    /// <summary>
    ///     This class generates random strings in various formats.
    /// </summary>
    public static class RandomString
    {
        private const string ALPHA = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string ALPHA_SPACE = "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890";

        private const string EXTENDED_ALPHA =
            "abcdefghijklmnopqrstuvwxyz!@#$%^&*()è+òàìùç©¡£¿\r\nABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;':[]{}_=";

        private const string EXTENDED_ALPHA_SPACE =
            "abcdefghijklmnopqrstuvwxyz !@#$%^&*()è+òàìùç©¡£¿\r\nABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,.;':[]{}_=";

        private static readonly Random seed = new Random();

        /// <summary>
        ///     Get a random string made of 10 alphanumeric characters, both uppercase and lowercase, with numbers.
        /// </summary>
        /// <returns>A random string of 10 alphanumeric characters.</returns>
        public static string Get()
        {
            return Get(10);
        }

        /// <summary>
        ///     Get a random string made of 10 alphanumeric characters, both uppercase and lowercase,
        ///     with symbols and numbers.
        /// </summary>
        /// <returns>A random string 10 alphanumeric characters, both uppercase and lowercase, with symbols and numbers.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase",
             MessageId = "Member")]
        public static string GetEx()
        {
            return GetEx(10);
        }

        /// <summary>
        ///     Get a random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers.
        /// </summary>
        /// <param name="len">The length of the random generated string.</param>
        /// <returns>A random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers.</returns>
        public static string Get(int len)
        {
            return GetEx(len, false);
        }

        /// <summary>
        ///     Get a random string made of 'len' alphanumeric characters, both uppercase and lowercase,
        ///     with symbols and numbers.
        /// </summary>
        /// <param name="len">
        ///     A random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers and
        ///     symbols.
        /// </param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase",
             MessageId = "Member")]
        public static string GetEx(int len)
        {
            return GetEx(len, true);
        }

        /// <summary>
        ///     Determines whether the character c between alphanumeric characters, both uppercase and lowercase,
        ///     with numbers.
        /// </summary>
        /// <param name="c">The character to be found</param>
        /// <returns>
        ///     <c>true</c> if c in the specified character set; otherwise, <c>false</c>.
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
        ///     Determines whether the character c between alphanumeric characters, both uppercase and lowercase,
        ///     with numbers.
        /// </summary>
        /// <param name="c">The character to be found</param>
        /// <returns>
        ///     <c>true</c> if c in the specified character set; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase",
             MessageId = "Member")]
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
        ///     Get a random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers.
        ///     If 'withSpaces' then the resulting string may contain one or more spaces.
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
        ///     Get a random string made of 'len' alphanumeric characters, both uppercase and lowercase, with numbers and symbols.
        ///     If 'withSpaces' then the resulting string may contain one or more spaces.
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
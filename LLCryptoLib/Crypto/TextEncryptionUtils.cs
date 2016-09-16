using System;
using System.IO;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     Utility for text encryption
    /// </summary>
    public sealed class TextEncryptionUtils
    {
        private TextEncryptionUtils()
        {
        }

        /// <summary>
        ///     Convert a MemoryStream to a Base64 representation of bytes
        /// </summary>
        /// <param name="instream">Memory Stream containing bytes</param>
        /// <returns>Base 64 representation of bytes</returns>
        /// <exception cref="ArgumentNullException" />
        public static string MemoryToBase64String(MemoryStream instream)
        {
            if (instream == null)
            {
                throw new ArgumentNullException("instream");
            }

            return Convert.ToBase64String(instream.ToArray());
        }

        /// <summary>
        ///     Returns true if the input string CAN BE a Base64 string.
        ///     Since there is not an exact way to determine if a string is a Base64 string
        ///     and not simple text, this method returns true if the string is 'compliant'
        ///     with Base64, and so it can be a result of a Base64 encoding.
        /// </summary>
        /// <param name="toCheck">A string to check for Base64</param>
        /// <returns>Returns true if the input string CAN BE a Base64 string</returns>
        /// <exception cref="ArgumentNullException" />
        public static bool IsBase64(string toCheck)
        {
            if (toCheck == null)
            {
                throw new ArgumentNullException("toCheck");
            }

            const string b64chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

            // check to see if the string is a well-formed Base64 string

            string legalString = b64chars + "=";

            // check for bad string length (must be a multiple of 4)
            if (toCheck.Length%4 > 0)
            {
                return false;
            }

            // check for illegal characters
            for (int j = 0; j < toCheck.Length; j++)
            {
                if (legalString.IndexOf(toCheck[j]) < 0)
                {
                    return false;
                }
            }

            // make sure any '=' are only at the end

            int whereIsEqual = toCheck.IndexOf('=');

            if (whereIsEqual == -1)
            {
                // ok
            }
            else if (whereIsEqual == toCheck.Length)
            {
                // ok
            }
            else if (whereIsEqual < toCheck.Length)
            {
                // NOT OK
                return false;
            }
            else
            {
                // NOT OK if the last is not equal, but equals are found inside
                if (toCheck[toCheck.Length] != '=')
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Convert a MemoryStream to a clear text string
        /// </summary>
        /// <param name="instream">Memory Stream containing text</param>
        /// <returns>Clear text</returns>
        /// <exception cref="ArgumentNullException" />
        public static string MemoryToString(MemoryStream instream)
        {
            if (instream == null)
            {
                throw new ArgumentNullException("instream");
            }

            return BytesToString(instream.ToArray());
        }

        /// <summary>
        ///     Convert a Base64 string to its bytes
        /// </summary>
        /// <param name="inBase">Base64 inputString</param>
        /// <returns>Bytes in Base64 string</returns>
        public static byte[] Base64StringToBytes(string inBase)
        {
            return Convert.FromBase64String(inBase);
        }

        /// <summary>
        ///     Convert an array of bytes to a Base64 representation of bytes
        /// </summary>
        /// <param name="inBytes">Input array of bytes</param>
        /// <returns>Base 64 representation of bytes</returns>
        public static string BytesToBase64String(byte[] inBytes)
        {
            return Convert.ToBase64String(inBytes);
        }

        /// <summary>
        ///     Convert a string into an array of bytes
        /// </summary>
        /// <param name="instr">String to convert</param>
        /// <returns>Array of bytes</returns>
        public static byte[] StringToBytes(string instr)
        {
            return Config.TextEncoding.GetBytes(instr);
        }

        /// <summary>
        ///     Convert an array of bytes into an hexadecimal string
        /// </summary>
        /// <param name="arrBytes">Array of bytes to convert</param>
        /// <returns>Hexadecimal String</returns>
        public static string BytesToString(byte[] arrBytes)
        {
            return Config.TextEncoding.GetString(arrBytes);
        }
    }
}
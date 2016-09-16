using System;
using System.Globalization;
using System.Text;

namespace LLCryptoLib.Utils
{
    /// <summary>
    ///     Utility class to transform a string or bytes sequence into a hex series.
    /// </summary>
    public static class Hexer
    {
        /// <summary>
        ///     Gets the number of bytes in input hexadecimal string.
        /// </summary>
        /// <param name="hexInput">The hexadecimal string.</param>
        /// <returns>The number of bytes in hex string</returns>
        /// <exception cref="ArgumentNullException" />
        public static int GetNumberOfBytesInHexString(string hexInput)
        {
            if (hexInput == null)
            {
                throw new ArgumentNullException("hexInput");
            }

            int numHexChars = 0;
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexInput.Length; i++)
            {
                c = hexInput[i];
                if (IsHexDigit(c))
                    numHexChars++;
            }
            // if odd number of characters, discard last character
            if (numHexChars%2 != 0)
            {
                numHexChars--;
            }
            return numHexChars/2; // 2 characters per byte
        }

        /// <summary>
        ///     Determines whether the specified char is a hex digit, that is
        ///     it is between 0..9..F
        /// </summary>
        /// <param name="c">The character</param>
        /// <returns>
        ///     <c>true</c> if c is hex digit; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsHexDigit(char c)
        {
            int numChar;
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            c = char.ToUpper(c);
            numChar = Convert.ToInt32(c);
            if ((numChar >= numA) && (numChar < numA + 6))
                return true;
            if ((numChar >= num1) && (numChar < num1 + 10))
                return true;
            return false;
        }

        /// <summary>
        ///     Converts an array of bytes in the corresponding string using UTF8 encoding.
        /// </summary>
        /// <param name="ibytes">The input bytes.</param>
        /// <returns>The string corresponding from UTF8 byte conversion</returns>
        public static string Bytes2Text(byte[] ibytes)
        {
            Encoding enc = Encoding.UTF8;
            return enc.GetString(ibytes);
        }

        /// <summary>
        ///     Creates a byte array from a string using UTF8 encoding.
        /// </summary>
        /// <param name="text">A string to convert to a byte array.</param>
        /// <returns>A byte array resulting from the conversion of the input string with UTF8 encoding</returns>
        public static byte[] Text2Bytes(string text)
        {
            Encoding enc = Encoding.UTF8;
            return enc.GetBytes(text);
        }

        /// <summary>
        ///     Creates a byte array from the hexadecimal string. Each two characters are combined
        ///     to create one byte. First two hexadecimal characters become first byte in returned array.
        ///     Non-hexadecimal characters are ignored.
        /// </summary>
        /// <param name="hexInput">A hexadecimal string to convert to byte array</param>
        /// <returns>A byte array, in the same left-to-right order as the hexString</returns>
        /// <exception cref="ArgumentNullException" />
        public static byte[] Hex2Bytes(string hexInput)
        {
            if (hexInput == null)
            {
                throw new ArgumentNullException("hexInput");
            }

            int discarded = 0;
            StringBuilder newString = new StringBuilder();
            char c;
            // remove all non A-F, 0-9, characters
            for (int i = 0; i < hexInput.Length; i++)
            {
                c = hexInput[i];
                if (IsHexDigit(c))
                {
                    newString.Append(c);
                }
                else
                {
                    discarded++;
                }
            }

            string cleanHexString = newString.ToString();
            // if odd number of characters, discard last character
            if (cleanHexString.Length%2 != 0)
            {
                discarded++;
                cleanHexString = cleanHexString.Substring(0, cleanHexString.Length - 1);
            }

            int byteLength = cleanHexString.Length/2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new string(new[] {cleanHexString[j], cleanHexString[j + 1]});
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }

        /// <summary>
        ///     Transform a string to hex, without dividing character between hexes
        /// </summary>
        /// <param name="text">input string</param>
        /// <returns>Hex version of input string</returns>
        /// <exception cref="ArgumentNullException" />
        public static string Text2Hex(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            int txtlen = text.Length;
            StringBuilder hexstring = new StringBuilder();

            string curHex;
            int tmpchar;

            for (int k = 1; k <= txtlen; k++)
            {
                tmpchar = text[k - 1];
                curHex = tmpchar.ToString("X");
                if (curHex.Length == 1)
                {
                    hexstring.Append('0');
                }
                hexstring.Append(curHex);
            }

            return hexstring.ToString();
        }

        /// <summary>
        ///     Transform a string to hex
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="style">hexadecimal style</param>
        /// <returns>Hex version of input string</returns>
        /// <exception cref="ArgumentNullException" />
        public static string Text2Hex(string text, HexEnum style)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            HexStyler hstyler = new HexStyler(style);
            int txtlen = text.Length;
            StringBuilder hexstring = new StringBuilder();

            string curHex;
            int tmpchar;

            for (int k = 1; k <= txtlen; k++)
            {
                tmpchar = text[k - 1];
                curHex = tmpchar.ToString("X");
                if (curHex.Length == 1)
                {
                    hexstring.Append('0');
                }
                hexstring.Append(curHex);
            }

            return hstyler.Format(hexstring.ToString());
        }

        /// <summary>
        ///     Transform a string to a int number summing
        ///     the ASCII value if its characters
        /// </summary>
        /// <param name="text">input string</param>
        /// <returns>Integer number of the string</returns>
        /// <exception cref="ArgumentNullException" />
        public static int Text2Int(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            int txtlen = text.Length;
            int sum = 0;

            for (int k = 0; k < txtlen; k++)
            {
                sum += text[0];
            }

            return sum;
        }

        /// <summary>
        ///     Transform hex series into the corresponding string
        /// </summary>
        /// <param name="hex">Hex series (ie: 2A 3B 4C FF ...)</param>
        /// <param name="style">Style of the given hex series</param>
        /// <returns>Text string</returns>
        /// <exception cref="ArgumentNullException" />
        public static string Hex2Text(string hex, HexEnum style)
        {
            if (hex == null)
            {
                throw new ArgumentNullException("hex");
            }

            HexStyler hstyler = new HexStyler(style);
            string tmphex = hex.Trim();
            StringBuilder tmpout = new StringBuilder();
            string plainHex = hstyler.ToPlain(tmphex);

            try
            {
                string curhexnumber;
                byte curbyte;
                int c = 0;
                while (c < plainHex.Length)
                {
                    curhexnumber = plainHex.Substring(c, 2);
                    curbyte = byte.Parse(curhexnumber, NumberStyles.HexNumber);
                    tmpout.Append((char) curbyte);
                    c += 2;
                }
            }
            catch (FormatException nf)
            {
                tmpout = new StringBuilder("Error: ");
                tmpout.Append(nf.Message);
            }

            return tmpout.ToString();
        }


        /// <summary>
        ///     Convert a stream of bytes into a string of hexadecimal numbers
        /// </summary>
        /// <param name="inbytes">Array of bytes</param>
        /// <param name="divide">dividing char between hexes</param>
        /// <returns>String of hexadecimal values</returns>
        public static string BytesToHex(byte[] inbytes, char divide)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in inbytes)
            {
                sb.Append(b.ToString("X2"));
                sb.Append(divide);
            }
            return sb.ToString().Trim();
        }


        /// <summary>
        ///     Convert a stream of bytes into a string of hexadecimal numbers, without divisor char.
        /// </summary>
        /// <param name="inbytes">Array of bytes</param>
        /// <param name="style">Hexadecimal style</param>
        /// <returns>String of hexadecimal values</returns>
        public static string BytesToHex(byte[] inbytes, HexEnum style)
        {
            HexStyler hstyler = new HexStyler(style);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in inbytes)
            {
                sb.Append(b.ToString("X2"));
            }

            return hstyler.Format(sb.ToString());
        }

        /// <summary>
        ///     Check if supplied string is a series of hexadecimal numbers.
        ///     The check is made ONLY on the first 20 hex numbers, if these are present.
        /// </summary>
        /// <param name="text">String to verify</param>
        /// <param name="style">Style of the supplied text string</param>
        /// <returns>True, if supplied string appears to be hexadecimal</returns>
        /// <exception cref="ArgumentNullException" />
        public static bool IsHex(string text, HexEnum style)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            HexStyler hstyler = new HexStyler(style);
            bool isHex = true;
            string tmphex = text.Trim();
            tmphex = hstyler.ToPlain(text);
            short count = 0;
            string curhexnumber;

            try
            {
                while (tmphex.Length > 1)
                {
                    if (++count > 20)
                    {
                        break;
                    }

                    curhexnumber = tmphex.Substring(0, 2);
                    int.Parse(curhexnumber, NumberStyles.HexNumber);
                    tmphex = tmphex.Substring(2);
                }
            }
            catch (FormatException)
            {
                isHex = false;
            }

            return isHex;
        }

        /// <summary>
        ///     Converts 1 or 2 character string into equivalant byte value
        /// </summary>
        /// <param name="hex">1 or 2 character string</param>
        /// <returns>byte</returns>
        /// <exception cref="ArgumentException">'hex' must be 1 or 2 characters in length.</exception>
        private static byte HexToByte(string hex)
        {
            if ((hex.Length > 2) || (hex.Length <= 0))
            {
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            }

            byte newByte = byte.Parse(hex, NumberStyles.HexNumber);
            return newByte;
        }
    }
}
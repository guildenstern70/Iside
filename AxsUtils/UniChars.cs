/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved
 * 
 * AxsUtils.Dates.cs
 * 
 */

using System;

namespace AxsUtils
{
    public static class UniChars
    {
        /// <summary>
        /// Gets a Unicode printable char
        /// </summary>
        /// <param name="charNum">A number from 0 to 254</param>
        /// <returns>A printable Unicode char as a string</returns>
        public static string CharAt(int charNum)
        {
            if ((charNum > 255) || (charNum < 0))
            {
                throw new ArgumentException("charNum must be between 0 and 255", "charNum");
            }
            int printableChar = UniChars.CharInt[charNum];
            return Char.ConvertFromUtf32(printableChar);
        }

        private static int[] CharInt
        {
            get
            {
                int[] ints = new int[255];
                int count = 0;
                for (int j = 33; j < 127; j++)
                {
                    ints[count++] = j;
                }
                for (int k = 160; k < 321; k++)
                {
                    ints[count++] = k;
                }
                return ints;
            }
        }
    }
}

using System;
using System.Text;

namespace AxsUtils
{
    /// <summary>
    ///     Converter for roman numbers
    /// </summary>
    public sealed class RomanConverter : ConverterBase
    {
        private static readonly int MAX = 3998;

        public override string ToString(int value)
        {
            if (value > MAX)
                throw new ArgumentOutOfRangeException("value");

            string[,] romanDigits =
            {
                {"M", "C", "X", "I"},
                {"MM", "CC", "XX", "II"},
                {"MMM", "CCC", "XXX", "III"},
                {null, "CD", "XL", "IV"},
                {null, "D", "L", "V"},
                {null, "DC", "LX", "VI"},
                {null, "DCC", "LXX", "VII"},
                {null, "DCCC", "LXXX", "VIII"},
                {null, "CM", "XC", "IX"}
            };

            StringBuilder result = new StringBuilder(15);

            for (int index = 0; index < 4; index++)
            {
                int power = (int) Math.Pow(10, 3 - index);
                int digit = value/power;
                value -= digit*power;
                if (digit > 0)
                    result.Append(romanDigits[digit - 1, index]);
            }

            return result.ToString();
        }

        public override int ToInt(string number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }

            int result = 0;
            int oldValue = 1000;

            for (int index = 0; index < number.Length; index++)
            {
                int newValue = RomanDigit(number[index]);
                if (newValue > oldValue)
                    result = result + newValue - 2*oldValue;
                else
                    result += newValue;

                oldValue = newValue;
            }
            return result;
        }

        private static int RomanDigit(char digit)
        {
            switch (digit)
            {
                case 'I':
                    return 1;
                case 'V':
                    return 5;
                case 'X':
                    return 10;
                case 'L':
                    return 50;
                case 'C':
                    return 100;
                case 'D':
                    return 500;
                case 'M':
                    return 1000;
                default:
                    throw new ArgumentOutOfRangeException("digit");
            }
        }
    }
}
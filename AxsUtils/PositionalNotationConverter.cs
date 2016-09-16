using System;
using System.Text;

namespace AxsUtils
{
    /// <summary>
    ///     Converter for dual positional notations like binary, octal, hexadecimal numbers
    /// </summary>
    public sealed class PositionalNotationConverter : ConverterBase
    {
        private readonly byte bits;
        private readonly string digits;
        private readonly byte radix;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="radix">Radix of the system</param>
        public PositionalNotationConverter(byte radix)
        {
            this.radix = radix;
            this.bits = (byte) Math.Round(Math.Log(radix, 2));
            this.digits = "0123456789ABCDEF".Substring(0, radix);
        }

        public override string ToString(int value)
        {
            StringBuilder result = new StringBuilder(128 >> this.bits);
            do
            {
                result.Insert(0, this.digits[value%this.radix]);
                value = value >> this.bits;
            } while (value != 0);

            return result.ToString();
        }

        public override int ToInt(string number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }

            int result = 0;
            foreach (char digit in number.ToUpper())
            {
                result = result << this.bits;

                int value = this.digits.IndexOf(digit);
                if (value == -1)
                    throw new ArgumentOutOfRangeException("number", number);

                result = result + value;
            }
            return result;
        }
    }
}
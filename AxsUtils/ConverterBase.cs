namespace AxsUtils
{
    /// <summary>
    ///     Abstract Converter class
    /// </summary>
    public abstract class ConverterBase
    {
        private static volatile ConverterBase roman;

        private static volatile ConverterBase hexadecimal;
        private static volatile ConverterBase octal;
        private static volatile ConverterBase binary;

        /// <summary>
        ///     Singleton access a converter for roman numbers
        /// </summary>
        public static ConverterBase Roman
        {
            get
            {
                if (roman == null)
                    roman = new RomanConverter();
                return roman;
            }
        }

        /// <summary>
        ///     Singleton access a converter for hexadecimal numbers
        /// </summary>
        public static ConverterBase Hexadecimal
        {
            get
            {
                if (hexadecimal == null)
                    hexadecimal = new PositionalNotationConverter(16);
                return hexadecimal;
            }
        }

        /// <summary>
        ///     Singleton access a converter for octal numbers
        /// </summary>
        public static ConverterBase Octal
        {
            get
            {
                if (octal == null)
                    octal = new PositionalNotationConverter(8);
                return octal;
            }
        }

        /// <summary>
        ///     Singleton access a converter for octal numbers
        /// </summary>
        public static ConverterBase Binary
        {
            get
            {
                if (binary == null)
                    binary = new PositionalNotationConverter(2);
                return binary;
            }
        }


        /// <summary>
        ///     Convert a integer value into the foreign number system
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>number</returns>
        public abstract string ToString(int value);

        /// <summary>
        ///     Convert from a number system into a integer value
        /// </summary>
        /// <param name="number">number in different system</param>
        /// <returns>Integer-Value</returns>
        public abstract int ToInt(string number);
    }
}
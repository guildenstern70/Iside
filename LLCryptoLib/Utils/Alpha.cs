namespace LLCryptoLib.Utils
{
    /// <summary>
    ///     Utility class to handle alphabetic shifts.
    ///     It defines a standard set of characters, the "Characters" string, which is
    ///     <code>
    /// <![CDATA[ABCDEFGHIJKLMNOPQRSTUVWXYZ{} []()<>,.?;:'+-=@!#$%^&*~`_|/abcdefghijklmnopqrstuvwxyz0123456789]]>
    /// </code>
    ///     and then performs method on the "Characters" string.
    /// </summary>
    public static class Alpha
    {
        /// <summary>
        ///     Get a string with ASCII alphanumeric characters.
        ///     The character string is defined as follows:
        ///     <code>
        /// <![CDATA[ABCDEFGHIJKLMNOPQRSTUVWXYZ{} []()<>,.?;:'+-=@!#$%^&*~`_|/abcdefghijklmnopqrstuvwxyz0123456789]]>
        /// </code>
        /// </summary>
        public static string Characters { get; set; } =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ{} []()<>,.?;:'+-=@!#$%^&*~`_|/abcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        ///     Return the number of characters in Characters string
        /// </summary>
        /// <returns>The number of characters in Characters string</returns>
        public static short CharactersLen
        {
            get { return (short) Characters.Length; }
        }

        /// <summary>
        ///     Return true if character is in Characters string
        /// </summary>
        /// <param name="car">Character to be checked</param>
        /// <returns>True if car is inside the Characters string</returns>
        public static bool IsInAlfa(char car)
        {
            int dove;

            dove = Characters.IndexOf(car);

            if (dove == -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Return the char in Characters at position pos
        /// </summary>
        /// <param name="pos">Position</param>
        /// <returns>Corresponding char in Characters string</returns>
        internal static char GetCharAt(int pos)
        {
            int lenB = Characters.Length;

            //charAt va da 0 a lenB-1
            if (pos > lenB - 1)
            {
                pos -= lenB;
            }
            else if (pos < 0)
            {
                pos += lenB;
            }

            return Characters[pos];
        }

        /// <summary>
        ///     Get position of char 'car' in Characters string
        /// </summary>
        /// <param name="car">Given Character</param>
        /// <returns>Position</returns>
        internal static int GetPosOf(char car)
        {
            return Characters.IndexOf(car);
        }
    }

    /// <summary>
    ///     LLCryptoLib.Utils library contains the utilities
    ///     used throughout the code, such as Input/Output, Password meter,
    ///     Windows registry and so on.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}
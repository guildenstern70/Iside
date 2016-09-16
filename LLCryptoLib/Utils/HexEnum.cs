using System.Runtime.InteropServices;

namespace LLCryptoLib.Utils
{
    /// <summary>
    ///     Hexadecimal representation style.
    ///     Every enum constant represent a style,
    ///     that is a representation of an hexadecimal number.
    /// </summary>
    [ComVisible(true)]
    public enum HexEnum
    {
        /// <summary>
        ///     An unknown hexadecimal style
        /// </summary>
        UNKNOWN = 0,

        /// <summary>
        ///     Classic hexadecimal style: FF12AB4D
        /// </summary>
        CLASSIC = 16,

        /// <summary>
        ///     UNIX hexadecimal style: ff12ab4d
        /// </summary>
        UNIX = 12,

        /// <summary>
        ///     SPACE hexadecimal style: FF 12 AB 4D
        /// </summary>
        SPACE = 14,

        /// <summary>
        ///     Netscape(TM) hexadecimal style: FF:12:AB:4D
        /// </summary>
        NETSCAPE = 18,

        /// <summary>
        ///     Modern hexadecimal style: ff 12 ab 4d
        /// </summary>
        MODERN = 22
    }
}
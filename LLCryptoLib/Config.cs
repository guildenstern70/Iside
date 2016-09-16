using System.Globalization;
using System.Text;

namespace LLCryptoLib
{
    /// <summary>
    ///     Configuration items.
    ///     TextEncoding = Encoding for TextTransformations
    ///     CULTURE = Default Culture info
    ///     NUMBER_FORMAT = Default Number Format
    /// </summary>
    internal static class Config
    {
        /// <summary>
        ///     TextEncoding = Unicode
        /// </summary>
        internal static Encoding TextEncoding = Encoding.Unicode;

        /// <summary>
        ///     The number format in the current culture
        /// </summary>
        internal static NumberFormatInfo NUMBERFORMAT = CultureInfo.CurrentCulture.NumberFormat;
    }

    /// <summary>
    ///     LLCryptoLib is a .NET Framework library which allows programmers to easily add encryption,
    ///     integrity and authentication services to their software. It is compatible with Microsoft .NET
    ///     and Mono environments. It offers both symmetrical and asymmetrical text with stream encryption classes
    ///     and hashing functions. It also offers full file shredding functions and a certificate management
    ///     utilities.
    ///     The library is divided into these namespaces:
    ///     <ul>
    ///         <li>LLCryptoLib.Crypto.<br></br>Contains encryption classes, both for streams and text.</li>
    ///         <li>LLCryptoLib.Hash<br></br>Contains hash code generator classes.</li>
    ///         <li>
    ///             LLCryptoLib.Security<br></br>Contains certificate management classes along with relative cryptographic
    ///             functions.
    ///         </li>
    ///         <li>LLCryptoLib.Shred<br></br>Contains the shredding - secure delete - functions</li>
    ///         <li>LLCryptoLib.Utils<br></br>Contains helping classes such as I/O and Hexadecimal output.</li>
    ///     </ul>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}
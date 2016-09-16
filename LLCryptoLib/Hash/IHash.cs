using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using LLCryptoLib.Utils;

namespace LLCryptoLib.Hash
{
    /// <summary>
    ///     Base interface for all Hash objects.
    /// </summary>
    /// <example>
    ///     <code>
    /// IHash hashObject = new Hash();
    /// hashObject.SetAlgorithm(AvailableHash.MD5);
    /// Console.WriteLine("MD5 hash: {0}", hashObject.ComputeHashFileStyle(fileToHash.FullName, HexStyle.UNIX));
    /// </code>
    /// </example>
    [ComVisible(true)]
    public interface IHash
    {
        /// <summary>
        ///     Set Hashing Function algorithm. It's not a property following COM standards.
        /// </summary>
        /// <param name="algoId">
        ///     Algorithm int ID identifier:
        ///     FAKE = 0,
        ///     ADLER32 = 12,
        ///     CRC32 = 6,
        ///     FCS16 = 19,
        ///     FCS32 = 20,
        ///     GOST = 22,
        ///     MD2 = 7,
        ///     MD4 = 8,
        ///     MD5 = 5,
        ///     SHA1 = 2,
        ///     SHA224 = 21,
        ///     SHA256 = 3,
        ///     SHA384 = 1,
        ///     SHA512 = 4,
        ///     TIGER = 10,
        ///     RIPEMD160 = 9,
        ///     HAVAL128 = 13,
        ///     HAVAL160 = 14,
        ///     HAVAL192 = 15,
        ///     HAVAL224 = 16,
        ///     HAVAL256 = 17,
        ///     HMACSHA1 = 11,
        ///     WHIRLPOOL = 18
        /// </param>
        void SetAlgorithmInt(int algoId);

        /// <summary>
        ///     Set Hashing Function algorithm. It's not a property following COM standards.
        /// </summary>
        /// <param name="sh">Selected Hash Algorithm</param>
        void SetAlgorithm(AvailableHash sh);

        /// <summary>
        ///     Set Hashing Function algorithm. It's not a property following COM standards.
        /// </summary>
        /// <param name="sha">Selected Hash Algorithm</param>
        void SetAlgorithmAlgo(SupportedHashAlgo sha);

        /// <summary>
        ///     Set hashing algorithm - keyed hash algorithm
        ///     <see cref="SupportedHashAlgo" />
        /// </summary>
        /// <param name="ha">Supported Algorithms</param>
        /// <param name="key">Vigenere for keyed hash algorithm</param>
        void SetAlgorithmBytes(AvailableHash ha, byte[] key);

        /// <summary>
        ///     Compute hash function of a text string
        /// </summary>
        /// <param name="message">Text string</param>
        /// <param name="textEncoding">The text encoding (ie: UTF8, ASCII).</param>
        /// <returns>Hash as a sequence of hexadecimal characters</returns>
        string ComputeHash(string message, Encoding textEncoding);

        /// <summary>
        ///     Compute Hash of given string
        /// </summary>
        /// <param name="message">Message to compute the hash for</param>
        /// <param name="divide">Char to divide hexes.</param>
        /// <param name="textEncoding">The text encoding (ie: UTF8, ASCII).</param>
        /// <returns>Hash string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase",
             MessageId = "Member")]
        string ComputeHashEx(string message, char divide, Encoding textEncoding);

        /// <summary>
        ///     Compute Hash of given string
        /// </summary>
        /// <param name="message">Message to compute the hash for</param>
        /// <param name="style">Style of displaying hex numbers</param>
        /// <param name="textEncoding">The text encoding (ie: UTF8, ASCII).</param>
        /// <returns>Hash string</returns>
        string ComputeHashStyle(string message, HexEnum style, Encoding textEncoding);

        /// <summary>
        ///     Computes the hash from a set of bytes and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="contents">The contents as a set of bytes.</param>
        /// <returns>Hash</returns>
        string ComputeHashFromBytes(byte[] contents);

        /// <summary>
        ///     Computes the hash from a set of bytes and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="contents">The contents as a set of bytes.</param>
        /// <param name="style">The hexadecimal style.</param>
        /// <returns>Hash</returns>
        string ComputeHashFromBytesStyle(byte[] contents, HexEnum style);

        /// <summary>
        ///     Compute hash function of a file
        /// </summary>
        /// <param name="filePath">File to get the hash for</param>
        /// <returns>Hash as a sequence of hexadecimale characters</returns>
        string ComputeHashFile(string filePath);

        /// <summary>
        ///     Compute Hash of given file.
        /// </summary>
        /// <param name="filePath">Absolute path to a file</param>
        /// <param name="style">Style of displaying hex numbers</param>
        /// <returns>Hash</returns>
        string ComputeHashFileStyle(string filePath, HexEnum style);

        /// <summary>
        ///     Compute Hash of given file.
        /// </summary>
        /// <param name="completeFileName">Absolute path to a file</param>
        /// <param name="style">Style of displaying hex numbers</param>
        /// <param name="cbe">Delegate to be reported of operation advancement</param>
        /// <param name="resetDemand">A reset event to call when the operation must be cancelled</param>
        /// <returns>Hash</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase",
             MessageId = "Member")]
        string ComputeHashFileStyleEx(string completeFileName, HexEnum style, CallbackEntry cbe,
            AutoResetEvent resetDemand);

        /// <summary>
        ///     Compute Hash of given file. The given file must be a text file.
        /// </summary>
        /// <param name="completeFileName">Absolute path to a file</param>
        /// <param name="enc">Text encoding</param>
        /// <returns>Hash</returns>
        string ComputeHashTextFile(string completeFileName, Encoding enc);

        /// <summary>
        ///     Compute a single hash from more than one file
        /// </summary>
        /// <param name="files">The array of files to be computed</param>
        /// <param name="style">A style for the Hash code in output</param>
        /// <param name="cbe">A callback delegate function for feedback purposes, ie: ProgressBar</param>
        /// <param name="resetDemand">A reset event to call when the operation must be cancelled</param>
        /// <returns>The computed hash as a string</returns>
        string ComputeHashFiles(FileInfo[] files, HexEnum style, CallbackEntry cbe, AutoResetEvent resetDemand);

        /// <summary>
        ///     Copy hash of a file to the system clipboard
        /// </summary>
        /// <param name="filePath">File to get the hash for</param>
        void CopyHashFile(string filePath);

        /// <summary>
        ///     Compare the hash of  given file with the one in the clipboard.
        /// </summary>
        /// <param name="filePath">File to get the hash for</param>
        /// <returns>True if the hash of file is the same as the one in the clipboard</returns>
        bool CompareHashClipboard(string filePath);

        /// <summary>
        ///     Compare the hash of  given file with the one in the clipboard.
        ///     The hash in the clipboard must be in the form: FILEPATH+"::"+HASH
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>A detailed feedback of the operation</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase",
             MessageId = "Member")]
        string CompareHashClipboardEx(string filePath);

        /// <summary>
        ///     Compare the hash of  given file with the one given.
        /// </summary>
        /// <param name="filePath">File to get the hash for</param>
        /// <param name="hashToCompare">Hash string to be compared</param>
        /// <returns>True if the hash of file is the same as hashToCompare string</returns>
        bool CompareHash(string filePath, string hashToCompare);

        /// <summary>
        ///     Get the full library path (without dll name, without ending backslash)
        /// </summary>
        /// <returns>Library path (without dll name, without ending backslash)</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        string GetLibraryPath();
    }
}
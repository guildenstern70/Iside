using LLCryptoLib.Utils;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     A TextCrypter object is used to encrypt or decrypt strings.
    /// </summary>
    /// <example>
    ///     <code>
    ///  
    ///   // TextROT13 TEXT TRANSFORMATION
    ///   TextAlgorithmParameters parms = new TextAlgorithmParameters(3);
    ///   TextCrypter textEncrypter = TextCrypterFactory.Create(SupportedTextAlgorithms.ROT13,parms);
    ///   string encrypted = textEncrypter.TextEncryptDecrypt(origString, true);
    ///   Console.WriteLine("Encrypted string: " + encrypted);
    ///   string decrypted = textEncrypter.TextEncryptDecrypt(encrypted, false);
    ///   Console.WriteLine("Decrypted string: " + decrypted);
    /// 	 Console.WriteLine(); 
    ///  </code>
    /// </example>
    public class TextCrypter
    {
        private readonly TextAlgorithm mtd; // Encryption Algorithm

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="algorithm">Encryption or decryption algorithm</param>
        internal TextCrypter(TextAlgorithm algorithm)
        {
            this.mtd = algorithm;
        }

        /// <summary>
        ///     Apply encryption algorithm to text. Output is a text string.
        /// </summary>
        /// <param name="text">Text to encode/decode</param>
        /// <param name="coding">If true, code, else decode</param>
        /// <returns>Encrypted/Decrypted Text</returns>
        /// <example>
        ///     <code>
        ///  
        ///          // TextROT13 TEXT TRANSFORMATION
        ///         TextAlgorithmParameters parms = new TextAlgorithmParameters(3);
        ///         TextCrypter textEncrypter = TextCrypterFactory.Create(SupportedTextAlgorithms.ROT13,parms);
        ///         string encrypted = textEncrypter.TextEncryptDecrypt(origString, true);
        ///         Console.WriteLine("Encrypted string: " + encrypted);
        ///         string decrypted = textEncrypter.TextEncryptDecrypt(encrypted, false);
        ///         Console.WriteLine("Decrypted string: " + decrypted);
        /// 		   Console.WriteLine(); 
        ///  
        ///  </code>
        /// </example>
        public string TextEncryptDecrypt(string text, bool coding)
        {
            TextCryptEngine crypter = Transform(text);
            return crypter.Transform(this.mtd, coding);
        }

        /// <summary>
        ///     Apply encryption algorithm to text. Output is a Base64 text string.
        /// </summary>
        /// <param name="text">Text to encode/decode</param>
        /// <param name="coding">If true, code, else decode</param>
        /// <returns>Encrypted/Decrypted Text</returns>
        public string Base64EncryptDecrypt(string text, bool coding)
        {
            TextCryptEngine crypter = Transform(text);
            return crypter.Transform(this.mtd, coding);
        }

        /// <summary>
        ///     Apply encryption algorithm to text. Output is in hex numbers
        /// </summary>
        /// <param name="text">Text to encode/decode</param>
        /// <param name="coding">If true, code, else decode</param>
        /// <param name="style">Hexadecimal style to use</param>
        /// <returns>Encrypted/Decrypted Text</returns>
        public string HexEncryptDecrypt(string text, bool coding, HexEnum style)
        {
            TextCryptEngine crypter = Transform(text);
            return crypter.HexTransform(this.mtd, coding, style);
        }

        /// <summary>
        ///     The description of algorithm and its paramters
        /// </summary>
        /// <returns>The description of algorithm and its paramters</returns>
        public override string ToString()
        {
            return this.mtd.ToString();
        }

        private static TextCryptEngine Transform(string text)
        {
            return new TextCryptEngine(text);
        }
    }
}
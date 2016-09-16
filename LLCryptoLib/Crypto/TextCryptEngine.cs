using System;
using LLCryptoLib.Utils;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     A TextCryptEngine is a transformation box that takes in input
    ///     a string and returns a transformed string in output.
    ///     The main method that do this is Transform(algorithm, crypt/decrypt).
    ///     The engine applies algorithms by polymorphically apply the
    ///     Code and Decode methods of an ITextEncryption object.
    /// </summary>
    internal class TextCryptEngine
    {
        private string pText;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="text">Text to crypt/decrypt</param>
        internal TextCryptEngine(string text)
        {
            this.pText = text;
        }

        /// <summary>
        ///     Encrypt/decrypt a string, given a crypto method, into a scrambled text string.
        ///     If the method is a text method (TextROT13, TextPlayfair, TextVigenere...) the coded
        ///     string is text. If the method is a byte method (DES, Rijndael) the coded string
        ///     is a Base64 string.
        /// </summary>
        /// <param name="mtd">Crypto method (shift, key, des...)</param>
        /// <param name="codeDecode">If true, code; else, decode</param>
        /// <returns>Transformed string</returns>
        internal string Transform(TextAlgorithm mtd, bool codeDecode)
        {
            string tmp;

            if (codeDecode)
            {
                tmp = mtd.Code(this.pText);
            }
            else
            {
                tmp = mtd.Decode(this.pText);
            }

            return tmp;
        }

        /// <summary>
        ///     Encrypt or decrypt a string, given a crypto method, into a series of hex numbers
        /// </summary>
        /// <param name="algo">Crypto method (shift, key, des...)</param>
        /// <param name="codeDecode">If true, code; else, decode</param>
        /// <param name="style">Style of hexadecimal output</param>
        /// <returns>Transformed string</returns>
        internal string HexTransform(TextAlgorithm algo, bool codeDecode, HexEnum style)
        {
            string base64String;
            string output = null;

            if (codeDecode)
            {
                // Encode to base64
                base64String = this.Transform(algo, codeDecode);
                if (algo.AlgorithmType == TextAlgorithmType.BINARY)
                {
                    // Transform base64 into bytes
                    byte[] bytes = TextEncryptionUtils.Base64StringToBytes(base64String);
                    // Hex output
                    output = Hexer.BytesToHex(bytes, style);
                }
            }
            else
            {
                // pText is a Hex string: turn into bytes, then into bas64
                if (algo.AlgorithmType == TextAlgorithmType.BINARY)
                {
                    byte[] bytes = Hexer.Hex2Bytes(this.pText);
                    this.pText = Convert.ToBase64String(bytes);
                }

                output = this.Transform(algo, codeDecode);
            }
            return output;
        }
    }
}
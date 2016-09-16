namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     A TextCryptFactory is a factory class for TextCrypter object.
    ///     The user must supply an algorithm ID and some parameters and he gets
    ///     an initialized TextCrypter object
    /// </summary>
    /// <example>
    ///     <code>
    ///  
    ///  // TextROT13 TEXT TRANSFORMATION
    ///  TextAlgorithmParameters parms = new TextAlgorithmParameters(3);
    ///  TextCrypter textEncrypter = TextCrypterFactory.Create(SupportedTextAlgorithms.ROT13,parms);
    ///  string encrypted = textEncrypter.TextEncryptDecrypt(origString, true);
    ///  Console.WriteLine("Encrypted string: " + encrypted);
    ///  string decrypted = textEncrypter.TextEncryptDecrypt(encrypted, false);
    ///  Console.WriteLine("Decrypted string: " + decrypted);
    /// 	Console.WriteLine(); 
    ///  
    ///  </code>
    /// </example>
    public sealed class TextCrypterFactory
    {
        private TextCrypterFactory()
        {
        }

        /// <summary>
        ///     TextCrypter factory method.
        ///     For each SupportedTextAlgorithm, these parameters are taken into account:
        ///     <list type="bullet">
        ///         <item>TextROT13</item><description>parameters.Shift (rot13 shift)</description>
        ///         <item>POLYALPHABETIC</item><description>parameters.Key and parameters.Shift</description>
        ///         <item>PLAYFAIR</item><description>No parameter neeeded. You can pass parameters=null</description>
        ///         <item>PSEUDODES</item><description>parameters.Key and parameters.Shift</description>
        ///         <item>DES</item>
        ///         <description>parameters.Key as password. KeyLen and BlockLen are turned to 8 in any case.</description>
        ///         <item>TRIPLEDES</item>
        ///         <description>parameters.Key as password. KeyLen 16 or 24. BlockLen is always 8.</description>
        ///         <item>RIJNDAEL</item>
        ///         <description>
        ///             Allowed values for parameters.keylen is 16,24,32.Allowed values for parameters.blocklen are
        ///             16,24,32. If these values are not specified, then 16-16 is taken.
        ///         </description>
        ///     </list>
        ///     <see cref="TextAlgorithmParameters" />
        /// </summary>
        /// <param name="algorithm">The SupportedTextAlgorithms ID for the algorithm to be used</param>
        /// <param name="parameters">A set of parameters</param>
        /// <returns>An initialized TextCrypter object</returns>
        /// <exception cref="LLCryptoLibException" />
        public static TextCrypter Create(SupportedTextAlgorithms algorithm, TextAlgorithmParameters parameters)
        {
            TextAlgorithmParameters p = parameters;
            TextAlgorithm mtd;

            switch (algorithm)
            {
                case SupportedTextAlgorithms.ROT13:
                    mtd = new TextROT13(p);
                    System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT, "Using Caesar with {0} shift",
                        p.Shift));
                    break;

                case SupportedTextAlgorithms.POLYALPHABETIC:
                    mtd = new TextVigenere(p);
                    System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT,
                        "Using Vigenere with {0} key and {1} shift", p.Key, p.Shift));
                    break;

                case SupportedTextAlgorithms.PLAYFAIR:
                    mtd = new TextPlayfair();
                    System.Diagnostics.Debug.WriteLine("Using Playfair");
                    break;

                case SupportedTextAlgorithms.PSEUDODES:
                    mtd = new TextPseudoDes(p);
                    System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT,
                        "Using Polyalphabetical with {0} key for {1} times", p.Key, p.Shift));
                    break;

                case SupportedTextAlgorithms.DES:
                    mtd = new TextDES(p);
                    System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT, "Using DES at 64 bit"));
                    break;

                case SupportedTextAlgorithms.TRIPLEDES:
                    mtd = new TextTripleDES(p);
                    System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT,
                        "Using 3DES with {0} keylen and {1} blocklen", p.KeySize, p.BlockSize));
                    break;

                case SupportedTextAlgorithms.RIJNDAEL:
                    mtd = new TextRijndael(p);
                    System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT,
                        "Using RIJNDAEL with {0} keylen and {1} blocklen", p.KeySize, p.BlockSize));
                    break;

                default:
                    throw new LLCryptoLibException("Unsupported text encryption method");
            }

            return new TextCrypter(mtd);
        }
    }
}
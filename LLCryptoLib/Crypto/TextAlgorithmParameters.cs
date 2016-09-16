using LLCryptoLib.Utils;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     Class to share parameters between different crypto methods.
    /// </summary>
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
    public class TextAlgorithmParameters
    {
        /// <summary>
        ///     Constructor with shift.
        /// </summary>
        /// <param name="shift">Shift for caesar method (IE: shift=3, A+shift=D)</param>
        public TextAlgorithmParameters(int shift)
        {
            this.Shift = shift;
        }

        /// <summary>
        ///     Constructor with key
        /// </summary>
        /// <param name="key">key-string for single key</param>
        public TextAlgorithmParameters(string key)
        {
            this.Key = key;
        }

        /// <summary>
        ///     Constructor with key and shift
        /// </summary>
        /// <param name="shift">Shift</param>
        /// <param name="key">TextVigenere</param>
        public TextAlgorithmParameters(int shift, string key)
        {
            this.Key = key;
            this.Shift = shift;
        }


        /// <summary>
        ///     Constructor with key and shift
        /// </summary>
        /// <param name="shift">Shift</param>
        /// <param name="key">TextVigenere</param>
        /// <param name="keyBlock">Block of the key in bytes</param>
        /// <param name="keyBytes">Size of the key in bytes</param>
        public TextAlgorithmParameters(int shift, string key, short keyBytes, short keyBlock)
        {
            this.Key = key;
            this.Shift = shift;
            this.BlockSize = keyBlock;
            this.KeySize = keyBytes;
        }

        /// <summary>
        ///     Size of the key in bytes
        /// </summary>
        public short KeySize { get; }

        /// <summary>
        ///     Size of the block in bytes
        /// </summary>
        public short BlockSize { get; }

        /// <summary>
        ///     Get Shift
        /// </summary>
        public int Shift { get; }

        /// <summary>
        ///     Get TextVigenere
        /// </summary>
        public string Key { get; }

        /// <summary>
        ///     Get the shift given the position in the alphabetic sequence
        /// </summary>
        /// <param name="pos">Position in ALFABETO const string</param>
        /// <returns>Shift</returns>
        public int GetShiftAt(int pos)
        {
            int jpos = pos%this.Key.Length;
            char ch = this.Key[jpos];
            return Alpha.GetPosOf(ch);
        }
    }
}
namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     Supported Text Encryption Algorithms
    /// </summary>
    public enum SupportedTextAlgorithms
    {
        /// <summary>
        ///     TextROT13 (Caesar)
        /// </summary>
        ROT13,

        /// <summary>
        ///     Polyalphabetic
        /// </summary>
        POLYALPHABETIC,

        /// <summary>
        ///     PseudoDES
        /// </summary>
        PSEUDODES,

        /// <summary>
        ///     TextPlayfair
        /// </summary>
        PLAYFAIR,

        /// <summary>
        ///     Rijndael or AES
        /// </summary>
        RIJNDAEL,

        /// <summary>
        ///     DES
        /// </summary>
        DES,

        /// <summary>
        ///     TripleDES or 3DES
        /// </summary>
        TRIPLEDES
    }
}
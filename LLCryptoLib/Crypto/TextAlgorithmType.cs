namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     The type of text algorithm
    /// </summary>
    public enum TextAlgorithmType
    {
        /// <summary>
        ///     This algorithm works by scrambling alphabetic letters and numbers
        /// </summary>
        TEXT,

        /// <summary>
        ///     This algorithm works by scrambling bytes. String representations are handled by Base64 conversions
        /// </summary>
        BINARY
    }
}
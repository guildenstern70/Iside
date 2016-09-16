namespace LLCryptoLib.Hash
{
    /// <summary>The order in which to store the bytes for integers.</summary>
    public enum EndianType
    {
        /// <summary>The Least Significant Byte is first.</summary>
        LittleEndian,

        /// <summary>The Most Significant Byte is first.</summary>
        BigEndian
    }
}
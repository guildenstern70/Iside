namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     IShredMethod.
    /// </summary>
    public interface IShredMethod
    {
        /// <summary>
        ///     Shredding method Name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Number of passes
        /// </summary>
        int Passes { get; }

        /// <summary>
        ///     Available Shred Enum
        /// </summary>
        AvailableShred Id { get; }

        /// <summary>
        ///     Detailed description
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Sequence of bytes to be written
        /// </summary>
        /// <returns>The sequence of bytes of this shredding method</returns>
        byte[] GetSequence();
    }
}
namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     Available Shredding Methods
    /// </summary>
    public enum AvailableShred
    {
        /// <summary>
        ///     No Shred
        /// </summary>
        NOTHING = 0,

        /// <summary>
        ///     Single Overwrite Shred
        /// </summary>
        SIMPLE,

        /// <summary>
        ///     Complex Shred
        /// </summary>
        COMPLEX,

        /// <summary>
        ///     Random Shred
        /// </summary>
        RANDOM,

        /// <summary>
        ///     HMG Infosec Standard 5 Enhanced
        /// </summary>
        HMGIS5ENH,

        /// <summary>
        ///     German VSITR Shred
        /// </summary>
        GERMAN,

        /// <summary>
        ///     Department Of Defence Shred
        /// </summary>
        DOD,

        /// <summary>
        ///     Gutmann Shred
        /// </summary>
        GUTMANN
    }

    /// <summary>
    ///     LLCryptoLib.Shred library contains the functions to wipe
    ///     a file from the disk, using a variety of standard algorothms,
    ///     such Gutmann or DoD.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}
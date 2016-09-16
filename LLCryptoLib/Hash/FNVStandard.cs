namespace LLCryptoLib.Hash
{
    /// <summary>Predefined standards for FNV algorithms.</summary>
    public enum FNVStandard
    {
        /// <summary>A 32bit FNV-0 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] FNV0_32,

        /// <summary>A 64bit FNV-0 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] FNV0_64,

        /// <summary>A 32bit FNV-1 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] FNV1_32,

        /// <summary>A 64bit FNV-1 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] FNV1_64,

        /// <summary>A 32bit FNV-1a algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] FNV1A_32,

        /// <summary>A 64bit FNV-1a algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] FNV1A_64
    }
}
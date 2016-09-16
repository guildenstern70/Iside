namespace LLCryptoLib.Hash
{
    /// <summary>Predefined standard parameters for GHash algorithms.</summary>
    public enum GHashStandard
    {
        /// <summary>GHash with a shift of 3 bits.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] GHash_3,

        /// <summary>GHash with a shift of 5 bits.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] GHash_5
    }
}
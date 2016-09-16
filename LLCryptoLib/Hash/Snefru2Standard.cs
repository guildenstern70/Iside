namespace LLCryptoLib.Hash
{
    /// <summary>Predefined standard parameters for Snefru2 algorithms.</summary>
    public enum Snefru2Standard
    {
        /// <summary>Four passes with a 128bit result hash.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] Snefru2_4_128,

        /// <summary>Four passes with a 256bit result hash.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] Snefru2_4_256,

        /// <summary>Eight passes with a 128bit result hash.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] Snefru2_8_128,

        /// <summary>Eight passes with a 256bit result hash.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] Snefru2_8_256
    }
}
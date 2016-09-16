namespace LLCryptoLib.Hash
{
    /// <summary>Predefined standards for CRC algorithms.</summary>
    public enum CRCStandard
    {
        /// <summary>The standard CRC8 algorithm.</summary>
        CRC8,

        /// <summary>The IBM standard CRC16 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] CRC16_IBM,

        /// <summary>The CCITT standard CRC16 algorithm. Used in things such as X.25, SDLC, and HDLC.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] CRC16_CCITT,

        /// <summary>A variation on the CRC16 algorithm. Used in ARC.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] CRC16_ARC,

        /// <summary>A variation on the CRC16 algorithm. Used in XMODEM.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] CRC16_XMODEM,

        /// <summary>A variation on the CRC16 algorithm. Used in ZMODEM.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] CRC16_ZMODEM,

        /// <summary>The standard CRC24 algorithm. Used in things such as PGP.</summary>
        CRC24,

        /// <summary>The standard CRC32 algorithm. Used in things such as PKZip, SFV, AUTODIN II, Ethernet, and FDDI.</summary>
        CRC32,

        /// <summary>A variation on the CRC32 algorithm. Used in JAMCRC.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] CRC32_JAMCRC,

        /// <summary>A variation on the CRC32 algorithm. Used in BZip2.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] CRC32_BZIP2,

        /// <summary>The ISO standard CRC64 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] CRC64_ISO,

        /// <summary>The ECMA standard CRC64 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
             "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")] CRC64_ECMA
    }
}
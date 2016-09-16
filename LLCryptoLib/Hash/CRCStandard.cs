/*
 * LLCryptoLib - Advanced .NET Encryption and Hashing Library
 * v.$id$
 * 
 * The contents of this file are subject to the license distributed with
 * the package (the License). This file cannot be distributed without the 
 * original LittleLite Software license file. The distribution of this
 * file is subject to the agreement between the licensee and LittleLite
 * Software.
 * 
 * Customer that has purchased Source Code License may alter this
 * file and distribute the modified binary redistributables with applications. 
 * Except as expressly authorized in the License, customer shall not rent,
 * lease, distribute, sell, make available for download of this file. 
 * 
 * This software is not Open Source, nor Free. Its usage must adhere
 * with the License obtained from LittleLite Software.
 * 
 * The source code in this file may be derived, all or in part, from existing
 * other source code, where the original license permit to do so.
 * 
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */


using System;

namespace LLCryptoLib.Hash
{
    /// <summary>Predefined standards for CRC algorithms.</summary>
    public enum CRCStandard
    {
        /// <summary>The standard CRC8 algorithm.</summary>
        CRC8,

        /// <summary>The IBM standard CRC16 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        CRC16_IBM,

        /// <summary>The CCITT standard CRC16 algorithm. Used in things such as X.25, SDLC, and HDLC.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        CRC16_CCITT,

        /// <summary>A variation on the CRC16 algorithm. Used in ARC.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        CRC16_ARC,

        /// <summary>A variation on the CRC16 algorithm. Used in XMODEM.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        CRC16_XMODEM,

        /// <summary>A variation on the CRC16 algorithm. Used in ZMODEM.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        CRC16_ZMODEM,

        /// <summary>The standard CRC24 algorithm. Used in things such as PGP.</summary>
        CRC24,

        /// <summary>The standard CRC32 algorithm. Used in things such as PKZip, SFV, AUTODIN II, Ethernet, and FDDI.</summary>
        CRC32,

        /// <summary>A variation on the CRC32 algorithm. Used in JAMCRC.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        CRC32_JAMCRC,

        /// <summary>A variation on the CRC32 algorithm. Used in BZip2.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        CRC32_BZIP2,

        /// <summary>The ISO standard CRC64 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        CRC64_ISO,

        /// <summary>The ECMA standard CRC64 algorithm.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
        CRC64_ECMA,
    }
}

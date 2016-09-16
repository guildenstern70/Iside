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
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */


using System;

namespace LLCryptoLib.Shred
{
    /// <summary>
    /// German VSITR shred method.
    /// This shred method overwrites the file area with 0's, 1's and finally a random byte.
    /// </summary>
    public sealed class ShredGermanVSITR : ShredBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShredGermanVSITR"/> class.
        /// </summary>
        public ShredGermanVSITR()
        {
            this.bitSequence = new byte[7] { 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x41 };
            this._passes = 7;
        }

        /// <summary>
        /// Shredding method Name
        /// </summary>
        /// <value></value>
        public override string Name
        {
            get { return LLCryptoLib.Strings.S00016; }
        }

        /// <summary>
        /// Shredding method Available Shred Enum
        /// </summary>
        /// <value></value>
        public override AvailableShred Id
        {
            get { return AvailableShred.GERMAN; }
        }

        /// <summary>
        /// Shredding method Detailed description
        /// </summary>
        /// <value></value>
        public override string Description
        {
            get { return LLCryptoLib.Strings.S00017; }
        }
    }
}

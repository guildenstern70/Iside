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

namespace LLCryptoLib.Shred
{
    /// <summary>
    /// Rewrites file area one time with 0x00 byte
    /// </summary>
    public sealed class ShredSimple : ShredBase
    {
        /// <summary>
        /// ShredSimple constructor
        /// </summary>
        public ShredSimple()
        {
            this.bitSequence = new byte[1] { 0x00 };
            this._passes = 1;
        }

        #region IShredMethod Members

        /// <summary>
        /// Shred Method Name
        /// </summary>
        public override string Name
        {
            get
            {
                return Strings.S00004;
            }
        }

        /// <summary>
        /// Shred Enum ID
        /// </summary>
        public override AvailableShred Id
        {
            get
            {
                return AvailableShred.SIMPLE;
            }
        }

        /// <summary>
        /// Return "Overwrite the file area with a series of 0 (0x00) bits"
        /// </summary>
        public override string Description
        {
            get
            {
                return Strings.S00005;
            }
        }

        #endregion

    }
}

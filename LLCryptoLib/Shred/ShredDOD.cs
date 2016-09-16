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
    /// Rewrites the file area 7 times with 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xF6 bytes.
    /// </summary>
    public sealed class ShredDOD : ShredBase
    {
        /// <summary>
        /// Shred DOD (Department of Defense) constructor
        /// </summary>
        public ShredDOD()
        {
            this.bitSequence = new byte[7] { 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xF6 };
            this._passes = 7;
        }

        #region IShredMethod Members

        /// <summary>
        /// Shred Method Name
        /// </summary>
        public override string Name
        {
            get
            {
                return Strings.S00010;
            }
        }

        /// <summary>
        /// Shred Enum ID
        /// </summary>
        public override AvailableShred Id
        {
            get
            {
                return AvailableShred.DOD;
            }
        }

        /// <summary>
        /// Returns "Three iterations completely overwrite the file area six times. Each iteration makes two write-passes over the entire drive: the first pass inscribes ONEs (1) and the next pass inscribes ZEROes (0). After the third iteration, a seventh pass writes the government-designated code 246 across the drive (in hex 0xF6)"
        /// </summary>
        public override string Description
        {
            get
            {
                return Strings.S00011;
            }
        }


        #endregion
    }
}

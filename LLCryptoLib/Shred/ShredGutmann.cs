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
    /// Rewrites the file area 35 times with 0x34, 0x12, 0x1B, 0x00, 0x55, 0xAA, 0x24, 0x92, 
    /// 0x49, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 
    /// 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 
    /// 0xFF, 0x24, 0x92, 0x49, 0xDB, 0x6D, 0xB6, 0x12, 
    /// 0xFF, 0x82, 0x9A
    /// </summary>
    public sealed class ShredGutmann : ShredBase
    {

        /// <summary>
        /// Gutmann constructor
        /// </summary>
        public ShredGutmann()
        {
            Random rnd = new Random();

            this.bitSequence = new byte[35] { 0x34, 0x12, 0x1B, 0x00, 0x55, 0xAA, 0x24, 0x92, 
												  0x49, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 
												  0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 
												  0xFF, 0x24, 0x92, 0x49, 0xDB, 0x6D, 0xB6, 0x12, 
												  0xFF, 0x82, 0x9A };

            byte[] initialSequence = new byte[4];
            byte[] endingSequence = new byte[4];

            rnd.NextBytes(initialSequence);
            rnd.NextBytes(endingSequence);

            this._passes = 35;

            for (int j = 0; j < 4; j++)
            {
                this.bitSequence[0 + j] = initialSequence[j];
                this.bitSequence[this._passes - 4 + j] = endingSequence[j];
            }

            
        }

        #region IShredMethod Members

        /// <summary>
        /// Shred Method Name
        /// </summary>
        public override string Name
        {
            get
            {
                return Strings.S00012;
            }
        }

        /// <summary>
        /// Shred Enum ID
        /// </summary>
        public override AvailableShred Id
        {
            get
            {
                return AvailableShred.GUTMANN;
            }
        }

        /// <summary>
        /// Return "Overwrite the file area following the Gutmann standard recommendations."
        /// </summary>
        public override string Description
        {
            get
            {
                return Strings.S00013;
            }
        }

        #endregion

    }
}

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
    /// Base class for shred methods
    /// </summary>
    public abstract class ShredBase : IShredMethod
    {
        /// <summary>
        /// Shred method passes. IE: Complex method has 3 passes.
        /// </summary>
        protected int _passes;

        /// <summary>
        /// Shred method bit sequence
        /// </summary>
        protected byte[] bitSequence;

        /// <summary>
        /// Shredding method Name
        /// </summary>
        /// <value></value>
        public abstract string Name { get; }

        /// <summary>
        /// Shredding method Available Shred Enum
        /// </summary>
        /// <value></value>
        public abstract AvailableShred Id { get; }

        /// <summary>
        /// Shredding method Detailed description
        /// </summary>
        /// <value></value>
        public abstract string Description { get; }

        /// <summary>
        /// Returns method passes
        /// </summary>
        public int Passes
        {
            get
            {
                return this._passes;
            }
        }

        /// <summary>
        /// Returns the DOD byte sequence: 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xF6
        /// </summary>
        public byte[] GetSequence()
        {
            return this.bitSequence;
        }

        /// <summary>
        /// Returns the method description string.
        /// </summary>
        /// <returns>Returns the method description string, ie:"DoD 5220-22M"</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}

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
    /// <summary>A class that contains the parameters necessary to initialize a Snefru2 algorithm.</summary>
    public class Snefru2Parameters : HashAlgorithmParameters
    {
        private short passes;
        private short length;


        /// <summary>Gets or sets the number of passes.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public short Passes
        {
            get { return passes; }
            set
            {
                if ((value != 4) && (value != 8))
                {
                    throw new ArgumentException("The number of passes can only be 4 or 8.", "Passes");
                }
                else
                {
                    passes = value;
                }
            }
        }

        /// <summary>Gets or sets the bit length.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public short Length
        {
            get { return length; }
            set
            {
                if ((value != 128) && (value != 256))
                {
                    throw new ArgumentException("The Snefru2 bit length can only be 128 or 256 bits long.", "Length");
                }
                else
                {
                    length = value;
                }
            }
        }


        /// <summary>Initializes a new instance of the Snefru2Paramters class.</summary>
        /// <param name="passes">How many transformation passes to do.</param>
        /// <param name="length">The bit length of the final hash.</param>
        public Snefru2Parameters(short passes, short length)
        {
            this.Passes = passes;
            this.Length = length;
        }


        /// <summary>Retrieves a standard set of Snefru2 parameters.</summary>
        /// <param name="standard">The name of the standard parameter set to retrieve.</param>
        /// <returns>The Snefru2 Parameters for the given standard.</returns>
        public static Snefru2Parameters GetParameters(Snefru2Standard standard)
        {
            Snefru2Parameters temp;

            switch (standard)
            {
                case Snefru2Standard.Snefru2_4_128: temp = new Snefru2Parameters(4, 128); break;
                case Snefru2Standard.Snefru2_4_256: temp = new Snefru2Parameters(4, 256); break;
                case Snefru2Standard.Snefru2_8_128: temp = new Snefru2Parameters(8, 128); break;
                case Snefru2Standard.Snefru2_8_256: temp = new Snefru2Parameters(8, 256); break;
                default: temp = new Snefru2Parameters(8, 256); break;
            }

            return temp;
        }
    }
}

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
    /// <summary>A class that contains the parameters necessary to initialize a GHash algorithm.</summary>
    public class GHashParameters : HashAlgorithmParameters
    {
        private int shift;


        /// <summary>Gets or sets the shift value.</summary>
        public int Shift
        {
            get { return shift; }
            set { shift = value; }
        }


        /// <summary>Initializes a new instance of the GHashParamters class.</summary>
        /// <param name="shift">How many bits to shift.</param>
        public GHashParameters(int shift)
        {
            Shift = shift;
        }


        /// <summary>Retrieves a standard set of GHash parameters.</summary>
        /// <param name="standard">The name of the standard parameter set to retrieve.</param>
        /// <returns>The GHash Parameters for the given standard.</returns>
        public static GHashParameters GetParameters(GHashStandard standard)
        {
            GHashParameters temp;

            switch (standard)
            {
                case GHashStandard.GHash_3: temp = new GHashParameters(3); break;
                case GHashStandard.GHash_5: temp = new GHashParameters(5); break;
                default: temp = new GHashParameters(5); break;
            }

            return temp;
        }
    }
}

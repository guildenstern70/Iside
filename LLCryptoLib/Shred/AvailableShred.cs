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
	/// Available Shredding Methods
	/// </summary>
	public enum AvailableShred
	{
		/// <summary>
		/// No Shred
		/// </summary>
		NOTHING = 0,
		/// <summary>
		/// Single Overwrite Shred
		/// </summary>
		SIMPLE,
		/// <summary>
		/// Complex Shred
		/// </summary>
		COMPLEX,
		/// <summary>
		/// Random Shred
		/// </summary>
		RANDOM,
        /// <summary>
        /// HMG Infosec Standard 5 Enhanced
        /// </summary>
        HMGIS5ENH,
        /// <summary>
        /// German VSITR Shred
        /// </summary>
        GERMAN,
		/// <summary>
		/// Department Of Defence Shred
		/// </summary>
		DOD,
		/// <summary>
		/// Gutmann Shred
		/// </summary>
		GUTMANN,
	}

    /// <summary>
    ///  LLCryptoLib.Shred library contains the functions to wipe
    ///  a file from the disk, using a variety of standard algorothms, 
    ///  such Gutmann or DoD.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}

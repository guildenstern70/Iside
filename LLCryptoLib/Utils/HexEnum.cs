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

using System.Runtime.InteropServices;

namespace LLCryptoLib.Utils
{
	/// <summary>
	/// Hexadecimal representation style.
    /// Every enum constant represent a style,
    /// that is a representation of an hexadecimal number.
	/// </summary>
    [ComVisible(true)]
	public enum HexEnum : int
	{
		/// <summary>
		/// An unknown hexadecimal style
		/// </summary>
		UNKNOWN = 0,
		/// <summary>
		/// Classic hexadecimal style: FF12AB4D
		/// </summary>
		CLASSIC = 16,
		/// <summary>
		/// UNIX hexadecimal style: ff12ab4d
		/// </summary>
		UNIX = 12,
		/// <summary>
		/// SPACE hexadecimal style: FF 12 AB 4D
		/// </summary>
		SPACE = 14,
		/// <summary>
		/// Netscape(TM) hexadecimal style: FF:12:AB:4D
		/// </summary>
		NETSCAPE = 18,
        /// <summary>
        /// Modern hexadecimal style: ff 12 ab 4d
        /// </summary>
        MODERN = 22
	}
}

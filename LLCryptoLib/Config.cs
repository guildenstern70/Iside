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

using System.Globalization;
using System.Text;

namespace LLCryptoLib
{
	/// <summary>
	/// Configuration items.
	/// TextEncoding = Encoding for TextTransformations
	/// CULTURE = Default Culture info
	/// NUMBER_FORMAT = Default Number Format
	/// </summary>
    internal static class Config
	{
		/// <summary>
		/// TextEncoding = Unicode
		/// </summary>
        internal static Encoding TextEncoding = Encoding.Unicode;

		/// <summary>
		/// The number format in the current culture
		/// </summary>
        internal static NumberFormatInfo NUMBERFORMAT = CultureInfo.CurrentCulture.NumberFormat;
	}

    /// <summary>
    ///  LLCryptoLib is a .NET Framework library which allows programmers to easily add encryption, 
    ///  integrity and authentication services to their software. It is compatible with Microsoft .NET 
    ///  and Mono environments. It offers both symmetrical and asymmetrical text with stream encryption classes 
    ///  and hashing functions. It also offers full file shredding functions and a certificate management
    ///  utilities.
    ///  The library is divided into these namespaces:
    ///  <ul>
    ///  <li>LLCryptoLib.Crypto.<br></br>Contains encryption classes, both for streams and text.</li>
    ///  <li>LLCryptoLib.Hash<br></br>Contains hash code generator classes.</li>
    ///  <li>LLCryptoLib.Security<br></br>Contains certificate management classes along with relative cryptographic functions.</li>
    ///  <li>LLCryptoLib.Shred<br></br>Contains the shredding - secure delete - functions</li>
    ///  <li>LLCryptoLib.Utils<br></br>Contains helping classes such as I/O and Hexadecimal output.</li>
    ///  </ul>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}

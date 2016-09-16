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
#if !MONO
using System.Windows.Forms;
#endif

namespace LLCryptoLib.Utils
{

	/// <summary>
	/// Clipboard management class, used to
    /// send and retrieve strings from the operating system.
    /// <remarks>
    /// This class is not implemented in Mono environments.
    /// </remarks>
	/// </summary>
	public static class ClipboardUtils
	{
		/// <summary>
		/// Copy text string to clipboard
		/// </summary>
		/// <param name="what">The string to be copied into the clipboard</param>
        /// <exception cref="LLCryptoLibException">This method is not supported in Mono</exception>
		public static void Copy(String what)
		{
#if MONO
			throw new LLCryptoLibException("ClipboardUtils.Copy Not Supported in Mono");
#else
			Clipboard.SetDataObject(what,true);
#endif
		}

		/// <summary>
		/// Return true if the clipboard contains something pasteable as a string
		/// </summary>
		/// <returns>true if the clipboard contains something pasteable</returns>
        /// <exception cref="LLCryptoLibException">This method is not supported in Mono</exception>
		public static bool CanPaste()
		{
#if MONO
			throw new LLCryptoLibException("ClipboardUtils.CanPatse() Not Supported in Mono");			
#else
			IDataObject iData = Clipboard.GetDataObject();
			if (iData.GetDataPresent(DataFormats.Text)) 
			{
				return true;
			}
			return false;
#endif
		}

		/// <summary>
		/// Get text string from clipboard
		/// </summary>
		/// <returns>text string from clipboard</returns>
        /// <exception cref="LLCryptoLibException">This method is not supported in Mono</exception>
		public static string Get()
		{
#if MONO
			throw new LLCryptoLibException("ClipboardUtils.Get Not Supported in Mono");
#else
			String what = String.Empty;
			if (ClipboardUtils.CanPaste())
			{
				what = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
			}
			return what;
#endif
		}
	}


}

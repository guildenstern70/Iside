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
using System.Runtime.Serialization;

namespace LLCryptoLib
{
	/// <summary>
	/// LLCryptoLibException.
	/// </summary>
	[Serializable()]
	public class LLCryptoLibException : Exception
	{
		/// <summary>
		/// LLCryptoLibException object
		/// </summary>
		public LLCryptoLibException(): base("LLCryptoLib General Exception")
		{
		}

		/// <summary>
		/// LLCryptoLibException object
		/// </summary>
		/// <param name="message">Exception message</param>
		public LLCryptoLibException(string message): base(message) 
		{
		}

		/// <summary>
		/// LLCryptoLibException object
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Inner exception object</param>
		public LLCryptoLibException(string message, Exception innerException): base (message, innerException)
		{
		}

		/// <summary>
		/// LLCryptoLibException object
		/// </summary>
		/// <param name="info">Serialization Info handle</param>
		/// <param name="context">Streaming context</param>
		protected LLCryptoLibException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

	}
}

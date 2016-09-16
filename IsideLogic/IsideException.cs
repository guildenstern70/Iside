/**
 **   Iside
 **   Confront files with a single click
 **
 **   Copyright © LittleLite Software
 **
 **
 **/

using System;
using System.Runtime.Serialization;

namespace IsideLogic
{
	/// <summary>
	/// LLCryptoLibException.
	/// </summary>
	[Serializable()]
	public class IsideException : Exception
	{
		public IsideException(): base("Iside General Exception")
		{
		}

		public IsideException(string message): base(message) 
		{
		}

		public IsideException(string message, Exception innerException): base (message, innerException)
		{
		}

		protected IsideException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

	}
}


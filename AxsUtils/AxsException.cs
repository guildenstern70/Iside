/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved
 * 
 * AxsUtils.Dates.cs
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace AxsUtils
{
    /// <summary>
    /// AxsException.
    /// </summary>
    [Serializable()]
    public class AxsException : Exception
    {
        public AxsException()
            : base("AxsUtils General Exception")
        {
        }

        public AxsException(string message)
            : base(message)
        {
        }

        public AxsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AxsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}

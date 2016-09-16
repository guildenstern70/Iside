/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved
 * 
 * AxsUtils.Logger.cs
 * 
 */

using System;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.IO;

namespace AxsUtils
{
    /// <summary>
    /// A logger utility class based on trace .NET
    /// </summary>
    public class Logger
    {
    
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(string message)
        {
            DateTime dt = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.ToString(CultureInfo.CreateSpecificCulture("en-US")));
            sb.Append(" > ");
            sb.Append(message.TrimEnd());
            Trace.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Adds an empty line.
        /// </summary>
        public static void AddEmptyLine()
        {
            Trace.WriteLine(String.Empty);
        }

        /// <summary>
        /// Logs the specified message as an error.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogError(string message)
        {
            DateTime dt = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.ToString(CultureInfo.CreateSpecificCulture("en-US")));
            sb.Append(" ERROR > ");
            sb.Append(message.TrimEnd());
            Trace.WriteLine(sb.ToString());
        }

    }
}

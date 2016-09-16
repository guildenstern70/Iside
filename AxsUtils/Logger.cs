using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace AxsUtils
{
    /// <summary>
    ///     A logger utility class based on trace .NET
    /// </summary>
    public class Logger
    {
        /// <summary>
        ///     Logs the specified message.
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
        ///     Adds an empty line.
        /// </summary>
        public static void AddEmptyLine()
        {
            Trace.WriteLine(string.Empty);
        }

        /// <summary>
        ///     Logs the specified message as an error.
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
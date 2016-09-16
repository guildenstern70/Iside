/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved
 * 
 * AxsUtils.TraceManager.cs
 * 
 */

using System;
using System.Diagnostics;

namespace AxsUtils
{

    public static class TraceManager
    {
        public static void Error(string message, string module)
        {
            WriteLine(message, "error", module);
        }

        public static void Error(Exception ex, string module)
        {
            WriteLine(ex.Message, "error", module);
        }

        public static void Warning(string message, string module)
        {
            WriteLine(message, "warning", module);
        }

        public static void Info(string message, string module)
        {
            WriteLine(message, "info", module);
        }

        private static void WriteLine(string message, string type, string module)
        {
            Trace.WriteLine(
                    string.Format("{0} - {1} - {2} > {3}",
                                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                  type,
                                  module,
                                  message));
        }

    }
}

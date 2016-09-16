/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved 
 * 
 * AxsUtils.FileManager.cs
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AxsUtils
{
    internal static class NativeMethods
    {
        [DllImport("Kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetEndOfFile(IntPtr handle);
    }
}

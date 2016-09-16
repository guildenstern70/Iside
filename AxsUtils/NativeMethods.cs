using System;
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
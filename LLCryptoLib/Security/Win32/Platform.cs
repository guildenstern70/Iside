using System;
using System.Globalization;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Win32
{
    internal class Platform
    {
        /*public static void AssertWin2000() {
            if (m_OS.Version.Major < 5)
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, ResourceController.GetString("Error_UnsupportedOS"), "Windows 2000"));
        }*/

        private static readonly OperatingSystem m_OS = Environment.OSVersion;

        private Platform()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void AssertWinXP()
        {
            if ((m_OS.Version.Major < 5) || ((m_OS.Version.Major == 5) && (m_OS.Version.Minor < 1)))
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
                    ResourceController.GetString("Error_UnsupportedOS"), "Windows XP"));
        }
    }
}
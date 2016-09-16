using System;

namespace LLCryptoLib.Utils
{
    /// <summary>
    ///     Operating System class, returns a description
    ///     of the running operating system (name, version).
    /// </summary>
    public static class OS
    {
        /// <summary>
        ///     Return the Operating System information
        /// </summary>
        public static string OperatingSystem
        {
            get { return GetOperationSystemInformation(); }
        }

        private static string GetOperationSystemInformation()
        {
            OperatingSystem m_os = Environment.OSVersion;
            string m_osName = "Unknown";

            switch (m_os.Platform)
            {
                case PlatformID.Win32Windows:
                    switch (m_os.Version.Minor)
                    {
                        case 0:
                            m_osName = "Windows 95";
                            break;
                        case 10:
                            m_osName = "Windows 98";
                            break;
                        case 90:
                            m_osName = "Windows ME";
                            break;
                    }
                    break;

                case PlatformID.Win32NT:
                    switch (m_os.Version.Major)
                    {
                        case 3:
                            m_osName = "Windws NT 3.51";
                            break;
                        case 4:
                            m_osName = "Windows NT 4";
                            break;
                        case 5:
                            if (m_os.Version.Minor == 0)
                                m_osName = "Windows 2000";
                            else if (m_os.Version.Minor == 1)
                                m_osName = "Windows XP";
                            else if (m_os.Version.Minor == 2)
                                m_osName = "Windows Server 003";
                            break;
                        case 6:
                            m_osName = "Vista";
                            break;
                    }
                    break;
            }
            return m_osName + ", " + m_os.Version;
        }
    }
}
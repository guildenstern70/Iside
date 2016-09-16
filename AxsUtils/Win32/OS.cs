using System;
using System.Drawing;
using System.Security.Principal;
using System.Text;

namespace AxsUtils.Win32
{
    /// <summary>
    ///     Operating System class
    /// </summary>
    public sealed class OS
    {
        private OS()
        {
        }

        /// <summary>
        ///     Gets the name of the machine.
        /// </summary>
        /// <value>The name of the machine.</value>
        public static string MachineName
        {
            get { return Environment.MachineName; }
        }

        /// <summary>
        ///     Gets the user info.
        /// </summary>
        /// <value>The user info.</value>
        public static string UserInfo
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.UserDomainName);
                sb.Append(@"\");
                sb.Append(Environment.UserName);
                return sb.ToString();
            }
        }


        /// <summary>
        ///     Gets the operating system string.
        /// </summary>
        /// <value>The operating system.</value>
        public static string OperatingSystem
        {
            get { return GetOperationSystemInformation(); }
        }

        /// <summary>
        ///     Gets a value indicating whether the user is in the admin group
        /// </summary>
        /// <value><c>true</c> if user is admin; otherwise, <c>false</c>.</value>
        public static bool IsAdmin
        {
            get
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal p = new WindowsPrincipal(id);
                return p.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        ///     Returns true if the operating system is Vista or later
        /// </summary>
        /// <returns></returns>
        public static bool IsVistaOrLater
        {
            get { return Environment.OSVersion.Version.Major > 5; }
        }

        /// <summary>
        ///     Return the default application font: Vista = Segoe UI and XP = Tahoma and so on.
        ///     <example>
        ///         // Correct font
        ///         foreach (Control c in this.Controls)
        ///         {
        ///         Font old = c.Font;
        ///         c.Font = AxsUtils.Win32.OS.CrossVersionFont(old, c.Tag);
        ///         }
        ///     </example>
        /// </summary>
        /// <param name="designTimeFont">The font at design time</param>
        /// <param name="tag">If not null, then retain design size</param>
        public static Font CrossVersionFont(Font designTimeFont, object tag)
        {
            if (designTimeFont == null)
            {
                throw new ArgumentNullException("designTimeFont");
            }

            string fontName = SystemFonts.MessageBoxFont.FontFamily.Name;
            float fontSize = SystemFonts.MessageBoxFont.Size;

            if (tag != null)
            {
                fontSize = designTimeFont.Size;
            }

            return new Font(fontName, fontSize, designTimeFont.Style);
        }

        /// <summary>
        ///     Return the default application font: Vista = Segoe UI and XP = Tahoma and so on.
        ///     <example>
        ///         // Correct font
        ///         foreach (Control c in this.Controls)
        ///         {
        ///         Font old = c.Font;
        ///         c.Font = AxsUtils.Win32.OS.CrossVersionFont(old, c.Tag);
        ///         }
        ///     </example>
        /// </summary>
        /// <param name="designTimeFont">The font at design time</param>
        public static Font CrossVersionFont(Font designTimeFont)
        {
            return CrossVersionFont(designTimeFont, null);
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
                            m_osName = "Windows NT 3.51";
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
                                m_osName = "Windows Server 2003";
                            break;
                        case 6:
                            if (m_os.Version.Minor == 1)
                                m_osName = "Windows 7";
                            else if (m_os.Version.Minor == 2)
                                m_osName = "Windows 8";
                            else
                                m_osName = "Windows Vista";
                            break;
                    }
                    break;
            }
            return m_osName + " [Windows " + m_os.Version + "]";
        }
    }
}
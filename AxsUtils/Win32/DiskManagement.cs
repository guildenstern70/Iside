using System;
using System.Management;
using System.Text;

namespace AxsUtils.Win32
{
    /// <summary>
    ///     DiskManagement class
    /// </summary>
    public sealed class DiskManagement
    {
        private DiskManagement()
        {
        }

        /// <summary>
        ///     Returns free disk space
        /// </summary>
        /// <param name="drive">Example: "C:", "D:", "F:"</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ulong GetDiskSpace(string drive)
        {
            if (drive == null)
            {
                throw new ArgumentNullException("drive");
            }

            if ((drive.Length != 2) && !drive.EndsWith(":"))
            {
                throw new ArgumentException("Drive parameter must be as in C:");
            }

            StringBuilder sb = new StringBuilder("SELECT FreeSpace FROM Win32_LogicalDisk WHERE deviceID = '");
            sb.Append(drive);
            sb.Append("'");
            ManagementObjectCollection disks = new
                ManagementObjectSearcher(new SelectQuery(sb.ToString())).Get();

            ulong space = 0;

            foreach (ManagementObject mboDisk in disks)
            {
                space = (ulong) mboDisk["FreeSpace"];
            }

            System.Diagnostics.Debug.WriteLine("Drive " + drive + " has " + Convert.ToString(space) + " bytes free.");

            return space;
        }
    }
}
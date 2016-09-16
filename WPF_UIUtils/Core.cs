using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace WPFUtils
{
    public static class Core
    {

        public static string AppDataPath
        {
            get
            {
                string userAppDataRoamingDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string littleliteAppDataDir = Path.Combine(userAppDataRoamingDir, "LittleLite Software");
                string isideAppDataDir = Path.Combine(littleliteAppDataDir, "Iside");
                if (!Directory.Exists(isideAppDataDir))
                {
                    System.Diagnostics.Debug.WriteLine("Creating AppData dir > " + isideAppDataDir);
                    Directory.CreateDirectory(isideAppDataDir);
                }
                System.Diagnostics.Debug.WriteLine("Writing AppData to > " + isideAppDataDir);
                return isideAppDataDir;
            }
        }

        public static string StartupPath
        {
            get
            {
                return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            }
        }
    }
}

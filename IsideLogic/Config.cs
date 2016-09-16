/**
 **   Iside
 **   Confront files with a single click
 **
 **   Copyright © LittleLite Software
 **
 **
 **/

using System;
using System.Reflection;
using System.Diagnostics;

using LLCryptoLib.Hash;
using System.IO;
using System.Text;

using CheckForUpdatesWPF;


namespace IsideLogic
{
	/// <summary>
	/// Config.
	/// </summary>
	public static class Config
	{
		public const string APPNAME = "Iside";
        public const string APPTAG = "Iside";
		public const string APPFOLDER = APPNAME + " Folder";
		public const string APPKEY = "LittleLite Software\\Iside";
		public const string SYSVOL = "94118";
		public const string APPURL = "http://www.littlelite.net/iside/";
        public const string APPDOWNLOAD = "http://www.littlelite.net/iside/freedownload.html";
        public const string APPBUY = "http://www.littlelite.net/iside/buy.html";
        public const string APPBUY_DL = "http://metro.esellerate.net/ws/s.aspx?skurefnum=SKU62789593986";
		public const string FONTFILE = "fontstyle.dat";

        /// <summary>
        /// Application information
        /// </summary>
        /// <remarks>TO DO</remarks>
        public static ApplicationInfo AppInfo
        {
            get
            {
                ApplicationInfo ai = new ApplicationInfo(Config.APPNAME, Config.APPTAG, Config.APPKEY, 
                                                         Config.AppVersion, Config.Build, Config.APPDOWNLOAD);
                return ai;
            }
        }

        /// <summary>
        /// Gets the app name and version.
        /// </summary>
        /// <value>The app name and version.</value>
        public static string AppNameAndVersion
        {
            get
            {
                StringBuilder sb = new StringBuilder(Config.APPNAME);
                sb.Append(' ');
                sb.Append(Config.AppVersion);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the app version with build.
        /// </summary>
        /// <value>The app version with build.</value>
        public static string AppVersionWithBuild
        {
            get
            {
                StringBuilder sb = new StringBuilder("v.");
                Assembly isideAssembly = Assembly.GetCallingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(isideAssembly.Location);
                sb.Append(fvi.FileMajorPart);
                sb.Append('.');
                sb.Append(fvi.FileMinorPart);
                sb.Append('.');
                sb.Append(fvi.FileBuildPart);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the app version with build.
        /// </summary>
        /// <value>The app version with build.</value>
        public static string AppVersion
        {
            get
            {
                StringBuilder sb = new StringBuilder("v.");
                Assembly isideAssembly = Assembly.GetCallingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(isideAssembly.Location);
                sb.Append(fvi.FileMajorPart);
                sb.Append('.');
                sb.Append(fvi.FileMinorPart);
                return sb.ToString();
            }
        }

		/// <summary>
		/// Application version
		/// </summary>
		public static string Build
		{
			get
			{
                Assembly isideAssembly = Assembly.GetCallingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(isideAssembly.Location);
				return Convert.ToString(fvi.FileBuildPart);
			}
		}

	}
}

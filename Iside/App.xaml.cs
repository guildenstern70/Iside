/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using AxsUtils;
using Iside.Logic;
using IsideLogic;
using LiteErrorReportWPF;
using LLCryptoLib.Hash;
using WPFUtils;
using System.Windows.Threading;

namespace Iside
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string MODULE = "Iside.App";

        public App()
            : base()
        {
            System.Diagnostics.Debug.WriteLine("Welcome to Iside", MODULE);
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        private void Application_Startup(object sender, StartupEventArgs e) 
        {
            System.Diagnostics.Debug.WriteLine("Application_Startup", MODULE);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Window main = null;

            try
            {
                bool registered = App.IsRegistered();

                if (e != null)
                {
                    main = App.ArgumentParser(e.Args, registered);
                }
                else
                {
                    main = App.ArgumentParser(null, registered);
                }
            }
            catch (NullReferenceException)
            {
                main = new IsideWindow(false);
            }

            if (main != null)
            {
                main.Show();
            }
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            this.HandleException(e.Exception);
            e.Handled = true;
        }

        private static bool IsRegistered()
        {
            return true;
        }

        private static Window ArgumentParser(string[] args, bool isRegistered)
        {
            System.Diagnostics.Debug.WriteLine("ArgumentParser. Registered = " + isRegistered, MODULE);
            if (args != null)
            {
                System.Diagnostics.Debug.WriteLine("Args > ", MODULE);
                foreach (string arg in args)
                {
                    System.Diagnostics.Debug.WriteLine(" -> " + arg, MODULE);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No Args", MODULE);
            }

            Window mf = null;

            if ((args == null)||(args.Length == 0))
            {
                mf = new IsideWindow(isRegistered);
            }
            if (args.Length == 1)
            {
                #region 1_Argument
                if (args[0].EndsWith("-md5"))
                {
                    mf = new VerifyChecksum(ChecksumType.MD5SUM, true); // Iside launch in MD5sum mode
                }
                else if (args[0].Equals("-createsum"))
                {
                    mf = new Checksum(ChecksumType.MD5SUM, true);
                }
                else if (args[0].Equals("-sfv"))
                {
                    mf = new VerifyChecksum(ChecksumType.SFV, true);
                }
                else if (args[0].Equals("-createsfv"))
                {
                    mf = new Checksum(ChecksumType.SFV, true);
                }
#if (DEBUG)
                else if (args[0].Equals("-test"))
                {
                    IsideLogic.ShellIntegrationOption.Test();
                }
#endif
                if (args[0].StartsWith("-file"))
                {
                    System.Diagnostics.Debug.WriteLine("Received " + args[0]);
                    string filePath = args[0].Substring(5);
                    filePath = filePath.Replace('?', ' ');
                    mf = new IsideWindow(filePath, isRegistered); // Iside launch with one file
                }
                else // argument is a file
                {
                    try
                    {
                        FileInfo argFile = new FileInfo(args[0]);
                        if (argFile.Exists)
                        {
                            mf = new IsideWindow(argFile.FullName, isRegistered);
                        }
                    }
                    catch
                    {
                        mf = new IsideWindow(isRegistered);
                    }
                }
                #endregion
            }
            else if (args.Length == 2)
            {
                #region 2_Arguments
                if (args[0].Equals("-md5"))
                {
                    mf = new VerifyChecksum(ChecksumType.MD5SUM, args[1], true);
                }
                else if (args[0].Equals("-sfv"))
                {
                    mf = new VerifyChecksum(ChecksumType.SFV, args[1], true);
                }
                else if (args[0].Equals("-fastmd5"))
                {
                    string filePath = args[1].Substring(5);
                    filePath = filePath.Replace('?', ' ');
                    mf = new IsideWindow(filePath, AvailableHash.MD5, isRegistered);
                }
                else if (args[0].Equals("-fastsha1"))
                {
                    string filePath = args[1].Substring(5);
                    filePath = filePath.Replace('?', ' ');
                    mf = new IsideWindow(filePath, AvailableHash.SHA1, isRegistered);
                }
                else if (args[0].Equals("-fastcrc"))
                {
                    mf = new IsideWindow(args[1], AvailableHash.CRC32, isRegistered);
                }
                else if (args[0].Equals("-createsum"))
                {
                    mf = new Checksum(ChecksumType.MD5SUM, true, args[1]);
                }
                else if (args[0].Equals("-createsfv"))
                {
                    mf = new Checksum(ChecksumType.SFV, true, args[1]);
                }
                else if (args[0].StartsWith("-file"))
                {
                    System.Diagnostics.Debug.WriteLine("Received " + args[0] + " " + args[1]);
                    string filePath1 = args[0].Substring(5);
                    filePath1 = filePath1.Replace('?', ' ');
                    string filePath2 = args[1].Substring(5);
                    filePath2 = filePath2.Replace('?', ' ');
                    mf = new IsideWindow(filePath1, filePath2, isRegistered); // Iside launch with two files
                }
                #endregion
            }

            return mf;

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.HandleException((Exception)e.ExceptionObject);
        }

        private void HandleException(Exception exc)
        {
            try
            {

                string errorNumber = ErrorReportLib.ErrorCodeNumber(exc.Message);

                // Preparing report
                StringBuilder sb = new StringBuilder("ISIDE Critical error.");
                sb.AppendLine("<br />");
                sb.AppendLine("Version: " + Config.AppVersionWithBuild);
                sb.AppendLine("Date: " + DateTime.Now.ToString());
                sb.Append("<br />");
                sb.AppendLine("OS: " + AxsUtils.Win32.OS.OperatingSystem);
                sb.Append("<br />");
                sb.AppendLine("Error: " + exc.Source);
                sb.Append("<br />");
                sb.AppendLine("Error Code: " + errorNumber);
                sb.Append("<br />");
                sb.AppendLine("Message: " + exc.Message);
                sb.Append("<br />");
                sb.AppendLine();
                sb.Append("<br />");
                sb.Append("<br />");
                sb.Append("<pre>");
                sb.AppendLine(exc.ToString());
                sb.Append("</pre>");
                sb.AppendLine();
                sb.Append("<br />");
                sb.Append("<pre>");
                RegistryOptions options = new RegistryOptions();
                sb.AppendLine(options.ToString());
                sb.Append("</pre>");
                sb.Append("<br />");
                string report = sb.ToString();

                // Saving log
                FileManager fm = FileManager.Reference;
                string errorPath = Path.Combine(WPFUtils.Core.AppDataPath, "critical_error.log");
                fm.SaveFile(errorPath, report, true);

                // Send to LittleLite
                CriticalError ceForm = new CriticalError(Config.APPNAME,
                                                         Config.APPKEY,
                                                         report, errorNumber);
                ceForm.ShowDialog();

            }
            finally
            {
                Application.Current.Shutdown();
            }
        }


    }
}

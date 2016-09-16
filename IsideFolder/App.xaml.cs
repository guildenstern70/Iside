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
using Iside;
using IsideLogic;
using LiteErrorReportWPF;
using WPFUtils;
using System.Windows.Threading;

namespace IsideFolder
{
    /// <summary>
    /// App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                bool registered = true;

                if (e != null)
                {
                    this.RunIsideFolder(e.Args, registered);
                }
                else
                {
                    this.RunIsideFolder(null, registered);
                }
            }
            catch (NullReferenceException)
            {
                this.RunIsideFolder(null, false);
            }
            
        }

        private void RunIsideFolder(string[] args, bool registered)
        {
            MainWindow main = null;

            if ((args == null)||(args.Length == 0))
            {
                main = new MainWindow(registered);
            }
            else if (args.Length == 1)
            {
                if ((args[0].EndsWith(".iscmp") || (args[0].EndsWith(".isrl"))))
                {
                    System.Diagnostics.Debug.WriteLine("Launching IsideFolder with argument = " + args[0]);
                    if (File.Exists(args[0]))
                    {
                        main = new MainWindow(args[0], registered);
                    }
                }
            }
            else if (args.Length == 2)
            {
                main = new MainWindow(args[0], args[1], registered);
            }
            else if (args.Length == 3)
            {
                if ((args[0].Equals("-unattended")))
                {
                    main = new MainWindow(registered);
                    string comparisonFile = args[1];
                    string logTargetDir = args[2];
                    main.Unattended = true;

                    if (!Directory.Exists(logTargetDir))
                    {
                        MessageTaskDialog.Show(null, IsideLogic.Config.APPFOLDER, 
                                        "Directory " + logTargetDir + " does not exist.", 
                                        "IsideFolder Unattended Mode Error Message", 
                                        TaskDialogType.ERROR);
                        main.Close();
                        main = null;
                    }
                    else
                    {
                        main.LogDirectory = logTargetDir;
                        if (!File.Exists(comparisonFile))
                        {
                            MessageTaskDialog.Show(null, IsideLogic.Config.APPFOLDER, 
                                       "File " + comparisonFile + " does not exist.", 
                                       "IsideFolder Unattended Mode Error Message", 
                                       TaskDialogType.ERROR);
                            main.Close();
                            main = null;
                        }
                        else
                        {
                            // Run Iside Folders Unattended
                            main.OpenComparison(comparisonFile);
                        }
                    }
                }

            }

            if (main != null)
            {
                main.Show();
            }
            else
            {
                Application.Current.Shutdown();
            }

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

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            this.HandleException(e.Exception);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.HandleException((Exception)e.ExceptionObject);
        }
    }


}

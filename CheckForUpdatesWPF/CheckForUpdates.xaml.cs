/**
    Check For Updates Utility - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using AxsUtils.Http;
using AxsUtils.Xml;
using CheckForUpdates;

namespace CheckForUpdatesWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CheckForUpdates : Window
    {
        private HttpAxsRequest versionRequest;
        private ApplicationInfo app;
        private ProxyDetails readGad;
        private string appDownloadUrl;
        private BackgroundWorker checkBw;

        public CheckForUpdates()
        {
            this.app = new ApplicationInfo("Folder Crypt", "FolderCrypt",
                                            @"LittleLite Software\Folder Crypt\Options",
                                            "v3.2", "1034", "http://www.littlelite.net/foldercrypt/download.html");
            this.Init();
        }

        public CheckForUpdates(ApplicationInfo appInfo)
        {
            this.app = appInfo;
            this.Init();
        }

        private void Init()
        {
            this.appDownloadUrl = String.Empty;
            this.checkBw = new BackgroundWorker();
            InitializeComponent();
            this.SetDancingProgress(Dancing.START);
            this.readGad = new ProxyDetails(this.app.Name, this.app.RegistryKey);
            this.InitProxy();
            this.InitializeBackgroundWorker();
 
        }

        private void InitializeBackgroundWorker()
        {
            this.checkBw.WorkerReportsProgress = true;
            this.checkBw.DoWork += new DoWorkEventHandler(checkBw_DoWork);
            this.checkBw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(checkBw_RunWorkerCompleted);
            this.checkBw.ProgressChanged += new ProgressChangedEventHandler(checkBw_ProgressChanged);
        }

        private void ResetUI()
        {
            this.progressBar.Value = 0;
            this.btnCheck.IsEnabled = true;
            this.statusBar.Content = Strings.S00015;
        }

        private void SetDancingProgress(Dancing status)
        {
            switch (status)
            {
                case Dancing.START:
                    this.progressBar.IsIndeterminate = false;
                    this.progressBar.Value = this.progressBar.Minimum;
                    break;

                case Dancing.INPROCESS:
                    this.progressBar.IsIndeterminate = true;
                    break;

                case Dancing.FINISH:
                    this.progressBar.IsIndeterminate = false;
                    this.progressBar.Value = this.progressBar.Maximum;
                    break;
            }

        }

        private void InitProxy()
        {
            this.versionRequest = new HttpAxsRequest(10000, this.app.Name);
            if ((this.readGad.UsesProxy) && (this.readGad.ProxyUrl != null))
            {
                this.versionRequest.SetProxy(this.readGad.Proxy);
            }
        }

        private int GetBuild(string build)
        {
            int b = Int32.Parse(build);
            return b;
        }

        private float GetVersion(string version)
        {
            string floatVersion = version;
            if (version.StartsWith("v."))
            {
                floatVersion = version.Substring(2);
            }
            else if (version.StartsWith("v"))
            {
                floatVersion = version.Substring(1);
            }
            float v = Single.Parse(floatVersion);
            return v;
        }

        private string GetSimpleVersionString()
        {
            string version = this.app.Version;

            if (version.StartsWith("v."))
            {
                version = version.Substring(2);
            }
            else if (version.StartsWith("v"))
            {
                version = version.Substring(1);
            }

            if (version.EndsWith("beta"))
            {
                version = version.Substring(0, version.Length - 4);
            }

            return version.Trim();
        }

        /// <summary>
        /// Determines whether is there a new version available.
        /// </summary>
        /// <param name="requestedVersion">The version found available on Internet.</param>
        /// <returns>
        /// 	<c>true</c> if a new version is available; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNewVersionAvailable(string requestedVersion)
        {
            bool newAvailable = false;

            if (requestedVersion.StartsWith("Error"))
            {
                return false;
            }

            try
            {
                string version = this.GetSimpleVersionString();
                Debug.WriteLine("This is version: " + version);

                float fVersion = this.GetVersion(version);
                int build = this.GetBuild(this.app.Build);

                int indexBuild = requestedVersion.IndexOf(' ');
                if (indexBuild < 1)
                {
                    System.Diagnostics.Debug.WriteLine("BAD FORMAT VERSION IN INPUT: missing space in " + requestedVersion);
                    return false;
                }

                float availVersion = this.GetVersion(requestedVersion.Substring(0, indexBuild));
                int availBuild = this.GetBuild(requestedVersion.Substring(indexBuild));

                if (fVersion < availVersion)
                {
                    // New Major release available
                    newAvailable = true;
                }
                else if (fVersion == availVersion)
                {
                    // Same version, new build?
                    if (build < availBuild)
                    {
                        // New minor release available
                        newAvailable = true;
                    }
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine("Error in reading version from Web: " + exc.Message);
            }

            return newAvailable;
        }

        private void AsyncCheck()
        {
            this.statusBar.Content = Strings.S00001;
            Mouse.OverrideCursor = Cursors.Wait;
            this.SetDancingProgress(Dancing.INPROCESS);

            if (this.checkBw.IsBusy != true)
            {
                // Start the asynchronous operation.
                this.checkBw.RunWorkerAsync();
            }
        }

        private string EvaluateDownloadedXML(XmlDocument xml)
        {
            string feedback = String.Empty;

            // 1. Get version
            XmlNodeReader reader = new XmlNodeReader(xml);
            string foundVersion = XmlElementReader.GetElement(reader, this.app.XmlTag, "version");
            if (foundVersion != null)
            {
                feedback = foundVersion;
            }
            else
            {
                feedback = "Not found.";
            }

            if (feedback == null)
            {
                feedback = Strings.S00002 + this.app.XmlTag;
            }
            else
            {
                if (!feedback.StartsWith("Error"))
                {
                    feedback += " ";
                    // 2. Get build
                    reader = new XmlNodeReader(xml);
                    string foundBuild = XmlElementReader.GetElement(reader, this.app.XmlTag, "build");
                    if (foundBuild != null)
                    {
                        feedback += foundBuild;
                        Debug.WriteLine("Latest version available: " + feedback);
                    }
                    else
                    {
                        Debug.WriteLine("Cant find Build");
                    }

                    // 2b. Get download URL
                    reader = new XmlNodeReader(xml);
                    string foundUrl = XmlElementReader.GetElement(reader, this.app.XmlTag, "download");
                    if (foundUrl != null)
                    {
                        this.appDownloadUrl = foundUrl;
                        Debug.WriteLine("Download URL: " + this.appDownloadUrl);
                    }
                    else
                    {
                        Debug.WriteLine("Cant find URL");
                    }
                }
            }
            return feedback;
        }

        private string Check()
        {
            Stream s = null;
            string feedback = String.Empty;

            try
            {
                this.checkBw.ReportProgress(20);

                string xmlText = this.versionRequest.MakeRequest("http://www.littlelite.net/", "Products.xml");
                if (!xmlText.StartsWith("Error"))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(xmlText);
                    this.checkBw.ReportProgress(66);
                    feedback = this.EvaluateDownloadedXML(xml);
                }
                else
                {
                    feedback = xmlText;
                }
                this.checkBw.ReportProgress(90);
            }
            catch (Exception exc)
            {
                feedback = exc.Message;
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            return feedback;
        }

        #region Command Handlers
        private void btnSetProxy_Click(object sender, RoutedEventArgs e)
        {
            this.ResetUI();

            SetWebProxy proxyForm = new SetWebProxy(this.app.Name, this.app.RegistryKey);
            proxyForm.Owner = this;
            proxyForm.ShowDialog();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            this.AsyncCheck();
        }

        private void checkBw_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = this.Check();
        }

        private void checkBw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
        }

        // This event handler deals with the results of the background operation.
        private void checkBw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            string xmlDownloadingResult = e.Result as string;

            if (this.IsNewVersionAvailable(xmlDownloadingResult))
            {
                xmlDownloadingResult = Strings.S00003 + xmlDownloadingResult;
                xmlDownloadingResult += Strings.S00004;
                this.lnkMoreInfo.Content = Strings.S00005;
                this.Title = Strings.S00009;
            }
            else
            {
                xmlDownloadingResult = Strings.S00007;
                this.Title = Strings.S00010;
            }

            Mouse.OverrideCursor = null;
            this.SetDancingProgress(Dancing.FINISH);
            this.btnCheck.IsEnabled = false;
            this.btnCancel.Content = Strings.S00008;
            this.statusBar.Content = xmlDownloadingResult;


        }

        private void lnkMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            if (this.lnkMoreInfo.Content.Equals(Strings.S00005))
            {
                if (this.appDownloadUrl.Length > 0)
                {
                    this.statusBar.Content = Strings.S00011;
                    Process.Start(this.appDownloadUrl);
                    this.statusBar.Content = Strings.S00012;
                }
            }
            else
            {
                Process.Start(this.app.Url);
            }
        }

        #endregion




            
    }
}

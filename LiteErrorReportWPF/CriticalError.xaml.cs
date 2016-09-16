using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Input;
using CheckForUpdates;

namespace LiteErrorReportWPF
{
    /// <summary>
    /// CriticalError form
    /// </summary>
    public partial class CriticalError : Window
    {
        private string applicationName;
        private string applicationKey;
        private string report;
        private BackgroundWorker bw;

        public CriticalError()
        {
            string[] parms = Environment.GetCommandLineArgs();
            if (parms.Length == 5)
            {
                System.Diagnostics.Debug.WriteLine("Found 4 arguments: ");
                foreach (string arg in parms)
                {
                    System.Diagnostics.Debug.WriteLine("> " + arg);
                }
                this.Initialize(parms[1], parms[2], parms[3], parms[4]);
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        public CriticalError(string appName, string appKey, string appReport, string errNum)
        {
            this.Initialize(appName, appKey, appReport, errNum);
        }

        private void Initialize(string appName, string appKey, string appReport, string errNum)
        {
            this.applicationName = appName;
            this.applicationKey = appKey;
            this.report = appReport;

            this.bw = new BackgroundWorker();
            this.bw.DoWork += bw_DoWork;
            this.bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            this.Title = appName + " Critical Error ";
            this.Title += errNum;

            InitializeComponent();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;
            this.progressBar.Visibility = System.Windows.Visibility.Hidden;
            this.btnCancel.IsEnabled = true;
            this.btnCancel.Content = "Exit";

            if (e.Error != null)
            {
                MessageBox.Show(this, "Error in sending error message.", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string result = e.Result as string;
            MessageBox.Show(this, result, "Error sent", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = this.AsyncSend();
        }

        private string AsyncSend()
        {
            // Is there a proxy?
            ProxyDetails gpa = new ProxyDetails(this.applicationName, this.applicationKey);
            IWebProxy webProxy = gpa.Proxy;
            net.littlelite.www.ProgramFeedbackSoapClient pf = new net.littlelite.www.ProgramFeedbackSoapClient();
            if (webProxy != null)
            {
                System.Diagnostics.Debug.WriteLine("Found web proxy: " + webProxy.ToString());
                WebRequest.DefaultWebProxy = webProxy;
            }

            // Call WS
            return pf.SendFeedback(this.applicationName, this.report);
        }

        private void SendReport()
        { 
            Mouse.OverrideCursor = Cursors.Wait;
            this.progressBar.Visibility = System.Windows.Visibility.Visible;

            this.btnSend.IsEnabled = false;
            this.btnCancel.IsEnabled = false;

            this.bw.RunWorkerAsync();
        }

        private void CriticalErrorWnd_Loaded(object sender, RoutedEventArgs e)
        {
            string message = "xxx has encountered an error and needs to close. \n\n" +
                             "We have created an error report that you can send to " +
                             "help us improve xxx. The report is anonymous and does" +
                             " not contain any private information.";

            message = message.Replace("xxx", this.applicationName);

            this.labelError.Text = message;

            Mouse.OverrideCursor = null;
            this.progressBar.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            this.SendReport();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

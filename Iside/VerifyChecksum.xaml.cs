using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AxsUtils;
using IsideLogic;
using LLCryptoLib;
using LLCryptoLib.Hash;
using WPFUtils;

namespace Iside
{
    /// <summary>
    ///     Verification Parameters
    /// </summary>
    internal sealed class VerifyChecksumParams
    {
        internal string Md5SumFilePath { get; set; }
        internal DirectoryInfo ValidationPath { get; set; }
        internal bool SubCheck { get; set; }
        internal bool IgnoreMd5File { get; set; }
        internal CallbackEntry Feedback { get; set; }
        internal SupportedHashAlgo Algorithm { get; set; }
    }

    /// <summary>
    ///     Interaction logic for VerifyChecksum.xaml
    /// </summary>
    public partial class VerifyChecksum : Window
    {
        private BackgroundWorker bwGenerator;
        private ChecksumGenerator gen;
        private readonly bool isStandalone;
        private Microsoft.Win32.OpenFileDialog openFileDialog;

        #region Constructors

        public VerifyChecksum(ChecksumType type, bool isAlone)
        {
            this.InitializeComponent();
            this.isStandalone = isAlone;
            this.Init(type);
        }

        /// <summary>
        ///     Constructor with folderPath already indicated
        /// </summary>
        /// <param name="folderPath"></param>
        public VerifyChecksum(ChecksumType type, string folderPath, bool isAlone)
        {
            if (folderPath == null)
            {
                throw new ArgumentNullException("folderPath");
            }

            this.isStandalone = isAlone;
            this.InitializeComponent();
            if (folderPath.EndsWith(".md5"))
            {
                if (File.Exists(folderPath))
                {
                    this.txtMD5file.Text = folderPath;
                    this.txtMD5file.Width = 272;
                    this.txtMD5file.IsReadOnly = true;
                    this.btnLoadMd5Sum.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageTaskDialog.Show(this, Config.APPNAME, "File " + folderPath + " does not exist",
                        Config.APPNAME, TaskDialogType.WARNING);
                }
            }
            else
            {
                if (Directory.Exists(folderPath))
                {
                    this.txtVerificationDir.Text = folderPath;
                    this.txtVerificationDir.IsReadOnly = true;
                    this.btnVerify.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageTaskDialog.Show(this, Config.APPNAME, "File " + folderPath + " does not exist",
                        Config.APPNAME, TaskDialogType.WARNING);
                }
            }
            this.Init(type);
        }

        #endregion

        #region Private Methods

        private void InitializeBackgroundWorker()
        {
            this.bwGenerator = new BackgroundWorker();
            this.bwGenerator.WorkerReportsProgress = true;
            this.bwGenerator.WorkerSupportsCancellation = true;
            this.bwGenerator.DoWork += this.bwGenerator_DoWork;
            this.bwGenerator.RunWorkerCompleted += this.bwGenerator_RunWorkerCompleted;
            this.bwGenerator.ProgressChanged += this.bwGenerator_ProgressChanged;
        }

        private void SelectHash(string hashName)
        {
            ComboBoxItem cboItem = null;
            for (int j = 0; j < this.cboHash.Items.Count; j++)
            {
                cboItem = this.cboHash.Items[j] as ComboBoxItem;
                string cboItemString = cboItem.Content as string;
                if (hashName == cboItemString)
                {
                    this.cboHash.SelectedIndex = j;
                    break;
                }
            }
        }

        private void Init(ChecksumType type)
        {
            GUI.ComboBoxItemsAdd(this.cboHash, SupportedHashAlgorithms.GetHashAlgorithms());
            this.btnView.IsEnabled = this.isStandalone;
            this.openFileDialog = new Microsoft.Win32.OpenFileDialog();
            this.InitializeBackgroundWorker();

            switch (type)
            {
                case ChecksumType.MD5SUM:
                    this.SelectHash("MD5");
                    this.label1.Text = "Md5Sum file:";
                    this.openFileDialog.Filter = "MD5 files (*.md5)|*.md5|All files|*.*";
                    this.openFileDialog.Title = "Select MD5Sum file";
                    this.chkIgnoreMd5sum.Content = "Ignore Md5Sum file if found";
                    this.Title = "Iside - Verify MD5Sum";
                    this.gen = new Md5SumGenerator();
                    break;

                case ChecksumType.SFV:
                    this.SelectHash("CRC32");
                    this.label1.Text = "SFV file:";
                    this.openFileDialog.Filter = "SFV files (*.sfv)|*.sfv|All files|*.*";
                    this.openFileDialog.Title = "Select SFV file";
                    this.chkIgnoreMd5sum.Content = "Ignore SFV file if found";
                    this.Title = "Iside - Verify SFV";
                    this.gen = new SFVGenerator();
                    break;
            }
        }

        private void Reset()
        {
            this.progressBar.Value = this.progressBar.Minimum;
        }


        private bool ValidateMD5Sum(DirectoryElements de, VerifyChecksumParams parms)
        {
            this.gen.Initialize(de, parms.Algorithm);
            return this.gen.VerifySum(parms.Md5SumFilePath, parms.Feedback);
        }

        private void SetProgressBar(int i)
        {
            this.bwGenerator.ReportProgress(i);
        }

        #endregion

        #region Event Handlers

        #region BackgroundWorker Events

        private void bwGenerator_DoWork(object sender, DoWorkEventArgs e)
        {
            VerifyChecksumParams param = e.Argument as VerifyChecksumParams;

            DirectoryElements validationPath = new DirectoryElements();
            validationPath.Scan(param.ValidationPath, param.SubCheck);

            if (param.IgnoreMd5File)
            {
                if (validationPath.RemoveElement(new FileInfo(param.Md5SumFilePath)))
                {
                    System.Diagnostics.Debug.WriteLine("Removed Md5sum file from collection.");
                }
            }

            // Update maximum value of progress bar
            this.bwGenerator.ReportProgress(-validationPath.NrOfFiles);
            e.Result = this.ValidateMD5Sum(validationPath, param);
        }

        private void bwGenerator_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            if (progress < 0) // Initialize Progress Bar
            {
                this.progressBar.Minimum = 0;
                this.progressBar.Maximum = -progress;
                this.progressBar.Value = 0;
            }
            else
            {
                this.progressBar.Value = progress;
            }
        }

        private void bwGenerator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar.Value = this.progressBar.Maximum;

            Mouse.OverrideCursor = null;

            if (e.Cancelled)
            {
                MessageBox.Show(this, "Verification Halted", Config.APPNAME,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!(e.Error == null))
            {
                MessageBox.Show(this, "Error: " + e.Error.Message, Config.APPNAME,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if ((bool) e.Result)
                {
                    MessageTaskDialog.Show(this, Config.APPNAME, "Verification successful", Config.APPNAME,
                        TaskDialogType.INFO);
                }
                else
                {
                    MessageTaskDialog.Show(this, Config.APPNAME, "Verification failed", Config.APPNAME,
                        TaskDialogType.INFO);
                }
            }

            this.btnGo.IsEnabled = false;
            this.btnCancel.Content = "Exit";
        }

        #endregion

        #region Other Events

        private void btnLoadMd5Sum_Click(object sender, RoutedEventArgs e)
        {
            if (this.progressBar.Value != this.progressBar.Minimum)
            {
                this.txtMD5file.Text = string.Empty;
                this.btnView.IsEnabled = false;
                this.Reset();
            }

            bool? result = this.openFileDialog.ShowDialog();
            if (result == true)
            {
                this.txtMD5file.Text = this.openFileDialog.FileName;
                this.btnView.IsEnabled = true;
            }
        }

        private void btnVerify_Click(object sender, RoutedEventArgs e)
        {
            if (this.progressBar.Value != this.progressBar.Minimum)
            {
                this.txtVerificationDir.Text = string.Empty;
                this.Reset();
            }

            string selectedPath = GUI.SelectFolderPath(this, false);

            if (!string.IsNullOrEmpty(selectedPath))
            {
                this.txtVerificationDir.Text = selectedPath;
                if (this.txtMD5file.Text.Length > 0)
                {
                    this.btnGo.IsEnabled = true;
                }
            }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            string md5Path = this.txtMD5file.Text;

            if (md5Path.Length > 0)
            {
                if (File.Exists(md5Path))
                {
                    if (this.txtVerificationDir.Text.Length > 0)
                    {
                        if (Directory.Exists(this.txtVerificationDir.Text))
                        {
                            ComboBoxItem selHash = this.cboHash.SelectedItem as ComboBoxItem;
                            VerifyChecksumParams verifyParms = new VerifyChecksumParams();
                            verifyParms.Md5SumFilePath = md5Path;
                            verifyParms.IgnoreMd5File = this.chkIgnoreMd5sum.IsChecked.GetValueOrDefault(true);
                            verifyParms.SubCheck = this.chkIncludeSubDirs.IsChecked.GetValueOrDefault(true);
                            verifyParms.ValidationPath = new DirectoryInfo(this.txtVerificationDir.Text);
                            verifyParms.Feedback = this.SetProgressBar;
                            verifyParms.Algorithm = SupportedHashAlgoFactory.FromName((string) selHash.Content);
                            Mouse.OverrideCursor = Cursors.Wait;
                            this.bwGenerator.RunWorkerAsync(verifyParms);
                        }
                        else
                        {
                            MessageTaskDialog.Show(this, Config.APPNAME,
                                "Verification path " + md5Path + " does not exist.", Config.APPNAME,
                                TaskDialogType.WARNING);
                        }
                    }
                    else
                    {
                        MessageTaskDialog.Show(this, Config.APPNAME, "Specify a verification path.", Config.APPNAME,
                            TaskDialogType.WARNING);
                    }
                }
                else
                {
                    MessageTaskDialog.Show(this, Config.APPNAME, "MD5Sum file " + md5Path + " does not exist.",
                        Config.APPNAME, TaskDialogType.WARNING);
                }
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            string strNewFile;

            // Copy the file in application directory
            if (this.txtMD5file.Text.Length > 0)
            {
                strNewFile = Path.GetFileName(this.txtMD5file.Text) + ".txt";
                strNewFile = Path.Combine(Path.GetTempPath(), strNewFile);
                try
                {
                    File.Copy(this.txtMD5file.Text, strNewFile, true);
                    System.Diagnostics.Process.Start(strNewFile);
                }
                catch
                {
                    MessageTaskDialog.Show(this, Config.APPNAME, "Cannot show " + this.txtMD5file.Text, Config.APPNAME,
                        TaskDialogType.WARNING);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.isStandalone)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (this.isStandalone)
            {
                this.btnCancel.Content = "Exit";
            }
        }

        #endregion
    }
}
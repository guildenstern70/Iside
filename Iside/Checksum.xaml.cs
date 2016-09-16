using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using IsideLogic;
using LLCryptoLib;
using LLCryptoLib.Hash;
using Microsoft.Win32;
using WPFUtils;

namespace Iside
{
    /// <summary>
    ///     The type of checksum requested
    /// </summary>
    public enum ChecksumType
    {
        /// <summary>
        ///     Md5Sum checksum type
        /// </summary>
        MD5SUM,

        /// <summary>
        ///     SFV Checksum type
        /// </summary>
        SFV
    }

    /// <summary>
    ///     Interaction logic for Checksum.xaml
    /// </summary>
    public partial class Checksum : Window
    {
        private BackgroundWorker bwGenerator;
        private string checksumName;
        private ChecksumGenerator chksumGen;
        private bool isStandalone;
        private SaveFileDialog saveFileDialog;

        public Checksum(ChecksumType type, bool standalone)
        {
            this.Initialize(type, standalone, null);
        }

        public Checksum(ChecksumType type, bool standalone, string folder)
        {
            this.Initialize(type, standalone, folder);
        }

        private void Initialize(ChecksumType type, bool standalone, string folder)
        {
            this.InitializeComponent();
            this.InitType(type);
            this.InitializeBackgroundWorker();
            this.isStandalone = standalone;

            if (folder != null)
            {
                if (Directory.Exists(folder))
                {
                    this.txtFolder.Text = folder;
                    this.btnSelectFolder.TabIndex = 99;
                    this.btnGenerate.IsEnabled = true;
                }
            }
            this.isStandalone = standalone;
        }

        private void InitType(ChecksumType type)
        {
            string[] availableHashes = SupportedHashAlgorithms.GetHashAlgorithms();
            availableHashes.ToList().ForEach(item => this.cboHash.Items.Add(item));
            this.txtFolder.IsReadOnly = true;
            this.txtFolder.IsReadOnlyCaretVisible = false;
            this.saveFileDialog = new SaveFileDialog();

            switch (type)
            {
                case ChecksumType.MD5SUM:
                    this.checksumName = "Md5Sum";
                    this.cboHash.SelectedItem = "MD5";
                    this.Title = "Iside - Md5Sum Generator";
                    this.saveFileDialog.DefaultExt = "md5";
                    this.saveFileDialog.Filter = "MD5 files|*.md5|All files|*.*";
                    this.saveFileDialog.Title = "Select where MD5Sum will be saved";
                    this.chksumGen = new Md5SumGenerator();
                    break;

                case ChecksumType.SFV:
                    this.checksumName = "SFV";
                    this.cboHash.SelectedItem = "CRC32";
                    this.Title = "Iside - SFV Generator";
                    this.saveFileDialog.DefaultExt = "md5";
                    this.saveFileDialog.Filter = "SFV files|*.sfv|All files|*.*";
                    this.saveFileDialog.Title = "Select where SFV will be saved";
                    this.chksumGen = new SFVGenerator();
                    break;
            }
        }

        private void InitializeBackgroundWorker()
        {
            this.bwGenerator = new BackgroundWorker();
            this.bwGenerator.WorkerReportsProgress = true;
            this.bwGenerator.WorkerSupportsCancellation = true;
            this.bwGenerator.DoWork += this.bwGenerator_DoWork;
            this.bwGenerator.RunWorkerCompleted += this.bwGenerator_RunWorkerCompleted;
            this.bwGenerator.ProgressChanged += this.bwGenerator_ProgressChanged;
        }

        private string AskForFilename()
        {
            string md5SumFile = null;
            if (this.saveFileDialog.ShowDialog(this) == true)
            {
                md5SumFile = this.saveFileDialog.FileName;
            }
            return md5SumFile;
        }

        private void SaveMd5SumFile(string md5filepath, string md5sumcontents)
        {
            if (md5sumcontents != null)
            {
                var fm = AxsUtils.FileManager.Reference;
                bool savedOk = fm.SaveFile(md5filepath, md5sumcontents, false);
                if (this.progressBar.Value != this.progressBar.Maximum)
                {
                    this.progressBar.Value = this.progressBar.Maximum;
                }

                if (savedOk)
                {
                    MessageBox.Show(this, this.checksumName + " file saved", Config.APPNAME,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(this, "Cannot save " + this.checksumName + " as " + this.saveFileDialog.FileName,
                        Config.APPNAME, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void PrepareForGeneration()
        {
            if (string.IsNullOrEmpty(this.txtFolder.Text))
            {
                return;
            }

            DirectoryInfo di = new DirectoryInfo(this.txtFolder.Text);

            if (!di.Exists)
            {
                MessageBox.Show(this, "The folder does not exist.",
                    Config.APPNAME, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SupportedHashAlgo hashAlgo = SupportedHashAlgoFactory.FromName((string) this.cboHash.SelectedItem);

            if (hashAlgo.IsKeyed)
            {
                KeyedHashAlgorithm khash = (KeyedHashAlgorithm) hashAlgo.Algorithm;
                // Ask for key only if it was not just entered
                KeyedHash khf = new KeyedHash();
                khf.Owner = this;
                if (khf.ShowDialog() == true)
                {
                    khash.Key = khf.GetKey();
                }
            }

            if (!this.bwGenerator.IsBusy)
            {
                if (this.AskForFilename() != null)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    this.btnGenerate.IsEnabled = false;

                    CallbackEntry cbe = this.UpdateProgressBar;
                    ChecksumParameters parms = new ChecksumParameters();
                    parms.Callback = cbe;
                    parms.SumDir = di;
                    parms.Hash = hashAlgo;
                    parms.CheckSubdirs = this.chkSubdirs.IsChecked.Value;

                    this.bwGenerator.RunWorkerAsync(parms);
                }
            }
        }

        private string Generate(ChecksumParameters parms, AxsUtils.DirectoryElements de)
        {
            string md5sum = null;

            // Initialize generator
            this.chksumGen.Initialize(de, parms.Hash);

            // Generate sum
            md5sum = this.chksumGen.ProduceSum(parms.Callback);

            return md5sum;
        }

        private void UpdateProgressBar(int pValue)
        {
            this.bwGenerator.ReportProgress(pValue);
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            this.PrepareForGeneration();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (this.isStandalone)
            {
                this.btnCancel.Content = "Exit";
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.isStandalone)
            {
                Application.Current.Shutdown();
            }
        }

        #region Event Handlers

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
                this.progressBar.Value = e.ProgressPercentage;
            }
        }

        private void bwGenerator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;

            if (e.Cancelled)
            {
                MessageBox.Show(this, "Checksum Halted", Config.APPNAME,
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!(e.Error == null))
            {
                MessageBox.Show(this, "Error: " + e.Error.Message, Config.APPNAME,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.SaveMd5SumFile(this.saveFileDialog.FileName, e.Result as string);
                this.btnGenerate.Visibility = Visibility.Hidden;
                this.btnCancel.Content = "Exit";
            }

            this.btnGenerate.IsEnabled = true;
        }

        private void bwGenerator_DoWork(object sender, DoWorkEventArgs e)
        {
            ChecksumParameters param = e.Argument as ChecksumParameters;

            var de = new AxsUtils.DirectoryElements();
            de.Scan(param.SumDir, param.CheckSubdirs);

            // Update maximum value of progress bar
            this.bwGenerator.ReportProgress(-de.NrOfFiles);

            e.Result = this.Generate(param, de);
        }

        private void btnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            this.txtFolder.Text = GUI.SelectFolderPath(this,
                Environment.GetFolderPath(Environment.SpecialFolder.Personal), false);
        }

        #endregion
    }

    internal sealed class ChecksumParameters
    {
        public DirectoryInfo SumDir { get; set; }
        public CallbackEntry Callback { get; set; }
        public SupportedHashAlgo Hash { get; set; }
        public bool CheckSubdirs { get; set; }
    }
}
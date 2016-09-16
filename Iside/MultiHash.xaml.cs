using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using AxsUtils;
using Iside.Logic;
using Iside.Properties;
using LLCryptoLib;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;
using WPFUtils;

namespace Iside
{
    /// <summary>
    ///     Interaction logic for MultiHash.xaml
    /// </summary>
    public partial class MultiHash : Window
    {
        private BackgroundWorker backgroundWorker;
        private readonly DirectoryInfo cdromDirectory;
        private AutoResetEvent reset;

        public MultiHash()
        {
            this.InitializeComponent();
            this.Init();
        }

        /// <summary>
        ///     MultiHashForm for CDROM
        /// </summary>
        /// <param name="di">Path to CDROM file system</param>
        public MultiHash(DirectoryInfo di)
        {
            this.InitializeComponent();
            this.Init();

            this.Title = "DVD/CD-ROM Hash Computer";
            this.btnAddFiles.Visibility = Visibility.Hidden;
            this.btnAddDir.Visibility = Visibility.Hidden;
            this.lstFiles.Items.Add("Reading DVD/CDROM files list...");
            this.cdromDirectory = di;
        }

        private void Init()
        {
            this.reset = new AutoResetEvent(false);
            this.lstFiles.ContextMenuOpening += this.lstFiles_ContextMenuOpening;
            this.RefreshStatusBar();

            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += this.backgroundWorker_DoWork;
            this.backgroundWorker.ProgressChanged += this.backgroundWorker_ProgressChanged;
            this.backgroundWorker.RunWorkerCompleted += this.backgroundWorker_RunWorkerCompleted;
        }

        private void lstFiles_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            string selItem = (string) this.lstFiles.SelectedItem;
            if (selItem != null)
            {
                if (selItem.Length > 0)
                {
                    this.ctxMenuRemoveItem.IsEnabled = true;
                }
                else
                {
                    this.ctxMenuRemoveItem.IsEnabled = false;
                }
            }
        }

        private void InitHashCombo()
        {
            string[] availableHashAlgos = SupportedHashAlgorithms.GetNoKeyedHashAlgorithms();
            foreach (string s in availableHashAlgos)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = s;
                this.cboHash.Items.Add(cbi);
            }
            AvailableHash primaryHash = Settings.Default.PrimaryHash;
            HashLogic.SyncHashCombo(primaryHash, this.cboHash);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = false;
            folderBrowser.Description = "Please select a directory:";
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.BuildFileList(new DirectoryInfo(folderBrowser.SelectedPath));
            }

            this.RefreshStatusBar();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.InitHashCombo();
        }

        private void Reset()
        {
            this.progressBar.Value = this.progressBar.Minimum;
            this.txtHash.Text = string.Empty;
        }

        private void AddFiles()
        {
            if (this.progressBar.Value == this.progressBar.Maximum)
            {
                this.Reset();
            }

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string s in openFileDialog.FileNames)
                {
                    if (!this.lstFiles.Items.Contains(s))
                    {
                        this.lstFiles.Items.Add(s);
                    }
                }
            }

            this.RefreshStatusBar();
        }

        private void AsyncComputeHashes(MultiHashParameters mhp)
        {
            this.backgroundWorker.RunWorkerAsync(mhp);
        }

        /// <summary>
        ///     Set the value of the progress bar.
        /// </summary>
        /// <param name="i">
        ///     Value of the progress bar. If negative, set the maximum value
        ///     of the progress bar to -i
        /// </param>
        private void SetProgressBar(int i)
        {
            this.backgroundWorker.ReportProgress(i);
        }

        private void GoHashing()
        {
            int nr = this.lstFiles.Items.Count;

            if (nr > 0)
            {
                // Get files list
                FileInfo[] files = new FileInfo[nr];
                for (int j = 0; j < nr; j++)
                {
                    files[j] = new FileInfo((string) this.lstFiles.Items[j]);
                }

                this.Reset();

                // Get hash algorithm
                Hash hash = new Hash();
                GUI.SetCursorWait(true);
                string algo = ((ComboBoxItem) this.cboHash.SelectedItem).Content as string;
                SupportedHashAlgo hashAlgo = SupportedHashAlgoFactory.FromName(algo);
                hash.SetAlgorithm(hashAlgo.Id);
                this.progressBar.Maximum = nr;

                // Set callback delegate
                CallbackEntry cbe = this.SetProgressBar;

                // Get hash style
                HexEnum hs = Settings.Default.HashStyle;

                MultiHashParameters mhp = new MultiHashParameters(files, hash, hs, cbe);
                this.txtHash.Text = "Computing...";
                this.statusBar.Content = "Computing hash...";
                this.AsyncComputeHashes(mhp);
            }
        }

        private void RefreshStatusBar()
        {
            int totalFiles = this.lstFiles.Items.Count;
            if (totalFiles > 0)
            {
                this.statusBar.Content = totalFiles + " files to hash.";
            }
            else
            {
                this.statusBar.Content = "Ready";
            }
        }

        private void BuildFileList(DirectoryInfo di)
        {
            this.lstFiles.Items.Clear();

            // Disable buttons
            this.btnAddDir.IsEnabled = false;
            this.btnGo.IsEnabled = false;
            this.btnAddFiles.IsEnabled = false;
            GUI.SetCursorWait(true);

            try
            {
                AxsUtils.DirectoryElements de = new AxsUtils.DirectoryElements();
                de.Scan(di, true);
                this.lstFiles.BeginInit();
                foreach (FileInfo fi in de.Files)
                {
                    this.lstFiles.Items.Add(fi.FullName);
                }
                this.lstFiles.EndInit();
            }
            catch (AxsException)
            {
                GUI.SetCursorWait(false);
                MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME,
                    "The list contain access denied or protected files. Operation aborted.",
                    IsideLogic.Config.APPNAME, TaskDialogType.ERROR);
            }
            finally
            {
                GUI.SetCursorWait(false);

                // Enable buttons
                this.btnAddDir.IsEnabled = true;
                this.btnGo.IsEnabled = true;
                this.btnAddFiles.IsEnabled = true;
            }
        }

        private void btnAddFiles_Click(object sender, RoutedEventArgs e)
        {
            this.AddFiles();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            this.GoHashing();
        }

        private void cboHash_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.progressBar.Value == this.progressBar.Maximum)
            {
                this.Reset();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            MultiHashParameters mhp = e.Argument as MultiHashParameters;
            Hash hash = mhp.Hash;
            e.Result = hash.ComputeHashFiles(mhp.Files, mhp.HexStyle, mhp.Callback, this.reset);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int i = e.ProgressPercentage;

            if (i > this.progressBar.Maximum)
            {
                return;
            }

            if (i >= 0)
            {
                this.progressBar.Value = i;
            }
            else
            {
                this.progressBar.Maximum = -i;
                this.progressBar.Value = 0;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GUI.SetCursorWait(false);
            this.progressBar.Value = this.progressBar.Maximum;

            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME, "Process was canceled by user",
                    "Operation aborted", TaskDialogType.INFO);
            }
            else if (e.Error != null) // this will print Exceptions thrown in the DoWork phase
            {
                // There was an error during the operation.
                MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME, e.Error.Message, "Hash error",
                    TaskDialogType.ERROR);
            }
            else
            {
                this.txtHash.Text = e.Result as string;
            }

            this.statusBar.Content = "Done.";
        }

        private void Ctx_MoreFiles_Click(object sender, RoutedEventArgs e)
        {
            this.AddFiles();
        }

        private void Ctx_Remove_Click(object sender, RoutedEventArgs e)
        {
            this.lstFiles.Items.Remove((string) this.lstFiles.SelectedItem);
        }

        private void Ctx_Clear_Click(object sender, RoutedEventArgs e)
        {
            this.lstFiles.Items.Clear();
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            if (this.cdromDirectory != null)
            {
                this.BuildFileList(this.cdromDirectory);
            }

            this.Activated -= this.Window_Activated_1;
        }
    }
}
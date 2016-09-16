/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AxsUtils;
using Iside;
using Iside.Logic;
using IsideFolder.Properties;
using IsideLogic;
using LLCryptoLib.Hash;
using Microsoft.WindowsAPICodePack.Taskbar;
using MS.WindowsAPICodePack.Internal;
using WPFUtils;

namespace IsideFolder
{
    enum Semaphor
    {
        BLUE,
        RED,
        GREEN
    }

    /// <summary>
    /// Interaction logic for IsideFolder.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FolderComparer folderComparison;
        private AvailableHash folderHashOption;
        private FolderHashAlgorithms hashOption;
        private BackgroundWorker backgroundWorker;
        private bool unattendedExecution;
        private string logDirectory;
        private bool initializing;
        private bool isWin7;
        private string leftFolderPath;
        private string rightFolderPath;
        private RecentItems recentComparisons;

        // Command enablers
        private bool canCompareNow;
        private bool canSelectComparisonFolder;
        private bool canOpenResultsList;
        private bool canShowLog;
        private bool canViewResults;

        #region Constructors

        public MainWindow()
        {
            this.Init(false);
        }

        public MainWindow(bool isRegistered)
        {
            this.Init(isRegistered);
        }

        /// <summary>
        /// Constructor with filename
        /// </summary>
        /// <param name="filename">The .iscmp comparison to open, or the *.isrl results list to open</param>
        public MainWindow(string filename, bool isRegistered)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            this.Init(isRegistered);

            if (File.Exists(filename))
            {
                if (filename.EndsWith(".isrl"))
                {
                    this.OpenResultsList(filename);
                }
                else if (filename.EndsWith(".iscmp"))
                {
                    this.OpenComparison(filename);
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:IsideFolderForm"/> class.
        /// </summary>
        /// <param name="filenameLeft">The filename left.</param>
        /// <param name="fileNameRight">The file name right.</param>
        public MainWindow(string filenameLeft, string filenameRight, bool isRegistered)
        {
            this.Init(isRegistered);

            if (FileManager.Exists(filenameLeft))
            {
                if (FileManager.Exists(filenameRight))
                {
                    this.LeftFolder = filenameLeft;
                    this.RightFolder = filenameRight;
                    this.CompareRefresh();
                }
                else
                {
                    throw new AxsException(filenameRight + " does not exist.");
                }
            }
            else
            {
                throw new AxsException(filenameLeft + " does not exist.");
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Directory in which saving logs
        /// </summary>
        public string LogDirectory
        {
            get
            {
                return this.logDirectory;
            }

            set
            {
                this.logDirectory = value;
            }
        }

        /// <summary>
        /// Unattended execution
        /// </summary>
        public bool Unattended
        {
            get
            {
                return this.unattendedExecution;
            }

            set
            {
                this.unattendedExecution = value;
                this.Title = this.Title + " - Unattended Execution ";
            }
        }

        /// <summary>
        /// Path of folder 1
        /// </summary>
        public string LeftFolder
        {
            set
            {
                this.leftFolderPath = value;
                this.txtPath1.Text = this.leftFolderPath;

                if (!String.IsNullOrEmpty(value))
                {
                    this.txtPath1.ToolTip = value;

                    this.canSelectComparisonFolder = true;
                    this.canOpenResultsList = true;

                    this.CompareRefresh();
                }
            }

            get
            {
                return this.leftFolderPath;
            }
        }


        /// <summary>
        /// Path of folder 2
        /// </summary>
        public string RightFolder
        {
            set
            {
                this.rightFolderPath = value;
                this.txtPath2.Text = this.rightFolderPath;
                if (!String.IsNullOrEmpty(value))
                {
                    this.txtPath2.ToolTip = value;
                    if (value.EndsWith(".isrl"))
                    {
                        this.txtPath2.Background = new SolidColorBrush(Colors.WhiteSmoke);
                    }
                    else
                    {
                        this.txtPath2.Background = new SolidColorBrush(Colors.White);
                    }

                    this.CompareRefresh();
                }
            }

            get
            {
                return this.rightFolderPath;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the comparison.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void OpenComparison(string filename)
        {
            Comparison loaded = null;

            loaded = Comparison.FromFile(filename);
            if (loaded != null)
            {
                this.OpenComparison(loaded);
            }
        }

        /// <summary>
        /// Opens the results list.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void OpenResultsList(string fileName)
        {
            this.RightFolder = fileName;
            this.CompareRefresh();
        }

        public void CompareRefresh()
        {
            this.canViewResults = false;
            this.canShowLog = false;

            this.ResetPicturesColor();

            if (!String.IsNullOrEmpty(this.LeftFolder) && (!String.IsNullOrEmpty(this.RightFolder)))
            {
                GUIUtils.SetCursorWait(true);

                this.canCompareNow = true;

                // Try to setup this new comparison 
                if (!this.CreateNewComparison())
                {
                    GUIUtils.SetCursorWait(false);
                    this.Reset();
                    return;
                }

                if (this.folderComparison.ErrorMessage.Length > 0)
                {
                    this.folderComparison.Log.AddLineToFooter(this.folderComparison.ErrorMessage);
                    if (!this.unattendedExecution)
                    {
                        MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, 
                                               this.folderComparison.ErrorMessage, 
                                               "Comparison Error", TaskDialogType.ERROR);
                    }
                }
                else
                {
                    long nrFiles1 = this.folderComparison.DirElements1.NrOfFiles;
                    long nrFiles2 = 0;
                    int nrFolders2 = 0;

                    if (this.RightFolder.EndsWith(".isrl"))
                    {
                        this.txtNrSubDirs2.Text = "-";
                        // Reading results file to get results and saved hash code
                        ResultsFile rf = FolderComparer.ReadResultsFile(this.RightFolder);
                        if (rf != null)
                        {
                            nrFiles2 = rf.NrOfFiles;
                            nrFolders2 = rf.NrOfFolders;
                        }
                    }
                    else
                    {
                        nrFiles2 = this.folderComparison.DirElements2.NrOfFiles;
                        nrFolders2 = this.folderComparison.DirElements2.NrOfSubdirectories;
                    }

                    this.txtNrFiles1.Text = String.Format("{0}", nrFiles1);
                    this.txtNrFiles2.Text = String.Format("{0}", nrFiles2);
                    this.txtNrSubDirs1.Text = String.Format("{0}", this.folderComparison.DirElements1.NrOfSubdirectories);
                    this.txtNrSubDirs2.Text = String.Format("{0}", nrFolders2);

                    if (nrFiles1 == nrFiles2)
                    {
                        this.progressMainBar.Maximum = (int)nrFiles1;
                        this.progressMainBar.Minimum = 1;
                        this.canCompareNow = true;
                    }
                    else
                    {
                        this.folderComparison.Log.AddLineToFooter("Specyfied folders contain different items");
                        if (!this.unattendedExecution)
                        {
                            MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Specyfied folders contain different items", "Comparison Error", TaskDialogType.ERROR);
                            this.SetPictureColor("Error", true);
                        }
                    }
                }

                GUIUtils.SetCursorWait(false);
                CommandManager.InvalidateRequerySuggested();

            }
        }

        #endregion

        #region Private Methods

        private void PerformUnattendedExecution()
        {
            string resultSummary;
            string detailedSummary;

            if (this.txtNrFiles1.Text.StartsWith("0"))
            {
                resultSummary = "The selected directories are empty.";
                detailedSummary = resultSummary;
            }
            else
            {
                this.SyncComparison();
            }


            FileManager fm = FileManager.Reference;
            DateTime now = DateTime.Now;
            string resultsFileName = this.logDirectory + @"\" + FilenameFromDate(now) + "_results.txt";
            string logFileName = this.logDirectory + @"\" + FilenameFromDate(now) + "_log.txt";
            Console.WriteLine("Saving results to {0}", resultsFileName);

            resultSummary = this.folderComparison.Summary;
            detailedSummary = this.folderComparison.Log.SummaryAsText();

            fm.SaveFile(resultsFileName, resultSummary, false);
            Console.WriteLine("Saving log to {0}", logFileName);
            fm.SaveFile(logFileName, detailedSummary, false);
            Console.WriteLine("Done.");
        }

        private void ShowRegisteredControls(bool registered)
        {
            System.Windows.Visibility visible = System.Windows.Visibility.Visible;
            if (registered)
            {
                visible = System.Windows.Visibility.Collapsed;
            }

            this.mnuBuyNow.Visibility = visible;
            this.mnuActivate.Visibility = visible;
            this.mnuOrderingInfo.Visibility = visible;
            this.sepOrders.Visibility = visible;
        }

        private void SyncComparison()
        {
            this.AsyncComparison(null);
        }

        private void AsyncComparison(BackgroundWorker bw)
        {
            if (this.RightFolder.EndsWith(".isrl"))
            {
                this.folderComparison.CompareAll(this.RightFolder, bw);
            }
            else
            {
                this.folderComparison.CompareAll(bw);
            }
        }


        private void DoDragDrop(object sender, DragEventArgs e)
        {
            try
            {
                TextBox txtControl = sender as TextBox;

                // Data dropped is one directory?
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    if (a.Length == 1)
                    {
                        string filename = a.GetValue(0).ToString();
#if DEBUG
                        Console.WriteLine("I'm doing drag/drop of {0}", filename);
#endif

                        if (filename.EndsWith("isrl") && txtControl.Name == "txtPath2")
                        {
                            txtControl.Text = filename;
                        }
                        else if (Directory.Exists(filename))
                        {
                            txtControl.Text = filename;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }
        }

        private void DoDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void FinalizeComparison()
        {
            bool bName = this.folderComparison.NameEquality;
            bool bSize = this.folderComparison.SizeEquality;
            bool bHash = this.folderComparison.HashEquality;

            this.SetPictureColor("Name", bName);
            this.SetPictureColor("Size", bSize);
            this.SetPictureColor("Hash", bHash);

            this.canViewResults = true;
            this.canShowLog = true;
            this.canCompareNow = false;

            CommandManager.InvalidateRequerySuggested();
        }

        private void OpenResultsList()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = false;
            dlg.DefaultExt = ".isrl";
            dlg.Filter = "Iside Folder Results List (*.isrl)|*.isrl";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                this.OpenResultsList(dlg.FileName);
            }
        }

        private string SaveResultsList()
        {
            string resultListPath = null;

            if (this.folderComparison != null)
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.DefaultExt = "isrl";
                saveFileDialog.Filter = "Iside Folder Results (*.isrl)|*.isrl";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                Nullable<bool> result = saveFileDialog.ShowDialog();

                if (result == true)
                {
                    int nrOfFiles = Int32.Parse(this.txtNrFiles1.Text);
                    int nrOfFolders = Int32.Parse(this.txtNrSubDirs1.Text);
                    if (this.folderComparison.SaveResults(saveFileDialog.FileName, nrOfFolders, nrOfFiles))
                    {
                        MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Results file " + saveFileDialog.FileName + " saved.", "Results saved", TaskDialogType.INFO);
                        resultListPath = saveFileDialog.FileName;
                    }
                    else
                    {
                        MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Cannot save results to: " + saveFileDialog.FileName, "Results file error", TaskDialogType.ERROR);
                    }
                }
            }

            return resultListPath;
        }


        private void FolderSelect(int folderNumber, string folderPath)
        {
            if (!String.IsNullOrEmpty(folderPath))
            {
                if ((folderNumber == 2) && (folderPath.EndsWith("isrl")))
                {
                    // Compare with a results list
                    if (File.Exists(folderPath))
                    {
                        this.RightFolder = folderPath;
                    }
                    else
                    {
                        this.txtPath2.Text = String.Empty;
                        MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Results list " + folderPath + " does not exist.", "File not found", TaskDialogType.ERROR);
                    }
                }
                else
                {
                    DirectoryInfo di = null;

                    try
                    {
                        di = new DirectoryInfo(folderPath);
                    }
                    catch (NotSupportedException) { }

                    if ((di != null) && (di.Exists))
                    {
                        if (folderNumber == 1)
                        {
                            this.LeftFolder = folderPath;
                        }
                        else
                        {
                            this.RightFolder = folderPath;
                        }
                        this.CompareRefresh();
                    }
                    else
                    {
                        if (folderNumber == 1)
                        {
                            this.txtPath1.Text = String.Empty;
                        }
                        else
                        {
                            this.txtPath2.Text = String.Empty;
                        }
                        MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, 
                            "Directory " + folderPath + " does not exist.", "Folder not found", TaskDialogType.WARNING);
                    }
                }
            }
        }

        private void OpenComparison()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = false;
            dlg.DefaultExt = ".iscmp";
            dlg.Filter = "Iside Folder Comparison (*.iscmp)|*.iscmp|All documents (*.*)|*.*"; // Filter files by extension 
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                this.OpenComparison(dlg.FileName);
            }
        }

        private void OpenComparison(Comparison c)
        {
            bool success = true;

            if (c.RightPath.Exists)
            {
                this.RightFolder = c.RightPath.FullName;
            }
            else
            {
                success = false;
                if (this.folderComparison != null)
                {
                    this.folderComparison.Log.AddLineToFooter(c.RightPath.FullName + " not found.");
                    if (!this.unattendedExecution)
                    {
                        MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, c.RightPath.FullName + " not found.", "File not found", TaskDialogType.ERROR);
                    }
                }
            }

            if (c.LeftPath.Exists)
            {
                this.LeftFolder = c.LeftPath.FullName;
            }
            else
            {
                success = false;
                if (this.folderComparison != null)
                {
                    this.folderComparison.Log.AddLineToFooter(c.LeftPath.FullName + " not found.");
                }

                if (!this.unattendedExecution)
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, c.LeftPath.FullName + " not found.", "File not found", TaskDialogType.WARNING);
                }
            }

            if (!success)
            {
                this.Title = Config.APPFOLDER;
                if (!this.initializing)
                {
                    this.Reset();
                }
            }
            else
            {
                this.Title = Config.APPFOLDER + " - " + c.Name;
                this.progressMainBar.Value = 0;
            }

        }

        private bool CreateNewComparison()
        {
            bool comparisonCreated = false;

            try
            {
                DetailedComparison newComparison = new DetailedComparison(this.LeftFolder, this.RightFolder);
                newComparison.IncludeHidden = this.mnuCheckHidden.IsChecked;
                newComparison.IncludeSystem = this.mnuCheckSystem.IsChecked;
                newComparison.IncludeArchive = this.mnuCheckArchive.IsChecked;

                int folders = -1;
                int files = -1;
                try
                {
                    folders = Int32.Parse(this.txtNrSubDirs1.Text);
                    files = Int32.Parse(this.txtNrFiles1.Text);
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.Message);
                }

                newComparison.NrOfFolders = folders;
                newComparison.NrOfFiles = files;
                AvailableHash hashAlgo = Settings.Default.FolderHash;

                this.folderComparison = new FolderComparer(newComparison, hashAlgo);
                this.folderComparison.ExitOnFirstDiff = this.mnuExitOnFirstDiff.IsChecked;
                comparisonCreated = true;
            }
            catch (IsideException)
            {
                MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "The folders contain access denied or protected files. Operation aborted.", "Comparison Error", TaskDialogType.ERROR);
            }
            catch (ArgumentException)
            {
                MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Illegal characters in path. Operation aborted.", "Comparison Error", TaskDialogType.ERROR);
            }

            return comparisonCreated;
        }

        private void SetSpecificSemaphor(string what, Semaphor color)
        {
            if (what.Equals("Name"))
            {
                this.SetSemaphor(this.semaphorName, color);
            }
            else if (what.Equals("Size"))
            {
                this.SetSemaphor(this.semaphorSize, color);
            }
            else if (what.Equals("Hash"))
            {
                this.SetSemaphor(this.semaphorHash, color);
            }
            else if (what.Equals("Error"))
            {
                this.SetSemaphors(Semaphor.RED, Semaphor.RED, Semaphor.RED);
            }
        }

        private void SetPictureColor(string what, bool condition)
        {
            Semaphor color = Semaphor.GREEN;
            if (!condition)
                color = Semaphor.RED;

            this.SetSpecificSemaphor(what, color);
        }

        private void Reset()
        {
            this.LeftFolder = "";
            this.RightFolder = "";
            this.txtNrFiles1.Text = "-";
            this.txtNrFiles2.Text = "-";
            this.txtNrSubDirs1.Text = "-";
            this.txtNrSubDirs2.Text = "-";
            this.SetWin7TaskbarValue(0, 100);
            this.ResultsReset();
        }

        private void InitializeBackgroundWorker()
        {
            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
        }

        private void Init(bool isRegistered)
        {

            try
            {
                InitializeComponent();

                this.initializing = true;
                this.InitializeBackgroundWorker();
                this.isWin7 = CoreHelpers.RunningOnWin7;
                Settings reg = Settings.Default;

                this.folderHashOption = reg.FolderHash;

                this.mnuCheckSystem.IsChecked = reg.ScanSystemFiles;
                this.mnuCheckHidden.IsChecked = reg.ScanHiddenFiles;
                this.mnuCheckArchive.IsChecked = reg.ScanArchiveFiles;

                this.folderComparison = null;
                this.Title = IsideLogic.Config.APPFOLDER;
                ResetPicturesColor();
                this.hashOption = new FolderHashAlgorithms();
                this.InitializeComboHash();

                // Comparisons
                this.recentComparisons = new RecentItems();
                this.RefreshRecentComparisons();

                // Registered controls
                this.ShowRegisteredControls(isRegistered);
            }
            catch (InvalidOperationException ioe)
            {
                System.Diagnostics.Debug.WriteLine(ioe.Message);
            }

        }

        private void RefreshRecentComparisons()
        {
            this.mnuRecentItems.Items.Clear();
            foreach (string item in this.recentComparisons.Items)
            {
                System.Diagnostics.Debug.WriteLine("-- Menu item add > " + item);
                MenuItem newMenuItem = new MenuItem();
                MenuItem recentItems = this.mnuRecentItems;
                newMenuItem.Header = item;
                newMenuItem.Click += recentMenuItem_Click;
                recentItems.Items.Add(newMenuItem);
            }
        }

        private void InitializeComboHash()
        {
            // Add items
            string[] hashes = this.hashOption.GetAvailableHashes();
            GUI.ComboBoxItemsAdd(this.toolStripCboAlgo, hashes);
            this.SetFolderHashAlgorithm(this.hashOption.DefaultHash);
        }

        private void SaveComparisonAs()
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".iscmp"; // Default file extension
            dlg.Filter = "Iside Folder Comparison (*.iscmp)|*.iscmp";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                this.SaveComparison(filename);

                // Recent comparisons
                this.recentComparisons.AddItem(filename);
                this.RefreshRecentComparisons();
            }
        }

        private void New()
        {
            this.folderComparison = null;
            this.Title = IsideLogic.Config.APPFOLDER;
            this.Reset();
        }

        private void SaveComparison(string fileName)
        {
            Comparison actual = this.folderComparison.ActualComparison;

            if (actual != null)
            {
                if (!actual.Save(fileName))
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Cannot save comparison file", "File error", TaskDialogType.ERROR);
                }
                else
                {
                    if (!String.IsNullOrEmpty(actual.Name))
                    {
                        this.Title = Config.APPFOLDER + " - " + actual.Name;
                    }
                }
            }
        }

        private void SaveComparison(Stream comparisonStream)
        {
            Comparison actual = this.folderComparison.ActualComparison;

            if (actual != null)
            {
                if (!actual.Save(comparisonStream))
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Cannot save comparison file", "File error", TaskDialogType.ERROR);
                }
            }
        }

        private void SetFolderHashAlgorithm(AvailableHash hash)
        {
            this.SetFolderHashAlgorithm(SupportedHashAlgoFactory.GetAlgo(hash).Name);
        }

        private void SetFolderHashAlgorithm(string newAlgo)
        {
            this.ResultsReset();

            // Synchromize toolstrip combo
            for (int j = 0; j < this.toolStripCboAlgo.Items.Count; j++)
            {
                ComboBoxItem currentItem = this.toolStripCboAlgo.Items[j] as ComboBoxItem;
                if ((string)currentItem.Content == newAlgo)
                {
                    this.toolStripCboAlgo.SelectedIndex = j;
                    break;
                }
            }

            // Set in registry
            this.hashOption.DefaultHash = newAlgo;

            // Set in program, if a comparison exists
            if (this.folderComparison != null)
            {
                this.folderComparison.HashAlgorithm = SupportedHashAlgoFactory.FromName(newAlgo).Id;
            }

            this.CompareRefresh();
        }

        private void ResultsReset()
        {
            this.progressMainBar.Value = this.progressMainBar.Minimum;
            this.ResetPicturesColor();
            this.folderComparison = null;
            this.canViewResults = false;
            this.canShowLog = false;
            this.canCompareNow = false;
        }

        private void SetSemaphors(Semaphor name, Semaphor size, Semaphor hash)
        {
            this.SetSemaphor(this.semaphorName, name);
            this.SetSemaphor(this.semaphorSize, size);
            this.SetSemaphor(this.semaphorHash, hash);
        }

        private void ViewLog()
        {
            string leftPath = this.LeftFolder;
            string rightPath = this.RightFolder;

            if (this.folderComparison != null)
            {
                if (this.folderComparison.HasLog)
                {
                    FileSummary fs = new FileSummary("Detailed Log", this.txtPath1.Text, this.txtPath2.Text);
                    this.folderComparison.Log.RowsNumber = this.folderComparison.DirElements1.NrOfFiles;
                    fs.SummaryDetails(this.folderComparison.Log);
                    fs.Summary = this.folderComparison.Log.SummaryAsText();
                    fs.Owner = this;
                    fs.ShowDialog();
                }
            }
        }

        private void ResetPicturesColor()
        {
            this.SetSemaphors(Semaphor.BLUE, Semaphor.BLUE, Semaphor.BLUE);
        }

        private void ResetStatusBar()
        {
            this.statusBar.Content = "Ready";
        }

        private void CompareNow()
        {
            this.CompareRefresh();

            if (this.folderComparison != null)
            {
                if (this.backgroundWorker.IsBusy)
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Please wait until the current comparison is finished", "Another comparison is running...", TaskDialogType.WARNING);
                }
                else
                {
                    if (this.txtNrFiles1.Text.StartsWith("0"))
                    {
                        MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "The selected directories contain no files", "No file to be compared.", TaskDialogType.WARNING);
                    }
                    else
                    {
                        GUIUtils.SetCursorWait(true);
                        if (this.RightFolder.EndsWith(".isrl"))
                        {
                            this.statusBar.Content = "Comparing folder against a saved results list";
                        }
                        else
                        {
                            this.statusBar.Content = "Comparing folders...";
                        }
                        this.backgroundWorker.RunWorkerAsync();
                    }
                }
            }
        }

        private void ViewResults()
        {
            if (this.folderComparison != null)
            {
                if (this.folderComparison.Summary.Length > 0)
                {
                    FileSummary fs = new FileSummary("Results", this.txtPath1.Text, this.txtPath2.Text);
                    fs.Summary = this.folderComparison.Summary;
                    fs.Owner = this;
                    fs.ShowDialog();
                }
            }
        }

        private void UpdateProgressBar(int point)
        {
            if (point > this.progressMainBar.Maximum)
            {
                return;
            }

            if (point >= 0)
            {
                this.progressMainBar.Value = point;
            }
            else
            {
                this.progressMainBar.Value = this.progressMainBar.Minimum;
            }

            this.SetWin7TaskbarValue(this.progressMainBar.Value, this.progressMainBar.Maximum);
        }

        private void SetWin7TaskbarValue(double value, double maximum)
        {
            if (this.isWin7)
            {
                int iValue = Convert.ToInt32(value);
                int iMaximum = Convert.ToInt32(maximum);
                TaskbarManager.Instance.SetProgressValue(iValue, iMaximum);
            }
        }

        private void SetSemaphor(TextBox control, Semaphor color)
        {
            switch (color)
            {
                case Semaphor.BLUE:
                    control.Template = (ControlTemplate)FindResource("BlueSemaphor");
                    break;
                case Semaphor.RED:
                    control.Template = (ControlTemplate)FindResource("RedSemaphor");
                    break;
                case Semaphor.GREEN:
                    control.Template = (ControlTemplate)FindResource("GreenSemaphor");
                    break;
            }
        }

        private static string FilenameFromDate(DateTime dt)
        {
            StringBuilder db = new StringBuilder();

            db.Append(dt.Year);
            db.Append(dt.Month);
            db.Append(dt.Day);
            db.Append("_");
            db.Append(dt.Hour);
            db.Append(dt.Minute);
            db.Append(dt.Second);

            return db.ToString();
        }

        #endregion

        #region Event Handlers

        private void txtPath1_DragEnter(object sender, DragEventArgs e)
        {
            this.DoDragEnter(sender, e);
        }

        private void txtPath1_Drop(object sender, DragEventArgs e)
        {
            this.DoDragDrop(sender, e);
            this.FolderSelect(1, this.txtPath1.Text);
        }

        private void txtPath2_Drop(object sender, DragEventArgs e)
        {
            this.DoDragDrop(sender, e);
            this.FolderSelect(2, this.txtPath2.Text);
        }

        private void txtPath2_DragEnter(object sender, DragEventArgs e)
        {
            this.DoDragEnter(sender, e);
        }

        private void txtPath1_LostFocus(object sender, RoutedEventArgs e)
        {
            this.FolderSelect(1, this.txtPath1.Text);
        }

        private void txtPath2_LostFocus(object sender, RoutedEventArgs e)
        {
            this.FolderSelect(2, this.txtPath2.Text);
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            this.ResetStatusBar();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GUIUtils.SetCursorWait(false);

            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Process was canceled by user", "Operation aborted", TaskDialogType.WARNING);
            }
            else if (e.Error != null) // this will print Exceptions thrown in the DoWork phase
            {
                // There was an error during the operation.
                MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, e.Error.Message, "Comparison Error", TaskDialogType.ERROR);
            }
            else
            {
                this.FinalizeComparison();
            }

            this.statusBar.Content = "Ready";
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!this.unattendedExecution)
            {
                this.UpdateProgressBar(e.ProgressPercentage);
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            this.AsyncComparison(bw);
        }

        private void recentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (!String.IsNullOrEmpty((string)item.Header))
            {
                if (File.Exists((string)item.Header))
                {
                    this.OpenComparison((string)item.Header);
                }
                else
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "The comparison file does not exist anymore", "File error", TaskDialogType.ERROR);
                    this.recentComparisons.RemoveItem((string)item.Header);
                    this.RefreshRecentComparisons();
                }
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("-- Saving recent items --");
            this.recentComparisons.Save();

            Settings.Default.FolderWinLeft = this.Left;
            Settings.Default.FolderWinTop = this.Top;
            Settings.Default.Save();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            this.Activated -= new System.EventHandler(this.MainWindow_Activated);

            if (this.unattendedExecution)
            {
                if (this.logDirectory == null)
                {
                    throw new ArgumentNullException("logDirectory");
                }

                this.WindowState = System.Windows.WindowState.Minimized;
                this.PerformUnattendedExecution();
                this.Close();
                Application.Current.Shutdown();
            }

            this.initializing = false; // Initialization phase ends here

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.FolderWinLeft > 0)
            {
                this.Left = Settings.Default.FolderWinLeft;
                this.Top = Settings.Default.FolderWinTop;
            }
            else
            {
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            }
        }

        #endregion

        #region Menu StatusBar Helper
        private void MenuHelper_StatusBar(object sender, MouseEventArgs e)
        {
            MenuItem sendM = (MenuItem)sender;
            RoutedUICommand cmd = (RoutedUICommand)sendM.Command;
            this.statusBar.Content = cmd.Text;
        }
        #endregion

        #region Command Event Handlers

        private void Folder_RunCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.canCompareNow;
        }

        private void Folder_RunExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.CompareNow();
        }

        private void Folder_RefreshCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_RefreshExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.CompareRefresh();
        }

        private void Folder_IsideFilesCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_IsideFilesExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Process: run Iside
            try
            {
                string isideExePath = "Iside.exe";
#if (DEBUG)
                isideExePath = @"D:\Codice\VisualStudio\Iside\Iside\bin\Debug\Iside.exe";
#endif
                if (File.Exists(isideExePath))
                {
                    Process.Start(isideExePath);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Cannot find " + isideExePath);
                }
            }
            catch (System.ComponentModel.Win32Exception) { }
        }

        private void Folder_ViewResultsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.canViewResults;
        }

        private void Folder_ViewResultsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewResults();
        }

        private void Folder_ViewLogCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.canShowLog;
        }

        private void Folder_ViewLogExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.ViewLog();
        }

        private void Folder_Check_ExitOnFirstCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_Check_ExitOnFirstExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Nothing to do here. Checked is handled by WPF
        }

        private void Folder_Check_SystemCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_Check_SystemExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Nothing to do here. Checked is handled by WPF
        }

        private void Folder_Check_HiddenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_Check_HiddenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Nothing to do here. Checked is handled by WPF
        }

        private void Folder_Check_ArchiveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_Check_ArchiveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Nothing to do here. Checked is handled by WPF
        }

        private void HelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string currentPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            System.Windows.Forms.Help.ShowHelp(null, currentPath + @"\help\Iside.chm");
        }

        private void ChekForUpdatesCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChekForUpdatesExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CheckForUpdatesWPF.Updates.Check(this, IsideLogic.Config.AppInfo);
        }

        private void OrderingInfoCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OrderingInfoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                Process.Start(IsideLogic.Config.APPBUY);
            }
            catch (System.ComponentModel.Win32Exception) { }
        }

        private void BuyNowCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void BuyNowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void ActivateCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ActivateExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void IsideWebCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void IsideWebExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                Process.Start(IsideLogic.Config.APPURL);
            }
            catch (System.ComponentModel.Win32Exception) { }
        }

        private void LittleLiteWebCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void LittleLiteWebExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                Process.Start("http://www.littlelite.net/");
            }
            catch (System.ComponentModel.Win32Exception) { }
        }

        private void AboutCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AboutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            About aboutForm = new About();
            aboutForm.Owner = this;
            aboutForm.ShowDialog();
        }

        private void Folder_NewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.New();
        }

        private void Folder_OpenComparisonCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_OpenComparisonExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.OpenComparison();
        }

        private void Folder_SetOriginalCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_SetOriginalExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.LeftFolder = GUI.SelectFolderPath(this);
        }

        private void Folder_SetComparisonCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.canSelectComparisonFolder;
        }

        private void Folder_SetComparisonExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.RightFolder = GUI.SelectFolderPath(this);
        }

        private void Folder_SaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool canSaveAs = false;

            if (this.txtPath1 != null)
            {
                if (!String.IsNullOrEmpty(this.txtPath1.Text))
                {
                    if (!String.IsNullOrEmpty(this.txtPath2.Text))
                    {
                        canSaveAs = true;
                    }
                }
            }

            e.CanExecute = canSaveAs;
        }

        private void Folder_SaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SaveComparisonAs();
        }

        private void Folder_ResultsList_OpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.canOpenResultsList;
        }

        private void Folder_ResultsList_OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.OpenResultsList();
        }

        private void Folder_ResultsList_SaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.canViewResults;  // If you can view results, you can save them!
        }

        private void Folder_ResultsList_SaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SaveResultsList();
        }

        private void Folder_RecentComparisonsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_RecentComparisonsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Nothing to do. The command exist only to show info on the toolbar
        }

        private void Folder_ExitCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Folder_ExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void toolStripCboAlgo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedNewAlgo = this.toolStripCboAlgo.SelectedItem as ListBoxItem;
            string newAlgo = selectedNewAlgo.Content as string;
            if (!String.IsNullOrEmpty(newAlgo))
            {
                this.SetFolderHashAlgorithm(newAlgo);
            }
        }

        #endregion


    }

}

/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using ColorFont;
using Iside.Logic;
using Iside.Properties;
using IsideLogic;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using WPFUtils;

namespace Iside
{
    /// <summary>
    /// Iside Window code behind
    /// </summary>
    public partial class IsideWindow : Window
    {
        // Keep them private: must be accessed from accessors
        private SupportedHashAlgo prHash;
        private SupportedHashAlgo altHash;
        private HexEnum hexRegistry;
        private string[] reportToPrint;
        private HashLogic logic;
        private SaveFileDialog saveFileDialog;

        // Accessed directly from IsideLogic
        internal bool[] skipHashComputing = { false, false };
        internal FormWidth guiWidth;
        internal FileProperties[] fileProperties = { new FileProperties(), new FileProperties() };
        internal AutoResetEvent reset;
        internal bool canCompareNow;

        #region Constructors

        public IsideWindow()
        {
            this.Init(false);
        }

        public IsideWindow(bool isRegistered)
        {
            this.Init(isRegistered);
        }

        /// <summary>
        ///  Iside computing hash code for file1 with registry saved hash algo
        /// </summary>
        /// <param name="file1">The file1.</param>
        public IsideWindow(string file1, bool isRegistered)
        {
            this.Init(isRegistered);
            this.LeftFile = file1;
        }

        /// <summary>
        /// Iside computing hash code for file1 with algo by user
        /// </summary>
        /// <param name="file1"></param>
        public IsideWindow(string file1, AvailableHash hash, bool isRegistered)
        {
            this.Init(isRegistered);
            this.RefreshUI(hash, AvailableHash.FAKE);
            this.LeftFile = file1;
        }

        /// <summary>
        /// Iside computing hash code for file1 and file2 with registry saved hash algo
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        public IsideWindow(string file1, string file2, bool isRegistered)
        {
            this.Init(isRegistered);
            this.LeftFile = file1;
            this.RightFile = file2;
            if (this.guiWidth == FormWidth.SINGLE) // force double size, if this constructor is called
            {
                this.logic.SetSize(FormWidth.DOUBLE);
            }
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// File 1 to compare
        /// </summary>
        public string LeftFile
        {
            get
            {
                return this.fileProperties[0].FileName;
            }
            set
            {
                this.fileProperties[0].FileName = value;
                this.skipHashComputing[0] = false;
            }
        }

        /// <summary>
        /// File 2 to compare
        /// </summary>
        public string RightFile
        {
            get
            {
                return this.fileProperties[1].FileName;
            }
            set
            {
                this.fileProperties[1].FileName = value;
                this.skipHashComputing[1] = false;
            }
        }
        #endregion

        #region Internal Properties
        internal SupportedHashAlgo PrimaryHash
        {
            get
            {
                if (this.prHash == null)
                {
                    this.prHash = SupportedHashAlgoFactory.GetAlgo(Settings.Default.PrimaryHash);
                }
                return this.prHash;
            }
        }

        internal SupportedHashAlgo AlternativeHash
        {
            get
            {
                if (this.altHash == null)
                {
                    this.altHash = SupportedHashAlgoFactory.GetAlgo(Settings.Default.AlternativeHash);
                }

                return this.altHash;
            }
        }

        internal HexEnum HexadecimalStyle
        {
            get
            {
                this.hexRegistry = Settings.Default.HashStyle;
                return this.hexRegistry;
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

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            this.ResetStatusBar();
        }

        #endregion

        #region Commands Event Handlers

        private void Expander_Expanded_1(object sender, RoutedEventArgs e)
        {
            if (this.logic == null) return;
            this.logic.SetSize(FormWidth.DOUBLE);
        }

        private void Expander_Collapsed_1(object sender, RoutedEventArgs e)
        {
            if (this.logic == null) return;
            this.logic.SetSize(FormWidth.SINGLE);
            
        }

        private void NewHashCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsThereSomethingToHash;
        }

        private void NewHashExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.New();
        }

        private void OpenLeftCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenLeftExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.logic.OpenLeftFile();
        }

        private void OpenRightCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenRightExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.logic.OpenRightFile();
        }

        private void SaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsThereSomethingToHash;
        }

        private void SaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Save();
        }

        private void ExportReportCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsThereSomethingToHash;
        }

        private void ExportReportExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.ExportReport();
        }

        private void ReportPreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsThereSomethingToHash;
        }

        private void ReportPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.PreviewPrint();
        }

        private void PrintReportCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsThereSomethingToHash;
        }

        private void PrintReportExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.PrintReport();
        }

        private void OptionsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OptionsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Options();
        }

        private void ExitCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void HashNowCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsThereSomethingToHash;
        }

        private void HashNowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.HashNow();
        }

        private void CompareNowCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.canCompareNow == false)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = this.IsThereSomethingToCompare;
            }
        }

        private void CompareNowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.logic.CompareNow();
        }

        private void FoldersCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void FoldersExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Process -> Launch Iside Folder
            try
            {
                string isideExePath = "IsideFolder.exe";
                string startupPath = WPFUtils.Core.StartupPath;

#if (DEBUG)
                startupPath = @"D:\Codice\VisualStudio\Iside\IsideFolder\bin\Debug";
#endif
                String folderPath = Path.Combine(startupPath, isideExePath);

                if (File.Exists(folderPath))
                {
                    Process.Start(folderPath);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Cannot find " + folderPath);
                }
            }
            catch (System.ComponentModel.Win32Exception) { }
        }

        private void MultiFileCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void MultiFileExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MultiHash mhForm = new MultiHash();
            mhForm.Owner = this;
            mhForm.ShowDialog();
        }

        private void CdDvdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CdDvdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectCDROM selCDROM = new SelectCDROM();
            selCDROM.Owner = this;
            if (selCDROM.ShowDialog() == true)
            {
                DirectoryInfo dicd = selCDROM.SelectedDrive;
                bool ok = true;
                try
                {
                    // try to access CDROM
                    // if it's not inserted the program will recycle
                    int files = dicd.GetFiles().Length;
                }
                catch (IOException)
                {
                    ok = false;
                }

                if (ok)
                {
                    MultiHash mhf = new MultiHash(dicd);
                    mhf.Owner = this;
                    mhf.ShowDialog();
                }
                else
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME, "No CDROM/DVD found in drive.", "CDROM Error", TaskDialogType.ERROR);
                }
            }
        }

        private void Md5GenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Md5GenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Checksum md5Form = new Checksum(ChecksumType.MD5SUM, false);
            md5Form.Owner = this;
            md5Form.ShowDialog();
        }

        private void Md5VerifyCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Md5VerifyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            VerifyChecksum md5verify = new VerifyChecksum(ChecksumType.MD5SUM, false);
            md5verify.Owner = this;
            md5verify.ShowDialog();
        }

        private void SfvGenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SfvGenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Checksum sfvForm = new Checksum(ChecksumType.SFV, false);
            sfvForm.Owner = this;
            sfvForm.ShowDialog();
        }

        private void SfvVerifyCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SfvVerifyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            VerifyChecksum md5verify = new VerifyChecksum(ChecksumType.SFV, false);
            md5verify.Owner = this;
            md5verify.ShowDialog();
        }

        private void HelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                string currentPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                System.Windows.Forms.Help.ShowHelp(null, currentPath + @"\help\Iside.chm");
            }
            catch (IOException) { }
        }

        private void ChekForUpdatesCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChekForUpdatesExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CheckForUpdatesWPF.Updates.Check(this, IsideLogic.Config.AppInfo);
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

        private void OrderingInfoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                Process.Start(IsideLogic.Config.APPBUY);
            }
            catch (System.ComponentModel.Win32Exception) { }
        }

        private void OrderingInfoCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void BuyNowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void BuyNowCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ActivateExecuted(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void ActivateCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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


        #endregion

        #region Private Methods
        private void New()
        {
            this.fileProperties[0] = new FileProperties();
            this.fileProperties[1] = new FileProperties();
            this.Reset();
        }

        private void Reset()
        {
            System.Diagnostics.Debug.WriteLine("Iside Window Reset");

            SolidColorBrush background = new SolidColorBrush(Colors.White);

            this.txtHash1Left.Text = String.Empty;
            this.txtHash1Left.Background = background;

            this.txtHash1Right.Text = String.Empty;
            this.txtHash1Right.Background = background;

            this.txtHash2Left.Text = String.Empty;
            this.txtHash2Left.Background = background;

            this.txtHash2Right.Text = String.Empty;
            this.txtHash2Right.Background = background;

            this.txtCreationDate.Text = String.Empty;
            this.txtCreationDate.Background = background;

            this.txtCreationDateVS.Text = String.Empty;
            this.txtCreationDateVS.Background = background;

            this.txtFileName.Text = String.Empty;
            this.txtFileName.Background = background;

            this.txtFileNameVS.Text = String.Empty;
            this.txtFileNameVS.Background = background;

            this.txtFileSize.Text = String.Empty;
            this.txtFileSize.Background = background;

            this.txtFileSizeVS.Text = String.Empty;
            this.txtFileSizeVS.Background = background;

            this.txtLastAccess.Text = String.Empty;
            this.txtLastAccess.Background = background;

            this.txtLastAccessVS.Text = String.Empty;
            this.txtLastAccessVS.Background = background;

            this.txtLastModified.Text = String.Empty;
            this.txtLastModified.Background = background;

            this.txtLastModifiedVS.Text = String.Empty;
            this.txtLastModifiedVS.Background = background;

            this.statusBar.Content = "Ready";

            this.prHash = null;
            this.altHash = null;

            this.Init();
        }

        private void HashNow()
        {
            if (!String.IsNullOrEmpty(this.txtFileName.Text))
            {
                this.StartHashProcess(Quadrant.UPPER_LEFT, this.txtFileName.Text);
            }

            if (!String.IsNullOrEmpty(this.txtFileNameVS.Text))
            {
                this.StartHashProcess(Quadrant.UPPER_RIGHT, this.txtFileNameVS.Text);
            }
        }

        private void StartHashProcess(Quadrant q, string fileName)
        {
            if (!File.Exists(fileName))
            {
                System.Windows.Forms.DialogResult dr = MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME, "File does not exist. \nDo you want to compute the hash of the inserted string?", "File Save Error", TaskDialogType.QUESTION);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    this.logic.TextHashCompute(fileName, q);
                }
            }
            else
            {
                GUI.SetCursorWait(true);
                this.logic.ManuallySetFile(q, fileName);
                GUI.SetCursorWait(false);
            }
        }

        private void Save()
        {
            if ((this.txtHash1Left.Text.Length > 0) || (this.txtHash1Right.Text.Length > 0))
            {
                this.saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == true) 
                {
                    if (this.logic.SaveHashes(saveFileDialog.FileName))
                    {
                        MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME, "Hash codes saved to " + saveFileDialog.FileName, "File saved", TaskDialogType.INFO);
                    }
                }
            }
        }

        private void Options()
        {
            Options opt = new Options();
            opt.Owner = this;

            if (opt.ShowDialog() == true)
            {
                // Save Options.
                this.statusBar.Content = "Options saved.";
                this.prHash = null;
                this.altHash = null;
                this.skipHashComputing[0] = false;
                this.skipHashComputing[1] = false;
                this.SetLayout();
                this.logic.PropertiesRefresh();
            }
            else
            {
                this.skipHashComputing[0] = true;
                this.skipHashComputing[1] = true;
            }
        }

        private void RefreshHash(Quadrant quadran)
        {

            try
            {
                ComboBox thisHashCombo = null;
                ComboBox twinHashCombo = null;
                TabControl currentTab = null;
                string fileItem = null;
                string selected;

                switch (quadran)
                {
                    case Quadrant.UPPER_LEFT:
                        thisHashCombo = this.cboSelHashLeft1;
                        twinHashCombo = this.cboSelHashRight1;
                        currentTab = this.tabControl1;
                        fileItem = this.txtFileName.Text;
                        this.skipHashComputing[0] = false;
                        selected = ((ComboBoxItem)thisHashCombo.SelectedItem).Content as string;
                        this.prHash = SupportedHashAlgoFactory.FromName(selected);
                        HashLogic.SyncHashCombo(this.prHash.Id, twinHashCombo);
                        break;

                    case Quadrant.UPPER_RIGHT:
                        thisHashCombo = this.cboSelHashRight1;
                        twinHashCombo = this.cboSelHashLeft1;
                        currentTab = this.tabControl2;
                        fileItem = this.txtFileNameVS.Text;
                        this.skipHashComputing[1] = false;
                        selected = ((ComboBoxItem)thisHashCombo.SelectedItem).Content as string;
                        this.prHash = SupportedHashAlgoFactory.FromName(selected);
                        HashLogic.SyncHashCombo(this.prHash.Id, thisHashCombo);
                        break;

                    case Quadrant.LOWER_LEFT:
                        thisHashCombo = this.cboSelHashLeft2;
                        twinHashCombo = this.cboSelHashRight2;
                        currentTab = this.tabControl1;
                        fileItem = this.txtFileName.Text;
                        this.skipHashComputing[0] = false;
                        selected = ((ComboBoxItem)thisHashCombo.SelectedItem).Content as string;
                        this.altHash = SupportedHashAlgoFactory.FromName(selected);
                        HashLogic.SyncHashCombo(this.altHash.Id, twinHashCombo);
                        break;

                    case Quadrant.LOWER_RIGHT:
                        thisHashCombo = this.cboSelHashRight2;
                        twinHashCombo = this.cboSelHashLeft2;
                        currentTab = this.tabControl2;
                        fileItem = this.txtFileNameVS.Text;
                        this.skipHashComputing[1] = false;
                        selected = ((ComboBoxItem)thisHashCombo.SelectedItem).Content as string;
                        this.altHash = SupportedHashAlgoFactory.FromName(selected);
                        HashLogic.SyncHashCombo(this.altHash.Id, thisHashCombo);
                        break;
                }

                if (fileItem.Length > 0)
                {
                    if (File.Exists(fileItem))
                    {
                        this.logic.RefreshSingleHash(quadran);
                    }
                    else
                    {
                        this.logic.TextHashCompute(fileItem, quadran);
                    }
                    currentTab.Focus();
                }
            }
            catch (IsideException isex)
            {
                MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME, isex.Message, "Iside Exception", TaskDialogType.WARNING);
            }

        }

        private void SetLayout()
        {
            FontInfo hashFont = null;
            string fontFile = Path.Combine(Core.AppDataPath, IsideLogic.Config.FONTFILE);

            try
            {

                if (File.Exists(fontFile))
                {
                    hashFont = FontInfo.Deserialize(fontFile);
                }

                if (hashFont != null)
                {
                    FontInfo.ApplyFont(this.txtHash1Left, hashFont);
                    FontInfo.ApplyFont(this.txtHash2Left, hashFont);
                    FontInfo.ApplyFont(this.txtHash1Right, hashFont);
                    FontInfo.ApplyFont(this.txtHash2Right, hashFont);
                }
            }
            catch (IOException ioex)
            {
                System.Diagnostics.Debug.WriteLine("Error in deserializing font: " + ioex.Message);
            }
        }

        private void ShowRegisteredControls(bool registered)
        {

            System.Windows.Visibility visible = System.Windows.Visibility.Visible;
            if (registered)
            {
                visible = System.Windows.Visibility.Collapsed;
            }

            this.sepOrders.Visibility = visible;
        }

        private void RefreshUI(AvailableHash hash1, AvailableHash hash2)
        {
            // Init hash algorithms
            this.prHash = SupportedHashAlgoFactory.GetAlgo(hash1);
            this.altHash = SupportedHashAlgoFactory.GetAlgo(hash2);

            // Init hash combos
            string[] hashes = SupportedHashAlgorithms.GetHashAlgorithms();
            string[] hashesWithNone = SupportedHashAlgorithms.GetHashAlgorithmsWithNone();

            if (this.cboSelHashLeft1.Items.Count == 0)
            {
                GUI.ComboBoxItemsAdd(this.cboSelHashLeft1, hashes);
            }

            if (this.cboSelHashLeft2.Items.Count == 0)
            {
                GUI.ComboBoxItemsAdd(this.cboSelHashLeft2, hashesWithNone);
            }

            if (this.cboSelHashRight1.Items.Count == 0)
            {
                GUI.ComboBoxItemsAdd(this.cboSelHashRight1, hashes);
            }

            if (this.cboSelHashRight2.Items.Count == 0)
            {
                GUI.ComboBoxItemsAdd(this.cboSelHashRight2, hashesWithNone);
            }

            HashLogic.SyncHashCombo(hash1, this.cboSelHashLeft1);
            HashLogic.SyncHashCombo(hash2, this.cboSelHashLeft2);
            HashLogic.SyncHashCombo(hash1, this.cboSelHashRight1);
            HashLogic.SyncHashCombo(hash2, this.cboSelHashRight2);

            // Compute hashes (if any...)
            this.logic.PropertiesRefresh();
        }

        private void Init(bool isRegistered)
        {
            try
            {
                InitializeComponent();
                this.Init();
                this.ShowRegisteredControls(isRegistered);
            }
            catch (InvalidOperationException ioe)
            {
                System.Diagnostics.Debug.WriteLine(ioe.Message);
            }
        }

        private void Init()
        {
            this.logic = new HashLogic(this);
            this.reset = new AutoResetEvent(false);
            this.canCompareNow = false;
            AvailableHash hash1 = Settings.Default.PrimaryHash;
            AvailableHash hash2 = Settings.Default.AlternativeHash;
            this.RefreshUI(hash1, hash2);
        }

        private void ResetStatusBar()
        {
            this.statusBar.Content = "Ready";
        }

        private bool IsThereSomethingToHash
        {
            get
            {
                if (this.txtFileName == null)
                    return false;

                bool isThereAFileToHash = false;

                if (!String.IsNullOrEmpty(this.txtFileName.Text))
                {
                    isThereAFileToHash = true;
                }

                return isThereAFileToHash;
            }
        }

        private bool IsThereSomethingToCompare
        {
            get
            {

                if (this.txtFileName == null)
                    return false;

                bool isCompare = false;

                if (this.IsThereSomethingToHash)
                {
                    if (!String.IsNullOrEmpty(this.txtFileNameVS.Text))
                    {
                        if (this.txtHash1Right.Text.Length > 0)
                        {
                            isCompare = true;
                        }
                    }
                }

                return isCompare;
               
            }
        }

        private void PreviewPrint()
        {
            string _previewWindowXaml =
                        @"<Window
                            xmlns                 ='http://schemas.microsoft.com/netfx/2007/xaml/presentation'
                            xmlns:x               ='http://schemas.microsoft.com/winfx/2006/xaml'
                            Title                 ='Iside Hash Print Preview'
                            Height                ='600'
                            Width                 ='600'
                            WindowStartupLocation ='CenterOwner'>
                            <DocumentViewer Name='dv1'/>
                         </Window>";

            string previewFilePath = Path.Combine(Core.AppDataPath, System.IO.Path.GetRandomFileName());
            //FlowDocumentScrollViewer visual = (FlowDocumentScrollViewer)(_parent.FindName("fdsv1"));

            this.PrepareReport();
            FlowDocument documentToPrint = this.ComposeReportToPrint();
            documentToPrint.ColumnWidth = double.PositiveInfinity;

            try
            {
                // write the XPS document
                using (XpsDocument doc = new XpsDocument(previewFilePath, FileAccess.ReadWrite))
                {
                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    DocumentPaginator dp = ((IDocumentPaginatorSource)documentToPrint).DocumentPaginator;
                    writer.Write(dp);
                }

                // Read the XPS document into a dynamically generated
                // preview Window 
                using (XpsDocument doc = new XpsDocument(previewFilePath, FileAccess.Read))
                {
                    FixedDocumentSequence fds = doc.GetFixedDocumentSequence();

                    string s = _previewWindowXaml;
                    using (var reader = new System.Xml.XmlTextReader(new StringReader(s)))
                    {
                        Window preview = System.Windows.Markup.XamlReader.Load(reader) as Window;

                        DocumentViewer dv1 = LogicalTreeHelper.FindLogicalNode(preview, "dv1") as DocumentViewer;
                        dv1.ApplyTemplate();
                        ContentControl cc = dv1.Template.FindName("PART_FindToolBarHost", dv1) as ContentControl;
                        cc.Visibility = Visibility.Collapsed;
                        dv1.Document = fds as IDocumentPaginatorSource;

                        preview.ShowDialog();
                    }
                }
            }
            finally
            {
                if (File.Exists(previewFilePath))
                {
                    try
                    {
                        File.Delete(previewFilePath);
                    }
                    catch
                    {
                    }
                }
            }

        }

        private void PrepareReport()
        {

            string[] filenames = new string[2];
            SupportedHashAlgo[] algosLeft = new SupportedHashAlgo[2];
            string[] hashesLeft = new string[2];
            SupportedHashAlgo[] algosRight = new SupportedHashAlgo[2];
            string[] hashesRight = new string[2];

            filenames[0] = this.txtFileName.Text;
            if (String.IsNullOrEmpty(this.txtFileNameVS.Text))
            {
                filenames[1] = null;
            }
            else
            {
                filenames[1] = this.txtFileNameVS.Text;
            }

            algosLeft[0] = this.prHash;
            algosLeft[1] = this.altHash;
            hashesLeft[0] = this.txtHash1Left.Text;
            hashesLeft[1] = this.txtHash2Left.Text;
            algosRight[0] = this.prHash;
            algosRight[1] = this.altHash;
            hashesRight[0] = this.txtHash1Right.Text;
            hashesRight[1] = this.txtHash2Right.Text;

            HashReport[] reports = { new HashReport(filenames[0], algosLeft, hashesLeft), new HashReport(filenames[1], algosRight, hashesRight) };
            TextReport tr = new TextReport(reports);
            this.reportToPrint = tr.GetReport();
        }

        private FlowDocument ComposeReportToPrint()
        {
            FlowDocument document = null;

            if (this.reportToPrint != null)
            {
                document = new FlowDocument();
                try
                {
                    document.FontFamily = new System.Windows.Media.FontFamily("Courier New");
                    document.FontSize = 11.0;
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.Message);
                }

                foreach (string s in this.reportToPrint)
                {
                    Paragraph para = new Paragraph();
                    para.Inlines.Add(s);
                    document.Blocks.Add(para);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(">>> Error: reportToPrint is NULL");
            }

            return document;
        }

        private void PrintReport()
        {
            this.PrepareReport();
            if (this.PrintReport(this.ComposeReportToPrint()))
            {
                MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME, "Report sent to printer", "Report Printed", TaskDialogType.INFO);
            }
        }

        private bool PrintReport(System.Windows.Documents.FlowDocument document)
        {

            bool printedOk = true;

            // Clone the source document's content into a new FlowDocument.
            // This is because the pagination for the printer needs to be
            // done differently than the pagination for the displayed page.
            // We print the copy, rather that the original FlowDocument.
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            TextRange source = new TextRange(document.ContentStart, document.ContentEnd);
            source.Save(s, DataFormats.Xaml);
            FlowDocument copy = new FlowDocument();
            TextRange dest = new TextRange(copy.ContentStart, copy.ContentEnd);
            dest.Load(s, DataFormats.Xaml);

            // Create a XpsDocumentWriter object, implicitly opening a Windows common print dialog,
            // and allowing the user to select a printer.

            // get information about the dimensions of the seleted printer+media.
            System.Printing.PrintDocumentImageableArea ia = null;
            System.Windows.Xps.XpsDocumentWriter docWriter = 
                System.Printing.PrintQueue.CreateXpsDocumentWriter(ref ia);

            if (docWriter != null && ia != null)
            {
                DocumentPaginator paginator = ((IDocumentPaginatorSource)copy).DocumentPaginator;

                // Change the PageSize and PagePadding for the document to match the CanvasSize for the printer device.
                paginator.PageSize = new System.Windows.Size(ia.MediaSizeWidth, ia.MediaSizeHeight);
                Thickness t = new Thickness(72);  // copy.PagePadding;
                copy.PagePadding = new Thickness(
                                 Math.Max(ia.OriginWidth, t.Left),
                                   Math.Max(ia.OriginHeight, t.Top),
                                   Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), t.Right),
                                   Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), t.Bottom));

                copy.ColumnWidth = double.PositiveInfinity;
                //copy.PageWidth = 528; // allow the page to be the natural with of the output device

                // Send content to the printer.
                docWriter.Write(paginator);
            }
            else
            {
                printedOk = false;
            }

            return printedOk;

        }

        private void InitializeUIFirstTime()
        {
            if (Settings.Default.IsideWinLeft > 0)
            {
                this.Left = Settings.Default.IsideWinLeft;
                this.Top = Settings.Default.IsideWinTop;
            }
            else
            {
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            }

            this.SetLayout();
            this.guiWidth = Settings.Default.SizeForm;
            this.logic.SetSize(this.guiWidth);
            if (this.guiWidth == FormWidth.SINGLE)
            {
                this.expSize.IsExpanded = false;
            }
            this.hexRegistry = Settings.Default.HashStyle;

        }

        private void ExportReport()
        {
            this.saveFileDialog = new SaveFileDialog();
            this.saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            this.saveFileDialog.FilterIndex = 1;
            this.saveFileDialog.RestoreDirectory = true;
            if (this.saveFileDialog.ShowDialog(this) == true)
            {
                this.PrepareReport();
                StringBuilder sb = new StringBuilder();
                foreach (string line in this.reportToPrint)
                {
                    sb.AppendLine(line);
                }
                FileManager fm = FileManager.Reference;
                string path = this.saveFileDialog.FileName;
                if (fm.SaveFile(path, sb.ToString()))
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME, "Report saved to " + path, "File Saved", TaskDialogType.INFO);
                }
                else
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPNAME, fm.ErrorMessage, "File error", TaskDialogType.ERROR);
                }
            }
        }
        #endregion

        #region Window Event Handlers

        private void Iside_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Iside Windows Loaded");
            this.InitializeUIFirstTime();
        }

        private void Iside_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Iside_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.IsideWinLeft = this.Left;
            Settings.Default.IsideWinTop = this.Top;
            Settings.Default.SizeForm = this.guiWidth;
            Settings.Default.Save();
        }

        private void Iside_Activated(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Iside Window activated");
            this.Activated -= new EventHandler(this.Iside_Activated);
            this.logic.PropertiesRefresh();
        }

        #endregion

        #region Combo, Buttons And Other Events

        private void cboSelHashLeft1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Refreshing UPPER-LEFT");
            this.RefreshHash(Quadrant.UPPER_LEFT);
        }

        private void cboSelHashLeft2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Refreshing LOWER-LEFT");
            this.RefreshHash(Quadrant.LOWER_LEFT);
        }

        private void cboSelHashRight1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Refreshing UPPER_RIGHT");
            this.RefreshHash(Quadrant.UPPER_RIGHT);
        }

        private void cboSelHashRight2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Refreshing LOWER_RIGHT");
            this.RefreshHash(Quadrant.LOWER_RIGHT);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.reset.Set();
        }

        private void txtFileName_LostFocus(object sender, RoutedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private void txtFileNameVS_LostFocus(object sender, RoutedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion

        #region Drag&Drop Handling & Events
        private void DoLeftDragAndDrop(object sender, DragEventArgs e)
        {
            try
            {
                // Data dropped is a file?
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    this.skipHashComputing[0] = false;
                    this.skipHashComputing[1] = true;
                    if (a.Length == 1)
                    {
                        string filename = a.GetValue(0).ToString();
#if DEBUG
                        Console.WriteLine("I'm doing drag/drop of {0}", filename);
#endif
                        this.fileProperties[0].FileName = filename;
                        this.logic.PropertiesRefresh();
                    }
                    else if (a.Length > 1)
                    {
                        this.skipHashComputing[1] = false;
                        if (this.guiWidth == FormWidth.SINGLE)
                        {
                            this.guiWidth = FormWidth.DOUBLE;
                        }
                        string leftFile = a.GetValue(0).ToString();
                        string rightFile = a.GetValue(1).ToString();
                        this.fileProperties[0].FileName = leftFile;
                        this.fileProperties[1].FileName = rightFile;
                        this.logic.PropertiesRefresh();
                    }
                }
            }
            catch (Exception) { }
        }

        private void DoRightDragAndDrop(object sender, DragEventArgs e)
        {
            try
            {
                // Data dropped is a file?
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    this.skipHashComputing[0] = true;
                    this.skipHashComputing[1] = false;
                    if (a.Length == 1)
                    {
                        string filename = a.GetValue(0).ToString();
                        this.fileProperties[1].FileName = filename;
                        this.logic.PropertiesRefresh();
                    }
                    else if (a.Length > 1)
                    {
                        this.skipHashComputing[0] = false;
                        if (this.guiWidth == FormWidth.SINGLE)
                        {
                            this.guiWidth = FormWidth.DOUBLE;
                        }
                        string leftFile = a.GetValue(0).ToString();
                        string rightFile = a.GetValue(1).ToString();
                        this.fileProperties[0].FileName = leftFile;
                        this.fileProperties[1].FileName = rightFile;
                        this.logic.PropertiesRefresh();
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
            // Accept files only
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

        private void txtFileName_Drop(object sender, DragEventArgs e)
        {
            this.DoLeftDragAndDrop(sender, e);
        }

        private void txtFileName_DragEnter(object sender, DragEventArgs e)
        {
            this.DoDragEnter(sender, e);
        }

        private void txtFileNameVS_Drop(object sender, DragEventArgs e)
        {
            this.DoRightDragAndDrop(sender, e);
        }

        private void txtFileNameVS_DragEnter(object sender, DragEventArgs e)
        {
            this.DoDragEnter(sender, e);
        }

        #endregion



    }
}

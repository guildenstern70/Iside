/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/


using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using AxsUtils;
using AxsUtils.Win32;
using WPFUtils;

namespace IsideFolder
{
    /// <summary>
    /// FileSummary.xaml
    /// </summary>
    public partial class FileSummary : Window
    {
        private DetailedSummary handleLog;
        private string rightPath;
        private string leftPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSummary"/> class.
        /// </summary>
        /// <param name="title">The title of this window</param>
        /// <param name="rPath">The relative path of the comparison, taken from the left comparison folder</param>
        public FileSummary(string title, string rPath, string lPath)
        {
            if (title == null)
            {
                title = "Results";
            }

            if (rPath == null)
            {
                throw new ArgumentNullException("rPath");
            }

            if (lPath == null)
            {
                throw new ArgumentNullException("lPath");
            }

            this.rightPath = rPath;
            this.leftPath = lPath;

            InitializeComponent();

            this.Title = title;
            if (title.Equals("Results"))
            {
                this.btnHTML.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        internal void SummaryDetails(DetailedSummary log)
        {
            this.handleLog = log;
        }

        public string Summary
        {
            get
            {
                return this.textBox1.Text;
            }

            set
            {
                this.textBox1.Text = value;
            }
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            ClipboardUtils.Copy(this.textBox1.Text);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            FileManager fm = FileManager.Reference;
            string contents = "";

            if (this.handleLog == null) // Results list
            {
                saveFileDialog.Filter = "Text files|*.txt|All files|*.*";
            }
            else
            {
                saveFileDialog.Filter = "CSV Files|*.csv|Text files|*.txt|HTML File|*.html";
            }

            if (saveFileDialog.ShowDialog() == true)
            {
                string saveFileName = saveFileDialog.FileName;
                if (this.handleLog != null) // Detailed log
                {
                    if (saveFileDialog.FilterIndex == 1) // CSV File
                    {
                        contents = this.handleLog.SummaryAsCSV(this.rightPath, this.leftPath);
                    }
                    else if (saveFileDialog.FilterIndex == 2) // TXT File
                    {
                        contents = this.textBox1.Text;
                    }
                    else if (saveFileDialog.FilterIndex == 3) // HTML file
                    {
                        contents = this.handleLog.SummaryAsHTML(this.rightPath, this.leftPath);
                    }
                }
                else
                {
                    contents = this.textBox1.Text;
                }

                if (!fm.SaveFile(saveFileName, contents))
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, "Cannot save " + saveFileName + "\r\nIs it opened by another application?", "Save error", TaskDialogType.ERROR);
                }
                else
                {
                    MessageTaskDialog.Show(this, IsideLogic.Config.APPFOLDER, saveFileName + " saved.", "File saved", TaskDialogType.INFO);
                }
            }
        }

        private void btnHTML_Click(object sender, RoutedEventArgs e)
        {
            FileManager fm = FileManager.Reference;
            string saveFileName = Path.Combine(Path.GetTempPath(), "report.html");
            string contents = this.handleLog.SummaryAsHTML(this.rightPath, this.leftPath);
            fm.SaveFile(saveFileName, contents);
            try
            {
                System.Diagnostics.Process.Start(saveFileName);
            }
            catch (Win32Exception) { }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

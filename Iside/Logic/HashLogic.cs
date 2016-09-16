/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Iside.Properties;
using IsideLogic;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;
using WPFUtils;

namespace Iside.Logic
{
    public class HashLogic
    {
        private IsideWindow parent;
        private HashWorks hashOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:IsideShowLogic"/> class.
        /// </summary>
        /// <param name="main">The main form.</param>
        public HashLogic(IsideWindow main)
        {
            this.parent = main;
            this.hashOperations = new HashWorks(main);
        }

        /// <summary>
        /// Propertieses the refresh.
        /// </summary>
        internal void PropertiesRefresh()
        {
            this.SetSize(this.parent.guiWidth);

            if (!String.IsNullOrEmpty(parent.fileProperties[0].FileName))
            {
                this.hashOperations.HashKickOff();
            }
        }

        /// <summary>
        /// Refreshes a single hash.
        /// </summary>
        /// <param name="upper">if set to <c>true</c> will refresh the UPPER hash, else the BOTTOM</param>
        internal void RefreshSingleHash(Quadrant q)
        {
            TextBox toRefreshUpperLeft;
            TextBox toRefreshUpperRight;
            TextBox toRefreshLowerLeft;
            TextBox toRefreshLowerRight;

            HashQuadrant hq = new HashQuadrant(q);

            if (hq.IsUpper)
            {
                toRefreshUpperLeft = parent.txtHash1Left;
                toRefreshUpperRight = parent.txtHash1Right;
                toRefreshLowerLeft = null;
                toRefreshLowerRight = null;
            }
            else
            {
                toRefreshUpperLeft = null;
                toRefreshUpperRight = null;
                toRefreshLowerLeft = parent.txtHash2Left;
                toRefreshLowerRight = parent.txtHash2Right;
            }

            this.hashOperations.HashKickOff(toRefreshUpperLeft, toRefreshUpperRight, 
                                            toRefreshLowerLeft, toRefreshLowerRight);

        }

        internal void SetSize(FormWidth width)
        {
            if (width == FormWidth.SINGLE)
            {
                this.parent.Width = Constants.HALF_WIDTH;
            }
            else
            {
                this.parent.Width = Constants.FULL_WIDTH;
            }

            if (this.parent.guiWidth != width)
            {
                System.Diagnostics.Debug.WriteLine("Setting size to " + width.ToString());
                this.parent.guiWidth = width;
            }
        }

        /// <summary>
        /// Compares the now.
        /// </summary>
        internal void CompareNow()
        {
            bool iside = false;
            string message;
            TaskDialogType dialogType = TaskDialogType.INFO;

            string hash1a = parent.txtHash1Left.Text;
            string hash1b = parent.txtHash2Left.Text;
            string hash2a = parent.txtHash1Right.Text;
            string hash2b = parent.txtHash2Right.Text;

            if (hash1a.Equals(hash2a))
            {
                if (hash1b.Equals(hash2b))
                {
                    iside = true;
                }
            }

            if (iside)
            {
                message = "File contents are equal.";
            }
            else
            {
                message = "File contents are different.";
                dialogType = TaskDialogType.WARNING;
            }

            MessageTaskDialog.Show(parent, IsideLogic.Config.APPNAME, message, "Iside File Comparison", dialogType);

        }

        /// <summary>
        /// Restores the textboxes background colors.
        /// </summary>
        internal void RestoreColors()
        {
            SolidColorBrush highlight = new SolidColorBrush(Colors.White);
            parent.txtFileSize.Background = highlight;
            parent.txtFileSizeVS.Background = highlight;
            parent.txtCreationDate.Background = highlight;
            parent.txtCreationDateVS.Background = highlight;
            parent.txtLastAccess.Background = highlight;
            parent.txtLastAccessVS.Background = highlight;
            parent.txtLastModified.Background = highlight;
            parent.txtLastModifiedVS.Background = highlight;
            parent.txtHash1Left.Background = highlight;
            parent.txtHash1Right.Background = highlight;
            parent.txtHash2Left.Background = highlight;
            parent.txtHash2Right.Background = highlight;
        }

        /// <summary>
        /// Opens the left file for hashing.
        /// </summary>
        internal void OpenLeftFile()
        {
            parent.statusBar.Content = "Opening File";
            parent.skipHashComputing[0] = false;

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "All documents (*.*)|*.*"; // Filter files by extension 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                this.RestoreColors();
                parent.fileProperties[0].FileName = dlg.FileName;
                this.PropertiesRefresh();
            }

            parent.statusBar.Content = "Ready";
        }

        /// <summary>
        /// Opens the right file for hasing.
        /// </summary>
        internal void OpenRightFile()
        {
            parent.statusBar.Content = "Opening File";
            parent.skipHashComputing[1] = false;

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "All documents (.*)|*.*"; // Filter files by extension 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                this.RestoreColors();
                if (this.parent.guiWidth == FormWidth.SINGLE)
                {
                    this.SetSize(FormWidth.DOUBLE);
                }
                parent.fileProperties[1].FileName = dlg.FileName;
                this.PropertiesRefresh();
            }

            parent.statusBar.Content = "Ready";
        }

        /// <summary>
        /// Computes hash for a text string
        /// </summary>
        /// <param name="textToHash">The text to hash.</param>
        internal void TextHashCompute(string textToHash, Quadrant q)
        {
            TextBox t1;
            TextBox t2;

            HashQuadrant hq = new HashQuadrant(q);

            if (hq.IsLeft)
            {
                t1 = this.parent.txtHash1Left;
                t2 = this.parent.txtHash2Left;
            }
            else
            {
                t1 = this.parent.txtHash1Right;
                t2 = this.parent.txtHash2Right;
            }

            Hash h = new Hash();
            h.SetAlgorithmAlgo(parent.PrimaryHash);
            HexEnum style = Settings.Default.HashStyle;
            t1.Text = h.ComputeHashStyle(textToHash, style, Encoding.ASCII);
            h.SetAlgorithmAlgo(parent.AlternativeHash);
            t2.Text = h.ComputeHashStyle(textToHash, style, Encoding.ASCII);
        }

        internal void ManuallySetFile(Quadrant q, string path)
        {

            HashQuadrant hq = new HashQuadrant(q);

            if (hq.IsLeft)
            {
                parent.skipHashComputing[0] = false;
                parent.LeftFile = path;
            }
            else
            {
                parent.skipHashComputing[1] = false;
                parent.RightFile = path;
            }

            this.PropertiesRefresh();
        }

        [Obsolete("TO-DO Method")]
        internal void HashCDROM()
        {
        }

        
        /// <summary>
        /// Saves the hashes.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        internal bool SaveHashes(string fileName)
        {
            bool okSaved = true;
            AxsUtils.FileManager fm = AxsUtils.FileManager.Reference;
            StringBuilder sb = new StringBuilder();

            // Header
            sb.Append("# Hash codes generated with ");
            sb.Append(IsideLogic.Config.AppNameAndVersion);
            sb.Append(" (");
            sb.Append(IsideLogic.Config.APPURL);
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("# Operating System: " + AxsUtils.Win32.OS.OperatingSystem);
            sb.Append(Environment.NewLine);
            sb.Append("# Hash codes taken " + DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString());
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            if (this.parent.txtFileName.Text.Length > 0)
            {
                sb.Append("File: " + this.parent.txtFileName.Text);
                sb.Append(Environment.NewLine);
                sb.Append("Size: " + this.parent.txtFileSize.Text);
                sb.Append(Environment.NewLine);
                sb.Append(this.parent.cboSelHashLeft1.Text + ": " + this.parent.txtHash1Left.Text);
                sb.Append(Environment.NewLine);
                sb.Append(this.parent.cboSelHashLeft2.Text + ": " + this.parent.txtHash2Left.Text);
                sb.Append(Environment.NewLine);
            }

            if (this.parent.txtFileNameVS.Text.Length > 0)
            {
                sb.Append(Environment.NewLine);
                sb.Append("File: " + this.parent.txtFileNameVS.Text);
                sb.Append(Environment.NewLine);
                sb.Append("Size: " + this.parent.txtFileSizeVS.Text);
                sb.Append(Environment.NewLine);
                sb.Append(this.parent.cboSelHashRight1.Text + ": " + this.parent.txtHash1Right.Text);
                sb.Append(Environment.NewLine);
                sb.Append(this.parent.cboSelHashRight2.Text + ": " + this.parent.txtHash2Right.Text);
            }

            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            if (!fm.SaveFile(fileName, sb.ToString(), false))
            {
                MessageTaskDialog.Show(parent, IsideLogic.Config.APPNAME, "Cannot save " + fileName + ": " + fm.ErrorMessage, "File Save Error", TaskDialogType.ERROR);
                okSaved = false;
            }

            return okSaved;
        }

        #region STATIC MEMBERS

        /// <summary>
        /// Set selected index for an hash combo box as the given hash
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="cbo"></param>
        internal static void SyncHashCombo(AvailableHash hashAlgo, ComboBox cbo)
        {

            // Do not process if combo has not been initialized
            if (cbo.Items.Count == 0)
            {
                return;
            }

            SupportedHashAlgo hash = SupportedHashAlgoFactory.GetAlgo(hashAlgo);
            string hashName = hash.Name;

            ComboBoxItem cboItem = null;
            for (int j = 0; j < cbo.Items.Count; j++)
            {
                cboItem = cbo.Items[j] as ComboBoxItem;
                string cboItemString = cboItem.Content as string;
                if (hashName == cboItemString) 
                {
                    cbo.SelectedIndex = j;
                    break;
                }
            }

            if (cbo.SelectedIndex == -1)
            {
                cbo.SelectedIndex = 0;
            }

        }


        #endregion

    }
}

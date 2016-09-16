/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using IsideLogic;
using LLCryptoLib;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;
using Microsoft.WindowsAPICodePack.Taskbar;
using MS.WindowsAPICodePack.Internal;
using WPFUtils;

namespace Iside.Logic
{
    public class HashWorks
    {
        private IsideWindow parent;
        private Object lockCritical;
        private bool isWin7;
        private BackgroundWorker backWorkerLeft;
        private BackgroundWorker backWorkerRight;

        public HashWorks(IsideWindow main)
        {
            this.parent = main;
            this.lockCritical = new Object();
            this.backWorkerLeft = this.CreateBackgroundWorker();
            this.backWorkerRight = this.CreateBackgroundWorker();
            this.isWin7 = CoreHelpers.RunningOnWin7;
        }

        public void HashKickOff()
        {
            this.HashKickOff(parent.txtHash1Left, parent.txtHash1Right, parent.txtHash2Left, parent.txtHash2Right);
        }

        public void HashKickOff(TextBox toRefreshUpperLeft, TextBox toRefreshUpperRight,
                                TextBox toRefreshLowerLeft, TextBox toRefreshLowerRight)
        {
            // Left file hash
            if ((parent.fileProperties[0].FileName.Length > 0) && (!parent.skipHashComputing[0]))
            {
                this.RefreshFileProperties(true);
                parent.skipHashComputing[0] = true;
                Quadrant[] qs = { Quadrant.UPPER_LEFT, Quadrant.LOWER_LEFT };
                this.StartHashProcess(parent.reset, parent.fileProperties[0].FullPath, qs);
            }

            // Right file hash
            if ((parent.fileProperties[1].FileName.Length > 0) && (!parent.skipHashComputing[1]))
            {
                this.RefreshFileProperties(false);
                parent.skipHashComputing[1] = true;
                Quadrant[] qs = { Quadrant.UPPER_RIGHT, Quadrant.LOWER_RIGHT };
                this.StartHashProcess(parent.reset, parent.fileProperties[1].FullPath, qs);
            }
        }

        internal void InitProgressBar(long filesize)
        {
            // Init progress bar
            if (filesize > 2000000)
            {
                ProgressBar pb = this.parent.progressBar;
                pb.Visibility = System.Windows.Visibility.Visible;
                pb.Minimum = 0;
                pb.Maximum = (int)(filesize / Hash.ChunkSize) + 1;
                pb.Value = 0;
                this.SetWin7TaskbarValue(0, Convert.ToInt32(pb.Maximum));
            }
        }

        internal void RefreshFileProperties(bool isLeft)
        {
            FileProperties fp = parent.fileProperties[0];

            if (!isLeft)
            {
                fp = parent.fileProperties[1];
            }

            if (fp != null)
            {
                if (isLeft)
                {
                    parent.txtFileName.Text = fp.FullPath;
                    parent.txtFileSize.Text = fp.Size;
                    parent.txtLastAccess.Text = fp.LastAccessDate;
                    parent.txtLastModified.Text = fp.LastModifiedDate;
                    parent.txtCreationDate.Text = fp.CreationDate;
                    parent.txtFileName.ToolTip = fp.FullPath;
                    parent.chkArchive.IsChecked = fp.IsArchive;
                    parent.chkHidden.IsChecked = fp.IsHidden;
                    parent.chkReadonly.IsChecked = fp.IsReadOnly;
                }
                else
                {
                    parent.txtFileNameVS.Text = fp.FullPath;
                    parent.txtFileSizeVS.Text = fp.Size;
                    parent.txtLastAccessVS.Text = fp.LastAccessDate;
                    parent.txtLastModifiedVS.Text = fp.LastModifiedDate;
                    parent.txtCreationDateVS.Text = fp.CreationDate;
                    parent.txtFileNameVS.ToolTip = fp.FullPath;
                    parent.chkArchiveVS.IsChecked = fp.IsArchive;
                    parent.chkHiddenVS.IsChecked = fp.IsHidden;
                    parent.chkReadonlyVS.IsChecked = fp.IsReadOnly;
                }
            }
        }


        /// <summary>
        /// Computes the hash codes.
        /// </summary>
        /// <param name="resetDemand">The reset demand.</param>
        /// <param name="filepath1">The filepath1.</param>
        /// <param name="txtMd">The TXT md.</param>
        /// <param name="txtSHA">The TXT SHA.</param>
        private void StartHashProcess(AutoResetEvent resetDemand,
                                      string filepath1, Quadrant[] quadrants)
        {
            FileInfo hf = new FileInfo(filepath1);

            if (!hf.Exists)
            {
                string message = "File not found.";
                this.PrintHash(quadrants, message);
                return;
            }

            if (hf.Length > 0)
            {
                GUI.SetCursorWait(true);
                this.InitProgressBar(hf.Length);
                SupportedHashAlgo[] algorithMs = { parent.PrimaryHash, parent.AlternativeHash };
                this.ComputeHashes(resetDemand, algorithMs, hf.FullName, parent.HexadecimalStyle, quadrants);
            }
            else
            {
                string message = "Empty file.";
                this.PrintHash(quadrants, message);
            }

        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="resetDemand">The reset demand.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="path">The path.</param>
        /// <param name="style">The style.</param>
        /// <param name="cbe">The cbe.</param>
        /// <returns></returns>
        private void ComputeHashes(AutoResetEvent resetDemand, SupportedHashAlgo[] hashAlgorithms, string filepath,
                                   HexEnum style, Quadrant[] quadrants)
        {

            System.Diagnostics.Debug.WriteLine("==> Compute Hashes for " + filepath);

            if (hashAlgorithms.Length != 2 || quadrants.Length != 2)
            {
                throw new ArgumentException();
            }

            foreach (SupportedHashAlgo algo in hashAlgorithms)
            {
                if (algo.IsKeyed)
                {
                    System.Diagnostics.Debug.WriteLine("Asking key for " + filepath);
                    KeyedHashAlgorithm khash = (KeyedHashAlgorithm)algo.Algorithm;
                    // Ask for key only if it was not just entered
                    KeyedHash khf = new KeyedHash();
                    khf.Owner = this.parent;
                    if (khf.ShowDialog() == true)
                    {
                        khash.Key = khf.GetKey();
                    }
                }
            }

            this.RunWorker(resetDemand, filepath, style, hashAlgorithms, quadrants);
           
        }

        private void RunWorker(AutoResetEvent resetDemand, string filepath, HexEnum style, 
                               SupportedHashAlgo[] hashAlgorithms, Quadrant[] qs)
        {
            BackgroundWorker backWorker = this.backWorkerLeft;
            if (qs[0] == Quadrant.UPPER_RIGHT)
            {
                backWorker = this.backWorkerRight;
            }

            CallbackEntry cbe = new CallbackEntry(backWorker.ReportProgress);
            HashOperationParams hop = new HashOperationParams(resetDemand, filepath, style, cbe);
            hop.Hashes = this.SetHashBoxes(hashAlgorithms, qs);

            if (!backWorker.IsBusy)
            {
                backWorker.RunWorkerAsync(hop);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Backworker is BUSY! Abort.");
            }
        }

        private List<HashBox> SetHashBoxes(SupportedHashAlgo[] hashAlgorithms, Quadrant[] quadrants)
        {
            HashBox upper = new HashBox();
            upper.Algorithm = hashAlgorithms[0];
            upper.HashQuadrant = quadrants[0];
            HashBox lower = new HashBox();
            lower.Algorithm = hashAlgorithms[1];
            lower.HashQuadrant = quadrants[1];
            List<HashBox> hashes = new List<HashBox>();
            hashes.Add(upper);
            hashes.Add(lower);
            return hashes;
        }

        private void PrintHash(Quadrant[] qs, string message)
        {
            foreach (Quadrant q in qs)
            {
                this.PrintHash(q, message);
            }
        }

        private void PrintHash(Quadrant q, string message)
        {
            HashBox hb = new HashBox();
            hb.HashQuadrant = q;
            hb.Message = message;
            hb.Hash = message;
            this.PrintHash(hb);
        }

        private void PrintHash(HashBox box)
        {
            TextBox target = null;

            switch (box.HashQuadrant)
            {
                case Quadrant.UPPER_LEFT:
                    target = this.parent.txtHash1Left;
                    break;

                case Quadrant.LOWER_LEFT:
                    target = this.parent.txtHash2Left;
                    break;

                case Quadrant.UPPER_RIGHT:
                    target = this.parent.txtHash1Right;
                    break;

                case Quadrant.LOWER_RIGHT:
                    target = this.parent.txtHash2Right;
                    break;
            }

            GUI.SetHashResult(target, box.Hash);
        }

        private void UpdateProgress(int progress, HashBox partialResult)
        {
            // Update hash
            if (partialResult != null)
            {
                System.Diagnostics.Debug.WriteLine("Receiving partial result message: " + partialResult.Message);
                if (!String.IsNullOrEmpty(partialResult.Message))
                {
                    parent.statusBar.Content = partialResult.Message;
                }

                if (!String.IsNullOrEmpty(partialResult.Hash))
                {
                    System.Diagnostics.Debug.WriteLine("Printing hash " + partialResult.Hash);
                    this.PrintHash(partialResult);
                }
            }

            // Update progress
            try
            {
                double maxVal = this.parent.progressBar.Maximum;
                if (progress >= 0)
                {
                    // Update no more than 100 times
                    // WPF CRITICAL!!!
                    int updateFactor = Convert.ToInt32(maxVal / 100.0);
                    if ((progress % updateFactor == 0) || (progress == maxVal))
                    {
                        this.parent.progressBar.Value = progress;
                        this.SetWin7TaskbarValue(progress, Convert.ToInt32(maxVal));
                    }
                }
                else if (progress == -1)
                {
                    // Do nothing
                }
                else
                {
                    this.parent.progressBar.Maximum = -progress;
                    this.parent.progressBar.Value = 0;
                    this.SetWin7TaskbarValue(0, -progress);
                }
            }
            catch (ArgumentException)
            {
                this.parent.progressBar.Value = this.parent.progressBar.Maximum;
                System.Diagnostics.Debug.WriteLine("Progress Bar value exception: i=" + progress +
                                                   ". Maximum is " + this.parent.progressBar.Maximum);
            }
        }


        private void AsyncHashCompute(BackgroundWorker bw, DoWorkEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("=== BEGIN AsyncCompute " + bw.GetHashCode().ToString() + " ===");

            lock (this.lockCritical)
            {
                IHash hashGen = new Hash();

                // Extract arguments
                HashOperationParams arg = (HashOperationParams)e.Argument;
                HashBox result = arg.Hashes[0];

                // File to hash (set max progress bar)
                FileInfo hf = new FileInfo(arg.FilePath);
                result.Message = String.Format("Hashing {0}", this.ConciseFilePath(hf.FullName));
                bw.ReportProgress(0, result);

                // Setup hash algo and quadrant
                foreach (HashBox hash in arg.Hashes)
                {
                    AvailableHash hashAlgorithm = hash.Algorithm.Id;
                    Quadrant quadrant = hash.HashQuadrant;

                    hashGen.SetAlgorithm(hashAlgorithm);
                    System.Diagnostics.Debug.WriteLine("= Computing " + hashAlgorithm.ToString() + " for " + quadrant.ToString());
                    hash.Hash = hashGen.ComputeHashFileStyleEx(arg.FilePath, arg.Style, arg.Cbe, arg.ResetDemand);

                    bw.ReportProgress(0, hash);
                }

                // If the operation was canceled by the user, 
                // set the DoWorkEventArgs.Cancel property to true.
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                }  
            }

            System.Diagnostics.Debug.WriteLine("=== END AsyncCompute " + bw.GetHashCode().ToString() + " ===");
        }

        private BackgroundWorker CreateBackgroundWorker()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            return bw;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Background progress complete");

            GUI.SetCursorWait(false);
            this.parent.progressBar.Visibility = System.Windows.Visibility.Hidden;

            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageTaskDialog.Show(parent, IsideLogic.Config.APPNAME, "Process was canceled by user", "Operation aborted", TaskDialogType.INFO);
            }
            else if (e.Error != null) // this will print Exceptions thrown in the DoWork phase
            {
                // There was an error during the operation.
                MessageTaskDialog.Show(parent, IsideLogic.Config.APPNAME, e.Error.Message, "Hash error", TaskDialogType.ERROR);
            }

            if (parent.skipHashComputing[1] && parent.skipHashComputing[0])
            {
                parent.canCompareNow = true;
            }

            this.RefreshCombos();
            this.ClearWin7TaskbarProgress();
            this.HighlightEquals();
            parent.statusBar.Content = "Ready";
        }

        private void HighlightEquals()
        {
            SolidColorBrush defaultColor = new SolidColorBrush(Colors.White);
            SolidColorBrush highlight = new SolidColorBrush(Color.FromRgb(220, 250, 220));

            if (parent.fileProperties[1].FileName.Length > 0 && parent.fileProperties[0].FileName.Length > 0)
            {
                // File properties
                if (parent.txtFileSize.Text.Equals(parent.txtFileSizeVS.Text))
                {
                    parent.txtFileSize.Background = highlight;
                    parent.txtFileSizeVS.Background = highlight;
                }
                else
                {
                    parent.txtFileSize.Background = defaultColor;
                    parent.txtFileSizeVS.Background = defaultColor;
                }

                if (parent.txtCreationDate.Text.Equals(parent.txtCreationDateVS.Text))
                {
                    parent.txtCreationDate.Background = highlight;
                    parent.txtCreationDateVS.Background = highlight;
                }
                else
                {
                    parent.txtCreationDate.Background = defaultColor;
                    parent.txtCreationDateVS.Background = defaultColor;
                }

                if (parent.txtLastAccess.Text.Equals(parent.txtLastAccessVS.Text))
                {
                    parent.txtLastAccess.Background = highlight;
                    parent.txtLastAccessVS.Background = highlight;
                }
                else
                {
                    parent.txtLastAccess.Background = defaultColor;
                    parent.txtLastAccessVS.Background = defaultColor;
                }

                if (parent.txtLastModified.Text.Equals(parent.txtLastModifiedVS.Text))
                {
                    parent.txtLastModified.Background = highlight;
                    parent.txtLastModifiedVS.Background = highlight;
                }
                else
                {
                    parent.txtLastModified.Background = defaultColor;
                    parent.txtLastModifiedVS.Background = defaultColor;
                }

                // Hashes
                if (parent.txtHash1Left.Text.Equals(parent.txtHash1Right.Text))
                {
                    parent.txtHash1Left.Background = highlight;
                    parent.txtHash1Right.Background = highlight;
                }
                else
                {
                    parent.txtHash1Left.Background = defaultColor;
                    parent.txtHash1Right.Background = defaultColor;
                }
                if (parent.txtHash2Left.Text.Equals(parent.txtHash2Right.Text))
                {
                    parent.txtHash2Left.Background = highlight;
                    parent.txtHash2Right.Background = highlight;
                }
                else
                {
                    parent.txtHash2Left.Background = defaultColor;
                    parent.txtHash2Right.Background = defaultColor;
                }
            }
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker snd = sender as BackgroundWorker;
            System.Diagnostics.Debug.WriteLine("Starting Background progress " + snd.GetHashCode().ToString());
            this.AsyncHashCompute(snd, e);
        }

        private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.UpdateProgress(e.ProgressPercentage, (HashBox)e.UserState);
        }

        private void ClearWin7TaskbarProgress()
        {
            if (this.isWin7)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            }
        }

        private void SetWin7TaskbarValue(int value, int maximum)
        {
            if (this.isWin7)
            {
                TaskbarManager.Instance.SetProgressValue(value, maximum);
            }
        }

        /// <summary>
        /// Refreshes the combos.
        /// </summary>
        private void RefreshCombos()
        {

            AvailableHash hash1 = parent.PrimaryHash.Id;
            AvailableHash hash2 = parent.AlternativeHash.Id;

            HashLogic.SyncHashCombo(hash1, this.parent.cboSelHashLeft1);
            HashLogic.SyncHashCombo(hash2, this.parent.cboSelHashLeft2);
            HashLogic.SyncHashCombo(hash1, this.parent.cboSelHashRight1);
            HashLogic.SyncHashCombo(hash2, this.parent.cboSelHashRight2);

        }

        private string ConciseFilePath(string filepath)
        {
            int maxChars = 34;

            if (this.parent.guiWidth == FormWidth.DOUBLE)
            {
                maxChars = 64;
            }

            string concise = filepath;

            if (concise.Length > maxChars)
            {
                string fileName = Path.GetFileName(filepath);
                int fileNameLen = fileName.Length;
                string restOfPath = Path.GetDirectoryName(filepath) + Path.DirectorySeparatorChar;
                int restOfPathLen = restOfPath.Length;

                int counter = 0;
                concise = fileName;
                while (concise.Length < maxChars)
                {
                    counter++;
                    if (counter <= restOfPathLen)
                    {
                        concise = restOfPath[restOfPathLen - counter] + concise;
                    }
                    else
                    {
                        break;
                    }
                }
                concise = "..." + concise;
            }

            return concise;
        }

    }
}

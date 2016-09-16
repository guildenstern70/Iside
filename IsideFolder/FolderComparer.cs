/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using AxsUtils;
using IsideLogic;
using LLCryptoLib.Hash;

namespace IsideFolder
{
    /// <summary>
    /// FolderComparer.
    /// </summary>
    public class FolderComparer
    {

        private StringBuilder briefSummary;
        private DetailedSummary detailedSummary;

        private Comparison actualComparison;

        private long nrFiles1;
        private long nrFiles2;

        private DirectoryElements de1;
        private DirectoryElements de2;

        private bool bNameEquality;
        private bool bSizeEquality;
        private bool bHashEquality;

        private bool exitOnFirstDifference;
        private AvailableHash hashFunction;


        private string errorMsg;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderComparer" /> class.
        /// </summary>
        /// <param name="comp">The comp.</param>
        /// <param name="comparingHash">The comparing hash.</param>
        /// <param name="includeHidden">if set to <c>true</c> [include hidden].</param>
        /// <param name="includeSystem">if set to <c>true</c> [include system].</param>
        /// <param name="includeArchive">if set to <c>true</c> [include archive].</param>
        /// <exception cref="IsideException">Throws AxsException when folder contains access denied elements.</exception>
        public FolderComparer(DetailedComparison comp, AvailableHash comparingHash)
        {
            this.hashFunction = comparingHash;
            this.detailedSummary = new DetailedSummary(SupportedHashAlgoFactory.GetAlgo(comparingHash));

            this.briefSummary = new StringBuilder("SUMMARY");
            this.briefSummary.Append(Environment.NewLine);
            this.briefSummary.Append(Environment.NewLine);
            this.briefSummary.Append(this.SummaryProperties(false, comp.NrOfFolders, comp.NrOfFiles));

            this.actualComparison = comp;
            try
            {
                this.Initialize(comp.IncludeHidden, comp.IncludeSystem, comp.IncludeArchive);
            }
            catch (AxsException)
            {
                throw new IsideException("Folder contains access denied files.");
            }
            this.BuildSummaryHeader();

        }

        /// <summary>
        /// Count file number and populates "files to check" list
        /// </summary>
        /// <returns>Number of files to check, or -1 if number of files differs</returns>
        /// <exception cref="AxsException">Invalid directory elements list.</exception>
        private void Initialize(bool wHidden, bool wSystem, bool wArchive)
        {

            this.errorMsg = "";
            this.exitOnFirstDifference = true;

            this.detailedSummary.RowsNumber = this.nrFiles1;

            this.de1 = new DirectoryElements();
            this.de1.IncludeArchiveFiles = wArchive;
            this.de1.IncludeHiddenFiles = wHidden;
            this.de1.IncludeSystemFiles = wSystem;
            this.de1.Scan(this.actualComparison.LeftPath, true);

            this.nrFiles1 = this.de1.NrOfFiles;

            if (!this.actualComparison.RightPath.FullName.EndsWith(".isrl"))
            {
                this.de2 = new DirectoryElements();
                this.de2.IncludeArchiveFiles = wArchive;
                this.de2.IncludeHiddenFiles = wHidden;
                this.de2.IncludeSystemFiles = wSystem;
                this.de2.Scan((DirectoryInfo)this.actualComparison.RightPath, true);
                this.nrFiles2 = this.de2.NrOfFiles;
            }
            else
            {
                FileManager fm = FileManager.Reference;
                ResultsFile rf = ResultsFile.FromFile(this.actualComparison.RightPath.FullName);
                this.nrFiles2 = rf.NrOfFiles;
                System.Diagnostics.Debug.WriteLine("File " + this.actualComparison.RightPath.FullName + " has " + this.nrFiles2 + " lines.");
            }
        }

        #region Properties
        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary
        {
            get
            {
                return this.briefSummary.ToString();
            }
        }

        /// <summary>
        /// Gets the actual comparison.
        /// </summary>
        /// <value>The actual comparison.</value>
        public Comparison ActualComparison
        {
            get
            {
                return this.actualComparison;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has log.
        /// </summary>
        /// <value><c>true</c> if this instance has log; otherwise, <c>false</c>.</value>
        public bool HasLog
        {
            get
            {
                return this.detailedSummary.HasLog;
            }
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        internal DetailedSummary Log
        {
            get
            {
                return this.detailedSummary;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [exit on first diff].
        /// </summary>
        /// <value><c>true</c> if [exit on first diff]; otherwise, <c>false</c>.</value>
        public bool ExitOnFirstDiff
        {
            get
            {
                return this.exitOnFirstDifference;
            }

            set
            {
                this.exitOnFirstDifference = value;
            }
        }

        public AvailableHash HashAlgorithm
        {
            get
            {
                return this.hashFunction;
            }

            set
            {
                this.hashFunction = value;
                if (this.detailedSummary != null)
                {
                    this.detailedSummary.HashInfo = SupportedHashAlgoFactory.GetAlgo(value);
                }
            }
        }

        public bool SizeEquality
        {
            get
            {
                return this.bSizeEquality;
            }
        }

        public bool NameEquality
        {
            get
            {
                return this.bNameEquality;
            }
        }

        public bool HashEquality
        {
            get
            {
                return this.bHashEquality;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this.errorMsg;
            }
        }

        public DirectoryElements DirElements1
        {
            get
            {
                return this.de1;
            }
        }

        public DirectoryElements DirElements2
        {
            get
            {
                return this.de2;
            }
        }
        #endregion

        /// <summary>
        /// Comparison based on results list. Significantly different from
        /// the normal comparison -> CompareAll(worker)
        /// </summary>
        /// <param name="applicationPath"></param>
        /// <param name="resultPath"></param>
        /// <param name="worker"></param>
        internal void CompareAll(string resultPath, BackgroundWorker worker)
        {
            FileInfo[] fi1 = new FileInfo[this.de1.Files.Count];
            this.de1.Files.CopyTo(fi1, 0);
            int numberOfItems = fi1.Length;

            this.CompareInit();
            ResultsFile rf = FolderComparer.ReadResultsFile(resultPath);
            this.hashFunction = rf.Hash;
            string[] results = rf.Results();

            // Sort arrays
            FileInfoComparer comparer = new FileInfoComparer();
            Array.Sort(fi1, comparer); // (results should be already sorted)

            // Check Name Equality
            string[] namesResult = new string[numberOfItems];
            Array.Copy(results, 1, namesResult, 0, numberOfItems);
            this.bNameEquality = this.CheckEquality(ComparisonMethod.NAME, fi1, namesResult, worker, true);
            System.Diagnostics.Debug.WriteLine("Name Equality = " + this.bNameEquality);

            // Check Size Equality
            if (this.hasToExitOnDifference(this.bNameEquality)) return;
            if (worker != null)
            {
                worker.ReportProgress(-1); // reset feedback progress 
            }
            string[] sizesResult = new string[numberOfItems];
            Array.Copy(results, numberOfItems+2, sizesResult, 0, numberOfItems);
            this.bSizeEquality = this.CheckEquality(ComparisonMethod.SIZE, fi1, sizesResult, worker, true);
            System.Diagnostics.Debug.WriteLine("Size Equality = " + this.bSizeEquality);

            // Check Hash Equality
            if (this.hasToExitOnDifference(this.bSizeEquality)) return;
            worker.ReportProgress(-1);
            string[] hashesResult = new string[numberOfItems];
            Array.Copy(results, numberOfItems * 2+3, hashesResult, 0, numberOfItems);
            this.bHashEquality = this.CheckEquality(ComparisonMethod.HASH, fi1, hashesResult, worker, true);
            System.Diagnostics.Debug.WriteLine("Hash Equality = " + this.bHashEquality);

            this.CompareFinalize();
        }

        /// <summary>
        /// Normal comparison between 2 folders
        /// </summary>
        /// <param name="applicationPath"></param>
        /// <param name="i"></param>
        internal void CompareAll(BackgroundWorker bw)
        {
            FileInfo[] fi1 = new FileInfo[this.de1.Files.Count];
            FileInfo[] fi2 = new FileInfo[this.de2.Files.Count];

            this.de1.Files.CopyTo(fi1, 0);
            this.de2.Files.CopyTo(fi2, 0);

            this.CompareInit();

            // Check on number of folder files
            if (fi1.Length != fi2.Length)
            {
                this.briefSummary.Append(Environment.NewLine);
                this.briefSummary.Append("Folders contain different number of files.");
                return;
            }

            // Sort arrays
            FileInfoComparer comparer = new FileInfoComparer();
            Array.Sort(fi1, comparer);
            Array.Sort(fi2, comparer);

            // Check Name Equality
            this.bNameEquality = this.CheckEquality(ComparisonMethod.NAME, fi1, fi2, bw, true);
            System.Diagnostics.Debug.WriteLine("Name Equality = " + this.bNameEquality);

            // Check Size Equality
            if (this.hasToExitOnDifference(this.bNameEquality)) return;
            if (bw != null)
            {
                bw.ReportProgress(-1); // reset feedback progress bar
            }
            this.bSizeEquality = this.CheckEquality(ComparisonMethod.SIZE, fi1, fi2, bw, true);
            System.Diagnostics.Debug.WriteLine("Size Equality = " + this.bSizeEquality);

            // Check Hash Equality
            if (this.hasToExitOnDifference(this.bSizeEquality)) return;
            if (bw != null)
            {
                bw.ReportProgress(-1);
            }
            this.bHashEquality = this.CheckEquality(ComparisonMethod.HASH, fi1, fi2, bw, true);
            System.Diagnostics.Debug.WriteLine("Hash Equality = " + this.bHashEquality);


            this.CompareFinalize();
        }

        /// <summary>
        /// Reads an ".isrl" file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static ResultsFile ReadResultsFile(string path)
        {
            ResultsFile rf = null;
            if (path != null)
            {
                if (File.Exists(path))
                {
                    rf = ResultsFile.FromFile(path);
                }
            }
            return rf;
        }

        /// <summary>
        /// Saves the ISRL results.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="nrOfFolders">The nr of folders.</param>
        /// <param name="nrOfFiles">The nr of files.</param>
        /// <returns></returns>
        public bool SaveResults(string path, int nrOfFolders, int nrOfFiles)
        {
            bool success = false;
            string contents = this.SummaryProperties(true, nrOfFolders, nrOfFiles);

            if (this.detailedSummary != null)
            {
                contents += this.detailedSummary.GetResults();
            }

            if (contents != null)
            {
                FileManager fm = FileManager.Reference;
                success = fm.SaveFile(path, contents);
            }

            return success;
        }

        /// <summary>
        /// Check for equality.
        /// A list of files (f1) is compared, based on ComparisonMethod c, against results stored in results.
        /// </summary>
        /// <param name="c">Comparison method (ie: name, size, hash)</param>
        /// <param name="fi1">A list of files</param>
        /// <param name="results">A list of results</param>
        /// <param name="i"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        private bool CheckEquality(ComparisonMethod c, FileInfo[] fi1, string[] results, BackgroundWorker worker, bool record)
        {

            System.Diagnostics.Debug.Assert(results.Length == fi1.Length, "Results # is different from FileInfo #");
            ComparerFunctor cf = new ComparerFunctor(c, this.hashFunction);
            System.Diagnostics.Debug.Write("Checking {0} equality against saved results", cf.ToString());
            int sectionIndex = this.detailedSummary.AddSection(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cf.ToString()));

            bool equality = true;
            int currentElement = 0;

            foreach (FileInfo fiTemp1 in fi1)
            {
                string savedResult = results[currentElement];

                System.Diagnostics.Debug.Write(fiTemp1.FullName);
                System.Diagnostics.Debug.Write("  >>> ");
                System.Diagnostics.Debug.Write(savedResult);

                if (cf.CompareOn(fiTemp1, savedResult))
                {
                    System.Diagnostics.Debug.WriteLine("  are (==)");
                }
                else
                {
                    this.BuildSummaryString(fiTemp1, savedResult, cf.ToString());
                    equality = false;
                    System.Diagnostics.Debug.WriteLine("  are (!=)");
                }

                if (record)
                {
                    ResultRecord rr = new ResultRecord(fiTemp1, "Results list", cf.LeftProperty, cf.RightProperty);
                    this.detailedSummary.Sections[sectionIndex].AddSummaryRow(rr);
                }


                // Run callback function
                currentElement++;

                if (worker != null)
                {
                    worker.ReportProgress(currentElement);
                }

                if (!equality)
                {
                    // Return at the first non equality found?
                    if ((this.exitOnFirstDifference) && (worker != null))
                    {
                        worker.ReportProgress(fi1.Length);
                        return false;
                    }
                }
            }

            return equality;
        }

        private bool CheckEquality(ComparisonMethod c, FileInfo[] fi1, FileInfo[] fi2, BackgroundWorker bw, bool record)
        {
            System.Diagnostics.Debug.Assert(fi2.Length == fi1.Length, "FileInfo Arrays contain different number of items");

            ComparerFunctor cf = new ComparerFunctor(c, this.hashFunction);
            System.Diagnostics.Debug.WriteLine(String.Format("Checking {0} equality", cf.ToString()));
            int sectionIndex = this.detailedSummary.AddSection(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cf.ToString()));

            bool equality = true;

            int currentElement = 0;

            foreach (FileInfo fiTemp1 in fi1)
            {
                FileInfo fiTemp2 = fi2[currentElement];

                System.Diagnostics.Debug.Write(fiTemp1.FullName);
                System.Diagnostics.Debug.Write("  >>> ");
                System.Diagnostics.Debug.Write(fiTemp2.FullName);

                if (cf.CompareOn(fiTemp1, fiTemp2))
                {
                    System.Diagnostics.Debug.WriteLine("  are (==)");
                }
                else
                {
                    this.BuildSummaryString(fiTemp1, fiTemp2, "Different " + cf.ToString());
                    equality = false;
                    System.Diagnostics.Debug.WriteLine("  are (!=)");
                }

                if (record)
                {
                    ResultRecord rr = new ResultRecord(fiTemp1, fiTemp2, cf.LeftProperty, cf.RightProperty);
                    this.detailedSummary.Sections[sectionIndex].AddSummaryRow(rr);
                }


                // Run callback function
                currentElement++;
                if (bw != null)
                {
                    bw.ReportProgress(currentElement);
                }

                if (!equality)
                {
                    // Return at the first non equality found?
                    if ((this.exitOnFirstDifference) && (bw != null))
                    {
                        bw.ReportProgress(fi1.Length);
                        return false;
                    }
                }
            }

            return equality;
        }

        private void CompareInit()
        {
            this.detailedSummary.SetStart();
        }

        private void CompareFinalize()
        {
            // If no differences are found...
            if (this.bHashEquality)
            {
                this.briefSummary.Append(Environment.NewLine);
                this.briefSummary.Append("No differences found.");
            }
        }

        private void BuildSummaryString(FileInfo f1, FileInfo f2, string reason)
        {
            string newline = Environment.NewLine;

            this.briefSummary.Append(newline);
            this.briefSummary.Append(f1.FullName);

            if (f2 != null)
            {
                this.briefSummary.Append(newline);
                this.briefSummary.Append("  <>  ");
                this.briefSummary.Append(newline);
                this.briefSummary.Append(f2.FullName);
            }

            this.briefSummary.Append(newline);
            this.briefSummary.Append("(");
            this.briefSummary.Append(reason);
            this.briefSummary.Append(")");
            this.briefSummary.Append(newline);
            this.briefSummary.Append(newline);
        }

        private void BuildSummaryString(FileInfo f1, string s, string reason)
        {
            string newline = Environment.NewLine;

            this.briefSummary.Append(newline);
            this.briefSummary.Append(f1.FullName);
            this.briefSummary.Append(" does not match with saved result.");
            this.briefSummary.Append(newline);

            if (s != null)
            {
                this.briefSummary.Append("Different ");
                this.briefSummary.Append(reason);
                this.briefSummary.Append(": ");
                this.briefSummary.Append(s);
            }

            this.briefSummary.Append(newline);
            this.briefSummary.Append(newline);
        }

        private void BuildSummaryHeader()
        {
            this.briefSummary.Append(Environment.NewLine);
            this.briefSummary.Append(Environment.NewLine);
            this.briefSummary.Append("Left directory:  ");
            this.briefSummary.Append(this.actualComparison.LeftPath);
            if (this.de1 != null)
            {
                this.briefSummary.Append(" (");
                this.briefSummary.Append(this.de1.NrOfFiles);
                this.briefSummary.Append(" files)");
            }
            this.briefSummary.Append(Environment.NewLine);
            this.briefSummary.Append("Right directory: ");
            this.briefSummary.Append(this.actualComparison.RightPath);
            if (this.de2 != null)
            {
                this.briefSummary.Append(" (");
                this.briefSummary.Append(this.de2.NrOfFiles);
                this.briefSummary.Append(" files)");
            }
            this.briefSummary.Append(Environment.NewLine);
            this.briefSummary.Append(Environment.NewLine);
            this.briefSummary.Append("COMPARISON RESULTS");
            this.briefSummary.Append(Environment.NewLine);
        }

        private string SummaryProperties(bool commented, int nrOfFolders, int nrOfFiles)
        {
            StringBuilder sb = new StringBuilder();
            string hashName = SupportedHashAlgoFactory.GetAlgo(this.hashFunction).Name;

            if (!commented)
            {
                sb.AppendLine("Machine:               " + AxsUtils.Win32.OS.MachineName);
                sb.AppendLine("Operating system:      " + AxsUtils.Win32.OS.OperatingSystem);
                sb.AppendLine("Operations date:       " + DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString());
                sb.AppendLine("Operations started by: " + AxsUtils.Win32.OS.UserInfo);
                sb.AppendLine("Hash algorithm:        " + hashName);
                sb.AppendLine("Number of Folders:     " + nrOfFolders.ToString());
                sb.AppendLine("Number of Files:       " + nrOfFiles.ToString());
            }
            else
            {
                sb.AppendLine("# Machine:               " + AxsUtils.Win32.OS.MachineName);
                sb.AppendLine("# Operating system:      " + AxsUtils.Win32.OS.OperatingSystem);
                sb.AppendLine("# Operations date:       " + DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString());
                sb.AppendLine("# Operations started by: " + AxsUtils.Win32.OS.UserInfo);
                sb.AppendLine("# Hash algorithm:        " + hashName);
                sb.AppendLine("# Number of Folders:     " + nrOfFolders.ToString());
                sb.AppendLine("# Number of Files:       " + nrOfFiles.ToString());
            }

            sb.AppendLine();

            return sb.ToString();
        }

        private bool hasToExitOnDifference(bool equalityResult)
        {
            bool hasToExit = false;
            if (this.exitOnFirstDifference)
            {
                if (!equalityResult)
                    hasToExit = true;
            }
            return hasToExit;
        }

    }
}

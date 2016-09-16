/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Collections.Generic;
using IsideLogic;
using LLCryptoLib.Hash;

namespace IsideFolder
{
    /// <summary>
    /// DetailedSummary
    /// </summary>
    internal class DetailedSummary
    {
        private StringCollection _header;
        private StringCollection _footer;
        private List<DetailedSummarySection> _sections;
        private long _rows;
        private SupportedHashAlgo _hashInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        public DetailedSummary(SupportedHashAlgo hashName)
        {
            this._header = new StringCollection();
            this._footer = new StringCollection();
            this._sections = new List<DetailedSummarySection>();
            this._rows = -1;
            this._hashInfo = hashName;
        }

        /// <summary>
        /// Gets or sets the hash info.
        /// </summary>
        /// <value>The hash info.</value>
        public SupportedHashAlgo HashInfo
        {
            get
            {
                return this._hashInfo;
            }

            set
            {
                this._hashInfo = value;
            }
        }

        /// <summary>
        /// If the summary contains a log
        /// </summary>
        public bool HasLog
        {
            get
            {
                bool hasRecords = true;

                if (this.RecordCount == 0)
                {
                    hasRecords = false;
                }

                return hasRecords;
            }
        }

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <value>
        /// The record count.
        /// </value>
        public int RecordCount
        {
            get
            {
                int records = 0;

                foreach (DetailedSummarySection dss in this._sections)
                {
                    records += dss.Count;
                }

                return records;
            }
        }

        /// <summary>
        /// The number of files processed. This is useful
        /// because each file is processed a number of times
        /// (ie: 3, for name size and hash)
        /// </summary>
        public long RowsNumber
        {
            get
            {
                return this._rows;
            }

            set
            {
                this._rows = value;
            }
        }

        /// <summary>
        /// Adds the line to footer.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void AddLineToHeader(string msg)
        {
            this._header.Add(msg);
        }

        /// <summary>
        /// Adds the line to footer.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void AddLineToFooter(string msg)
        {
            this._footer.Add(msg);
        }


        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <returns></returns>
        public string GetResults()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DetailedSummarySection section in this._sections)
            {
                sb.AppendLine(section.GetResults());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Adds the section.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The index of the section created</returns>
        /// <exception cref="System.Exception"></exception>
        public int AddSection(string name)
        {
            if (this._rows < 0)
            {
                throw new Exception("Detailed summary rows not initialized!");
            }

            this._sections.Add(new DetailedSummarySection(name, this._rows));

            return (this._sections.Count - 1);
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        public IList<DetailedSummarySection> Sections
        {
            get
            {
                return this._sections;
            }
        }

        /// <summary>
        /// Set operations start (updates header)
        /// </summary>
        public void SetStart()
        {
            this.Header();
        }

        /// <summary>
        /// Summaries as text.
        /// </summary>
        /// <returns></returns>
        public string SummaryAsText()
        {
            StringBuilder log = new StringBuilder();

            foreach (string s in this._header)
            {
                log.Append(s);
                log.Append(Environment.NewLine);
            }

            foreach (DetailedSummarySection section in this._sections)
            {
                log.Append(section.SectionSummaryAsText());
            }

            foreach (string s in this._footer)
            {
                log.Append(s);
                log.Append(Environment.NewLine);
            }

            return log.ToString();
        }

        /// <summary>
        /// Summaries as CSV.
        /// </summary>
        /// <returns></returns>
        public string SummaryAsCSV(string rightPath, string leftPath)
        {
            StringBuilder sb = new StringBuilder();

            // Header
            sb.Append(IsideLogic.Config.APPFOLDER + " " + IsideLogic.Config.AppVersion);
            sb.Append(';');
            sb.Append("Comparison results of " + DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString());
            sb.Append(';');
            sb.Append(" ");
            sb.Append(Environment.NewLine);
            if (this._hashInfo != null)
            {
                sb.Append("Hash algorithm: ");
                sb.Append(this._hashInfo);
            }
            else
            {
                sb.Append(" ");
            }
            sb.Append(';');
            sb.Append(" ");
            sb.Append(';');
            sb.Append(" ");
            sb.Append(Environment.NewLine);

            // Root left and right
            string rootLeft = rightPath;
            string rootRight = leftPath;

            // Body
            sb.Append(" ");
            sb.Append(';');
            sb.Append(rootLeft);
            sb.Append(';');
            sb.Append(rootRight);
            sb.Append(Environment.NewLine);

            foreach (DetailedSummarySection section in this._sections)
            {
                sb.Append(section.SectionSummaryAsCSV(rootLeft));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Summaries as HTML.
        /// </summary>
        /// <returns></returns>
        public string SummaryAsHTML(string rightPath, string leftPath)
        {
            StringBuilder sb = new StringBuilder();

            string machine = AxsUtils.Win32.OS.MachineName;
            string operatingsystem = AxsUtils.Win32.OS.OperatingSystem;
            string date = DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString();
            string started = AxsUtils.Win32.OS.UserInfo;

            sb.Append(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">");
            sb.Append(@"<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=iso-8859-1""><title>Iside Folder Report</title><style type=""text/css"">");
            sb.Append(@"<!-- .style1 {color: #FFFFFF} .style2 { font-family: ""Courier New"", Courier, mono; font-size: small; } .style4 {color: #FFFFFF; font-weight: bold; } .style5 { font-family: Verdana, Arial, Helvetica, sans-serif; font-weight: bold; color: #0033ff; } .style6 {font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 10px; } -->");
            sb.Append(@"</style></head><body><span class=""style5"">Iside Folder Comparison</span><br><span class=""style6""><a href=""http://www.littlelite.net/iside"">LittleLite Iside</a> ");
            sb.Append(IsideLogic.Config.AppVersionWithBuild);
            sb.Append("</span></span></p>");
            sb.Append(@"<p><p class=""style6"">Machine:&nbsp;");
            sb.Append(machine);
            sb.Append(@"<br>Operating system:&nbsp;");
            sb.Append(operatingsystem);
            sb.Append(@"<br>Operations date:&nbsp;<br>");
            sb.Append(date);
            sb.Append(@"<br>Operations started by:&nbsp;");
            sb.Append(started);
            if (this._hashInfo != null)
            {
                sb.Append(@"<br>Hash algorithm:&nbsp;");
                sb.Append(this._hashInfo);
            }

            sb.Append("</p>");
            sb.Append(@"<table width=""90%""  border=""1""><tr bgcolor=""#0033FF""><td><span class=""style1""><b>File</b></span></td>");

            // Root left and right
            string rootLeft = rightPath;
            string rootRight = leftPath;

            // Header
            sb.Append(@"<td><span class=""style1"">");
            sb.Append(rootLeft);
            sb.Append(@"</span></td><td><span class=""style1"">");
            sb.Append(rootRight);
            sb.Append(@"</span></td></tr>");

            // Body
            foreach (DetailedSummarySection section in this._sections)
            {
                sb.Append(section.SectionSummaryAsHTML(rootLeft));
            }

            // End
            sb.Append("</table><p>&nbsp; </p></body></html>");

            return sb.ToString();
        }

        private void Header()
        {
            string headerInfo = TextReport.StateInfo(this._hashInfo);
            this._header.Add("COMPARISON RESULTS");
            this._header.Add(Environment.NewLine);
        }
    }
}

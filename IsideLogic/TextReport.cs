/**
 **   Iside
 **   Confront files with a single click
 **
 **   Copyright © LittleLite Software
 **
 **
 **/

using System;
using System.Text;
using AxsUtils;
using LLCryptoLib.Hash;

namespace IsideLogic
{
    /// <summary>
    /// 
    /// </summary>
    public class TextReport
    {
        private HashReport[] reports;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextReport"/> class.
        /// </summary>
        /// <param name="hashcodesSummary">The hashcodes summary.</param>
        /// <param name="areIdentical">if set to <c>true</c> [are identical].</param>
        public TextReport(HashReport[] hashcodesSummary)
        {
            this.reports = hashcodesSummary;
        }

        /// <summary>
        /// Gets the report.
        /// </summary>
        public string[] GetReport()
        {
            return this.BuildReport();
        }

        private string[] BuildReport()
        {
            StringBuilder report = new StringBuilder();
            report.Append(TextReport.StateInfo(null));
            report.AppendLine();

            report.Append(this.reports[0].ToString());
            if (this.reports[1].FilePath != null)
            {
                report.Append(this.reports[1].ToString());
                report.AppendLine();
                if (this.reports[0].Hashes[0].Equals(this.reports[1].Hashes[0]) &&
                    this.reports[0].Hashes[1].Equals(this.reports[1].Hashes[1]))
                {
                    report.AppendLine("Files are identical.");
                }
                else
                {
                    report.AppendLine("Files are NOT identical.");
                }
            }
            report.AppendLine();

            string reportstr = report.ToString();
            return reportstr.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }


        /// <summary>
        /// States the info.
        /// </summary>
        /// <param name="hashInfo">The hash info.</param>
        /// <returns></returns>
        public static string StateInfo(SupportedHashAlgo hashInfo)
        {
            StringBuilder header = new StringBuilder();

            string machine = AxsUtils.Win32.OS.MachineName;
            string operatingsystem = AxsUtils.Win32.OS.OperatingSystem;
            string date = DateTime.Now.ToShortDateString()+", "+DateTime.Now.ToLongTimeString();
            string started = AxsUtils.Win32.OS.UserInfo;

            header.AppendLine(IsideLogic.Config.APPNAME + " " + IsideLogic.Config.AppVersionWithBuild);
            header.AppendLine();
            header.AppendLine("Machine: " + machine);
            header.AppendLine("Operating system: " + operatingsystem);
            header.AppendLine("Operations date: " + date);
            header.AppendLine("Operations started by: " + started);
			if (hashInfo!=null)
			{
                header.AppendLine("Hash Algorithm: " + hashInfo.Name);
			}
            header.AppendLine();

            return header.ToString();
        }
    }
}

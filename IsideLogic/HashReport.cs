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
using LLCryptoLib.Hash;

namespace IsideLogic
{
    /// <summary>
    /// 
    /// </summary>
    public class HashReport
    {
        private string filePath;
        private SupportedHashAlgo[] algos;
        private string[] hashes;

        private StringBuilder report;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashReport"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="hashAlgos">The hash algos.</param>
        /// <param name="hashCodes">The hash codes.</param>
        public HashReport(string filename, SupportedHashAlgo[] hashAlgos, string[] hashCodes)
        {
            this.filePath = filename;
            this.algos = hashAlgos;
            this.hashes = hashCodes;

            this.report = new StringBuilder();
        }

        /// <summary>
        /// Gets the hashes.
        /// </summary>
        /// <value>
        /// The hashes.
        /// </value>
        public string[] Hashes
        {
            get { return hashes; }
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
        }

        private void AppendReport(int index)
        {
            if (String.IsNullOrEmpty(this.hashes[index]))
            {
                return;
            }

            if (this.algos[index].Id == AvailableHash.FAKE)
            {
                return;
            }

            this.report.Append("Hash (");
            this.report.Append(this.algos[index].Name);
            this.report.AppendLine("):");
            this.report.AppendLine(this.hashes[index]);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string report = String.Empty;

            if (this.filePath != null)
            {
                this.report.AppendLine();
                this.report.AppendLine("Hash code(s) extracted from: ");
                this.report.AppendLine(this.filePath);
                this.report.AppendLine();
                this.AppendReport(0);
                this.AppendReport(1);
                report = this.report.ToString();
            }

            return report;
        }

    }
}

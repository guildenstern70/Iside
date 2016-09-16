using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsideFolder
{
    class DetailedSummarySection
    {
        private List<ResultRecord> _resultRecords;
        private string _name;
        private long _rows;

        public DetailedSummarySection(string sectionName, long rowsNumber)
        {
            this._rows = rowsNumber;
            this._name = sectionName;
            this._resultRecords = new List<ResultRecord>();
        }

        /// <summary>
        /// Adds a new record to the log
        /// </summary>
        /// <param name="rr"></param>
        public void AddSummaryRow(ResultRecord rr)
        {
            this._resultRecords.Add(rr);
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <returns></returns>
        public string GetResults()
        {
            StringBuilder results = new StringBuilder();

            results.AppendLine(this._name);

            foreach (ResultRecord rr in this._resultRecords)
            {
                results.Append(rr.PropertyLeft);
                results.Append(Environment.NewLine);
            }

            return results.ToString();
        }

        public string SectionSummaryAsText()
        {
            StringBuilder log = new StringBuilder();

            log.AppendLine(this._name);
            foreach (char c in this._name)
            {
                log.Append('=');
            }
            log.Append(Environment.NewLine);

            foreach (ResultRecord rr in this._resultRecords)
            {
                log.Append(rr.ToString());
                log.Append(Environment.NewLine);
            }

            return log.ToString();
        }

        public string SectionSummaryAsCSV(string rootLeft)
        {
            StringBuilder sb = new StringBuilder();

            int count = 0;
            foreach (ResultRecord rr in this._resultRecords)
            {
                if ((this._rows > 0) && (count == this._rows))
                {
                    sb.Append(Environment.NewLine);
                    count = 0;
                }
                sb.Append(rr.ToCSVString(rootLeft));
                sb.Append(Environment.NewLine);
                count++;
            }

            return sb.ToString();
        }

        public string SectionSummaryAsHTML(string rootLeft)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr><td colspan='3'><b>");
            sb.Append(this._name);
            sb.Append("</b></td></td>");

            int count = 0;
            foreach (ResultRecord rr in this._resultRecords)
            {
                if ((this._rows > 0) && (count == this._rows))
                {
                    sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                    count = 0;
                }
                sb.Append(@"<tr class=""style2"">");
                sb.Append(rr.ToHTMLString(rootLeft));
                sb.Append("</tr>");
                count++;
            }

            return sb.ToString();
        }

        public int Count
        {
            get
            {
                return this._resultRecords.Count;
            }
        }


    }
}

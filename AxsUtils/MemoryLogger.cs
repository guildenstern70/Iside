using System;
using System.Collections.Generic;
using System.Text;

namespace AxsUtils
{
    /// <summary>
    ///     Older logger utility class
    /// </summary>
    public class MemoryLogger
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Logger" /> class.
        /// </summary>
        public MemoryLogger()
        {
            this.LogLines = new List<string>();
        }

        /// <summary>
        ///     Gets the log and flushes memory
        /// </summary>
        /// <value>The log.</value>
        public string Log
        {
            get
            {
                StringBuilder log = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Header))
                {
                    log.AppendLine(this.Header);
                    log.AppendLine();
                    log.AppendLine();
                }
                foreach (string msg in this.LogLines)
                {
                    log.AppendLine(msg);
                }
                log.AppendLine();
                log.AppendLine();
                if (!string.IsNullOrEmpty(this.Footer))
                {
                    log.AppendLine(this.Footer);
                }
                this.Clear();
                return log.ToString();
            }
        }

        /// <summary>
        ///     Gets the log lines.
        /// </summary>
        /// <value>The log lines.</value>
        public List<string> LogLines { get; }

        /// <summary>
        ///     Gets the number of messages in this log
        /// </summary>
        /// <value>The number of messages in this log.</value>
        public int Count { get; private set; }

        /// <summary>
        ///     Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header { get; set; }

        /// <summary>
        ///     Gets or sets the footer.
        /// </summary>
        /// <value>The footer.</value>
        public string Footer { get; set; }

        /// <summary>
        ///     Clears current log
        /// </summary>
        public void Clear()
        {
            this.LogLines.Clear();
        }

        /// <summary>
        ///     Adds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Add(string message)
        {
            this.Count++;
            this.LogLines.Add(message);
        }

        /// <summary>
        ///     Appends the specified log to append.
        /// </summary>
        /// <param name="logToAppend">The log to append.</param>
        public void Append(MemoryLogger logToAppend)
        {
            this.LogLines.AddRange(logToAppend.LogLines);
        }

        /// <summary>
        ///     Adds a line.
        /// </summary>
        public void AddEmptyLine()
        {
            this.LogLines.Add(string.Empty);
        }

        /// <summary>
        ///     Adds the specified message with a time stamp
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddWithTime(string message)
        {
            this.Count++;
            this.LogLines.Add(TimeStamp(message));
        }

        /// <summary>
        ///     Adds the time stamp
        /// </summary>
        private static string TimeStamp(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToLongTimeString());
            sb.Append(" > ");
            sb.Append(message);
            return sb.ToString();
        }
    }
}
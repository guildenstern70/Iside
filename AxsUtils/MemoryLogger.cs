using System;
using System.Collections.Generic;
using System.Text;

namespace AxsUtils
{
    /// <summary>
    /// Older logger utility class
    /// </summary>
    public class MemoryLogger
    {
        private List<string> messages;
        private string header;
        private string footer;
        private int messageCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Logger"/> class.
        /// </summary>
        public MemoryLogger()
        {
            this.messages = new List<string>();
        }

        /// <summary>
        /// Gets the log and flushes memory
        /// </summary>
        /// <value>The log.</value>
        public string Log
        {
            get
            {
                StringBuilder log = new StringBuilder();
                if (!String.IsNullOrEmpty(this.header))
                {
                    log.AppendLine(this.header);
                    log.AppendLine();
                    log.AppendLine();
                }
                foreach (string msg in this.messages)
                {
                    log.AppendLine(msg);
                }
                log.AppendLine();
                log.AppendLine();
                if (!String.IsNullOrEmpty(this.footer))
                {
                    log.AppendLine(this.footer);
                }
                this.Clear();
                return log.ToString();
            }
        }

        /// <summary>
        /// Gets the log lines.
        /// </summary>
        /// <value>The log lines.</value>
        public List<string> LogLines
        {
            get
            {
                return this.messages;
            }
        }

        /// <summary>
        /// Gets the number of messages in this log
        /// </summary>
        /// <value>The number of messages in this log.</value>
        public int Count
        {
            get
            {
                return this.messageCount;
            }
        }

        /// <summary>
        /// Clears current log
        /// </summary>
        public void Clear()
        {
            this.messages.Clear();
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header
        {
            get
            {
                return this.header;
            }

            set
            {
                this.header = value;
            }
        }

        /// <summary>
        /// Gets or sets the footer.
        /// </summary>
        /// <value>The footer.</value>
        public string Footer
        {
            get
            {
                return this.footer;
            }

            set
            {
                this.footer = value;
            }
        }

        /// <summary>
        /// Adds the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Add(string message)
        {
            this.messageCount++;
            this.messages.Add(message);
        }

        /// <summary>
        /// Appends the specified log to append.
        /// </summary>
        /// <param name="logToAppend">The log to append.</param>
        public void Append(MemoryLogger logToAppend)
        {
            this.messages.AddRange(logToAppend.messages);
        }

        /// <summary>
        /// Adds a line.
        /// </summary>
        public void AddEmptyLine()
        {
            this.messages.Add(String.Empty);
        }

        /// <summary>
        /// Adds the specified message with a time stamp
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddWithTime(string message)
        {
            this.messageCount++;
            this.messages.Add(MemoryLogger.TimeStamp(message));
        }

        /// <summary>
        /// Adds the time stamp
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

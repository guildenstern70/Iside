using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IsideLogic
{
    /// <summary>
    /// File Properties
    /// </summary>
    public class FileProperties
    {

        private FileInfo theFile;

        public FileProperties() { } // DO NOT TOUCH! CONSTRUCTOR NEEDED

        /// <summary>
        /// Constructor with file specification
        /// </summary>
        /// <param name="filename"></param>
        public FileProperties(string filename)
        {
            if (File.Exists(filename))
            {
                this.theFile = new FileInfo(filename);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this file is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return ((this.theFile.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this file is hidden.
        /// </summary>
        /// <value><c>true</c> if this instance is hidden; otherwise, <c>false</c>.</value>
        public bool IsHidden
        {
            get
            {
                return ((this.theFile.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this file is archive.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is archive; otherwise, <c>false</c>.
        /// </value>
        public bool IsArchive
        {
            get
            {
                return ((this.theFile.Attributes & FileAttributes.Archive) == FileAttributes.Archive);
            }
        }

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName
        {
            get
            {
                if (this.theFile != null)
                {
                    return this.theFile.Name;
                }

                return "";
            }

            set
            {
                if (File.Exists(value))
                {
                    this.theFile = new FileInfo(value);
                }
            }
        }

        /// <summary>
        /// Full Path
        /// </summary>
        public string FullPath
        {
            get
            {
                if (this.theFile != null)
                {
                    return this.theFile.FullName;
                }

                return "";
            }
        }

        /// <summary>
        /// File Size
        /// </summary>
        public string Size
        {
            get
            {
                if (this.theFile != null)
                {
                    string sLen = String.Format("{0:n0}", this.theFile.Length);
                    return sLen;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Last Modified
        /// </summary>
        public string LastModifiedDate
        {
            get
            {
                if (this.theFile != null)
                {
                    return FileProperties.DateToString(this.theFile.LastWriteTime);
                }
                return "";
            }
        }

        /// <summary>
        /// Creation Date
        /// </summary>
        public string CreationDate
        {
            get
            {
                if (this.theFile != null)
                {
                    return FileProperties.DateToString(this.theFile.CreationTime);
                }
                return "";
            }
        }

        /// <summary>
        /// Last Access Date
        /// </summary>
        public string LastAccessDate
        {
            get
            {
                if (this.theFile != null)
                {
                    return FileProperties.DateToString(this.theFile.LastAccessTime);
                }
                return "";
            }
        }


        private static string DateToString(DateTime dt)
        {
            IFormatProvider culture =
                new System.Globalization.CultureInfo("en-US", true);
            string[] engDate = dt.GetDateTimeFormats('g', culture);
            return engDate[0];
        }

    }
}

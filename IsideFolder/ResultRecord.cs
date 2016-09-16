/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Text;
using System.IO;

using LLCryptoLib;
using LLCryptoLib.Utils;

namespace IsideFolder
{
    /// <summary>
    /// ResultRecord class holds information about
    /// files being compared by IsideFolder
    /// </summary>
    internal struct ResultRecord
    {
        private FileInfo _fileLeft;
        private FileInfo _fileRight;
        private string _propertyLeft;
        private string _propertyRight;

        public ResultRecord(FileInfo left, FileInfo right, string propLeft, string propRight)
        {
            this._fileLeft = left;
            this._fileRight = right;
            this._propertyLeft = propLeft;
            this._propertyRight = propRight;
        }

        public ResultRecord(FileInfo left, string right, string propLeft, string propRight)
        {
            this._fileLeft = left;
            this._fileRight = new FileInfo(right); // dummy file.
            this._propertyLeft = propLeft;
            this._propertyRight = propRight;
        }

        public bool Equality
        {
            get
            {
                if (this._propertyLeft.Equals(this._propertyRight))
                {
                    return true;
                }

                return false;
            }
        }

        public FileInfo FileLeft
        {
            get
            {
                return this._fileLeft;
            }
        }

        public FileInfo FileRight
        {
            get
            {
                return this._fileRight;
            }
        }

        public string PropertyLeft
        {
            get
            {
                return this._propertyLeft;
            }
        }

        public string PropertyRight
        {
            get
            {
                return this._propertyRight;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ResultRecord a, ResultRecord b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ResultRecord a, ResultRecord b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Equality is true when all fields are equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ResultRecord))
            {
                return false;
            }

            ResultRecord rr = (ResultRecord)obj;
            bool eq = false;

            if (this._fileLeft.Equals(rr.FileLeft) && (this._fileRight.Equals(rr.FileRight)))
            {
                if (this._propertyLeft.Equals(rr.PropertyLeft) && (this._propertyRight.Equals(rr.PropertyRight)))
                {
                    eq = true;
                }
            }

            return eq;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return (Hexer.Text2Int(this._propertyLeft) + Hexer.Text2Int(this._propertyRight));
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Environment.NewLine);
            sb.Append(this._fileLeft.FullName);
            sb.Append(Environment.NewLine);
            if (this._fileRight.Exists)
            {
                sb.Append(this._fileRight.FullName);
            }
            else
            {
                sb.Append("> Compared with value stored in ");
                sb.Append(this._fileRight.Name);
            }
            sb.AppendLine();
            sb.AppendLine();

            sb.Append(this._propertyLeft);
            sb.Append(Environment.NewLine);
            if (!this.Equality)
            {
                sb.Append("<***>");
            }
            sb.Append(this._propertyRight);
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        /// <summary>
        /// Toes the CSV string.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <returns></returns>
        public string ToCSVString(string rootPath)
        {

            StringBuilder sb = new StringBuilder();
            string filename = this._fileLeft.FullName;

            string relativePath;
            try
            {
                relativePath = filename.Substring(rootPath.Length);
                System.Diagnostics.Debug.WriteLine("root: " + rootPath + ", filename: " + filename + ", relative: " + relativePath);
            }
            catch (ArgumentOutOfRangeException aox)
            {
                System.Diagnostics.Debug.WriteLine(aox.Message);
                relativePath = String.Empty;
            }
            sb.Append(relativePath);
            sb.Append(';');
            sb.Append(this._propertyLeft);
            sb.Append(';');
            if (!this.Equality)
            {
                sb.Append("<***>");
            }
            sb.Append(this._propertyRight);

            return sb.ToString();
        }

        /// <summary>
        /// Toes the HTML string.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <returns></returns>
        public string ToHTMLString(string rootPath)
        {
            StringBuilder sb = new StringBuilder();
            string filename = this._fileLeft.FullName;
            string relativePath;
            try
            {
                relativePath = filename.Substring(rootPath.Length);
                System.Diagnostics.Debug.WriteLine("root: " + rootPath + ", relative: " + relativePath);
            }
            catch (ArgumentOutOfRangeException aox)
            {
                System.Diagnostics.Debug.WriteLine(aox.Message);
                relativePath = String.Empty;
            }
            sb.Append("<td>");
            sb.Append(relativePath);
            sb.Append("</td><td>");
            sb.Append(this._propertyLeft);
            sb.Append("</td><td>");
            if (!this.Equality)
            {
                sb.Append(@"<font color=""red"">");
            }
            sb.Append(this._propertyRight);
            if (!this.Equality)
            {
                sb.Append(@"</font>");
            }
            sb.Append("</td>");
            return sb.ToString();
        }

    }
}

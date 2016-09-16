/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using LLCryptoLib.Utils;
using System;
using System.IO;

namespace IsideFolder
{
    /// <summary>
    /// Comparison.
    /// A comparison is the description of a comparison of
    /// folders used in Iside Folder
    /// </summary>
    public class Comparison
    {
        protected const int MAX_BRIEF_STRING_LEN = 20;
        protected DirectoryInfo leftPathInfo;
        protected FileSystemInfo rightPathInfo; // can be a file (.isrl) or a folder
        protected string filename;
        protected string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Comparison"/> class.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        public Comparison(string left, string right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            if (left == null)
            {
                throw new ArgumentNullException("left");
            }

            // Left Directory
            this.leftPathInfo = new DirectoryInfo(left.Trim());

            // Right Directory or Results List
            string rightItem = right.Trim();
            if (rightItem.EndsWith(".isrl"))
            {
                this.rightPathInfo = new FileInfo(rightItem);
            }
            else
            {
                this.rightPathInfo = new DirectoryInfo(rightItem);
            }

            this.name = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Comparison"/> class.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        public Comparison(DirectoryInfo left, FileSystemInfo right)
        {
            this.leftPathInfo = left;
            this.rightPathInfo = right;
            this.name = "";
        }

        /// <summary>
        /// Gets the comparison name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets the left path.
        /// </summary>
        /// <value>The left path.</value>
        public DirectoryInfo LeftPath
        {
            get
            {
                return this.leftPathInfo;
            }
        }

        /// <summary>
        /// Gets the right path.
        /// </summary>
        /// <value>The right path.</value>
        public FileSystemInfo RightPath
        {
            get
            {
                return this.rightPathInfo;
            }
        }

        /// <summary>
        /// Toes the brief string.
        /// </summary>
        /// <returns></returns>
        public string ToBriefString()
        {
            string s1 = this.leftPathInfo.FullName;
            string s2 = this.rightPathInfo.FullName;

            if (s1.Length > MAX_BRIEF_STRING_LEN)
            {
                s1 = "[...]" + s1.Substring(s1.Length - MAX_BRIEF_STRING_LEN);
            }

            if (s2.Length > MAX_BRIEF_STRING_LEN)
            {
                s2 = "[...]" + s2.Substring(s2.Length - MAX_BRIEF_STRING_LEN);
            }

            return s1 + " <-> " + s2;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return this.leftPathInfo.FullName + " <-> " + this.rightPathInfo.FullName;
        }

        /// <summary>
        /// Equality is true when all fields are equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            // safe because of the GetType check
            Comparison c = (Comparison)obj;
            bool eq = false;

            if (this.leftPathInfo.Equals(c.LeftPath))
            {
                if (this.rightPathInfo.Equals(c.RightPath))
                {
                    eq = true;
                }
            }

            return eq;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return (Hexer.Text2Int(this.rightPathInfo.ToString() + this.leftPathInfo.ToString()));
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            if (this.filename == null)
            {
                return false;
            }

            return this.Save(this.filename);
        }

        /// <summary>
        /// Saves the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public bool Save(string path)
        {
            this.filename = path;
            AxsUtils.FileManager fm = AxsUtils.FileManager.Reference;
            this.name = Path.GetFileNameWithoutExtension(path);
            return fm.SaveFile(path, this.ToString());
        }

        /// <summary>
        /// Saves the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public bool Save(Stream stream)
        {
            bool success = true;

            try
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine(this.ToString());
                }
            }
            catch (IOException ioex)
            {
                System.Diagnostics.Debug.WriteLine(ioex.Message);
                System.Diagnostics.Debug.WriteLine(ioex.StackTrace);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Froms the string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static Comparison FromString(string s)
        {

            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            Comparison c = null;
            int sparti = s.IndexOf(" <-> ");

            if (sparti != -1)
            {
                DirectoryInfo d1;
                FileSystemInfo d2;

                string di1 = s.Substring(0, sparti);
                string di2 = s.Substring(sparti + 5);

                d1 = new DirectoryInfo(di1);

                if (di2.EndsWith(".isrl"))
                {
                    d2 = new FileInfo(di2);
                }
                else
                {
                    d2 = new DirectoryInfo(di2);
                }

                c = new Comparison(d1, d2);
            }

            return c;
        }

        /// <summary>
        /// Froms the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static Comparison FromFile(string path)
        {
            Comparison c = null;

            try
            {
                AxsUtils.FileManager fm = AxsUtils.FileManager.Reference;
                string[] contents = fm.ReadFileLines(path);
                if (contents.Length > 0)
                {
                    c = Comparison.FromString(contents[0]);
                    c.Name = Path.GetFileNameWithoutExtension(path);
                }
            }
            catch (NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("File not found: " + path);
            }

            return c;
        }

    }
}

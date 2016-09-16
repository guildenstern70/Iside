using System;
using System.IO;

using LLCryptoLib.Hash;
using System.Text;
using System.Collections.Generic;

namespace IsideFolder
{
    internal class ComparerFunctor
    {
        private ComparisonMethod pC;
        private IHash hash;

        private string left;
        private string right;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparerFunctor"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="hash">The hash.</param>
        public ComparerFunctor(ComparisonMethod c, AvailableHash hash)
        {
            this.Init(c);
            this.hash.SetAlgorithm(hash);
        }

        /// <summary>
        /// Gets the left property.
        /// </summary>
        /// <value>The left property.</value>
        public string LeftProperty
        {
            get
            {
                return this.left;
            }
        }

        /// <summary>
        /// Gets the right property.
        /// </summary>
        /// <value>The right property.</value>
        public string RightProperty
        {
            get
            {
                return this.right;
            }
        }

        /// <summary>
        /// Compares the the items based on Comparison method
        /// </summary>
        /// <param name="f1">The f1.</param>
        /// <param name="f2">The f2.</param>
        /// <returns></returns>
        public bool CompareOn(FileInfo f1, FileInfo f2)
        {
            bool condition = false;

            try
            {
                switch (this.pC)
                {
                    case ComparisonMethod.NAME:
                        this.left = ComparerFunctor.FindCommonPath('\\', f1.FullName, f2.FullName);
                        this.right = ComparerFunctor.FindCommonPath('\\', f2.FullName, f1.FullName);
                        condition = (this.left == this.right);
                        break;

                    case ComparisonMethod.SIZE:
                        this.left = String.Format("{0} bytes", f1.Length);
                        this.right = String.Format("{0} bytes", f2.Length);
                        condition = (f1.Length == f2.Length);
                        break;

                    case ComparisonMethod.HASH:
                        this.left = hash.ComputeHashFile(f1.FullName);
                        this.right = hash.ComputeHashFile(f2.FullName);
                        condition = (this.left == this.right);
                        break;
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine("Comparer Functor Exception: " + Environment.NewLine + exc.ToString());
            }

            return condition;
        }


        /// <summary>
        /// Compares the on.
        /// </summary>
        /// <param name="f1">The f1.</param>
        /// <param name="rl">The rl.</param>
        /// <returns></returns>
        public bool CompareOn(FileInfo f1, string rl)
        {
            bool condition = false;

            switch (this.pC)
            {
                case ComparisonMethod.NAME:
                    this.left = ComparerFunctor.FindCommonPath('\\', f1.FullName, rl);
                    this.right = ComparerFunctor.FindCommonPath('\\', rl, f1.FullName);;
                    break;

                case ComparisonMethod.SIZE:
                    this.left = String.Format("{0} bytes", f1.Length);
                    this.right = rl;
                    break;

                case ComparisonMethod.HASH:
                    this.left = hash.ComputeHashFile(f1.FullName);
                    this.right = rl;
                    break;
            }

            condition = (this.left.Equals(this.right));

            return condition;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string cond = "unknown";

            switch (this.pC)
            {
                case ComparisonMethod.NAME:
                    cond = "name";
                    break;

                case ComparisonMethod.SIZE:
                    cond = "size";
                    break;

                case ComparisonMethod.HASH:
                    cond = "hash";
                    break;
            }

            return cond;
        }

        private void Init(ComparisonMethod c)
        {
            this.left = "Unknown";
            this.right = "Unknown";
            this.pC = c;
            this.hash = new Hash();
        }

        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Finds the common path between two paths
        /// </summary>
        /// <param name="separator">The path separator char.</param>
        /// <param name="leftPath">The left path.</param>
        /// <param name="rightPath">The right path.</param>
        /// <returns>A string with the common path between the two paths, including the filename</returns>
        public static string FindCommonPath(char separator, string leftPath, string rightPath)
        {
            List<string> commonParts = new List<string>();

            string[] leftParts = leftPath.Split(separator);
            string[] rightParts = rightPath.Split(separator);
            string comparingPath;

            int indexRight = rightParts.Length - 1;

            for (int j = leftParts.Length - 1; j >= 0; j--)
            {
                string part = leftParts[j];
                comparingPath = rightParts[indexRight];

                if (part.EndsWith(":") || (comparingPath.EndsWith(":")))
                {
                    continue;
                }

                if ((part == comparingPath) || (indexRight == rightParts.Length - 1))
                {
                    commonParts.Add(part);
                }
                else
                {
                    break;
                }

                indexRight--;

                if (indexRight < 0) break;
            }

            string retstr = "";

            for (int k = commonParts.Count - 1; k >= 0; k--)
            {
                retstr += commonParts[k];
                retstr += separator;
            }

            return retstr.Substring(0, retstr.Length - 1);

        }

    }

}

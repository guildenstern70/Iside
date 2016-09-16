using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LLCryptoLib.Utils
{
    /// <summary>
    ///     DirectoryElements contain a directory description,
    ///     with all its files and the number of Directories
    ///     it contains. The directory is recursively scanned
    ///     to know about its contents and the contents of every
    ///     subfolder in it.
    /// </summary>
    public class DirectoryElements
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DirectoryElements" /> class.
        /// </summary>
        public DirectoryElements()
        {
            this.IncludeHiddenFiles = true;
            this.IncludeSystemFiles = true;
            this.IncludeArchiveFiles = true;

            this.Files = new List<FileInfo>();
            this.Subdirectories = new List<DirectoryInfo>();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [include hidden files].
        /// </summary>
        /// <value><c>true</c> if [include hidden files]; otherwise, <c>false</c>.</value>
        public bool IncludeHiddenFiles { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [include system files].
        /// </summary>
        /// <value><c>true</c> if [include system files]; otherwise, <c>false</c>.</value>
        public bool IncludeSystemFiles { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [include archive files].
        /// </summary>
        /// <value><c>true</c> if [include archive files]; otherwise, <c>false</c>.</value>
        public bool IncludeArchiveFiles { get; set; }


        /// <summary>
        ///     Gets a value indicating whether this instance is with sub directories.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is with sub dirs; otherwise, <c>false</c>.
        /// </value>
        public bool IsWithSubDirs { get; private set; }

        /// <summary>
        ///     Initial Directory
        /// </summary>
        public DirectoryInfo Directory { get; private set; }

        /// <summary>
        ///     A list (Collection) of files into the directory
        /// </summary>
        public List<FileInfo> Files { get; }


        /// <summary>
        ///     Gets all subdirectories inside original one, without original one.
        /// </summary>
        /// <value>The subdirectories inside this directory</value>
        public List<DirectoryInfo> Subdirectories { get; }

        /// <summary>
        ///     Number of files into the directory and all of its subdirectories
        /// </summary>
        public int NrOfFiles
        {
            get { return this.Files.Count; }
        }

        /// <summary>
        ///     Number of subdirectories
        /// </summary>
        public int NrOfSubdirectories { get; set; }

        /// <summary>
        ///     Scans the specified directory (and subdirectories, if required)
        /// </summary>
        /// <param name="di">The directory to be scanned</param>
        /// <param name="includeSubDirs">if set to <c>true</c> include subdirectories of this directory.</param>
        public void Scan(DirectoryInfo di, bool includeSubDirs)
        {
            this.Directory = di;
            this.IsWithSubDirs = includeSubDirs;

            this.BuildItemsList();
        }

        /// <summary>
        ///     Removes a file from the current directory elements list.
        /// </summary>
        /// <param name="fileToRemove">The file to remove.</param>
        /// <returns>True, if the element was correctly removed.</returns>
        public bool RemoveElement(FileInfo fileToRemove)
        {
            bool removed = false;

            int fileIndex = -1;
            int count = 0;
            foreach (FileInfo fi in this.Files)
            {
                if (fi.Name.Equals(fileToRemove.Name))
                {
                    fileIndex = count;
                    break;
                }

                count++;
            }

            if (fileIndex > 0)
            {
                this.Files.RemoveAt(fileIndex);
                removed = true;
            }

            return removed;
        }

        /// <summary>
        ///     Build a list of all files within a folder.
        /// </summary>
        /// <param name="di">The directory to be scanned.</param>
        /// <returns>True if the operations succeeds</returns>
        private bool BuildList(DirectoryInfo di)
        {
            bool success = true;

            if (!di.Exists)
            {
                return false;
            }

            try
            {
                foreach (FileInfo fi in di.GetFiles())
                {
                    bool isHidden = (fi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                    bool isArchive = (fi.Attributes & FileAttributes.Archive) == FileAttributes.Archive;
                    bool isSystem = (fi.Attributes & FileAttributes.System) == FileAttributes.System;

                    if (!this.IncludeHiddenFiles)
                    {
                        if (isHidden)
                        {
                            System.Diagnostics.Debug.WriteLine(fi.Name +
                                                               " is hidden, and we do not include hidden files. Skipping.");
                            continue;
                        }
                    }

                    if (!this.IncludeArchiveFiles)
                    {
                        if (isArchive)
                        {
                            System.Diagnostics.Debug.WriteLine(fi.Name +
                                                               " is archive, and we do not include archive files. Skipping.");
                            continue;
                        }
                    }

                    if (!this.IncludeSystemFiles)
                    {
                        if (isSystem)
                        {
                            System.Diagnostics.Debug.WriteLine(fi.Name +
                                                               " is system, and we do not include system files. Skipping.");
                            continue;
                        }
                    }

                    this.Files.Add(fi);
                }

                if (this.IsWithSubDirs)
                {
                    DirectoryInfo[] subfolders = di.GetDirectories();
                    foreach (DirectoryInfo subfolder in subfolders)
                    {
                        this.NrOfSubdirectories++;
                        this.Subdirectories.Add(subfolder);
                        if (this.BuildList(subfolder) == false)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    this.NrOfSubdirectories = 0;
                }
            }
            catch (IOException exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Source);
                System.Diagnostics.Debug.WriteLine(exc.Message);
                System.Diagnostics.Debug.WriteLine(exc.StackTrace);
                success = false;
            }
            catch (UnauthorizedAccessException unax)
            {
                System.Diagnostics.Debug.WriteLine(unax.Source);
                System.Diagnostics.Debug.WriteLine(unax.Message);
                System.Diagnostics.Debug.WriteLine(unax.StackTrace);
                success = false;
            }

            return success;
        }

        /// <summary>
        ///     Initializes the specified di.
        /// </summary>
        /// <exception cref="LLCryptoLibException">Invalid directory elements list.</exception>
        private void BuildItemsList()
        {
            if (!this.BuildList(this.Directory))
            {
                throw new LLCryptoLibException("Invalid Directory Elements list");
            }
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String"></see> that represents a description of the current directory
        ///     <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        ///     Ie: "Contains 1290 files and 12 subdirectories";
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Contains ");
            sb.Append(this.NrOfFiles);
            sb.Append(" files");
            if (this.NrOfSubdirectories > 0)
            {
                sb.Append(" and ");
                sb.Append(this.NrOfSubdirectories);
                sb.Append(" subdirectories.");
            }
            else
            {
                sb.Append(".");
            }
            return sb.ToString();
        }
    }
}
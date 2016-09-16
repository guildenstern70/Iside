/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved 
 * 
 * AxsUtils.DirectoryElements.cs
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AxsUtils
{
	/// <summary>
	/// DirectoryElements contain a directory description
	/// with all its files and the number of Directories
	/// it contains
	/// </summary>
	public class DirectoryElements
	{
		private DirectoryInfo folder;
        private List<FileInfo> fileList;   // Array of FileInfo objects
        private List<DirectoryInfo> directoryList; // Array of Subdirectories 

		private int subDirectories;
        private bool incSubdirs;

        private bool includeHidden;
        private bool includeSystem;
        private bool includeArchive;

        private ulong totalSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DirectoryElements"/> class.
        /// </summary>
        public DirectoryElements()
        {
            this.includeHidden = true;
            this.includeSystem = true;
            this.includeArchive = true;

            this.fileList = new List<FileInfo>();
            this.directoryList = new List<DirectoryInfo>();

            this.totalSize = 0;
        }

        /// <summary>
        /// Scans the specified directory (and subdirectories, if required)
        /// </summary>
        /// <param name="di">The directory to be scanned</param>
        /// <param name="includeSubDirs">if set to <c>true</c> include subdirectories of this directory.</param>
        /// <exception cref="AxsException">Invalid directory elements list.</exception>
        public void Scan(DirectoryInfo di, bool includeSubDirs)
        {
            this.folder = di;
            this.incSubdirs = includeSubDirs;

            this.BuildItemsList();
        }

        /// <summary>
        /// Gets or sets a value indicating whether [include hidden files].
        /// </summary>
        /// <value><c>true</c> if [include hidden files]; otherwise, <c>false</c>.</value>
        public bool IncludeHiddenFiles
        {
            get { return includeHidden; }
            set { includeHidden = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [include system files].
        /// </summary>
        /// <value><c>true</c> if [include system files]; otherwise, <c>false</c>.</value>
        public bool IncludeSystemFiles
        {
            get { return includeSystem; }
            set { includeSystem = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [include archive files].
        /// </summary>
        /// <value><c>true</c> if [include archive files]; otherwise, <c>false</c>.</value>
        public bool IncludeArchiveFiles
        {
            get { return includeArchive; }
            set { includeArchive = value; }
        }


        /// <summary>
        /// Gets a value indicating whether this instance is with sub directories.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is with sub dirs; otherwise, <c>false</c>.
        /// </value>
        public bool IsWithSubDirs
        {
            get
            {
                return this.incSubdirs;
            }
        }

        /// <summary>
        /// Initial Directory
        /// </summary>
		public DirectoryInfo Directory
		{
			get
			{
				return this.folder;
			}
		}

        /// <summary>
        /// A list (Collection) of files into the directory
        /// </summary>
        public List<FileInfo> Files
		{
			get
			{
				return this.fileList;
			}
		}


        /// <summary>
        /// Gets all subdirectories inside original one, without original one.
        /// </summary>
        /// <value>The subdirectories inside this directory</value>
        public List<DirectoryInfo> Subdirectories
        {
            get
            {
                return this.directoryList;
            }
        }

        /// <summary>
        /// Number of files into the directory and all of its subdirectories
        /// </summary>
		public int NrOfFiles
		{
			get
			{
				return this.fileList.Count;
			}
		}

        /// <summary>
        /// Gets the total size of the files in DirectoryElements.
        /// </summary>
        /// <remarks>The Scan method must be called before</remarks>
        /// <value>The total size of the files in DirectoryElements.</value>
        [CLSCompliant(false)]
        public ulong OverallFileSize
        {
            get
            {
                return this.totalSize;
            }
        }

        /// <summary>
        /// Number of subdirectories
        /// </summary>
		public int NrOfSubdirectories
		{
			get
			{
				return this.subDirectories;
			}

			set
			{
				this.subDirectories = value;
			}
		}

        /// <summary>
        /// Removes a file from the current directory elements list.
        /// </summary>
        /// <param name="fileToRemove">The file to remove.</param>
        /// <returns>True, if the element was correctly removed.</returns>
        public bool RemoveElement(FileInfo fileToRemove)
        {
            bool removed = false;

            int fileIndex = -1;
            int count = 0;
            foreach (FileInfo fi in this.fileList)
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
                this.fileList.RemoveAt(fileIndex);
                removed = true;
            }

            return removed;
        }

		/// <summary>
		/// Build a list of all files within a folder.
		/// </summary>
		/// <param name="folder">Root folder to analyze</param>
		/// <param name="fileList">Array of FileInfo to populate</param>
		/// <param name="dircount">Number of directories inside the folder</param>
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
                    bool isHidden = ((fi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden);
                    bool isArchive = ((fi.Attributes & FileAttributes.Archive) == FileAttributes.Archive);
                    bool isSystem = ((fi.Attributes & FileAttributes.System) == FileAttributes.System);

                    if (!this.includeHidden)
                    {
                        if (isHidden)
                        {
                            System.Diagnostics.Debug.WriteLine(fi.Name + " is hidden, and we do not include hidden files. Skipping.");
                            continue;
                        }
                    }

                    if (!this.includeArchive)
                    {
                        if (isArchive)
                        {
                            System.Diagnostics.Debug.WriteLine(fi.Name + " is archive, and we do not include archive files. Skipping.");
                            continue;
                        }
                    }

                    if (!this.includeSystem)
                    {
                        if (isSystem)
                        {
                            System.Diagnostics.Debug.WriteLine(fi.Name + " is system, and we do not include system files. Skipping.");
                            continue;
                        }
                    }

                    this.fileList.Add(fi);
                    this.totalSize += (ulong)fi.Length;
                }

                if (this.incSubdirs)
                {
                    DirectoryInfo[] subfolders = di.GetDirectories();
                    foreach (DirectoryInfo subfolder in subfolders)
                    {
                        this.subDirectories++;
                        this.directoryList.Add(subfolder);
                        if (this.BuildList(subfolder) == false)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    this.subDirectories = 0;
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
        /// Initializes the specified di.
        /// </summary>
        /// <param name="di">The di.</param>
        /// <param name="includeSubDirs">if set to <c>true</c> [include sub dirs].</param>
        /// <exception cref="AxsException">Invalid directory elements list.</exception>
        private void BuildItemsList()
        {
            if (!this.BuildList(this.folder))
            {
                throw new AxsUtils.AxsException("Files in the specified directory are in use by another process.");
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents a description of the current directory <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// Ie: "Contains 1290 files and 12 subdirectories";
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

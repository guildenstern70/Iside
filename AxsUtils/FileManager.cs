/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved 
 * 
 * AxsUtils.FileManager.cs
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AxsUtils
{
	/// <summary>
	/// File Manager. 
	/// </summary>
	public class FileManager
	{

		private static FileManager fRef;
		private string errMsg;

		private FileManager()
		{
			errMsg = String.Empty;
		}

		/// <summary>
		/// Error Message. Every time it's called, reset ErrorMessage to "".
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				string err = this.errMsg;
                this.errMsg = String.Empty;
				return err;
			}
		}

        /// <summary>
        /// Check if filename is valid (does not contain invalid characters)
        /// </summary>
        /// <param name="inputFileName">Name of the input file.</param>
        /// <returns>
        /// 	<c>true</c> if [is filename valid] [the specified input file name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFilenameValid(string inputFileName)
        {
            Match m = Regex.Match(inputFileName, @"[\\\/\:\*\?\" + Convert.ToChar(34) + @"\<\>\|]");
            return !(m.Success);
        }

        /// <summary>
        /// Write a binary file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>True if the file was correctly written</returns>
        /// <remarks>In case this method return false, check <see cref="ErrorMessage"/></remarks>
		public bool WriteBinaryFile(string filePath, byte[] contents)
		{
            bool okok = true;
            FileStream fs = null;

            try
            {
                fs = new FileStream(filePath, FileMode.CreateNew);
                {
                    using (BinaryWriter w = new BinaryWriter(fs))
                    {
                        fs = null;
                        w.Write(contents);
                        w.Flush();
                    }
                }
            }
            catch (IOException e)
            {
                this.errMsg = e.Message;
                okok = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return okok;
        }



		/// <summary>
		/// Returns true if the given file is Read only
		/// </summary>
		/// <param name="fullName">Full file path</param>
		/// <returns>True if the given file is Read only</returns>
		public static bool IsReadOnly(string fullName)
		{
			bool isReadOnly = false;
			FileInfo filePath = new FileInfo(fullName);

			if (filePath.Exists)
			{
				if ((filePath.Attributes & FileAttributes.ReadOnly) != 0)
				{
					isReadOnly = true;
				}
			}

			return isReadOnly;
		}

        /// <summary>
        /// Set the attribute read only of a file
        /// </summary>
        /// <param name="fullName">Full path of file</param>
        /// <param name="readOnly">If true, the file will be set readonly. Else will be set writable</param>
        /// <remarks>In case this method return false, check <see cref="ErrorMessage"/></remarks>
        /// <returns>True, if the attribute was set</returns>
		public bool SetReadOnly(string fullName, bool readOnly)
		{
            bool success = true;
			FileInfo filePath = new FileInfo(fullName);

			try
			{
				if (filePath.Exists)
				{
					FileAttributes attr;
					if (readOnly)
					{
						attr = (FileAttributes)(filePath.Attributes | FileAttributes.ReadOnly);
					}
					else
					{
						attr = (FileAttributes)(filePath.Attributes - FileAttributes.ReadOnly);
					}
					File.SetAttributes(filePath.FullName, attr);
				}

			}
			catch (IOException e)
			{
				System.Diagnostics.Debug.WriteLine(e.Source);
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				this.errMsg = "Cannot set read only on "+filePath;
                success = false;
			}

            return success;
		}

        /// Set the attribute read only of a file
        /// </summary>
        /// <param name="fullName">Full path of file</param>
        /// <param name="readOnly">If true, the file will be set hidden. Else will be set clear</param>
        /// <remarks>In case this method return false, check <see cref="ErrorMessage"/></remarks>
        /// <returns>True, if the attribute was set</returns>
        public bool SetHidden(string fullName, bool hidden)
        {
            bool success = true;
            FileInfo filePath = new FileInfo(fullName);

            try
            {
                if (filePath.Exists)
                {
                    FileAttributes attr;
                    if (hidden)
                    {
                        attr = (FileAttributes)(filePath.Attributes | FileAttributes.Hidden);
                    }
                    else
                    {
                        attr = (FileAttributes)(filePath.Attributes - FileAttributes.Hidden);
                    }
                    File.SetAttributes(filePath.FullName, attr);
                }

            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Source);
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                this.errMsg = "Cannot set hidden on " + filePath;
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Read a binary file. This method reads a small binary file.
        /// Since it does not implement a cache, it is not suitable for large files.
        /// </summary>
        /// <param name="filePath">Absolute Path of file to read from.</param>
        /// <example>byte[] myFile = myFileObj.ReadBinaryFile(@"C:\log.txt");</example>
        /// <remarks>In case this method return false, check <see cref="ErrorMessage"/></remarks>
        /// <returns>Byte contents of the file.</returns>
        /// <exception cref="LLCryptoLibException">Input file too long</exception>
        /// <exception cref="ArgumentNullException" />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public byte[] ReadBinaryFile(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            byte[] file = null;

            FileStream fs = null;
            BufferedStream bufs = null;
            BinaryReader bs = null;

            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                bufs = new BufferedStream(fs);
                bs = new BinaryReader(bufs);
                long lFile = bs.BaseStream.Length;

                if (lFile > Int32.MaxValue) // if file > 2GB
                {
                    bs.Close();
                    throw new AxsException("Input file too long");
                }

                file = bs.ReadBytes((int)lFile);
                this.errMsg = "";
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Source);
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                this.errMsg = "Cannot load " + filePath;
                Console.WriteLine(this.errMsg);
            }
            finally
            {
                if (bs != null)
                    bs.Close();
                if (bufs != null)
                    bufs.Close();
                if (fs != null)
                    fs.Close();
            }

            return file;
        }

        /// <summary>
        /// Reads a text file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <remarks>In case this method return 'null', check <see cref="ErrorMessage"/>.</remarks>
        /// <returns>The content of the read file as a string.</returns>
        public string ReadTextFile(string filePath)
        {
            string allTheFile = null;
            try
            {
                using (TextReader tr = File.OpenText(filePath))
                {
                    allTheFile = tr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Source);
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                this.errMsg = "Cannot load " + filePath;
                System.Diagnostics.Debug.WriteLine(this.errMsg);
            }

            return allTheFile;
        }

        /// <summary>
        /// Reads a text file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="enc">The text encoding.</param>
        /// <returns>
        /// The content of the read file as a string.
        /// </returns>
        /// <remarks>In case this method return 'null', check <see cref="ErrorMessage"/>.</remarks>
		public string ReadTextFile(string filePath, Encoding enc)
		{
            if (enc == null)
            {
                enc = Encoding.UTF8;
            }

			FileStream fs = null;
			string retStr = null;

			try
			{
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 8192); //we use the default buffer
                StringBuilder result = new StringBuilder(8192);
                byte[] buffer = new byte[8192];
                int count = fs.Read(buffer, 0, 8192); //reading a block from the file

                while (count != 0) //stop when Read returns 0
                {
                    result.Append(enc.GetString(buffer));
                    count = fs.Read(buffer, 0, 8192);
                }

                retStr = result.ToString();

			}
			catch (IOException e)
			{
				System.Diagnostics.Debug.WriteLine(e.Source);
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				this.errMsg = "Cannot load "+filePath;
				Console.WriteLine(this.errMsg);
			}
			finally
			{
				if (fs!=null) fs.Close();
			}
			
			return retStr;
		}

		/// <summary>
		/// Rename a file
		/// </summary>
		/// <param name="pathSource">Complete path of the file to be renamed</param>
		/// <param name="newname">New name of the file - without path</param>
		/// <returns>True if operation was successful</returns>
		public bool RenameFile(string pathSource, string newname)
		{
			bool success = true;

			try
			{
				string fpath = Path.GetDirectoryName(pathSource);
				File.Move(pathSource,fpath+Path.DirectorySeparatorChar+newname);
			}
			catch (IOException exc)
			{
				success = false;
				this.errMsg = exc.Message;
			}

			return success;
		}

		/// <summary>
		/// Reads a Text File
		/// </summary>
		/// <param name="absolutePath">Absolute Path of file to read from</param>
		/// <returns>Text Contents of the file</returns>
		public string ReadFile(string absolutePath)
		{
			return this.ReadFile(absolutePath, Encoding.Default);
		}

		/// <summary>
		/// Reads a Text File
		/// </summary>
		/// <param name="absolutePath">Absolute Path of file to read from</param>
		/// <param name="enc">Encoding of the text file. Use Encoding static members.</param>
		/// <returns>Text Contents of the file</returns>
		public string ReadFile(string absolutePath, Encoding enc)
		{
			StreamReader objReader=null;
			StringBuilder sb = new StringBuilder();

			try
			{
				objReader = new StreamReader(absolutePath,enc,false,1024);
				sb.Append(objReader.ReadToEnd());
				errMsg="";
			}
			catch (IOException e)
			{
				System.Diagnostics.Debug.WriteLine(e.Source);
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				this.errMsg = "Cannot load "+absolutePath;
				Console.WriteLine(this.errMsg);
			}
			finally
			{
				if (objReader!=null)
				{
					objReader.Close();
				}
			}

			return sb.ToString();
		}

        /// <summary>
        /// Get the size of a file
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public long FileSize(string fileName)
        {
            long size = 0;

            try
            {
                FileInfo fi = new FileInfo(fileName);
                size = fi.Length;
            }
            catch (IOException)
            {
                size = -1;
            }

            return size;
        }

		/// <summary>
		/// Returns the number of lines inside a text file
		/// </summary>
		/// <param name="path"></param>
		/// <returns>The number of lines inside a text file</returns>
		public int FileLines(string absolutePath)
		{
			StreamReader objReader=null;
			int lines = 0;

			try
			{
				objReader = new StreamReader(absolutePath);

				// Determine lines number
				while (objReader.ReadLine() != null) 
				{
					++lines;
				}

			}
			catch (IOException e)
			{
				System.Diagnostics.Debug.WriteLine(e.Source);
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				this.errMsg = "Cannot read "+absolutePath;
				Console.WriteLine(this.errMsg);
			}
			finally
			{
				if (objReader!=null)
				{
					objReader.Close();
				}
			}

			return lines;

		}

		/// <summary>
		/// Reads a Text File Line by Line. Return an array of strings,
		/// using the default encoding.
		/// </summary>
		/// <param name="absolutePath">Absolute Path of file to read from</param>
		/// <returns>Text Contents of the file (one string per line)</returns>
		public string[] ReadFileLines(string absolutePath)
		{
			return (this.ReadFileLines(absolutePath, Encoding.Default));
		}


		/// <summary>
		/// Reads a Text File Line by Line. Return an array of strings.
		/// </summary>
		/// <param name="absolutePath">Absolute Path of file to read from</param>
		/// <param name="enc">Encoding of the text file. Use Encoding static members.</param>
		/// <returns>Text Contents of the file (one string per line)</returns>
		public string[] ReadFileLines(string absolutePath, Encoding enc)
		{
			StreamReader objReader=null;
			string[] contents = null;

			try
			{
				int lines = this.FileLines(absolutePath);

				if (lines>0)
				{
					objReader = new StreamReader(absolutePath,enc,false,1024);
					contents = new string[lines];

					for (int j=0; j<lines; j++) 
					{
                        contents[j] = objReader.ReadLine();
					}
				}

			}
			catch (IOException e)
			{
				contents = null;
				System.Diagnostics.Debug.WriteLine(e.Source);
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
				this.errMsg = "Cannot read "+absolutePath;
				Console.WriteLine(this.errMsg);
			}
			finally
			{
				if (objReader!=null)
				{
					objReader.Close();
				}
			}

			return contents;
		}

		/// <summary>
		/// Delete file
		/// </summary>
		/// <param name="absolutePath">Absolute path of file to delete</param>
		/// <returns>True if file was actually deleted</returns>
		public bool DeleteFile(string absolutePath)
		{
			this.errMsg="";

			try
			{
				File.Delete(absolutePath);
			}
			catch (IOException ioex)
			{
				this.errMsg="Can't delete "+absolutePath+": "+ioex.Message;
				return false;
			}

			if (File.Exists(absolutePath))
			{
				this.errMsg="Can't delete : "+absolutePath;
				return false;
			}
			return true;
		}


		/// <summary>
		/// Save File
		/// </summary>
		/// <param name="absolutePath">Absolute path of file to save</param>
		/// <param name="contents">String contents</param>
		/// <returns>True, if file was correctly saved. If false, read ErrorMessage property to know what happened</returns>
		public bool SaveFile(string absolutePath, string contents)
		{
			return this.SaveFile(absolutePath,contents,false);
		}


		/// <summary>
		/// Save File as UTF-8 text file
		/// </summary>
		/// <param name="absolutePath">Absolute path of file to sav</param>
		/// <param name="contents">String contents</param>
		/// <param name="append">If true, contents will be append to absolutePath</param>
		/// <returns>True, if file was correctly saved. If false, read ErrorMessage property to know what happened</returns>
		public bool SaveFile(string absolutePath, string contents, bool append)
		{
			bool okok = false;

            if (absolutePath == null)
            {
                throw new ArgumentNullException("absolutePath");
            }

            if (absolutePath.Length == 0)
            {
                throw new AxsException("Trying SaveFile with an empty filename");
            }

            StreamWriter fSave = null;

            try
            {
                
                if (append)
                {
                    fSave = File.AppendText(absolutePath);
                }
                else
                {
                    fSave = File.CreateText(absolutePath);
                }

                fSave.WriteLine(contents);
                okok = true;
                this.errMsg = String.Empty;

            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Source);
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                this.errMsg = "Cannot save " + absolutePath + ":" + e.Message;
                Console.WriteLine(e.Message);
                okok = false;
            }
            finally
            {
                if (fSave != null)
                {
                    fSave.Flush();
                    fSave.Close();
                }
            }
        
			return okok;
		}

		/// <summary>
		/// File Existance
		/// </summary>
		/// <param name="absolutePath"></param>
		/// <returns></returns>
		public bool ExistFile(string absolutePath)
		{
			errMsg="";
			return File.Exists(absolutePath);
		}

        /// <summary>
        /// Copy entire folder recursively
        /// </summary>
        /// <param name="sourceFolderPath">The folder to be copied.</param>
        /// <param name="destinationFolderPath">The folder to where the folder to be copied will be copied.</param>
        /// <param name="overwrite">Overwrite existing files and folders that exist.</param>
        public bool CopyFolder(string sourceFolderPath, string destinationFolderPath, bool overwrite)
        {
            if (Directory.Exists(sourceFolderPath))
            {

                if (Directory.Exists(destinationFolderPath + "\\" + (new DirectoryInfo(sourceFolderPath).Name)) && (!overwrite))
                {
                    this.errMsg = "Folder " + destinationFolderPath + " already exists.";
                    return false;
                }
                else if (!Directory.Exists(destinationFolderPath + "\\" + (new DirectoryInfo(sourceFolderPath).Name)))
                {
                    Directory.CreateDirectory(destinationFolderPath + "\\" + (new DirectoryInfo(sourceFolderPath).Name));
                }

                //Copy all the files
                FileInfo[] fiA = new DirectoryInfo(sourceFolderPath).GetFiles();

                foreach (FileInfo fi in fiA)
                {
                    string newFileName = destinationFolderPath + "\\" + (new DirectoryInfo(sourceFolderPath).Name) + "\\" + fi.Name;
                    if (File.Exists(newFileName))
                    {
                        File.Delete(newFileName);
                    }
                    fi.CopyTo(newFileName);
                }

                //Recursively fill the child directories
                DirectoryInfo[] diA = new System.IO.DirectoryInfo(sourceFolderPath).GetDirectories();
                foreach (DirectoryInfo di in diA)
                {
                    this.CopyFolder(di.FullName, destinationFolderPath + "\\" + (new DirectoryInfo(sourceFolderPath).Name), overwrite);

                }
            }
            else
            {
                this.errMsg = "Source folder " + sourceFolderPath + " doesn't exist.";
                return false;
            }

            return true;

        }

		/// <summary>
		/// Return the path of a file, given its absolute path complete of file name.
		/// It returns the string without the file name, and without the last "\".
		/// </summary>
		/// <param name="completeAbsolutePath">absolute path complete of file name</param>
		/// <returns>A string representing the path of the file</returns>
		public static string GetPath(string completeAbsolutePath)
		{
            if (completeAbsolutePath == null)
            {
                throw new ArgumentNullException("completeAbsolutePath");
            }

			int fileNamePos = completeAbsolutePath.LastIndexOf("\\");
			return completeAbsolutePath.Substring(0,fileNamePos);
		}

        /// <summary>
        /// If the given file or directory exists
        /// </summary>
        /// <param name="fileOrDirectoryPath">The file or directory path.</param>
        /// <returns>True if this file or directory exists</returns>
        public static bool Exists(string fileOrDirectoryPath)
        {
            bool exists = false;

            if (File.Exists(fileOrDirectoryPath))
            {
                exists = true;
            }
            else
            {
                if (Directory.Exists(fileOrDirectoryPath))
                {
                    exists = true;
                }
            }

            return exists;
        }

		/// <summary>
		/// Get the only instance of this class (singleton).
		/// </summary>
		/// <returns>Handle to FileManager object</returns>
		public static FileManager Reference
		{
            get
            {
                if (fRef == null)
                {
                    fRef = new FileManager();
                }
                return fRef;
            }
		}

	}
}

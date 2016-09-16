/*
 * LLCryptoLib - Advanced .NET Encryption and Hashing Library
 * v.$id$
 * 
 * The contents of this file are subject to the license distributed with
 * the package (the License). This file cannot be distributed without the 
 * original LittleLite Software license file. The distribution of this
 * file is subject to the agreement between the licensee and LittleLite
 * Software.
 * 
 * Customer that has purchased Source Code License may alter this
 * file and distribute the modified binary redistributables with applications. 
 * Except as expressly authorized in the License, customer shall not rent,
 * lease, distribute, sell, make available for download of this file. 
 * 
 * This software is not Open Source, nor Free. Its usage must adhere
 * with the License obtained from LittleLite Software.
 * 
 * The source code in this file may be derived, all or in part, from existing
 * other source code, where the original license permit to do so.
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Threading;

using LLCryptoLib.Utils;

namespace LLCryptoLib.Shred
{

	/// <summary>
	/// Shredder.
	/// The shredder class is tipically called in this way:
	/// <pre lang="cs">
	/// Shredder s = new Shredder(new Random())
	/// FileInfo theFile = new FileInfo(@"C:\temp\log.txt");
    /// IShredMethod method = ShredMethods.Get(AvailableShred.COMPLEX);
    /// if (s.WipeFile(theFile, method, true))
    /// {
    ///     log("File shredded.");
    /// }
	/// </pre>
	/// </summary>
	public class Shredder
	{
        
		private const string NORMAL = "qwertyuioplkjhgfdsazxcvbnm1234567890QWERTYUIOPLKJHGFDSAZXCVBNM";
        private const int CHUNK_SIZE = 2048;

		private string errorMessage;
        private string lastShreddedItemPath;
		private Random rnd;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rand">A random seed</param>
		public Shredder(Random rand)
		{           
			this.rnd = rand;
            this.lastShreddedItemPath = String.Empty;
		}

        /// <summary>
        /// Gets the full path to the last shredded file or folder
        /// </summary>
        /// <value>The last shredded path.</value>
        public string LastShreddedPath
        {
            get
            {
                return this.lastShreddedItemPath;
            }
        }

		/// <summary>
		/// Wipe a file
		/// </summary>
		/// <param name="filePath">Complete file path of file to be wiped</param>
		/// <param name="method">Shredding method</param>
		/// <param name="delete">If false it just rewrites the file and filename. If true, rewrites and then delete.</param>
		/// <returns>True if the file has been successfully shredded</returns>
		public bool WipeFile(FileInfo filePath, IShredMethod method, bool delete)
		{
			return this.WipeFile(filePath, method, null, null, delete);
		}

        /// <summary>
        /// Wipe a file with a feedback on operation progress
        /// </summary>
        /// <param name="filePath">Complete file path of file to be wiped</param>
        /// <param name="method">Shredding method</param>
        /// <param name="cbe">Callback entry point for feedback. The callback method will be called every 1024 bytes shredded. An integer will be passed to callback method with the current kilobyte being erased. For instance, if you want to erase a 200kb file, this delegate will call the method for 200 times passing to it integers from 1 to 200.</param>
        /// <param name="phase">The phase feedback message</param>
        /// <param name="delete">If false it just rewrites the file and filename. If true, rewrites and then delete</param>
        /// <returns>
        /// True if the file has been successfully shredded
        /// </returns>
        public bool WipeFile(FileInfo filePath, IShredMethod method, CallbackEntry cbe, CallbackPoint phase, bool delete)
        {
            return this.WipeFile(filePath, method, cbe, phase, null, delete);
        }


        /// <summary>
        /// Wipe a file with a feedback on operation progress and an autoreset event requese.
        /// </summary>
        /// <param name="filePath">Complete file path of file to be wiped</param>
        /// <param name="method">Shredding method</param>
        /// <param name="cbe">Callback entry point for feedback. The callback method will be called every 1024 bytes shredded. An integer will be passed to callback method with the current kilobyte being erased. For instance, if you want to erase a 200kb file, this delegate will call the method for 200 times passing to it integers from 1 to 200.</param>
        /// <param name="phase">The phase feedback message</param>
        /// <param name="resEvent">A stop operations request event. If set, the method stop current operation and returns false</param>
        /// <param name="delete">If false it just rewrites the file and filename. If true, rewrites and then delete</param>
        /// <returns>
        /// True if the file has been successfully shredded
        /// </returns>
		public bool WipeFile(FileInfo filePath, IShredMethod method, CallbackEntry cbe, CallbackPoint phase, AutoResetEvent resEvent, bool delete)
		{
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

			// Is File Read Only?
			if ((filePath.Attributes & FileAttributes.ReadOnly) != 0)
			{
				// Set the file writable
				FileAttributes attr = (FileAttributes)(filePath.Attributes - FileAttributes.ReadOnly);
				File.SetAttributes(filePath.FullName, attr);
			}

			// Wipe File
            System.Diagnostics.Debug.WriteLine("Shredding " + filePath.FullName + "...");
			bool status = Shredder.WipeFile(filePath.FullName, method, cbe, phase, resEvent);

			// Rename
			if (status)
			{
                System.Diagnostics.Debug.WriteLine("Done");
				try
				{
                    string newname = this.GetRandomFileName(20);
					FileManager fm = FileManager.Reference;
                    System.Diagnostics.Debug.WriteLine("Renaming to " + newname);
                    if (!fm.RenameFile(filePath.FullName, newname))
					{
						status = false;
						this.errorMessage = fm.ErrorMessage;
					}

                    this.lastShreddedItemPath = Path.Combine(filePath.DirectoryName, newname);

					if (delete)
					{
                        System.Diagnostics.Debug.WriteLine("Deleting " + this.lastShreddedItemPath);
                        File.Delete(this.lastShreddedItemPath);
					}
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine(e.Source);
					System.Diagnostics.Debug.WriteLine(e.Message);
					System.Diagnostics.Debug.WriteLine(e.StackTrace);
					this.errorMessage = e.Message;
					status = false;
				}
			}

			return status;

		}

		/// <summary>
		/// Random folder name
		/// </summary>
		/// <param name="characters">The number of characters in the folder name</param>
		/// <returns>A random folder name of 'characters' length</returns>
		public string GetRandomFolderName(int characters)
		{
			StringBuilder sb = new StringBuilder("D@",characters+2);
			int position;

			for (int j=0; j<characters; j++)
			{
				position = this.rnd.Next(NORMAL.Length);
				sb.Append(NORMAL[position]);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Return a random filename
		/// </summary>
		/// <param name="characters">The number of characters in the file name</param>
		/// <returns>A random filename of 'characters' number of character. A dot will be placed before the last three characters, ie: ejeskjekjw.ejh</returns>
		public string GetRandomFileName(uint characters)
		{
			int ichars = (int)characters;  // done to prevent arithmetic overflow
			StringBuilder sb = new StringBuilder(ichars);
			int position;

			for (int j=0; j<ichars; j++)
			{
				position = this.rnd.Next(NORMAL.Length);
				if (j!=ichars-4)
				{
					sb.Append(NORMAL[position]);
				}
				else
				{
					
					sb.Append('.');
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Wipe an empty directory: rename old empty directory and
		/// then deletes it. Two passes: rename each dir, then delete each renamed dir.
		/// </summary>
		/// <param name="di">The directory to be wiped</param>
		/// <returns>The random name to which the directory was named before it was deleted</returns>
		public string WipeEmptyDir(DirectoryInfo di)
		{
            if (di == null)
            {
                throw new ArgumentNullException("di");
            }
			string newDirectoryName = null;

			if (di.Exists)
			{
				if (di.GetDirectories().Length==0)
				{
					if (di.GetFiles().Length==0)
					{
						try
						{
							string newfoldername = this.GetRandomFolderName(18);
                            System.Diagnostics.Debug.WriteLine("Renaming directory " + di.Name + " to: " + newfoldername);
                            this.lastShreddedItemPath = Path.Combine(di.Parent.FullName, newfoldername);
                            if (!Directory.Exists(this.lastShreddedItemPath))
							{
                                    di.MoveTo(this.lastShreddedItemPath);
                                    newDirectoryName = newfoldername;
                                    System.Diagnostics.Debug.WriteLine("Renamed directory: " + this.lastShreddedItemPath);

							}
							else
							{
								System.Diagnostics.Debug.WriteLine(">>> Strange: random dir exists: wiping out!");
								di.Delete(true);
							}

                            DirectoryInfo toDelete = new DirectoryInfo(this.lastShreddedItemPath);
							if (toDelete.Exists)
							{
								try
								{
                                    Thread.Sleep(250);
									toDelete.Delete(true);
                                    System.Diagnostics.Debug.WriteLine("Deleted directory: " + this.lastShreddedItemPath);
								}
								catch (IOException ieox)
								{
                                    System.Diagnostics.Debug.WriteLine(ieox.Message);
                                    System.Diagnostics.Debug.WriteLine(ieox.StackTrace);

                                    // Retry...
                                    Thread.Sleep(250);
                                    Directory.Delete(toDelete.FullName, true);
								}
							}
							else
							{
								System.Diagnostics.Debug.WriteLine(">>> Strange: renamed dir does not exists!");
							}
						}
						catch (Exception ioe)
						{
							System.Diagnostics.Debug.WriteLine(ioe.Source);
							System.Diagnostics.Debug.WriteLine(ioe.Message);
							System.Diagnostics.Debug.WriteLine(ioe.StackTrace);
						}	
					}
#if DEBUG
					else
					{
						Console.WriteLine(">>> WARNING: Trying to wipe a directory with files inside. Nothing will be done.");
					}
#endif
				}
				else
				{
					System.Diagnostics.Debug.WriteLine(">>> WARNING: Trying to wipe a directory with subdirectories inside: it will be deleted later.");
				}
			}

			return newDirectoryName;
		}

		/// <summary>
		/// Error message
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				string message = this.errorMessage;
				this.errorMessage = "";
				return message;
			}
		}

        /// <summary>
        /// Wipe single file
        /// </summary>
        /// <param name="fullName">Full path to file</param>
        /// <param name="method">Shredding method</param>
        /// <param name="fileShreddingFeedback">Callback entry point for feedback (can be null). The callback method will be called every 1024 bytes shredded. An integer will be passed to callback method with the current kilobyte being erased. For instance, if you want to erase a 200kb file, this delegate will call the method for 200 times passing to it integers from 1 to 200.</param>
        /// <param name="phaseFeedback">The phase feedback. A delegate is called at any shredding phase. IE: if the shredding method overwrites 3 times with 0x00, 0xff, 0x00, the delegate is called 3 times with the current byte being written as a message.</param>
        /// <param name="resetReq">A reset request</param>
        /// <returns>True if the file was correctly wiped out</returns>
		private static bool WipeFile(string fullName, 
                                     IShredMethod method, 
                                     CallbackEntry fileShreddingFeedback,
                                     CallbackPoint phaseFeedback,
                                     AutoResetEvent resetReq)
		{
			bool success = true;
            bool resetNow = false;
			long fileLength = 0;
            byte[] pattern = method.GetSequence();

			FileStream sw = null;

            int phaseCount = 0;
            byte[] chunk;
            int chunkSize;
            long steps;
            long slack;

            foreach (byte phaseByte in pattern)
            {
                phaseCount++;

                if (resetNow)
                {
                    break;
                }

                string phase = phaseByte.ToString("X2");
                System.Diagnostics.Debug.WriteLine("Writing byte phase " + phase);
                if (phaseFeedback != null)
                {
                    phaseFeedback(phaseCount, "0x" + phase);
                }

                try
                {
                    sw = new FileStream(fullName, FileMode.Open, FileAccess.Write, FileShare.None);
                    fileLength = sw.Length;

                    chunk = Shredder.GetChunkSequence(phaseByte);
                    chunkSize = chunk.Length;
                    steps = fileLength / chunkSize;
                    slack = fileLength % chunkSize;

                    sw.Seek(0, SeekOrigin.Begin);
                    for (long b = 0; b < steps; b++)
                    {
                        
                        sw.Write(chunk, 0, chunkSize);  // Writes the computed chunk (approx. 2kb)

                        // Feedback
                        if (fileShreddingFeedback != null)
                        {
                            fileShreddingFeedback((int)(b));
                        }

                        if (resetReq != null)
                        {
                            // Process reset request
                            // Check if the user wants to stop
                            if (resetReq.WaitOne(0, false))
                            {
                                System.Diagnostics.Debug.WriteLine("-- Shredding aborted by User request --");
                                resetNow = true;
                                break;
                            }
                        }
                    }

                    sw.Write(chunk, 0, (int)slack); // Flushes the file
                }
                catch (IOException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Source);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    success = false;
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                    }
                }
            }

            if (resetNow)
            {
                success = false;
            }

			return success;

		}

        /// <summary>
        /// This method initializes the chunk array to be written in chunks on disk
        /// </summary>
        /// <param name="fillingByte">The byte for this phase sequence, ie: FF</param>
        /// <returns>A circa 2000 bytes length byte array filled with sequence</returns>
        private static byte[] GetChunkSequence(byte fillingByte)
        {
            byte[] newArr = new byte[CHUNK_SIZE];
            for (int k = 0; k < CHUNK_SIZE; k++)
            {
                newArr[k] = fillingByte;
            }
            return newArr;
        }

        /// <summary>
        /// Gets the size of the chunk (length in bytes of the single shredding step).
        /// </summary>
        /// <returns>The size length in bytes of the single shredding step</returns>
        public static int ChunkSize
        {
            get
            {
                return CHUNK_SIZE;
            }
        }
	}
}

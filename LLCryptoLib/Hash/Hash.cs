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
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */


using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Runtime.InteropServices;

using LLCryptoLib.Utils;

namespace LLCryptoLib.Hash
{
	/// <summary>
	/// Base Hash class. Wraps all hashing functions.
	/// </summary>
    /// <example>
    /// <code>
    /// IHash hashObject = new Hash();
    /// hashObject.SetAlgorithm(AvailableHash.MD5);
    /// Console.WriteLine("MD5 hash: {0}", hashObject.ComputeHashFileStyle(fileToHash.FullName, HexStyle.UNIX));
    /// </code>
    /// </example>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces"), ComVisible(true)]
	public sealed class Hash : IHash, IDisposable
	{
        private const HexEnum DEFAULT_HEXSTYLE = HexEnum.UNIX;

		private const short STEP = 4096; // Step for in streaming file reading

#if !MONO
        private const string LIBRARY_REGISTRY_PATH = @"LittleLite Software\Iside";
#endif

		private HashAlgorithm algorithm;  // Hash Algorithm


		/// <summary>
		/// Hash code.
		/// Default algortithm SHA256
		/// </summary>
		public Hash()
		{
			this.algorithm = new SHA256Managed(); // default
		}

		/// <summary>
		/// Step for in streaming file reading.
        /// Currently set to 4096
		/// </summary>
		public static short ChunkSize
		{
			get
			{
				return Hash.STEP;
			}
		}

		/// <summary>
		/// Set hashing algorithm. It's not a property following COM standards.
		/// <see cref="SupportedHashAlgo"/>
		/// </summary>
        /// <param name="sh">Hash algorithm ID</param>
        /// <example>
        /// <code>
        /// IHash hashObject = new Hash();
        /// hashObject.SetAlgorithm(AvailableHash.MD5);
        /// Console.WriteLine("MD5 hash: {0}", hashObject.ComputeHashFileStyle(fileToHash.FullName, HexStyle.UNIX));
        /// </code>
        /// </example>
		public void SetAlgorithm(AvailableHash sh)
		{
			this.algorithm = SupportedHashAlgoFactory.GetAlgo(sh).Algorithm;
		}

		/// <summary>
		/// Set hashing algorithm. It's not a property following COM standards.
		/// The int parameter is for easy COM parameters exchange
        /// <see cref="SupportedHashAlgo"/>
        /// <see cref="AvailableHash"/>
		/// </summary>
		/// <param name="algoId">See AvailableHash for a list of Id's
        /// </param>
		public void SetAlgorithmInt(int algoId)
		{
			AvailableHash ah = (AvailableHash)algoId;
			this.algorithm = SupportedHashAlgoFactory.GetAlgo(ah).Algorithm;
		}

		/// <summary>
		/// Set hashing algorithm - keyed hash algorithm
		/// <see cref="SupportedHashAlgo"/>
		/// </summary>
		/// <param name="ha">Supported Algorithms</param>
		/// <param name="key">Vigenere for keyed hash algorithm</param>
		public void SetAlgorithmBytes(AvailableHash ha, byte[] key)
		{
			this.algorithm = SupportedHashAlgoFactory.GetAlgo(ha).Algorithm;
		}

		/// <summary>
		/// Set hashing algorithm. It's not a property following COM standards.
		/// <see cref="SupportedHashAlgo"/>
		/// </summary>
		/// <param name="sha">Supported Hash Algorithm object</param>
        /// <exception cref="ArgumentNullException" />
		public void SetAlgorithmAlgo(SupportedHashAlgo sha)
		{
            if (sha == null)
            {
                throw new ArgumentNullException("sha");
            }

			this.algorithm = sha.Algorithm;
		}

		/// <summary>
		/// Get hashing algorithm. It's not a property following COM standards.
		/// </summary>
		/// <returns>Hashing algorithm</returns>
		public HashAlgorithm Algorithm
		{
            get
            {
                return this.algorithm;
            }
		}

        /// <summary>
        /// Compute Hash of given string
        /// </summary>
        /// <param name="message">Message to compute the hash for</param>
        /// <param name="textEncoding">The text encoding (ie: UTF8, ASCII).</param>
        /// <returns>Hash string</returns>
		public string ComputeHash(string message, Encoding textEncoding)
		{
			return this.ComputeHashStyle(message, DEFAULT_HEXSTYLE, textEncoding);
		}

        /// <summary>
        /// Compute Hash of given string
        /// </summary>
        /// <param name="message">Message to compute the hash for</param>
        /// <param name="style">Style of displaying hex numbers</param>
        /// <param name="textEncoding">The text encoding (ie: UTF8, ASCII).</param>
        /// <returns>Hash string</returns>
		public string ComputeHashStyle(string message, HexEnum style, Encoding textEncoding)
		{
			string strHex;
			byte[] result = this.HashCode(message, textEncoding);
			strHex = Hexer.BytesToHex(result,style);
			return strHex;
		}

        /// <summary>
        /// Compute Hash of given string
        /// </summary>
        /// <param name="message">Message to compute the hash for</param>
        /// <param name="divide">Char to divide hexes.</param>
        /// <param name="textEncoding">The text encoding (ie: UTF8, ASCII).</param>
        /// <returns>Hash string</returns>
		public string ComputeHashEx(string message, char divide, Encoding textEncoding)
		{
			string strHex;
			byte[] result = this.HashCode(message, textEncoding);
			strHex = Hexer.BytesToHex(result,divide);
			return strHex.Trim();
		}

		/// <summary>
		/// Compute Hash of given file. 
		/// </summary>
        /// <param name="filePath">Absolute path to file</param>
		/// <returns>Hash</returns>
		public string ComputeHashFile(string filePath)
		{
			return this.ComputeHashFileStyle(filePath, DEFAULT_HEXSTYLE);
		}

        /// <summary>
        /// Computes the hash from a set of bytes and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="contents">The contents as a set of bytes.</param>
        /// <returns>Hash</returns>
        public string ComputeHashFromBytes(byte[] contents)
        {
            return this.ComputeHashFromBytesStyle(contents, DEFAULT_HEXSTYLE);
        }

        /// <summary>
        /// Computes the hash from a set of bytes and returns it as a hexadecimal string.
        /// </summary>
        /// <param name="contents">The contents as a set of bytes.</param>
        /// <param name="style">The hexadecimal style.</param>
        /// <returns>Hash</returns>
        public string ComputeHashFromBytesStyle(byte[] contents, HexEnum style)
        {
            string hash = null;

            if (this.algorithm != null)
            {
                algorithm.ComputeHash(contents);
                byte[] result = algorithm.Hash;
                hash = Hexer.BytesToHex(result, style);
            }

            return hash;
        }

		/// <summary>
		/// Compute Hash of given file. 
		/// </summary>
        /// <param name="filePath">Absolute path to file</param>
		/// <param name="style">Style of displaying hex numbers</param>
		/// <returns>Hash</returns>
		public string ComputeHashFileStyle(string filePath, HexEnum style)
		{
			return this.ComputeHashFileStyleEx(filePath, style, null, null);
		}

		/// <summary>
		/// Compute Hash of given file. 
		/// </summary>
		/// <param name="completeFileName">Absolute path to a file</param>
		/// <param name="style">Style of displaying hex numbers</param>
		/// <param name="cbe">Delegate to be reported of operation advancement</param>
		/// <param name="resetDemand">A reset event to call when the operation must be cancelled</param>
		/// <returns>Hash</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public string ComputeHashFileStyleEx(string completeFileName, HexEnum style, CallbackEntry cbe, AutoResetEvent resetDemand)
		{

			FileStream fs=null;
			byte[] hashBytes;
			byte[] buffer = new byte[STEP];
			string hash;
			bool resetRequest = false;
			bool resetCheck = true;

			if (resetDemand == null)
			{
				resetCheck = false;
			}

			if (this.algorithm==null)
			{
				return "Hash algorithm not set.";
			}

			try
			{
				this.algorithm.Initialize();

#if DEBUG
				if (this.algorithm is HMACSHA1)
				{
					HMACSHA1 hmac = (HMACSHA1)this.algorithm;
					System.Diagnostics.Debug.WriteLine("HMAC-SHA1 with password "+Hexer.BytesToHex(hmac.Key, DEFAULT_HEXSTYLE));
				}
#endif

				fs = new FileStream(completeFileName,FileMode.Open,FileAccess.Read);
				int len;
				long rdlen = 0;
				long totlen = fs.Length;

				while (rdlen < totlen)
				{
					if (resetCheck)
					{
						// Check if the user wants to stop
						if (resetDemand.WaitOne(0, false))
						{
							resetRequest = true;
							break;
						}
					}

					len = fs.Read(buffer,0,STEP);
					rdlen += len;
					if (rdlen<totlen)
					{
						this.algorithm.TransformBlock(buffer,0,STEP,buffer,0);
					}
					else
					{
						this.algorithm.TransformFinalBlock(buffer,0,len);
					}
					
					if (cbe!=null)
					{
						cbe((int)(rdlen/STEP));
					}
				}

				if (!resetRequest)
				{
					hashBytes = this.algorithm.Hash;
					hash = Hexer.BytesToHex(hashBytes,style);
				}
				else
				{
					hash = "-- Aborted --";
				}
			}
			catch (Exception e)
			{
				hash = "Error: "+e.Message;
			}
			finally
			{
				if (fs!=null)
				{
					fs.Close();
				}
			}
			return hash;
		}


		/// <summary>
		/// Compute the hash of multiple files
		/// </summary>
		/// <param name="files">Array of files</param>
		/// <param name="style">Style of text hash</param>
		/// <param name="cbe">Callback entry to call for feedback on operations</param>
        /// <param name="resetDemand">A reset event to call when the operation must be cancelled</param>
		/// <returns>A string representing the hash code</returns>
        public string ComputeHashFiles(FileInfo[] files, HexEnum style, CallbackEntry cbe, AutoResetEvent resetDemand)
		{
			FileStream fs=null;
			byte[] hashBytes;
			byte[] buffer = new byte[STEP];
			string hash;
            bool resetRequest = false;

			if (this.algorithm==null)
			{
				return "Hash algorithm not set.";
			}

			try
			{
				long rdlen = 0;			// bytes read
				long totlen = 0;		// total bytes to be read

				int fileProcessed = 0;  // begin processing first file
				bool processNext = true;

				while (processNext)
				{
                    using (fs = new FileStream(files[fileProcessed].FullName, FileMode.Open, FileAccess.Read))
                    {
                        int len;

                        totlen = files[fileProcessed].Length;
                        rdlen = 0;

                        while (rdlen < totlen)
                        {
                            if (resetDemand != null)
                            {
                                // Check if the user wants to stop
                                if (resetDemand.WaitOne(0, false))
                                {
                                    resetRequest = true;
                                    break;
                                }
                            }

                            len = fs.Read(buffer, 0, STEP);
                            rdlen += len;
                            if (rdlen < totlen)
                            {
                                this.algorithm.TransformBlock(buffer, 0, STEP, buffer, 0);
                            }
                            else
                            {
                                if (fileProcessed == files.Length - 1)
                                {
                                    this.algorithm.TransformFinalBlock(buffer, 0, len);
                                    processNext = false;
                                }
                                else
                                {
                                    this.algorithm.TransformBlock(buffer, 0, len, buffer, 0);
                                }
                            }
                        }

                        if (resetRequest)
                        {
                            break;
                        }

                        if (cbe != null)
                        {
                            cbe(fileProcessed);
                        }
                        fileProcessed++;
                    }
				}

                if (!resetRequest)
                {
                    hashBytes = this.algorithm.Hash;
                    hash = Hexer.BytesToHex(hashBytes, style);
                }
                else
                {
                    hash = "-- Aborted --";
                }

			}
			catch (Exception e)
			{
				hash = "Error in Hash.ComputeHashFiles(): "+e.Message.Trim();
			}

			return hash;
		}

		/// <summary>
		/// Compute Hash of given file. The given file must be a text file.
		/// </summary>
		/// <param name="completeFileName">Absolute path to a file</param>
		/// <param name="enc">Text encoding - of file and of return string</param>
		/// <returns>Hash</returns>
		public string ComputeHashTextFile(string completeFileName, Encoding enc)
		{
			FileManager fm = FileManager.Reference;
			string compareString = fm.ReadTextFile(completeFileName,enc);
			string hash;
			if (fm.ErrorMessage.Length==0)
			{
                hash = this.ComputeHash(compareString, enc);
			}
			else
			{
				hash = "Error";
			}
			return hash;
		}

		/// <summary>
		/// Copy hash of a file to the system clipboard.
		/// The file will be copied in the form: FILEPATH+"::"+HASH
		/// </summary>
		/// <param name="filePath">File to get the hash for</param>
		public void CopyHashFile(string filePath)
		{
			string hash = this.ComputeHashFile(filePath);
			string clipString = filePath+"::"+hash;
			ClipboardUtils.Copy(clipString);
		}

		/// <summary>
		/// Compare the hash of  given file with the one in the clipboard.
		/// The hash in the clipboard must be in the form: FILEPATH+"::"+HASH
		/// </summary>
		/// <param name="filePath">The file to be compared with the hash in the clipboard</param>
		/// <returns>A detailed feedback of the operation</returns>
		public string CompareHashClipboardEx(string filePath)
		{
			string comparisonFeedback = "You must 'Iside copy' a file before verifying it with another.";

			if (ClipboardUtils.CanPaste())
			{
				string comparedFile;
				string hashInMemory = ClipboardUtils.Get();

				System.Diagnostics.Debug.WriteLine(String.Format(Config.NUMBERFORMAT,"String in the clipboard is {0}", hashInMemory));

				int separatorIndex = hashInMemory.IndexOf("::");

				System.Diagnostics.Debug.WriteLine(String.Format(Config.NUMBERFORMAT,"Index of :: is {0}",separatorIndex));

				if (separatorIndex!=-1)
				{
					comparedFile = hashInMemory.Substring(0,separatorIndex);
					hashInMemory = hashInMemory.Substring(separatorIndex+2);

					if (hashInMemory.Length==Hash.HashStringLen(DEFAULT_HEXSTYLE,this.algorithm))
					{
						string hashFile = this.ComputeHashFile(filePath);
						System.Diagnostics.Debug.WriteLine(String.Format(Config.NUMBERFORMAT,"The hash for {0} is {1} ", filePath,hashFile));

                        comparisonFeedback = Hash.FormatTabString(comparedFile, hashInMemory, filePath, hashFile);
						comparisonFeedback += "\n\n";

						if (hashInMemory.Equals(hashFile))
						{
							comparisonFeedback += "File contents are equal.\n";
						}
						else
						{
							comparisonFeedback += "File contents are NOT equal.\n";
						}	
					}
					else
					{
						comparisonFeedback = "Error: Different hash algorithm.";
					}
				}
			
			}
			return comparisonFeedback;
		}


		/// <summary>
		/// Compare the hash of  given file with the one in the clipboard.
		/// The hash in the clipboard must be in the form: FILEPATH+"::"+HASH
		/// </summary>
		/// <param name="filePath">File to get the hash for</param>
		/// <returns>null if comparison can't be made. Else a detailed comparison result string</returns>
		public bool CompareHashClipboard(string filePath)
		{
			bool okCompared = false;

			System.Diagnostics.Debug.WriteLine(String.Format(Config.NUMBERFORMAT,"CompareHashClipboard for file {0}",filePath));

			if (ClipboardUtils.CanPaste())
			{
				string hashInMemory = ClipboardUtils.Get();

				System.Diagnostics.Debug.WriteLine(Config.NUMBERFORMAT,String.Format(Config.NUMBERFORMAT,"String in the clipboard is {0}", hashInMemory));

				int separatorIndex = hashInMemory.IndexOf("::");
				if (separatorIndex!=-1)
				{
					hashInMemory = hashInMemory.Substring(separatorIndex+2);
                    if (hashInMemory.Length == Hash.HashStringLen(DEFAULT_HEXSTYLE, this.algorithm))
					{
						okCompared = this.CompareHash(filePath,hashInMemory);
					}
				}
			}
			return okCompared;
		}
	
		/// <summary>
		/// Compare the hash of  given file with the one given.
		/// </summary>
		/// <param name="filePath">File to get the hash for</param>
		/// <param name="hashToCompare">Hash string to be compared</param>
		/// <returns>True if the hash of file is the same as hashToCompare string</returns>
		public bool CompareHash(string filePath, string hashToCompare)
		{
			bool okCompared = false;


			System.Diagnostics.Debug.WriteLine(String.Format(Config.NUMBERFORMAT,"CompareHash for file {0}",filePath));

			// Compute Hash of filePath
			string hashFile = this.ComputeHashFile(filePath);
			System.Diagnostics.Debug.WriteLine(String.Format(Config.NUMBERFORMAT,"Hash of file is    > {0}",hashFile));
			System.Diagnostics.Debug.WriteLine(String.Format(Config.NUMBERFORMAT,"Hash to compare is > {0}",hashToCompare));

			if (hashFile.Equals(hashToCompare))
			{
				okCompared = true;
			}

			return okCompared;
		}

		/// <summary>
		/// Returns the len of the hash string of an hashAlgo with the given HexStyler
		/// </summary>
		/// <param name="hashAlgo">Algorithm</param>
		/// <param name="hex">Style of hash text</param>
		/// <returns>Length of the hash string of an hashAlgo with the given HexStyler</returns>
		private static int HashStringLen(HexEnum hex, HashAlgorithm hashAlgo)
		{
			int size = hashAlgo.HashSize; // ie MD5 = 128
			int stringLen = size/4;       // MD5 = 32

			switch (hex)
			{
				case HexEnum.CLASSIC:
				case HexEnum.UNIX:
				default:
					;
					break;

				case HexEnum.NETSCAPE:
				case HexEnum.SPACE:
					stringLen += (stringLen/2)-1;
					break;
			}

			return stringLen;
		}

		/// <summary>
		/// Get Library Path
		/// </summary>
		/// <returns>Library Path, without filename and without ending backslash</returns>
		public string GetLibraryPath()
		{
			string lPath = String.Empty;
			
#if !MONO
			try
			{
				lPath = WinRegistry.GetHKCUValue(LIBRARY_REGISTRY_PATH, "");
			}
			catch (Exception e)
			{
				lPath = e.Message;
			}

			System.Diagnostics.Debug.WriteLine("Library Path is: "+lPath);
#else
			System.Diagnostics.Debug.WriteLine("Library Path is NOT SUPPORTED IN MONO");		
#endif

			return lPath;
		}

        /// <summary>
        /// Compute the hash code of a given string
        /// </summary>
        /// <param name="txt">Message</param>
        /// <param name="textEncoding">The text encoding.</param>
        /// <returns>Hash bytes</returns>
		private byte[] HashCode(string txt, Encoding textEncoding)
		{
			byte[] result = null;

			if (this.algorithm!=null)
			{
                byte[] bytePhrase = textEncoding.GetBytes(txt);
				algorithm.ComputeHash(bytePhrase);
				result = algorithm.Hash;
			}

			return result;
		}

		/// <summary>
		/// Return a formatted string such as column B (oneB and two B) result aligned.
		/// </summary>
		/// <param name="oneA">A string</param>
		/// <param name="oneB">A string</param>
		/// <param name="twoA">A string</param>
		/// <param name="twoB">A string</param>
		/// <returns>Formatted string such as column B (oneB and two B) result aligned</returns>
		private static string FormatTabString(string oneA, string oneB, string twoA, string twoB)
		{
			string ricstring = oneA;
			ricstring += "          \n";
			ricstring += "[";
			ricstring += oneB;
			ricstring += "]";
			ricstring += "          \n\n";
			ricstring += twoA;
			ricstring += "          \n";
			ricstring += "[";
			ricstring += twoB;
			ricstring += "]";
			ricstring += "          \n";

			return ricstring;
		}
		#region IDisposable Members

		/// <summary>
		/// Set the Hash algorithm to null
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "algorithm")]
        public void Dispose()
		{
			this.algorithm = null;
		}

		#endregion
	}
}

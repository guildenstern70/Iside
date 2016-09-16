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
using System.CodeDom.Compiler;

namespace LLCryptoLib.Utils
{
	/// <summary>
	/// Temp file class holds a list of temporary file.
	/// The convenient method "DeleteTempFiles" deletes
	/// every temp file created with "GetTempFileFullName"
	/// </summary>
	public sealed class TempFile : IDisposable
	{

		private TempFileCollection tempColl;

		/// <summary>
		/// Constructor
		/// </summary>
		public TempFile()
		{
			tempColl = new TempFileCollection();
		}

        /// <summary>
        /// Number of temp files handled
        /// </summary>
        /// <value>The temp files.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public int TempFiles
		{
			get
			{
				return tempColl.Count;
			}
		}

        /// <summary>
        /// Obtain a fresh temp file name complete of full path.
        /// Use "TempDir" property to get the path only.
        /// </summary>
        /// <value>The name of the temp file.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public string TempFileName
		{
			get
			{
				string tempFile = tempColl.BasePath+RandomString.Get()+".tmp";

				this.tempColl.AddFile(tempFile,false);
#if (DEBUG)
				Console.WriteLine("Temp file created: {0}",tempFile);
#endif
				return tempFile;
			}
		}

		/// <summary>
		/// Delete temporary files
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public void Delete()
		{
			this.tempColl.Delete();
		}


		#region IDisposable Members

		/// <summary>
		/// Delete temporary files and dispose them
		/// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "tempColl")]
        public void Dispose()
		{
			this.Delete();
		}

		#endregion
	}
}

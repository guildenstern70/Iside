/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All rights reserved
 * 
 * AxsUtils.Win32.AxsCursor.cs
 * 
 */
using System;
using System.IO;
using System.CodeDom.Compiler;

namespace AxsUtils.Win32
{
	/// <summary>
	/// Temp file class holds a list of temporary file.
	/// The convenient method "DeleteTempFiles" deletes
	/// every temp file created with "GetTempFileFullName"
	/// </summary>
	public sealed class TempFile : IDisposable
	{

		private TempFileCollection tempColl;

		public TempFile()
		{
			tempColl = new TempFileCollection();
		}

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

		public void Delete()
		{
			this.tempColl.Delete();
		}


		#region IDisposable Members

		public void Dispose()
		{
			this.Delete();
		}

		#endregion
	}
}

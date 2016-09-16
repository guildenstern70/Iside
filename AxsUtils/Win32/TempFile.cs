using System;
using System.CodeDom.Compiler;

namespace AxsUtils.Win32
{
    /// <summary>
    ///     Temp file class holds a list of temporary file.
    ///     The convenient method "DeleteTempFiles" deletes
    ///     every temp file created with "GetTempFileFullName"
    /// </summary>
    public sealed class TempFile : IDisposable
    {
        private readonly TempFileCollection tempColl;

        public TempFile()
        {
            this.tempColl = new TempFileCollection();
        }

        public int TempFiles
        {
            get { return this.tempColl.Count; }
        }

        /// <summary>
        ///     Obtain a fresh temp file name complete of full path.
        ///     Use "TempDir" property to get the path only.
        /// </summary>
        public string TempFileName
        {
            get
            {
                string tempFile = this.tempColl.BasePath + RandomString.Get() + ".tmp";

                this.tempColl.AddFile(tempFile, false);
#if (DEBUG)
                Console.WriteLine("Temp file created: {0}", tempFile);
#endif
                return tempFile;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Delete();
        }

        #endregion

        public void Delete()
        {
            this.tempColl.Delete();
        }
    }
}
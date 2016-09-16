using System;
using System.CodeDom.Compiler;

namespace LLCryptoLib.Utils
{
    /// <summary>
    ///     Temp file class holds a list of temporary file.
    ///     The convenient method "DeleteTempFiles" deletes
    ///     every temp file created with "GetTempFileFullName"
    /// </summary>
    public sealed class TempFile : IDisposable
    {
        private readonly TempFileCollection tempColl;

        /// <summary>
        ///     Constructor
        /// </summary>
        public TempFile()
        {
            this.tempColl = new TempFileCollection();
        }

        /// <summary>
        ///     Number of temp files handled
        /// </summary>
        /// <value>The temp files.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public int TempFiles
        {
            get { return this.tempColl.Count; }
        }

        /// <summary>
        ///     Obtain a fresh temp file name complete of full path.
        ///     Use "TempDir" property to get the path only.
        /// </summary>
        /// <value>The name of the temp file.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
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

        /// <summary>
        ///     Delete temporary files and dispose them
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
             MessageId = "tempColl")]
        public void Dispose()
        {
            this.Delete();
        }

        #endregion

        /// <summary>
        ///     Delete temporary files
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public void Delete()
        {
            this.tempColl.Delete();
        }
    }
}
/**
 **   Iside
 **   Confront files with a single click
 **
 **   Copyright © LittleLite Software
 **
 **
 **/

using System.Drawing;

using LLCryptoLib.Hash;
using LiteSerializer;
using System.Text;
using AxsUtils.Win32;
using LLCryptoLib;
using LLCryptoLib.Utils;

namespace IsideLogic
{
	/// <summary>
	/// RegistryOptions.
	/// </summary>
	public class RegistryOptions : RegistryOptionsBase
	{
		private RegistryOption regFolderHash;
        private RegistryOption regDefaultHash;
		private RegistryOption regShellIntegration;
        private RegistryOption regScanHiddenFiles;
        private RegistryOption regScanSystemFiles;
        private RegistryOption regScanArchiveFiles;

		/// <summary>
		/// Constructor. You can always construct safely
		/// this object because it does not read from registry.
        /// The read/write from/to registry is done by Getter/Setter of RegistryOption.
		/// </summary>
		public RegistryOptions() : base(Config.APPKEY)
		{
			RegistrySpecs regFolderHashSpecs = new RegistrySpecs(typeof(LLCryptoLib.Hash.AvailableHash), appKey, "HashFolder", AvailableHash.MD5);
            RegistrySpecs regDefaultHashSpecs = new RegistrySpecs(typeof(System.Int32), appKey, "DefaultHash", (int)AvailableHash.MD5);
			RegistrySpecs regShellIntegrationSpecs = new RegistrySpecs(typeof(System.Int32), appKey, "ShellIntegration", 3);
			RegistrySpecs regScanHiddenSpecs = new RegistrySpecs(typeof(System.Boolean), appKey, "ScanHiddenFiles", true);
            RegistrySpecs regScanSystemSpecs = new RegistrySpecs(typeof(System.Boolean), appKey, "ScanSystemFiles", false);
            RegistrySpecs regScanArchiveSpecs = new RegistrySpecs(typeof(System.Boolean), appKey, "ScanArchiveFiles", true);
           
			this.regFolderHash = new RegistryOption(regFolderHashSpecs);
			this.regShellIntegration = new RegistryOption(regShellIntegrationSpecs);
            this.regDefaultHash = new RegistryOption(regDefaultHashSpecs);
            this.regScanHiddenFiles = new RegistryOption(regScanHiddenSpecs);
            this.regScanArchiveFiles = new RegistryOption(regScanArchiveSpecs);
            this.regScanSystemFiles = new RegistryOption(regScanSystemSpecs);

		}

        /// <summary>
        /// Gets the folder hash.
        /// </summary>
        /// <value>The folder hash.</value>
		public RegistryOption FolderHash
		{
			get
			{
				return this.regFolderHash;
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether to scan hidden files.
        /// </summary>
        /// <value><c>true</c> if hidden files has to be scanned; otherwise, <c>false</c>.</value>
        public bool ScanHiddenFiles
        {
            get
            {
                return FromInt(this.regScanHiddenFiles.RegistryValue);
            }

            set
            {
                this.regScanHiddenFiles.RegistryValue = value;
            }
        }

        /// <summary>
        /// Gets the default hash.
        /// </summary>
        /// <value>The default hash.</value>
        public RegistryOption DefaultHash
        {
            get
            {
                return this.regDefaultHash;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to scan system files.
        /// </summary>
        /// <value><c>true</c> if system files has to be scanned; otherwise, <c>false</c>.</value>
        public bool ScanSystemFiles
        {
            get
            {
                return FromInt(this.regScanSystemFiles.RegistryValue);
            }

            set
            {
                this.regScanSystemFiles.RegistryValue = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to scan system files.
        /// </summary>
        /// <value><c>true</c> if system files has to be scanned; otherwise, <c>false</c>.</value>
        public bool ScanArchiveFiles
        {
            get
            {
                return FromInt(this.regScanArchiveFiles.RegistryValue);
            }

            set
            {
                this.regScanArchiveFiles.RegistryValue = value;
            }
        }

        /// <summary>
        /// Gets the shell integration.
        /// </summary>
        /// <value>The shell integration.</value>
		public RegistryOption ShellIntegration
		{
			get
			{
				return this.regShellIntegration;
			}
		}


        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder opts = new StringBuilder("== REGISTRY OPTIONS ==");
            opts.AppendLine();

            if (this.SerialNumber.Length > 0)
            {
                opts.AppendLine("Serial Number: " + this.SerialNumber);
                opts.AppendLine("Serial Number: " + this.SerialNumber);
            }
            else
            {
                opts.AppendLine("-- Non registered copy --");
            }
            opts.AppendLine(this.regFolderHash.ToString());
            opts.AppendLine(this.regShellIntegration.ToString());

            return opts.ToString();
        }
	}
}

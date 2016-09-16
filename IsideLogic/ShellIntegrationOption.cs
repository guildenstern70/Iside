/**
 **   Iside
 **   Confront files with a single click
 **
 **   Copyright © LittleLite Software
 **
 **
 **/

using AxsUtils;
using System;
using LLCryptoLib.Hash;
using LiteSerializer;

using AxsUtils.Win32;

namespace IsideLogic
{

	[FlagsAttribute]
	enum ShellIntOptions : int 
	{ 
		NONE				= 0x000,
		FILE_SHELLEXT		= 0x001, 
		MD5_FILEASSOCIATION = FILE_SHELLEXT << 1, 
		FOLDER_SHELLINT		= MD5_FILEASSOCIATION << 1 
	};

	/// <summary>
	/// ShellIntegrationOption.
	/// </summary>
	public class ShellIntegrationOption
	{
		private RegistryOption shellOptionsKey;
		private ShellIntOptions shellOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellIntegrationOption"/> class.
        /// </summary>
		public ShellIntegrationOption()
		{
			this.shellOptionsKey = null;
			this.shellOptions = ShellIntOptions.NONE;
		}

		/// <summary>
		/// The shell integration option is composed by two options:
		/// - The hash algo to be used in shell integration
		/// - If the user wants MD5 file association and/or file context menu.
		/// The first will be saved into a registry key.
		/// The second will be directly written into the appropriate registry location
		/// </summary>
		/// <param name="shellIntegrationValue">The value read from registry</param>
		public ShellIntegrationOption(RegistryOption shellOption)
		{
			this.shellOptionsKey = shellOption;
			this.shellOptions = (ShellIntOptions)this.shellOptionsKey.RegistryValue;
		}

        /// <summary>
        /// Gets the shell registry option.
        /// </summary>
        /// <value>The shell registry option.</value>
		public RegistryOption ShellRegistryOption
		{
			get
			{
				return this.shellOptionsKey;
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether shell extension is set
        /// </summary>
        /// <value><c>true</c> if [file integration]; otherwise, <c>false</c>.</value>
		public bool FileIntegration
		{
			get
			{
				int number = (int)(this.shellOptions & ShellIntOptions.FILE_SHELLEXT);
				return (number != 0);
			}

			set
			{
				if (value)
				{
					this.shellOptions = (this.shellOptions |= ShellIntOptions.FILE_SHELLEXT);
				}
				else
				{
					this.shellOptions = (this.shellOptions &= ~ShellIntOptions.FILE_SHELLEXT);
				}
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether folder shell integration is set.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [folder shell integration]; otherwise, <c>false</c>.
        /// </value>
		public bool FolderShellIntegration
		{
			get
			{
				return (this.shellOptions & ShellIntOptions.FOLDER_SHELLINT) != 0;
			}

			set
			{
				if (value)
				{
					this.shellOptions = (this.shellOptions |= ShellIntOptions.FOLDER_SHELLINT);
				}
				else
				{
					this.shellOptions = (this.shellOptions &= ~ShellIntOptions.FOLDER_SHELLINT);
				}
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether MD5 file association is set
        /// </summary>
        /// <value><c>true</c> if [MD5 file association]; otherwise, <c>false</c>.</value>
		public bool Md5FileAssociation
		{
			get
			{
				return (this.shellOptions & ShellIntOptions.MD5_FILEASSOCIATION) != 0;
			}

			set
			{
				if (value)
				{
					this.shellOptions = (this.shellOptions |= ShellIntOptions.MD5_FILEASSOCIATION);
				}
				else
				{
					this.shellOptions = (this.shellOptions &= ~ShellIntOptions.MD5_FILEASSOCIATION);
				}
			}
		}

        /// <summary>
        /// Writes the options to registry.
        /// </summary>
		public void WriteOptionsToRegistry()
		{
			if (this.shellOptionsKey != null)
			{
				this.shellOptionsKey.RegistryValue = (int)shellOptions;
                ShellIntegrationOption.SetIsideContextMenu(this.FileIntegration);
                ShellIntegrationOption.SetMd5SumAssociation(this.Md5FileAssociation);
                ShellIntegrationOption.SetIsideFolderShellIntegration(this.FolderShellIntegration);
			}

		}

		private static void SetIsideFolderShellIntegration(bool isToBeSet)
		{
			if (isToBeSet)
			{
				string applicationPath = WinRegistry.GetHKCUValue(@"LittleLite Software\Iside","");
				WinRegistry.CreateHKCRKey(@"Directory\shell\isidemd5");
				WinRegistry.CreateHKCRKey(@"Directory\shell\isidemd5\command");
				WinRegistry.SetHKCRValue(@"Directory\shell","isidemd5","","Iside Md5sum");
				WinRegistry.SetHKCRValue(@"Directory\shell\isidemd5","command","",applicationPath+@"Iside.exe -createsum ""%L""");
			}
			else
			{
                WinRegistry.DeleteHKCRKey(@"Directory\shell\isidemd5\command");
				WinRegistry.DeleteHKCRKey(@"Directory\shell\isidemd5");
			}
		}

		private static void SetIsideContextMenu(bool isToBeSet)
		{
			if (isToBeSet)
			{
				WinRegistry.CreateHKCRKey(@"*\shellex\ContextMenuHandlers\IsideShellExtension");
				WinRegistry.SetHKCRValue(@"*\shellex\ContextMenuHandlers","IsideShellExtension", "", "{69D701AF-64F5-40FA-A280-2C7C02AC4921}");
			}
			else
			{
				WinRegistry.DeleteHKCRKey(@"*\shellex\ContextMenuHandlers\IsideShellExtension");
			}
		}

		private static void SetMd5SumAssociation(bool isToBeSet)
		{
			if (isToBeSet)
			{
				System.Diagnostics.Debug.WriteLine("Setting .md5 association to true.");

                // Where is Iside.exe?
			    string applicationPath = WinRegistry.GetHKCUValue(@"LittleLite Software\Iside","");

				if (applicationPath!=null)
				{
					WinRegistry.CreateHKCRKey(@".md5");
					WinRegistry.SetHKCRValue(@".md5","","","LittleLite.Iside.md5sum");
					WinRegistry.CreateHKCRKey(@".md5\LittleLite.Iside.md5sum");
					WinRegistry.CreateHKCRKey(@".md5\LittleLite.Iside.md5sum\shellnew");
					WinRegistry.CreateHKCRKey(@".md5\OpenWithProgids");
					WinRegistry.CreateHKCRKey(@".md5\OpenWithProgids\LittleLite.Iside.md5sum");
					WinRegistry.CreateHKCRKey(@"LittleLite.Iside.md5sum");
					WinRegistry.SetHKCRValue(@"LittleLite.Iside.md5sum","","","Iside MD5Sum");
					WinRegistry.CreateHKCRKey(@"LittleLite.Iside.md5sum\DefaultIcon");
					WinRegistry.SetHKCRValue(@"LittleLite.Iside.md5sum","DefaultIcon","",applicationPath+@"\res\isidemd5.ico");
					WinRegistry.CreateHKCRKey(@"LittleLite.Iside.md5sum\shell");
					WinRegistry.CreateHKCRKey(@"LittleLite.Iside.md5sum\shell\open");
					WinRegistry.SetHKCRValue(@"LittleLite.Iside.md5sum\shell","open","","Open with &Iside");
					WinRegistry.CreateHKCRKey(@"LittleLite.Iside.md5sum\shell\open\command");
					WinRegistry.SetHKCRValue(@"LittleLite.Iside.md5sum\shell\open","command","",applicationPath+@"Iside.exe -md5 ""%1""");
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Setting .md5 association to FALSE.");

				WinRegistry.SetHKCRValue(@"",".md5","","");
				WinRegistry.DeleteHKCRKey(@".md5\LittleLite.Iside.md5sum\shellnew");
				WinRegistry.DeleteHKCRKey(@".md5\LittleLite.Iside.md5sum");
				WinRegistry.DeleteHKCRKey(@".md5\OpenWithProgids\LittleLite.Iside.md5sum");
				WinRegistry.DeleteHKCRKey(@"LittleLite.Iside.md5sum\shell\open\command");
				WinRegistry.DeleteHKCRKey(@"LittleLite.Iside.md5sum\shell\open");
				WinRegistry.DeleteHKCRKey(@"LittleLite.Iside.md5sum\shell");
				WinRegistry.DeleteHKCRKey(@"LittleLite.Iside.md5sum\DefaultIcon");
				WinRegistry.DeleteHKCRKey(@"LittleLite.Iside.md5sum");
			}
		}

		private static string GetBinaryRepr(int number)
		{
			string sx = ConverterBase.Binary.ToString(number);
			if (sx.Length != 3)
			{
				if (sx.Length == 1)
				{
					sx = "00"+sx;
				}
				else
				{
					sx = "0"+sx;
				}
			}
			return sx;
		}

		private static void PrintSO(string x, ShellIntegrationOption so)
		{
			Console.WriteLine(x+" - FolderInt = {0}; Md5 Files = {1}; FileInt = {2}. Current is {3}. ", so.FolderShellIntegration,  so.Md5FileAssociation, so.FileIntegration, ShellIntegrationOption.GetBinaryRepr((int)so.shellOptions));
		}

		public static void Test() 
		{
			ShellIntegrationOption so = new ShellIntegrationOption();
			
			// 100
			so.FolderShellIntegration = true;
			so.Md5FileAssociation = false;
			so.FileIntegration = false;
			so.WriteOptionsToRegistry();
			ShellIntegrationOption.PrintSO("100",so);

			// 111
			so.FileIntegration = true;
			so.Md5FileAssociation = true;
			so.FolderShellIntegration = true;
			so.WriteOptionsToRegistry();
			ShellIntegrationOption.PrintSO("111",so);

			// 010
			so.FileIntegration = false;
			so.Md5FileAssociation = true;
			so.FolderShellIntegration = false;
			so.WriteOptionsToRegistry();
			ShellIntegrationOption.PrintSO("010",so);

			// 011
			so.FileIntegration = true;
			so.Md5FileAssociation = true;
			so.FolderShellIntegration = false;
			so.WriteOptionsToRegistry();
			ShellIntegrationOption.PrintSO("011",so);

		}
	}


}

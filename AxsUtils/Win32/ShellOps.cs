/**
  
    LittleLite NShred
	
    Copyright (C) 2004-2009 LittleLite Software
    Author Alessio Saltarin
	
**/

using System;
using System.Runtime.InteropServices;

namespace AxsUtils.Win32
{
	internal enum FoFunc : uint
	{
		FO_MOVE = 0x0001,
		FO_COPY = 0x0002,
		FO_DELETE = 0x0003,
		FO_RENAME = 0x0004,
	}

    #region enum HChangeNotifyEventID
    /// <summary>
    /// Describes the event that has occurred. 
    /// Typically, only one event is specified at a time. 
    /// If more than one event is specified, the values contained 
    /// in the <i>dwItem1</i> and <i>dwItem2</i> 
    /// parameters must be the same, respectively, for all specified events. 
    /// This parameter can be one or more of the following values. 
    /// </summary>
    /// <remarks>
    /// <para><b>Windows NT/2000/XP:</b> <i>dwItem2</i> contains the index 
    /// in the system image list that has changed. 
    /// <i>dwItem1</i> is not used and should be <see langword="null"/>.</para>
    /// <para><b>Windows 95/98:</b> <i>dwItem1</i> contains the index 
    /// in the system image list that has changed. 
    /// <i>dwItem2</i> is not used and should be <see langword="null"/>.</para>
    /// </remarks>
    [Flags]
    internal enum HChangeNotifyEventID
    {
        /// <summary>
        /// All events have occurred. 
        /// </summary>
        SHCNE_ALLEVENTS = 0x7FFFFFFF,

        /// <summary>
        /// A file type association has changed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> 
        /// must be specified in the <i>uFlags</i> parameter. 
        /// <i>dwItem1</i> and <i>dwItem2</i> are not used and must be <see langword="null"/>. 
        /// </summary>
        SHCNE_ASSOCCHANGED = 0x08000000,

        /// <summary>
        /// The attributes of an item or folder have changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the item or folder that has changed. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
        /// </summary>
        SHCNE_ATTRIBUTES = 0x00000800,

        /// <summary>
        /// A nonfolder item has been created. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the item that was created. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>.
        /// </summary>
        SHCNE_CREATE = 0x00000002,

        /// <summary>
        /// A nonfolder item has been deleted. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the item that was deleted. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_DELETE = 0x00000004,

        /// <summary>
        /// A drive has been added. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive that was added. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_DRIVEADD = 0x00000100,

        /// <summary>
        /// A drive has been added and the Shell should create a new window for the drive. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive that was added. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_DRIVEADDGUI = 0x00010000,

        /// <summary>
        /// A drive has been removed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive that was removed.
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_DRIVEREMOVED = 0x00000080,

        /// <summary>
        /// Not currently used. 
        /// </summary>
        SHCNE_EXTENDED_EVENT = 0x04000000,

        /// <summary>
        /// The amount of free space on a drive has changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive on which the free space changed.
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_FREESPACE = 0x00040000,

        /// <summary>
        /// Storage media has been inserted into a drive. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive that contains the new media. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_MEDIAINSERTED = 0x00000020,

        /// <summary>
        /// Storage media has been removed from a drive. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the root of the drive from which the media was removed. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_MEDIAREMOVED = 0x00000040,

        /// <summary>
        /// A folder has been created. <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> 
        /// or <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that was created. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_MKDIR = 0x00000008,

        /// <summary>
        /// A folder on the local computer is being shared via the network. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that is being shared. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_NETSHARE = 0x00000200,

        /// <summary>
        /// A folder on the local computer is no longer being shared via the network. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that is no longer being shared. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_NETUNSHARE = 0x00000400,

        /// <summary>
        /// The name of a folder has changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the previous pointer to an item identifier list (PIDL) or name of the folder. 
        /// <i>dwItem2</i> contains the new PIDL or name of the folder. 
        /// </summary>
        SHCNE_RENAMEFOLDER = 0x00020000,

        /// <summary>
        /// The name of a nonfolder item has changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the previous PIDL or name of the item. 
        /// <i>dwItem2</i> contains the new PIDL or name of the item. 
        /// </summary>
        SHCNE_RENAMEITEM = 0x00000001,

        /// <summary>
        /// A folder has been removed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that was removed. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_RMDIR = 0x00000010,

        /// <summary>
        /// The computer has disconnected from a server. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the server from which the computer was disconnected. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// </summary>
        SHCNE_SERVERDISCONNECT = 0x00004000,

        /// <summary>
        /// The contents of an existing folder have changed, 
        /// but the folder still exists and has not been renamed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_IDLIST"/> or 
        /// <see cref="HChangeNotifyFlags.SHCNF_PATH"/> must be specified in <i>uFlags</i>. 
        /// <i>dwItem1</i> contains the folder that has changed. 
        /// <i>dwItem2</i> is not used and should be <see langword="null"/>. 
        /// If a folder has been created, deleted, or renamed, use SHCNE_MKDIR, SHCNE_RMDIR, or 
        /// SHCNE_RENAMEFOLDER, respectively, instead. 
        /// </summary>
        SHCNE_UPDATEDIR = 0x00001000,

        /// <summary>
        /// An image in the system image list has changed. 
        /// <see cref="HChangeNotifyFlags.SHCNF_DWORD"/> must be specified in <i>uFlags</i>. 
        /// </summary>
        SHCNE_UPDATEIMAGE = 0x00008000,

    }
    #endregion // enum HChangeNotifyEventID

    #region public enum HChangeNotifyFlags
    /// <summary>
    /// Flags that indicate the meaning of the <i>dwItem1</i> and <i>dwItem2</i> parameters. 
    /// The uFlags parameter must be one of the following values.
    /// </summary>
    [Flags]
    internal enum HChangeNotifyFlags
    {
        /// <summary>
        /// The <i>dwItem1</i> and <i>dwItem2</i> parameters are DWORD values. 
        /// </summary>
        SHCNF_DWORD = 0x0003,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of ITEMIDLIST structures that 
        /// represent the item(s) affected by the change. 
        /// Each ITEMIDLIST must be relative to the desktop folder. 
        /// </summary>
        SHCNF_IDLIST = 0x0000,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of 
        /// maximum length MAX_PATH that contain the full path names 
        /// of the items affected by the change. 
        /// </summary>
        SHCNF_PATHA = 0x0001,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of 
        /// maximum length MAX_PATH that contain the full path names 
        /// of the items affected by the change. 
        /// </summary>
        SHCNF_PATHW = 0x0005,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that 
        /// represent the friendly names of the printer(s) affected by the change. 
        /// </summary>
        SHCNF_PRINTERA = 0x0002,
        /// <summary>
        /// <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that 
        /// represent the friendly names of the printer(s) affected by the change. 
        /// </summary>
        SHCNF_PRINTERW = 0x0006,
        /// <summary>
        /// The function should not return until the notification 
        /// has been delivered to all affected components. 
        /// As this flag modifies other data-type flags, it cannot by used by itself.
        /// </summary>
        SHCNF_FLUSH = 0x1000,
        /// <summary>
        /// The function should begin delivering notifications to all affected components 
        /// but should return as soon as the notification process has begun. 
        /// As this flag modifies other data-type flags, it cannot by used by itself.
        /// </summary>
        SHCNF_FLUSHNOWAIT = 0x2000
    }
    #endregion // enum HChangeNotifyFlags


    internal struct SHFILEOPSTRUCT
	{
		public IntPtr hwnd;
		public FoFunc wFunc;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pFrom;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pTo;
		public ushort fFlags;
		public bool fAnyOperationsAborted;
		public IntPtr hNameMappings;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszProgressTitle;
	}

	internal class NativeMethods
	{
		private NativeMethods() {}

		[DllImport("shell32.dll",  CharSet = CharSet.Unicode)]
		internal static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);

		[DllImport("shell32.dll")]
		internal static extern int SHEmptyRecycleBin(IntPtr hWnd, string pszRootPath, uint dwFlags);

        [DllImport("shell32.dll")]
        internal static extern void SHChangeNotify(HChangeNotifyEventID wEventId, HChangeNotifyFlags uFlags, IntPtr dwItem1, IntPtr dwItem2);
	}

	public class ShellOps
	{

		private SHFILEOPSTRUCT _ShFile;
		private FILEOP_FLAGS fFlags;

		public IntPtr hwnd
		{
			set
			{
				this._ShFile.hwnd = value;
			}
		}

		internal FoFunc wFunc
		{
			set
			{
				this._ShFile.wFunc = value;
			}
		}

		public string pFrom
		{
			set
			{
				this._ShFile.pFrom = value + '\0' + '\0';
			}
		}

		public string pTo
		{
			set
			{
				this._ShFile.pTo = value + '\0' + '\0';
			}
		}

		public bool fAnyOperationsAborted
		{
			set
			{
				this._ShFile.fAnyOperationsAborted = value;
			}
		}

		public IntPtr hNameMappings
		{
			set
			{
				this._ShFile.hNameMappings = value;
			}
		}

		public string lpszProgressTitle
		{
			set
			{
				this._ShFile.lpszProgressTitle = value + '\0';
			}
		}

		public ShellOps()
		{
			this.fFlags = new FILEOP_FLAGS();
			this._ShFile = new SHFILEOPSTRUCT();
			this._ShFile.hwnd = IntPtr.Zero;
			this._ShFile.wFunc = FoFunc.FO_COPY;
			this._ShFile.pFrom = "";
			this._ShFile.pTo = "";
			this._ShFile.fAnyOperationsAborted = false;
			this._ShFile.hNameMappings = IntPtr.Zero;
			this._ShFile.lpszProgressTitle = "";
		}

		public bool Execute()
		{
			this._ShFile.fFlags = this.fFlags.Flag;
			int ReturnValue = NativeMethods.SHFileOperation(ref this._ShFile);
			if ( ReturnValue == 0 )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public class FILEOP_FLAGS
		{
			private enum FILEOP_FLAGS_ENUM : ushort
			{
				FOF_MULTIDESTFILES = 0x0001,
				FOF_CONFIRMMOUSE = 0x0002,
				FOF_SILENT = 0x0004,  // don't create progress/report
				FOF_RENAMEONCOLLISION = 0x0008,
				FOF_NOCONFIRMATION = 0x0010,  // Don't prompt the user.
				FOF_WANTMAPPINGHANDLE = 0x0020,  // Fill in SHFILEOPSTRUCT.hNameMappings
				// Must be freed using SHFreeNameMappings
				FOF_ALLOWUNDO = 0x0040,
				FOF_FILESONLY = 0x0080,  // on *.*, do only files
				FOF_SIMPLEPROGRESS = 0x0100,  // means don't show names of files
				FOF_NOCONFIRMMKDIR = 0x0200,  // don't confirm making any needed dirs
				FOF_NOERRORUI = 0x0400,  // don't put up error UI
				FOF_NOCOPYSECURITYATTRIBS = 0x0800,  // dont copy NT file Security Attributes
				FOF_NORECURSION = 0x1000,  // don't recurse into directories.
				FOF_NO_CONNECTED_ELEMENTS = 0x2000,  // don't operate on connected elements.
				FOF_WANTNUKEWARNING = 0x4000,  // during delete operation, warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
				FOF_NORECURSEREPARSE = 0x8000,  // treat reparse points as objects, not containers
			}


			public bool FOF_MULTIDESTFILES = false;
			public bool FOF_CONFIRMMOUSE = false;
			public bool FOF_SILENT = false;
			public bool FOF_RENAMEONCOLLISION = false;
			public bool FOF_NOCONFIRMATION = false;
			public bool FOF_WANTMAPPINGHANDLE = false;
			public bool FOF_ALLOWUNDO = false;
			public bool FOF_FILESONLY = false;
			public bool FOF_SIMPLEPROGRESS = false;
			public bool FOF_NOCONFIRMMKDIR = false;
			public bool FOF_NOERRORUI = false;
			public bool FOF_NOCOPYSECURITYATTRIBS = false;
			public bool FOF_NORECURSION = false;
			public bool FOF_NO_CONNECTED_ELEMENTS = false;
			public bool FOF_WANTNUKEWARNING = false;
			public bool FOF_NORECURSEREPARSE = false;


            [CLSCompliant(false)] 
			public ushort Flag
			{
				get
				{
					ushort ReturnValue = 0;


					if ( this.FOF_MULTIDESTFILES == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_MULTIDESTFILES;
					if ( this.FOF_CONFIRMMOUSE == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_CONFIRMMOUSE;
					if ( this.FOF_SILENT == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_SILENT;
					if ( this.FOF_RENAMEONCOLLISION == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_RENAMEONCOLLISION;
					if ( this.FOF_NOCONFIRMATION == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOCONFIRMATION;
					if ( this.FOF_WANTMAPPINGHANDLE == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_WANTMAPPINGHANDLE;
					if ( this.FOF_ALLOWUNDO == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_ALLOWUNDO;
					if ( this.FOF_FILESONLY == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_FILESONLY;
					if ( this.FOF_SIMPLEPROGRESS == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_SIMPLEPROGRESS;
					if ( this.FOF_NOCONFIRMMKDIR == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOCONFIRMMKDIR;
					if ( this.FOF_NOERRORUI == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOERRORUI;
					if ( this.FOF_NOCOPYSECURITYATTRIBS == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOCOPYSECURITYATTRIBS;
					if ( this.FOF_NORECURSION == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NORECURSION;
					if ( this.FOF_NO_CONNECTED_ELEMENTS == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NO_CONNECTED_ELEMENTS;
					if ( this.FOF_WANTNUKEWARNING == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_WANTNUKEWARNING;
					if ( this.FOF_NORECURSEREPARSE == true )
						ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NORECURSEREPARSE;


					return ReturnValue;
				}
			}
		}

		//     No dialog box confirming the deletion of the objects will be displayed.
		const int SHERB_NOCONFIRMATION = 0x00000001;
		//     No dialog box indicating the progress will be displayed. 
		const int SHERB_NOPROGRESSUI = 0x00000002;
		//     No sound will be played when the operation is complete. 
		const int SHERB_NOSOUND = 0x00000004;

		public static void SendFileToRecycleBin(string originalPath)
		{
			ShellOps so = new ShellOps();
			so.wFunc = FoFunc.FO_DELETE;
			so.pFrom = originalPath;
			so.fFlags.FOF_ALLOWUNDO = true;
			so.fFlags.FOF_NOCONFIRMATION = true;
			so.Execute();
		}

        public static void ShellChangeNotify()
        {
            NativeMethods.SHChangeNotify(HChangeNotifyEventID.SHCNE_ALLEVENTS, HChangeNotifyFlags.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
        }

        public static void ShellChangeDirNotify(string path)
        {
            IntPtr cstrPath = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path);
            NativeMethods.SHChangeNotify(HChangeNotifyEventID.SHCNE_UPDATEDIR, HChangeNotifyFlags.SHCNF_PATHA, cstrPath, IntPtr.Zero);
        }

		public static void EmptyRecycleBin()
		{
			ShellOps.EmptyRecycleBin ( string.Empty );
		}

		public static void EmptyRecycleBin( string rootPath )
		{
			int hresult = NativeMethods.SHEmptyRecycleBin(IntPtr.Zero, rootPath, SHERB_NOCONFIRMATION | SHERB_NOPROGRESSUI | SHERB_NOSOUND);
			System.Diagnostics.Debug.Write(hresult);
		}
	}

}

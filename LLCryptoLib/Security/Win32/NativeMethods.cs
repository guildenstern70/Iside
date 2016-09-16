using System;
using System.Runtime.InteropServices;
using System.Security;

namespace LLCryptoLib.Security.Win32
{
    [SuppressUnmanagedCodeSecurity]
    internal unsafe class NativeMethods
    {
        internal const int ALG_CLASS_DATA_ENCRYPT = 3 << 13;
        internal const int ALG_CLASS_HASH = 4 << 13;
        internal const int ALG_SID_AES_128 = 14;
        internal const int ALG_SID_AES_192 = 15;
        internal const int ALG_SID_AES_256 = 16;
        internal const int ALG_SID_MD2 = 1;
        internal const int ALG_SID_MD4 = 2;
        internal const int ALG_SID_RC4 = 1;
        internal const int ALG_TYPE_ANY = 0;
        internal const int ALG_TYPE_BLOCK = 3 << 9;
        internal const int ALG_TYPE_STREAM = 4 << 9;
        internal const int AT_KEYEXCHANGE = 1;
        internal const int CALG_AES_128 = ALG_CLASS_DATA_ENCRYPT | ALG_TYPE_BLOCK | ALG_SID_AES_128;
        internal const int CALG_AES_192 = ALG_CLASS_DATA_ENCRYPT | ALG_TYPE_BLOCK | ALG_SID_AES_192;
        internal const int CALG_AES_256 = ALG_CLASS_DATA_ENCRYPT | ALG_TYPE_BLOCK | ALG_SID_AES_256;
        internal const int CALG_MD2 = ALG_CLASS_HASH | ALG_TYPE_ANY | ALG_SID_MD2;
        internal const int CALG_MD4 = ALG_CLASS_HASH | ALG_TYPE_ANY | ALG_SID_MD4;
        internal const int CALG_RC4 = ALG_CLASS_DATA_ENCRYPT | ALG_TYPE_STREAM | ALG_SID_RC4;
        internal const int CRED_MAX_CREDENTIAL_BLOB_SIZE = 512;
        internal const int CRED_MAX_GENERIC_TARGET_NAME_LENGTH = 32767;
        internal const int CRED_MAX_STRING_LENGTH = 256;
        internal const int CRED_PERSIST_NONE = 0;
        internal const int CRED_PERSIST_SESSION = 1;
        internal const int CRED_PERSIST_LOCAL_MACHINE = 2;
        internal const int CRED_PERSIST_ENTERPRISE = 3;
        internal const int CRED_TYPE_GENERIC = 1;
        internal const int CRED_TYPE_MAXIMUM = 5; // Maximum supported cred type
        internal const int CRYPT_EXPORTABLE = 0x00000001;
        internal const int CRYPT_FIRST = 1;
        internal const int CRYPT_MACHINE_KEYSET = 0x00000020;
        internal const int CRYPT_NEWKEYSET = 0x00000008;
        internal const int CRYPT_SILENT = 0x40;
        internal const int ERROR_NOT_FOUND = 1168;
        internal const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        internal const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        internal const int HP_HASHVAL = 0x0002; // Hash value
        internal const string KEY_CONTAINER = "{23C31E77-BF71-4194-BA63-7E9A8404D3C2}";
        internal const int KP_ALGID = 7; // Key algorithm
        internal const int KP_IV = 1; // Initialization vector
        internal const int KP_KEYLEN = 9; // Length of key in bits
        internal const int KP_MODE = 4; // Mode of the cipher
        internal const int KP_MODE_BITS = 5; // Number of bits to feedback
        internal const int KP_PADDING = 3; // Padding values
        internal const int NTE_BAD_KEYSET = -2146893802;
        internal const int PKCS5_PADDING = 1; // PKCS 5 (sec 6.2) padding method
        internal const int PP_ENUMALGS_EX = 22;
        internal const int PRIVATEKEYBLOB = 0x7;
        internal const int PROV_RSA_AES = 24;
        internal const int PROV_RSA_FULL = 1;
        internal const int SCARD_ABSENT = 1;
        internal const int SCARD_ATTR_VENDOR_NAME = (SCARD_CLASS_VENDOR_INFO << 16) | 0x0100;
        internal const int SCARD_ATTR_VENDOR_IFD_TYPE = (SCARD_CLASS_VENDOR_INFO << 16) | 0x0101;
        internal const int SCARD_ATTR_VENDOR_IFD_VERSION = (SCARD_CLASS_VENDOR_INFO << 16) | 0x0102;
        internal const int SCARD_ATTR_VENDOR_IFD_SERIAL_NO = (SCARD_CLASS_VENDOR_INFO << 16) | 0x0103;
        internal const int SCARD_AUTOALLOCATE = -1;
        internal const int SCARD_CLASS_VENDOR_INFO = 1; // Vendor information definitions
        internal const int SCARD_EJECT_CARD = 3;
        internal const int SCARD_LEAVE_CARD = 0;
        internal const int SCARD_NEGOTIABLE = 5;
        internal const int SCARD_POWERED = 4;
        internal const int SCARD_PRESENT = 2;
        internal const int SCARD_PROTOCOL_RAW = 0x00010000;
        internal const int SCARD_PROTOCOL_T0 = 0x00000001;
        internal const int SCARD_PROTOCOL_T1 = 0x00000002;
        internal const int SCARD_PROTOCOL_UNDEFINED = 0x00000000;
        internal const int SCARD_S_SUCCESS = 0;
        internal const int SCARD_SCOPE_SYSTEM = 2;
        internal const int SCARD_SCOPE_USER = 0;
        internal const int SCARD_SHARE_DIRECT = 3;
        internal const int SCARD_SHARE_EXCLUSIVE = 1;
        internal const int SCARD_SHARE_SHARED = 2;
        internal const int SCARD_SPECIFIC = 6;
        internal const int SCARD_STATE_UNAWARE = 0x00000000;
        internal const int SCARD_STATE_IGNORE = 0x00000001;
        internal const int SCARD_STATE_UNAVAILABLE = 0x00000008;
        internal const int SCARD_STATE_EMPTY = 0x00000010;
        internal const int SCARD_STATE_PRESENT = 0x00000020;
        internal const int SCARD_SWALLOWED = 3;
        internal const int SCARD_RESET_CARD = 1;
        internal const int SCARD_UNPOWER_CARD = 2;
        internal const int SIMPLEBLOB = 0x1;
        internal const int ZERO_PADDING = 3;

        private NativeMethods()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int CredDelete([MarshalAs(UnmanagedType.LPWStr)] string TargetName, int Type, int Flags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", EntryPoint = "CredEnumerateW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int CredEnumerate([MarshalAs(UnmanagedType.LPWStr)] string Filter, int Flags,
            out int Count, out IntPtr paCredentials);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll")]
        internal static extern void CredFree(IntPtr Buffer);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", SetLastError = true)]
        internal static extern int CredGetSessionTypes(int MaximumPersistCount, [Out] int[] MaximumPersist);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int CredRead([MarshalAs(UnmanagedType.LPWStr)] string TargetName, int Type, int Flags,
            out IntPtr Credential);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", EntryPoint = "CredRenameW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int CredRename([MarshalAs(UnmanagedType.LPWStr)] string OldTargetName,
            [MarshalAs(UnmanagedType.LPWStr)] string NewTargetName, int Type, int Flags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int CredWrite(ref CREDENTIAL Credential, int Flags);

        //[DllImport(@"advapi32.dll", EntryPoint = "CryptAcquireContextA", CharSet = CharSet.Ansi, SetLastError = true)] // do not remove SetLastError
        //internal static extern int CryptAcquireContext(ref IntPtr phProv, IntPtr pszContainer, string pszProvider, int dwProvType, int dwFlags);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2101:SpecifyMarshalingForPInvokeStringArguments")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptAcquireContextA", CharSet = CharSet.Ansi, SetLastError = true)]
        // do not remove SetLastError
        internal static extern int CryptAcquireContext(ref IntPtr phProv, string pszContainer, string pszProvider,
            int dwProvType, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptCreateHash")]
        internal static extern int CryptCreateHash(IntPtr hProv, int Algid, IntPtr hKey, int dwFlags, out IntPtr phHash);

        [DllImport(@"advapi32.dll", SetLastError = true)]
        internal static extern int CryptDecrypt(IntPtr hKey, IntPtr hHash, int Final, int dwFlags, byte[] pbData,
            ref int pdwDataLen);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptDestroyHash(IntPtr hHash);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptDestroyKey(IntPtr hKey);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptEncrypt(IntPtr hKey, IntPtr hHash, int Final, int dwFlags, IntPtr pbData,
            ref int pdwDataLen, int dwBufLen);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptEncrypt(IntPtr hKey, IntPtr hHash, int Final, int dwFlags, byte[] pbData,
            ref int pdwDataLen, int dwBufLen);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", SetLastError = true)]
        internal static extern int CryptExportKey(IntPtr hKey, IntPtr hExpKey, int dwBlobType, int dwFlags,
            IntPtr pbData, ref int pdwDataLen);

        //[DllImport(@"advapi32.dll", SetLastError = true)]
        //internal static extern int CryptExportKey(IntPtr hKey, IntPtr hExpKey, int dwBlobType, int dwFlags, byte[] pbData, ref int pdwDataLen);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGenKey(IntPtr hProv, int Algid, int dwFlags, ref IntPtr phKey);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGenRandom(IntPtr hProv, int dwLen, byte* pbBuffer);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptGetHashParam")]
        internal static extern int CryptGetHashParam(IntPtr hHash, int dwParam, byte[] pbData, ref int pdwDataLen,
            int dwFlags);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGetKeyParam(IntPtr hKey, int dwParam, ref int pbData, ref int pdwDataLen,
            int dwFlags);

        //[DllImport(@"advapi32.dll")]
        //internal static extern int CryptGetKeyParam(IntPtr hKey, int dwParam, byte[] pbData, ref int pdwDataLen, int dwFlags);
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGetKeyParam(IntPtr hKey, int dwParam, ref IntPtr pbData, ref int pdwDataLen,
            int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGetProvParam(IntPtr hProv, int dwParam, IntPtr pbData, ref int pdwDataLen,
            int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptHashData")]
        internal static extern int CryptHashData(IntPtr hHash, byte[] pbData, int dwDataLen, int dwFlags);

        //[DllImport(@"advapi32.dll", EntryPoint = "CryptHashData")]
        //internal static extern int CryptHashData(IntPtr hHash, IntPtr pbData, int dwDataLen, int dwFlags);
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptImportKey(IntPtr hProv, IntPtr pbData, int dwDataLen, IntPtr hPubKey,
            int dwFlags, ref IntPtr phKey);

        [DllImport(@"advapi32.dll", SetLastError = true)]
        internal static extern int CryptImportKey(IntPtr hProv, byte[] pbData, int dwDataLen, IntPtr hPubKey,
            int dwFlags, ref IntPtr phKey);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptReleaseContext(IntPtr hProv, int dwFlags);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptSetKeyParam(IntPtr hKey, int dwParam, byte[] pbData, int dwFlags);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptSetKeyParam(IntPtr hKey, int dwParam, ref int pbData, int dwFlags);

        //[DllImport(@"advapi32.dll", SetLastError = true)]
        //internal static extern int CryptSetKeyParam(IntPtr hKey, int dwParam, ref CRYPT_DATA_BLOB pbData, int dwFlags);
        [DllImport(@"kernel32.dll", EntryPoint = "FormatMessageA", CharSet = CharSet.Ansi)]
        internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, uint dwMessageId, int dwLanguageId,
            out IntPtr lpBuffer, int nSize, IntPtr Arguments);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LocalFree(IntPtr hMem);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll")]
        internal static extern int SCardEstablishContext(int dwScope, IntPtr pvReserved1, IntPtr pvReserved2,
            out IntPtr phContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll")]
        internal static extern int SCardBeginTransaction(IntPtr hCard);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2101:SpecifyMarshalingForPInvokeStringArguments")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll", EntryPoint = "SCardConnectA", CharSet = CharSet.Ansi)]
        internal static extern int SCardConnect(IntPtr hContext, [MarshalAs(UnmanagedType.LPStr)] string szReader,
            int dwShareMode, int dwPreferredProtocols, out IntPtr phCard, out int pdwActiveProtocol);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll")]
        internal static extern int SCardDisconnect(IntPtr hCard, int dwDisposition);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll")]
        internal static extern int SCardEndTransaction(IntPtr hCard, int dwDisposition);

        [DllImport("winscard.dll")]
        internal static extern int SCardFreeMemory(IntPtr hContext, IntPtr pvMem);

        [DllImport("winscard.dll")]
        internal static extern int SCardFreeMemory(IntPtr hContext, byte* pvMem);

        [DllImport("winscard.dll")]
        internal static extern int SCardGetAttrib(IntPtr hCard, int dwAttrId, out IntPtr pbAttr, ref int pcbAttrLen);

        [DllImport("winscard.dll")]
        internal static extern int SCardGetAttrib(IntPtr hCard, int dwAttrId, out uint pbAttr, ref int pcbAttrLen);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll", CharSet = CharSet.Auto)]
        internal static extern int SCardGetStatusChange(IntPtr hContext, int dwTimeout,
            [In] [Out] SCARD_READERSTATE[] rgReaderStates, int cReaders);

        //[DllImport("winscard.dll", EntryPoint = "SCardListReadersA", CharSet = CharSet.Ansi)]
        //internal static extern int SCardListReaders(IntPtr hContext, IntPtr mszGroups, out IntPtr mszReaders, ref int pcchReaders);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll", EntryPoint = "SCardListReadersA", CharSet = CharSet.Ansi)]
        internal static extern int SCardListReaders(IntPtr hContext, IntPtr mszGroups, out byte* mszReaders,
            ref int pcchReaders);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll")]
        internal static extern int SCardReconnect(IntPtr hCard, int dwShareMode, int dwPreferredProtocols,
            int dwInitialization, out int pdwActiveProtocol);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll")]
        internal static extern int SCardReleaseContext(IntPtr hContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll")]
        internal static extern int SCardStatus(IntPtr hCard, out IntPtr szReaderName, ref int pcchReaderLen,
            out int pdwState, out int pdwProtocol, byte[] pbAtr, ref int pcbAtrLen);

        //[DllImport("winscard.dll")]
        //internal static extern int SCardTransmit(IntPtr hCard, ref SCARD_IO_REQUEST pioSendPci, byte[] pbSendBuffer, int cbSendLength, IntPtr pioRecvPci, ref IntPtr pbRecvBuffer, ref int pcbRecvLength);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("winscard.dll")]
        internal static extern int SCardTransmit(IntPtr hCard, ref SCARD_IO_REQUEST pioSendPci, byte[] pbSendBuffer,
            int cbSendLength, IntPtr pioRecvPci, byte[] pbRecvBuffer, ref int pcbRecvLength);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        internal static string FormatMessage(uint error)
        {
            IntPtr buffer;
            int ret = FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero,
                error, 0, out buffer, 0, IntPtr.Zero);
            if (ret == 0)
            {
                return "";
            }
            string message = Marshal.PtrToStringAnsi(buffer, ret);
            LocalFree(buffer);
            return message.Trim('\r', '\n');
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CREDENTIAL
    {
        public int Flags;
        public int Type;
        [MarshalAs(UnmanagedType.LPWStr)] public string TargetName;
        [MarshalAs(UnmanagedType.LPWStr)] public string Comment;
        public long LastWritten;
        public int CredentialBlobSize;
        public byte* CredentialBlob;
        public int Persist;
        public int AttributeCount;
        public IntPtr Attributes;
        [MarshalAs(UnmanagedType.LPWStr)] public string TargetAlias;
        [MarshalAs(UnmanagedType.LPWStr)] public string UserName;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CREDENTIAL_ATTRIBUTE
    {
        [MarshalAs(UnmanagedType.LPWStr)] public string Keyword;
        public int Flags;
        public int ValueSize;
        public IntPtr Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PUBLICKEYSTRUC
    {
        public byte bType;
        public byte bVersion;
        public short reserved;
        public int aiKeyAlg;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct CRYPT_DATA_BLOB
    {
        //CRYPT_DATA_BLOB, CRYPTOAPI_BLOB
        public int cbData;
        public IntPtr pbData;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct PROV_ENUMALGS_EX
    {
        public int aiAlgid;
        public int dwDefaultLen;
        public int dwMinLen;
        public int dwMaxLen;
        public int dwProtocols;
        public int dwNameLen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)] public string szName;
        public int dwLongNameLen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)] public string szLongName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct SCARD_READERSTATE
    {
        [MarshalAs(UnmanagedType.LPTStr)] public string szReader;
        public IntPtr pvUserData;
        public int dwCurrentState;
        public int dwEventState;
        public int cbAtr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)] public byte[] rgbAtr;
    }

    internal struct SCARD_IO_REQUEST
    {
        public SCARD_IO_REQUEST(int protocol, int length)
        {
            this.dwProtocol = protocol;
            this.cbPciLength = length;
        }

        public int dwProtocol;
        public int cbPciLength;
    }

    internal enum CryptoAlgorithm
    {
        Rijndael128 = NativeMethods.CALG_AES_128,
        Rijndael192 = NativeMethods.CALG_AES_192,
        Rijndael256 = NativeMethods.CALG_AES_256,
        RC4 = NativeMethods.CALG_RC4
    }

    internal enum CryptoProvider
    {
        RsaFull = NativeMethods.PROV_RSA_FULL,
        RsaAes = NativeMethods.PROV_RSA_AES
    }

    internal enum CryptoMethod
    {
        Encrypt,
        Decrypt
    }

    /// <summary>
    ///     LLCryptoLib.Security.Win32 is the place for digital
    ///     certificates related native methods for Windows platform.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}
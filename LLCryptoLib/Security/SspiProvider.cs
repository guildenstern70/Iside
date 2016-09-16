using System;
using System.Runtime.InteropServices;

//using SecLib.Ssl;

namespace LLCryptoLib.Security
{
    /// <summary>
    ///     Defines the external methods of the CryptoAPI.
    /// </summary>
    internal sealed class SspiProvider
    {
        private SspiProvider()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertCloseStore(IntPtr hCertStore, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern IntPtr CertFindCertificateInStore(IntPtr hCertStore, int dwCertEncodingType,
            int dwFindFlags, int dwFindType, IntPtr pvFindPara, IntPtr pPrevCertContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", EntryPoint = "CertFindCertificateInStore")]
        internal static extern IntPtr CertFindDataBlobCertificateInStore(IntPtr hCertStore, int dwCertEncodingType,
            int dwFindFlags, int dwFindType, ref DataBlob pvFindPara, IntPtr pPrevCertContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", EntryPoint = "CertFindCertificateInStore")]
        internal static extern IntPtr CertFindStringCertificateInStore(IntPtr hCertStore, int dwCertEncodingType,
            int dwFindFlags, int dwFindType, [MarshalAs(UnmanagedType.LPWStr)] string pvFindPara,
            IntPtr pPrevCertContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertFreeCertificateContext(IntPtr pCertContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern IntPtr CertDuplicateCertificateContext(IntPtr pCertContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern IntPtr CertDuplicateStore(IntPtr hCertStore);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr PFXImportCertStore(ref DataBlob pPFX, string szPassword, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", CharSet = CharSet.Unicode)]
        internal static extern int PFXVerifyPassword(ref DataBlob pPFX, string szPassword, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int PFXIsPFXBlob(ref DataBlob pPFX);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2101:SpecifyMarshalingForPInvokeStringArguments")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", CharSet = CharSet.Ansi)]
        internal static extern IntPtr CertOpenStore(IntPtr lpszStoreProvider, int dwMsgAndCertEncodingType,
            int hCryptProv, int dwFlags, string pvPara);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", EntryPoint = "CertOpenStore")]
        internal static extern IntPtr CertOpenStoreData(IntPtr lpszStoreProvider, int dwMsgAndCertEncodingType,
            IntPtr hCryptProv, int dwFlags, ref DataBlob pvPara);

        [DllImport(@"crypt32.dll")]
        internal static extern int CertGetCertificateContextProperty(IntPtr pCertContext, int dwPropId, byte[] pvData,
            ref int pcbData);

        [DllImport(@"crypt32.dll")]
        internal static extern int CertGetCertificateContextProperty(IntPtr pCertContext, int dwPropId, IntPtr pvData,
            ref int pcbData);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", EntryPoint = "CertGetNameStringA")]
        internal static extern int CertGetNameString(IntPtr pCertContext, int dwType, int dwFlags, IntPtr pvTypePara,
            IntPtr pszNameString, int cchNameString);

        [DllImport(@"advapi32.dll", SetLastError = true)]
        internal static extern int CryptExportKey(int hKey, int hExpKey, int dwBlobType, int dwFlags, IntPtr pbData,
            ref int pdwDataLen);

        [DllImport(@"advapi32.dll", SetLastError = true)]
        internal static extern int CryptExportKey(int hKey, int hExpKey, int dwBlobType, int dwFlags, byte[] pbData,
            ref int pdwDataLen);

        [DllImport(@"advapi32.dll", EntryPoint = "CryptAcquireContextA", CharSet = CharSet.Ansi, SetLastError = true)]
        // do not remove SetLastError
        internal static extern int CryptAcquireContext(ref int phProv, IntPtr pszContainer, string pszProvider,
            int dwProvType, int dwFlags);

        [DllImport(@"advapi32.dll", EntryPoint = "CryptAcquireContextA", CharSet = CharSet.Ansi, SetLastError = true)]
        // do not remove SetLastError
        internal static extern int CryptAcquireContext(ref int phProv, string pszContainer, string pszProvider,
            int dwProvType, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptReleaseContext(int hProv, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", EntryPoint = "CertFindCertificateInStore")]
        internal static extern IntPtr CertFindUsageCertificateInStore(IntPtr hCertStore, int dwCertEncodingType,
            int dwFindFlags, int dwFindType, ref TrustListUsage pvFindPara, IntPtr pPrevCertContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertVerifyTimeValidity(IntPtr pTimeToVerify, IntPtr pCertInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2101:SpecifyMarshalingForPInvokeStringArguments")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", EntryPoint = "CertFindExtension", CharSet = CharSet.Ansi)]
        internal static extern IntPtr CertFindExtension([MarshalAs(UnmanagedType.LPStr)] string pszObjId,
            int cExtensions, IntPtr rgExtensions);

        [DllImport(@"crypt32.dll")]
        internal static extern int CryptDecodeObject(int dwCertEncodingType, IntPtr lpszStructType, IntPtr pbEncoded,
            int cbEncoded, int dwFlags, IntPtr pvStructInfo, ref int pcbStructInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertGetPublicKeyLength(int dwCertEncodingType, IntPtr pPublicKey);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2101:SpecifyMarshalingForPInvokeStringArguments")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", CharSet = CharSet.Ansi)]
        internal static extern IntPtr CertFindRDNAttr(string pszObjId, IntPtr pName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertGetIntendedKeyUsage(int dwCertEncodingType, IntPtr pCertInfo, IntPtr pbKeyUsage,
            int cbKeyUsage);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertGetEnhancedKeyUsage(IntPtr pCertContext, int dwFlags, IntPtr pUsage,
            ref int pcbUsage);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertGetCertificateChain(IntPtr hChainEngine, IntPtr pCertContext, IntPtr pTime,
            IntPtr hAdditionalStore, ref ChainParameters pChainPara, int dwFlags, IntPtr pvReserved,
            ref IntPtr ppChainContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern void CertFreeCertificateChain(IntPtr pChainContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertVerifyCertificateChainPolicy(IntPtr pszPolicyOID, IntPtr pChainContext,
            ref ChainPolicyParameters pPolicyPara, ref ChainPolicyStatus pPolicyStatus);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern IntPtr CertGetIssuerCertificateFromStore(IntPtr hCertStore, IntPtr pSubjectContext,
            IntPtr pPrevIssuerContext, ref int pdwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CryptAcquireCertificatePrivateKey(IntPtr pCert, int dwFlags, IntPtr pvReserved,
            ref int phCryptProv, ref int pdwKeySpec, ref int pfCallerFreeProv);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertGetValidUsages(int cCerts, IntPtr rghCerts, ref int cNumOIDs, IntPtr rghOIDs,
            ref int pcbOIDs);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", SetLastError = true)] // do not remove SetLastError
        internal static extern int CertAddCertificateContextToStore(IntPtr hCertStore, IntPtr pCertContext,
            int dwAddDisposition, IntPtr ppStoreContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertDeleteCertificateFromStore(IntPtr pCertContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", CharSet = CharSet.Unicode)]
        internal static extern int PFXExportCertStoreEx(IntPtr hStore, ref DataBlob pPFX, string szPassword,
            IntPtr pvReserved, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertCompareCertificate(int dwCertEncodingType, IntPtr pCertId1, IntPtr pCertId2);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertSaveStore(IntPtr hCertStore, int dwMsgAndCertEncodingType, int dwSaveAs,
            int dwSaveTo, ref DataBlob pvSaveToPara, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern IntPtr CertCreateCertificateContext(int dwCertEncodingType, IntPtr pbCertEncoded,
            int cbCertEncoded);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "1")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGenKey(int hProv, IntPtr Algid, int dwFlags, ref int phKey);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptDestroyKey(int hKey);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptImportKey(int hProv, IntPtr pbData, int dwDataLen, int hPubKey, int dwFlags,
            ref int phKey);

        [DllImport(@"advapi32.dll", SetLastError = true)]
        internal static extern int CryptImportKey(int hProv, byte[] pbData, int dwDataLen, int hPubKey, int dwFlags,
            ref int phKey);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGetKeyParam(int hKey, int dwParam, ref int pbData, ref int pdwDataLen,
            int dwFlags);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGetKeyParam(int hKey, int dwParam, byte[] pbData, ref int pdwDataLen,
            int dwFlags);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGetKeyParam(int hKey, int dwParam, ref IntPtr pbData, ref int pdwDataLen,
            int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGetProvParam(int hProv, int dwParam, IntPtr pbData, ref int pdwDataLen,
            int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll")]
        internal static extern int CryptGenRandom(int hProv, int dwLen, IntPtr pbBuffer);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptSetKeyParam(int hKey, int dwParam, byte[] pbData, int dwFlags);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptSetKeyParam(int hKey, int dwParam, ref int pbData, int dwFlags);

        [DllImport(@"advapi32.dll", SetLastError = true)]
        internal static extern int CryptSetKeyParam(int hKey, int dwParam, ref DataBlob pbData, int dwFlags);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptEncrypt(int hKey, int hHash, int Final, int dwFlags, IntPtr pbData,
            ref int pdwDataLen, int dwBufLen);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptEncrypt(int hKey, int hHash, int Final, int dwFlags, byte[] pbData,
            ref int pdwDataLen, int dwBufLen);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptEncrypt(IntPtr hKey, int hHash, int Final, int dwFlags, byte[] pbData,
            ref int pdwDataLen, int dwBufLen);

        [DllImport(@"advapi32.dll")]
        internal static extern int CryptDecrypt(int hKey, int hHash, int Final, int dwFlags, byte[] pbData,
            ref int pdwDataLen);

        [DllImport(@"advapi32.dll", SetLastError = true)]
        internal static extern int CryptDecrypt(IntPtr hKey, int hHash, int Final, int dwFlags, byte[] pbData,
            ref int pdwDataLen);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CryptFindCertificateKeyProvInfo(IntPtr pCert, int dwFlags, IntPtr pvReserved);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CryptImportPublicKeyInfoEx(int hCryptProv, int dwCertEncodingType,
            ref CERT_PUBLIC_KEY_INFO pInfo, int aiKeyAlg, int dwFlags, IntPtr pvAuxInfo, ref int phKey);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptCreateHash")]
        internal static extern int CryptCreateHash(int hProv, int Algid, int hKey, int dwFlags, out int phHash);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptDuplicateHash")]
        internal static extern int CryptDuplicateHash(int hHash, IntPtr pdwReserved, int dwFlags, out int phHash);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptDestroyHash")]
        internal static extern int CryptDestroyHash(int hHash);

        [DllImport(@"advapi32.dll", EntryPoint = "CryptHashData")]
        internal static extern int CryptHashData(int hHash, byte[] pbData, int dwDataLen, int dwFlags);

        [DllImport(@"advapi32.dll", EntryPoint = "CryptHashData")]
        internal static extern int CryptHashData(int hHash, IntPtr pbData, int dwDataLen, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptGetHashParam")]
        internal static extern int CryptGetHashParam(int hHash, int dwParam, byte[] pbData, ref int pdwDataLen,
            int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"crypt32.dll", EntryPoint = "CryptProtectData", CharSet = CharSet.Unicode)]
        internal static extern int CryptProtectData(ref DataBlob pDataIn, string szDataDescr,
            ref DataBlob pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DataBlob pDataOut);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"crypt32.dll", EntryPoint = "CryptUnprotectData", CharSet = CharSet.Unicode)]
        internal static extern int CryptUnprotectData(ref DataBlob pDataIn, IntPtr ppszDataDescr,
            ref DataBlob pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DataBlob pDataOut);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptSetHashParam")]
        internal static extern int CryptSetHashParam(int hHash, int dwParam, byte[] pbData, int dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"crypt32.dll", EntryPoint = "CryptImportPublicKeyInfo")]
        internal static extern int CryptImportPublicKeyInfo(int hCryptProv, int dwCertEncodingType,
            ref CERT_PUBLIC_KEY_INFO pInfo, out int phKey);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptVerifySignatureA", CharSet = CharSet.Ansi)]
        internal static extern int CryptVerifySignature(int hHash, byte[] pbSignature, int dwSigLen, int hPubKey,
            IntPtr sDescription, int dwFlags);

        [DllImport(@"crypt32.dll")]
        internal static extern int CryptDecodeObject(int dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded,
            int cbEncoded, int dwFlags, IntPtr pvStructInfo, ref int pcbStructInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptSignHash")]
        internal static extern int CryptSignHash(int hHash, int dwKeySpec, IntPtr sDescription, int dwFlags,
            byte[] pbSignature, ref int pdwSigLen);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", EntryPoint = "CertSetCertificateContextProperty")]
        internal static extern int CertSetCertificateContextProperty(IntPtr pCertContext, int dwPropId, int dwFlags,
            ref CRYPT_KEY_PROV_INFO pvData);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll", EntryPoint = "CertStrToNameW", CharSet = CharSet.Unicode)]
        internal static extern int CertStrToName(int dwCertEncodingType, string pszX500, int dwStrType,
            IntPtr pvReserved, IntPtr pbEncoded, ref int pcbEncoded, IntPtr ppszError);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertAddStoreToCollection(IntPtr hCollectionStore, IntPtr hSiblingStore,
            int dwUpdateFlag, int dwPriority);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern void CertRemoveStoreFromCollection(IntPtr hCollectionStore, IntPtr hSiblingStore);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
             "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"advapi32.dll", EntryPoint = "CryptGetUserKey", CharSet = CharSet.Ansi)]
        internal static extern int CryptGetUserKey(int hProv, int dwKeySpec, ref int phUserKey);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern IntPtr CertCreateCRLContext(int dwCertEncodingType, byte[] pbCrlEncoded, int cbCrlEncoded);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertVerifyCRLRevocation(int dwCertEncodingType, IntPtr pCertId, int cCrlInfo,
            ref IntPtr rgpCrlInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertFreeCRLContext(IntPtr pCrlContext);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(@"crypt32.dll")]
        internal static extern int CertFindCertificateInCRL(IntPtr pCert, IntPtr pCrlContext, int dwFlags,
            IntPtr pvReserved, ref IntPtr ppCrlEntry);

        //[DllImport(@"crypt32.dll")]	
        //internal static extern int CryptImportPublicKeyInfo(IntPtr hCryptProv, int dwCertEncodingType, IntPtr pInfo, ref IntPtr phKey);

        //[DllImport(@"kernel32.dll", EntryPoint="RtlMoveMemory")]	
        //internal static extern void CopyMemory(IntPtr Destination, ref IntPtr Source, int Length);

        //[DllImport(@"crypt32.dll", EntryPoint="CertNameToStrA", CharSet=CharSet.Ansi)]
        //internal static extern int CertNameToStr(int dwCertEncodingType, ref DataBlob pName, int dwStrType, IntPtr psz, int csz);

        //[DllImport(@"crypt32.dll", EntryPoint="CertNameToStrA", CharSet=CharSet.Ansi)]
        //internal static extern int CertNameToStr(int dwCertEncodingType, IntPtr pName, int dwStrType, IntPtr psz, int csz);

        //[DllImport(@"crypt32.dll", EntryPoint="CryptDecodeObject", CharSet=CharSet.Ansi)]
        //internal static extern int CryptDecodeObject(int dwCertEncodingType, [MarshalAs(UnmanagedType.LPStr)] string lpszStructType, IntPtr pbEncoded, int cbEncoded, int dwFlags, IntPtr pvStructInfo, ref int pcbStructInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2101:SpecifyMarshalingForPInvokeStringArguments")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(@"crypt32.dll", EntryPoint = "CertOpenSystemStoreA", CharSet = CharSet.Ansi)]
        internal static extern IntPtr CertOpenSystemStore(int hProv, string szSubsystemProtocol);
    }
}
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
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using LLCryptoLib.Security;

namespace LLCryptoLib.Security.Cryptography
{
    internal class CAPIProvider
    {
        internal CAPIProvider() { }
        public static int Handle
        {
            get
            {
                m_Provider.CreateInternalHandle(ref m_Provider.m_Handle, null);
                return m_Provider.m_Handle;
            }
        }
        public static int ContainerHandle
        {
            get
            {
                m_Provider.CreateInternalHandle(ref m_Provider.m_ContainerHandle, SecurityConstants.KEY_CONTAINER);
                return m_Provider.m_ContainerHandle;
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public void CreateInternalHandle(ref int handle, string container)
        {
            if (handle == 0)
            {
                lock (this)
                {
                    if (handle == 0 && !m_Error)
                    {
                        int flags = 0;
                        if (!Environment.UserInteractive && Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
                            flags = SecurityConstants.CRYPT_SILENT | SecurityConstants.CRYPT_MACHINE_KEYSET;
                        for (int i = 0; i < m_Providers.Length; i++)
                        {
                            if (SspiProvider.CryptAcquireContext(ref handle, container, null, m_Providers[i], flags) == 0)
                            {
                                if (Marshal.GetLastWin32Error() == SecurityConstants.NTE_BAD_KEYSET)
                                {
                                    SspiProvider.CryptAcquireContext(ref handle, container, null, m_Providers[i], flags | SecurityConstants.CRYPT_NEWKEYSET);
                                }
                            }
                            if (handle != 0)
                                break;
                        }
                        if (handle == 0)
                            m_Error = true;
                    }
                    if (m_Error)
                        throw new CryptographicException("Couldn't acquire crypto service provider context.");
                }
            }
        }
        ~CAPIProvider()
        {
            if (m_Handle != 0)
                SspiProvider.CryptReleaseContext(m_Handle, 0);
            if (m_ContainerHandle != 0)
                SspiProvider.CryptReleaseContext(m_ContainerHandle, 0);
        }
        private int m_Handle = 0;
        private int m_ContainerHandle = 0;
        private bool m_Error = false;
        private static int[] m_Providers = new int[] { SecurityConstants.PROV_RSA_AES, SecurityConstants.PROV_RSA_FULL };
        private static CAPIProvider m_Provider = new CAPIProvider();
    }
    /// <summary>
    /// Specifies the type of encryption method to use when protecting data.
    /// </summary>
    public enum ProtectionType
    {
        /// <summary>The encrypted data is associated with the local machine. Any user on the computer on which the data is encrypted can decrypt the data.</summary>
        LocalMachine,
        /// <summary>The encrypted data is associated with the current user. Only a user with logon credentials matching those of the encrypter can decrypt the data.</summary>
        CurrentUser
    }
    /// <summary>
    /// Specifies the type of algorithm to be used when performing unmanaged cryptographic transformations.
    /// </summary>
    internal enum CryptoAlgorithm : int
    {
        /// <summary>The Rijndael algorithm with a key size of 128 bits.</summary>
        Rijndael128 = SecurityConstants.CALG_AES_128,
        /// <summary>The Rijndael algorithm with a key size of 192 bits.</summary>
        Rijndael192 = SecurityConstants.CALG_AES_192,
        /// <summary>The Rijndael algorithm with a key size of 256 bits.</summary>
        Rijndael256 = SecurityConstants.CALG_AES_256,
        /// <summary>The RC4 algorithm.</summary>
        RC4 = SecurityConstants.CALG_RC4
    }
    /// <summary>
    /// Specifies the type of CSP to be used when performing unmanaged cryptographic transformations.
    /// </summary>
    internal enum CryptoProvider
    {
        /// <summary>Microsoft's full RSA CSP.</summary>
        RsaFull = SecurityConstants.PROV_RSA_FULL,
        /// <summary>Microsoft's full RSA CSP that supports the AES.</summary>
        RsaAes = SecurityConstants.PROV_RSA_AES
    }
    /// <summary>
    /// Specifies the type of transformation for a cryptographic operation.
    /// </summary>
    internal enum CryptoMethod
    {
        /// <summary>Encrypt the data.</summary>
        Encrypt,
        /// <summary>Decrypt the data.</summary>
        Decrypt
    }
    /// <summary>
    /// The PUBLICKEYSTRUC structure, also known as the BLOBHEADER structure, indicates a key's BLOB type and the algorithm that the key uses. One of these structures is located at the beginning of the pbData member of every key BLOB.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PUBLICKEYSTRUC
    {
        /// <summary>Key BLOB type. The only BLOB types currently defined are PUBLICKEYBLOB, PRIVATEKEYBLOB, SIMPLEBLOB, and PLAINTEXTBLOB. Other key BLOB types will be defined as needed. </summary>
        public byte bType;
        /// <summary>Version number of the key BLOB format. This currently must always have a value of CUR_BLOB_VERSION (0x02).</summary>
        public byte bVersion;
        /// <summary>WORD reserved for future use. Must be set to zero.</summary>
        public short reserved;
        /// <summary>Algorithm identifier for the key contained by the key BLOB. Some examples are CALG_RSA_SIGN, CALG_RSA_KEYX, CALG_RC2, and CALG_RC4.</summary>
        public IntPtr aiKeyAlg;
    }
}
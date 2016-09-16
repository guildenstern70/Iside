using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LLCryptoLib.Security.Cryptography
{
    internal class CAPIProvider
    {
        private static readonly int[] m_Providers = {SecurityConstants.PROV_RSA_AES, SecurityConstants.PROV_RSA_FULL};
        private static readonly CAPIProvider m_Provider = new CAPIProvider();
        private int m_ContainerHandle;
        private bool m_Error;
        private int m_Handle;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public void CreateInternalHandle(ref int handle, string container)
        {
            if (handle == 0)
            {
                lock (this)
                {
                    if ((handle == 0) && !this.m_Error)
                    {
                        int flags = 0;
                        if (!Environment.UserInteractive && (Environment.OSVersion.Platform == PlatformID.Win32NT) &&
                            (Environment.OSVersion.Version.Major >= 5))
                            flags = SecurityConstants.CRYPT_SILENT | SecurityConstants.CRYPT_MACHINE_KEYSET;
                        for (int i = 0; i < m_Providers.Length; i++)
                        {
                            if (SspiProvider.CryptAcquireContext(ref handle, container, null, m_Providers[i], flags) ==
                                0)
                            {
                                if (Marshal.GetLastWin32Error() == SecurityConstants.NTE_BAD_KEYSET)
                                {
                                    SspiProvider.CryptAcquireContext(ref handle, container, null, m_Providers[i],
                                        flags | SecurityConstants.CRYPT_NEWKEYSET);
                                }
                            }
                            if (handle != 0)
                                break;
                        }
                        if (handle == 0)
                            this.m_Error = true;
                    }
                    if (this.m_Error)
                        throw new CryptographicException("Couldn't acquire crypto service provider context.");
                }
            }
        }

        ~CAPIProvider()
        {
            if (this.m_Handle != 0)
                SspiProvider.CryptReleaseContext(this.m_Handle, 0);
            if (this.m_ContainerHandle != 0)
                SspiProvider.CryptReleaseContext(this.m_ContainerHandle, 0);
        }
    }

    /// <summary>
    ///     Specifies the type of encryption method to use when protecting data.
    /// </summary>
    public enum ProtectionType
    {
        /// <summary>
        ///     The encrypted data is associated with the local machine. Any user on the computer on which the data is
        ///     encrypted can decrypt the data.
        /// </summary>
        LocalMachine,

        /// <summary>
        ///     The encrypted data is associated with the current user. Only a user with logon credentials matching those of
        ///     the encrypter can decrypt the data.
        /// </summary>
        CurrentUser
    }

    /// <summary>
    ///     Specifies the type of algorithm to be used when performing unmanaged cryptographic transformations.
    /// </summary>
    internal enum CryptoAlgorithm
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
    ///     Specifies the type of CSP to be used when performing unmanaged cryptographic transformations.
    /// </summary>
    internal enum CryptoProvider
    {
        /// <summary>Microsoft's full RSA CSP.</summary>
        RsaFull = SecurityConstants.PROV_RSA_FULL,

        /// <summary>Microsoft's full RSA CSP that supports the AES.</summary>
        RsaAes = SecurityConstants.PROV_RSA_AES
    }

    /// <summary>
    ///     Specifies the type of transformation for a cryptographic operation.
    /// </summary>
    internal enum CryptoMethod
    {
        /// <summary>Encrypt the data.</summary>
        Encrypt,

        /// <summary>Decrypt the data.</summary>
        Decrypt
    }

    /// <summary>
    ///     The PUBLICKEYSTRUC structure, also known as the BLOBHEADER structure, indicates a key's BLOB type and the algorithm
    ///     that the key uses. One of these structures is located at the beginning of the pbData member of every key BLOB.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PUBLICKEYSTRUC
    {
        /// <summary>
        ///     Key BLOB type. The only BLOB types currently defined are PUBLICKEYBLOB, PRIVATEKEYBLOB, SIMPLEBLOB, and
        ///     PLAINTEXTBLOB. Other key BLOB types will be defined as needed.
        /// </summary>
        public byte bType;

        /// <summary>Version number of the key BLOB format. This currently must always have a value of CUR_BLOB_VERSION (0x02).</summary>
        public byte bVersion;

        /// <summary>WORD reserved for future use. Must be set to zero.</summary>
        public short reserved;

        /// <summary>
        ///     Algorithm identifier for the key contained by the key BLOB. Some examples are CALG_RSA_SIGN, CALG_RSA_KEYX,
        ///     CALG_RC2, and CALG_RC4.
        /// </summary>
        public IntPtr aiKeyAlg;
    }
}
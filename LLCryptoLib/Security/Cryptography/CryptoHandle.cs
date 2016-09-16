using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using LLCryptoLib.Security.Resources;
using LLCryptoLib.Security.Win32;
using Microsoft.Win32;

namespace LLCryptoLib.Security.Cryptography
{
    internal class CryptoHandle
    {
        private static readonly int[] m_Providers = {NativeMethods.PROV_RSA_AES, NativeMethods.PROV_RSA_FULL};
        private static readonly CryptoHandle m_Provider = new CryptoHandle();
        private static int m_RequiresFips = -1;
        private bool m_Error;
        private IntPtr m_Handle;
        private int m_HandleProviderType;

        public static IntPtr Handle
        {
            get
            {
                m_Provider.CreateInternalHandle(ref m_Provider.m_Handle, null);
                return m_Provider.m_Handle;
            }
        }

        public static int HandleProviderType
        {
            get
            {
                m_Provider.CreateInternalHandle(ref m_Provider.m_Handle, null);
                return m_Provider.m_HandleProviderType;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2106:SecureAsserts")]
        internal static bool PolicyRequiresFips
        {
            get
            {
                // we do not require our callers to have a RegistryPermission
                RegistryPermission regPerm = new RegistryPermission(RegistryPermissionAccess.Read,
                    @"System\CurrentControlSet\Control\Lsa\FIPSAlgorithmPolicy");
                regPerm.Assert();

                if (m_RequiresFips == -1)
                {
                    m_RequiresFips = 0;
                    using (
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Lsa",
                            false))
                    {
                        if (key != null)
                        {
                            object val = key.GetValue("FIPSAlgorithmPolicy");
                            if (val != null)
                            {
                                m_RequiresFips = (int) val;
                            }
                        }
                    }
                }
                return m_RequiresFips == 1;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
             "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public void CreateInternalHandle(ref IntPtr handle, string container)
        {
            if (handle == IntPtr.Zero)
            {
                lock (this)
                {
                    if ((handle == IntPtr.Zero) && !this.m_Error)
                    {
                        int flags, fs = 0, fmk = 0;
                        if (!Environment.UserInteractive && (Environment.OSVersion.Platform == PlatformID.Win32NT) &&
                            (Environment.OSVersion.Version.Major >= 5))
                        {
                            fs = NativeMethods.CRYPT_SILENT;
                            fmk = NativeMethods.CRYPT_MACHINE_KEYSET;
                        }
                        for (int i = 0; i < m_Providers.Length; i++)
                        {
                            flags = fs | fmk;
                            this.m_HandleProviderType = m_Providers[i];
                            if (NativeMethods.CryptAcquireContext(ref handle, container, null, m_Providers[i], flags) ==
                                0)
                            {
                                if (Marshal.GetLastWin32Error() == NativeMethods.NTE_BAD_KEYSET)
                                {
                                    NativeMethods.CryptAcquireContext(ref handle, container, null, m_Providers[i],
                                        flags | NativeMethods.CRYPT_NEWKEYSET);
                                }
                                else if (fmk != 0)
                                {
                                    flags = fs;
                                    if (
                                        NativeMethods.CryptAcquireContext(ref handle, container, null, m_Providers[i],
                                            flags) == 0)
                                    {
                                        if (Marshal.GetLastWin32Error() == NativeMethods.NTE_BAD_KEYSET)
                                        {
                                            NativeMethods.CryptAcquireContext(ref handle, container, null,
                                                m_Providers[i], flags | NativeMethods.CRYPT_NEWKEYSET);
                                        }
                                    }
                                }
                            }
                            if (handle != IntPtr.Zero)
                                break;
                        }
                        if (handle == IntPtr.Zero)
                        {
                            this.m_Error = true;
                            this.m_HandleProviderType = 0;
                        }
                    }
                    if (this.m_Error)
                        throw new CryptographicException(ResourceController.GetString("Error_AcquireCSP"));
                }
            }
        }


        ~CryptoHandle()
        {
            if (this.m_Handle != IntPtr.Zero)
                NativeMethods.CryptReleaseContext(this.m_Handle, 0);
        }
    }
}
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
using System.Security.Permissions;
using LLCryptoLib.Security.Resources;
using LLCryptoLib.Security.Win32;
using Microsoft.Win32;

namespace LLCryptoLib.Security.Cryptography
{
    internal class CryptoHandle
    {
        internal CryptoHandle() { }
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public void CreateInternalHandle(ref IntPtr handle, string container)
        {
            if (handle == IntPtr.Zero)
            {
                lock (this)
                {
                    if (handle == IntPtr.Zero && !m_Error)
                    {
                        int flags, fs = 0, fmk = 0;
                        if (!Environment.UserInteractive && Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
                        {
                            fs = NativeMethods.CRYPT_SILENT;
                            fmk = NativeMethods.CRYPT_MACHINE_KEYSET;
                        }
                        for (int i = 0; i < m_Providers.Length; i++)
                        {
                            flags = fs | fmk;
                            m_HandleProviderType = m_Providers[i];
                            if (NativeMethods.CryptAcquireContext(ref handle, container, null, m_Providers[i], flags) == 0)
                            {
                                if (Marshal.GetLastWin32Error() == NativeMethods.NTE_BAD_KEYSET)
                                {
                                    NativeMethods.CryptAcquireContext(ref handle, container, null, m_Providers[i], flags | NativeMethods.CRYPT_NEWKEYSET);
                                }
                                else if (fmk != 0)
                                {
                                    flags = fs;
                                    if (NativeMethods.CryptAcquireContext(ref handle, container, null, m_Providers[i], flags) == 0)
                                    {
                                        if (Marshal.GetLastWin32Error() == NativeMethods.NTE_BAD_KEYSET)
                                        {
                                            NativeMethods.CryptAcquireContext(ref handle, container, null, m_Providers[i], flags | NativeMethods.CRYPT_NEWKEYSET);
                                        }
                                    }
                                }
                            }
                            if (handle != IntPtr.Zero)
                                break;
                        }
                        if (handle == IntPtr.Zero)
                        {
                            m_Error = true;
                            m_HandleProviderType = 0;
                        }
                    }
                    if (m_Error)
                        throw new CryptographicException(ResourceController.GetString("Error_AcquireCSP"));
                }
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2106:SecureAsserts")]
        internal static bool PolicyRequiresFips
        {
            get
            {
                // we do not require our callers to have a RegistryPermission
                RegistryPermission regPerm = new RegistryPermission(RegistryPermissionAccess.Read, @"System\CurrentControlSet\Control\Lsa\FIPSAlgorithmPolicy");
                regPerm.Assert();

                if (m_RequiresFips == -1)
                {
                    m_RequiresFips = 0;
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Lsa", false))
                    {
                        if (key != null)
                        {
                            object val = key.GetValue("FIPSAlgorithmPolicy");
                            if (val != null)
                            {
                                m_RequiresFips = (int)val;
                            }
                        }
                    }
                }
                return m_RequiresFips == 1;
            }
        }


        ~CryptoHandle()
        {
            if (m_Handle != IntPtr.Zero)
                NativeMethods.CryptReleaseContext(m_Handle, 0);
        }
        private IntPtr m_Handle;
        private bool m_Error;
        private int m_HandleProviderType;
        private static int[] m_Providers = new int[] { NativeMethods.PROV_RSA_AES, NativeMethods.PROV_RSA_FULL };
        private static CryptoHandle m_Provider = new CryptoHandle();
        private static int m_RequiresFips = -1;
    }
}
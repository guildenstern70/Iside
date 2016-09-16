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
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Win32
{
    internal class Platform
    {
        private Platform() { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void AssertWinXP()
        {
            if (m_OS.Version.Major < 5 || (m_OS.Version.Major == 5 && m_OS.Version.Minor < 1))
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, ResourceController.GetString("Error_UnsupportedOS"), "Windows XP"));
        }
        /*public static void AssertWin2000() {
            if (m_OS.Version.Major < 5)
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, ResourceController.GetString("Error_UnsupportedOS"), "Windows 2000"));
        }*/

        private static OperatingSystem m_OS = Environment.OSVersion;
    }
}
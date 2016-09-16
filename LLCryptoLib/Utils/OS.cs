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

namespace LLCryptoLib.Utils
{
	/// <summary>
	/// Operating System class, returns a description
    /// of the running operating system (name, version).
	/// </summary>
	public static class OS
	{
		
		/// <summary>
		/// Return the Operating System information
		/// </summary>
		public static string OperatingSystem
		{
			get
			{
				return OS.GetOperationSystemInformation();
			}
		}

		private static string GetOperationSystemInformation()
		{  
			System.OperatingSystem m_os = System.Environment.OSVersion;  
			string m_osName = "Unknown";  

			switch(m_os.Platform) 
			{   
				case System.PlatformID.Win32Windows:     
				switch(m_os.Version.Minor)     
				{       
					case 0:         
						m_osName = "Windows 95";
						break;       
					case 10:         
						m_osName = "Windows 98";         
						break;       
					case 90:         
						m_osName = "Windows ME";         
						break;     
				}     
					break;   

				case System.PlatformID.Win32NT:     
				switch(m_os.Version.Major)     
				{       
					case 3:         
						m_osName = "Windws NT 3.51";
						break;       
					case 4:         
						m_osName = "Windows NT 4"; 
						break;       
					case 5:         
						if(m_os.Version.Minor == 0)
							m_osName = "Windows 2000"; 
						else if(m_os.Version.Minor == 1)
							m_osName = "Windows XP";
						else if(m_os.Version.Minor == 2)       
							m_osName = "Windows Server 003";         
						break;       
					case 6:          
						m_osName = "Vista";         
						break;     
				}     
					break;  
			}  
			return m_osName + ", " + m_os.Version.ToString();
		}
	}
}

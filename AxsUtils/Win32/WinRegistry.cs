/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All rights reserved
 * 
 * AxsUtils.Win32.WinRegistry.cs
 * 
 */

using System;
using Microsoft.Win32;
using System.Security.Permissions;

namespace AxsUtils.Win32
{
   
	/// <summary>
	/// Writes/Read from registry
	/// </summary>
	public sealed class WinRegistry
	{

		private WinRegistry() {}

		/// <summary>
		/// Create an HKCR key. Does not produce errors if the key exists.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void CreateHKCRKey(string key)
		{
			try
			{
				Registry.ClassesRoot.CreateSubKey(key);
				System.Diagnostics.Debug.WriteLine("Created key "+key);
			}
			catch (Exception exc)
			{
				Console.WriteLine("AxsUtils.WinRegistry.CreateHKCRKey -> "+exc.Message);
			}
		}

		/// <summary>
		/// Delete HKLM key and all of its subkeys
		/// </summary>
		/// <param name="key">Example: Software\LittleLite Software\NCrypt</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKLMKeyAll(string key)
		{
			try
			{
				Registry.LocalMachine.DeleteSubKeyTree(key);
				System.Diagnostics.Debug.WriteLine("Deleted key "+key+" WITH ALL SUBKEYS");
			}
			catch(Exception exc)
			{
                Console.WriteLine("AxsUtils.WinRegistry.DeleteHKLMKeyAll -> Can't delete key: " + key + " -> " + exc.Message);
			}
		}


		/// <summary>
		/// Delete HKLM key without deleting key's subkeys
		/// </summary>
		/// <param name="key">Example: Software\LittleLite Software\NCrypt</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKLMKey(string key)
		{
			try
			{
				if (Registry.LocalMachine.OpenSubKey(key)!=null)
				{
					Registry.LocalMachine.DeleteSubKey(key);
					System.Diagnostics.Debug.WriteLine("Deleted key "+key);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Can't delete key "+key+": it does not exist.");
				}
				
			}
			catch(Exception exc)
			{
                Console.WriteLine("AxsUtils.WinRegistry.DeleteHKLMKey -> " + exc.Message);
			}
		}

		/// <summary>
		///  Delete an HKLM value
		/// </summary>
		/// <param name="keypath">Key name, Example: Software\LittleLite Software\NCrypt</param>
		/// <param name="keyValue">Key value: Example: Size</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKLMValue(string keypath, string keyValue)
		{
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keypath, true))
            {
                try
                {
                    key.DeleteValue(keyValue, true);
                    System.Diagnostics.Debug.WriteLine("Deleted value " + keyValue + " on key " + key.Name);
                }
                catch (Exception exc)
                {
                    if (key != null)
                    {
                        Console.WriteLine("Can't delete value " + keyValue + " on key " + key.Name + " -> " + exc.Message);
                    }
                    else
                    {
                        Console.WriteLine("AxsUtils.WinRegistry.DeleteHKCUValue -> " + exc.Message);
                    }
                }
            }
		}

		/// <summary>
		///  Delete an HKLM value
		/// </summary>
		/// <param name="keypath">Key name, Example: Software\LittleLite Software\NCrypt</param>
		/// <param name="keyValue">Key value: Example: Size</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKCUValue(string keypath, string keyValue)
		{
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath, true))
            {
                try
                {
                    key.DeleteValue(keyValue, true);
                    System.Diagnostics.Debug.WriteLine("Deleted value "+keyValue+" on key "+key.Name);
                }
                catch (Exception exc)
                {
                    if (key!=null)
				    {
					    Console.WriteLine("Can't delete value "+keyValue+" on key "+key.Name+" -> "+exc.Message);
				    }
				    else
				    {
                        Console.WriteLine("AxsUtils.WinRegistry.DeleteHKCUValue -> " + exc.Message);
				    }
                }
            }
		}

		/// <summary>
		/// Deletes a key in HKCR
		/// </summary>
		/// <param name="key">Example: Software\LittleLite Software\NCrypt</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKCRKey(string key)
		{
			try
			{
				Registry.ClassesRoot.DeleteSubKey(key);
				System.Diagnostics.Debug.WriteLine("Deleted key "+key);
			}
			catch(Exception exc)
			{
				Console.WriteLine("AxsUtils.WinRegistry.DeleteHKCRKey -> "+exc.Message);
			}

		}


		/// <summary>
		/// Deletes a key in HKCU
		/// </summary>
		/// <param name="key">Example: Software\LittleLite Software\NCrypt</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKCUKey(string key)
		{
			try
			{
				Registry.CurrentUser.DeleteSubKey(key);
				System.Diagnostics.Debug.WriteLine("Deleted key "+key);
			}
			catch(Exception exc)
			{
				Console.WriteLine("AxsUtils.WinRegistry.DeleteHKLMKey -> "+exc.Message);
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="keypath"></param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string[] GetHKLMValues(string keypath)
		{
			string[] subkeys = null;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keypath, false))
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Querying key " + key.Name);
                    if (key != null)
                    {
                        subkeys = key.GetValueNames();
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("AxsUtils.WinRegistry.GetHKLMValues -> " + exc.Message);
                }
            }

			return subkeys;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keypath"></param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string[] GetHKCUValues(string keypath)
		{
			string[] subkeys = null;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath,false))
            {
			    try
			    {
				    System.Diagnostics.Debug.WriteLine("Querying key "+key.Name);
				    if (key!=null)
				    {
					    subkeys = key.GetValueNames();
				    }
			    }
			    catch (Exception exc)
			    {
				    Console.WriteLine("AxsUtils.WinRegistry.GetHKLMValues -> "+exc.Message);
			    }
            }

			return subkeys;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="keypath"></param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string[] GetHKCUSubkeys(string keypath)
		{
			string[] subkeys = null;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath,false))
            {
			    try
			    {
				    System.Diagnostics.Debug.WriteLine("Querying key "+key.Name);
				    if (key!=null)
				    {
					    subkeys = key.GetSubKeyNames();
				    }
			    }
			    catch (Exception exc)
			    {
				    Console.WriteLine("AxsUtils.WinRegistry.GetHKLMValues -> "+exc.Message);
			    }
            }

			return subkeys;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="keypath"></param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string[] GetHKLMSubkeys(string keypath)
		{
			string[] subkeys = null;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keypath,false))
            {
			    try
			    {
				    System.Diagnostics.Debug.WriteLine("Querying key "+key.Name);
				    if (key!=null)
				    {
					    subkeys = key.GetSubKeyNames();
				    }
			    }
			    catch (Exception exc)
			    {
				    Console.WriteLine("AxsUtils.WinRegistry.GetHKLMValues -> "+exc.Message);
			    }
            }

			return subkeys;
		}

		/// <summary>
		/// Get the value of a key in a registry HKCR (classes) subkey
		/// </summary>
		/// <param name="keypath">Path of subkey, ie: Folder</param>
		/// <param name="keyname">Name of the key</param>
		/// <param name="name">Name of the value</param>
		/// <returns>The string if correctly read, null else</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string GetHKCRValue(string keypath, string keyname, string name)
		{
			string retval = null;

            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(keypath, false))
            {
			    try
			    {
				    if (key!=null)
				    {
                        using (RegistryKey downkey = key.OpenSubKey(keyname, false))
                        {
                            if (downkey != null)
                            {
                                System.Diagnostics.Debug.WriteLine("Opening HKCR key " + downkey.Name);
                                object tmpVal = downkey.GetValue(name);
                                if (tmpVal != null)
                                {
                                    retval = tmpVal.ToString();
                                    System.Diagnostics.Debug.WriteLine(downkey.Name + " = " + retval);
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Warning: HKCR key " + key.Name + " does not contain " + keyname);
                            }
                        }
				    }
    #if (DEBUG)		
				    else
				    {
					    Console.WriteLine("key {0} is NULL",keypath);
				    }
    #endif
			    }
			    catch (Exception exc)
			    {
				    Console.WriteLine("AxsUtils.WinRegistry.GetHKCRValue -> "+exc.Message);
			    }
            }

			return retval;
		}

        public static byte[] GetHKLMBlob(string keyname, string name)
        {
            byte[] retval = null;

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", false);
                System.Diagnostics.Debug.WriteLine(@"Trying to open HKLM\Software\" + keyname + @"\" + name);
                if (key != null)
                {
                    using (RegistryKey downkey = key.OpenSubKey(keyname, false))
                    {
                        if (downkey != null)
                        {
                            object tmpVal = downkey.GetValue(name);
                            if (tmpVal != null)
                            {
                                retval = (byte[])tmpVal;
                                System.Diagnostics.Debug.WriteLine(downkey.Name + ": " + name + " bytes retrieved.");
                            }
#if DEBUG
                            else
                            {
                                Console.WriteLine("Can't find value {0} in this key", name);
                            }
#endif
                        }
#if DEBUG
                        else
                        {
                            Console.WriteLine(keyname + ": does not exist");
                        }
#endif
                    }

                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.GetHKLMBlob -> " + exc.Message);
            }

            return retval;

        }

		/// <summary>
		/// Get the value of a key in a registry HKLM\Software subkey
		/// </summary>
		/// <param name="keyname">Name of the key</param>
		/// <param name="name">Name of the value</param>
		/// <returns>The string if correctly read, null else</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string GetHKLMValue(string keyname, string name)
		{
			string retval = null;

			try
			{
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", false);
				System.Diagnostics.Debug.WriteLine(@"Trying to open HKLM\Software\"+keyname+@"\"+name);
				if (key!=null)
				{
                    using (RegistryKey downkey = key.OpenSubKey(keyname, false))
                    {
                        if (downkey != null)
                        {
                            object tmpVal = downkey.GetValue(name);
                            if (tmpVal != null)
                            {
                                retval = tmpVal.ToString();
                                System.Diagnostics.Debug.WriteLine(downkey.Name + ": " + name + " = " + retval);
                            }
#if DEBUG
                            else
                            {
                                Console.WriteLine("Can't find value {0} in this key", name);
                            }
#endif
                        }
#if DEBUG
                        else
                        {
                            Console.WriteLine(keyname + ": does not exist");
                        }
#endif
                    }

				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("AxsUtils.WinRegistry.GetHKLMValue -> "+exc.Message);
			}

			return retval;
		}

		/// <summary>
		/// Get the value of a key in a registry "HKCU (Current User)\Software" subkey
		/// </summary>
		/// <param name="keyname">Name of the key</param>
		/// <param name="name">Name of the value</param>
		/// <returns>The string if correctly read, null else</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string GetHKCUValue(string keyname, string name)
		{
			string retval = null;

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", false);
                System.Diagnostics.Debug.WriteLine("Reading key " + key.Name);
                if (key != null)
                {
                    using (RegistryKey downkey = key.OpenSubKey(keyname, false))
                    {
                        if (downkey != null)
                        {
                            object tmpVal = downkey.GetValue(name);
                            if (tmpVal != null)
                            {
                                retval = tmpVal.ToString();
                                System.Diagnostics.Debug.WriteLine(downkey.Name + ": " + name + " = " + retval);
                            }
#if DEBUG
                            else
                            {
                                Console.WriteLine("Can't find value {0} in this key ", name);
                            }
#endif
                        }
                    }
                }
            }
            catch (NullReferenceException) { }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.GetHKCUValue -> " + exc.Message);
            }

			return retval;
		}

		/// <summary>
		/// Get the value of a key in a registry "HKCU (Current User)\Software" subkey
		/// </summary>
		/// <param name="keyname">Name of the key</param>
		/// <param name="name">Name of the value</param>
		/// <returns>The number if correctly read, null else</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static int GetHKCUDWORDValue(string keyname, string name)
		{
			int retval = Int32.MinValue; 
            string retStrVal;

			try
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey("Software",false);
				System.Diagnostics.Debug.WriteLine("Reading key "+key.Name);
				if (key!=null)
				{

                    using (RegistryKey downkey = key.OpenSubKey(keyname, false))
                    {
                        if (downkey != null)
                        {
                            retStrVal = downkey.GetValue(name).ToString();
                            retval = Int32.Parse(retStrVal);
                            System.Diagnostics.Debug.WriteLine(downkey.Name + ": " + name + " = " + retval);
                        }
#if DEBUG
                        else
                        {
                            Console.WriteLine("Can't find value {0} in this key", name);
                        }
#endif
                    }
				}
				
			}
			catch (Exception exc)
			{
				Console.WriteLine("AxsUtils.WinRegistry.GetHKCUDWORDValue -> "+exc.Message);
			}

			return retval;
		}

		/// <summary>
		/// Adds a keypair to an existing key under the HKCR branch.
		/// If any error (ie: key does not exist) return false.
		/// </summary>
		/// <param name="keypath">Path of an EXISTING key: ie: Folder</param>
		/// <param name="keyname">Name of the key </param>
		/// <param name="name"></param>
		/// <param name="valkey"></param>
		/// <returns>True if key is correctly set.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool SetHKCRValue(string keypath, string keyname, string name, string valkey)
		{
			bool okok=true;

			try
			{
				// Open existring key under HKEY_CLASSES_ROOT\Software 
				RegistryKey key = Registry.ClassesRoot.OpenSubKey(keypath, true); 
				System.Diagnostics.Debug.WriteLine("Opening key "+key.ToString());
				if (key!=null)
				{
					// Add one more sub key 
                    using (RegistryKey newkey = key.OpenSubKey(keyname, true))
                    {
                        // Set value of sub key 
                        if (newkey != null)
                        {
                            newkey.SetValue(name, valkey);
                            System.Diagnostics.Debug.WriteLine(newkey.Name + " => " + valkey);
                        }
#if DEBUG
                        else
                        {
                            Console.WriteLine("Can't find {0}", newkey.Name);
                        }
#endif
                    }
				}
			}
			catch (Exception exc)
			{
				okok=false;
				Console.WriteLine("AxsUtils.WinRegistry.SetHKCRValue -> "+exc.Message);
			}

			return okok;
		}

        public static bool SetHKLMBlob(string keyname, string name, byte[] valkey)
        {
            bool okok = true;

            try
            {
                // Open existring key under HKEY_LOCAL_MACHINE\Software 
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
                System.Diagnostics.Debug.WriteLine("Writing to key " + key.ToString());
                if (key != null)
                {
                    // Add one more sub key 
                    RegistryKey newkey = key.OpenSubKey(keyname, true);
                    if (newkey == null)
                    {
                        key.CreateSubKey(keyname);
                        System.Diagnostics.Debug.WriteLine(newkey.Name + " => Created");
                    }
                    // Set value of sub key 
                    newkey.SetValue(name, valkey, RegistryValueKind.Binary);
                    System.Diagnostics.Debug.WriteLine(newkey.Name + " => Binary Written ");
                }
            }
            catch (Exception exc)
            {
                okok = false;
                Console.WriteLine("AxsUtils.WinRegistry.SetHKLMValue -> " + exc.Message);
            }

            return okok;

        }

		/// <summary>
		/// Adds a keypair to an existing key under the HKLM\Software branch.
		/// If any error (ie: key does not exist) return false.
		/// </summary>
		/// <param name="keyname">Name of an existing key</param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns>True if key is correctly set.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool SetHKLMValue(string keyname, string name, string valkey)
		{
			bool okok=true;

			try
			{
				// Open existring key under HKEY_LOCAL_MACHINE\Software 
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true); 
				System.Diagnostics.Debug.WriteLine("Writing to key "+key.ToString());
				if (key!=null)
				{
					// Add one more sub key 
                    RegistryKey newkey = key.OpenSubKey(keyname, true);
                    if (newkey == null)
                    {
                        newkey = key.CreateSubKey(keyname);
                        System.Diagnostics.Debug.WriteLine(newkey.Name + " => Created");
                    }
                    // Set value of sub key 
                    newkey.SetValue(name, valkey);
                    System.Diagnostics.Debug.WriteLine(newkey.Name + " => " + valkey);
				}
			}
			catch (Exception exc)
			{
				okok=false;
				Console.WriteLine("AxsUtils.WinRegistry.SetHKLMValue -> "+exc.Message);
			}

			return okok;
		}

		/// <summary>
		/// Adds a keypair to an existing key under the HKCU (Current User)\Software branch.
		/// If any error return false.
		/// If key does not exist, it will be created.
		/// </summary>
		/// <param name="keyname">Name of an existing key</param>
		/// <param name="name">Name of key-value pair</param>
		/// <param name="value">Value to associate to this key</param>
		/// <returns>True if key is correctly set.</returns>
		public static bool SetHKCUValue(string keyname, string name, string valkey)
		{
			bool okok=true;

			try
			{
				// Open existring key under HKEY_CURRENT_USER\Software 
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true); 
				System.Diagnostics.Debug.WriteLine("Writing to key "+key.ToString());
				if (key!=null)
				{
					// Add one more sub key 
                    RegistryKey newkey = key.OpenSubKey(keyname, true);
                    // Set value of sub key. Create the key if necessary
                    if (newkey == null)
                    {
                        newkey = key.CreateSubKey(keyname);
                        System.Diagnostics.Debug.WriteLine(newkey.Name + " => Created");
                    }
                    newkey.SetValue(name, valkey);
                    System.Diagnostics.Debug.WriteLine(newkey.Name + " => " + valkey);
				}
			}
			catch (Exception exc)
			{
				okok=false;
				Console.WriteLine("AxsUtils.WinRegistry.SetHKCUValue -> "+exc.Message);
			}

			return okok;
		}

		/// <summary>
		/// Adds a keypair to an existing key under the HKCU (Current User)\Software branch.
		/// If any error return false.
		/// If key does not exist, it will be created.
		/// </summary>
		/// <param name="keyname">Name of an existing key</param>
		/// <param name="name">Name of key-value pair</param>
		/// <param name="value">Value to associate to this key</param>
		/// <returns>True if key is correctly set.</returns>
		public static bool SetHKCUValue(string keyname, string name, int valkey)
		{
			bool okok=true;

			try
			{
				// Open existring key under HKEY_CURRENT_USER\Software 
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true); 
				System.Diagnostics.Debug.WriteLine("Writing to key "+key.ToString());
				if (key!=null)
				{
					// Add one more sub key 
                    RegistryKey newkey = key.OpenSubKey(keyname, true);
                    // Set value of sub key. Create the key if necessary
                    if (newkey == null)
                    {
                        newkey = key.CreateSubKey(keyname);
                        System.Diagnostics.Debug.WriteLine(newkey.Name + " => Created");
                    }
                    newkey.SetValue(name, valkey);
                    System.Diagnostics.Debug.WriteLine(newkey.Name + " => " + valkey);
				}
			}
			catch (Exception exc)
			{
				okok=false;
				Console.WriteLine("AxsUtils.WinRegistry.SetHKCUValue -> "+exc.Message);
			}

			return okok;
		}

		/// <summary>
		/// Get the human-readable file type of this extension
		/// </summary>
		/// <param name="fileExtension"></param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string GetFileType(string fileExtension)
		{
            if (fileExtension == null)
            {
                throw new ArgumentNullException("fileExtension");
            }

			RegistryKey objExtReg = Registry.ClassesRoot; 
			RegistryKey objAppReg = Registry.ClassesRoot; 
			string strExtValue = null; 
			try 
			{ 
				//  Add trailing period if doesn't exist
				if ( fileExtension.Substring( 0, 1 ) != "." ) 
				{ 
					fileExtension = "." + fileExtension; 
				} 
				//  Open registry areas containing launching app details
				objExtReg = objExtReg.OpenSubKey( fileExtension.Trim() ); 
				strExtValue = System.Convert.ToString( objExtReg.GetValue( "" ) ); 
				return objAppReg.OpenSubKey(strExtValue).GetValue("").ToString();
			} 
			catch  
			{ 
				return ""; 
			} 

		}

		/// <summary>
		/// Get the associated program
		/// </summary>
		/// <param name="FileExtension"></param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string GetAssociatedProgram( string fileExtension ) 
		{
            if (fileExtension == null)
            {
                throw new ArgumentNullException("fileExtension");
            }

			RegistryKey objExtReg = Registry.ClassesRoot; 
			RegistryKey objAppReg = Registry.ClassesRoot; 
			string strExtValue = null; 
			try 
			{ 
				//  Add trailing period if doesn't exist
				if ( fileExtension.Substring( 0, 1 ) != "." ) 
				{ 
					fileExtension = "." + fileExtension; 
				} 
				//  Open registry areas containing launching app details
				objExtReg = objExtReg.OpenSubKey( fileExtension.Trim() ); 
				strExtValue = System.Convert.ToString( objExtReg.GetValue( "" ) ); 
				objAppReg = objAppReg.OpenSubKey( strExtValue + @"\shell\open\command" ); 
				//  Parse out, tidy up and return result
				string app = System.Convert.ToString( objAppReg.GetValue( null ) ); 
				string[] SplitArray = app.Split('"');
				if ( SplitArray[ 0 ].Trim().Length > 0 ) 
				{ 
					return SplitArray[ 0 ].Replace( "%1", "" ); 
				} 
				else 
				{ 
					return SplitArray[ 1 ].Replace( "%1", "" ); 
				} 
			} 
			catch  
			{ 
				return "Unknown"; 
			} 
		} 

	}
}

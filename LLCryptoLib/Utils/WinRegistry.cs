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
using Microsoft.Win32;

namespace LLCryptoLib.Utils
{
    /// <summary>
    /// WinRegistry class Writes/Read data from registry.
    /// </summary>
    public static class WinRegistry
    {

        /// <summary>
        /// Check if HKLM key exist
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool HKLMExists(string key)
        {
            bool exist = false;

            try
            {
                if (Registry.LocalMachine.OpenSubKey(key) != null)
                {
                    exist = true;
                }
            }
            catch { }

            return exist;
        }

        /// <summary>
        /// Check if HKCU key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool HKCUExists(string key)
        {
            bool exist = false;

            try
            {
                if (Registry.CurrentUser.OpenSubKey(key) != null)
                {
                    exist = true;
                }
            }
            catch { }

            return exist;
        }

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
                System.Diagnostics.Debug.WriteLine("Created key " + key);
            }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.CreateHKCRKey -> " + exc.Message);
            }
        }

        /// <summary>
        /// Delete HKLM key and all of its subkeys
        /// </summary>
        /// <param name="key">Example: SOFTWARE\LittleLite Software\NCrypt</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKLMKeyAll(string key)
        {
            try
            {
                Registry.LocalMachine.DeleteSubKeyTree(key);
                System.Diagnostics.Debug.WriteLine("Deleted key " + key + " WITH ALL SUBKEYS");
            }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.DeleteHKLMKeyAll -> Can't delete key: " + key + " -> " + exc.Message);
            }
        }


        /// <summary>
        /// Delete HKLM key without deleting key's subkeys
        /// </summary>
        /// <param name="key">Example: SOFTWARE\LittleLite Software\NCrypt</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKLMKey(string key)
        {
            try
            {
                if (Registry.LocalMachine.OpenSubKey(key) != null)
                {
                    Registry.LocalMachine.DeleteSubKey(key);
                    System.Diagnostics.Debug.WriteLine("Deleted key " + key);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Can't delete key " + key + ": it does not exist.");
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.DeleteHKLMKey -> " + exc.Message);
            }
        }

        /// <summary>
        ///  Delete an HKLM value
        /// </summary>
        /// <param name="keypath">Key name, Example: SOFTWARE\LittleLite Software\NCrypt</param>
        /// <param name="keyValue">Key value: Example: Size</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKLMValue(string keypath, string keyValue)
        {
            RegistryKey key = null;

            try
            {
                key = Registry.LocalMachine.OpenSubKey(keypath, false);
                key.DeleteValue(keyValue, true);
                System.Diagnostics.Debug.WriteLine("Deleted value " + keyValue + " on key " + key.Name);
            }
            catch (Exception exc)
            {
                if (key != null)
                {
                    Console.WriteLine("Can't delete value: " + keyValue + " on key " + key.Name + " -> " + exc.Message);
                }
                else
                {
                    Console.WriteLine("AxsUtils.WinRegistry.DeleteHKLMValue -> " + exc.Message);
                }
            }
        }

        /// <summary>
        ///  Delete an HKLM value
        /// </summary>
        /// <param name="keypath">Key name, Example: SOFTWARE\LittleLite Software\NCrypt</param>
        /// <param name="keyValue">Key value: Example: Size</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKCUValue(string keypath, string keyValue)
        {
            RegistryKey key = null;

            try
            {
                key = Registry.CurrentUser.OpenSubKey(keypath, false);
                key.DeleteValue(keyValue, true);
                System.Diagnostics.Debug.WriteLine("Deleted value " + keyValue + " on key " + key.Name);
            }
            catch (Exception exc)
            {
                if (key != null)
                {
                    Console.WriteLine("Can't delete value: " + keyValue + " on key " + key.Name + " -> " + exc.Message);
                }
                else
                {
                    Console.WriteLine("AxsUtils.WinRegistry.DeleteHKCUValue -> " + exc.Message);
                }
            }
        }

        /// <summary>
        /// Deletes a key in HKCR
        /// </summary>
        /// <param name="key">Example: SOFTWARE\LittleLite Software\NCrypt</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKCRKey(string key)
        {
            try
            {
                Registry.ClassesRoot.DeleteSubKey(key);
                System.Diagnostics.Debug.WriteLine("Deleted key " + key);
            }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.DeleteHKCRKey -> " + exc.Message);
            }

        }


        /// <summary>
        /// Deletes a key in HKCU
        /// </summary>
        /// <param name="key">Example: SOFTWARE\LittleLite Software\NCrypt</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void DeleteHKCUKey(string key)
        {
            try
            {
                Registry.CurrentUser.DeleteSubKey(key);
                System.Diagnostics.Debug.WriteLine("Deleted key " + key);
            }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.DeleteHKLMKey -> " + exc.Message);
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

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(keypath, false);
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

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath, false);
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

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath, false);
                System.Diagnostics.Debug.WriteLine("Querying key " + key.Name);
                if (key != null)
                {
                    subkeys = key.GetSubKeyNames();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.GetHKLMValues -> " + exc.Message);
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

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(keypath, false);
                System.Diagnostics.Debug.WriteLine("Querying key " + key.Name);
                if (key != null)
                {
                    subkeys = key.GetSubKeyNames();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.GetHKLMValues -> " + exc.Message);
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

            try
            {
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(keypath, false);
                if (key != null)
                {
                    System.Diagnostics.Debug.WriteLine("Opening HKCR key " + key.Name);
                    RegistryKey downkey = key.OpenSubKey(keyname, false);
                    if (downkey != null)
                    {
                        object tmpVal = downkey.GetValue(name);
                        if (tmpVal != null)
                        {
                            retval = tmpVal.ToString();
                            System.Diagnostics.Debug.WriteLine(downkey.Name + " = " + retval);
                        }
                    }
                }
#if (DEBUG)
                else
                {
                    Console.WriteLine("key {0} is NULL", keypath);
                }
#endif
            }
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.GetHKCRValue -> " + exc.Message);
            }

            return retval;
        }

        /// <summary>
        /// Gets the HKCU BLOB.
        /// </summary>
        /// <param name="keyname">The keyname.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static byte[] GetHKCUBlob(string keyname, string name)
        {
            byte[] retval = null;

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE", false);
                System.Diagnostics.Debug.WriteLine(@"Trying to open HKLM\Software\" + keyname + @"\" + name);
                if (key != null)
                {
                    RegistryKey downkey = key.OpenSubKey(keyname, false);
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
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.GetHKLMBlob -> " + exc.Message);
            }

            return retval;

        }

        /// <summary>
        /// Gets the HKLM BLOB.
        /// </summary>
        /// <param name="keyname">The keyname.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static byte[] GetHKLMBlob(string keyname, string name)
        {
            byte[] retval = null;

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE", false);
                System.Diagnostics.Debug.WriteLine(@"Trying to open HKLM\Software\" + keyname + @"\" + name);
                if (key != null)
                {
                    RegistryKey downkey = key.OpenSubKey(keyname, false);
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
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE", false);
                System.Diagnostics.Debug.WriteLine(@"Trying to open HKLM\Software\" + keyname + @"\" + name);
                if (key != null)
                {
                    RegistryKey downkey = key.OpenSubKey(keyname, false);
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
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.GetHKLMValue -> " + exc.Message);
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
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE", false);
                System.Diagnostics.Debug.WriteLine("Reading key " + key.Name);
                if (key != null)
                {
                    RegistryKey downkey = key.OpenSubKey(keyname, false);
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
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", false);
                System.Diagnostics.Debug.WriteLine("Reading key " + key.Name);
                if (key != null)
                {

                    RegistryKey downkey = key.OpenSubKey(keyname, false);
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
            catch (Exception exc)
            {
                Console.WriteLine("AxsUtils.WinRegistry.GetHKCUDWORDValue -> " + exc.Message);
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
            bool okok = true;

            try
            {
                // Open existring key under HKEY_CLASSES_ROOT\Software 
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(keypath, true);
                System.Diagnostics.Debug.WriteLine("Opening key " + key.ToString());
                if (key != null)
                {
                    // Add one more sub key 
                    RegistryKey newkey = key.OpenSubKey(keyname, true);
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
            catch (Exception exc)
            {
                okok = false;
                Console.WriteLine("AxsUtils.WinRegistry.SetHKCRValue -> " + exc.Message);
            }

            return okok;
        }

        /// <summary>
        /// Sets the HKLM BLOB.
        /// </summary>
        /// <param name="keyname">The keyname.</param>
        /// <param name="name">The name.</param>
        /// <param name="valkey">The valkey.</param>
        /// <returns></returns>
        public static bool SetHKLMBlob(string keyname, string name, byte[] valkey)
        {
            bool okok = true;

            try
            {
                // Open existring key under HKEY_LOCAL_MACHINE\Software 
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                System.Diagnostics.Debug.WriteLine("Writing to key " + key.ToString());
                if (key != null)
                {
                    // Add one more sub key 
                    RegistryKey newkey = key.OpenSubKey(keyname, true);
                    if (newkey == null)
                    {
                        newkey = key.CreateSubKey(keyname);
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
        /// Sets the HKCU BLOB.
        /// </summary>
        /// <param name="keyname">The keyname.</param>
        /// <param name="name">The name.</param>
        /// <param name="valkey">The valkey.</param>
        /// <returns></returns>
        public static bool SetHKCUBlob(string keyname, string name, byte[] valkey)
        {
            bool okok = true;

            try
            {
                // Open existring key under HKEY_LOCAL_MACHINE\Software 
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                System.Diagnostics.Debug.WriteLine("Writing to key " + key.ToString());
                if (key != null)
                {
                    // Add one more sub key 
                    RegistryKey newkey = key.OpenSubKey(keyname, true);
                    if (newkey == null)
                    {
                        newkey = key.CreateSubKey(keyname);
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
        /// <param name="name">Name of the key value</param>
        /// <param name="valkey">Value to be set</param>
        /// <returns>True if key is correctly set.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool SetHKLMValue(string keyname, string name, string valkey)
        {
            bool okok = true;

            try
            {
                // Open existring key under HKEY_LOCAL_MACHINE\Software 
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                System.Diagnostics.Debug.WriteLine("Writing to key " + key.ToString());
                if (key != null)
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
                okok = false;
                Console.WriteLine("AxsUtils.WinRegistry.SetHKLMValue -> " + exc.Message);
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
        /// <param name="valkey">Value to associate to this key</param>
        /// <returns>True if key is correctly set.</returns>
        public static bool SetHKCUValue(string keyname, string name, string valkey)
        {
            bool okok = true;

            try
            {
                // Open existring key under HKEY_CURRENT_USER\Software 
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                System.Diagnostics.Debug.WriteLine("Writing to key " + key.ToString());
                if (key != null)
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
                okok = false;
                Console.WriteLine("AxsUtils.WinRegistry.SetHKCUValue -> " + exc.Message);
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
        /// <param name="valkey">Value to associate to this key</param>
        /// <returns>True if key is correctly set.</returns>
        public static bool SetHKCUValue(string keyname, string name, int valkey)
        {
            bool okok = true;

            try
            {
                // Open existring key under HKEY_CURRENT_USER\Software 
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                System.Diagnostics.Debug.WriteLine("Writing to key " + key.ToString());
                if (key != null)
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
                okok = false;
                Console.WriteLine("AxsUtils.WinRegistry.SetHKCUValue -> " + exc.Message);
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
                if (fileExtension.Substring(0, 1) != ".")
                {
                    fileExtension = "." + fileExtension;
                }
                //  Open registry areas containing launching app details
                objExtReg = objExtReg.OpenSubKey(fileExtension.Trim());
                strExtValue = System.Convert.ToString(objExtReg.GetValue(""));
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
        /// <param name="fileExtension">File extension (ie: .doc)</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static string GetAssociatedProgram(string fileExtension)
        {
            if (fileExtension == null)
            {
                throw new ArgumentNullException(fileExtension);
            }

            RegistryKey objExtReg = Registry.ClassesRoot;
            RegistryKey objAppReg = Registry.ClassesRoot;
            string strExtValue = null;
            try
            {
                //  Add trailing period if doesn't exist
                if (fileExtension.Substring(0, 1) != ".")
                {
                    fileExtension = "." + fileExtension;
                }
                //  Open registry areas containing launching app details
                objExtReg = objExtReg.OpenSubKey(fileExtension.Trim());
                strExtValue = System.Convert.ToString(objExtReg.GetValue(""));
                objAppReg = objAppReg.OpenSubKey(strExtValue + @"\shell\open\command");
                //  Parse out, tidy up and return result
                string app = System.Convert.ToString(objAppReg.GetValue(null));
                string[] SplitArray = app.Split('"');
                if (SplitArray[0].Trim().Length > 0)
                {
                    return SplitArray[0].Replace("%1", "");
                }
                else
                {
                    return SplitArray[1].Replace("%1", "");
                }
            }
            catch
            {
                return "Unknown";
            }
        }

   }
}

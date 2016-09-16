/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All rights reserved
 * 
 * AxsUtils.Win32.ClipboardUtils.cs
 * 
 */

using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AxsUtils.Win32
{

    /// <summary>
    /// Clipboard management
    /// </summary>
    public static class ClipboardUtils
    {
        /// <summary>
        /// Copy text string to clipboard
        /// </summary>
        /// <param name="what">The string to be copied into the clipboard</param>
        /// <exception cref="LLCryptoLibException">This method is not supported in Mono</exception>
        public static void Copy(String what)
        {
#if MONO
			throw new LLCryptoLibException("ClipboardUtils.Copy Not Supported in Mono");
#else
            try
            {
                Clipboard.SetDataObject(what, true);
            }
            catch { }
#endif
        }

        /// <summary>
        /// Return true if the clipboard contains something pasteable as a string
        /// </summary>
        /// <returns>true if the clipboard contains something pasteable</returns>
        /// <exception cref="LLCryptoLibException">This method is not supported in Mono</exception>
        public static bool CanPaste()
        {
#if MONO
			throw new LLCryptoLibException("ClipboardUtils.CanPatse() Not Supported in Mono");			
#else
            try
            {
                IDataObject iData = Clipboard.GetDataObject();
                if (iData.GetDataPresent(DataFormats.Text))
                {
                    return true;
                }
            }
            catch { }
            return false;
#endif
        }

        /// <summary>
        /// Get text string from clipboard
        /// </summary>
        /// <returns>text string from clipboard</returns>
        /// <exception cref="LLCryptoLibException">This method is not supported in Mono</exception>
        public static string Get()
        {
#if MONO
			throw new LLCryptoLibException("ClipboardUtils.Get Not Supported in Mono");
#else

            String what = String.Empty;
            try
            {
                if (ClipboardUtils.CanPaste())
                {
                    what = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
                }
            }
            catch { }
            return what;
#endif
        }
    }


}

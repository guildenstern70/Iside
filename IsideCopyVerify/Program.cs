/**
 **   IsideCopyVerify
 **   Confront files with a single click
 **
 **   Copyright © LittleLite Software
 **
 **/

using System;
using System.Windows.Forms;
using LLCryptoLib.Hash;
using Microsoft.Win32;
using System.Text;
using System.IO;

namespace IsideCopyVerify
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Run(args);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
#if DEBUG
                MessageBox.Show(null, exc.Message, "Iside", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(null, exc.ToString(), "Iside", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            }
        }

        static void Run(string[] args)
        {
#if DEBUG
            PrintArgs(args);
#endif
            if (args.Length == 2)
            {
                if (args[0] == "-copy")
                {
                    Program.Copy(args[1]);
                }
                else if (args[0] == "-verify")
                {
                    Program.Verify(args[1]);
                }
            }
        }

        static void PrintArgs(string[] args)
        {
            StringBuilder sb = new StringBuilder("IsideCopyVerify.exe ");
            foreach (string a in args)
            {
                sb.Append(a);
                sb.Append(' ');
            }
            MessageBox.Show(null, sb.ToString(), "Iside", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static void Verify(string filename)
        {
#if DEBUG          
            MessageBox.Show(null, "Verifying "+filename, "Iside", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
            Hash hashEngine = new Hash();
            hashEngine.SetAlgorithmInt(Program.DefaultAlgorithm);
            string result = hashEngine.CompareHashClipboardEx(filename);
            MessageBox.Show(null, result, "Iside", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static void Copy(string filePath)
        {
            if (File.Exists(filePath))
            {
                Hash hashEngine = new Hash();
                hashEngine.SetAlgorithmInt(Program.DefaultAlgorithm);
                hashEngine.CopyHashFile(filePath);
                MessageBox.Show(null, "Hash copied", "Iside", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(null, "File does not exist", "Iside", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        static int DefaultAlgorithm
        {
            get
            {
                int algo = 5; // default value

                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE", false);
                    if (key != null)
                    {
                        RegistryKey downkey = key.OpenSubKey(@"LittleLite Software\Iside", false);
                        object tmpVal = downkey.GetValue("DefaultHash");
                        string retval;
                        if (tmpVal != null)
                        {
                            retval = tmpVal.ToString();
                            System.Diagnostics.Debug.WriteLine("Default algorithm is " + retval);
                            algo = Int32.Parse(retval);
                        }
                    }
                }
                catch { }

                return algo;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace WPFUtils
{
    public sealed class Win32Window : System.Windows.Forms.IWin32Window
    {
        public Win32Window(IntPtr handle)
        {
            this.Handle = handle;
        }

        public IntPtr Handle
        {
            get;
            private set;
        }
    }

    public static class GUI
    {
        public static void SetCursorWait(bool isWait)
        {
            if (isWait)
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }
            else
            {
                Mouse.OverrideCursor = null;
            }
        }

        public static void SetHashResult(TextBox targetTextBox, string result)
        {
            if (targetTextBox != null)
            {
                string printableResult = result;
                if (result == "Hash algorithm not set.")
                {
                    printableResult = String.Empty;
                }
                targetTextBox.Text = printableResult;
            }
        }

        public static string SelectFolderPath(Window owner)
        {
            string initialFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            bool showNewFolderButton = false;
            return GUI.SelectFolderPath(owner, initialFolder, showNewFolderButton);
        }

        public static string SelectFolderPath(Window owner, bool showNewFolderButton)
        {
            string initialFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return GUI.SelectFolderPath(owner, initialFolder, showNewFolderButton);
        }

        public static string SelectFolderPath(Window owner, string initialFolder)
        {
            return GUI.SelectFolderPath(owner, initialFolder, false);
        }

        public static string SelectFolderPath(Window owner, string initialFolder, bool showNewFolderButton)
        {
            string folderPath = null;

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = showNewFolderButton;
                dialog.RootFolder = Environment.SpecialFolder.Desktop;
                dialog.SelectedPath = initialFolder;

                var wih = new WindowInteropHelper(owner);
                var w = new Win32Window(wih.Handle);

                if (dialog.ShowDialog(w) == System.Windows.Forms.DialogResult.OK)
                {
                    folderPath = dialog.SelectedPath;
                }
            }

            return folderPath;
        }

        public static void ComboBoxItemsAdd(ComboBox source, string[] items)
        {
            foreach (string s in items)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = s;
                source.Items.Add(cbi);
            }
        }
    }
}

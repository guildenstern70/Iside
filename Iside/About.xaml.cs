/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Iside
{
    /// <summary>
    /// About.xaml
    /// </summary>
    public partial class About : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="About" /> class.
        /// </summary>
        public About()
        {
            InitializeComponent();
            this.Init();
        }

        private void Init()
        {
            this.lblAppnameVersion.Text = IsideLogic.Config.APPNAME + " " + IsideLogic.Config.AppVersion;
            this.label3.Text = String.Format("Build {1} - CLR {0}", System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion(),
                                                  IsideLogic.Config.Build);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            string currentPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            System.Windows.Forms.Help.ShowHelp(null, currentPath + @"\help\Iside.chm");
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            
        }

    }
}

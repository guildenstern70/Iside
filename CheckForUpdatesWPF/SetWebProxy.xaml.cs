/**
    Check For Updates Utility - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CheckForUpdates;

namespace CheckForUpdatesWPF
{
    /// <summary>
    /// Interaction logic for SetWebProxy.xaml
    /// </summary>
    public partial class SetWebProxy : Window
    {
        private string applicationKey;
        private ProxyDetails proxyAddress;
        private string applicationName;

        public SetWebProxy(string appName, string appKey)
        {
            this.applicationName = appName;
            this.applicationKey = appKey;

            InitializeComponent();

            this.Init();
        }

        private void Init()
        {

            this.proxyAddress = new ProxyDetails(this.applicationName, this.applicationKey);
            this.DataContext = this.proxyAddress;

            if ((proxyAddress.ProxyUrl != null) && proxyAddress.ProxyUrl != "http://")
            {
                Debug.WriteLine("Proxy info found on registry: {0}", proxyAddress.ProxyUrl);
                this.txtAddress.Text = proxyAddress.ProxyUrl;
                this.chkUseProxy.IsChecked = true;
                this.chkLoginCredentials.IsEnabled = true;
                this.txtAddress.IsEnabled = true;
                this.txtPort.IsEnabled = true;

                if (proxyAddress.ProxyPort != null)
                {
                    if (this.txtAddress.Text.Length == 0)
                    {
                        this.txtPort.Text = "80";
                    }
                    else
                    {
                        this.txtPort.Text = proxyAddress.ProxyPort;
                    }
                }

                if (proxyAddress.ProxyAuthUsername != null)
                {
                    if (proxyAddress.ProxyAuthUsername.Length > 0)
                    {
                        this.chkLoginCredentials.IsChecked = true;
                        this.txtUsername.Text = proxyAddress.ProxyAuthUsername;
                        this.txtPassword.Text = proxyAddress.ProxyAuthPassword;
                    }
                }
            }
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            this.txtAddress.IsEnabled = true;
            this.txtPort.IsEnabled = true;
            this.chkLoginCredentials.IsEnabled = true;
        }

        private void chkProxyAuth_Checked(object sender, RoutedEventArgs e)
        {
            this.txtUsername.IsEnabled = true;
            this.txtPassword.IsEnabled = true;
        }

        private void chkProxy_Unchecked(object sender, RoutedEventArgs e)
        {
            this.txtAddress.IsEnabled = false;
            this.txtPort.IsEnabled = false;
            this.chkLoginCredentials.IsEnabled = false;
        }

        private void chkProxyAuth_Unchecked(object sender, RoutedEventArgs e)
        {
            this.txtUsername.IsEnabled = false;
            this.txtPassword.IsEnabled = false;
        }
    }
}

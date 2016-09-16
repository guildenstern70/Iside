/**
    Check For Updates Utility - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Diagnostics;
using System.Net;
using System.Security.Permissions;
using AxsUtils.Win32;
using CheckForUpdatesWPF;


namespace CheckForUpdates
{
	/// <summary>
	/// GetProxyAddr.
	/// </summary>
	public class ProxyDetails
	{
        private const string REG_PROXY_USES = "IsProxy";
        private const string REG_PROXY_AUTH = "IsProxyAuth";
		private const string REG_PROXY_URL = "ProxyUrl";
		private const string REG_PROXY_PORT = "ProxyPort";
		private const string REG_PROXY_USER = "ProxyUser";
		private const string REG_PROXY_PASSWORD = "ProxyPassword";

		private string applicationName;
		private string applicationKey;
        private string registryProxyKey;
        private bool useProxy;
        private bool isProxyAuthenticated;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProxyAddr"/> class.
        /// </summary>
        /// <param name="appName">Name of the app.</param>
        /// <param name="appKey">The app key.</param>
		public ProxyDetails(string appName, string appKey)
		{
			this.applicationName = appName;
			this.applicationKey = appKey;
            this.registryProxyKey = @"SOFTWARE\" + this.applicationKey;
		}

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <value>The proxy.</value>
        public System.Net.IWebProxy Proxy
        {
            get
            {
                WebProxy proxyObject;

                if (!this.UsesProxy)
                {
                    proxyObject = null;
                }
                else
                {
                    proxyObject = new WebProxy();
                    string proxyurlandport;
                    if (this.ProxyPort == null)
                    {
                        proxyurlandport = this.ProxyUrl;
                    }
                    else
                    {
                        proxyurlandport = String.Format("{0}:{1}", this.ProxyUrl, this.ProxyPort);
                    }
                    Uri proxyUri = new Uri(proxyurlandport);
                    proxyObject = new WebProxy(proxyUri);
                    if (this.IsProxyAuthenticated)
                    {
                        string username = this.ProxyAuthUsername;
                        string password = this.ProxyAuthPassword;
                        int indexDomain = username.IndexOf(@"\");

                        if (username.IndexOf(@"\") > 0)
                        {
                            string domain = username.Substring(0, indexDomain);
                            string user = username.Substring(indexDomain + 1);

                            proxyObject.Credentials = new NetworkCredential(user, this.ProxyAuthPassword, domain);
                        }
                        else
                        {
                            proxyObject.Credentials = new NetworkCredential(this.ProxyAuthUsername, this.ProxyAuthPassword);
                        }
                    }

                }

                return proxyObject;
            }
        }

        public bool UsesProxy
        {
            get
            {
                string uses = this.GetUserKey(REG_PROXY_USES);
                if (String.IsNullOrEmpty(uses))
                {
                    this.useProxy = false;
                }
                else
                {
                    if (uses.StartsWith("Y"))
                    {
                        this.useProxy = true;
                    }
                }
                return this.useProxy;
            }

            set
            {
                string registryString = "No";
                if (value)
                {
                    registryString = "Yes";
                }

                SaveUserKey(REG_PROXY_USES, registryString);
                Debug.WriteLine("Updated registry!");

                this.useProxy = value;
            }
        }

        public bool IsProxyAuthenticated
        {
            get
            {
                string isAuth = this.GetUserKey(REG_PROXY_AUTH);
                if (String.IsNullOrEmpty(isAuth))
                {
                    this.isProxyAuthenticated = false;
                }
                else
                {
                    if (isAuth.StartsWith("Y"))
                    {
                        this.isProxyAuthenticated = true;
                    }
                }

                return this.isProxyAuthenticated;
            }

            set
            {
                string registryString = "No";
                if (value)
                {
                    registryString = "Yes";
                }

                SaveUserKey(REG_PROXY_AUTH, registryString);
                Debug.WriteLine("Updated registry!");

                this.isProxyAuthenticated = value;

            }
        }

        /// <summary>
        /// Gets or sets the px URL.
        /// </summary>
        /// <value>The px URL.</value>
		public string ProxyUrl
		{
			get
			{
                Debug.WriteLine("Reading PROXY URL");
                return this.GetUserKey(REG_PROXY_URL);
			}

			set
            {
                Debug.WriteLine("Writing PROXY URL...");

                if (String.IsNullOrEmpty(value))
                {
                    string currentProxyUrl = this.GetUserKey(REG_PROXY_URL);
                    if (currentProxyUrl != null)
                    {
                        WinRegistry.DeleteHKCUValue(this.registryProxyKey, REG_PROXY_URL);
                    }
                    Debug.WriteLine("PROXY URL is null. Nothing to do here.");
                    return;
                }

                if (!value.StartsWith("http://"))
                {
                    throw new ApplicationException(Strings.S00013);
                }
                else
                {
                    SaveUserKey(REG_PROXY_URL, value);
                    Debug.WriteLine("Done!");
                }
			}
		}

        /// <summary>
        /// Gets or sets the px port.
        /// </summary>
        /// <value>The px port.</value>
		public string ProxyPort
		{
			get
			{
                Debug.WriteLine("Reading PROXY Port");
                return this.GetUserKey(REG_PROXY_PORT);
			}
			set
			{
                Debug.WriteLine("Writing PROXY Port");

                if (String.IsNullOrEmpty(value))
                {
                    string currentProxyPort = this.GetUserKey(REG_PROXY_PORT);
                    if (currentProxyPort != null)
                    {
                        WinRegistry.DeleteHKCUValue(this.registryProxyKey, REG_PROXY_URL);
                    }

                    Debug.WriteLine("PROXY Port is null. Nothing to do here.");
                    return;
                }

                int number;
                bool isNum = Int32.TryParse(value, out number);
                if (!isNum)
                {
                    throw new ApplicationException(Strings.S00014);
                }
                else
                {
                    SaveUserKey(REG_PROXY_PORT, number.ToString()); 
                }
			}
		}

        /// <summary>
        /// Gets or sets the name of the px.
        /// </summary>
        /// <value>The name of the px.</value>
		public string ProxyAuthUsername
		{
			get
			{
                return this.GetUserKey(REG_PROXY_USER);
			}
			set
			{
				SaveUserKey(REG_PROXY_USER, value);
			}
		}

        /// <summary>
        /// Gets or sets the px password.
        /// </summary>
        /// <value>The px password.</value>
		public string ProxyAuthPassword
		{
			get
			{
                return this.GetUserKey(REG_PROXY_PASSWORD);
			}
			set
			{
				SaveUserKey(REG_PROXY_PASSWORD, value);
			}
		}

        private void DeleteAllUserKeys()
        {

            WinRegistry.DeleteHKCUValue(this.registryProxyKey, REG_PROXY_URL);
            WinRegistry.DeleteHKCUValue(this.registryProxyKey, REG_PROXY_PORT);
            this.DeleteAuthenticationKeys();

        }

        private void DeleteAuthenticationKeys()
        {
            WinRegistry.DeleteHKCUValue(this.registryProxyKey, REG_PROXY_USER);
            WinRegistry.DeleteHKCUValue(this.registryProxyKey, REG_PROXY_PASSWORD);
        }

		/// <summary>
		/// Save RC to the registry (Current User)
		/// </summary>
		/// <param name="rc">RC</param>
		/// <exception cref="Exception">When trying to save a wrong RC</exception>
		private void SaveUserKey(string key, string valkey)
		{
			WinRegistry.SetHKCUValue(this.applicationKey, key, valkey);
		}

		/// <summary>
		/// Get  value from registry (Current User) as a string. If nothing was found, or in case of error
		/// returns a null string.
		/// </summary>
		/// <param name="key">The key to read in the registry</param>
		/// <returns>The value associated to "key", or null if anything went wrong.</returns>
		private string GetUserKey(string key)
		{
			string rc=null;

			try
			{
				rc = WinRegistry.GetHKCUValue(this.applicationKey,key);
			}
			catch (Exception x)
			{
				Console.WriteLine("RegistryOpn.GetKey > Intercepted Exception: "+x.Message);
			}

			return rc;
		}


    }
}

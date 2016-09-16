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
using System.Threading;
using System.IO;

using LLCryptoLib.Security.Certificates;
using LLCryptoLib.Utils;
using LLCryptoLib.Hash;
using System.Text;

namespace LLCryptoLib.Security.Certificates
{
	/// <summary>
	/// A wrapper around <see cref="CertificateStore"/>.
    /// Adds name and file path, plus a <see cref="StoreLocation"/>
	/// </summary>
	public class LLCertificateStore : CertificateStore
	{
		private string path;
		private string name;
		private bool readOnly;
		private StoreLocation location;

        /// <summary>
        /// Deletes a certificate store.
        /// </summary>
        /// <param name="location">The store location as in <see cref="StoreLocation"/></param>
        /// <param name="name">The name of the store</param>
		public static void DeleteCertificateStore(StoreLocation location, string name)
		{
			const string CertificateLocation = @"SOFTWARE\Microsoft\SystemCertificates";
			if (location==StoreLocation.LocalMachine)
			{
				WinRegistry.DeleteHKLMKey(CertificateLocation+@"\"+name+@"\Certificates");
				WinRegistry.DeleteHKLMKey(CertificateLocation+@"\"+name+@"\CRLs");
				WinRegistry.DeleteHKLMKey(CertificateLocation+@"\"+name+@"\CTLs");
				WinRegistry.DeleteHKLMKey(CertificateLocation+@"\"+name);
			}
			else
			{
				WinRegistry.DeleteHKCUKey(CertificateLocation+@"\"+name+@"\Certificates");
				WinRegistry.DeleteHKCUKey(CertificateLocation+@"\"+name+@"\CRLs");
				WinRegistry.DeleteHKCUKey(CertificateLocation+@"\"+name+@"\CTLs");
				WinRegistry.DeleteHKCUKey(CertificateLocation+@"\"+name);
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="LLCertificateStore"/> class.
        /// </summary>
        /// <param name="location">The location of the store</param>
        /// <param name="store">The name of the store to open.</param>
        public LLCertificateStore(StoreLocation location, string store)
            : base(location, store)
        {
            this.name = store;
            this.readOnly = true;
            this.location = location;
        }

		/// <summary>
		/// Create an LLCertificateStore from a CertificateStore.
		/// This will be a read only certificate.
		/// </summary>
		/// <param name="store">The certificate store.</param>
		public LLCertificateStore(CertificateStore store) : base(store)
		{
            this.name = store.Handle.ToString();
			this.readOnly = true;
			this.location = StoreLocation.CurrentUser;
		}

        /// <summary>
        /// Create an LLCertificateStore from a CertificateStore, specifying attributes.
        /// </summary>
        /// <param name="store">The certificate Store</param>
        /// <param name="sname">The store name.</param>
        /// <param name="location">The store location as in <see cref="StoreLocation"/></param>
		public LLCertificateStore(CertificateStore store, string sname, StoreLocation location): base(store)
		{
			this.name = sname;
			this.readOnly = true;
			this.location = location;
		}

        /// <summary>
        /// Create an LLCertificateStore from a CertificateStore, specifying attributes.
        /// This will be a writable certificate.
        /// </summary>
        /// <param name="store">The certificate store.</param>
        /// <param name="spath">Folder in which saving certificates (may be temporary)</param>
        /// <param name="sname">The certificate store name.</param>
        /// <param name="location">The certificate store location.</param>
		public LLCertificateStore(CertificateStore store, string spath, string sname, StoreLocation location): base(store)
		{
			this.path = spath;
			this.name = sname;
			this.location = location;
		}

		/// <summary>
		/// Temporary save of the certificate store certificate
		/// </summary>
		/// <returns>The full path of the certificate store file</returns>
		public string SaveCertificateStoreFile()
		{
			// Get a temporary directory
			string temp = System.IO.Path.GetTempPath();
			string tempCerPath = temp+"tempCer.cer";
			try
			{
				if (File.Exists(tempCerPath))
				{
					File.Delete(tempCerPath);
				}
				this.ToCerFile(tempCerPath,CertificateStoreType.Pkcs7Message);
			}
			catch (Exception)
			{
				Console.WriteLine("Cannot save temporary certificate file!");
				tempCerPath = null;
				throw;
			}

			return tempCerPath;
		}

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
		public StoreLocation Location
		{
			get
			{
				return this.location;
			}

			set
			{
				this.location = value;
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
		public bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set 
			{
				this.readOnly = value;
			}

		}

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
		public string Path
		{
			get
			{
				return this.path;
			}
		}

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

        /// <summary>
        /// Gets the relativ store location in the registry.
        /// </summary>
        /// <value>The store registry key (relative to SOFTWARE\).</value>
        public string RegistryKey
        {
            get
            {
                string key = null;
                if (this.location == StoreLocation.LocalMachine || this.location == StoreLocation.CurrentUser)
                {
                    StringBuilder keyPath = new StringBuilder(@"Microsoft\SystemCertificates\");
                    keyPath.Append(this.Name);
                    key = keyPath.ToString();
                }
                return key;
            }
        }

        /// <summary>
        /// Gets the serial number.
        /// The serial number is the SHA1 of the certificate name + certificate location
        /// </summary>
        /// <value>The serial number of this certificate store.</value>
        public string SerialNumber
        {
            get
            {
                string identifier = this.name;
                identifier += this.location.ToString();
                IHash hash = new LLCryptoLib.Hash.Hash();
                hash.SetAlgorithm(AvailableHash.MD5);
                return hash.ComputeHashStyle(identifier, HexEnum.CLASSIC, Config.TextEncoding);
            }
        }

	}
}

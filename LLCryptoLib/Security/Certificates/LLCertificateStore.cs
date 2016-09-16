using System;
using System.IO;
using System.Text;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;

namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    ///     A wrapper around <see cref="CertificateStore" />.
    ///     Adds name and file path, plus a <see cref="StoreLocation" />
    /// </summary>
    public class LLCertificateStore : CertificateStore
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LLCertificateStore" /> class.
        /// </summary>
        /// <param name="location">The location of the store</param>
        /// <param name="store">The name of the store to open.</param>
        public LLCertificateStore(StoreLocation location, string store)
            : base(location, store)
        {
            this.Name = store;
            this.ReadOnly = true;
            this.Location = location;
        }

        /// <summary>
        ///     Create an LLCertificateStore from a CertificateStore.
        ///     This will be a read only certificate.
        /// </summary>
        /// <param name="store">The certificate store.</param>
        public LLCertificateStore(CertificateStore store) : base(store)
        {
            this.Name = store.Handle.ToString();
            this.ReadOnly = true;
            this.Location = StoreLocation.CurrentUser;
        }

        /// <summary>
        ///     Create an LLCertificateStore from a CertificateStore, specifying attributes.
        /// </summary>
        /// <param name="store">The certificate Store</param>
        /// <param name="sname">The store name.</param>
        /// <param name="location">The store location as in <see cref="StoreLocation" /></param>
        public LLCertificateStore(CertificateStore store, string sname, StoreLocation location) : base(store)
        {
            this.Name = sname;
            this.ReadOnly = true;
            this.Location = location;
        }

        /// <summary>
        ///     Create an LLCertificateStore from a CertificateStore, specifying attributes.
        ///     This will be a writable certificate.
        /// </summary>
        /// <param name="store">The certificate store.</param>
        /// <param name="spath">Folder in which saving certificates (may be temporary)</param>
        /// <param name="sname">The certificate store name.</param>
        /// <param name="location">The certificate store location.</param>
        public LLCertificateStore(CertificateStore store, string spath, string sname, StoreLocation location)
            : base(store)
        {
            this.Path = spath;
            this.Name = sname;
            this.Location = location;
        }

        /// <summary>
        ///     Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public StoreLocation Location { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        public bool ReadOnly { get; set; }

        /// <summary>
        ///     Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        ///     Gets the relativ store location in the registry.
        /// </summary>
        /// <value>The store registry key (relative to SOFTWARE\).</value>
        public string RegistryKey
        {
            get
            {
                string key = null;
                if ((this.Location == StoreLocation.LocalMachine) || (this.Location == StoreLocation.CurrentUser))
                {
                    StringBuilder keyPath = new StringBuilder(@"Microsoft\SystemCertificates\");
                    keyPath.Append(this.Name);
                    key = keyPath.ToString();
                }
                return key;
            }
        }

        /// <summary>
        ///     Gets the serial number.
        ///     The serial number is the SHA1 of the certificate name + certificate location
        /// </summary>
        /// <value>The serial number of this certificate store.</value>
        public string SerialNumber
        {
            get
            {
                string identifier = this.Name;
                identifier += this.Location.ToString();
                IHash hash = new Hash.Hash();
                hash.SetAlgorithm(AvailableHash.MD5);
                return hash.ComputeHashStyle(identifier, HexEnum.CLASSIC, Config.TextEncoding);
            }
        }

        /// <summary>
        ///     Deletes a certificate store.
        /// </summary>
        /// <param name="location">The store location as in <see cref="StoreLocation" /></param>
        /// <param name="name">The name of the store</param>
        public static void DeleteCertificateStore(StoreLocation location, string name)
        {
            const string CertificateLocation = @"SOFTWARE\Microsoft\SystemCertificates";
            if (location == StoreLocation.LocalMachine)
            {
                WinRegistry.DeleteHKLMKey(CertificateLocation + @"\" + name + @"\Certificates");
                WinRegistry.DeleteHKLMKey(CertificateLocation + @"\" + name + @"\CRLs");
                WinRegistry.DeleteHKLMKey(CertificateLocation + @"\" + name + @"\CTLs");
                WinRegistry.DeleteHKLMKey(CertificateLocation + @"\" + name);
            }
            else
            {
                WinRegistry.DeleteHKCUKey(CertificateLocation + @"\" + name + @"\Certificates");
                WinRegistry.DeleteHKCUKey(CertificateLocation + @"\" + name + @"\CRLs");
                WinRegistry.DeleteHKCUKey(CertificateLocation + @"\" + name + @"\CTLs");
                WinRegistry.DeleteHKCUKey(CertificateLocation + @"\" + name);
            }
        }

        /// <summary>
        ///     Temporary save of the certificate store certificate
        /// </summary>
        /// <returns>The full path of the certificate store file</returns>
        public string SaveCertificateStoreFile()
        {
            // Get a temporary directory
            string temp = System.IO.Path.GetTempPath();
            string tempCerPath = temp + "tempCer.cer";
            try
            {
                if (File.Exists(tempCerPath))
                {
                    File.Delete(tempCerPath);
                }
                this.ToCerFile(tempCerPath, CertificateStoreType.Pkcs7Message);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot save temporary certificate file!");
                tempCerPath = null;
                throw;
            }

            return tempCerPath;
        }
    }
}
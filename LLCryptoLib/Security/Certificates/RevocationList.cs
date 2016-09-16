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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

using LLCryptoLib.Utils;

namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    /// A revocation list class
    /// </summary>
    [Serializable()]
    public class RevocationList
    {
        private StoreLocation registryLocation;
        private string storeName;
        private List<RevocationItem> certificates;

        /// <summary>
        /// Get the CRL name
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns>The name of the list</returns>
        public static string ListName(LLCertificateStore store)
        {
            return store.SerialNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RevocationList"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public RevocationList(LLCertificateStore store)
        {
            this.storeName = store.Name;
            this.registryLocation = store.Location;
            this.Init();
        }

        /// <summary>
        /// Updates the revocation of a certificate.
        /// </summary>
        /// <param name="serial">The serial.</param>
        /// <param name="revoked">if set to <c>true</c> [revoked].</param>
        public void UpdateRevocation(string serial, bool revoked)
        {
            RevocationItem riToUpdate = null;
            List<RevocationItem> newList = new List<RevocationItem>(this.certificates.Count);

            foreach (RevocationItem ri in this.certificates)
            {
                if (ri.Certificate == serial)
                {
                    riToUpdate = ri;
                }
                else
                {
                    newList.Add(ri);
                }
            }

            if (riToUpdate != null)
            {
                riToUpdate.Revoked = revoked;
                newList.Add(riToUpdate);
            }

            this.certificates = newList;
        }


        /// <summary>
        /// Gets the CRL.
        /// </summary>
        /// <returns></returns>
        public List<RevocationItem> GetCRL()
        {
            return this.certificates;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RevocationList"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="name">The name.</param>
        public RevocationList(StoreLocation location, string name)
        {
            this.storeName = name;
            this.registryLocation = location;
            this.Init();
        }

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        public bool SerializeCRL()
        {
            bool success = true;

            try
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                MemoryStream mstream = new MemoryStream();
                bformatter.Serialize(mstream, this.certificates);
                this.SaveCertificatesToRegistry(mstream.ToArray());
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                success = false;
            }

            return success;
        }

        private void Deserialize(byte[] certStream)
        {
            BinaryFormatter bformatter = new BinaryFormatter();
            MemoryStream mstream = new MemoryStream(certStream);
            this.certificates = (List<RevocationItem>)bformatter.Deserialize(mstream);
        }

        private void SaveCertificatesToRegistry(byte[] serializedObj)
        {
            // Get store
            LLCertificateStore theStore = this.Store;

            // Set Windows Registry key name
            string clrRegistry = RevocationList.CRLRegistryKey(theStore);

            // Save Blob
            if (theStore.Location == StoreLocation.LocalMachine)
            {
                WinRegistry.SetHKLMBlob(clrRegistry, "Blob", serializedObj);
            }
            else
            {
                WinRegistry.SetHKCUBlob(clrRegistry, "Blob", serializedObj);
            }
        }

        private void LoadCertificatesFromRegistry(LLCertificateStore theStore)
        {

            // Set Windows Registry key name
            string clrRegistry = RevocationList.CRLRegistryKey(theStore);

            // Load Blob
            System.Diagnostics.Debug.WriteLine("Trying to read CRL from registry...");
            byte[] certStream = null;
            if (theStore.Location == StoreLocation.LocalMachine)
            {
                certStream = WinRegistry.GetHKLMBlob(clrRegistry, "Blob");
            }
            else
            {
                certStream = WinRegistry.GetHKCUBlob(clrRegistry, "Blob");
            }
            System.Diagnostics.Debug.WriteLine("Done.");

            System.Diagnostics.Debug.WriteLine("Deserializing blob...");
            this.Deserialize(certStream);
            System.Diagnostics.Debug.WriteLine("Done.");

            System.Diagnostics.Debug.WriteLine("CRL contains " + this.certificates.Count + " item(s)");

        }

        private void Init()
        {
            // Get store
            LLCertificateStore theStore = this.Store;

            // Get revocation list name
            // string crlName = RevocationList.ListName(theStore);

            // Open registry key if it exists
            string certificateLocation = @"SOFTWARE\";
            certificateLocation += RevocationList.CRLRegistryKey(theStore);

            bool keyExist;
            if (theStore.Location == StoreLocation.CurrentUser)
            {
                keyExist = WinRegistry.HKCUExists(certificateLocation);
            }
            else
            {
                keyExist = WinRegistry.HKLMExists(certificateLocation);
            }

            if (keyExist)
            {
                // Load values from the key
                try
                {
                    this.LoadCertificatesFromRegistry(theStore);
                    System.Diagnostics.Debug.WriteLine("CRL correctly read from registry");
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine("Error in retrieving CRL: " + exc.Message);
                }

            }
            else
            {
                this.InitializeCRL();
            }

        }

        private void InitializeCRL()
        {
            this.certificates = new List<RevocationItem>();
            foreach (Certificate cert in this.Store.EnumCertificates())
            {
                this.certificates.Add(new RevocationItem(cert.SerialNumberString,
                                      cert.Name, false));
            }
            this.SerializeCRL();
        }

        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        private LLCertificateStore Store
        {
            get
            {
                LLCertificateStore theStore = new LLCertificateStore(this.registryLocation, this.storeName);
                System.Diagnostics.Debug.WriteLine("Opened store: " + theStore.Name);
                System.Diagnostics.Debug.WriteLine("This store has " + theStore.EnumCertificates().Length + " certificates.");
                return theStore;
            }
        }


        private static string CRLRegistryKey(LLCertificateStore theStore)
        {
            StringBuilder crlLocation = new StringBuilder(theStore.RegistryKey);
            crlLocation.Append(@"\CRLs\");
            crlLocation.Append(RevocationList.ListName(theStore));
            return crlLocation.ToString();
        }

    }
}

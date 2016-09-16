/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Collections;
using LLCryptoLib.Hash;
using System.Collections.Generic;
using IsideFolder.Properties;

namespace IsideFolder
{
    /// <summary>
    /// FolderHashAlgorithms.
    /// This class holds the available hash algorithms for Iside Folder
    /// and synchronize it with the registry, setting the hash function
    /// choosen.
    /// </summary>
    public class FolderHashAlgorithms
    {
        private AvailableHash currentHash;
        private string[] availableHashes;

        /// <summary>
        /// Folder Hash
        /// </summary>
        /// <param name="hashes"></param>
        /// <param name="hashOption"></param>
        public FolderHashAlgorithms()
        {
            // Choose fast algorithms as available
            this.availableHashes = SupportedHashAlgorithms.GetFastHashAlgorithms();
            this.currentHash = Settings.Default.FolderHash;
        }

        /// <summary>
        /// Gets the available hashes.
        /// </summary>
        /// <value>The available hashes.</value>
        public string[] GetAvailableHashes()
        {
            return this.availableHashes;
        }

        /// <summary>
        /// Gets or sets the default hash.
        /// </summary>
        /// <value>The default hash.</value>
        public string DefaultHash
        {
            get
            {
                return SupportedHashAlgoFactory.GetAlgo(this.currentHash).Name;
            }

            set
            {
                SupportedHashAlgo sha = SupportedHashAlgoFactory.FromName(value);
                Settings.Default.FolderHash = sha.Id;
                Settings.Default.Save();
            }
        }


    }
}

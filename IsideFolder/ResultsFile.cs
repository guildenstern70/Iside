/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Collections.Generic;
using System.Text;
using LLCryptoLib.Hash;
using AxsUtils;

namespace IsideFolder
{
    internal class ResultsFile
    {
        private string[] contents;
        private AvailableHash resultsHash;
        public int NrOfFiles { get; set; }
        public int NrOfFolders { get; set; }

        private ResultsFile(AvailableHash hash, string[] results)
        {
            this.resultsHash = hash;
            this.contents = results;
        }

        internal string[] Results()
        {
            return this.contents;
        }

        internal AvailableHash Hash
        {
            get
            {
                return this.resultsHash;
            }
        }

        private static int ReadNrOfItems(string result)
        {
            int nrOfItems = -1;
            if (result.Length > 25)
            {
                string nrFolders = result.Substring(25);
                try
                {
                    nrOfItems = Int32.Parse(nrFolders);
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine("Error in reading NR of items: " + exc.Message);
                }
            }
            return nrOfItems;
        }

        internal static ResultsFile FromFile(string isrlFilePath)
        {
            System.Diagnostics.Debug.WriteLine("Reading RL file from " + isrlFilePath);
            List<string> results = new List<string>();
            FileManager fm = FileManager.Reference;
            AvailableHash ah = AvailableHash.MD5;
            ResultsFile rf = null;
            int readNrOfFiles = -1;
            int readNrOfFolders = -1;

            foreach (string result in fm.ReadFileLines(isrlFilePath))
            {
                if (result.Length > 0)
                {
                    if (!result.StartsWith("#"))
                    {
                        results.Add(result);
                    }
                    else
                    {
                        if (result.StartsWith("# Hash "))
                        {
                            if (result.Length > 25)
                            {
                                string hashSaved = result.Substring(25);
                                SupportedHashAlgo sha = SupportedHashAlgoFactory.FromName(hashSaved);

                                if (sha != null)
                                {
                                    System.Diagnostics.Debug.WriteLine("Found hash in ISRL: " + sha.Name);
                                    ah = sha.Id;
                                }
                            }
                        }
                        else if (result.StartsWith("# Number of Folders"))
                        {
                            readNrOfFolders = ReadNrOfItems(result);
                        }
                        else if (result.StartsWith("# Number of Files"))
                        {
                            readNrOfFiles = ReadNrOfItems(result);

                        }
                    }
                }
            }

            rf = new ResultsFile(ah, results.ToArray());
            if (readNrOfFiles > 0)
            {
                rf.NrOfFiles = readNrOfFiles;
            }
            if (readNrOfFolders > 0)
            {
                rf.NrOfFolders = readNrOfFolders;
            }

            return rf;
        }
    }

    
}

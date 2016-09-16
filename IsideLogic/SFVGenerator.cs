/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LLCryptoLib;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;

namespace IsideLogic
{
    public class SFVGenerator : ChecksumGenerator
    {

        private const string ZEROFILE = "[0 bytes file";

        /// <summary>
        /// Initializes a new instance of the <see cref="SFVGenerator"/> class.
        /// </summary>
        public SFVGenerator()
        {
            this.checksumName = "SFV";
            this.commentChar = ';';
        }

        /// <summary>
        /// Produces the sum.
        /// </summary>
        /// <param name="cbe">The cbe.</param>
        /// <returns></returns>
        public override string ProduceSum(LLCryptoLib.CallbackEntry cbe)
        {
            if (this.files == null)
            {
                throw new LLCryptoLibException(this.checksumName + " Generator with Directory Elements = null");
            }

            if (this.hashAlgo == null)
            {
                throw new LLCryptoLibException(this.checksumName + " Generator with algo = null");
            }

            StringBuilder sb = new StringBuilder();
            Hash hs = new Hash();
            hs.SetAlgorithm(this.hashAlgo.Id);

            // Header
            sb.Append(this.Header);
            int countFiles = 0;
            foreach (FileInfo fi in this.files.Files)
            {
                sb.Append(SFVGenerator.FullFileName(this.files.Directory.FullName, fi));
                sb.Append(" ");
                sb.Append(SFVGenerator.HashCode(fi, hs));
                sb.Append(Environment.NewLine);
                if (cbe != null)
                {
                    cbe(countFiles++);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Verifies the sum.
        /// </summary>
        /// <param name="md5Path">The MD5 path.</param>
        /// <param name="cbe">The cbe.</param>
        /// <returns></returns>
        public override bool VerifySum(string sfvPath, LLCryptoLib.CallbackEntry cbe)
        {
            if (this.files == null)
            {
                throw new LLCryptoLibException(this.checksumName + " Generator with Directory Elements = null");
            }

            if (this.hashAlgo == null)
            {
                throw new LLCryptoLibException(this.checksumName + " Generator with algo = null");
            }

            if (sfvPath == null)
            {
                throw new ArgumentNullException(sfvPath);
            }

            bool sumVerificationOk = true;

            AxsUtils.FileManager fm = AxsUtils.FileManager.Reference;

            string[] lines = fm.ReadFileLines(sfvPath, System.Text.Encoding.UTF8);
            string[] hashes = new string[lines.Length];

            long count = 0;

            // Hashes holds [hash] [filename]
            foreach (string s in lines)
            {
                if (s.Length > 0)
                {
                    if (s[0] != this.commentChar)
                    {
                        hashes[count] = s;
                        count++;
                    }
                }
            }

            if (count != this.files.NrOfFiles)
            {
                return false;
            }

            Hash newHash = new Hash();
            newHash.SetAlgorithm(this.hashAlgo.Id);

            string currentHash;
            string savedHash;
            string savedPath;
            string currentPath;
            int current = 0;

            foreach (FileInfo fi in this.files.Files)
            {
                string s = hashes[current];
                System.Diagnostics.Debug.Write("Examining: " + fi.FullName + " vs " + s + "> ");

                // Actual file hash
                currentHash = newHash.ComputeHashFileStyle(fi.FullName, HexEnum.UNIX);
                savedHash = SFVGenerator.GetSavedHash(s);
                if (savedHash != null)
                {
                    savedPath = SFVGenerator.GetSavedPath(s);
                    currentPath = fi.FullName.Substring(this.files.Directory.FullName.Length + 1);

                    if (savedPath.Contains(ZEROFILE))
                    {
                        int beginZeroFile = savedPath.IndexOf(ZEROFILE);
                        savedPath = savedPath.Substring(0, savedPath.Length - 14);
                    }

                    if (currentPath.Equals(savedPath))
                    {
                        // [0 the file is 0 bytes
                        if ((currentHash == savedHash) || (savedHash.StartsWith("[0")))
                        {
                            System.Diagnostics.Debug.WriteLine(" OK");
                            current++;
                            cbe(current);
                            continue;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("savedPath = " + savedPath);
                        System.Diagnostics.Debug.WriteLine("savedHash = " + savedHash);
                        System.Diagnostics.Debug.WriteLine("currentPath = " + currentPath);
                        System.Diagnostics.Debug.WriteLine("Different path found: " + currentPath);
                    }

                }

                System.Diagnostics.Debug.WriteLine(" KO");
                sumVerificationOk = false;
                break;
            }

            return sumVerificationOk;
        }

        private static string FullFileName(string initialDirectory, FileInfo fi)
        {
            return fi.FullName.Substring(initialDirectory.Length + 1);
        }

        private static string HashCode(FileInfo fi, Hash hs)
        {
            string hashCode;

            if (fi.Length > 0)
            {
                hashCode = hs.ComputeHashFileStyle(fi.FullName, HexEnum.UNIX);
            }
            else
            {
                StringBuilder strb = new StringBuilder(ZEROFILE);
                int chars = (hs.Algorithm.HashSize / 8) * 2;
                for (int j = strb.Length; j <= chars - 2; j++)
                {
                    strb.Append('#');
                }
                strb.Append(']');
                hashCode = strb.ToString();
            }

            return hashCode;
        }

        private static string GetSavedHash(string s)
        {
            int pathSepPos = s.LastIndexOf(' ');
            string savedHash = null;
            if (pathSepPos > 0)
            {
                try
                {
                    savedHash = s.Substring(pathSepPos + 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    System.Diagnostics.Debug.WriteLine("Can't find last space on " + s);
                }
            }
            return savedHash;
        }

        private static string GetSavedPath(string s)
        {
            int pathSepPos = s.LastIndexOf(' ');
            string savedPath = null;
            if (pathSepPos > 0)
            {
                try
                {
                    savedPath = s.Substring(0, pathSepPos);
                }
                catch (ArgumentOutOfRangeException)
                {
                    System.Diagnostics.Debug.WriteLine("Can't find first space on " + s);
                }
            }
            return savedPath;
        }
    }
}

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
    public class Md5SumGenerator : ChecksumGenerator
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Md5SumGenerator"/> class.
        /// </summary>
        public Md5SumGenerator()
        {
            this.checksumName = "Md5Sum";
            this.commentChar = '#';
        }

        /// <summary>
        /// Produces the Md5sum.
        /// </summary>
        /// <param name="cbe">The delegate feedback method</param>
        /// <returns>A string containing the Md5sum file contents</returns>
        public override string ProduceSum(CallbackEntry cbe)
        {
            if (this.files == null)
            {
                throw new LLCryptoLibException("Md5Sum Generator with Directory Elements = null");
            }

            if (this.hashAlgo == null)
            {
                throw new LLCryptoLibException("Md5Sum Generator with algo = null");
            }

            StringBuilder sb = new StringBuilder();
            Hash hs = new Hash();
            hs.SetAlgorithm(this.hashAlgo.Id);

            // Header
            sb.Append(this.Header);

            string initialDirectory = this.files.Directory.FullName;
            int countFiles = 0;

            foreach (FileInfo fi in this.files.Files)
            {
                string hashCode;
                if (fi.Length > 0)
                {
                    hashCode = hs.ComputeHashFileStyle(fi.FullName, HexEnum.UNIX);
                }
                else
                {
                    StringBuilder strb = new StringBuilder("[0 bytes file ");
                    int chars = (hs.Algorithm.HashSize / 8) * 2;
                    for (int j = strb.Length; j <= chars - 2; j++)
                    {
                        strb.Append('#');
                    }
                    strb.Append(']');
                    hashCode = strb.ToString();
                }
                sb.Append(hashCode);
                sb.Append("  ");
                sb.Append(fi.FullName.Substring(initialDirectory.Length + 1));
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
        /// <param name="cbe">A string containing the Md5sum file contents</param>
        /// <returns></returns>
        public override bool VerifySum(string md5Path, CallbackEntry cbe)
        {
            if (this.files == null)
            {
                throw new LLCryptoLibException("Md5Sum Generator with Directory Elements = null");
            }

            if (this.hashAlgo == null)
            {
                throw new LLCryptoLibException("Md5Sum Generator with algo = null");
            }

            if (md5Path == null)
            {
                throw new ArgumentNullException(md5Path);
            }

            bool sumVerificationOk = true;

            AxsUtils.FileManager fm = AxsUtils.FileManager.Reference;

            string[] lines = fm.ReadFileLines(md5Path, System.Text.Encoding.UTF8);
            string[] hashes = new string[lines.Length];

            long count = 0;

            Hash newHash = new Hash();
            newHash.SetAlgorithm(this.hashAlgo.Id);

            // Build hash codes
            foreach (string s in lines)
            {

                if (s.Length > 0)
                {
                    if (s[0] != this.commentChar)
                    {
                        hashes[count] = s;
                        count++;
                    }
                    else if (s.StartsWith("# Hash algorithm"))
                    {
                        // We try to infer the hash algorithm from the file
                        try
                        {
                            string hashName = s.Substring(17).Trim();
                            SupportedHashAlgo sha = SupportedHashAlgoFactory.FromName(hashName);
                            if (sha != null)
                            {
                                newHash.SetAlgorithm(sha.Id);
                            }
                        }
                        catch (Exception) { }
                    }
                }
            }

            if (count != this.files.NrOfFiles)
            {
                return false;
            }

            string currentHash;
            string savedHash;
            string savedPath;
            string currentPath;
            int current = 0;

            foreach (FileInfo fi in this.files.Files)
            {
                string s = hashes[current];

                System.Diagnostics.Debug.Write("Examining: " + fi.FullName + " vs " + s + "> ");

                currentHash = newHash.ComputeHashFileStyle(fi.FullName, HexEnum.UNIX);
                int pathSepPos = s.IndexOf("  ");

                // Another trial
                if (pathSepPos < 0)
                {
                    pathSepPos = s.IndexOf(" *");
                }

                if (pathSepPos > 0)
                {
                    savedHash = s.Substring(0, pathSepPos);
                    savedPath = s.Substring(pathSepPos + 2);

                    if (savedPath[0] == '\\')
                    {
                        savedPath = s.Substring(pathSepPos + 3);
                    }

                    currentPath = fi.FullName.Substring(this.files.Directory.FullName.Length + 1);

                    if (currentPath.Equals(savedPath))
                    {
                        // [0 the file is 0 bytes
                        if ((currentHash.Equals(savedHash)) || (savedHash.StartsWith("[0")))
                        {
                            System.Diagnostics.Debug.WriteLine(" OK");
                            current++;
                            cbe(current);
                            continue;
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine(" KO");
                sumVerificationOk = false;
                break;
            }

            return sumVerificationOk;
        }
    }
}

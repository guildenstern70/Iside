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
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */

using System.Security.Cryptography;

namespace LLCryptoLib.Hash
{

    /// <summary>
    /// A class to summarize the library supported Hash Algorithm
    /// </summary>
    public static class SupportedHashAlgorithms
    {
        internal static SupportedHashAlgo[] Algos = {
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.FAKE),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.ADLER32),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.CRC32),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.FCS16),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.FCS32),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.FNV32),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.FNV64),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.FNV1A32),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.FNV1A64),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.GHASH323),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.GHASH325),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.GOST),
													    SupportedHashAlgoFactory.GetAlgo(AvailableHash.HAVAL128),
													    SupportedHashAlgoFactory.GetAlgo(AvailableHash.HAVAL160),
													    SupportedHashAlgoFactory.GetAlgo(AvailableHash.HAVAL192),
													    SupportedHashAlgoFactory.GetAlgo(AvailableHash.HAVAL224),
													    SupportedHashAlgoFactory.GetAlgo(AvailableHash.HAVAL256),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.MD2),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.MD4),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.MD5),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.SHA1),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.SHA256),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.SHA224),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.SHA384),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.SHA512),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.SKEIN224),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.SKEIN256),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.SKEIN384),
                                                        SupportedHashAlgoFactory.GetAlgo(AvailableHash.SKEIN512),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.RIPEMD160),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.TIGER),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.WHIRLPOOL),
														SupportedHashAlgoFactory.GetAlgo(AvailableHash.HMACSHA1)
												   };

        /// <summary>
        /// Number of supported hash algorithms
        /// </summary>
        public static int Count
        {
            get
            {
                return SupportedHashAlgorithms.Count;
            }
        }

        /// <summary>
        /// List of fast hash algortihms
        /// </summary>
        /// <returns>An array of string names of the algorithms that are supposed to perform fast.</returns>
        public static string[] GetFastHashAlgorithms()
        {
            string[] fasts = new string[SupportedHashAlgorithms.Algos.Length];
            int counter = 0;

            foreach (SupportedHashAlgo sha in SupportedHashAlgorithms.Algos)
            {
                if (sha.IsFast)
                {
                    fasts[counter++] = sha.Name;
                }
            }

            string[] retstr = new string[counter];

            for (int j = 0; j < counter; j++)
            {
                retstr[j] = fasts[j];
            }

            return retstr;
        }

        /// <summary>
        /// List of all hash algorithms without Keyed ones
        /// </summary>
        /// <returns>All hash algorithms except the ones that needs a key (Keyed)</returns>
        public static string[] GetNoKeyedHashAlgorithms()
        {
            string[] fasts = new string[SupportedHashAlgorithms.Algos.Length];
            int counter = 0;

            foreach (SupportedHashAlgo sha in SupportedHashAlgorithms.Algos)
            {
                if (!sha.IsKeyed)
                {
                    if (sha.Id > 0)
                    {
                        fasts[counter++] = sha.Name;
                    }
                }
            }

            string[] retstr = new string[counter];

            for (int j = 0; j < counter; j++)
            {
                retstr[j] = fasts[j];
            }

            return retstr;
        }

        /// <summary>
        /// List of all available hash algorithms
        /// </summary>
        /// <returns>An array of strings containing all the names of the supported hash algorithms</returns>
        public static string[] GetHashAlgorithms()
        {
            string[] fasts = new string[SupportedHashAlgorithms.Algos.Length]; ;
            int counter = 0;

            foreach (SupportedHashAlgo sha in SupportedHashAlgorithms.Algos)
            {
                if (sha.Id > 0)
                {
                    fasts[counter++] = sha.Name;
                }
            }

            string[] retstr = new string[counter];

            for (int j = 0; j < counter; j++)
            {
                retstr[j] = fasts[j];
            }

            return retstr;
        }


        /// <summary>
        /// List of all available hash algorithm, plus "None" for no algorithm
        /// </summary>
        /// <returns>An array of strings containing all the names of the supported hash algorithms, plus a "None" string to indicate that no algorithm is wanted.</returns>
        public static string[] GetHashAlgorithmsWithNone()
        {
            string[] fasts = new string[SupportedHashAlgorithms.Algos.Length];
            int counter = 0;

            foreach (SupportedHashAlgo sha in SupportedHashAlgorithms.Algos)
            {
                string algoName = sha.Name;
                fasts[counter++] = algoName;
            }

            return fasts;
        }

    }
}

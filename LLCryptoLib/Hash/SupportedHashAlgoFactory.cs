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

using System;
using System.Security.Cryptography;
#if MONO
using Mono.Security.Cryptography;
#endif

namespace LLCryptoLib.Hash
{
	/// <summary>
	/// Supported Hash Algorithm Create.
	/// </summary>
	public static class SupportedHashAlgoFactory
	{

		/// <summary>
		/// Get SupportedHash Algo from Name
		/// </summary>
		/// <param name="xName">The name of the hash algorithm</param>
		/// <returns>The SupportedHashAlgo with the given name</returns>
		public static SupportedHashAlgo FromName(string xName)
		{

            if (xName == null)
            {
                throw new ArgumentNullException("xName");
            }

			SupportedHashAlgo selectedAlgo = null;

			if (xName.StartsWith("-"))
			{
				selectedAlgo = SupportedHashAlgoFactory.GetAlgo(AvailableHash.FAKE);
			}
			else
			{
				foreach (SupportedHashAlgo sha in SupportedHashAlgorithms.Algos)
				{
					if (sha.Name.Equals(xName))
					{
						selectedAlgo = sha;
						break;
					}
				}
			}

			return selectedAlgo;
		}

		/// <summary>
		/// Get SupportedHash Algo from Name
		/// </summary>
        /// <param name="algoId">The name of the hash algorithm</param>
		/// <returns>The SupportedHashAlgo with the given ID</returns>
		public static SupportedHashAlgo FromId(int algoId)
		{
			return SupportedHashAlgoFactory.GetAlgo((AvailableHash)algoId);
		}

		/// <summary>
		/// Get the HashAlgorithm from an AvailableHash constant
		/// </summary>
		/// <param name="hashAlgo">An AvailableHash constant</param>
		/// <returns>The SupportedHashAlgo structure</returns>
		public static SupportedHashAlgo GetAlgo(AvailableHash hashAlgo)
		{
			HashAlgorithm ha = null;
			SupportedHashAlgo sha = null;

			switch (hashAlgo)
			{
				case AvailableHash.FAKE:
					sha = new SupportedHashAlgo(AvailableHash.FAKE, "-", false, false, null);
					sha.Description = "No hash function";
					break;

				case AvailableHash.ADLER32:
					sha = new SupportedHashAlgo(AvailableHash.ADLER32, "ADLER32",false,true,new Adler32());
					sha.Description = "Adler-32 is composed of two sums accumulated per byte: s1 is the sum of all bytes, s2 is the sum of all s1 values. Both sums are done modulo 65521. s1 is initialized to 1, s2 to zero.  The Adler-32 checksum is stored as s2*65536 + s1 in most-significant-byte order.";
					break;

				case AvailableHash.CRC32:
					sha = new SupportedHashAlgo(AvailableHash.CRC32, "CRC32",false,true,new CRC32());
					sha.Description = "CRC32 is not precisely a hash function: it is a cyclic redundancy checksum polynomial of 32-bit lengths of the input bytes.";
					break;

				case AvailableHash.FCS16:
					sha = new SupportedHashAlgo(AvailableHash.FCS16, "FCS16",false,true,new FCS16());
					sha.Description = "The 16 bits Frame Check Sequence.";
					break;

				case AvailableHash.FCS32:
					sha = new SupportedHashAlgo(AvailableHash.FCS32, "FCS32",false,true,new FCS32());
					sha.Description = "The 32 bits Frame Check Sequence.";
					break;

                case AvailableHash.GHASH323:
                    sha = new SupportedHashAlgo(AvailableHash.GHASH323, "GHash-32-3", false, true, new GHash(GHashParameters.GetParameters(GHashStandard.GHash_3)));
                    sha.Description = "An implementation of the GHash-32 hash function, with a shift of 3 bits";
                    break;

                case AvailableHash.GHASH325:
                    sha = new SupportedHashAlgo(AvailableHash.GHASH325, "GHash-32-5", false, true, new GHash(GHashParameters.GetParameters(GHashStandard.GHash_5)));
                    sha.Description = "An implementation of the GHash-32 hash function, with a shift of 5 bits";
                    break;

				case AvailableHash.MD2:
					sha = new SupportedHashAlgo(AvailableHash.MD2, "MD2",false,true,new MD2());
					sha.Description = "The MD2 algorithm is a 128 bit hash function invented by Ron Rivest. The original message is split into blocks of 512 bits. Each block is then mixed with each other for four rounds, after a series of XOR, AND, OR bit operations. Then the message is compressed and a final 128 bit string is extracted.";
					break;

				case AvailableHash.MD4:
					sha = new SupportedHashAlgo(AvailableHash.MD4, "MD4",false,true,new MD4());
					sha.Description = "The MD4 algorithm is a successor to MD2 algorithm. The original message is split into blocks of 512 bits. Each block is then mixed with each other for four rounds, after a series of XOR, AND, OR bit operations. Then the message is compressed and a final 128 bit string is extracted.";
					break;

				case AvailableHash.MD5:
					sha = new SupportedHashAlgo(AvailableHash.MD5,"MD5",false,true,new System.Security.Cryptography.MD5CryptoServiceProvider());
					sha.Description = "The MD5 hash function invented by Ron Rivest. The original message is split into blocks of 512 bits. Each block is then mixed with each other for four rounds, after a series of XOR, AND, OR bit operations. Then the message is compressed and a final 128 bit string is extracted. MD5 is known to be one of the fastest hashing functions available for 32 bit CPU machines.";
					break;

				case AvailableHash.SHA1:
					sha =new SupportedHashAlgo(AvailableHash.SHA1,"SHA1",false,true,new SHA1Managed());
					sha.Description = "SHA1 is a cryptographic message digest algorithm similar to the MD5 family of hash functions developed by Rivest. It differs in that it adds an additional expansion operation, an extra round and the whole transformation was designed to accomodate the DSS block size for efficiency. The Secure Hash Algorithm takes a message of less than 264 bits in length and produces a 160-bit message digest which is designed so that it should be computationaly expensive to find a text which matches a given hash. ";
					break;

                case AvailableHash.SHA224:
                    sha = new SupportedHashAlgo(AvailableHash.SHA224, "SHA224", false, true, new SHA224());
                    sha.Description = "Similar to SHA256, SHA-224 message digest algorithm was proposed by NIST in the FIPS PUB 186-2 Change Notice 1.";
                    break;

				case AvailableHash.SHA256:
					sha = new SupportedHashAlgo(AvailableHash.SHA256,"SHA256",false,true, new SHA256Managed());
					sha.Description = "Same as SHA1, this algorithm outputs a 256 bits string. It is among the best hash functions available today, used by American Encryption Standard (AES) algorithm.";
					break;

				case AvailableHash.SHA384:
					sha = new SupportedHashAlgo(AvailableHash.SHA384,"SHA384",false,false, new SHA384Managed());
					sha.Description = "Same as SHA1, this algorithm outputs a 384 bits string. It is a very complex algorithm, used by American Encryption Standard (AES) algorithm. This algorithm requires a relatively long time to be computed.";
					break;

				case AvailableHash.SHA512:
					sha = new SupportedHashAlgo(AvailableHash.SHA512,"SHA512",false,false, new SHA512Managed());
					sha.Description = "Same as SHA1, this algorithm outputs a 512 bits string. It is a very complex algorithm, used by American Encryption Standard (AES) algorithm. This algorithm requires a relatively long time to be computed.";
					break;

				case AvailableHash.TIGER:
					sha = new SupportedHashAlgo(AvailableHash.TIGER,"TIGER",false,true, new Tiger());
					sha.Description = "Tiger is a fast new hash function, designed to be very fast on modern computers, and in particular on the state-of-the-art 64-bit computers (like DEC-Alpha), while it is still not slower than other suggested hash functions on 32-bit machines.";
					break;

                case AvailableHash.GOST:
                    sha = new SupportedHashAlgo(AvailableHash.GOST, "GOST", false, true, new GOSTHash());
                    sha.Description = "GOST R 34.11-94 is a 256-bit cryptographic hash function defined in the Russia's national standard GOST R 34.11-94 Information Technology - Cryptographic Information Security - Hash Function. The equivalent standard used by other member-states of the CIS is GOST 34.311-95.";
                    break;

				case AvailableHash.WHIRLPOOL:
					sha = new SupportedHashAlgo(AvailableHash.WHIRLPOOL,"WHIRLPOOL", false, false, new Whirlpool());
					sha.Description = "WHIRLPOOL is a hash function designed by Vincent Rijmen and Paulo S. L. M. Barreto. uses Merkle-Damgård strengthening and the Miyaguchi-Preneel hashing scheme with a dedicated 512-bit block cipher.";
					break;

				case AvailableHash.RIPEMD160:
                    sha = new SupportedHashAlgo(AvailableHash.RIPEMD160, "RIPEMD160", false, true, new RIPEMD160());
					sha.Description = "RIPEMD-160 is a 160-bit cryptographic hash function, designed by Hans Dobbertin, Antoon Bosselaers, and Bart Preneel. It is intended to be used as a secure replacement for the 128-bit hash functions MD4, MD5, and RIPEMD. The original RIPEMD was developed in the framework of the EU project RIPE (RACE Integrity Primitives Evaluation).";
					break;

				case AvailableHash.HAVAL128:
					ha = new HAVAL(HAVALParameters.GetParameters(HAVALStandard.HAVAL_3_128));
					sha = new SupportedHashAlgo(AvailableHash.HAVAL128, "HAVAL128", false, true, ha);
					sha.Description = "HAVAL 128 bit is a one-way hashing algorithm designed in 1992. While many of its peers, including MD4 and MD5, have been fully or partially broken, no successful attack on HAVAL has been reported so far. Hence it can serve as a drop-in replacement of MD5. This implementation is the 128 bits HAVAL with 3 passes.";
					break;

				case AvailableHash.HAVAL160:
					ha = new HAVAL(HAVALParameters.GetParameters(HAVALStandard.HAVAL_3_160));
					sha = new SupportedHashAlgo(AvailableHash.HAVAL160, "HAVAL160", false, true, ha);
					sha.Description = "HAVAL 160 bit is a one-way hashing algorithm designed in 1992. While many of its peers, including MD4 and MD5, have been fully or partially broken, no successful attack on HAVAL has been reported so far. Hence it can serve as a drop-in replacement of MD5. This implementation is the 160 bits HAVAL with 3 passes.";
					break;

				case AvailableHash.HAVAL192:
					ha = new HAVAL(HAVALParameters.GetParameters(HAVALStandard.HAVAL_3_192));
					sha = new SupportedHashAlgo(AvailableHash.HAVAL192, "HAVAL192", false, true, ha);
					sha.Description = "HAVAL 192 bit is a one-way hashing algorithm designed in 1992. While many of its peers, including MD4 and MD5, have been fully or partially broken, no successful attack on HAVAL has been reported so far. Hence it can serve as a drop-in replacement of MD5. This implementation is the 192 bits HAVAL with 3 passes.";
					break;

				case AvailableHash.HAVAL224:
					ha = new HAVAL(HAVALParameters.GetParameters(HAVALStandard.HAVAL_3_224));
					sha = new SupportedHashAlgo(AvailableHash.HAVAL224, "HAVAL224", false, true, ha);
					sha.Description = "HAVAL 224 bit is a one-way hashing algorithm designed in 1992. While many of its peers, including MD4 and MD5, have been fully or partially broken, no successful attack on HAVAL has been reported so far. Hence it can serve as a drop-in replacement of MD5. This implementation is the 224 bits HAVAL with 3 passes.";
					break;

				case AvailableHash.HAVAL256:
					ha = new HAVAL(HAVALParameters.GetParameters(HAVALStandard.HAVAL_3_256));
					sha = new SupportedHashAlgo(AvailableHash.HAVAL256, "HAVAL256", false, true, ha);
					sha.Description = "HAVAL 256 bit is a one-way hashing algorithm designed in 1992. While many of its peers, including MD4 and MD5, have been fully or partially broken, no successful attack on HAVAL has been reported so far. Hence it can serve as a drop-in replacement of MD5. This implementation is the 256 bits HAVAL with 3 passes.";
					break;

				case AvailableHash.HMACSHA1:
					ha = new HMACSHA1();
					sha = new SupportedHashAlgo(AvailableHash.HMACSHA1, "HMAC-SHA1", true, false, ha);
					sha.Description = "HMAC can be used in combination with any iterated cryptographic hash function. MD5 and SHA-1 are examples. It uses a secret key for calculation and verification of the message authentication values.";
					break;

                case AvailableHash.SKEIN224:
                    ha = new Skein224();
                    sha = new SupportedHashAlgo(AvailableHash.SKEIN224, "SKEIN224", false, false, ha);
                    sha.Description = "Skein 224 bit is a cryptographic hash function and one out of five finalists in the NIST hash function competition to design what will become the SHA-3 standard, the intended successor of SHA-1 and SHA-2. According to Stefan Lucks, the name Skein refers to how the Skein function intertwines the input, similar to a coil of yarn, which is called a skein.";
                    break;

                case AvailableHash.SKEIN256:
                    ha = new Skein256();
                    sha = new SupportedHashAlgo(AvailableHash.SKEIN256, "SKEIN256", false, false, ha);
                    sha.Description = "Skein 256 bit is a cryptographic hash function and one out of five finalists in the NIST hash function competition to design what will become the SHA-3 standard, the intended successor of SHA-1 and SHA-2. According to Stefan Lucks, the name Skein refers to how the Skein function intertwines the input, similar to a coil of yarn, which is called a skein.";
                    break;

                case AvailableHash.SKEIN384:
                    ha = new Skein384();
                    sha = new SupportedHashAlgo(AvailableHash.SKEIN384, "SKEIN384", false, false, ha);
                    sha.Description = "Skein 384 bit is a cryptographic hash function and one out of five finalists in the NIST hash function competition to design what will become the SHA-3 standard, the intended successor of SHA-1 and SHA-2. According to Stefan Lucks, the name Skein refers to how the Skein function intertwines the input, similar to a coil of yarn, which is called a skein.";
                    break;

                case AvailableHash.SKEIN512:
                    ha = new Skein512();
                    sha = new SupportedHashAlgo(AvailableHash.SKEIN512, "SKEIN512", false, false, ha);
                    sha.Description = "Skein 512 bit is a cryptographic hash function and one out of five finalists in the NIST hash function competition to design what will become the SHA-3 standard, the intended successor of SHA-1 and SHA-2. According to Stefan Lucks, the name Skein refers to how the Skein function intertwines the input, similar to a coil of yarn, which is called a skein.";
                    break;

                case AvailableHash.FNV32:
                    ha = new FNV(FNVParameters.GetParameters(FNVStandard.FNV1_32));
                    sha = new SupportedHashAlgo(AvailableHash.FNV32, "FNV32", false, true, ha);
                    sha.Description = "FNV1 32 bit. Fowler–Noll–Vo is a non-cryptographic hash function created by Glenn Fowler, Landon Curt Noll, and Phong Vo. The basis of the FNV hash algorithm was taken from an idea sent as reviewer comments to the IEEE POSIX P1003.2 committee by Glenn Fowler and Phong Vo back in 1991. In a subsequent ballot round, Landon Curt Noll improved on their algorithm. Some people tried this hash and found that it worked rather well. In an EMail message to Landon, they named the algorithm the Fowler/Noll/Vo or FNV hash. The current versions are FNV-1 and FNV-1a, which supply a means of creating non-zero FNV offset basis.";
                    break;

                case AvailableHash.FNV64:
                    ha = new FNV(FNVParameters.GetParameters(FNVStandard.FNV1_64));
                    sha = new SupportedHashAlgo(AvailableHash.FNV64, "FNV64", false, true, ha);
                    sha.Description = "FNV1 64 bit. Fowler–Noll–Vo is a non-cryptographic hash function created by Glenn Fowler, Landon Curt Noll, and Phong Vo. The basis of the FNV hash algorithm was taken from an idea sent as reviewer comments to the IEEE POSIX P1003.2 committee by Glenn Fowler and Phong Vo back in 1991. In a subsequent ballot round, Landon Curt Noll improved on their algorithm. Some people tried this hash and found that it worked rather well. In an EMail message to Landon, they named the algorithm the Fowler/Noll/Vo or FNV hash. The current versions are FNV-1 and FNV-1a, which supply a means of creating non-zero FNV offset basis.";
                    break;

                case AvailableHash.FNV1A32:
                    ha = new FNV(FNVParameters.GetParameters(FNVStandard.FNV1_32));
                    sha = new SupportedHashAlgo(AvailableHash.FNV1A32, "FNV1A32", false, true, ha);
                    sha.Description ="FNV 1A 32 bit. Fowler–Noll–Vo is a non-cryptographic hash function created by Glenn Fowler, Landon Curt Noll, and Phong Vo. The basis of the FNV hash algorithm was taken from an idea sent as reviewer comments to the IEEE POSIX P1003.2 committee by Glenn Fowler and Phong Vo back in 1991. In a subsequent ballot round, Landon Curt Noll improved on their algorithm. Some people tried this hash and found that it worked rather well. In an EMail message to Landon, they named the algorithm the Fowler/Noll/Vo or FNV hash. The current versions are FNV-1 and FNV-1a, which supply a means of creating non-zero FNV offset basis.";
                    break;

                case AvailableHash.FNV1A64:
                    ha = new FNV(FNVParameters.GetParameters(FNVStandard.FNV1_64));
                    sha = new SupportedHashAlgo(AvailableHash.FNV1A64, "FNV1A64", false, true, ha);
                    sha.Description = "FNV 1B 64 bit. Fowler–Noll–Vo is a non-cryptographic hash function created by Glenn Fowler, Landon Curt Noll, and Phong Vo. The basis of the FNV hash algorithm was taken from an idea sent as reviewer comments to the IEEE POSIX P1003.2 committee by Glenn Fowler and Phong Vo back in 1991. In a subsequent ballot round, Landon Curt Noll improved on their algorithm. Some people tried this hash and found that it worked rather well. In an EMail message to Landon, they named the algorithm the Fowler/Noll/Vo or FNV hash. The current versions are FNV-1 and FNV-1a, which supply a means of creating non-zero FNV offset basis.";
                    break;
			}
		
			return sha;
		}
	}
}
 

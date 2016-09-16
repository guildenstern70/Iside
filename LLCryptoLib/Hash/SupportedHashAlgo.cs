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

namespace LLCryptoLib.Hash
{
	/// <summary>
	/// SupportedHashAlgo.
	/// Container for any supported Hash Algorithm.
	/// </summary>
	public class SupportedHashAlgo
	{	
		private AvailableHash hId;
		private string hName;
		private bool hNeedKey;
		private bool hIsFast;
		private HashAlgorithm hHash;
		private string desc;

		internal SupportedHashAlgo(AvailableHash id, string sName, bool needKey, bool isFast, HashAlgorithm ha)
		{
			this.hId = id;
			this.hName = sName;
			this.hNeedKey = needKey;
			this.hIsFast = isFast;
			this.hHash = ha;
		}

		/// <summary>
		/// Hash Algorithm ID
		/// </summary>
		public AvailableHash Id 
		{ 
			get 
			{ 
				return this.hId; 
			} 
		}

		/// <summary>
		/// Hash Algorithm Name
		/// </summary>
		public string Name 
		{ 
			get
			{
				return this.hName;
			} 
		}

		/// <summary>
		/// Hash Algorithm Description
		/// </summary>
		public string Description
		{
			set
			{
				this.desc = value;
			}
			get
			{
				if (this.desc!=null)
				{
					return this.desc;
				}
				
				return "Unavailable";
			}
		}

		/// <summary>
		/// If the Algorithm is keyed
		/// </summary>
		public bool IsKeyed
		{
			get
			{
				return this.hNeedKey;
			}
		}

		/// <summary>
		/// If the Algorithm is supposed to be fast 
		/// </summary>
		public bool IsFast 
		{ 
			get
			{
				return this.hIsFast;
			}
		}

		/// <summary>
		/// The HashAlgorithm
		/// </summary>
		public HashAlgorithm Algorithm 
		{ 
			get
			{
				return this.hHash;
			}
		}

		/// <summary>
		/// A hash code for HashTable calculus. 
		/// </summary>
		/// <returns>A hash code for HashTable calculus</returns>
		public override int GetHashCode()
		{
			int hidx = (int)this.hId;
			return hidx*this.hName.Length;
		}

		/// <summary>
		/// The name of the hash algorithm
		/// </summary>
		/// <returns>The name of the hash algorithm</returns>
		public override string ToString()
		{
			return this.hName;
		}

		/// <summary>
		/// If two SupportedHashAlgo are the same
		/// </summary>
		/// <param name="obj">Another SupportedHashAlgo</param>
		/// <returns>True if the name of the two algorithms are the same</returns>
		public override bool Equals(object obj)
		{

			SupportedHashAlgo rr = obj as SupportedHashAlgo;

			if (obj == null)
			{
				return false;
			}

			bool eq = false;

			if ((this.hId == rr.Id) && (this.hName.Equals(rr.Name)))
			{
				eq = true;
			}

			return eq;
		}

	}

}
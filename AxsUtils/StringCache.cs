/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved
 * 
 * AxsUtils.Singleton.cs
 * 
 */

using System;
using System.Collections.Generic;

namespace AxsUtils
{
	/// <summary>
	/// StringCache
	/// </summary>
	public class StringCache
	{
		private Dictionary<string, string> cache;

        /// <summary>
        /// Constructor
        /// </summary>
		public StringCache()
		{
            this.cache = new Dictionary<string, string>();
		}

        /// <summary>
        /// A StringCache with n elements
        /// </summary>
        /// <param name="capacity">Number of elements in this StringCache</param>
		public StringCache(int capacity)
		{
            this.cache = new Dictionary<string, string>(capacity);
		}

        /// <summary>
        /// Add a key-value pair to the string cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="toBeCached">Value corresponding to the key</param>
		public void AddToCache(string key, string toBeCached)
		{
			this.cache.Add(key, toBeCached);
		}

        /// <summary>
        /// Get value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public string GetValue(string key)
		{
			return (string)this.cache[key];
		}

        /// <summary>
        /// If the key is already in cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public bool IsInCache(string key)
		{
			return this.cache.ContainsKey(key);
		}

        /// <summary>
        /// 
        /// </summary>
		public static void Test()
		{
			StringCache sc = new StringCache();

			string[] keys = new string[] { ".txt", ".doc", ".txt", ".txt", ".rtf", ".exe", ".cs", ".exe" };
			string inCache = "";

			foreach (string s in keys)
			{
				if (!sc.IsInCache(s))
				{
					// get value from extern source
					string k = "Text Document";
					sc.AddToCache(s,k);
					Console.WriteLine("Cannot find {0}: added", s);
				}
				else
				{
					inCache = sc.GetValue(".txt");
					Console.WriteLine("Found {0}: {1}", s, inCache);
				}
			}
		}
	}
}

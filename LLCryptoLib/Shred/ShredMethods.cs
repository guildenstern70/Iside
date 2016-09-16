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
using System.Collections.Generic;
namespace LLCryptoLib.Shred
{

#if MONO
	internal class Strings
	{
		internal const String S00001 = "Nothing";
		internal const String S00002 = "No shred";
		internal const String S00003 = "Delete only";
		internal const String S00004 = "Simple";
		internal const String S00005 = "Overwrite the file area with a series of 0 (0x00) bits";
		internal const String S00006 = "Complex";
		internal const String S00007 = "Three times overwrite the file area with a sequence of 0(0x00)-1(0xFF)-0(0x00) bits";
		internal const String S00008 = "Pseudorandom";
		internal const String S00009 = "Five times overwrite the file area with a sequence of pseudorandom bits";
		internal const String S00010 = "DoD 5220-22M";
		internal const String S00011 = "Three iterations completely overwrite the file area six times. Each iteration makes two write-passes over the file area: the first pass inscribes ONEs (1) and the next pass inscribes ZEROes (0). After the third iteration, a seventh pass writes the government-designated code 246 across the drive (in hex 0xF6)";
		internal const String S00012 = "Gutmann";
		internal const String S00013 = "Overwrite the file area 36 times, following the Gutmann standard recommendations.";
	}
#endif 


	/// <summary>
	/// ShredMethod is a struct describing the shredding methods and
	/// a factory of ShredMethod objects.
	/// </summary>
	public static class ShredMethods
	{

		/// <summary>
		/// Get a shred method
		/// </summary>
		/// <param name="shred">Enumeration shred methods</param>
		/// <returns>The shred method object</returns>
		public static IShredMethod Get(AvailableShred shred)
		{
			IShredMethod m = null; 

			switch (shred)
			{
				case AvailableShred.NOTHING:
					m = new ShredNothing();
					break;

				case AvailableShred.SIMPLE:
					m = new ShredSimple();
					break;

				case AvailableShred.COMPLEX:
					m = new ShredComplex();
					break;

				case AvailableShred.RANDOM:
					m = new ShredRandom();
					break;

				case AvailableShred.DOD:
					m = new ShredDOD();
					break;

                case AvailableShred.HMGIS5ENH:
                    m = new ShredHmgEnh();
                    break;

				case AvailableShred.GUTMANN:
					m = new ShredGutmann();
					break;

                case AvailableShred.GERMAN:
                    m = new ShredGermanVSITR();
                    break;

                default:
                    throw new LLCryptoLibException("Unknonw shred method.");
			}

			return m;
		}

		/// <summary>
		/// Return a shred method object from a string
		/// </summary>
		/// <param name="method">The method name as in GetSupportedMethods</param>
		/// <returns>A shred object</returns>
		public static IShredMethod FromString(string method)
		{
			IShredMethod m = null; 

			if (method == null)
			{
				System.Diagnostics.Debug.WriteLine(">>> Method not input, defaulting to PseudoRandom!!");
				m = new ShredRandom();
			}
			else
			{
                foreach (AvailableShred xx in Enum.GetValues(typeof(AvailableShred)))
                {
                    m = ShredMethods.Get(xx);
                    if (m.ToString().Equals(method))
                    {
                        break;
                    }
                }
			}

			return m;

		}

		/// <summary>
		/// Supported methods
		/// </summary>
		/// <returns>String array containing the supported methods</returns>
		public static string[] GetSupportedMethods()
		{
            List<string> methods = new List<string>();
            foreach (AvailableShred xx in Enum.GetValues(typeof(AvailableShred)))
            {
                IShredMethod sb = ShredMethods.Get(xx);
                methods.Add(sb.ToString());
            }
            return methods.ToArray();
		}

	}

}

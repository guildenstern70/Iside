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

namespace AxsUtils
{

	/// <summary>
	/// Template for Singleton classes
	/// </summary>
	class Singleton
	{ 
		private static Singleton s;

		private Singleton() {}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Singleton Reference
		{
			get
			{
				if (s==null)
				{
					s=new Singleton();
				}

				return s;
			}
		}
	}


}

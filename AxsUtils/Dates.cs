/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved
 * 
 * AxsUtils.Dates.cs
 * 
 */
using System;

namespace AxsUtils
{
	/// <summary>
	/// Basic operations with dates
	/// </summary>
	public sealed class Dates
	{

		private Dates() {}

        /// <summary>
        /// Gets the today date.
        /// </summary>
        /// <value>The today date.</value>
		public static DateTime TodayDate
		{
			get
			{
				return DateTime.Today;
			}
		}

        /// <summary>
        /// Dates to string.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
		public static string DateToString(DateTime dt)
		{
			return dt.ToShortDateString();
		}

        /// <summary>
        /// Strings to date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
		public static DateTime StringToDate(string date)
		{
			DateTime dt;

			try
			{
				dt = Convert.ToDateTime(date);
			}
			catch (FormatException)
			{
				dt = DateTime.MinValue;
			}

			return dt;
		}

        /// <summary>
        /// Returns the number of days from a certain date
        /// </summary>
        /// <param name="when">The starting date</param>
        /// <returns></returns>
		public static int DaysOld(DateTime when)
		{
			TimeSpan diff = DateTime.Today-when;
			return diff.Days;
		}
	}
}

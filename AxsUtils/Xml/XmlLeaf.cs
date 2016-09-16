/**
 * AXS C# Utils
 * Copyright © 2004-2013 LittleLite Software
 * 
 * All Rights Reserved
 * 
 * AxsUtils.Xml.XmlLeaf.cs
 * 
 */


using System;
using System.Text;
using System.Collections;

namespace AxsUtils.Xml
{
	/// <summary>
	/// XmlLeaf is basically an XML dictionary one to many.
	/// The XML leaf
	/// <code>
	///		<prova>
	///			<valore1>10</valore1>
	///			<valore2>20</valore2>
	///		</prova>
	/// </code>
	/// is rendered as
	/// XmlLeaf.category="prova"
	/// XmlLeaf.values={ valore1=10, valore2=20 }
	/// We call "Names" 'valore1' and 'valore2'
	/// and "Values" '10' and '20'.
	/// Since Values are stored in an HashTable based on Names key,
	/// the Names in a single Leaf cannot be equals.
	/// </summary>
	public class XmlLeaf
	{
		private string category;
		private Hashtable values;
		private int index;
		private Random r;

		/// <summary>
		/// Summary description for XmlLeaf.
		/// </summary>
		public XmlLeaf(string cCategory)
		{
			r = new Random();
			this.category=cCategory;
			values = new Hashtable();
		}

		/// <summary>
		/// Add name/value pair to current leaf.
		/// </summary>
		/// @param nome name of current item
		/// @param valore value of current item
		///
		public void AddValue(string nome, string valore)
		{
			string name=nome;

			// equality check
			if (this.values.ContainsKey((string)name))
			{
				name+=string.Format("{0}",++this.index);
			}
			values.Add(name, valore);
		}

		/// <summary>
		/// Get random value in current category.
		/// </summary>
		/// <returns></returns>
		public string GetRandomValue()
		{
			int valuesCount = this.values.Count;
			int rndPosition = r.Next(valuesCount);
			ICollection vals = this.values.Values;
			string[] strArray = new string[valuesCount];
			vals.CopyTo(strArray,0);
			return strArray[rndPosition];
		}

		/// <summary>
		/// Get value associated to a particular item (nome)
		/// </summary>
		/// @return value (string)
		///
		public string GetValue(string nome)
		{
			return (string)values[nome];
		}

		/// <summary>
		/// Get value associated to a particular item (nome)
		/// </summary>
		/// @return value (int)
		///
		public int GetIntValue(string nome)
		{
			string valore = (string)values[nome];
			return Int32.Parse(valore);
		}

		/// <summary>
		/// Get value associated to a particular item (nome)
		/// </summary>
		/// @return value (double)
		///
		public double GetDoubleValue(string nome)
		{
			string valore = (string)values[nome];
			return Double.Parse(valore);
		}

		/// <summary>
		/// Get container of items for this leaf.
		/// </summary>
		/// @return items in this leaf (Set)
		///
		public ICollection GetNames()
		{
			return this.values.Keys;
		}

		/// <summary>
		/// Get category of this leaf.
		/// </summary>
		/// @return category name
		///
		public string GetCategory()
		{
			return this.category;
		}

		/// <summary>
		/// Long description of leaf (category and values)
		/// </summary>
		public override string ToString()
		{
			IDictionaryEnumerator i = values.GetEnumerator();
			StringBuilder sb = new StringBuilder(this.category);
			sb.Append(":\n");
			while (i.MoveNext())
			{
				sb.Append("\t"); sb.Append(i.Key);
				sb.Append("="); sb.Append(i.Value);
				sb.Append("\n");
			}
			return sb.ToString();
		}

	}
}

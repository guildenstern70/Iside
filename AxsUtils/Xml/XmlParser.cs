/**
 * AXS C# Utils
 * Copyright © LittleLite Software
 * 
 * All Rights Reserved 
 * 
 * AxsUtils.Xml.XmlParser.cs
 * 
 */

using System;
using System.IO;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AxsUtils.Xml
{
	/// <summary>
	/// XML Navigator
	/// </summary>
	public class XmlParser
	{

		private String xmlpath;
        private Collection<XmlLeaf> contenutiXML; // Array of XmlLeaf

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="path">Complete FileName to XML file</param>
		public XmlParser(string path)
		{
			this.xmlpath=path;
            contenutiXML = new Collection<XmlLeaf>();
		}

		///
		/// Read XML file indicated in the constructor.
		/// Invoke parsing.
		///
		public void ReadXML()
		{
			Console.WriteLine("Trying to parse: "+this.xmlpath);
			this.Read();
		}

		/// <summary>
		/// Elements read in xml file
		/// </summary>
		/// <returns>Elements read in xml file</returns>
		public int Count
		{
			get
			{
				if (this.contenutiXML!=null)
				{
					return this.contenutiXML.Count;
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// Return Read Contents
		/// </summary>
		/// <returns>Array of XmlLeaf elements</returns>
        public Collection<XmlLeaf> GetLeaves()
		{
			return this.contenutiXML;
		}

		/// <summary>
		/// Return single leaf contents
		/// </summary>
		/// <param name="category">Category of XmlLeaf to search</param>
		/// <returns>A single XmlLeaf</returns>
		public XmlLeaf GetLeaf(string category)
		{
            XmlLeaf leaf = null;

			if (this.contenutiXML==null)
			{
				return new XmlLeaf("Unknown");
			}

            foreach (XmlLeaf l in this.contenutiXML)
            {
                if (l.GetCategory().Equals(category))
                {
                    leaf = l;
                    break;
                }
            }

            return leaf;			
		}

		private void Read()
		{
			
			StreamReader sr = new StreamReader(this.xmlpath);
			XmlTextReader reader = new XmlTextReader(sr);
			reader.WhitespaceHandling = WhitespaceHandling.None;
			
			//loop
			XmlLeaf currentLeaf=null;
			string docRoot=null;
			string currentValue = null;
			string currentName = null;
			
			while (reader.Read()) 
			{
				switch (reader.NodeType) 
				{
					case XmlNodeType.Element: // The node is an element.
						if (docRoot==null)
						{
							docRoot = reader.Name;
						}
						else if (currentLeaf==null)
						{
							currentLeaf = new XmlLeaf(reader.Name);
						}
						else
						{
							currentName = reader.Name;
						}
						break;
					case XmlNodeType.Text: //the text in each element.
						if (reader.Value.Length>0)
						{
							currentValue = reader.Value;
						}
						else
						{
							currentValue = "[empty]";
						}
						break;
					case XmlNodeType.EndElement: //the end of the element.
						if (reader.Name.Equals(docRoot))
						{
							reader.Close();
							return;
						}
						else if (reader.Name.Equals(currentLeaf.GetCategory()))
						{
							contenutiXML.Add((XmlLeaf)currentLeaf);
							currentLeaf=null;
						}
						else
						{
							currentLeaf.AddValue(currentName, currentValue);
							currentName=null; currentValue=null;
						}
						break;
				}
			}

			reader.Close();
		}

	}


}

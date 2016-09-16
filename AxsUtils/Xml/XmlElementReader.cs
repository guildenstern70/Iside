using System;
using System.IO;
using System.Xml;
using System.Collections.Specialized;

namespace AxsUtils.Xml
{
	/// <summary>
	/// XmlReader is useful for the parsing of small files
	/// to find a particular element, or element with an attribute.
	/// </summary>
	public static class XmlElementReader
	{

		public static StringCollection ReadElements(XmlTextReader xmlr)
		{

            if (xmlr == null)
            {
                throw new ArgumentNullException("xmlr");
            }

			StringCollection elements = new StringCollection();
			try
			{
				xmlr.WhitespaceHandling = WhitespaceHandling.None;
				xmlr.MoveToContent();
				while (xmlr.Read())
				{
					while (xmlr.NodeType == XmlNodeType.Element)
					{
						elements.Add(xmlr.ReadElementString());
					}
				}
			}
			catch (Exception exc)
			{
				elements.Clear();
				elements.Add("Error in parsing xml :"+exc.Message);
#if (DEBUG)
				Console.WriteLine(exc.StackTrace);
#endif
			}

			return elements;
		}

		public static StringCollection ReadElements(string xmlFilePath)
		{
			
			StringCollection elements = new StringCollection();

			try
			{
				XmlTextReader xmlr = new XmlTextReader(xmlFilePath);
				elements = XmlElementReader.ReadElements(xmlr);
			}
			catch (Exception exc)
			{
				elements.Clear();
				elements.Add("Error in parsing xml >>>"+exc.Message);
#if (DEBUG)
				Console.WriteLine(exc.StackTrace);
#endif
			}

			return elements;
		}


		public static string GetElement(string xmlFilePath, string name, string subname)
		{
			string xmlValue = null;
			try
			{
				XmlTextReader xmlr = new XmlTextReader(xmlFilePath);
				xmlValue = XmlElementReader.GetElement(xmlr, name, subname);
			}
			catch (Exception exc)
			{
				xmlValue = exc.Message;
#if (DEBUG)
				Console.WriteLine(exc.StackTrace);
#endif
			}
			return xmlValue;
		}

		public static string GetElement(XmlReader xmlr, string name, string subname)
		{

            if (xmlr == null)
            {
                throw new ArgumentNullException("xmlr");
            }

			string xmlValue = null;
			bool found = false;

            int counter = 0;

			try
			{
				xmlr.MoveToContent();
				while ((!xmlr.EOF)&&(!found))
				{
                    // Exit if element is not found after 10.000 passes
                    if (++counter > 10000)
                    {
                        break;
                    }
					xmlr.Read();
					if (xmlr.NodeType==XmlNodeType.Element)
					{
						string node = xmlr.Name;
						if (node.Equals(name))
						{
							while (xmlr.Read())
							{
								if (xmlr.NodeType==XmlNodeType.EndElement)
								{
									if (xmlr.Name.Equals(node))
										break;
								}
								else
								{
									if (xmlr.NodeType==XmlNodeType.Element)
									{
										if (xmlr.Name.Equals(subname))
										{
											xmlr.Read();
											if (xmlr.NodeType==XmlNodeType.Text)
											{
												xmlValue = xmlr.Value;
												found = true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				xmlValue = "Error: "+exc.Message;
#if (DEBUG)
				Console.WriteLine(exc.StackTrace);
#endif
			}
			finally
			{
				xmlr.Close();
			}
			return xmlValue;
		}
	}

}

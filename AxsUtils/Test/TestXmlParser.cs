
#if ENABLE_TESTS


using System;
using System.Collections;
using NUnit.Framework;
using AxsUtils.Xml;


namespace AxsUtils.Tests
{
  
	[TestFixture]
	public class TestXmlParser
	{

		private XmlParser xp;

		[SetUp] public void Init()
		{
			xp = new XmlParser("starnames.xml");
		} 
    
		[TearDown] public void Dispose()
		{
		} 

		[Test] public void TestXMLLoaded()
		{
			xp.ReadXML();
			Assert.IsTrue(xp.Count>0);
		}

		[Test] public void TestGetLeaf()
		{
			xp.ReadXML();
			XmlLeaf l = xp.GetLeaf("Stars");
			Assert.IsTrue(xp.Count>0);
		}

		[Test] public void TestGetRandomValue()
		{
			xp.ReadXML();
			XmlLeaf l = xp.GetLeaf("Stars");
			string valx = l.GetRandomValue();
			Console.WriteLine("Random value> {0}",valx);
			Assert.IsTrue(valx.Length>0);
		}

		[Test] public void TestGetTenDifferentRandomValues()
		{
			string valx;
			xp.ReadXML();
			XmlLeaf l = xp.GetLeaf("Stars");
			short counter = 0;

			ArrayList al = new ArrayList(10);
			while (counter<10)
			{
				valx = l.GetRandomValue();
				Console.WriteLine("Trying {0}",valx);
				if (!al.Contains(valx))
				{
					al.Add(valx);
					counter++;
				}
			}

			foreach (string s in al)
			{
				Assert.IsTrue(s.Length>0);
				Console.WriteLine("Random value> {0}",s);
			}
		}

		[Test] public void TestWriteLeaf()
		{
			xp.ReadXML();
			XmlLeaf l = xp.GetLeaf("Stars");
			Console.WriteLine(l.ToString());
		}
	}



}

#endif
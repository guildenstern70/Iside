
#if ENABLE_TESTS

using System;
using System.Collections.Specialized;
using NUnit.Framework;
using AxsUtils.Xml;

namespace AxsUtils.Test
{

	[TestFixture]
	public class TestXmlReader
	{
		private XmlElementReader xp;

		[SetUp] public void Init()
		{
			xp = new XmlElementReader();
		} 
    
		[TearDown] public void Dispose()
		{
		} 

		[Test] public void TestIsideVersion()
		{
			string v = xp.GetElement("Products.xml","Iside","version");
			Console.WriteLine("Iside version "+v);
			string build = xp.GetElement("Products.xml","Ncrypt","build");
			Console.WriteLine("NCrypt build "+build);

			Assert.IsTrue(v.Equals("1.1"));
			Assert.IsTrue(build.Equals("12345"));
		}

		[Test] public void TestRead()
		{
			StringCollection elems = xp.ReadElements("Products.xml");
			foreach (string s in elems)
			{
				Console.WriteLine(s);
			}
			string v1 = elems[0];
			string v2 = elems[1];
			string v3 = elems[2];
			Assert.AreEqual(elems.Count,6);
			Assert.IsTrue(v1.Equals("1.1"));
			Assert.IsTrue(v2.Equals("13321"));
			Assert.IsTrue(v3.Equals("1.2"));
		}

	}

}

#endif
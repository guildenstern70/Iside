/**
 * AXS C# Utils
 * Copyright Copyright (C) 2004-2009 LittleLite Software
 * 
 * All rights reserved
 * 
 * AxsUtils.Tests.TestXmlLeaf.cs
 * 
 */

#if ENABLE_TESTS

using System;
using System.Collections;
using AxsUtils.Xml;
using NUnit.Framework;

namespace AxsUtils.Tests
{
  
	[TestFixture]
	public class TestXmlLeaf
	{

		private XmlLeaf xl;

		/// <summary>
		/// Setup Test Case building object from tested class
		/// </summary>
		[SetUp] public void Init()
		{ 
			xl = new XmlLeaf("Category"); 
			xl.AddValue("IntValue","1");			// Name=IntValue
			xl.AddValue("DoubleValue","1.54");		// Name=DoubleValue
			xl.AddValue("StringValue","Trial");		// Name=StringValue
		} 
    
		/// <summary>
		/// Tear Down object
		/// </summary>
		[TearDown] public void Dispose()
		{
		} 

		/// <summary>
		/// Test GetDoubleValue Method
		/// </summary>
		[Test] public void TestDouble()
		{
			double d = xl.GetDoubleValue("DoubleValue");
			double e = 1.54;
			Assert.AreEqual(e,d,"Double Value");
		}

		/// <summary>
		/// Test GetIntValue Method
		/// </summary>
		[Test] public void TestInt()
		{
			Assert.AreEqual(1,xl.GetIntValue("IntValue"),"Int value");
		}

		/// <summary>
		/// Test GetValue Method
		/// </summary>
		[Test] public void TestString()
		{
			Assert.AreEqual("Trial",xl.GetValue("StringValue"),"String Valore");
		}

		/// <summary>
		/// Test GetNames Method
		/// </summary>
		[Test] public void TestGetNomi()
		{
			ICollection ic = xl.GetNames();
			Assert.AreEqual(3,ic.Count,"Nr. Of names");
		}

		/// <summary>
		/// Test GetCategory Method
		/// </summary>
		[Test] public void TestGetCategory()
		{
			Assert.AreEqual("Category",xl.GetCategory(),"Category");
		}
	}

}

#endif


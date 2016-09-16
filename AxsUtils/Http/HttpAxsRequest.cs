/**
 * AXS C# Utils
 * Copyright Copyright (C) 2004-2009 LittleLite Software
 * 
 * All Rights Reserved 
 * 
 * AxsUtils.Http.HttpAxsRequest.cs
 * 
 */

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace AxsUtils.Http
{
	/// <summary>
	/// 
	/// </summary>
	public class HttpAxsRequest
	{
		private int timeout;
		private string userAgent;
		private IWebProxy proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HttpAxsRequest"/> class.
        /// </summary>
		public HttpAxsRequest()
		{
			this.timeout = 8000;
			this.userAgent = "Axs Application";
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HttpAxsRequest"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
		public HttpAxsRequest(int time)
		{
			this.timeout = time;
			this.userAgent = "Axs Application";
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HttpAxsRequest"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="agent">The agent.</param>
		public HttpAxsRequest(int time, string agent)
		{
			this.timeout = time;
			this.userAgent = agent;
		}

        /// <summary>
        /// Sets the proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <param name="port">The port.</param>
		public void SetProxy(string inProxy, string port)
		{
			string proxyport = inProxy+":"+port;
            try
            {
                WebProxy loProxy = new WebProxy(proxyport, true);
                this.proxy = loProxy;
            }
            catch (UriFormatException uff)
            {
                System.Diagnostics.Debug.WriteLine(uff.ToString());
                this.proxy = null;
            }
		}

        /// <summary>
        /// Sets the proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <param name="port">The port.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
		public void SetProxy(string iProxy, string port, string username, string password)
		{
			string proxyport = iProxy+":"+port;
            try
            {
                WebProxy loProxy = new WebProxy(proxyport, true);
                loProxy.Credentials = new NetworkCredential(username, password);
                this.proxy = loProxy;
            }
            catch (UriFormatException uff)
            {
                System.Diagnostics.Debug.WriteLine(uff.ToString());
                this.proxy = null;
            }
		}

        /// <summary>
        /// Sets the proxy.
        /// </summary>
        /// <param name="subProxy">The proxy object</param>
        public void SetProxy(IWebProxy subProxy)
        {
            this.proxy = subProxy;
        }

        /// <summary>
        /// Makes the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
		public string MakeRequest(string url, string page)
		{
			string lcUrl = url+page;
			string lcXml;

			HttpWebResponse loWebResponse = null;
			StreamReader loResponseStream = null;
            Uri webUri = new Uri(lcUrl);

			try
			{
				// *** Establish the request 
                HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(webUri);

				// *** Set properties
				loHttp.Timeout = this.timeout;     
				loHttp.UserAgent = this.userAgent;
				if (this.proxy!=null)
				{
					loHttp.Proxy = this.proxy;
				}

				// *** Retrieve request info headers
				loWebResponse = (HttpWebResponse)loHttp.GetResponse();
				Encoding enc = Encoding.UTF8;

				loResponseStream = 
					new StreamReader(loWebResponse.GetResponseStream(),enc);

				lcXml = loResponseStream.ReadToEnd();
			}
			catch (Exception exc)
			{
				lcXml = "Error: "+exc.Message;
#if (DEBUG)
				Console.WriteLine(lcXml);
				Console.WriteLine(exc.StackTrace);
#endif
			}
			finally
			{
				if (loWebResponse!=null)
					loWebResponse.Close();
				if (loResponseStream!=null)
					loResponseStream.Close();	
			}

			return lcXml;
		}

//		/// <summary>
//		/// TO BE COMPLETED
//		/// </summary>
//		/// <returns></returns>
//		public string MakePostRequest()
//		{

//			string lcUrl = HttpAxsRequest.LittleLiteURL+HttpAxsRequest.LittleLiteVersionsPage;
//
//			HttpWebRequest loHttp = 
//				(HttpWebRequest) WebRequest.Create(lcUrl);
//
//			// *** Send any POST data
//			string lcPostData = "";
//			//	"Name=" + HttpUtility.UrlEncode("Rick Strahl") + 
//			//	"&Company=" + HttpUtility.UrlEncode("West Wind ");
//
// 
//
//			loHttp.Method="POST";
//			byte [] lbPostBuffer = System.Text.           
//				Encoding.GetEncoding(1252).GetBytes(lcPostData);
//			loHttp.ContentLength = lbPostBuffer.Length;
//
//			Stream loPostData = loHttp.GetRequestStream();
//			loPostData.Write(lbPostBuffer,0,lbPostBuffer.Length);
//			loPostData.Close();
//
//			HttpWebResponse loWebResponse = (HttpWebResponse) loHttp.GetResponse();
//
//			//Encoding enc = System.Text.Encoding.GetEncoding(1252);
//			Encoding enc = Encoding.UTF8;
//
//			StreamReader loResponseStream = 
//
//				new StreamReader(loWebResponse.GetResponseStream(),enc);
//
//			string readStr = loResponseStream.ReadToEnd();
//
//			loWebResponse.Close();
//			loResponseStream.Close();
//
//			return readStr;

//		}


	}
}


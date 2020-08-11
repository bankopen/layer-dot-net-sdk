using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Open
{
	public static class clsHTTPRequest
	{
		public static string HttpPost(string URI, List<KeyValuePair<string, string>> parameters, List<KeyValuePair<string, string>> headers)
		{
			try
			{
				System.Net.WebRequest req = System.Net.WebRequest.Create(URI);

				//req.Proxy = new System.Net.WebProxy(ProxyString, true);
				req.Timeout = 600000;
				req.ContentType = "application/x-www-form-urlencoded";

				foreach (KeyValuePair<string, string> hdr in headers)
				{
					if (!WebHeaderCollection.IsRestricted(hdr.Key))
						req.Headers.Add(hdr.Key, hdr.Value);
				}

				string param = "";
				foreach (KeyValuePair<string, string> pr in parameters)
				{
					param += pr.Key.ToString() + "=" + pr.Value.ToString() + "&";
				}

				param = param.TrimEnd('&');

				req.Method = "POST";
				//We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
				byte[] bytes = System.Text.Encoding.ASCII.GetBytes(param);
				req.ContentLength = bytes.LongLength;
				System.IO.Stream os = req.GetRequestStream();
				os.Write(bytes, 0, bytes.Length); //Push it out there
				os.Close();

				System.Net.WebResponse resp = req.GetResponse();
				if (resp == null) return null;
				System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
				return sr.ReadToEnd().Trim();
			}
			catch (WebException ex)
			{
				string errResponse = "";
				using (WebResponse response = ex.Response)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)response;
					using (Stream data = response.GetResponseStream())
					using (var reader = new StreamReader(data))
					{
						errResponse = reader.ReadToEnd();
						Console.WriteLine(errResponse);
					}
				}

				if (errResponse == "")
					throw new Exception(ex.Message);
				else
					return errResponse;

			}
		}

		public static string HttpPost(string URI, string body, List<KeyValuePair<string, string>> headers)
		{
			try
			{
				System.Net.WebRequest req = System.Net.WebRequest.Create(URI);

				//req.Proxy = new System.Net.WebProxy(ProxyString, true);
				req.Timeout = 600000;
				req.ContentType = "application/json";

				foreach (KeyValuePair<string, string> hdr in headers)
				{
					if (!WebHeaderCollection.IsRestricted(hdr.Key))
						req.Headers.Add(hdr.Key, hdr.Value);
				}

				req.Method = "POST";

				using (var streamWriter = new StreamWriter(req.GetRequestStream()))
				{
					streamWriter.Write(body);
					streamWriter.Flush();
					streamWriter.Close();
				}

				System.Net.WebResponse resp = req.GetResponse();
				if (resp == null)
					return null;

				System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
				return sr.ReadToEnd().Trim();
			}
			catch (WebException ex)
			{
				string errResponse = "";
				using (WebResponse response = ex.Response)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)response;
					using (Stream data = response.GetResponseStream())
					using (var reader = new StreamReader(data))
					{
						errResponse = reader.ReadToEnd();
						Console.WriteLine(errResponse);
					}
				}

				if (errResponse == "")
					throw new Exception(ex.Message);
				else
					return errResponse;
			}
		}

		public static string HttpGet(string URI, List<KeyValuePair<string, string>> headers)
		{
			List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
			return HttpGet(URI, parameters, headers);
		}

		public static string HttpGet(string URI, List<KeyValuePair<string, string>> parameters, List<KeyValuePair<string, string>> headers)
		{
			try
			{
				string param = "";
				foreach (KeyValuePair<string, string> pr in parameters)
				{
					param += pr.Key.ToString() + "=" + pr.Value.ToString() + "&";
				}

				param = param.TrimEnd('&');

				if (param != "")
					URI = URI + "?" + param;

				System.Net.WebRequest req = System.Net.WebRequest.Create(URI);

				//req.Proxy = new System.Net.WebProxy(ProxyString, true);
				req.Timeout = 600000;
				req.Method = "GET";

				WebHeaderCollection hc = req.Headers;
				foreach (KeyValuePair<string, string> hdr in headers)
				{
					if (!WebHeaderCollection.IsRestricted(hdr.Key))
						req.Headers.Add(hdr.Key, hdr.Value);
				}

				System.Net.WebResponse resp = req.GetResponse();
				if (resp == null)
					return null;

				System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
				return sr.ReadToEnd().Trim();
			}
			catch (WebException ex)
			{
				string errResponse = "";
				using (WebResponse response = ex.Response)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)response;
					using (Stream data = response.GetResponseStream())
					using (var reader = new StreamReader(data))
					{
						errResponse = reader.ReadToEnd();
						Console.WriteLine(errResponse);
					}
				}

				if (errResponse == "")
					throw new Exception(ex.Message);
				else
					return errResponse;

			}
		}

	}
}
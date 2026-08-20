using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

// Token: 0x02000003 RID: 3
public static class HTTPScraper
{
	// Token: 0x0600000B RID: 11 RVA: 0x00002494 File Offset: 0x00000694
	public static string GetPage(string Url, ProxyServer Proxy)
	{
		string text;
		try
		{
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = 7000;
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
			myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36";
			myHttpWebRequest.Headers.Add("Accept-Language", "en-US,fr-CA,it-IT;q=0.6");
			myHttpWebRequest.ContentType = "text/html";
			myHttpWebRequest.AllowAutoRedirect = true;
			myHttpWebRequest.Method = "GET";
			myHttpWebRequest.CookieContainer = new CookieContainer();
			myHttpWebRequest.CookieContainer.Add(new Uri("https://www.pagesjaunes.fr"), new CookieCollection());
			myHttpWebRequest.CookieContainer.Add(new Uri("https://www.yellowpages.com.au/"), new CookieCollection());
			myHttpWebRequest.CookieContainer.Add(new Uri("https://www.infobel.com/"), new CookieCollection());
			myHttpWebRequest.KeepAlive = false;
			myHttpWebRequest.MaximumAutomaticRedirections = 15;
			myHttpWebRequest.AllowAutoRedirect = true;
			if (Proxy != null)
			{
				WebProxy myProxy = new WebProxy(string.Format("{0}:{1}", Proxy.IP, Proxy.Port), false);
				myHttpWebRequest.Proxy = myProxy;
			}
			myHttpWebRequest.Timeout = 7000;
			HttpWebResponse httpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			string responseFromServer = streamReader.ReadToEnd();
			streamReader.Close();
			streamReader.Dispose();
			responseStream.Close();
			responseStream.Dispose();
			httpWebResponse.Close();
			text = responseFromServer;
		}
		catch (Exception ex)
		{
			text = ex.Message;
		}
		return text;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002614 File Offset: 0x00000814
	public static string GetPage(string Url, string PostData, ProxyServer Proxy)
	{
		string text;
		try
		{
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = 7000;
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
			myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36";
			myHttpWebRequest.Headers.Add("Accept-Language", "en-US,fr-CA,it-IT;q=0.6");
			myHttpWebRequest.MaximumAutomaticRedirections = 15;
			myHttpWebRequest.AllowAutoRedirect = true;
			if (Proxy != null)
			{
				WebProxy myProxy = new WebProxy(string.Format("{0}:{1}", Proxy.IP, Proxy.Port), false);
				myHttpWebRequest.Proxy = myProxy;
			}
			myHttpWebRequest.Timeout = 7000;
			myHttpWebRequest.KeepAlive = false;
			myHttpWebRequest.ContentType = "text/html";
			myHttpWebRequest.Method = "POST";
			byte[] byteArray = Encoding.UTF8.GetBytes(PostData);
			myHttpWebRequest.ContentLength = (long)byteArray.Length;
			Stream requestStream = myHttpWebRequest.GetRequestStream();
			requestStream.Write(byteArray, 0, byteArray.Length);
			requestStream.Close();
			HttpWebResponse httpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			string responseFromServer = streamReader.ReadToEnd();
			streamReader.Close();
			streamReader.Dispose();
			responseStream.Close();
			responseStream.Dispose();
			httpWebResponse.Close();
			text = responseFromServer;
		}
		catch
		{
			text = "";
		}
		return text;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002764 File Offset: 0x00000964
	public static string GetMarkeredText(string BPMarker, string EPMarker, string HTML, ref int StartPos)
	{
		int BeginPos = HTML.IndexOf(BPMarker, StartPos, StringComparison.InvariantCultureIgnoreCase);
		if (BeginPos <= -1)
		{
			return "";
		}
		int EndPos = HTML.IndexOf(EPMarker, BeginPos, StringComparison.InvariantCultureIgnoreCase);
		if (EndPos > -1)
		{
			StartPos = EndPos + EPMarker.Length;
			string Text = "";
			try
			{
				Text = HTML.Substring(BeginPos + BPMarker.Length, EndPos - BeginPos - BPMarker.Length);
			}
			catch
			{
			}
			return Text;
		}
		StartPos = HTML.Length - 1;
		string Text2 = "";
		try
		{
			Text2 = HTML.Substring(BeginPos + BPMarker.Length, HTML.Length - BeginPos - BPMarker.Length);
		}
		catch
		{
		}
		return Text2;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002814 File Offset: 0x00000A14
	public static string ClearTags(string HTML)
	{
		HTML = HTML.Trim().Replace("\n", string.Empty);
		HTML = HTML.Trim().Replace("\r", string.Empty);
		HTML = HTML.Trim().Replace("\t", string.Empty);
		HTML = HTML.Trim().Replace("&nbsp;", " ");
		return Regex.Replace(HTML, "<[^>]*>", " ").Trim();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002894 File Offset: 0x00000A94
	public static List<string[]> ParseHTML(string html, string pattern)
	{
		if (string.IsNullOrEmpty(html))
		{
			return new List<string[]>();
		}
		if (string.IsNullOrEmpty(pattern))
		{
			throw new ArgumentException("pattern cannot be null or empty.", "pattern");
		}
		RegexOptions options = RegexOptions.Singleline;
		Regex regex = new Regex(pattern, options);
		return HTTPScraper.ParseHTML(html, regex);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000028DC File Offset: 0x00000ADC
	public static List<string[]> ParseHTML(string html, Regex regex)
	{
		if (regex == null)
		{
			throw new ArgumentNullException("regex");
		}
		if (string.IsNullOrEmpty(html))
		{
			return new List<string[]>();
		}
		MatchCollection matchCollection = regex.Matches(html);
		List<string[]> results = new List<string[]>(matchCollection.Count);
		foreach (object obj in matchCollection)
		{
			Match match = (Match)obj;
			string[] values = new string[match.Groups.Count];
			for (int i = 0; i < match.Groups.Count; i++)
			{
				values[i] = match.Groups[i].Value;
			}
			results.Add(values);
		}
		return results;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000029A4 File Offset: 0x00000BA4
	public static string ClearString(string Source)
	{
		Source = Source.Replace("   ", " ");
		char[] Result = Source.ToCharArray();
		char[] CharsToRemove = new char[] { '\n', '\r', '\t' };
		for (int i = 0; i < Source.Length - 1; i++)
		{
			if (Source[i] == ' ' && Source[i + 1] == ' ')
			{
				Result[i] = '*';
				Result[i + 1] = '*';
			}
			for (int j = 0; j < CharsToRemove.Length; j++)
			{
				if (Result[i] == CharsToRemove[j])
				{
					Result[i] = '*';
				}
			}
		}
		return new string(Result).Replace("*", "");
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002A43 File Offset: 0x00000C43
	public static string ClearValue(string Value)
	{
		if (Value != null)
		{
			return Value.Replace("&#x27", "");
		}
		return "";
	}

	// Token: 0x02000079 RID: 121
	public struct Brand
	{
		// Token: 0x04000201 RID: 513
		public string Name;

		// Token: 0x04000202 RID: 514
		public string Url;
	}

	// Token: 0x0200007A RID: 122
	public struct Parameter
	{
		// Token: 0x04000203 RID: 515
		public string Name;

		// Token: 0x04000204 RID: 516
		public string Value;
	}
}

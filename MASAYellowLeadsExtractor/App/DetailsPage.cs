using System;
using System.Collections.Generic;

namespace MASAYellowLeadsExtractor
{
	// Token: 0x02000009 RID: 9
	internal static class DetailsPage
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00003724 File Offset: 0x00001924
		public static string GetPage(Settings AppSettings, string PageUrl, List<ProxyServer> Proxies)
		{
			string Page = "";
			if (AppSettings.ConnectionType == 0 || AppSettings.ConnectionType == 4)
			{
				Page = HTTPScraper.GetPage(PageUrl, null);
			}
			else if (AppSettings.ConnectionType == 1)
			{
				ProxyServer ps = new ProxyServer
				{
					IP = AppSettings.ProxyServer,
					Port = AppSettings.ProxyPort
				};
				Page = HTTPScraper.GetPage(PageUrl, ps);
			}
			else if (AppSettings.ConnectionType == 2)
			{
				int ProxyIndex = Program.Rnd.Next(AppSettings.ProxyList.Length);
				string[] array = AppSettings.ProxyList[ProxyIndex].Split(new char[] { ':' });
				string IP = array[0];
				int Port = 0;
				int.TryParse(array[1], out Port);
				ProxyServer ps2 = new ProxyServer
				{
					IP = IP,
					Port = Port
				};
				Page = HTTPScraper.GetPage(PageUrl, ps2);
			}
			else if (AppSettings.ConnectionType == 3)
			{
				do
				{
					int ProxyIndex2 = Program.Rnd.Next(Proxies.Count);
					ProxyServer ps3 = Proxies[ProxyIndex2];
					Page = HTTPScraper.GetPage(PageUrl, ps3);
				}
				while (Page == "");
			}
			return HTTPScraper.ClearString(Page);
		}
	}
}

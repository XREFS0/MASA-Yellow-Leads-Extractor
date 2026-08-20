using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200003F RID: 63
	public class BrasilDataScraper
	{
		// Token: 0x060001BB RID: 443 RVA: 0x00021644 File Offset: 0x0001F844
		public BrasilDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000216B0 File Offset: 0x0001F8B0
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			this.Data.Country = "Brasil";
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<li class=\"site\">(.*?)</li>");
			if (Items.Count > 0)
			{
				string text = Items[0][1];
				string pattern = "\\?url=([^\"&]+)";
				Match match = Regex.Match(text, pattern);
				if (match.Success)
				{
					this.Data.Website = HttpUtility.UrlDecode(match.Groups[1].Value);
					if (Program.AppSettings.ExtractEmails)
					{
						this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta" });
					}
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<a id=\"btn-whatsapp-scroll-desktop\"(.*?)href=\"(.*?)\"");
			if (Items.Count > 0)
			{
				string text2 = Items[0][2];
				string pattern2 = "phone%3D(.*?)text";
				Match match2 = Regex.Match(text2, pattern2);
				if (match2.Success)
				{
					this.Data.Fax = "WhatsApp: " + match2.Groups[1].Value.Split(new char[] { '%' })[0];
				}
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x040000E7 RID: 231
		private Thread MainThread;

		// Token: 0x040000E8 RID: 232
		private string PageUrl;

		// Token: 0x040000E9 RID: 233
		private Settings AppSettings;

		// Token: 0x040000EA RID: 234
		private List<ProxyServer> Proxies;

		// Token: 0x040000EB RID: 235
		public bool IsDone;

		// Token: 0x040000EC RID: 236
		public DataItem Data;
	}
}

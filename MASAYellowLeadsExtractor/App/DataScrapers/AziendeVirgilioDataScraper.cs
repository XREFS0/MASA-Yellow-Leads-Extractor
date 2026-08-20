using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200003E RID: 62
	public class AziendeVirgilioDataScraper
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x000213F0 File Offset: 0x0001F5F0
		public AziendeVirgilioDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0002145C File Offset: 0x0001F65C
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<!--sito--><li><a href=\"(.*?)\" target=\"_blank\" title");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			else
			{
				this.Data.Website = "";
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "href=\"tel:0(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Phone = "+39 0" + Items[0][1];
			}
			else
			{
				List<string[]> Items2 = HTTPScraper.ParseHTML(ClearPage, "href=\"tel:3(.*?)\"");
				if (Items2.Count > 0)
				{
					this.Data.Phone = "+39 3" + Items2[0][1];
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "href=\"mailto:(.*?)\" title=\"\"><span class=\"ico scrivi\"></span>");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1].Trim();
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"streetAddress\" class=\"street-address\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.Address = Items[0][1].Trim();
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"postal-code\" class=\"postalCode\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.PostalCode = Items[0][1].Trim();
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"addressLocality\" class=\"region\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][1].Trim();
			}
			if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta" });
			}
			this.IsDone = true;
		}

		// Token: 0x040000E1 RID: 225
		private Thread MainThread;

		// Token: 0x040000E2 RID: 226
		private string PageUrl;

		// Token: 0x040000E3 RID: 227
		private Settings AppSettings;

		// Token: 0x040000E4 RID: 228
		private List<ProxyServer> Proxies;

		// Token: 0x040000E5 RID: 229
		public bool IsDone;

		// Token: 0x040000E6 RID: 230
		public DataItem Data;
	}
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000056 RID: 86
	public class YelpFRDataScraper
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x00023B08 File Offset: 0x00021D08
		public YelpFRDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00023B74 File Offset: 0x00021D74
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(page, "<div class=\"categoria\"><span>Categoria: </span><a href=\"(.*?)\" rel=\"follow\">(.*?)</a>");
			if (Items.Count > 0)
			{
				this.Data.Category = Items[0][2].Replace("&nbsp;", " ").Replace("&amp;", "&");
			}
			Items = HTTPScraper.ParseHTML(page, "<span itemprop=\"addressLocality\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "<span itemprop=\"streetAddress\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.Address = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "<span itemprop=\"addressRegion\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.State = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "<span itemprop=\"postalCode\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.PostalCode = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "<meta content=\"(.*?)\" itemprop=\"addressCountry\">");
			if (Items.Count > 0)
			{
				this.Data.Country = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "<a href=\"(.*?)\" target=\"_blank\" rel=\"noopener\">(.*?)</a>");
			if (Items.Count > 0)
			{
				this.Data.Website = "http://www." + Items[0][2];
			}
			if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "konta", "conta" });
			}
			this.Data.MapLink = this.PageUrl.Replace("https", "http");
			this.IsDone = true;
		}

		// Token: 0x04000171 RID: 369
		private Thread MainThread;

		// Token: 0x04000172 RID: 370
		private string PageUrl;

		// Token: 0x04000173 RID: 371
		private Settings AppSettings;

		// Token: 0x04000174 RID: 372
		private List<ProxyServer> Proxies;

		// Token: 0x04000175 RID: 373
		public bool IsDone;

		// Token: 0x04000176 RID: 374
		public DataItem Data;
	}
}

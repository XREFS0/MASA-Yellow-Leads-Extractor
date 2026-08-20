using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000058 RID: 88
	public class YelpDataScraper
	{
		// Token: 0x060001ED RID: 493 RVA: 0x00024058 File Offset: 0x00022258
		public YelpDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000240C4 File Offset: 0x000222C4
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

		// Token: 0x0400017D RID: 381
		private Thread MainThread;

		// Token: 0x0400017E RID: 382
		private string PageUrl;

		// Token: 0x0400017F RID: 383
		private Settings AppSettings;

		// Token: 0x04000180 RID: 384
		private List<ProxyServer> Proxies;

		// Token: 0x04000181 RID: 385
		public bool IsDone;

		// Token: 0x04000182 RID: 386
		public DataItem Data;
	}
}

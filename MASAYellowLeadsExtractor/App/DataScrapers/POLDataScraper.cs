using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200004D RID: 77
	internal class POLDataScraper
	{
		// Token: 0x060001D7 RID: 471 RVA: 0x00022AA8 File Offset: 0x00020CA8
		public POLDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00022B14 File Offset: 0x00020D14
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> ItemsA = HTTPScraper.ParseHTML(page, "<dt>Slowa kluczowe</dt><dd><a href=\"(.*?)\" rel=\"nofollow\">(.*?)</a>");
			if (ItemsA.Count > 0)
			{
				this.Data.Category = ItemsA[0][2];
			}
			List<string[]> Addr = HTTPScraper.ParseHTML(page, "<span itemprop=\"streetAddress\">(.*?)</span>");
			if (Addr.Count > 0)
			{
				this.Data.Address = Addr[0][1];
			}
			List<string[]> Addr2 = HTTPScraper.ParseHTML(page, "<span itemprop=\"postalCode\">(.*?)</span>");
			if (Addr2.Count > 0)
			{
				this.Data.PostalCode = Addr2[0][1];
			}
			List<string[]> Addr3 = HTTPScraper.ParseHTML(page, "<span itemprop=\"addressLocality\">(.*?)</span>");
			if (Addr3.Count > 0)
			{
				this.Data.City = Addr3[0][1];
			}
			List<string[]> Addr4 = HTTPScraper.ParseHTML(page, "<span itemprop=\"addressCountry\">(.*?)</span>");
			if (Addr4.Count > 0)
			{
				this.Data.Country = Addr4[0][1];
			}
			List<string[]> ItemsB = HTTPScraper.ParseHTML(page, "<span class=\"phone-header\" data-phone-number=\"(.*?)\">");
			if (ItemsB.Count > 0)
			{
				this.Data.Phone = ItemsB[0][1];
			}
			List<string[]> Items = HTTPScraper.ParseHTML(page, "<a class=\"btn-link\" href=\"(.*?)\" target=\"_blank\" rel=\"nofollow\">(.*?)</a>");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][2];
			}
			if (this.AppSettings.ExtractEmails)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta", "konta" });
			}
			this.IsDone = true;
		}

		// Token: 0x0400013B RID: 315
		private Thread MainThread;

		// Token: 0x0400013C RID: 316
		private string PageUrl;

		// Token: 0x0400013D RID: 317
		private Settings AppSettings;

		// Token: 0x0400013E RID: 318
		private List<ProxyServer> Proxies;

		// Token: 0x0400013F RID: 319
		public bool IsDone;

		// Token: 0x04000140 RID: 320
		public DataItem Data;
	}
}

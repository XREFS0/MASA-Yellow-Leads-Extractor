using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000048 RID: 72
	internal class LATDataScraper
	{
		// Token: 0x060001CD RID: 461 RVA: 0x00022370 File Offset: 0x00020570
		public LATDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000223DC File Offset: 0x000205DC
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"MainInfo_categories__EfphS\"><a href=\"(.*?)\">(.*?)</a>");
			if (Items.Count > 0)
			{
				this.Data.Category = Items[0][2];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<a href=\"#map\" class=\"MainInfo_info-icon__seci4\"><span>(.*?)</span></div>(.*?)</a>");
			if (Items.Count > 0)
			{
				this.Data.Address = Items[0][2].Split(new char[] { '-' })[0];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "http://wa.me/(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Fax = "WhatsApp: " + Items[0][1];
			}
			this.IsDone = true;
		}

		// Token: 0x0400011D RID: 285
		private Thread MainThread;

		// Token: 0x0400011E RID: 286
		private string PageUrl;

		// Token: 0x0400011F RID: 287
		private Settings AppSettings;

		// Token: 0x04000120 RID: 288
		private List<ProxyServer> Proxies;

		// Token: 0x04000121 RID: 289
		public bool IsDone;

		// Token: 0x04000122 RID: 290
		public DataItem Data;
	}
}

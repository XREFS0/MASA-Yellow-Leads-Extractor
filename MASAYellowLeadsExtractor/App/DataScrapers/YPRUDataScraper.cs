using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000054 RID: 84
	public class YPRUDataScraper
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x000237C4 File Offset: 0x000219C4
		public YPRUDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00023830 File Offset: 0x00021A30
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			Items = HTTPScraper.ParseHTML(page, "href=\"mailto:(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "<a class=\"biz-link d-block ellipsis yp-click website-link\" href=\"(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x04000165 RID: 357
		private Thread MainThread;

		// Token: 0x04000166 RID: 358
		private string PageUrl;

		// Token: 0x04000167 RID: 359
		private Settings AppSettings;

		// Token: 0x04000168 RID: 360
		private List<ProxyServer> Proxies;

		// Token: 0x04000169 RID: 361
		public bool IsDone;

		// Token: 0x0400016A RID: 362
		public DataItem Data;
	}
}

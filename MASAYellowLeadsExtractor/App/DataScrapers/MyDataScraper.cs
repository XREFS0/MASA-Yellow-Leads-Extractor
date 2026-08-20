using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000049 RID: 73
	internal class MyDataScraper
	{
		// Token: 0x060001CF RID: 463 RVA: 0x000224A4 File Offset: 0x000206A4
		public MyDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00022510 File Offset: 0x00020710
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(page, "<a href=\"mailto:(.*?)\">Email:");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "Website : (.*?)");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "class=\"ng-tns-c91-0 ng-star-inserted\">(.*?)>/span>");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][1];
			}
			this.IsDone = true;
		}

		// Token: 0x04000123 RID: 291
		private Thread MainThread;

		// Token: 0x04000124 RID: 292
		private string PageUrl;

		// Token: 0x04000125 RID: 293
		private Settings AppSettings;

		// Token: 0x04000126 RID: 294
		private List<ProxyServer> Proxies;

		// Token: 0x04000127 RID: 295
		public bool IsDone;

		// Token: 0x04000128 RID: 296
		public DataItem Data;
	}
}

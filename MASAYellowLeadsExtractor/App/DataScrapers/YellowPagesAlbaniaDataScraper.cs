using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200005A RID: 90
	internal class YellowPagesAlbaniaDataScraper
	{
		// Token: 0x060001F1 RID: 497 RVA: 0x00024674 File Offset: 0x00022874
		public YellowPagesAlbaniaDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000246E0 File Offset: 0x000228E0
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> SubItems = HTTPScraper.ParseHTML(ClearPage, "<a href=\"tel: (.*?)\">(.*?)</a>");
			if (SubItems.Count > 0)
			{
				this.Data.Phone = SubItems[0][1];
			}
			if (this.Data.Website == "" && HTTPScraper.ParseHTML(ClearPage, "<span id=\"web\" class=\"web\"><a href=\"(.*?)\" target=\"_blank\">(.*?)</a></span>").Count > 0)
			{
				this.Data.Website = SubItems[0][2];
			}
			this.IsDone = true;
		}

		// Token: 0x04000189 RID: 393
		private Thread MainThread;

		// Token: 0x0400018A RID: 394
		private string PageUrl;

		// Token: 0x0400018B RID: 395
		private Settings AppSettings;

		// Token: 0x0400018C RID: 396
		private List<ProxyServer> Proxies;

		// Token: 0x0400018D RID: 397
		public bool IsDone;

		// Token: 0x0400018E RID: 398
		public DataItem Data;
	}
}

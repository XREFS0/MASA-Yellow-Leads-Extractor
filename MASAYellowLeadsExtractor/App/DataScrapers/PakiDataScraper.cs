using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200004C RID: 76
	internal class PakiDataScraper
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x000229A0 File Offset: 0x00020BA0
		public PakiDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00022A0C File Offset: 0x00020C0C
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			if (this.Data.Website == "")
			{
				List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "</div><a href=\"(.*?)\">Web Links</a><ul>");
				if (Items.Count > 0)
				{
					this.Data.Website = Items[0][1];
				}
			}
			this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
			this.IsDone = true;
		}

		// Token: 0x04000135 RID: 309
		private Thread MainThread;

		// Token: 0x04000136 RID: 310
		private string PageUrl;

		// Token: 0x04000137 RID: 311
		private Settings AppSettings;

		// Token: 0x04000138 RID: 312
		private List<ProxyServer> Proxies;

		// Token: 0x04000139 RID: 313
		public bool IsDone;

		// Token: 0x0400013A RID: 314
		public DataItem Data;
	}
}

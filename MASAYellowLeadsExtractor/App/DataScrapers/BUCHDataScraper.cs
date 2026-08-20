using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000040 RID: 64
	internal class BUCHDataScraper
	{
		// Token: 0x060001BD RID: 445 RVA: 0x0002180C File Offset: 0x0001FA0C
		public BUCHDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00021878 File Offset: 0x0001FA78
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

		// Token: 0x040000ED RID: 237
		private Thread MainThread;

		// Token: 0x040000EE RID: 238
		private string PageUrl;

		// Token: 0x040000EF RID: 239
		private Settings AppSettings;

		// Token: 0x040000F0 RID: 240
		private List<ProxyServer> Proxies;

		// Token: 0x040000F1 RID: 241
		public bool IsDone;

		// Token: 0x040000F2 RID: 242
		public DataItem Data;
	}
}

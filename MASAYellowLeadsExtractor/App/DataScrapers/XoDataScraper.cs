using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200004F RID: 79
	internal class XoDataScraper
	{
		// Token: 0x060001DB RID: 475 RVA: 0x00022E88 File Offset: 0x00021088
		public XoDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00022EF4 File Offset: 0x000210F4
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

		// Token: 0x04000147 RID: 327
		private Thread MainThread;

		// Token: 0x04000148 RID: 328
		private string PageUrl;

		// Token: 0x04000149 RID: 329
		private Settings AppSettings;

		// Token: 0x0400014A RID: 330
		private List<ProxyServer> Proxies;

		// Token: 0x0400014B RID: 331
		public bool IsDone;

		// Token: 0x0400014C RID: 332
		public DataItem Data;
	}
}

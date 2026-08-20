using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000047 RID: 71
	internal class JustDataScraper
	{
		// Token: 0x060001CB RID: 459 RVA: 0x00022268 File Offset: 0x00020468
		public JustDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000222D4 File Offset: 0x000204D4
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

		// Token: 0x04000117 RID: 279
		private Thread MainThread;

		// Token: 0x04000118 RID: 280
		private string PageUrl;

		// Token: 0x04000119 RID: 281
		private Settings AppSettings;

		// Token: 0x0400011A RID: 282
		private List<ProxyServer> Proxies;

		// Token: 0x0400011B RID: 283
		public bool IsDone;

		// Token: 0x0400011C RID: 284
		public DataItem Data;
	}
}

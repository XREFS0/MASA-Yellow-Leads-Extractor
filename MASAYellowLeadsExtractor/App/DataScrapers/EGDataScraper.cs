using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000042 RID: 66
	internal class EGDataScraper
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x00021A04 File Offset: 0x0001FC04
		public EGDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00021A70 File Offset: 0x0001FC70
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

		// Token: 0x040000F9 RID: 249
		private Thread MainThread;

		// Token: 0x040000FA RID: 250
		private string PageUrl;

		// Token: 0x040000FB RID: 251
		private Settings AppSettings;

		// Token: 0x040000FC RID: 252
		private List<ProxyServer> Proxies;

		// Token: 0x040000FD RID: 253
		public bool IsDone;

		// Token: 0x040000FE RID: 254
		public DataItem Data;
	}
}

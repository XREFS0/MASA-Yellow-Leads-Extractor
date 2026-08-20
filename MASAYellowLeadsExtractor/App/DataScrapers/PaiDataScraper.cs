using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200004B RID: 75
	internal class PaiDataScraper
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x00022860 File Offset: 0x00020A60
		public PaiDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000228CC File Offset: 0x00020ACC
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(page, "<a href=\"mailto:(.*?)\">(.*?)</a>");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][2];
			}
			Items = HTTPScraper.ParseHTML(page, "data-trackable-event=\"visit-webpage\" href=\"(.*?)\">visitar website</a>");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "tel:(.*?)\">");
			if (Items.Count > 0)
			{
				this.Data.Phone = " " + Items[0][1];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x0400012F RID: 303
		private Thread MainThread;

		// Token: 0x04000130 RID: 304
		private string PageUrl;

		// Token: 0x04000131 RID: 305
		private Settings AppSettings;

		// Token: 0x04000132 RID: 306
		private List<ProxyServer> Proxies;

		// Token: 0x04000133 RID: 307
		public bool IsDone;

		// Token: 0x04000134 RID: 308
		public DataItem Data;
	}
}

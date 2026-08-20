using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000045 RID: 69
	public class DetelefoongidsDataScraper
	{
		// Token: 0x060001C7 RID: 455 RVA: 0x00021FE0 File Offset: 0x000201E0
		public DetelefoongidsDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0002204C File Offset: 0x0002024C
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			this.Data.Country = "Holland";
			if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "" && this.Data.Email == "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta" });
			}
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<a href=\"(.*?)\" target=\"_blank\" class=\"button route\" rel=\"nofollow\"");
			if (Items.Count > 0)
			{
				this.Data.MapLink = Items[0][1];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x0400010B RID: 267
		private Thread MainThread;

		// Token: 0x0400010C RID: 268
		private string PageUrl;

		// Token: 0x0400010D RID: 269
		private Settings AppSettings;

		// Token: 0x0400010E RID: 270
		private List<ProxyServer> Proxies;

		// Token: 0x0400010F RID: 271
		public bool IsDone;

		// Token: 0x04000110 RID: 272
		public DataItem Data;
	}
}

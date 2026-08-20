using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000046 RID: 70
	internal class InfobelDataScraper
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x00022148 File Offset: 0x00020348
		public InfobelDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000221B4 File Offset: 0x000203B4
		public void ProcessPage()
		{
			Program.RequestDelay();
			List<string[]> Items = HTTPScraper.ParseHTML(DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies), "<a class=\"customer-info-detail\" rel=\"noopener external\" href=\"(.*?)\" targer=\"_blank\"");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta" });
			}
			this.IsDone = true;
		}

		// Token: 0x04000111 RID: 273
		private Thread MainThread;

		// Token: 0x04000112 RID: 274
		private string PageUrl;

		// Token: 0x04000113 RID: 275
		private Settings AppSettings;

		// Token: 0x04000114 RID: 276
		private List<ProxyServer> Proxies;

		// Token: 0x04000115 RID: 277
		public bool IsDone;

		// Token: 0x04000116 RID: 278
		public DataItem Data;
	}
}

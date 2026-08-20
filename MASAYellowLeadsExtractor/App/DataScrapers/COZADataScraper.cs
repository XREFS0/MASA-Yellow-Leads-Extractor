using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000041 RID: 65
	internal class COZADataScraper
	{
		// Token: 0x060001BF RID: 447 RVA: 0x00021914 File Offset: 0x0001FB14
		public COZADataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00021980 File Offset: 0x0001FB80
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(page, "href=\"mailto:(.*?)\">");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "title=\"website\" href=\"(.*?)\">");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			this.IsDone = true;
		}

		// Token: 0x040000F3 RID: 243
		private Thread MainThread;

		// Token: 0x040000F4 RID: 244
		private string PageUrl;

		// Token: 0x040000F5 RID: 245
		private Settings AppSettings;

		// Token: 0x040000F6 RID: 246
		private List<ProxyServer> Proxies;

		// Token: 0x040000F7 RID: 247
		public bool IsDone;

		// Token: 0x040000F8 RID: 248
		public DataItem Data;
	}
}

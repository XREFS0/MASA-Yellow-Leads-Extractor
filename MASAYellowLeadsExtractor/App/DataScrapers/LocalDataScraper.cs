using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200005E RID: 94
	public class LocalDataScraper
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x00024D20 File Offset: 0x00022F20
		public LocalDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.PageUrl = SourceDataItem.DetailsLink;
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00024D98 File Offset: 0x00022F98
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			new List<string[]>();
			List<string[]> Website = HTTPScraper.ParseHTML(page, "href=\"http(.*?)\">www.");
			if (Website.Count > 0)
			{
				this.Data.Website = "http" + Website[0][1];
			}
			List<string[]> Email = HTTPScraper.ParseHTML(page, "href=\"mailto:(.*?)\"");
			if (Email.Count > 0)
			{
				this.Data.Email = Email[0][1].Split(new char[] { '\'' })[0];
			}
			else if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x040001A1 RID: 417
		private Thread MainThread;

		// Token: 0x040001A2 RID: 418
		private string PageUrl;

		// Token: 0x040001A3 RID: 419
		private Settings AppSettings;

		// Token: 0x040001A4 RID: 420
		private List<ProxyServer> Proxies;

		// Token: 0x040001A5 RID: 421
		public bool IsDone;

		// Token: 0x040001A6 RID: 422
		public DataItem Data;
	}
}

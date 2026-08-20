using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000044 RID: 68
	public class GoldenPagesDataScraper
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x00021E50 File Offset: 0x00020050
		public GoldenPagesDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00021EBC File Offset: 0x000200BC
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			new List<string[]>();
			List<string[]> Website = HTTPScraper.ParseHTML(page, "href=\"http://(.*?).utm_source=fcrmedia(.*?)\"");
			if (Website.Count > 0)
			{
				this.Data.Website = Website[0][1];
			}
			List<string[]> Phone = HTTPScraper.ParseHTML(page, "href=\"tel:(.*?)\" data-ta=\"PhoneNumberClick\"");
			if (Phone.Count > 0)
			{
				this.Data.Phone = Phone[0][1];
			}
			List<string[]> Email = HTTPScraper.ParseHTML(page, "<a href=\"mailto:(.*?)\" class=\"t-c btn btn--action btn--border btn--icon\"");
			if (Email.Count > 0)
			{
				this.Data.Email = Email[0][1];
			}
			if (this.Data.Email == "" && this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
			}
			this.IsDone = true;
		}

		// Token: 0x04000105 RID: 261
		private Thread MainThread;

		// Token: 0x04000106 RID: 262
		private string PageUrl;

		// Token: 0x04000107 RID: 263
		private Settings AppSettings;

		// Token: 0x04000108 RID: 264
		private List<ProxyServer> Proxies;

		// Token: 0x04000109 RID: 265
		public bool IsDone;

		// Token: 0x0400010A RID: 266
		public DataItem Data;
	}
}

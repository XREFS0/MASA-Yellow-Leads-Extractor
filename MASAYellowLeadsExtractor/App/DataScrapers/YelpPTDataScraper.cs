using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000053 RID: 83
	public class YelpPTDataScraper
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x000235FC File Offset: 0x000217FC
		public YelpPTDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00023668 File Offset: 0x00021868
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(page, "<meta itemprop=\"url\" content=\"(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, ",\"faxNumber\":(.*?),");
			if (Items.Count > 0)
			{
				this.Data.Fax = Items[0][1].Replace("[", "").Replace("]", "").Replace("\"", "");
			}
			Items = HTTPScraper.ParseHTML(page, ",\"email\":\"(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1];
			}
			else if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "konta", "conta" });
			}
			this.Data.MapLink = this.PageUrl.Replace("https", "http");
			this.IsDone = true;
		}

		// Token: 0x0400015F RID: 351
		private Thread MainThread;

		// Token: 0x04000160 RID: 352
		private string PageUrl;

		// Token: 0x04000161 RID: 353
		private Settings AppSettings;

		// Token: 0x04000162 RID: 354
		private List<ProxyServer> Proxies;

		// Token: 0x04000163 RID: 355
		public bool IsDone;

		// Token: 0x04000164 RID: 356
		public DataItem Data;
	}
}

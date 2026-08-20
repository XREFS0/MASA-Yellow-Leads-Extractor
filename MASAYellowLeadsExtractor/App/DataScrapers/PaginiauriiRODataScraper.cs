using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200005F RID: 95
	internal class PaginiauriiRODataScraper
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00024EB8 File Offset: 0x000230B8
		public PaginiauriiRODataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00024F24 File Offset: 0x00023124
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			Items = HTTPScraper.ParseHTML(page, "<i class=\"icon-link\"></i><a itemprop=\"url\" href=\"([^\"]+)\" target=\"_blank\"");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "<i class=\"icon-fax\" title=\"fax\"></i>(.*?)<span");
			if (Items.Count > 0)
			{
				this.Data.Fax = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(page, "<i class=\"icon-email\" title=\"e-mail\"></i><a href=\"mailto:(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1];
			}
			else
			{
				this.Data.Email = "";
			}
			if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "" && this.Data.Email == "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta" });
			}
			this.Data.MapLink = "";
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x040001A7 RID: 423
		private Thread MainThread;

		// Token: 0x040001A8 RID: 424
		private string PageUrl;

		// Token: 0x040001A9 RID: 425
		private Settings AppSettings;

		// Token: 0x040001AA RID: 426
		private List<ProxyServer> Proxies;

		// Token: 0x040001AB RID: 427
		public bool IsDone;

		// Token: 0x040001AC RID: 428
		public DataItem Data;
	}
}

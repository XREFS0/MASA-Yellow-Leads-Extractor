using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000060 RID: 96
	public class YellowPagesCaDataScraper
	{
		// Token: 0x060001FD RID: 509 RVA: 0x00025088 File Offset: 0x00023288
		public YellowPagesCaDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000250F4 File Offset: 0x000232F4
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			List<string[]> Phone = HTTPScraper.ParseHTML(page, "<span class=\"mlr__sub-text\"(.*?)itemprop=\"telephone\">(.*?)</span>");
			if (Phone.Count > 0)
			{
				this.Data.Phone = Phone[0][1];
			}
			if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
				Items = HTTPScraper.ParseHTML(HTTPScraper.ClearString(HTTPScraper.GetPage(this.Data.Website.Replace("https:", "http:"), null)), "([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)");
				if (Items.Count > 0)
				{
					foreach (string[] email in Items)
					{
						if (email[0].IndexOf("mail.com", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf("example", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf(".png", StringComparison.InvariantCultureIgnoreCase) == -1)
						{
							this.Data.Email = email[0];
							break;
						}
					}
				}
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x040001AD RID: 429
		private Thread MainThread;

		// Token: 0x040001AE RID: 430
		private string PageUrl;

		// Token: 0x040001AF RID: 431
		private Settings AppSettings;

		// Token: 0x040001B0 RID: 432
		private List<ProxyServer> Proxies;

		// Token: 0x040001B1 RID: 433
		public bool IsDone;

		// Token: 0x040001B2 RID: 434
		public DataItem Data;
	}
}

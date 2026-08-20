using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000061 RID: 97
	public class YellowPagesDataScraper
	{
		// Token: 0x060001FF RID: 511 RVA: 0x0002528C File Offset: 0x0002348C
		public YellowPagesDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000252F8 File Offset: 0x000234F8
		public void ProcessPage()
		{
			Thread.Sleep(100);
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			Items = HTTPScraper.ParseHTML(ClearPage, "mailto:(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1].Split(new char[] { '"' })[0];
			}
			else if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "" });
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<h2 class=\"address\">(.*?)</h2>");
			if (Items.Count > 0)
			{
				this.Data.Address = Items[0][1].Replace("<span>", "").Replace("</span>", "-");
				List<string[]> AddressParts = HTTPScraper.ParseHTML(Items[0][1], "</span>(.*?), (.*?) (\\d+)");
				if (AddressParts.Count > 0)
				{
					this.Data.City = AddressParts[0][1];
					this.Data.State = AddressParts[0][2];
					this.Data.PostalCode = AddressParts[0][3];
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<div id=\"mapcontainer\" data-lon=\"(.*?)\" data-lat=\"(.*?)\" data-percorso=\"(.*?)\">");
			if (Items.Count > 0)
			{
				this.Data.MapLink = Items[0][3];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x040001B3 RID: 435
		private Thread MainThread;

		// Token: 0x040001B4 RID: 436
		private string PageUrl;

		// Token: 0x040001B5 RID: 437
		private Settings AppSettings;

		// Token: 0x040001B6 RID: 438
		private List<ProxyServer> Proxies;

		// Token: 0x040001B7 RID: 439
		public bool IsDone;

		// Token: 0x040001B8 RID: 440
		public DataItem Data;
	}
}

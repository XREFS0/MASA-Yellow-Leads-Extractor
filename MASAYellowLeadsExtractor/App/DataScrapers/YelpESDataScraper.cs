using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000051 RID: 81
	public class YelpESDataScraper
	{
		// Token: 0x060001DF RID: 479 RVA: 0x000232AC File Offset: 0x000214AC
		public YelpESDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00023318 File Offset: 0x00021518
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			Items = HTTPScraper.ParseHTML(page, "<span itemprop=\"streetAddress\"(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.Address = Items[0][1].Replace(" - ", "-");
			}
			Items = HTTPScraper.ParseHTML(page, "<span itemprop=\"addressLocality\"(.*?)>");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][1].Replace("content", "");
			}
			Items = HTTPScraper.ParseHTML(page, "<span itemprop=\"addressRegion\"(.*?)>");
			if (Items.Count > 0)
			{
				this.Data.State = Items[0][1].Replace("content", "");
			}
			Items = HTTPScraper.ParseHTML(page, "<dt>Tipo di veicolo</dt><dd><a href=\"(.*?)\">(.*?)</a></dd>");
			if (Items.Count > 0)
			{
				this.Data.PostalCode = Items[0][2];
			}
			Items = HTTPScraper.ParseHTML(page, "<span class=\"sc-font-l cldt-stage-primary-keyfact\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.MapLink = Items[0][1];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x04000153 RID: 339
		private Thread MainThread;

		// Token: 0x04000154 RID: 340
		private string PageUrl;

		// Token: 0x04000155 RID: 341
		private Settings AppSettings;

		// Token: 0x04000156 RID: 342
		private List<ProxyServer> Proxies;

		// Token: 0x04000157 RID: 343
		public bool IsDone;

		// Token: 0x04000158 RID: 344
		public DataItem Data;
	}
}

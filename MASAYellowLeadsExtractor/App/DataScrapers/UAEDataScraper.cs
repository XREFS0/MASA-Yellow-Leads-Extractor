using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200004E RID: 78
	internal class UAEDataScraper
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x00022CAC File Offset: 0x00020EAC
		public UAEDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00022D18 File Offset: 0x00020F18
		public void ProcessPage()
		{
			Program.RequestDelay();
			string page = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			Thread.Sleep(500);
			List<string[]> Items = HTTPScraper.ParseHTML(page, "[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][0].Replace("yellowpagesfreelistings@gmail.com", "");
			}
			Items = HTTPScraper.ParseHTML(page, "Location : (.*?)</p>");
			if (Items.Count > 0)
			{
				this.Data.Address = Items[0][1].Replace("<span>", "").Replace("</span>", "");
			}
			Items = HTTPScraper.ParseHTML(page, "City : (.*?)</p>");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][1].Replace("<span>", "").Replace("</span>", "");
			}
			Items = HTTPScraper.ParseHTML(page, "P.O Box : (.*?)</p>");
			if (Items.Count > 0)
			{
				this.Data.PostalCode = Items[0][1].Replace("<span>", "").Replace("</span>", "");
			}
			Items = HTTPScraper.ParseHTML(page, "<button target=\"_blank\" title=\"(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			this.IsDone = true;
		}

		// Token: 0x04000141 RID: 321
		private Thread MainThread;

		// Token: 0x04000142 RID: 322
		private string PageUrl;

		// Token: 0x04000143 RID: 323
		private Settings AppSettings;

		// Token: 0x04000144 RID: 324
		private List<ProxyServer> Proxies;

		// Token: 0x04000145 RID: 325
		public bool IsDone;

		// Token: 0x04000146 RID: 326
		public DataItem Data;
	}
}

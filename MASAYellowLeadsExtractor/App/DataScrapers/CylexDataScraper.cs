using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000052 RID: 82
	public class CylexDataScraper
	{
		// Token: 0x060001E1 RID: 481 RVA: 0x00023468 File Offset: 0x00021668
		public CylexDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000234D4 File Offset: 0x000216D4
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			File.WriteAllText("debug.html", ClearPage);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"dp.address.city\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "url\":\"(.*?)\",");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "mailto:(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1].ToString();
			}
			else if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta" });
			}
			this.Data.MapLink = "";
			this.IsDone = true;
		}

		// Token: 0x04000159 RID: 345
		private Thread MainThread;

		// Token: 0x0400015A RID: 346
		private string PageUrl;

		// Token: 0x0400015B RID: 347
		private Settings AppSettings;

		// Token: 0x0400015C RID: 348
		private List<ProxyServer> Proxies;

		// Token: 0x0400015D RID: 349
		public bool IsDone;

		// Token: 0x0400015E RID: 350
		public DataItem Data;
	}
}

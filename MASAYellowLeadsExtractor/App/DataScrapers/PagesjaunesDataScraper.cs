using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200004A RID: 74
	public class PagesjaunesDataScraper
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x000225BC File Offset: 0x000207BC
		public PagesjaunesDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00022628 File Offset: 0x00020828
		public void ProcessPage()
		{
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"activite\">(.*?)</span>");
			if (items.Count<string[]>() > 0)
			{
				this.Data.Category = items[0][1];
			}
			items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"value\">http(.*?)</span>");
			if (items.Count<string[]>() > 0)
			{
				this.Data.Website = "http" + HTTPScraper.ClearTags(items[0][1].Replace("\"", "").Trim());
			}
			items = HTTPScraper.ParseHTML(ClearPage, "email\":\"(.*?)\",");
			if (items.Count<string[]>() > 0)
			{
				this.Data.Email = HTTPScraper.ClearTags(items[0][1].Replace("\"", "").Trim());
			}
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"cphMainPage_NomeInserzionista\" class=\"testocontenitoreInserzionista\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"cphMainPage_lblTesto\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.Address = Items[0][1].Replace(";", "-").Replace(",", "-").Replace("<br>", " ")
					.Replace("<br />", " ");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"cphMainPage_prephone\" class=\"prephone secondaMano\">(.*?)</span>");
			if (Items.Count > 0)
			{
				List<string[]> Items2 = HTTPScraper.ParseHTML(ClearPage, "<span id=\"cphMainPage_postphone\" class=\"postphone hidden\">(.*?)</span>");
				if (Items2.Count > 0)
				{
					this.Data.Phone = Items[0][1] + Items2[0][1];
				}
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.DetailsLink, new string[] { "conta" });
			}
			this.IsDone = true;
		}

		// Token: 0x04000129 RID: 297
		private Thread MainThread;

		// Token: 0x0400012A RID: 298
		private string PageUrl;

		// Token: 0x0400012B RID: 299
		private Settings AppSettings;

		// Token: 0x0400012C RID: 300
		private List<ProxyServer> Proxies;

		// Token: 0x0400012D RID: 301
		public bool IsDone;

		// Token: 0x0400012E RID: 302
		public DataItem Data;
	}
}

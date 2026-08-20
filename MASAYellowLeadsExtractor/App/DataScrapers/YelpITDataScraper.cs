using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000057 RID: 87
	public class YelpITDataScraper
	{
		// Token: 0x060001EB RID: 491 RVA: 0x00023D6C File Offset: 0x00021F6C
		public YelpITDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00023DD8 File Offset: 0x00021FD8
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"categoria\"><span>Categoria: </span><a href=\"(.*?)\" rel=\"follow\">(.*?)</a>");
			if (Items.Count > 0)
			{
				this.Data.Category = Items[0][2].Replace("&nbsp;", " ").Replace("&amp;", "&");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"dp.address.city\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"dp.address.address\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.Address = HTTPScraper.ClearTags(Items[0][1]);
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"dp.address.postalcode\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.PostalCode = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "id=\"dp.phone\">(.*?)</span>");
			if (Items.Count > 0)
			{
				string num = " " + Items[0][1];
				this.Data.Phone = num;
			}
			else
			{
				Items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"icdd\">(.*?)<a name=\"");
				if (Items.Count > 0)
				{
					string num2 = " " + Items[0][1];
					this.Data.Phone = num2;
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "Fax.: (.*?)<br>");
			if (Items.Count > 0)
			{
				this.Data.Fax = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "id=\"dp.url\" href=\"(.*?)\"(.*?)rel=\"nofollow noopener\"");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			else
			{
				Items = HTTPScraper.ParseHTML(ClearPage, "<a id=\"dp.url\"(.*?)href=\"(.*?)\" target=\"_blank\"");
				if (Items.Count > 0)
				{
					this.Data.Website = Items[0][2];
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "href=\"mailto:(.*?)\"");
			if (Items.Count > 0)
			{
				this.Data.Email = WebUtility.HtmlDecode(Items[0][1]).ToString();
			}
			else if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta" });
			}
			this.Data.MapLink = "";
			this.IsDone = true;
		}

		// Token: 0x04000177 RID: 375
		private Thread MainThread;

		// Token: 0x04000178 RID: 376
		private string PageUrl;

		// Token: 0x04000179 RID: 377
		private Settings AppSettings;

		// Token: 0x0400017A RID: 378
		private List<ProxyServer> Proxies;

		// Token: 0x0400017B RID: 379
		public bool IsDone;

		// Token: 0x0400017C RID: 380
		public DataItem Data;
	}
}

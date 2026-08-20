using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000050 RID: 80
	public class YelpCZDataScraper
	{
		// Token: 0x060001DD RID: 477 RVA: 0x00022F90 File Offset: 0x00021190
		public YelpCZDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00022FFC File Offset: 0x000211FC
		public void ProcessPage()
		{
			Thread.Sleep(500);
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"scheda-azienda__companyCategory\"(.*?)><span class=\"bttn__label\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.Category = Items[0][2].Split(new char[] { ',' })[0];
			}
			else
			{
				Items = HTTPScraper.ParseHTML(ClearPage, "<span title=\"(.*?)\" class=\"header-scheda__category text-12 text-medium color-grey ln-1\">");
				if (Items.Count > 0)
				{
					this.Data.Category = Items[0][1].Split(new char[] { ',' })[0];
				}
				else
				{
					this.Data.Category = "Privato";
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"postalCode\" : \"([^\"]+)\"");
			if (Items.Count > 0)
			{
				this.Data.PostalCode = Items[0][1];
			}
			else
			{
				List<string[]> Items2 = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"postalCode\">(.*?)</span>");
				if (Items2.Count > 0)
				{
					this.Data.PostalCode = Items2[0][1];
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"addressLocality\":\"([^\"]+)\"");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][1];
			}
			else
			{
				List<string[]> Items3 = HTTPScraper.ParseHTML(ClearPage, "<span class=\"locality\" itemprop=\"addressLocality\">(.*?)</span>");
				if (Items3.Count > 0)
				{
					this.Data.City = Items3[0][1];
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"addressRegion\":\"([^\"]+)\"");
			if (Items.Count > 0)
			{
				this.Data.State = Items[0][1];
			}
			this.Data.Country = "Italia";
			Items = HTTPScraper.ParseHTML(ClearPage, "data-pag=\"www\"(.*?)href=(.*?)target=\"_blank\"");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][2].Split(new char[] { ' ' })[0].Replace("\"", "");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"email\" : \"(.*?)\",");
			if (Items.Count > 0 && Items[0][1].IndexOf('@') > -1)
			{
				this.Data.Email = Items[0][1];
			}
			else if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contatti", "conta" });
			}
			this.Data.MapLink = this.PageUrl.Replace("https", "http");
			this.IsDone = true;
		}

		// Token: 0x0400014D RID: 333
		private Thread MainThread;

		// Token: 0x0400014E RID: 334
		private string PageUrl;

		// Token: 0x0400014F RID: 335
		private Settings AppSettings;

		// Token: 0x04000150 RID: 336
		private List<ProxyServer> Proxies;

		// Token: 0x04000151 RID: 337
		public bool IsDone;

		// Token: 0x04000152 RID: 338
		public DataItem Data;
	}
}

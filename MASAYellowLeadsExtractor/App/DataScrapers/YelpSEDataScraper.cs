using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000055 RID: 85
	public class YelpSEDataScraper
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x000238D8 File Offset: 0x00021AD8
		public YelpSEDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00023944 File Offset: 0x00021B44
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			File.WriteAllText("debug.html", ClearPage);
			List<string[]> Items = new List<string[]>();
			Items = HTTPScraper.ParseHTML(ClearPage, "{\"number\":\"(.*?)\",");
			if (Items.Count > 0)
			{
				this.Data.Phone = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "{\"name\":\"(.*?)\",");
			if (Items.Count > 0)
			{
				this.Data.Category = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, ",\"countryName\":\"(.*?)\",");
			if (Items.Count > 0)
			{
				this.Data.Country = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, ",\"municipality\":\"(.*?)\",");
			if (Items.Count > 0 && this.Data.State == null)
			{
				this.Data.City = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, ",\"region\":\"(.*?)\",");
			if (Items.Count > 0 && this.Data.State == null)
			{
				this.Data.State = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, ",\"postalCode\":\"(.*?)\",");
			if (Items.Count > 0)
			{
				this.Data.PostalCode = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "homepage\",\"url\":\"(.*?)\"}");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "email\",\"link\":\"(.*?)\"},");
			if (Items.Count > 0)
			{
				this.Data.Email = Items[0][1];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x0400016B RID: 363
		private Thread MainThread;

		// Token: 0x0400016C RID: 364
		private string PageUrl;

		// Token: 0x0400016D RID: 365
		private Settings AppSettings;

		// Token: 0x0400016E RID: 366
		private List<ProxyServer> Proxies;

		// Token: 0x0400016F RID: 367
		public bool IsDone;

		// Token: 0x04000170 RID: 368
		public DataItem Data;
	}
}

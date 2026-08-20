using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000062 RID: 98
	public class PaginegialleDataScraper
	{
		// Token: 0x06000201 RID: 513 RVA: 0x000254BC File Offset: 0x000236BC
		public PaginegialleDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00025528 File Offset: 0x00023728
		public void ProcessPage()
		{
			Thread.Sleep(500);
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"scheda-azienda__companyAddress\">(.*?)<br>(\\d+)(.*?)</div>");
			if (Items.Count > 0)
			{
				try
				{
					this.Data.PostalCode = Items[0][2];
					this.Data.City = Items[0][3].Split(new char[] { '(' })[0];
				}
				catch
				{
				}
			}
			this.Data.Country = "Italia";
			Items = HTTPScraper.ParseHTML(ClearPage, "\"telephone\" : \"([^\"]+)\"");
			if (Items.Count > 0)
			{
				this.Data.Phone = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"faxNumber\" : \"([^\"]+)\"");
			if (Items.Count > 0)
			{
				for (int i = 0; i < Items.Count; i++)
				{
					if (i == 0)
					{
						DataItem data = this.Data;
						data.Fax = data.Fax + " Fax:" + Items[i][1];
					}
					else if (this.Data.Fax.IndexOf(Items[i][1]) == -1)
					{
						DataItem data2 = this.Data;
						data2.Fax = data2.Fax + ", " + Items[i][1].Replace(" ", "");
					}
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "(?i)data-pag=\"www\"(.*?)href=(.*?)target=\"_blank\"");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][2].Split(new char[] { ' ' })[0].Replace("\"", "");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "email\":\"(.*?)\",");
			if (Items.Count > 0 && Items[0][1].IndexOf('@') > -1)
			{
				this.Data.Email = Items[0][1];
			}
			else if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta" });
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x040001B9 RID: 441
		private Thread MainThread;

		// Token: 0x040001BA RID: 442
		private string PageUrl;

		// Token: 0x040001BB RID: 443
		private Settings AppSettings;

		// Token: 0x040001BC RID: 444
		private List<ProxyServer> Proxies;

		// Token: 0x040001BD RID: 445
		public bool IsDone;

		// Token: 0x040001BE RID: 446
		public DataItem Data;
	}
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000059 RID: 89
	public class KompassDataScraper
	{
		// Token: 0x060001EF RID: 495 RVA: 0x000242BC File Offset: 0x000224BC
		public KompassDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00024328 File Offset: 0x00022528
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "\"biz_city_state\":(.*?),(.*?),(.*?),");
			if (Items.Count > 0)
			{
				this.Data.City = Items[0][2].Replace("\"", "").Replace("]", "").Replace(" ", "");
				this.Data.State = Items[0][3].Replace("\"", "").Replace("]", "").Replace(" ", "");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"addressCountry\":(.*?)}");
			if (Items.Count > 0)
			{
				this.Data.Country = Items[0][1].Replace("\"", "").Replace("]", "").Replace(" ", "")
					.Replace("}", "")
					.Split(new char[] { '<' })[0];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"streetAddress\":(.*?),");
			if (Items.Count > 0)
			{
				this.Data.Address = Items[0][1].Replace("\"", "").Replace("]", "").Replace("[", "")
					.Replace("\n", " ");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"postalCode\":(.*?),");
			if (Items.Count > 0)
			{
				this.Data.PostalCode = Items[0][1].Replace("\"", "").Replace("}", "").Replace("{", "");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<a href=\"/biz_redir(.*?)&");
			if (Items.Count > 0)
			{
				this.Data.Website = Items[0][1].Replace("url=", "").Replace("%3A%2F%2F", "://").Replace("&amp", "")
					.Replace("%2F", "/")
					.Replace("\"", "")
					.Replace("{href:", "")
					.Replace(" ", "")
					.Replace("?", "");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "BusinessPhoneNumber&quot;,&quot;formatted&quot;:&quot;(.*?)&quot;");
			if (Items.Count > 0)
			{
				this.Data.Phone = Items[0][1].Replace("&quot;", "").Replace(",", "");
			}
			if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "konta", "conta" });
			}
			this.IsDone = true;
		}

		// Token: 0x04000183 RID: 387
		private Thread MainThread;

		// Token: 0x04000184 RID: 388
		private string PageUrl;

		// Token: 0x04000185 RID: 389
		private Settings AppSettings;

		// Token: 0x04000186 RID: 390
		private List<ProxyServer> Proxies;

		// Token: 0x04000187 RID: 391
		public bool IsDone;

		// Token: 0x04000188 RID: 392
		public DataItem Data;
	}
}

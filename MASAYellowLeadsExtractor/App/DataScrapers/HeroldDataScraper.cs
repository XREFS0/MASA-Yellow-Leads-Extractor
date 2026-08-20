using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200005B RID: 91
	internal class HeroldDataScraper
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x00024778 File Offset: 0x00022978
		public HeroldDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000247E4 File Offset: 0x000229E4
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			new List<string[]>();
			List<string[]> Website = HTTPScraper.ParseHTML(ClearPage, "\"URL\",\"(.*?)\",{");
			if (Website.Count > 0)
			{
				this.Data.Website = Website[0][1];
			}
			List<string[]> Via = HTTPScraper.ParseHTML(ClearPage, "ADR;WORK;CHARSET=utf-8:;;(.*?);");
			if (Via.Count > 0)
			{
				this.Data.Address = Via[0][1];
			}
			List<string[]> State = HTTPScraper.ParseHTML(ClearPage, "<meta itemprop=\"addressRegion\" content=\"(.*?)\"");
			if (State.Count > 0)
			{
				this.Data.State = State[0][1];
			}
			if (Website.Count > 0)
			{
				this.Data.Website = Website[0][1].Replace("://", "http://").Replace("s://", "https://").Replace("shttp", "http");
			}
			List<string[]> Email = HTTPScraper.ParseHTML(ClearPage, "href=\"mailto:(.*?)\"");
			if (Email.Count > 0)
			{
				this.Data.Email = Email[0][1].Split(new char[] { '"' })[0];
			}
			List<string[]> Tel = HTTPScraper.ParseHTML(ClearPage, "href=\"tel:(.*?)\"");
			if (Tel.Count > 0)
			{
				this.Data.Phone = Tel[0][1].Replace("\\", "");
			}
			if (this.Data.Email == "" && this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "")
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "kontakt" });
			}
			this.IsDone = true;
		}

		// Token: 0x0400018F RID: 399
		private Thread MainThread;

		// Token: 0x04000190 RID: 400
		private string PageUrl;

		// Token: 0x04000191 RID: 401
		private Settings AppSettings;

		// Token: 0x04000192 RID: 402
		private List<ProxyServer> Proxies;

		// Token: 0x04000193 RID: 403
		public bool IsDone;

		// Token: 0x04000194 RID: 404
		public DataItem Data;
	}
}

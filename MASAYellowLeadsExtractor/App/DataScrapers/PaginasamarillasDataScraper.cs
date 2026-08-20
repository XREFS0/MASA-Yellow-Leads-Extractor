using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200005D RID: 93
	public class PaginasamarillasDataScraper
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x00024BA8 File Offset: 0x00022DA8
		public PaginasamarillasDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00024C14 File Offset: 0x00022E14
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> SubItems = HTTPScraper.ParseHTML(ClearPage, "<a href=\"(.*?)\" class=\"sitio-web\"");
			if (SubItems.Count > 0)
			{
				this.Data.Website = SubItems[0][1].Split(new char[] { '?' })[0];
			}
			SubItems = HTTPScraper.ParseHTML(ClearPage, "\"customerMail\":\"(.*?)\",");
			if (SubItems.Count > 0)
			{
				this.Data.Email = SubItems[0][1];
			}
			else if (Program.AppSettings.ExtractEmails)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "conta" });
			}
			this.Data.MapLink = this.Data.DetailsLink + "?gm=comoIr";
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x0400019B RID: 411
		private Thread MainThread;

		// Token: 0x0400019C RID: 412
		private string PageUrl;

		// Token: 0x0400019D RID: 413
		private Settings AppSettings;

		// Token: 0x0400019E RID: 414
		private List<ProxyServer> Proxies;

		// Token: 0x0400019F RID: 415
		public bool IsDone;

		// Token: 0x040001A0 RID: 416
		public DataItem Data;
	}
}

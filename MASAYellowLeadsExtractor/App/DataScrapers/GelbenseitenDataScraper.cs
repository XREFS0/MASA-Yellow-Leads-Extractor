using System;
using System.Collections.Generic;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x02000043 RID: 67
	public class GelbenseitenDataScraper
	{
		// Token: 0x060001C3 RID: 451 RVA: 0x00021B0C File Offset: 0x0001FD0C
		public GelbenseitenDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00021B78 File Offset: 0x0001FD78
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"mod-TeilnehmerKopf__adresse-daten\">(.*?)</span>");
			if (Items.Count == 2)
			{
				this.Data.Address = HTTPScraper.ClearTags(Items[0][1]);
				this.Data.PostalCode = HTTPScraper.ClearTags(Items[1][1]);
			}
			else if (Items.Count == 1)
			{
				this.Data.Address = HTTPScraper.ClearTags(Items[0][1]);
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"mod-TeilnehmerKopf__adresse-daten--noborder\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.City = HTTPScraper.ClearTags(Items[0][1]);
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span data-selenium=\"teilnehmerkopf__branche\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.Category = HTTPScraper.ClearTags(Items[0][1]);
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"mod-TeilnehmerKopf--secret_suffix\">(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.Phone = HTTPScraper.ClearTags(Items[0][1]);
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"mod-Kontaktdaten__list-item contains-icon-big-homepage\"><a href=\"(.*?)\" data-wipe-realview=\"detailseite_webadresse\"");
			if (Items.Count > 0 && (this.Data.Website == "" || this.Data.Website == null))
			{
				this.Data.Website = HTTPScraper.ClearTags(Items[0][1]).Trim();
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "data-link=\"mailto:(.*?)?subject=(.*?)\">");
			if (Items.Count > 0 && (this.Data.Email == null || this.Data.Email == ""))
			{
				this.Data.Email = HTTPScraper.ClearTags(Items[0][1]).Replace("?", "");
			}
			if (this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "" && (this.Data.Email == null || this.Data.Email == "E-Mail schreiben"))
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "kontakt", "contact" });
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"mod-Kontaktdaten__list-item contains-icon-big-fax\"(.*?)><span>(.*?)</span>");
			if (Items.Count > 0)
			{
				this.Data.Fax = Items[0][2];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"box-map\" data-karte-aktiviert-url=\"(.*?)\" data-karte-deaktiviert-url=\"(.*?)\">");
			if (Items.Count > 0)
			{
				this.Data.MapLink = Items[0][1];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x040000FF RID: 255
		private Thread MainThread;

		// Token: 0x04000100 RID: 256
		private string PageUrl;

		// Token: 0x04000101 RID: 257
		private Settings AppSettings;

		// Token: 0x04000102 RID: 258
		private List<ProxyServer> Proxies;

		// Token: 0x04000103 RID: 259
		public bool IsDone;

		// Token: 0x04000104 RID: 260
		public DataItem Data;
	}
}

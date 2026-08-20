using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200003D RID: 61
	public class YellowPagesPlLinksScraper
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00020AFC File Offset: 0x0001ECFC
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "cc-content", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "h2", "card__title mdc-typography--headline6", true);
					if (Title.Count > 0)
					{
						WebScraper.GetChildren(WB, Title[0]);
						DataItem.BusinessName = Title[0].innerText;
						List<string[]> AddItems = HTTPScraper.ParseHTML(Title[0].innerHTML, "<a href=\"(.*?)\" title=");
						if (AddItems.Count > 0)
						{
							DataItem.DetailsLink = AddItems[0][1];
						}
					}
					DataItem.Country = "Polska";
					if (DataItem.DetailsLink != null && DataItem.DetailsLink != "")
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
							DataItem.Email, DataItem.MapLink, DataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
					}
if (Program.IsStopped())
					{
						return;
					}
					Application.DoEvents();
				}
				string OldUrl = WB.Url.ToString();
				if (Program.AppSettings.ExtractEmails)
				{
					YellowPagesPlLinksScraper.GetData1(ref PageItems, WB, mainForm);
				}
				WB.LoadUrlAndWait(OldUrl);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "div", "results-navigation-list", true);
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					List<string[]> AddressParts2 = HTTPScraper.ParseHTML(NextButton.innerHTML, "href=\"(.*?)\" rel=(.*?)Dalej");
					if (AddressParts2.Count > 0)
					{
						string NewUrl = AddressParts2[0][1].Replace("&amp;", "&");
						Thread.Sleep(500);
						WB.LoadUrlAndWait("https://www.yellowpages.pl" + NewUrl);
						Thread.Sleep(500);
					}
					else
					{
						NextButton = null;
					}
				}
				if (NextButton == null)
				{
					goto Block_7;
				}
			}

			Block_7:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00020E20 File Offset: 0x0001F020
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			string HTML = "";
			for (int i = 0; i < DataItems.Count; i++)
			{
				Program.RequestDelay();
				try
				{
					Thread.Sleep(1000);
					WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				}
				catch
				{
				}
				HTML = WB.GetHtml();
				List<string[]> Website = HTTPScraper.ParseHTML(HTML, "<a class=\"btn-link\" href=\"(.*?)\" target=\"_blank\" rel=\"nofollow\">(.*?)</a>");
				if (Website.Count > 0)
				{
					try
					{
						DataItems[i].Website = Website[0][2];
						YellowPagesPlLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
					catch
					{
					}
				}
				if (Program.AppSettings.ExtractEmails)
				{
					DataItems[i].Email = EmailMiner.GetEmail(DataItems[i].Website, new string[] { "conta", "konta" });
					YellowPagesPlLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Phone = HTTPScraper.ParseHTML(HTML, "<span class=\"phone-header\" data-phone-number=\"(.*?)\">");
				if (Phone.Count > 0)
				{
					try
					{
						DataItems[i].Phone = Phone[0][1];
						YellowPagesPlLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
					catch
					{
					}
				}
				List<string[]> Addr = HTTPScraper.ParseHTML(HTML, "<span itemprop=\"streetAddress\">(.*?)</span>");
				if (Addr.Count > 0)
				{
					try
					{
						DataItems[i].Address = Addr[0][1];
						YellowPagesPlLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
					catch
					{
					}
				}
				List<string[]> Addr2 = HTTPScraper.ParseHTML(HTML, "<span itemprop=\"postalCode\">(.*?)</span>");
				if (Addr2.Count > 0)
				{
					try
					{
						DataItems[i].PostalCode = Addr2[0][1];
						YellowPagesPlLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
					catch
					{
					}
				}
				List<string[]> Addr3 = HTTPScraper.ParseHTML(HTML, "<span itemprop=\"addressLocality\">(.*?)</span>");
				if (Addr3.Count > 0)
				{
					try
					{
						DataItems[i].City = Addr3[0][1];
						YellowPagesPlLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
					catch
					{
					}
				}
				List<string[]> Addr4 = HTTPScraper.ParseHTML(HTML, "<span itemprop=\"addressCountry\">(.*?)</span>");
				if (Addr4.Count > 0)
				{
					try
					{
						DataItems[i].Country = Addr4[0][1];
						YellowPagesPlLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
					catch
					{
					}
				}
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000210CC File Offset: 0x0001F2CC
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			POLDataScraper[] Pool = new POLDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new POLDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
				ScraperIndex++;
				i++;
			}
			ScraperIndex--;
			int EmptyPoolPositions = 0;
			while (EmptyPoolPositions < PoolSize)
			{
				EmptyPoolPositions = 0;
				for (int j = 0; j < PoolSize; j++)
				{
					if (Pool[j] != null && Pool[j].IsDone)
					{
						Completed++;
						YellowPagesPlLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new POLDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
						}
						else
						{
							Pool[j] = null;
						}
					}
					if (Pool[j] == null)
					{
						EmptyPoolPositions++;
					}
				}
				Thread.Sleep(500);
				Application.DoEvents();
				if (Program.IsStopped())
				{
					break;
				}
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000211E0 File Offset: 0x0001F3E0
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = YellowPagesPlLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[2].Value = YellowPagesPlLinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = YellowPagesPlLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = YellowPagesPlLinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = YellowPagesPlLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = YellowPagesPlLinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = YellowPagesPlLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[9].Value = YellowPagesPlLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = YellowPagesPlLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00002A43 File Offset: 0x00000C43
		private static string ClearValue(string Value)
		{
			if (Value != null)
			{
				return Value.Replace("&#x27", "");
			}
			return "";
		}
	}
}

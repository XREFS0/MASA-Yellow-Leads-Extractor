using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200002B RID: 43
	public class GoldenPagesLinksScraper
	{
		// Token: 0x06000146 RID: 326 RVA: 0x00016B94 File Offset: 0x00014D94
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			List<DataItem> PageItems = new List<DataItem>();
			GoldenPagesLinksScraper.WaitForBrowser(WB);
			int Iter = 0;
			List<Element> HTMLItems = new List<Element>();
			for (;;)
			{
				GoldenPagesLinksScraper.ScrollItDown(WB, 1);
				Application.DoEvents();
				Thread.Sleep(500);
				Iter++;
				mainForm.tssLabelListed.Text = string.Format("{0}% page extraction...", Iter * 30);
if (Program.IsStopped())
				{
					break;
				}
				Application.DoEvents();
				if (Iter > 2)
				{
					goto IL_006E;
				}
			}
			return;
			IL_006E:
			HTMLItems = WebScraper.GetElements(WB, "div", "result__link", true);
			foreach (Element HItem in HTMLItems)
			{
				DataItem DataItem = new DataItem();
				List<Element> Title = WebScraper.GetElements(WB, HItem, "h1", "result__title", true);
				if (Title.Count > 0)
				{
					DataItem.BusinessName = Title[0].innerText;
					DataItem.DetailsLink = "https://www.1307.be/";
				}
				List<Element> Address = WebScraper.GetElements(WB, HItem, "address", "company__address", true);
				if (Address.Count > 0 && Address[0].innerText != null)
				{
					DataItem.Address = Address[0].innerHTML.Replace("<br>", "- ");
					List<string[]> AddItems = HTTPScraper.ParseHTML(Address[0].innerHTML, "(.*?)<br>(\\d+) (.*?)");
					if (AddItems.Count > 0)
					{
						DataItem.PostalCode = AddItems[0][2];
						DataItem.City = AddItems[0][3];
						DataItem.State = "Belgium";
					}
				}
				else
				{
					DataItem.Address = "";
				}
				DataItem.Country = "Belgium";
				List<Element> Categories = WebScraper.GetElements(WB, HItem, "p", "category", true);
				if (Categories.Count > 0 && Categories[0].innerText != null)
				{
					List<string[]> AddItems2 = HTTPScraper.ParseHTML(Categories[0].innerText, "(.*?),(.*?)");
					if (AddItems2.Count > 0)
					{
						DataItem.Category = AddItems2[0][1];
					}
					else
					{
						DataItem.Category = Categories[0].innerText.Trim();
					}
				}
				List<Element> Phone = WebScraper.GetElements(WB, HItem, "span", "linksSN--number mobile--visible", true);
				if (Phone.Count > 0)
				{
					DataItem.Phone = Phone[0].innerText;
				}
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
			mainForm.tsProgress.Value = 0;

			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00016F94 File Offset: 0x00015194
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			GoldenPagesDataScraper[] Pool = new GoldenPagesDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new GoldenPagesDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						GoldenPagesLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new GoldenPagesDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				Thread.Sleep(1000);
				Application.DoEvents();
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000170A0 File Offset: 0x000152A0
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = GoldenPagesLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[7].Value = GoldenPagesLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = GoldenPagesLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = GoldenPagesLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = GoldenPagesLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = GoldenPagesLinksScraper.ClearValue(Item.MapLink);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000F83F File Offset: 0x0000DA3F
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1000)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0001722C File Offset: 0x0001542C
		private static void ScrollItDown(WebView WB, int Iterations)
		{
			WB.EvalScript("window.scrollTo(0,10000)");
			Application.DoEvents();
			Thread.Sleep(500);
			WB.EvalScript("window.scrollTo(0,20000)");
			Application.DoEvents();
			Thread.Sleep(500);
			WB.EvalScript("window.scrollTo(0,30000)");
			Application.DoEvents();
			Thread.Sleep(500);
			WB.EvalScript("window.scrollTo(0,40000)");
			Application.DoEvents();
			Thread.Sleep(500);
			WB.EvalScript("window.scrollTo(0,50000)");
			Application.DoEvents();
			Thread.Sleep(500);
			WB.EvalScript("window.scrollTo(0,60000)");
			Application.DoEvents();
			Thread.Sleep(500);
			WB.EvalScript("window.scrollTo(0,70000)");
			Application.DoEvents();
			Thread.Sleep(500);
			WB.EvalScript("window.scrollTo(0,80000)");
			Application.DoEvents();
			Thread.Sleep(500);
			WB.EvalScript("window.scrollTo(0,90000)");
			Application.DoEvents();
			Thread.Sleep(500);
			WB.EvalScript("window.scrollTo(0,100000)");
			Application.DoEvents();
			Thread.Sleep(500);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00002A43 File Offset: 0x00000C43
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

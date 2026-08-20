using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200002F RID: 47
	public class YellowPagesAlbaniaLinksScraper
	{
		// Token: 0x06000158 RID: 344 RVA: 0x000189DC File Offset: 0x00016BDC
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", " box-company-search overout", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a");
					if (Title.Count > 0)
					{
						try
						{
							DataItem.DetailsLink = Title[0]["href"].ToString();
						}
						catch
						{
							DataItem.DetailsLink = "";
						}
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "ico-box-company-search", true);
					if (Address.Count > 0 && Address[0].innerText.IndexOf('-') > -1)
					{
						DataItem dataItem = DataItem;
						dataItem.Address += Address[0].innerText;
						DataItem.Country = "Albania";
						DataItem.State = "Albania";
					}
					List<Element> Title2 = WebScraper.GetElements(WB, HItem, "div", "descr-box-company-search", true);
					if (Title2.Count > 0)
					{
						DataItem.BusinessName = Title2[0].innerText;
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "div", "category-box-compan-searchy", true);
					if (Categories.Count > 0)
					{
						DataItem.Category = Categories[0].innerText;
					}
					List<Element> Fax = WebScraper.GetElements(WB, HItem, "span", "fax", true);
					if (Fax.Count > 0)
					{
						DataItem.Fax = Fax[0].innerText;
					}
					List<Element> Email = WebScraper.GetElements(WB, HItem, "div", "ico-box-company-search ml-20", true);
					if (Email.Count > 0)
					{
						DataItem.Email = Email[0].innerText;
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
				YellowPagesAlbaniaLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
Element NextButton = null;
				List<Element> NextButtons = WebScraper.GetElements(WB, "ul", "pagination m-t-0 m-b-10", true);
				if (NextButtons.Count > 0)
				{
					for (int RowIndex = 0; RowIndex < NextButtons.Count; RowIndex++)
					{
						string innerHTML = NextButtons[RowIndex].innerHTML;
						NextButton = NextButtons[RowIndex];
						List<string[]> AddressParts2 = HTTPScraper.ParseHTML(innerHTML, "<li class=\" active\">(.*?)</li> <li class=\"\"> <a href=\"(.*?)\">(.*?)</a>");
						if (AddressParts2.Count > 0 && AddressParts2[0][3].IndexOf("»") == -1)
						{
							WB.Url.ToString();
							string NewUrl = AddressParts2[0][2].Replace("amp;", "");
							WB.LoadUrlAndWait(NewUrl);
							Thread.Sleep(500);
							RowIndex = NextButtons.Count - 1;
						}
					}
				}
				if (NextButton == null)
				{
					goto Block_6;
				}
			}

			Block_6:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00018E34 File Offset: 0x00017034
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			YellowPagesAlbaniaDataScraper[] Pool = new YellowPagesAlbaniaDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YellowPagesAlbaniaDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YellowPagesAlbaniaLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new YellowPagesAlbaniaDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x0600015A RID: 346 RVA: 0x00018F40 File Offset: 0x00017140
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[7].Value = YellowPagesAlbaniaLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[9].Value = YellowPagesAlbaniaLinksScraper.ClearValue(Item.Website);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00002A43 File Offset: 0x00000C43
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

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200002A RID: 42
	public class YPRULinksScraper
	{
		// Token: 0x06000141 RID: 321 RVA: 0x000165BC File Offset: 0x000147BC
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				while (!WB.IsReady)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "search-listing", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "yp-click", true);
					if (Title.Count > 0)
					{
						try
						{
							DataItem.BusinessName = Title[0].innerText.Trim();
							DataItem.DetailsLink = Title[0]["href"].ToString();
						}
						catch
						{
						}
					}
					DataItem.Country = "Philippines";
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "btn btn-yp-default mr-2 biz-btn-call yp-click", true);
					if (Phone.Count > 0)
					{
						DataItem.Phone = Phone[0]["href"].ToString();
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "span", "search-capsule-rounded search-badge", true);
					if (Categories.Count > 0)
					{
						DataItem.Category = Categories[0].innerText;
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "search-busines-address", true);
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerText;
						Match match = new Regex("[^,]+$").Match(DataItem.Address);
						if (match.Success)
						{
							DataItem.City = match.Value.Trim();
						}
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
				YPRULinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "page-link", true);
				if (NextButtons.Count > 0)
				{
					for (int RowIndex = 0; RowIndex < NextButtons.Count; RowIndex++)
					{
						if (NextButtons[RowIndex].outerHTML.IndexOf("Next") > -1)
						{
							NextButton = NextButtons[RowIndex];
							WB.Url.ToString();
							string NewUrl = NextButton["href"].ToString();
							WB.LoadUrlAndWait(NewUrl);
							Thread.Sleep(1000);
							Application.DoEvents();
						}
					}
				}
				else
				{
					NextButton = null;
				}
				if (NextButton == null)
				{
					goto Block_8;
				}
			}

			Block_8:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000169B0 File Offset: 0x00014BB0
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			YPRUDataScraper[] Pool = new YPRUDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YPRUDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YPRULinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new YPRUDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000143 RID: 323 RVA: 0x00016ABC File Offset: 0x00014CBC
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[9].Value = YPRULinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = YPRULinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00002A43 File Offset: 0x00000C43
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

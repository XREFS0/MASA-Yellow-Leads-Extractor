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
	// Token: 0x02000017 RID: 23
	public static class COZALinksScraper
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "serviceResultCard_service_result_card__", false))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElementsByTag(WB, HItem, "h6");
					if (Title.Count > 0)
					{
						try
						{
							DataItem.BusinessName = Title[0].innerText.Trim();
						}
						catch
						{
						}
					}
					List<Element> Title2 = WebScraper.GetElementsByTag(WB, HItem, "a");
					if (Title2.Count > 0)
					{
						try
						{
							DataItem.DetailsLink = Title2[0]["href"].ToString();
						}
						catch
						{
						}
					}
					DataItem.Country = "South Africa";
					List<Element> Address = WebScraper.GetElements(WB, HItem, "p", "text-2-liner location_location_name__", false);
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerText;
						string innerText = Address[0].innerText;
						string cityPattern = ",\\s*([^0-9,]+)\\s+\\d{4,}";
						string postalCodePattern = "(\\d{4,})";
						Match cityMatch = Regex.Match(innerText, cityPattern);
						Match postalCodeMatch = Regex.Match(innerText, postalCodePattern);
						DataItem.City = (cityMatch.Success ? cityMatch.Groups[1].Value.Trim() : "");
						DataItem.PostalCode = (postalCodeMatch.Success ? postalCodeMatch.Groups[1].Value : "");
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
				COZALinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "button", "MuiButtonBase-root MuiPaginationItem-root MuiPaginationItem-page", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[NextButtons.Count - 1];
					WebScraper.InvokeMember(WB, NextButton, "click");
					Thread.Sleep(10000);
					for (int i = 0; i < 1000; i++)
					{
						Application.DoEvents();
					}
				}
				else
				{
					NextButton = null;
				}
				if (NextButton == null)
				{
					goto Block_5;
				}
			}

			Block_5:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000CF00 File Offset: 0x0000B100
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			COZADataScraper[] Pool = new COZADataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new COZADataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						COZALinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new COZADataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000D4 RID: 212 RVA: 0x0000D00C File Offset: 0x0000B20C
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = COZALinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[7].Value = COZALinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[9].Value = COZALinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = COZALinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00002A43 File Offset: 0x00000C43
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

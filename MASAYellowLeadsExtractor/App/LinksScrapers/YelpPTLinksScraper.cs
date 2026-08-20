using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000024 RID: 36
	public class YelpPTLinksScraper
	{
		// Token: 0x0600011A RID: 282 RVA: 0x00013988 File Offset: 0x00011B88
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				while (!WB.IsReady)
				{
					Application.DoEvents();
					Thread.Sleep(1000);
				}
				string[] array = new string[] { "result-list-entry result-list-entry--clickable " };
				for (int i = 0; i < 1; i++)
				{
					foreach (Element HItem in WebScraper.GetElements(WB, "li", array[i], false))
					{
						DataItem DataItem = new DataItem();
						List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "result-list-entry-title entry-detail-link", true);
						if (Title.Count > 0)
						{
							DataItem.DetailsLink = Title[0]["href"].ToString();
							DataItem.BusinessName = Title[0].innerText;
						}
						else
						{
							List<Element> Title2 = WebScraper.GetElements(WB, HItem, "h2", "result-list-entry-title__headline", false);
							if (Title2.Count > 0)
							{
								DataItem.DetailsLink = "https://www.11880.com/";
								DataItem.BusinessName = Title2[0].innerText;
							}
						}
						List<Element> Category = WebScraper.GetElements(WB, HItem, "span", "trades-list", true);
						if (Category.Count > 0)
						{
							DataItem.Category = Category[0].innerText;
						}
						List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "d-block", true);
						if (Address.Count > 0)
						{
							DataItem.Address = Address[0].innerText;
						}
						List<Element> Postal = WebScraper.GetElements(WB, HItem, "span", "js-postal-code", true);
						if (Postal.Count > 0)
						{
							DataItem.PostalCode = Postal[0].innerText;
						}
						List<Element> City = WebScraper.GetElements(WB, HItem, "span", "js-address-locality", true);
						if (City.Count > 0)
						{
							DataItem.City = City[0].innerText;
						}
						DataItem.Country = "Germany";
						DataItem.State = "Germany";
						List<Element> Items = WebScraper.GetElements(WB, HItem, "span", "result-list-entry-phone-number__label", true);
						if (Items.Count > 0 && Items[0].innerText != null)
						{
							DataItem.Phone = Items[0].innerText;
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
				}
				YelpPTLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "icon-right", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					WebScraper.InvokeMember(WB, NextButtons[0], "click");
					Thread.Sleep(2000);
				}
				else
				{
					NextButton = null;
				}
				if (Program.IsStopped() || NextButton == null)
				{
					goto IL_03F6;
				}
			}

			IL_03F6:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00013DC4 File Offset: 0x00011FC4
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			YelpPTDataScraper[] Pool = new YelpPTDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YelpPTDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YelpPTLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new YelpPTDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00013ED8 File Offset: 0x000120D8
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[4].Value = HTTPScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[8].Value = HTTPScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = HTTPScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = HTTPScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00002A43 File Offset: 0x00000C43
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

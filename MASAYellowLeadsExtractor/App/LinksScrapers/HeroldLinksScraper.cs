using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000030 RID: 48
	public class HeroldLinksScraper
	{
		// Token: 0x0600015D RID: 349 RVA: 0x00019018 File Offset: 0x00017218
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "article", "transition-shadow", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "text-base", false);
					if (Title.Count > 0)
					{
						DataItem.BusinessName = Title[0].innerText;
						DataItem.DetailsLink = Title[0]["href"].ToString();
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "mt-[5px] text-sm", false);
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerText;
						List<string[]> AddItems = HTTPScraper.ParseHTML(DataItem.Address, "(\\d{4}) (.*)");
						if (AddItems.Count > 0)
						{
							DataItem.PostalCode = AddItems[0][1];
							DataItem.City = AddItems[0][2];
						}
						DataItem.Country = "Austria";
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "btn btn-primary btn-sm sr-only", true);
					if (Phone.Count > 0)
					{
						DataItem.Phone = Phone[0]["href"].ToString();
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "h3", "business-content_industry__rwLeS", true);
					if (Categories.Count > 0)
					{
						DataItem.Category = Categories[0].innerText;
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
				HeroldLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "flex aspect-square min-w-[2rem] items-center justify-center rounded-full transition bg-", false);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[NextButtons.Count - 1];
					string OldUrl = WB.Url.ToString();
					string NewUrl = NextButton["href"].ToString();
					if (NewUrl != OldUrl)
					{
						WB.LoadUrlAndWait(NewUrl);
						Thread.Sleep(1000);
						Application.DoEvents();
					}
					else
					{
						NextButton = null;
					}
				}
				else
				{
					NextButton = null;
				}
				if (Program.IsStopped() || NextButton == null)
				{
					goto IL_036F;
				}
			}

			IL_036F:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000193CC File Offset: 0x000175CC
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			HeroldDataScraper[] Pool = new HeroldDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new HeroldDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						HeroldLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new HeroldDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
					break;
				}
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000194E0 File Offset: 0x000176E0
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = HeroldLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[2].Value = HeroldLinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = HeroldLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = HeroldLinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = HeroldLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[7].Value = HeroldLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = HeroldLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = HeroldLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = HeroldLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = HeroldLinksScraper.ClearValue(Item.MapLink);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00002A43 File Offset: 0x00000C43
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

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
	// Token: 0x02000027 RID: 39
	public class YelpITLinksScraper
	{
		// Token: 0x0600012D RID: 301 RVA: 0x00015044 File Offset: 0x00013244
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			int PageNbr = 1;
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "cboxinnerL", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "serpFtitleT", true);
					if (Title.Count > 0)
					{
						DataItem.BusinessName = Title[0].innerText;
						DataItem.DetailsLink = Title[0]["href"].ToString();
					}
					DataItem.Country = "Italy";
					if (DataItem.DetailsLink != null && DataItem.DetailsLink != "")
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, "", DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
							"", DataItem.MapLink, DataItem.DetailsLink
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
				YelpITLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "div", "cboxinnerL", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					PageNbr++;
					NextButton = NextButtons[0];
					string newURL = Regex.Replace(WB.Url.ToString(), "-[0-9]+.html", string.Format("-{0}.html", PageNbr));
					WB.LoadUrlAndWait(newURL);
					WB.Url.ToString();
					Thread.Sleep(1000);
					for (int i = 0; i < 20; i++)
					{
						Thread.Sleep(100);
						Application.DoEvents();
					}
				}
				else
				{
					NextButton = null;
				}
				Thread.Sleep(500);
				if (Program.IsStopped() || (NextButton == null && WB.Url.ToString().IndexOf("-1.html") <= -1))
				{
					goto IL_02CB;
				}
			}

			IL_02CB:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00015354 File Offset: 0x00013554
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			YelpITDataScraper[] Pool = new YelpITDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YelpITDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YelpITLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new YelpITDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x0600012F RID: 303 RVA: 0x00015468 File Offset: 0x00013668
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = HTTPScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[2].Value = HTTPScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = HTTPScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = HTTPScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = HTTPScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = HTTPScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = HTTPScraper.ClearValue(Item.Phone);
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

		// Token: 0x06000130 RID: 304 RVA: 0x00002A43 File Offset: 0x00000C43
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

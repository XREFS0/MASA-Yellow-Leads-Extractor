using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200001A RID: 26
	public class GoldenLinksScraper
	{
		// Token: 0x060000DF RID: 223 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				while (!WB.IsReady)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "listing_content", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "listing_title_link", true);
					if (Title.Count > 0)
					{
						DataItem.BusinessName = Title[0].innerText.Split(new char[] { '.' }).Last<string>();
						DataItem.DetailsLink = Title[0]["href"].ToString();
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "listing_address", true);
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerText;
						DataItem.PostalCode = string.Join(" ", Address[0].innerText.Split(new char[] { ' ' }).Reverse<string>().Take<string>(2)
							.Reverse<string>());
					}
					DataItem.Country = "Ireland";
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "div", "listing_categories clearfix", true);
					if (Categories.Count > 0)
					{
						DataItem.Category = Categories[0].innerText;
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "link_listing_number", true);
					if (Phone.Count > 0)
					{
						DataItem.Phone = Phone[0].innerHTML;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "ul", "list_inline pull_left", true);
					if (Website.Count > 0)
					{
						List<string[]> Parts = HTTPScraper.ParseHTML(Website[0].innerHTML, "<a href=\"(.*?)\" target=\"_blank\"(.*?)>Website</a>");
						if (Parts.Count > 0)
						{
							DataItem.Website = Parts[0][1].Split(new char[] { '?' })[0];
						}
					}
					if (DataItem.Website != "")
					{
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "conta" });
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
List<Element> NextButtons = WebScraper.GetElements(WB, "button", "btn_normal btn_pagination clickable", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					WebScraper.InvokeMember(WB, NextButton, "click");
					Thread.Sleep(5000);
				}
				else
				{
					NextButton = null;
				}
				if (Program.IsStopped() || NextButton == null)
				{
					goto IL_03EA;
				}
			}

			IL_03EA:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000EAF8 File Offset: 0x0000CCF8
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			BrasilDataScraper[] Pool = new BrasilDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new BrasilDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						GoldenLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new BrasilDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000E1 RID: 225 RVA: 0x0000EC04 File Offset: 0x0000CE04
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
					mainForm.dgvResults.Rows[i].Cells[11].Value = HTTPScraper.ClearValue(Item.MapLink);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}
	}
}

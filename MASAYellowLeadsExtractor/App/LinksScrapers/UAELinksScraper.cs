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
	// Token: 0x0200001F RID: 31
	public static class UAELinksScraper
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00010EA4 File Offset: 0x0000F0A4
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				while (!WB.IsReady)
				{
					Application.DoEvents();
					Thread.Sleep(200);
				}
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "flex flex-grow flex-col w-full ", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "div", "text-xl font-bold text-", false);
					if (Title.Count > 0)
					{
						try
						{
							List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
							DataItem.BusinessName = Children[0].innerText.Replace("More Info", "");
							DataItem.DetailsLink = Children[0]["href"].ToString();
						}
						catch
						{
						}
					}
					DataItem.City = "...loading...";
					DataItem.Country = "UAE";
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "hidden whitespace-nowrap", true);
					if (Phone.Count > 1)
					{
						DataItem.Phone = " " + Phone[0]["href"].ToString() + ", Mobile " + Phone[1]["href"].ToString();
					}
					else if (Phone.Count > 0)
					{
						DataItem.Phone = " " + Phone[0]["href"].ToString();
					}
					List<Element> Category = WebScraper.GetElements(WB, HItem, "a", "font-bold text-", false);
					if (Category.Count > 0)
					{
						try
						{
							DataItem.Category = Category[0].innerText.Split(new char[] { ',' })[0].Replace("Products & Services :", "");
						}
						catch
						{
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
				UAELinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "button", "border-[1px] border-gray-400 rounded-lg px-2 h-[30px]", true);
				Match match = Regex.Match(NextButtons[1].outerHTML, "value\\s*=\\s*\"(.*?)\"", RegexOptions.IgnoreCase);
				Element NextButton;
				if (NextButtons[1].innerText.Contains("Next") && match.Groups[1].Value != "false")
				{
					NextButton = NextButtons[1];
					WebScraper.InvokeMember(WB, NextButton, "click");
					Thread.Sleep(2000);
				}
				else
				{
					NextButton = null;
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

		// Token: 0x060000FC RID: 252 RVA: 0x000112E8 File Offset: 0x0000F4E8
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			UAEDataScraper[] Pool = new UAEDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new UAEDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						UAELinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new UAEDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000FD RID: 253 RVA: 0x000113FC File Offset: 0x0000F5FC
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[2].Value = UAELinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = UAELinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = UAELinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = UAELinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[9].Value = UAELinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = UAELinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00002A43 File Offset: 0x00000C43
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

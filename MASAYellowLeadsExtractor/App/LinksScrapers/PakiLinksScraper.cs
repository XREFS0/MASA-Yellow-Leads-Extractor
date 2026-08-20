using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200001E RID: 30
	public static class PakiLinksScraper
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x00010898 File Offset: 0x0000EA98
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
				Application.DoEvents();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "pg-full-width", false))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "DetailPageLink", true);
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
					List<Element> Address = WebScraper.GetElementsByTag(WB, HItem, "address");
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerText.Trim();
						List<string[]> AddItems = HTTPScraper.ParseHTML(DataItem.Address, "(.*?),(.*?),(.*?)");
						if (AddItems.Count > 0)
						{
							DataItem.City = AddItems[0][3];
						}
						DataItem.Country = "Pakistan";
						DataItem.State = "Pakistan";
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "ul", "submenu", true);
					if (Phone.Count > 0)
					{
						List<Element> Children = WebScraper.GetChildren(WB, Phone[0]);
						DataItem.Phone = Children[3].innerText;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "i", "glyphicon glyphicon-globe link-icon", true);
					if (Website.Count > 0)
					{
						try
						{
							DataItem.Website = WebScraper.GetParent(WB, Website[0])["href"].ToString();
							goto IL_01B9;
						}
						catch
						{
							goto IL_01B9;
						}
						goto IL_01AD;
					}
					goto IL_01AD;
					IL_01B9:
					if (DataItem.Website != "")
					{
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "contact" });
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
					continue;
					IL_01AD:
					DataItem.Website = "";
					goto IL_01B9;
				}
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "glyphicon glyphicon-chevron-right pg-next", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					string OldUrl = WB.Url.ToString();
					string NewUrl = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NewUrl);
					while (WB.Url.ToString() == OldUrl || !WB.IsReady)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
						Thread.Sleep(1000);
					}
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
					goto Block_8;
				}
			}

			Block_8:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00010D18 File Offset: 0x0000EF18
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			PakiDataScraper[] Pool = new PakiDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PakiDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						PakiLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new PakiDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000F9 RID: 249 RVA: 0x00010E2C File Offset: 0x0000F02C
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00002A43 File Offset: 0x00000C43
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

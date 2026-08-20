using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200001D RID: 29
	public static class PaiLinksScraper
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00010014 File Offset: 0x0000E214
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "cell small-8 medium-8 mt-10", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "card-link", true);
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
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "card-address", false);
					if (Address.Count > 0)
					{
						try
						{
							DataItem.Address = Address[0].innerHTML.Replace("<br>", " | ");
							List<string[]> AddItems = HTTPScraper.ParseHTML(Address[0].innerHTML, "(\\d{4})-(\\d{3})(?:\\s+([A-ZÀ-Ú\\s]+))?");
							if (AddItems.Count > 0)
							{
								DataItem.PostalCode = AddItems[0][1] + "-" + AddItems[0][2];
								DataItem.City = AddItems[0][3];
							}
							goto IL_013F;
						}
						catch
						{
							goto IL_013F;
						}
						goto IL_0133;
					}
					goto IL_0133;
					IL_013F:
					DataItem.Country = "Portugal";
					DataItem.State = "Portugal";
					List<Element> Cat = WebScraper.GetElements(WB, HItem, "div", "card-metadata", true);
					if (Cat.Count > 0)
					{
						DataItem.Category = Cat[0].innerText;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "i", "glyphicon glyphicon-globe link-icon", true);
					if (Website.Count > 0)
					{
						try
						{
							DataItem.Website = WebScraper.GetParent(WB, Website[0])["href"].ToString();
							goto IL_01DE;
						}
						catch
						{
							goto IL_01DE;
						}
						goto IL_01D2;
					}
					goto IL_01D2;
					IL_01DE:
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
					IL_01D2:
					DataItem.Website = "";
					goto IL_01DE;
					IL_0133:
					DataItem.Address = "";
					goto IL_013F;
				}
				string OldUrl = WB.Url.ToString();
				if (Program.AppSettings.ExtractEmails)
				{
					PaiLinksScraper.GetData1(ref PageItems, WB, mainForm);
				}
				WB.LoadUrlAndWait(OldUrl);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "li", "next", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = WebScraper.GetChildren(WB, NextButtons[0])[0];
					WB.Url.ToString();
					string NewUrl = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NewUrl);
					Thread.Sleep(1000);
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
					goto Block_6;
				}
			}

			Block_6:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000104D4 File Offset: 0x0000E6D4
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				Program.RequestDelay();
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(800);
				string HTML = WB.GetHtml();
				Thread.Sleep(100);
				List<string[]> Email = HTTPScraper.ParseHTML(HTML, "mailto:(.*?)\"");
				if (Email.Count > 0)
				{
					DataItems[i].Email = Email[0][1];
					PaiLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Website = HTTPScraper.ParseHTML(HTML, "visit-webpage\" href=\"(.*?)\">");
				if (Website.Count > 0)
				{
					DataItems[i].Website = Website[0][1];
					if (DataItems[i].Email == "" || DataItems[i].Email == null)
					{
						DataItems[i].Email = EmailMiner.GetEmail(DataItems[i].Website, new string[] { "conta" });
					}
					PaiLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Tel = HTTPScraper.ParseHTML(HTML, "href=\"tel:(.*?)\"");
				if (Tel.Count > 0)
				{
					DataItems[i].Phone = " " + Tel[0][1].Replace("\"", "").Replace("[", " ").Replace("]", "");
					PaiLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0001067C File Offset: 0x0000E87C
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			PaiDataScraper[] Pool = new PaiDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PaiDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						PaiLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new PaiDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000F5 RID: 245 RVA: 0x00010790 File Offset: 0x0000E990
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[7].Value = PaiLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[9].Value = PaiLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = PaiLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00002A43 File Offset: 0x00000C43
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

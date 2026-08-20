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
	// Token: 0x02000023 RID: 35
	public class CylexLinksScraper
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00012F3C File Offset: 0x0001113C
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "lm-comp position-relative", false))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElementsByTag(WB, HItem, "a");
					if (Title.Count > 0)
					{
						DataItem.BusinessName = Title[0].innerText;
						DataItem.DetailsLink = Title[0]["href"].ToString();
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "div", "lm-ph", true);
					if (Phone.Count > 0)
					{
						try
						{
							DataItem.Phone = Phone[0].innerText;
						}
						catch
						{
						}
					}
					List<Element> City = WebScraper.GetElements(WB, HItem, "div", "pl-3 addr", true);
					if (City.Count > 0)
					{
						string a = City[0].innerText;
						DataItem.Address = a.Trim().Replace("\n", " ");
						Match match = new Regex("(\\d{4,5})\\s*([A-Za-zÀ-ÿ\\s'/-]+)$|,\\s*([A-Za-zÀ-ÿ\\s'/-]+),\\s*(\\d{4,5})$").Match(a);
						if (match.Success)
						{
							if (!string.IsNullOrEmpty(match.Groups[1].Value))
							{
								DataItem.PostalCode = match.Groups[1].Value.Trim();
								DataItem.City = match.Groups[2].Value.Trim();
							}
							else if (!string.IsNullOrEmpty(match.Groups[4].Value))
							{
								DataItem.PostalCode = match.Groups[4].Value.Trim();
								DataItem.City = match.Groups[3].Value.Trim();
							}
						}
					}
					Match url = Regex.Match(WB.Url.ToString(), "(http:|https:)\\/\\/(.+)\\/");
					DataItem.Country = url.ToString().Substring(url.ToString().Length - 4).Replace("/", "")
						.Replace(".", "");
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
				string OldUrl = WB.Url.ToString();
				if (Program.AppSettings.ExtractEmails)
				{
					CylexLinksScraper.GetData1(ref PageItems, WB, mainForm);
				}
				WB.LoadUrlAndWait(OldUrl);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "page-link mr-2", false);
				Element NextButton;
				if (NextButtons.Count > 2)
				{
					NextButton = NextButtons[NextButtons.Count - 2];
					WB.Url.ToString();
					string NewUrl = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NewUrl);
					Thread.Sleep(1000);
				}
				else if (NextButtons.Count == 2)
				{
					NextButton = NextButtons[NextButtons.Count - 1];
					WB.Url.ToString();
					string NewUrl2 = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NewUrl2);
					Thread.Sleep(1000);
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

		// Token: 0x06000115 RID: 277 RVA: 0x0001340C File Offset: 0x0001160C
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				Program.RequestDelay();
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(800);
				string HTML = WB.GetHtml();
				Thread.Sleep(100);
				List<string[]> Cat = HTTPScraper.ParseHTML(HTML, "subCategory\":(.*?)\",");
				if (Cat.Count > 0)
				{
					DataItems[i].Category = Cat[0][1].Replace("[", "").Replace("\"", "");
					CylexLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Email = HTTPScraper.ParseHTML(HTML, "mailto:(.*?)\"");
				if (Email.Count > 0)
				{
					DataItems[i].Email = Email[0][1];
					CylexLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Website = HTTPScraper.ParseHTML(HTML, "url\":\"(.*?)\",");
				if (Website.Count > 0)
				{
					DataItems[i].Website = Website[0][1];
					if (DataItems[i].Email == "" || DataItems[i].Email == null)
					{
						DataItems[i].Email = EmailMiner.GetEmail(DataItems[i].Website, new string[] { "conta" });
					}
					CylexLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Fax = HTTPScraper.ParseHTML(HTML, "faxnumber\":(.*?),");
				try
				{
					if (Fax.Count > 0)
					{
						DataItems[i].Fax = Fax[0][1].Replace("\"", "");
						CylexLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
				}
				catch
				{
				}
				if (DataItems[i].Phone == "" || DataItems[i].Phone == null)
				{
					List<string[]> Tel = HTTPScraper.ParseHTML(HTML, "telephone\":(.*?),");
					if (Tel.Count > 0)
					{
						DataItems[i].Phone = Tel[0][1].Replace("\"", "").Replace("[", " ").Replace("]", "");
						CylexLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
				}
				List<string[]> State = HTTPScraper.ParseHTML(HTML, "addressRegion\":\"(.*?)\"");
				try
				{
					if (State.Count > 0)
					{
						DataItems[i].State = State[0][1];
						CylexLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
				}
				catch
				{
				}
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000136E8 File Offset: 0x000118E8
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			CylexDataScraper[] Pool = new CylexDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new CylexDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						CylexLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new CylexDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000117 RID: 279 RVA: 0x000137FC File Offset: 0x000119FC
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = HTTPScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[4].Value = HTTPScraper.ClearValue(Item.State);
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

		// Token: 0x06000118 RID: 280 RVA: 0x00002A43 File Offset: 0x00000C43
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

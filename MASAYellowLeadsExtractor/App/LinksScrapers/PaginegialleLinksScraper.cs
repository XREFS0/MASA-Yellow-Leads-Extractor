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
	// Token: 0x0200003A RID: 58
	public class PaginegialleLinksScraper
	{
		// Token: 0x0600019E RID: 414 RVA: 0x0001EE2C File Offset: 0x0001D02C
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
				new List<Element>();
				string[] array = new string[] { "search-itm " };
				for (int i = 0; i < array.Length; i++)
				{
					foreach (Element HItem in WebScraper.GetElements(WB, "div", array[i], false))
					{
						DataItem DataItem = new DataItem();
						List<Element> Title = WebScraper.GetElements(WB, HItem, "h2", "search-itm__rag google_analytics_tracked", false);
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
						DataItem.Country = "Italia";
						List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "search-itm__adr", true);
						if (Address.Count > 0)
						{
							string adrText = Address[0].innerText.Trim();
							if (adrText.IndexOf("-") > -1)
							{
								string[] parts = adrText.Split(new char[] { '-' }, 2);
								DataItem.Address = parts[0].Trim();
								string text = parts[1].Trim();
								Match capMatch = Regex.Match(text, "\\b\\d{5}\\b");
								if (capMatch.Success)
								{
									DataItem.PostalCode = capMatch.Value;
								}
								string cityPart = text;
								cityPart = Regex.Replace(cityPart, "\\b\\d{5}\\b", "").Trim();
								Match stateMatch = Regex.Match(cityPart, "\\(([^)]+)\\)");
								if (stateMatch.Success)
								{
									DataItem.State = "(" + stateMatch.Groups[1].Value + ")";
									cityPart = Regex.Replace(cityPart, "\\([^)]+\\)", "").Trim();
								}
								else
								{
									DataItem.State = "";
								}
								DataItem.City = cityPart.Trim();
							}
							else
							{
								DataItem.Address = adrText;
							}
						}
						else
						{
							DataItem.Address = "";
						}
						List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "bttn bttn--outline shinystat_ssxl google_analytics_tracked", true);
						try
						{
							if (Phone.Count > 0 && Phone[0]["href"].ToString().Split(new char[] { '?' })[0].Replace("https://wa.me/", "").IndexOf("+") > -1)
							{
								DataItem.Fax = "WhatsApp: " + Phone[0]["href"].ToString().Split(new char[] { '?' })[0].Replace("https://wa.me/", "");
							}
							else
							{
								DataItem.Fax = "";
							}
						}
						catch
						{
						}
						List<Element> Phone2 = WebScraper.GetElements(WB, HItem, "span", "list-menu-item__inner search-itm__phone-item", true);
						try
						{
							if (Phone2.Count > 0)
							{
								DataItem.Phone = Phone2[0].innerText.Trim();
							}
							else
							{
								DataItem.Phone = "";
							}
						}
						catch
						{
						}
						List<Element> Category = WebScraper.GetElements(WB, HItem, "div", "search-itm__category", true);
						try
						{
							if (Category.Count > 0)
							{
								DataItem.Category = Category[0].innerText.Trim();
							}
							else
							{
								DataItem.Category = "";
							}
							DataItem.DetailsLink = WebScraper.GetParent(WB, Category[0])["href"].ToString();
						}
						catch
						{
						}
						if (DataItem.DetailsLink != null && DataItem.DetailsLink != "")
						{
							PageItems.Add(DataItem);
							mainForm.dgvResults.Rows.Add(new object[]
							{
								DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, "Italia", DataItem.Phone, DataItem.Fax, DataItem.Website,
								DataItem.Email, "", DataItem.DetailsLink
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
				string OldUrl = WB.Url.ToString();
				if (Program.AppSettings.ExtractEmails && PageItems.Count > 0)
				{
					PaginegialleLinksScraper.GetData1(ref PageItems, WB, mainForm);
					Thread.Sleep(1000);
				}
				WB.LoadUrl(OldUrl);
				Thread.Sleep(2500);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "div", "listing__showmore", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					string OldUrl2 = WB.Url.ToString();
					List<string[]> AddressParts2 = HTTPScraper.ParseHTML(NextButton.innerHTML, "data-pageurl=\"(.*?)\" href=");
					if (AddressParts2.Count > 0)
					{
						string NewUrl = AddressParts2[0][1];
						WB.LoadUrlAndWait("https://www.paginegialle.it" + NewUrl);
					}
					while (WB.Url.ToString() == OldUrl2 || !WB.IsReady)
					{
						Thread.Sleep(500);
						Application.DoEvents();
						Thread.Sleep(500);
					}
					for (int j = 0; j < 1000; j++)
					{
						Application.DoEvents();
					}
				}
				else
				{
					NextButton = null;
				}
				if (Program.IsStopped() || NextButton == null)
				{
					goto IL_0604;
				}
			}

			IL_0604:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			int i = 0;
			while (i < DataItems.Count)
			{
				if (Program.IsStopped())
				{
					return;
				}
				Program.RequestDelay();
				try
				{
					WB.LoadUrl(DataItems[i].DetailsLink);
					Thread.Sleep(PaginegialleLinksScraper._rnd.Next(1000, 2000));
				}
				catch
				{
					goto IL_0141;
				}
				goto IL_0049;
				IL_0141:
				i++;
				continue;
				IL_0049:
				string html = "";
				try
				{
					html = WB.GetHtml();
				}
				catch
				{
				}
				if (!string.IsNullOrEmpty(html))
				{
					List<string[]> Website = HTTPScraper.ParseHTML(html, "(?i)data-pag=\"www\"(.*?)href=(.*?)target=\"_blank\"");
					if (Website.Count > 0)
					{
						try
						{
							DataItems[i].Website = Website[0][2].Split(new char[] { ' ' })[0].Replace("\"", "");
							PaginegialleLinksScraper.UpdateTable(DataItems[i], mainForm, 0f);
						}
						catch
						{
						}
					}
					List<string[]> Email = HTTPScraper.ParseHTML(html, "email\":\"(.*?)\",");
					if (Email.Count > 0 && Email[0][1].ToString().Contains("@"))
					{
						try
						{
							DataItems[i].Email = Email[0][1].Split(new char[] { '"' })[0];
							PaginegialleLinksScraper.UpdateTable(DataItems[i], mainForm, 0f);
						}
						catch
						{
						}
					}
				}
				if (Program.IsStopped())
				{
					return;
				}
				goto IL_0141;
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0001F66C File Offset: 0x0001D86C
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			PaginegialleDataScraper[] Pool = new PaginegialleDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PaginegialleDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						PaginegialleLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new PaginegialleDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060001A1 RID: 417 RVA: 0x0001F780 File Offset: 0x0001D980
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[3].Value = HTTPScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = HTTPScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = HTTPScraper.ClearValue(Item.PostalCode);
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

		// Token: 0x040000DF RID: 223
		private static readonly Random _rnd = new Random();

		// Token: 0x040000E0 RID: 224
		private static volatile bool _blockHeavyResources = false;
	}
}

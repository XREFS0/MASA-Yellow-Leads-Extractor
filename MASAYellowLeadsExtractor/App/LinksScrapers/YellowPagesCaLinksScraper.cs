using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200003B RID: 59
	public class YellowPagesCaLinksScraper
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x0001F94C File Offset: 0x0001DB4C
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			while (!Program.IsStopped())
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "listing_right_section", true))
				{
					if (Program.IsStopped())
					{
						break;
					}
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "listing__name--link listing__link jsListingName", true);
					if (Title.Count > 0)
					{
						try
						{
							DataItem.BusinessName = Title[0].innerText;
							DataItem.DetailsLink = Title[0]["href"].ToString();
						}
						catch
						{
						}
					}
					if (Program.IsStopped())
					{
						break;
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "listing__address--full", true);
					if (Address.Count > 0)
					{
						List<string[]> AddItems = HTTPScraper.ParseHTML(Address[0].innerHTML, "itemprop=\"streetAddress\">(.*?)</span>");
						try
						{
							if (AddItems.Count > 0)
							{
								DataItem.Address = AddItems[0][1].Split(new char[] { '<' })[0];
							}
						}
						catch
						{
						}
					}
					if (Program.IsStopped())
					{
						break;
					}
					List<Element> Address2 = WebScraper.GetElements(WB, HItem, "span", "listing__address--full", true);
					if (Address2.Count > 0)
					{
						List<string[]> AddItems2 = HTTPScraper.ParseHTML(Address2[0].innerHTML, "itemprop=\"postalCode\">(.*)</span>");
						try
						{
							if (AddItems2.Count > 0)
							{
								DataItem.PostalCode = AddItems2[0][1].Split(new char[] { '<' })[0];
							}
						}
						catch
						{
						}
					}
					if (Program.IsStopped())
					{
						break;
					}
					List<Element> Address3 = WebScraper.GetElements(WB, HItem, "span", "listing__address--full", true);
					if (Address3.Count > 0)
					{
						List<string[]> AddItems3 = HTTPScraper.ParseHTML(Address3[0].innerHTML, "itemprop=\"addressLocality\">(.*)</span>");
						try
						{
							if (AddItems3.Count > 0)
							{
								DataItem.City = AddItems3[0][1].Split(new char[] { '<' })[0];
							}
						}
						catch
						{
						}
					}
					if (Program.IsStopped())
					{
						break;
					}
					DataItem.Country = "Canada";
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "div", "listing__headings__roots", true);
					if (Categories.Count > 0)
					{
						DataItem.Category = Categories[0].innerText.Split(new char[] { ',' })[0];
					}
					if (Program.IsStopped())
					{
						break;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "span", "ypicon ypicon-web mlr__icon", true);
					if (Website.Count > 0)
					{
						try
						{
							DataItem.Website = WebScraper.GetParent(WB, Website[0])["href"].ToString().Split(new char[] { '=' })[1].Replace("%3A%2F%2F", "://").Replace("%2F", "/");
						}
						catch
						{
							DataItem.Website = "";
						}
					}
					if (Program.IsStopped())
					{
						break;
					}
					if (Program.AppSettings.ExtractEmails)
					{
						if (Program.IsStopped())
						{
							break;
						}
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "contact" });
						if (Program.IsStopped())
						{
							break;
						}
					}
					List<Element> Phone = WebScraper.GetElementsByTag(WB, HItem, "h4");
					if (Phone.Count > 0)
					{
						DataItem.Phone = Phone[0].innerText;
					}
					if (Program.IsStopped())
					{
						break;
					}
					if (!string.IsNullOrEmpty(DataItem.DetailsLink))
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
}
				if (Program.IsStopped())
				{
					break;
				}
				mainForm.tsProgress.Value = 0;

				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "ypbtn btn-theme pageButton", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = null;
					if (Program.IsStopped())
					{
						break;
					}
					WB.Url.ToString();
					if (NextButtons.Count < 2 && NextButtons[0].outerHTML.IndexOf("Next") > -1)
					{
						NextButton = NextButtons[0];
						string NextUrl = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl);
						if (Program.IsStopped())
						{
							break;
						}
					}
					if (NextButtons.Count > 1)
					{
						NextButton = NextButtons[1];
						string NextUrl2 = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl2);
						if (Program.IsStopped())
						{
							break;
						}
					}
					SleepWithStop(1000);
					if (Program.IsStopped())
					{
						break;
					}
				}
				else
				{
					NextButton = null;
				}
				if (Program.IsStopped() || NextButton == null)
				{
					break;
				}
			}
			mainForm.tsProgress.Value = 0;
			if (!Program.IsStopped())
			{
				MessageBox.Show(Program.LanguagesManager.WorkIsDone);
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0001FFBC File Offset: 0x0001E1BC
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			YellowPagesCaDataScraper[] Pool = new YellowPagesCaDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YellowPagesCaDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YellowPagesCaLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new YellowPagesCaDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060001A6 RID: 422 RVA: 0x000200D0 File Offset: 0x0001E2D0
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[7].Value = YellowPagesCaLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[10].Value = YellowPagesCaLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00002A43 File Offset: 0x00000C43
		private static string ClearValue(string Value)
		{
			if (Value != null)
			{
				return Value.Replace("&#x27", "");
			}
			return "";
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000201A8 File Offset: 0x0001E3A8
		private static void SleepWithStop(int totalMs)
		{
			int step = 50;
			for (int waited = 0; waited < totalMs; waited += step)
			{
				if (Program.IsStopped())
				{
					return;
				}
				Thread.Sleep(step);
				Application.DoEvents();
			}
		}
	}
}

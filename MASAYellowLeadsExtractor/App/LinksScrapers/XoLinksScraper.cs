using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000020 RID: 32
	public static class XoLinksScraper
	{
		// Token: 0x060000FF RID: 255 RVA: 0x00011588 File Offset: 0x0000F788
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "listingWhiteArea", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "et-v2", true);
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
					List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "addressProfile", true);
					if (Address.Count > 0)
					{
						string innerHTML = Address[0].innerHTML;
						List<string[]> AItems = HTTPScraper.ParseHTML(innerHTML, "<span itemprop=\"streetAddress\">(.*?)</span>");
						if (AItems.Count > 0)
						{
							DataItem.Address = AItems[0][1];
						}
						List<string[]> AItemsB = HTTPScraper.ParseHTML(innerHTML, "<span itemprop=\"addressLocality\">(.*?)</span>");
						if (AItemsB.Count > 0)
						{
							DataItem.City = AItemsB[0][1];
						}
						List<string[]> AItemsC = HTTPScraper.ParseHTML(innerHTML, "<span itemprop=\"postalCode\">(.*?)</span>");
						if (AItemsC.Count > 0)
						{
							DataItem.PostalCode = AItemsC[0][1];
						}
					}
					DataItem.Country = "Greece";
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "et-v2-additional", true);
					if (Phone.Count > 0)
					{
						DataItem.Phone = Phone[0]["href"].ToString().Replace("tel:", "");
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "et-v2-additional", true);
					if (Website.Count > 1 && Website[1]["href"].ToString().IndexOf('w') > -1)
					{
						DataItem.Website = Website[1]["href"].ToString();
					}
					else
					{
						DataItem.Website = "";
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
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, "Greece", DataItem.Phone, DataItem.Fax, DataItem.Website,
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
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "page_next", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = null;
					if (NextButtons[0].outerHTML.IndexOf("Next") > -1)
					{
						NextButton = NextButtons[0];
						string NextUrl = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl);
					}
					if (NextButtons.Count > 1 && NextButtons[1].outerHTML.IndexOf("Next") > -1)
					{
						NextButton = NextButtons[1];
						string NextUrl2 = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl2);
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

		// Token: 0x06000100 RID: 256 RVA: 0x00011A48 File Offset: 0x0000FC48
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			XoDataScraper[] Pool = new XoDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new XoDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						XoLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new XoDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000101 RID: 257 RVA: 0x00011B5C File Offset: 0x0000FD5C
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

		// Token: 0x06000102 RID: 258 RVA: 0x00002A43 File Offset: 0x00000C43
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

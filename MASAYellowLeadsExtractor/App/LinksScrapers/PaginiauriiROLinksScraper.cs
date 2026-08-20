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
	// Token: 0x02000032 RID: 50
	internal class PaginiauriiROLinksScraper
	{
		// Token: 0x06000167 RID: 359 RVA: 0x0001A024 File Offset: 0x00018224
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "result", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "h2", "item-heading", true);
					if (Title.Count > 0)
					{
						List<string[]> AddressParts2 = HTTPScraper.ParseHTML(Title[0].innerHTML, "<a href=\"(.*?)\" class=\"t-fpbc\">(.*?)</a>");
						if (AddressParts2.Count > 0)
						{
							try
							{
								DataItem.BusinessName = AddressParts2[0][2];
								DataItem.DetailsLink = "https://www.paginiaurii.ro" + AddressParts2[0][1];
							}
							catch
							{
							}
						}
					}
					List<Element> Category = WebScraper.GetElements(WB, HItem, "span", "category am-inner", true);
					if (Category.Count > 0)
					{
						DataItem.Category = Category[0].innerText.Replace("Domeniul de activitate:", "");
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "address", true);
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerText;
						List<string[]> AddItems = HTTPScraper.ParseHTML(DataItem.Address.Replace("\r\n", ""), "(.*?), ([^\\,]+), Cod Postal (\\d+)");
						if (AddItems.Count > 0)
						{
							DataItem.Address = AddItems[0][1].Trim();
							DataItem.City = AddItems[0][2];
							DataItem.PostalCode = AddItems[0][3];
							DataItem.Country = "Romania";
						}
						else
						{
							string[] _a = DataItem.Address.Replace("\r\n", "").Split(new char[] { ',' });
							if (_a.Length > 1)
							{
								DataItem.City = _a[_a.Length - 1].Trim();
								_a = _a.Where<string>((string w) => w != _a[_a.Length - 1]).ToArray<string>();
								DataItem.Address = string.Join(",", _a).Trim(new char[] { ',' }).Trim();
								DataItem.Country = "Romania";
							}
						}
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "i", "icon-phone", true);
					if (Phone.Count > 0)
					{
						try
						{
							Element _parent = WebScraper.GetParent(WB, Phone[0]);
							DataItem.Phone = WebScraper.GetParent(WB, _parent).innerText;
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
				PaginiauriiROLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "ul", "pagination", false);
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					List<string[]> AddressParts3 = HTTPScraper.ParseHTML(NextButton.innerHTML, "<a href=\"(.*?)\">Urmatorul</a>");
					if (AddressParts3.Count > 0)
					{
						string NewUrl = AddressParts3[0][1].Replace("&amp;", "&");
						Thread.Sleep(500);
						WB.LoadUrlAndWait("https://www.paginiaurii.ro/" + NewUrl);
						Thread.Sleep(500);
					}
					else
					{
						NextButton = null;
					}
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

		// Token: 0x06000168 RID: 360 RVA: 0x0001A538 File Offset: 0x00018738
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			PaginiauriiRODataScraper[] Pool = new PaginiauriiRODataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PaginiauriiRODataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						PaginiauriiROLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new PaginiauriiRODataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000169 RID: 361 RVA: 0x0001A644 File Offset: 0x00018844
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
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

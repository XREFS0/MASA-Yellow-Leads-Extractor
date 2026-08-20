using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200002D RID: 45
	public class AustraliaLinksScraper
	{
		// Token: 0x06000152 RID: 338 RVA: 0x00017CA4 File Offset: 0x00015EA4
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				while (!WB.IsReady)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				string[] array = new string[] { "diamond", "gold", "silver", "free" };
				for (int i = 0; i < 4; i++)
				{
					foreach (Element HItem in WebScraper.GetElements(WB, "div", array[i], false))
					{
						DataItem DataItem = new DataItem();
						List<Element> Title = WebScraper.GetElements(WB, HItem, "h2", "aTitle ellipsis", true);
						if (Title.Count > 0)
						{
							DataItem.BusinessName = Title[0].innerText;
							WebScraper.GetChildren(WB, Title[0]);
						}
						List<Element> Link = WebScraper.GetElementsByTag(WB, HItem, "a");
						if (Link.Count > 0)
						{
							DataItem.DetailsLink = Link[0]["href"].ToString();
						}
						List<Element> Address = WebScraper.GetElements(WB, HItem, "address", "advAdress", false);
						if (Address.Count > 0)
						{
							try
							{
								string indirizzo = Address[0].innerText;
								DataItem.Address = indirizzo;
								List<string[]> AddressParts = HTTPScraper.ParseHTML(indirizzo, "(.*?) - (.*?) - (\\d{5})-(\\d{3})");
								if (AddressParts.Count > 0)
								{
									DataItem.Address = AddressParts[0][1];
									DataItem.City = AddressParts[0][2];
									DataItem.PostalCode = AddressParts[0][3] + "-" + AddressParts[0][4];
								}
							}
							catch
							{
							}
						}
						DataItem.Country = "Brazil";
						List<Element> Categories = WebScraper.GetElements(WB, HItem, "p", "advCategory", true);
						if (Categories.Count > 0)
						{
							DataItem.Category = Categories[0].innerText;
						}
						else
						{
							List<Element> CategoriesA = WebScraper.GetElements(WB, HItem, "span", "advCategory", false);
							if (CategoriesA.Count > 0)
							{
								try
								{
									DataItem.Category = CategoriesA[0].innerText;
								}
								catch
								{
								}
							}
						}
						List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "site", true);
						if (Website.Count > 0)
						{
							DataItem.Website = Website[0]["href"].ToString();
						}
						else
						{
							List<Element> WebsiteA = WebScraper.GetElements(WB, HItem, "a", "site", true);
							if (WebsiteA.Count > 0)
							{
								DataItem.Website = WebsiteA[1]["href"].ToString();
							}
						}
						if (DataItem.Website != "")
						{
							DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "conta" });
						}
						List<Element> Phone = WebScraper.GetElements(WB, HItem, "div", "col mr", false);
						if (Phone.Count > 0)
						{
							List<string[]> AddressParts2 = HTTPScraper.ParseHTML(Phone[0].innerHTML, "<a href=\"tel:(.*?)\"");
							if (AddressParts2.Count > 0)
							{
								DataItem.Phone = AddressParts2[0][1];
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
				}
				AustraliaLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "linkSeeMore my-4 d-block d-sm-none", true);
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					string currentUrl = WB.Url.ToString();
					string text = NextButton["href"].ToString();
					string keyword = HttpUtility.ParseQueryString(new Uri(currentUrl).Query)["what"];
					string newHref = text.Replace("?", "?searchbox=true&what=" + keyword + "&");
					string OldUrl = WB.Url.ToString();
					WB.LoadUrlAndWait(newHref);
					Thread.Sleep(1000);
					Application.DoEvents();
					while (WB.Url.ToString() == OldUrl)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
					}
				}
				else
				{
					Console.WriteLine("No 'See More' button found.");
				}
				if (Program.IsStopped() || NextButton == null)
				{
					goto IL_0572;
				}
			}

			IL_0572:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0001828C File Offset: 0x0001648C
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
						AustraliaLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
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

		// Token: 0x06000154 RID: 340 RVA: 0x00018398 File Offset: 0x00016598
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = HTTPScraper.ClearValue(Item.Category);
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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000039 RID: 57
	public static class PaginasamarillasLinksScraper
	{
		// Token: 0x06000199 RID: 409 RVA: 0x0001E500 File Offset: 0x0001C700
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "box", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "div", "row", false);
					if (Title.Count > 0)
					{
						try
						{
							List<Element> Children = WebScraper.GetChildren(WB, Title[1]);
							List<Element> _Children = WebScraper.GetChildren(WB, Children[1]);
							DataItem.BusinessName = _Children[0].innerText;
							DataItem.DetailsLink = (DataItem.DetailsLink = Children[1]["href"].ToString());
						}
						catch
						{
						}
					}
					List<Element> Category = WebScraper.GetElements(WB, HItem, "p", "categ", false);
					if (Category.Count > 0)
					{
						DataItem.Category = Category[0].innerText;
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "llama-desplegable btn btn-amarillo btn-block phone hidden d-none", true);
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
					List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "location", false);
					if (Address.Count > 0)
					{
						List<string[]> AddItems = HTTPScraper.ParseHTML(Address[0].innerHTML, "<span itemprop=\"streetAddress\">(.*?)</span>");
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
					List<Element> Address2 = WebScraper.GetElements(WB, HItem, "span", "location", false);
					if (Address2.Count > 0)
					{
						List<string[]> AddItems2 = HTTPScraper.ParseHTML(Address2[0].innerHTML, "<span itemprop=\"postalCode\">(.*)</span>");
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
					List<Element> Address3 = WebScraper.GetElements(WB, HItem, "span", "location", false);
					if (Address3.Count > 0)
					{
						List<string[]> AddItems3 = HTTPScraper.ParseHTML(Address3[0].innerHTML, "<span itemprop=\"addressLocality\">(.*)</span>");
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
					DataItem.Country = "Spain";
					if (DataItem.DetailsLink != null && DataItem.DetailsLink != "")
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category,
							DataItem.BusinessName,
							DataItem.Address,
							DataItem.City,
							DataItem.State,
							DataItem.PostalCode,
							DataItem.Country,
							DataItem.Phone,
							"",
							DataItem.Website,
							DataItem.Email,
							string.Format("{0}?gm=map", DataItem.DetailsLink),
							DataItem.DetailsLink
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
					PaginasamarillasLinksScraper.GetData1(ref PageItems, WB, mainForm);
				}
				WB.LoadUrlAndWait(OldUrl);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "i", "fa icon-flecha-derecha", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = WebScraper.GetParent(WB, NextButtons[0]);
					string OldUrl2 = WB.Url.ToString();
					string NewUrl = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NewUrl);
					Thread.Sleep(1000);
					Application.DoEvents();
					while (WB.Url.ToString() == OldUrl2)
					{
						Thread.Sleep(1000);
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

		// Token: 0x0600019A RID: 410 RVA: 0x0001EA5C File Offset: 0x0001CC5C
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(new Random().Next(1500, 3001));
				string HTML = WB.GetHtml();
				List<string[]> Website = HTTPScraper.ParseHTML(HTML, "adWebEstablecimiento(.*?),");
				if (Website.Count > 0)
				{
					DataItems[i].Website = "https://" + Website[0][1].Replace("&quot;", "").Replace(":", "").Replace("http//", "")
						.Replace("https//", "")
						.Split(new char[] { '?' })[0];
					PaginasamarillasLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Email = HTTPScraper.ParseHTML(HTML, "customerMail(.*?),");
				if (Email.Count > 0)
				{
					DataItems[i].Email = Email[0][1].Replace("&quot;", "").Replace(":", "");
					PaginasamarillasLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Cat = HTTPScraper.ParseHTML(HTML, ",&quot;activity(.*?),");
				if (Cat.Count > 0)
				{
					DataItems[i].Category = Cat[0][1].Replace("&quot;", "").Replace(":", "");
					PaginasamarillasLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0001EC10 File Offset: 0x0001CE10
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			PaginasamarillasDataScraper[] Pool = new PaginasamarillasDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PaginasamarillasDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						PaginasamarillasLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new PaginasamarillasDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				Thread.Sleep(2000);
				Application.DoEvents();
				if (Program.IsStopped())
				{
					break;
				}
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0001ED24 File Offset: 0x0001CF24
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = PaginasamarillasLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[9].Value = PaginasamarillasLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = PaginasamarillasLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00002A43 File Offset: 0x00000C43
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

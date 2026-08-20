using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000035 RID: 53
	public static class AziendeVirgilioLinksScraper
	{
		// Token: 0x06000172 RID: 370 RVA: 0x0001B69C File Offset: 0x0001989C
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
				foreach (Element HItem in WebScraper.GetElements(WB, "li", "box_azienda", false))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "h3");
					if (Title.Count > 0)
					{
						try
						{
							List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
							DataItem.BusinessName = Title[0].innerText.Trim();
							DataItem.DetailsLink = Children[0]["href"].ToString();
						}
						catch
						{
							DataItem.DetailsLink = "";
						}
					}
					List<Element> Category = WebScraper.GetElements(WB, HItem, "ul", "categorie_azienda", true);
					if (Category.Count > 0)
					{
						try
						{
							DataItem.Category = Category[0].innerText.Trim().Replace("\r\n", ", ");
						}
						catch
						{
						}
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "p", "adr", true);
					if (Address.Count > 0)
					{
						try
						{
							DataItem.Address = Address[0].innerText.Trim().Replace("\t", "");
						}
						catch
						{
						}
						List<Element> PostCode = WebScraper.GetElements(WB, Address[0], "span", "postal-code", true);
						if (PostCode.Count > 0)
						{
							try
							{
								DataItem.PostalCode = PostCode[0].innerText.Trim();
							}
							catch
							{
							}
						}
						List<Element> State = WebScraper.GetElements(WB, Address[0], "span", "region", true);
						if (State.Count > 0)
						{
							string StateCity = "";
							try
							{
								StateCity = State[0].innerText.Trim();
							}
							catch
							{
							}
							string[] arrStateCity = StateCity.Split(new char[] { '-' });
							DataItem.City = arrStateCity[0].Trim();
							if (arrStateCity.Length > 1)
							{
								DataItem.State = arrStateCity[1].Trim();
							}
						}
						DataItem.MapLink = "";
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "li", "show_mobile", true);
					if (Phone.Count > 0)
					{
						try
						{
							List<string[]> AItems = HTTPScraper.ParseHTML(Phone[0].innerHTML, "href=\"tel:(.*?)\"");
							if (AItems.Count > 0)
							{
								DataItem.Phone = " +39 " + AItems[0][1];
							}
						}
						catch
						{
						}
					}
					List<Element> Phone2 = WebScraper.GetElements(WB, HItem, "a", "cta-bttn cta-bttn--wh", true);
					if (Phone2.Count > 0)
					{
						try
						{
							DataItem.Fax = "WhatsApp:" + Phone2[0]["href"].ToString().Split(new char[] { '?' })[0].Replace("https://wa.me/", "");
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
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, "Italy", DataItem.Phone, DataItem.Fax, DataItem.Website,
							"", DataItem.MapLink, DataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
					}
					else
					{
						DataItem.DetailsLink = "";
					}
if (Program.IsStopped())
					{
						return;
					}
				}
				AziendeVirgilioLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "fine bgc10 c2", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					string OldUrl = WB.Url.ToString();
					WebScraper.InvokeMember(WB, NextButton, "click");
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

		// Token: 0x06000173 RID: 371 RVA: 0x0001BC80 File Offset: 0x00019E80
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			AziendeVirgilioDataScraper[] Pool = new AziendeVirgilioDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new AziendeVirgilioDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						AziendeVirgilioLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new AziendeVirgilioDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000174 RID: 372 RVA: 0x0001BD94 File Offset: 0x00019F94
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[2].Value = AziendeVirgilioLinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = AziendeVirgilioLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[5].Value = AziendeVirgilioLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[7].Value = AziendeVirgilioLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[9].Value = AziendeVirgilioLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = AziendeVirgilioLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00002A43 File Offset: 0x00000C43
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

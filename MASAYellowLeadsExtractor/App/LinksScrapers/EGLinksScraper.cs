using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000019 RID: 25
	public static class EGLinksScraper
	{
		// Token: 0x060000DB RID: 219 RVA: 0x0000E030 File Offset: 0x0000C230
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
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "col-xs-12 item-details", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "item-title", true);
					if (Title.Count > 0)
					{
						DataItem.BusinessName = Title[0].innerText;
						DataItem.DetailsLink = Title[0]["href"].ToString();
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "a", "address-text", true);
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerText;
						List<string[]> AddItems = HTTPScraper.ParseHTML(DataItem.Address, "(.*?),\\s(.*?),\\s(.+).");
						if (AddItems.Count > 0)
						{
							DataItem.City = AddItems[0][3];
						}
					}
					DataItem.Country = "Egypt";
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "span", "call-us-click", true);
					if (Phone.Count > 0)
					{
						WebScraper.InvokeMember(WB, Phone[0], "click");
						Thread.Sleep(500);
						List<string[]> AddItems2 = HTTPScraper.ParseHTML(WebScraper.GetParent(WB, Phone[0]).innerHTML, "tel:(.*?)\">");
						if (AddItems2.Count > 0)
						{
							DataItem.Phone = AddItems2[0][1].Split(new char[] { '&' })[0];
						}
					}
					List<Element> Fax = WebScraper.GetElements(WB, HItem, "a", "whatsAppLink", true);
					if (Fax.Count > 0)
					{
						DataItem.Fax = "WhatsApp: " + Fax[0]["href"].ToString().Replace("https://web.whatsapp.com/send?phone=", "");
					}
					List<Element> Cat = WebScraper.GetElements(WB, HItem, "span", "category", true);
					if (Cat.Count > 0)
					{
						DataItem.Category = Cat[0].innerText;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "website", true);
					if (Website.Count > 0)
					{
						try
						{
							DataItem.Website = Website[0]["href"].ToString();
							goto IL_026C;
						}
						catch
						{
							goto IL_026C;
						}
						goto IL_0260;
					}
					goto IL_0260;
					IL_026C:
					if (DataItem.Website != "")
					{
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "conta" });
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
					IL_0260:
					DataItem.Website = "";
					goto IL_026C;
				}
				mainForm.tsProgress.Value = 0;
Element NextButton = null;
				List<Element> NextButtons = WebScraper.GetElements(WB, "ul", "pagination center-pagination", true);
				if (NextButtons.Count > 0)
				{
					string code = NextButtons[0].innerHTML;
					NextButton = NextButtons[0];
					List<string[]> AddressParts2 = HTTPScraper.ParseHTML(code, "<a href=\"(.*?)\" aria-label=\"Next\">");
					if (AddressParts2.Count > 0)
					{
						WB.Url.ToString();
						string NewUrl = AddressParts2[0][1].Replace("amp;", "");
						WB.LoadUrlAndWait("https://yellowpages.com.eg" + NewUrl);
						Thread.Sleep(1200);
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

		// Token: 0x060000DC RID: 220 RVA: 0x0000E53C File Offset: 0x0000C73C
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			EGDataScraper[] Pool = new EGDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new EGDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						EGLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new EGDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000DD RID: 221 RVA: 0x0000E650 File Offset: 0x0000C850
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

		// Token: 0x060000DE RID: 222 RVA: 0x00002A43 File Offset: 0x00000C43
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

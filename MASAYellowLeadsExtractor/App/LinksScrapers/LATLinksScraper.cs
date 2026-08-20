using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200001C RID: 28
	public static class LATLinksScraper
	{
		// Token: 0x060000EE RID: 238 RVA: 0x0000F8B0 File Offset: 0x0000DAB0
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
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "advertise Advertise_advertise", false))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "div", "title Advertise_title", false);
					if (Title.Count > 0)
					{
						try
						{
							List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
							DataItem.BusinessName = Title[0].innerText.Trim();
							DataItem.DetailsLink = Children[0]["href"].ToString().Remove(Children[0]["href"].ToString().Length - 1);
							if (DataItem.DetailsLink.Contains(".com.ar"))
							{
								DataItem.Country = "Argentina";
							}
							if (DataItem.DetailsLink.Contains(".cl"))
							{
								DataItem.Country = "Chile";
							}
							if (DataItem.DetailsLink.Contains(".com.co"))
							{
								DataItem.Country = "Colombia";
							}
							if (DataItem.DetailsLink.Contains(".com.sv"))
							{
								DataItem.Country = "El Salvador";
							}
							if (DataItem.DetailsLink.Contains(".com.ec"))
							{
								DataItem.Country = "Ecuador";
							}
							if (DataItem.DetailsLink.Contains("com.gt"))
							{
								DataItem.Country = "Guatemala";
							}
							if (DataItem.DetailsLink.Contains("com.pa"))
							{
								DataItem.Country = "Panama";
							}
							if (DataItem.DetailsLink.Contains("com.ni"))
							{
								DataItem.Country = "Nicaragua";
							}
							if (DataItem.DetailsLink.Contains("com.pe"))
							{
								DataItem.Country = "Peru";
							}
						}
						catch
						{
						}
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "locality Advertise_locality", false);
					if (Address.Count > 0)
					{
						DataItem.City = Address[0].innerText.Trim();
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "phone Advertise_phone", false);
					if (Phone.Count > 0)
					{
						WebScraper.GetChildren(WB, Phone[0]);
						DataItem.Phone = Phone[0].innerText;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "div", "address Advertise_address", false);
					if (Website.Count > 0)
					{
						List<string[]> AddItems2 = HTTPScraper.ParseHTML(Website[0].innerHTML, "<b>(.*?)</b>");
						if (AddItems2.Count > 0)
						{
							DataItem.Address = AddItems2[0][1];
						}
						List<string[]> AddItems3 = HTTPScraper.ParseHTML(Website[0].innerHTML, ">www.(.*?)</a>");
						if (AddItems3.Count > 0)
						{
							DataItem.Website = "http://www." + AddItems3[0][1];
						}
					}
					else
					{
						DataItem.Website = "";
					}
					List<Element> Fax = WebScraper.GetElements(WB, HItem, "a", "btn btn-sm Advertise_btn", false);
					if (Fax.Count > 0)
					{
						DataItem.Fax = "WhatsApp: " + Fax[0]["href"].ToString().Replace("http://wa.me", "");
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
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "li", "page-item-next page-item", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = WebScraper.GetChildren(WB, NextButtons[0])[0];
					WB.Url.ToString();
					WebScraper.InvokeMember(WB, NextButton, "click");
					Thread.Sleep(2000);
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

		// Token: 0x060000EF RID: 239 RVA: 0x0000FE90 File Offset: 0x0000E090
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			LATDataScraper[] Pool = new LATDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new LATDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						LATLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new LATDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000F0 RID: 240 RVA: 0x0000FF9C File Offset: 0x0000E19C
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

		// Token: 0x060000F1 RID: 241 RVA: 0x00002A43 File Offset: 0x00000C43
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

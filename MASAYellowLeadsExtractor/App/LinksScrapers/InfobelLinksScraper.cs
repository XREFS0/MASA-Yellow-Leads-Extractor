using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000031 RID: 49
	internal class InfobelLinksScraper
	{
		// Token: 0x06000162 RID: 354 RVA: 0x0001971C File Offset: 0x0001791C
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				List<Element> Items = WebScraper.GetElements(WB, "div", "customer-box-item", true);
				foreach (Element a in WebScraper.GetElements(WB, "a", "customer-info-detail", true))
				{
					if (a["href"].ToString().IndexOf("click") > -1)
					{
						try
						{
							WebScraper.InvokeMember(WB, a, "click");
						}
						catch
						{
						}
						Thread.Sleep(100);
						Application.DoEvents();
					}
				}
				foreach (Element Item in Items)
				{
					DataItem DataItem = new DataItem();
					List<Element> BusinessName = WebScraper.GetElements(WB, Item, new string[] { "h1", "h2" }, "customer-item-name");
					if (BusinessName.Count > 0)
					{
						DataItem.BusinessName = "";
						string[] bnParts = BusinessName[0].innerText.Split(new char[] { '.' });
						for (int i = 1; i < bnParts.Length; i++)
						{
							DataItem dataItem = DataItem;
							dataItem.BusinessName = dataItem.BusinessName + bnParts[i] + " ";
						}
						DataItem.BusinessName = DataItem.BusinessName.Trim();
						List<Element> Children = WebScraper.GetChildren(WB, BusinessName[0]);
						DataItem.MapLink = Children[0]["href"].ToString();
						DataItem.DetailsLink = Children[0]["href"].ToString();
						List<string[]> PhoneParts = HTTPScraper.ParseHTML(DataItem.DetailsLink, "-(.*?)businessdetails.aspx");
						if (PhoneParts.Count > 0)
						{
							DataItem.Phone = "Tel: " + PhoneParts[0][1].Replace("/", "");
						}
					}
					List<Element> Phone = WebScraper.GetElementsByAttribute(WB, Item, "a", "id", "click-phone_", false);
					if (Phone.Count > 0)
					{
						try
						{
							Element Click = Phone[0];
							WebScraper.InvokeMember(WB, Click, "click");
							List<Element> Phone2 = WebScraper.GetElements(WB, Item, "span", "customer-info-detail", true);
							if (Phone2.Count > 0)
							{
								DataItem.Phone = Phone2[0].innerText;
							}
						}
						catch
						{
						}
					}
					List<Element> Category = WebScraper.GetElements(WB, Item, "div", "customer-item-labels-list", false);
					if (Category.Count > 0)
					{
						DataItem.Category = Category[0].innerText;
					}
					else
					{
						DataItem.Category = "";
					}
					List<Element> Address = WebScraper.GetElements(WB, Item, "span", "customer-info-detail highlighted address", true);
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerHTML;
						List<string[]> AddressParts = HTTPScraper.ParseHTML(DataItem.Address, "<span class=\"detail-text\">(.*?)<br>(.*?)</span>");
						if (AddressParts.Count > 0)
						{
							DataItem.Address = AddressParts[0][1].Trim();
							DataItem.City = AddressParts[0][2].Trim();
						}
						string[] url = WB.Url.ToString().Split(new char[] { '/' });
						for (int j = 0; j < url.Length; j++)
						{
							if (url[j].IndexOf("Search") > -1)
							{
								DataItem.Country = url[j - 1].Substring(0, 1).ToUpper() + url[j - 1].Substring(1);
								break;
							}
						}
					}
					List<Element> Mobile = WebScraper.GetElements(WB, Item, "span", "detail-icon font-icon icon-mobile-phone", true);
					if (Mobile.Count > 1)
					{
						DataItem dataItem2 = DataItem;
						dataItem2.Phone = dataItem2.Phone + ", " + WebScraper.GetParent(WB, Mobile[1]).innerText.Trim();
					}
					List<Element> Website = WebScraper.GetElements(WB, Item, "span", "detail-icon font-icon icon-globe", true);
					if (Website.Count > 0)
					{
						DataItem.Website = WebScraper.GetParent(WB, Website[0])["href"].ToString();
					}
					if (Program.AppSettings.ExtractEmails)
					{
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "contact" });
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
Program.RequestDelay();
				Element NextButton = null;
				List<Element> NextButtons = WebScraper.GetElementsByTag(WB, "li");
				if (NextButtons.Count > 0)
				{
					for (int RowIndex = 0; RowIndex < NextButtons.Count; RowIndex++)
					{
						string innerHTML = NextButtons[RowIndex].innerHTML;
						NextButton = NextButtons[RowIndex];
						List<string[]> AddressParts2 = HTTPScraper.ParseHTML(innerHTML, "<a href=\"(.*?)\" rel=\"nofollow\">(.*?)Next</span></a>");
						if (AddressParts2.Count > 0)
						{
							WB.Url.ToString();
							string NewUrl = AddressParts2[0][1];
							WB.LoadUrlAndWait("https://www.infobel.com" + NewUrl);
							Thread.Sleep(500);
							RowIndex = NextButtons.Count - 1;
						}
					}
				}
				if (NextButton == null)
				{
					goto Block_6;
				}
			}

			mainForm.tsProgress.Value = 0;
			return;
			Block_6:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00019E10 File Offset: 0x00018010
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			InfobelDataScraper[] Pool = new InfobelDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new InfobelDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						InfobelLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new InfobelDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000164 RID: 356 RVA: 0x00019F1C File Offset: 0x0001811C
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = InfobelLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[9].Value = InfobelLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = InfobelLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00002A43 File Offset: 0x00000C43
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

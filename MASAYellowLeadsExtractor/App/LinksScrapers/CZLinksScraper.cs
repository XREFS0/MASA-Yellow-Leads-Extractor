using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000018 RID: 24
	public class CZLinksScraper
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x0000D140 File Offset: 0x0000B340
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
				foreach (Element HItem in WebScraper.GetElements(WB, "li", "results-list__item", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "t-fpbc", true);
					if (Title.Count > 0)
					{
						try
						{
							DataItem.BusinessName = (Title[0].innerText ?? "").Trim();
							DataItem.DetailsLink = (Title[0]["href"] ?? "").ToString();
							if (!string.IsNullOrEmpty(DataItem.DetailsLink) && DataItem.DetailsLink.StartsWith("/"))
							{
								DataItem.DetailsLink = new Uri(new Uri(WB.Url), DataItem.DetailsLink).ToString();
							}
						}
						catch
						{
						}
					}
					List<Element> AddrLi = WebScraper.GetElementsByAttribute(WB, HItem, "li", "itemprop", "address", true);
					if (AddrLi.Count == 0)
					{
						AddrLi = WebScraper.GetElements(WB, HItem, "li", "poi", false);
					}
					if (AddrLi.Count > 0)
					{
						string street = "";
						string zip = "";
						string city = "";
						List<Element> mStreet = WebScraper.GetElementsByAttribute(WB, AddrLi[0], "meta", "itemprop", "streetAddress", true);
						if (mStreet.Count > 0)
						{
							try
							{
								street = (mStreet[0]["content"] ?? "").ToString().Trim();
							}
							catch
							{
							}
						}
						List<Element> mCity = WebScraper.GetElementsByAttribute(WB, AddrLi[0], "meta", "itemprop", "addressLocality", true);
						if (mCity.Count > 0)
						{
							try
							{
								city = (mCity[0]["content"] ?? "").ToString().Trim();
							}
							catch
							{
							}
						}
						List<Element> mZip = WebScraper.GetElementsByAttribute(WB, AddrLi[0], "meta", "itemprop", "zip", true);
						if (mZip.Count > 0)
						{
							try
							{
								zip = (mZip[0]["content"] ?? "").ToString().Trim();
							}
							catch
							{
							}
						}
						if (string.IsNullOrEmpty(street) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(zip))
						{
							List<Element> spanAddr = WebScraper.GetElements(WB, AddrLi[0], "span", "", false);
							if (spanAddr.Count > 0)
							{
								string input = (spanAddr[0].innerText ?? "").Trim();
								Match i = Regex.Match(input, "^\\s*(.+?),\\s*(\\d{5})\\s+(.+?)\\s*$");
								if (i.Success)
								{
									if (string.IsNullOrEmpty(street))
									{
										street = i.Groups[1].Value.Trim();
									}
									if (string.IsNullOrEmpty(zip))
									{
										zip = i.Groups[2].Value.Trim();
									}
									if (string.IsNullOrEmpty(city))
									{
										city = i.Groups[3].Value.Trim();
									}
								}
								else if (string.IsNullOrEmpty(DataItem.Address))
								{
									DataItem.Address = input;
								}
							}
						}
						if (!string.IsNullOrEmpty(street) || !string.IsNullOrEmpty(zip) || !string.IsNullOrEmpty(city))
						{
							DataItem.Address = street;
							DataItem.PostalCode = zip;
							DataItem.City = city;
						}
					}
					DataItem.Country = "Slovakia";
					List<Element> Cat = WebScraper.GetElements(WB, HItem, "ul", "categories", true);
					if (Cat.Count > 0)
					{
						try
						{
							List<Element> aCat = WebScraper.GetElements(WB, Cat[0], "a", "", false);
							if (aCat.Count > 0)
							{
								DataItem.Category = (aCat[0].innerText ?? "").Trim();
							}
							else
							{
								DataItem.Category = (Cat[0].innerText ?? "").Split(new char[] { ',' })[0].Trim();
							}
						}
						catch
						{
						}
					}
					List<Element> LinkLi = WebScraper.GetElements(WB, HItem, "li", "link", true);
					if (LinkLi.Count > 0)
					{
						try
						{
							List<Element> a = WebScraper.GetElements(WB, LinkLi[0], "a", "", false);
							if (a.Count > 0)
							{
								DataItem.Website = (a[0]["href"] ?? "").ToString().Trim();
							}
							else
							{
								DataItem.Website = (LinkLi[0].innerText ?? "").Trim();
							}
						}
						catch
						{
						}
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "span", "phone-value", true);
					if (Phone.Count > 0)
					{
						try
						{
							DataItem.Phone = (Phone[0].innerText ?? "").Trim();
						}
						catch
						{
						}
					}
					List<Element> Mail = WebScraper.GetElements(WB, HItem, "li", "mail", true);
					if (Mail.Count > 0)
					{
						try
						{
							List<Element> a2 = WebScraper.GetElements(WB, Mail[0], "a", "", false);
							if (a2.Count > 0)
							{
								string href = (a2[0]["href"] ?? "").ToString();
								if (!string.IsNullOrEmpty(href) && href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
								{
									DataItem.Email = href.Substring("mailto:".Length).Trim();
								}
								else
								{
									DataItem.Email = (a2[0].innerText ?? "").Trim();
								}
							}
							else
							{
								DataItem.Email = (Mail[0].innerText ?? "").Trim();
							}
						}
						catch
						{
						}
					}
					if (!string.IsNullOrEmpty(DataItem.DetailsLink))
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, "Slovakia", DataItem.Phone, DataItem.Fax, DataItem.Website,
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
				foreach (Element HItem2 in WebScraper.GetElements(WB, "li", "list-listing", true))
				{
					DataItem DataItem2 = new DataItem();
					List<Element> Title2 = WebScraper.GetElementsByTag(WB, HItem2, "h3");
					if (Title2.Count > 0)
					{
						List<Element> Children = WebScraper.GetChildren(WB, Title2[0]);
						DataItem2.BusinessName = Title2[0].innerText;
						DataItem2.DetailsLink = Children[0]["href"].ToString();
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem2, "ul", "icon-list", false);
					if (Address.Count > 0)
					{
						string html = Address[0].innerHTML;
						List<string[]> AItems = HTTPScraper.ParseHTML(html, "<li><i class=\"fa fa-map-marker\"></i>(.*?)</li>");
						if (AItems.Count > 0)
						{
							DataItem2.Address = AItems[0][1];
							DataItem2.City = AItems[0][1].Split(new char[] { ',' }).Last<string>();
						}
						List<string[]> AItems2 = HTTPScraper.ParseHTML(html, "<a href=\"(.*?)\">(.*?)</a>");
						if (AItems2.Count > 0)
						{
							DataItem2.Category = AItems2[0][1];
						}
					}
					DataItem2.Country = "Czech Republic";
					List<Element> Website = WebScraper.GetElements(WB, HItem2, "a", "btn btn-primary btn-outline t-c", true);
					if (Website.Count > 0)
					{
						DataItem2.Website = Website[0]["href"].ToString();
					}
					List<Element> Phone2 = WebScraper.GetElements(WB, HItem2, "button", "btn btn-success btn-outline", true);
					if (Phone2.Count > 0)
					{
						DataItem2.Phone = Phone2[0].innerText;
					}
					if (DataItem2.DetailsLink != null && DataItem2.DetailsLink != "")
					{
						PageItems.Add(DataItem2);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem2.Category, DataItem2.BusinessName, DataItem2.Address, DataItem2.City, DataItem2.State, DataItem2.PostalCode, "Czech Republic", DataItem2.Phone, DataItem2.Fax, DataItem2.Website,
							DataItem2.Email, DataItem2.MapLink, DataItem2.DetailsLink
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
				if (OldUrl.Contains(".cz"))
				{
					CZLinksScraper.GetData1(ref PageItems, WB, mainForm);
					WB.LoadUrlAndWait(OldUrl);
				}
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "ul", "pagination", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					List<string[]> AddressParts2 = HTTPScraper.ParseHTML(NextButton.innerHTML, "<li class=\"active\">(.*?)<li><a href=\"(.*?)\">»</a>");
					if (AddressParts2.Count > 0)
					{
						WB.Url.ToString();
						string NewUrl = AddressParts2[0][2].Split(new char[] { '"' }).Last<string>();
						WB.LoadUrlAndWait("https://www.zlatestranky.cz" + NewUrl);
						Thread.Sleep(500);
					}
					else
					{
						NextButton = null;
					}
				}
				else
				{
					List<Element> NextButtons2 = WebScraper.GetElements(WB, "ul", "paginate ptb30 divider", true);
					if (NextButtons2.Count > 0)
					{
						NextButton = NextButtons2[0];
						List<string[]> AddressParts3 = HTTPScraper.ParseHTML(NextButton.innerHTML, "<li><a href=\"(.*?)\">Dalšie</a>");
						if (AddressParts3.Count > 0)
						{
							WB.Url.ToString();
							string NewUrl2 = AddressParts3[0][1].Split(new char[] { '"' }).Last<string>();
							WB.LoadUrlAndWait("https://www.zlatestranky.sk" + NewUrl2);
							Thread.Sleep(500);
						}
						else
						{
							NextButton = null;
						}
					}
					else
					{
						NextButton = null;
					}
				}
				if (Program.IsStopped() || NextButton == null)
				{
					goto IL_0BB4;
				}
			}

			IL_0BB4:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000DE14 File Offset: 0x0000C014
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(500);
				string html = WB.GetHtml();
				Thread.Sleep(200);
				List<string[]> Email = HTTPScraper.ParseHTML(html, "href=\"mailto:(.*?)\" class=\"t-c\"");
				if (Email.Count > 0)
				{
					DataItems[i].Email = Email[0][1];
					CZLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Addr = HTTPScraper.ParseHTML(html, "<meta itemprop=\"addressLocality\" content=\"(.*?)\">");
				if (Addr.Count > 0)
				{
					DataItems[i].City = Addr[0][1];
					CZLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Addr2 = HTTPScraper.ParseHTML(html, "<meta itemprop=\"postalCode\" content=\"(.*?)\">");
				if (Addr2.Count > 0)
				{
					DataItems[i].PostalCode = Addr2[0][1];
					CZLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000DF28 File Offset: 0x0000C128
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[3].Value = CZLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[5].Value = CZLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[10].Value = CZLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00002A43 File Offset: 0x00000C43
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000038 RID: 56
	public class PagesjaunesLinksScraper
	{
		// Token: 0x06000195 RID: 405 RVA: 0x0001DA58 File Offset: 0x0001BC58
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				if (WB.IsReady)
				{
					List<DataItem> PageItems = new List<DataItem>();
					string[] array = new string[] { "bi bi-" };
					for (int i = 0; i < 1; i++)
					{
						foreach (Element HItem in WebScraper.GetElements(WB, "li", array[i], false))
						{
							DataItem DataItem = new DataItem();
							List<Element> Link = WebScraper.GetElements(WB, HItem, "div", "bi-clic-mobile", false);
							if (Link.Count > 0)
							{
								List<string[]> itemsA = HTTPScraper.ParseHTML(Link[0].innerHTML, "data-pjlb=\"{(.*?),(.*?)}\" data-pjstats=");
								if (itemsA.Count > 0)
								{
									byte[] data = Convert.FromBase64String(itemsA[0][1].Replace("url&quot;:", "").Replace("ucod&quot;:", "").Replace("&quot;", "")
										.Replace("b64u8", ""));
									string linkA = Encoding.UTF8.GetString(data);
									DataItem.DetailsLink = "https://www.pagesjaunes.fr" + linkA.Replace("carte", "pros/detail").Replace("/detail", "").Replace("?code_etablissement=", "/")
										.Replace("pros/", "pros/detail?code_etablissement=")
										.Replace("?code_localite", "&code_localite")
										.Replace("#zoneHoraires", "")
										.Replace("?code_rubrique", "&code_rubrique");
								}
							}
							List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "bi-denomination pj-", false);
							if (Title.Count > 0 && Title[0].innerText != null)
							{
								try
								{
									DataItem.BusinessName = Title[0].innerText.Trim();
								}
								catch
								{
								}
							}
							List<Element> Addresses = WebScraper.GetElements(WB, HItem, "div", "bi-address small", true);
							try
							{
								DataItem.Address = Addresses[0].innerText.Replace(" Voir le plan", "").Trim();
								List<string[]> Cit = HTTPScraper.ParseHTML(DataItem.Address, "(.*?)(\\d{5})\\s(.*)");
								if (Cit.Count > 0)
								{
									DataItem.PostalCode = Cit[0][2];
									DataItem.City = Cit[0][3];
								}
							}
							catch
							{
							}
							DataItem.Country = "France";
							List<Element> Category = WebScraper.GetElements(WB, HItem, "span", "bi-activity-unit small", false);
							if (Category.Count > 0 && Category[0].innerText != null)
							{
								DataItem.Category = Category[0].innerText.Trim();
							}
							else
							{
								DataItem.Category = "";
							}
							List<Element> Phone = WebScraper.GetElements(WB, HItem, "div", "number-contact", false);
							if (Phone.Count > 0 && Phone[0].innerText != null)
							{
								DataItem.Phone = Phone[0].innerText.Replace(" :", ":").Replace("                            ", " ");
							}
							else
							{
								DataItem.Phone = "";
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
						string OldUrl = WB.Url.ToString();
						if (Program.AppSettings.ExtractEmails)
						{
							PagesjaunesLinksScraper.GetData1(ref PageItems, WB, mainForm);
						}
						WB.LoadUrlAndWait(OldUrl);
						mainForm.tsProgress.Value = 0;
}
					List<Element> NextButtons = WebScraper.GetElements(WB, "div", "pagination", true);
					Element NextButton;
					if (NextButtons.Count > 0 && NextButtons[0].innerHTML.IndexOf("disabled next") == -1)
					{
						NextButton = NextButtons[0];
						List<string[]> itemsB = HTTPScraper.ParseHTML(NextButton.innerHTML, "href=\"#\" data-pjlb=\"{(.*?),(.*?)}\" data-pjstats=");
						if (itemsB.Count > 0)
						{
							string OldUrl2 = WB.Url.ToString();
							byte[] data2 = Convert.FromBase64String(itemsB[0][1].Replace("url&quot;:", "").Replace("ucod&quot;:", "").Replace("&quot;", "")
								.Replace("b64u8", ""));
							string NewUrl = Encoding.UTF8.GetString(data2);
							WB.LoadUrlAndWait("https://www.pagesjaunes.fr" + NewUrl.Replace("carte", "pros/detail").Replace("/detail", "").Replace("?code_etablissement=", "/")
								.Replace("&code_localite", "?code_localite")
								.Replace("pros/", "pros/detail?code_etablissement="));
							while (WB.Url.ToString() == OldUrl2)
							{
								Thread.Sleep(1000);
								Application.DoEvents();
							}
						}
						else
						{
							List<string[]> list = HTTPScraper.ParseHTML(NextButton.innerHTML, "title=\"Page suivante\" href=\"(.*?)\" data-pjstats=");
							string OldUrl3 = WB.Url.ToString();
							string NewUrl2 = list[0][1];
							WB.LoadUrlAndWait("https://www.pagesjaunes.fr" + NewUrl2);
							while (WB.Url.ToString() == OldUrl3)
							{
								Thread.Sleep(1000);
								Application.DoEvents();
							}
						}
					}
					else
					{
						NextButton = null;
					}
					if (NextButton == null)
					{
						goto Block_12;
					}
				}
				else
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
			}
			Block_5:

			Block_12:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0001E14C File Offset: 0x0001C34C
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				Program.RequestDelay();
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(new Random().Next(1900, 4201));
				string HTML = WB.GetHtml();
				List<string[]> Tel = HTTPScraper.ParseHTML(HTML, "<span class=\"coord-numero noTrad\">(.*?)</span>");
				if (Tel.Count > 0)
				{
					DataItems[i].Phone = Tel[0][1];
					PagesjaunesLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Website = HTTPScraper.ParseHTML(HTML, "<span class=\"icon icon-lien\"></span><span class=\"value\">(.*?)</span>");
				if (Website.Count > 0)
				{
					DataItems[i].Website = "http://www." + Website[0][1].Replace("www.", "");
					DataItems[i].Email = EmailMiner.GetEmail(DataItems[i].Website, new string[] { "conta" });
					PagesjaunesLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> City = HTTPScraper.ParseHTML(HTML, "<span class=\"noTrad\">(.*?)\\s(\\d{5})\\s(.*)</a>");
				if (City.Count > 0)
				{
					DataItems[i].City = City[0][3].Replace("<span class=noTrad\">", "").Replace("<span class=\"noTrad\">", "").Replace("<span>", "")
						.Replace("</span>", "");
					DataItems[i].PostalCode = City[0][2].Replace("<span class=noTrad\">", "").Replace("<span class=\"noTrad\">", "").Replace("<span>", "")
						.Replace("</span>", "");
					PagesjaunesLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0001E348 File Offset: 0x0001C548
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[4].Value = HTTPScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[6].Value = HTTPScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = HTTPScraper.ClearValue(Item.Phone);
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

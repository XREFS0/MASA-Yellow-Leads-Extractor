using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000034 RID: 52
	public class YellLinksScraper
	{
		// Token: 0x06000170 RID: 368 RVA: 0x0001B198 File Offset: 0x00019398
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
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "col-sm-15 col-md-14 col-lg-15 businessCapsule--mainContent", false))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "businessCapsule--title", true);
					if (Title.Count > 0)
					{
						try
						{
							DataItem.DetailsLink = Title[0]["href"].ToString();
							DataItem.MapLink = Title[0]["href"].ToString() + "#view=map";
						}
						catch
						{
						}
					}
					List<Element> Title2 = WebScraper.GetElements(WB, HItem, "h2", "businessCapsule--name text-h2", true);
					if (Title2.Count > 0)
					{
						DataItem.BusinessName = Title2[0].innerText;
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "a", "col-sm-24 businessCapsule--address businessCapsule--link", true);
					if (Address.Count > 0)
					{
						string outerHTML = Address[0].outerHTML;
						List<string[]> AItems = HTTPScraper.ParseHTML(outerHTML, "<span itemprop=\"streetAddress\">(.*?)</span>");
						if (AItems.Count > 0)
						{
							DataItem.Address = AItems[0][1];
						}
						AItems = HTTPScraper.ParseHTML(outerHTML, "<span itemprop=\"addressLocality\">(.*?)</span>");
						if (AItems.Count > 0)
						{
							DataItem.City = AItems[0][1];
						}
						AItems = HTTPScraper.ParseHTML(outerHTML, "<span itemprop=\"postalCode\">(.*?)</span>");
						if (AItems.Count > 0)
						{
							DataItem.PostalCode = AItems[0][1];
						}
						DataItem.Country = "United Kingdom";
					}
					else
					{
						Address = WebScraper.GetElements(WB, HItem, "span", "col-sm-24 businessCapsule--address", true);
						if (Address.Count > 0)
						{
							DataItem.Address = Address[0].innerText;
						}
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "span", "businessCapsule--classification", true);
					if (Categories.Count > 0)
					{
						DataItem.Category = Categories[0].innerText;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "div", "icon icon-Business-website", true);
					if (Website.Count > 0)
					{
						DataItem.Website = WebScraper.GetParent(WB, Website[0])["href"].ToString();
					}
					DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "contact" });
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "span", "business--telephoneNumber", true);
					if (Phone.Count > 0)
					{
						DataItem.Phone = Phone[0].innerHTML;
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
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "btn btn-blue btn-fullWidth pagination--next", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					string OldUrl = WB.Url.ToString();
					WB.LoadUrlAndWait(NextButton["href"].ToString());
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
				if (Program.IsStopped() || NextButton == null)
				{
					goto IL_04A5;
				}
			}

			IL_04A5:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}
	}
}

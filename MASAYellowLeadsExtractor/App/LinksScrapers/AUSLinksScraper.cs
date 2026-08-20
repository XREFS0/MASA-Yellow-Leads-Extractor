using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200002E RID: 46
	public class AUSLinksScraper
	{
		// Token: 0x06000156 RID: 342 RVA: 0x000184F8 File Offset: 0x000166F8
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
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "Box__Div-sc-dws99b-0 iOfhmk MuiPaper-root MuiCard-root FreeListing MuiPaper-elevation1 MuiPaper-rounded", true))
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "MuiTypography-root MuiLink-root MuiLink-underlineNone MuiTypography-colorPrimary", true);
					if (Title.Count > 0)
					{
						DataItem.BusinessName = Title[0].innerText;
						DataItem.DetailsLink = "https://www.yellowpages.com.au/";
					}
					List<Element> Clicks = WebScraper.GetElements(WB, HItem, "div", "Box__Div-sc-dws99b-0 LCzJY", true);
					if (Clicks.Count > 0)
					{
						Element Click = WebScraper.GetChildren(WB, Clicks[0])[0];
						WebScraper.InvokeMember(WB, Click, "click");
						Thread.Sleep(200);
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "Box__Div-sc-dws99b-0 bvRSwt", true);
					if (Address.Count > 0)
					{
						try
						{
							DataItem.Address = Address[0].innerText.Split(new char[] { '|' })[0];
						}
						catch
						{
						}
					}
					DataItem.Country = "Australia";
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "div", "Box__Div-sc-dws99b-0 bKFqNV", true);
					if (Categories.Count > 0)
					{
						try
						{
							List<string[]> AItems = HTTPScraper.ParseHTML(Categories[0].innerText, "(.*?),(.*?),(.*)");
							DataItem.Category = AItems[0][1];
							DataItem.City = AItems[0][2];
						}
						catch
						{
						}
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "ButtonWebsite", false);
					if (Website.Count > 0)
					{
						DataItem.Website = Website[0]["href"].ToString();
					}
					if (Program.AppSettings.ExtractEmails)
					{
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "contact" });
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "MuiButtonBase-root MuiButton-root MuiButton-text ButtonPhone wobble-call MuiButton-textSecondary MuiButton-fullWidth", true);
					if (Phone.Count > 0)
					{
						DataItem.Phone = Phone[0]["href"].ToString();
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
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, "Australia", DataItem.Phone, DataItem.Fax, DataItem.Website,
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
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "MuiButtonBase-root MuiButton-root MuiButton-outlined MuiButton-fullWidth", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = null;
					if (NextButtons[0].outerHTML.IndexOf("Next") > -1)
					{
						NextButton = NextButtons[0];
						string NextUrl = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl);
					}
					if (NextButtons.Count > 1 && NextButtons[1].outerHTML.IndexOf("Next") > -1)
					{
						NextButton = NextButtons[1];
						string NextUrl2 = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl2);
					}
					Thread.Sleep(1000);
				}
				else
				{
					NextButton = null;
				}
				if (Program.IsStopped() || NextButton == null)
				{
					goto IL_046E;
				}
			}

			IL_046E:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}
	}
}

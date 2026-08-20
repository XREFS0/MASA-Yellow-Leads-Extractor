using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200003C RID: 60
	public class YellowPagesLinksScraper
	{
		// Token: 0x060001AA RID: 426 RVA: 0x000201D8 File Offset: 0x0001E3D8
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "result", true))
				{
					YellowPagesLinksScraper.WaitForBrowser(WB);
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "business-name", true);
					if (Title.Count > 0)
					{
						try
						{
							DataItem.BusinessName = Title[0].innerText;
							DataItem.DetailsLink = Title[0]["href"].ToString();
							DataItem.MapLink = DataItem.DetailsLink;
						}
						catch
						{
						}
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "street-address", true);
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerText.Replace(",", "");
					}
					else
					{
						DataItem.Address = "";
					}
					DataItem.Country = "USA";
					List<Element> City = WebScraper.GetElements(WB, HItem, "div", "locality", true);
					if (City.Count > 0)
					{
						try
						{
							DataItem.City = City[0].innerText.Split(new char[] { ',' })[0];
							string citcap = City[0].innerText.Split(new char[] { ',' }).Last<string>().ToString();
							DataItem.PostalCode = citcap.Split(new char[] { ' ' }).Last<string>().ToString();
							DataItem.State = citcap.Split(new char[] { ' ' })[1];
						}
						catch
						{
						}
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "div", "categories", true);
					if (Categories.Count > 0)
					{
						List<Element> Children = WebScraper.GetChildren(WB, Categories[0]);
						DataItem.Category = Children[0].innerText;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "track-visit-website", true);
					if (Website.Count > 0)
					{
						DataItem.Website = Website[0]["href"].ToString();
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "div", "phones phone primary", true);
					if (Phone.Count > 0)
					{
						DataItem.Phone = Phone[0].innerText;
					}
					else
					{
						Phone = WebScraper.GetElements(WB, HItem, "li", "phone primary", true);
						if (Phone.Count > 0)
						{
							DataItem.Phone = Phone[0].innerText;
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
				if (Program.AppSettings.ExtractEmails)
				{
					YellowPagesLinksScraper.GetData(ref PageItems, mainForm);
				}
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "next ajax-page", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					string OldUrl = WB.Url.ToString();
					string NewUrl = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NewUrl);
					while (WB.Url.ToString() == OldUrl || !WB.IsReady)
					{
						Thread.Sleep(1200);
						Application.DoEvents();
						Thread.Sleep(1200);
					}
					for (int i = 0; i < 1000; i++)
					{
						Application.DoEvents();
					}
				}
				else
				{
					NextButton = null;
				}
				if (Program.IsStopped() || NextButton == null)
				{
					goto IL_04CC;
				}
			}

			IL_04CC:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0002071C File Offset: 0x0001E91C
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			string HTML = "";
			for (int i = 0; i < DataItems.Count; i++)
			{
				Program.RequestDelay();
				try
				{
					Thread.Sleep(1000);
					WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				}
				catch
				{
				}
				HTML = WB.GetHtml();
				List<string[]> Website = HTTPScraper.ParseHTML(HTML, "href=\"http(.*?)\">www.");
				if (Website.Count > 0)
				{
					try
					{
						DataItems[i].Website = "http" + Website[0][1].Split(new char[] { '"' })[0];
						YellowPagesLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
					catch
					{
					}
				}
				List<string[]> Email = HTTPScraper.ParseHTML(HTML, "href=\"mailto:(.*?)\"");
				if (Email.Count > 0)
				{
					try
					{
						DataItems[i].Email = Email[0][1].Split(new char[] { '"' })[0];
						YellowPagesLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
					catch
					{
					}
				}
				List<string[]> Phone = HTTPScraper.ParseHTML(HTML, "href=\"tel:(.*?)\"");
				if (Phone.Count > 0)
				{
					try
					{
						DataItems[i].Phone = "Tel: " + Phone[0][1];
						YellowPagesLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
					catch
					{
					}
				}
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000208B8 File Offset: 0x0001EAB8
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			YellowPagesDataScraper[] Pool = new YellowPagesDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YellowPagesDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YellowPagesLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new YellowPagesDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				Thread.Sleep(500);
				Application.DoEvents();
				if (Program.IsStopped())
				{
					break;
				}
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000209CC File Offset: 0x0001EBCC
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[6].Value = YellowPagesLinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[10].Value = YellowPagesLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = YellowPagesLinksScraper.ClearValue(Item.MapLink);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000F83F File Offset: 0x0000DA3F
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1000)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00020AD2 File Offset: 0x0001ECD2
		private static void WaitForBrowser1(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1200)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00012512 File Offset: 0x00010712
		private static void WaitForBrowser2(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00002A43 File Offset: 0x00000C43
		private static string ClearValue(string Value)
		{
			if (Value != null)
			{
				return Value.Replace("&#x27", "");
			}
			return "";
		}

		// Token: 0x02000084 RID: 132
		public class AutoClosingMessageBox
		{
			// Token: 0x06000248 RID: 584 RVA: 0x000261C0 File Offset: 0x000243C0
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x06000249 RID: 585 RVA: 0x00026220 File Offset: 0x00024420
			public static void Show(string text, string caption, int timeout)
			{
				new YellowPagesLinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x0600024A RID: 586 RVA: 0x0002622C File Offset: 0x0002442C
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = YellowPagesLinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				if (mbWnd != IntPtr.Zero)
				{
					YellowPagesLinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x0600024B RID: 587
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x0600024C RID: 588
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x0400022B RID: 555
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x0400022C RID: 556
			private string _caption;

			// Token: 0x0400022D RID: 557
			private const int WM_CLOSE = 16;
		}
	}
}

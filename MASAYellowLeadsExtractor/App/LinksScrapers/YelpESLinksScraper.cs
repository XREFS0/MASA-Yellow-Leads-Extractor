using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000022 RID: 34
	public class YelpESLinksScraper
	{
		// Token: 0x0600010B RID: 267 RVA: 0x0001253C File Offset: 0x0001073C
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				Thread.Sleep(1000);
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "srp-listing", false))
				{
					YelpESLinksScraper.WaitForBrowser(WB);
					DataItem DataItem = new DataItem();
					List<Element> Title0 = WebScraper.GetElementsByTag(WB, HItem, "h2");
					if (Title0.Count > 0)
					{
						DataItem.BusinessName = Title0[0].innerText;
					}
					List<Element> Title = WebScraper.GetElementsByTag(WB, HItem, "a");
					if (Title.Count > 0)
					{
						DataItem.DetailsLink = Title[0]["href"].ToString();
					}
					else
					{
						DataItem.DetailsLink = "";
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "street-address", true);
					if (Address.Count > 0)
					{
						List<string[]> list = HTTPScraper.ParseHTML(Address[0].innerHTML ?? "", "<span>(.*?)</span>");
						List<string> chunks = new List<string>();
						foreach (string[] array in list)
						{
							string t = (array[1] ?? "").Trim();
							if (!string.IsNullOrEmpty(t))
							{
								t = t.Trim().Trim(new char[] { ',' }).Trim();
								if (!string.IsNullOrEmpty(t))
								{
									chunks.Add(t);
								}
							}
						}
						if (chunks.Count > 0)
						{
							DataItem.Address = chunks[0];
						}
						else
						{
							DataItem.Address = "";
						}
						if (chunks.Count > 1)
						{
							DataItem.City = chunks[1];
						}
						if (chunks.Count > 0)
						{
							string last = chunks[chunks.Count - 1];
							Match i = Regex.Match(last, "\\b(\\d{3,6})\\b");
							if (i.Success)
							{
								DataItem.PostalCode = i.Groups[1].Value;
							}
							if (string.IsNullOrWhiteSpace(DataItem.City))
							{
								string withoutZip = Regex.Replace(last, "\\b\\d{3,6}\\b", "").Trim().Trim(new char[] { ',' });
								if (!string.IsNullOrEmpty(withoutZip))
								{
									DataItem.City = withoutZip;
								}
							}
						}
						if (string.IsNullOrEmpty(DataItem.State))
						{
							DataItem.State = "";
						}
					}
					else
					{
						DataItem.Address = "";
					}
					DataItem.Category = "Yellow.co.nz";
					DataItem.Country = "New Zealand";
					List<Element> Web = WebScraper.GetElements(WB, HItem, "a", "weblink-button btn-secondary", true);
					if (Web.Count > 0)
					{
						DataItem.Website = Web[0]["href"].ToString();
					}
					if (Program.AppSettings.ExtractEmails)
					{
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "conta" });
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "phones phone", true);
					if (Phone.Count > 0)
					{
						DataItem.Phone = Phone[0]["href"].ToString();
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
				GC.Collect();
				Thread.Sleep(1000);
List<Element> NextButtons = WebScraper.GetElements(WB, "a", "next ajax-page", true);
				Element NextButton;
				if (NextButtons.Count > 0)
				{
					NextButton = NextButtons[0];
					string NextUrl = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NextUrl);
					Thread.Sleep(3000);
					for (int j = 0; j < 1000; j++)
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
					goto IL_0519;
				}
			}

			IL_0519:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00012AB4 File Offset: 0x00010CB4
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				Program.RequestDelay();
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(10000);
				string html = WB.GetHtml();
				Thread.Sleep(500);
				List<string[]> Address = HTTPScraper.ParseHTML(html, "<span itemprop=\"streetAddress\">(.*?)</span>");
				if (Address.Count > 0)
				{
					DataItems[i].Address = Address[0][1].Replace("\"", "");
					YelpESLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> City = HTTPScraper.ParseHTML(html, "<span itemprop=\"addressLocality\" content=(.*?)>");
				if (City.Count > 0)
				{
					DataItems[i].City = City[0][1].Replace("\"", "");
					YelpESLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> State = HTTPScraper.ParseHTML(html, "<span itemprop=\"addressRegion\" content=(.*?)>");
				if (State.Count > 0)
				{
					DataItems[i].State = State[0][1].Replace("\"", "");
					YelpESLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Website = HTTPScraper.ParseHTML(html, "<meta itemprop=\"url\" content=(.*?)>");
				if (Website.Count > 0)
				{
					DataItems[i].Website = Website[0][1].Replace("\"", "");
					YelpESLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00012C4C File Offset: 0x00010E4C
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			YelpESDataScraper[] Pool = new YelpESDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YelpESDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YelpESLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new YelpESDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00012D58 File Offset: 0x00010F58
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[2].Value = YelpESLinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = YelpESLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = YelpESLinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = YelpESLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = YelpESLinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = YelpESLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[10].Value = YelpESLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = YelpESLinksScraper.ClearValue(Item.MapLink);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000F83F File Offset: 0x0000DA3F
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1000)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000124EB File Offset: 0x000106EB
		private static void WaitForBrowser1(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00012512 File Offset: 0x00010712
		private static void WaitForBrowser2(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00002A43 File Offset: 0x00000C43
		private static string ClearValue(string Value)
		{
			if (Value != null)
			{
				return Value.Replace("&#x27", "");
			}
			return "";
		}

		// Token: 0x0200007F RID: 127
		public class AutoClosingMessageBox
		{
			// Token: 0x0600022C RID: 556 RVA: 0x00025F2C File Offset: 0x0002412C
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x0600022D RID: 557 RVA: 0x00025F8C File Offset: 0x0002418C
			public static void Show(string text, string caption, int timeout)
			{
				new YelpESLinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x0600022E RID: 558 RVA: 0x00025F98 File Offset: 0x00024198
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = YelpESLinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				if (mbWnd != IntPtr.Zero)
				{
					YelpESLinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x0600022F RID: 559
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x06000230 RID: 560
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x0400021C RID: 540
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x0400021D RID: 541
			private string _caption;

			// Token: 0x0400021E RID: 542
			private const int WM_CLOSE = 16;
		}
	}
}

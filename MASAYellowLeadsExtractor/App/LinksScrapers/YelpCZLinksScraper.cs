using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000021 RID: 33
	public class YelpCZLinksScraper
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00011BD4 File Offset: 0x0000FDD4
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				Thread.Sleep(1000);
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "list-item-container", true))
				{
					YelpCZLinksScraper.WaitForBrowser(WB);
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "btn action-button-service-item", false);
					if (Title.Count > 0)
					{
						DataItem.DetailsLink = Title[0]["href"].ToString();
					}
					List<Element> Title2 = WebScraper.GetElements(WB, HItem, "h2");
					if (Title2.Count > 0)
					{
						string[] parts = Title2[0].innerText.Replace("&", "e").Split(new char[] { '|' });
						DataItem.BusinessName = ((parts.Length > 1) ? parts[1].Trim() : string.Empty);
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "list-item list-item--service-desktop--element", true);
					if (Address.Count > 0 && Address[1].innerText.IndexOf(",") > -1)
					{
						DataItem.Address = Address[1].innerText.Split(new char[] { '-' })[0];
						List<string[]> AddressParts = HTTPScraper.ParseHTML(Address[1].innerText, "(.*?)\\s-\\s(\\d+)\\s(.*)");
						try
						{
							if (AddressParts.Count > 0)
							{
								DataItem.PostalCode = AddressParts[0][2];
							}
						}
						catch
						{
						}
					}
					List<Element> Tel = WebScraper.GetElements(WB, HItem, "p", "list-item--service-desktop--numbers", true);
					if (Tel.Count > 0)
					{
						if (Tel[0].innerText.IndexOf(',') > -1)
						{
							try
							{
								if (Tel[0].innerText.Split(new char[] { ',' })[0].StartsWith("0"))
								{
									DataItem.Phone = Tel[0].innerText.Split(new char[] { ',' })[0];
								}
								else
								{
									DataItem.Fax = "Cell: " + Tel[0].innerText.Split(new char[] { ',' })[0];
								}
								if (Tel[0].innerText.Split(new char[] { ',' })[1].StartsWith("0"))
								{
									DataItem.Phone = Tel[0].innerText.Split(new char[] { ',' })[1];
								}
								else
								{
									DataItem.Fax = "Cell: " + Tel[0].innerText.Split(new char[] { ',' })[1];
								}
								goto IL_0312;
							}
							catch
							{
								goto IL_0312;
							}
						}
						if (Tel[0].innerText.StartsWith("0"))
						{
							DataItem.Phone = Tel[0].innerText;
						}
						else
						{
							DataItem.Fax = "Cell: " + Tel[0].innerText;
						}
					}
					IL_0312:
					if (DataItem.DetailsLink != null && DataItem.DetailsLink != "")
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.Country, DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
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
				YelpCZLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "button", "border-0 btn back-and-foward-button", true);
				Element NextButton;
				if (NextButtons.Count > 0 && NextButtons[0].innerText.IndexOf("f") > -1)
				{
					NextButton = NextButtons[0];
					WB.Url.ToString();
					WebScraper.InvokeMember(WB, NextButton, "click");
					Thread.Sleep(1100);
					for (int i = 0; i < 1000; i++)
					{
						Application.DoEvents();
					}
				}
				else if (NextButtons.Count > 0 && NextButtons[1].innerText.IndexOf("f") > -1)
				{
					NextButton = NextButtons[1];
					WB.Url.ToString();
					WebScraper.InvokeMember(WB, NextButton, "click");
					Thread.Sleep(1100);
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
					goto IL_057D;
				}
			}

			IL_057D:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000121C8 File Offset: 0x000103C8
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			YelpCZDataScraper[] Pool = new YelpCZDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YelpCZDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YelpCZLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new YelpCZDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000105 RID: 261 RVA: 0x000122DC File Offset: 0x000104DC
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = HTTPScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[3].Value = HTTPScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = HTTPScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = HTTPScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = HTTPScraper.ClearValue(Item.Country);
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

		// Token: 0x06000106 RID: 262 RVA: 0x0000F83F File Offset: 0x0000DA3F
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1000)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000124EB File Offset: 0x000106EB
		private static void WaitForBrowser1(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00012512 File Offset: 0x00010712
		private static void WaitForBrowser2(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00002A43 File Offset: 0x00000C43
		private static string ClearValue(string Value)
		{
			if (Value != null)
			{
				return Value.Replace("&#x27", "");
			}
			return "";
		}

		// Token: 0x0200007E RID: 126
		public class AutoClosingMessageBox
		{
			// Token: 0x06000227 RID: 551 RVA: 0x00025E74 File Offset: 0x00024074
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x06000228 RID: 552 RVA: 0x00025ED4 File Offset: 0x000240D4
			public static void Show(string text, string caption, int timeout)
			{
				new YelpCZLinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x06000229 RID: 553 RVA: 0x00025EE0 File Offset: 0x000240E0
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = YelpCZLinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				if (mbWnd != IntPtr.Zero)
				{
					YelpCZLinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x0600022A RID: 554
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x0600022B RID: 555
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x04000219 RID: 537
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x0400021A RID: 538
			private string _caption;

			// Token: 0x0400021B RID: 539
			private const int WM_CLOSE = 16;
		}
	}
}

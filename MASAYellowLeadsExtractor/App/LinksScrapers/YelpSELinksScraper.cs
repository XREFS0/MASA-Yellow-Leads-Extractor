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
	// Token: 0x02000025 RID: 37
	public class YelpSELinksScraper
	{
		// Token: 0x0600011F RID: 287 RVA: 0x0001400C File Offset: 0x0001220C
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				Thread.Sleep(1000);
				foreach (Element HItem in WebScraper.GetElements(WB, "div", "relative", true))
				{
					YelpSELinksScraper.WaitForBrowser(WB);
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "h2", "text-4xl", false);
					if (Title.Count > 0)
					{
						DataItem.BusinessName = Title[0].innerText;
						DataItem.DetailsLink = WebScraper.GetParent(WB, Title[0])["href"].ToString();
					}
					else
					{
						DataItem.DetailsLink = "";
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "flex flex-wrap ", false);
					if (Address.Count > 0)
					{
						DataItem.Address = Address[0].innerText;
					}
					else
					{
						DataItem.Address = "";
					}
					List<Element> Fax = WebScraper.GetElements(WB, HItem, "span", "cldt-price sc-font-xl sc-font-bold", true);
					if (Fax.Count > 0)
					{
						DataItem.Fax = Fax[0].innerText.Split(new char[] { ',' })[0];
					}
					if (!string.IsNullOrEmpty(DataItem.DetailsLink))
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, "", DataItem.State, "", "loading...", "", DataItem.Fax, DataItem.Website,
							"", DataItem.MapLink, DataItem.DetailsLink
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
					YelpSELinksScraper.GetData1(ref PageItems, WB, mainForm);
				}
				WB.LoadUrlAndWait(OldUrl);
				mainForm.tsProgress.Value = 0;
List<Element> elements = WebScraper.GetElements(WB, "a", "inline-flex items-center rounded-full p-", false);
				NextButton = null;
				foreach (Element button in elements)
				{
					if (button.innerHTML.Contains("d=\"M9"))
					{
						NextButton = button;
						break;
					}
				}
				if (NextButton != null)
				{
					string OldUrl2 = WB.Url.ToString();
					string NewUrl = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NewUrl);
					Thread.Sleep(1000);
					Application.DoEvents();
					while (WB.Url.ToString() == OldUrl2)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
					}
				}
				if (NextButton == null)
				{
					goto Block_7;
				}
			}

			Block_7:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000143D4 File Offset: 0x000125D4
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				Program.RequestDelay();
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(800);
				string html = WB.GetHtml();
				Thread.Sleep(1000);
				List<string[]> Cat = HTTPScraper.ParseHTML(html, "hover:bg-secondary-500 text-white\">(.*?)</span>");
				if (Cat.Count > 0)
				{
					DataItems[i].Category = Cat[0][1].Replace("[", "").Replace("\"", "");
					YelpSELinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Email = HTTPScraper.ParseHTML(html, ",\"email\":\"(.*?)\",");
				if (Email.Count > 0)
				{
					string extractedEmail = Email[0][1];
					DataItems[i].Email = extractedEmail;
					YelpSELinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Website = HTTPScraper.ParseHTML(html, "\\\\\"url\\\\\":\\\\\"(.*?)\\\\\"}");
				if (Website.Count > 0)
				{
					string selectedWebsite = Website[0][1];
					DataItems[i].Website = selectedWebsite;
					YelpSELinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> City = HTTPScraper.ParseHTML(html, ",\"addressLocality\":\"(.*?)\",");
				if (City.Count > 0)
				{
					DataItems[i].City = City[0][1].Replace("\"", "");
					YelpSELinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Naz = HTTPScraper.ParseHTML(html, ",\"addressCountry\":\"(.*?)\"},");
				if (Naz.Count > 0)
				{
					DataItems[i].Country = Naz[0][1].Replace("\"", "");
					YelpSELinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> CAP = HTTPScraper.ParseHTML(html, ",\"postalCode\":\"(.*?)\",");
				if (CAP.Count > 0)
				{
					DataItems[i].PostalCode = CAP[0][1].Replace("\"", "");
					YelpSELinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> Tel = HTTPScraper.ParseHTML(html, ",\"telephone\":\"(.*?)\",");
				if (Tel.Count > 0)
				{
					DataItems[i].Phone = Tel[0][1].Replace("\"", "").Replace("[", " ").Replace("]", "");
					YelpSELinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				List<string[]> State = HTTPScraper.ParseHTML(html, ",\"region\":\"(.*?)\",");
				try
				{
					if (State.Count > 0)
					{
						DataItems[i].State = State[0][1];
						YelpSELinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
				}
				catch
				{
				}
				if (Program.IsStopped())
				{
					return;
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000146C8 File Offset: 0x000128C8
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 3;
			}
			YelpSEDataScraper[] Pool = new YelpSEDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YelpSEDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YelpSELinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new YelpSEDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000122 RID: 290 RVA: 0x000147D4 File Offset: 0x000129D4
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = YelpSELinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[3].Value = YelpSELinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = YelpSELinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = YelpSELinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = YelpSELinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = YelpSELinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[9].Value = YelpSELinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = YelpSELinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = YelpSELinksScraper.ClearValue(Item.MapLink);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000F83F File Offset: 0x0000DA3F
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1000)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000124EB File Offset: 0x000106EB
		private static void WaitForBrowser1(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00012512 File Offset: 0x00010712
		private static void WaitForBrowser2(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00002A43 File Offset: 0x00000C43
		private static string ClearValue(string Value)
		{
			if (Value != null)
			{
				return Value.Replace("&#x27", "");
			}
			return "";
		}

		// Token: 0x02000080 RID: 128
		public class AutoClosingMessageBox
		{
			// Token: 0x06000231 RID: 561 RVA: 0x00025FE4 File Offset: 0x000241E4
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x06000232 RID: 562 RVA: 0x00026044 File Offset: 0x00024244
			public static void Show(string text, string caption, int timeout)
			{
				new YelpSELinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x06000233 RID: 563 RVA: 0x00026050 File Offset: 0x00024250
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = YelpSELinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				if (mbWnd != IntPtr.Zero)
				{
					YelpSELinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x06000234 RID: 564
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x06000235 RID: 565
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x0400021F RID: 543
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x04000220 RID: 544
			private string _caption;

			// Token: 0x04000221 RID: 545
			private const int WM_CLOSE = 16;
		}
	}
}

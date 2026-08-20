using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EO.Base;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using Newtonsoft.Json.Linq;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000037 RID: 55
	public class LocalLinksScraper
	{
		// Token: 0x0600018F RID: 399 RVA: 0x0001D234 File Offset: 0x0001B434
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			EO.Base.Runtime.EnableEOWP = true;
			Element nextBtn = null;
			Thread.Sleep(2000);
			for (;;)
			{
				if (WB.IsReady)
				{
					string json = WB.EvalScript(LocalLinksScraper.JsExtractItems) as string;
					if (string.IsNullOrEmpty(json))
					{
						json = "[]";
					}
					JArray jarray = JArray.Parse(json);
					List<DataItem> pageItems = new List<DataItem>();
					foreach (JToken it in jarray)
					{
						DataItem di = new DataItem();
						di.BusinessName = ((string)it["title"]) ?? "";
						di.DetailsLink = ((string)it["href"]) ?? "";
						di.Address = ((string)it["address"]) ?? "";
						di.Category = ((string)it["category"]) ?? "";
						di.Country = "Switzerland";
						Match i = Regex.Match(di.Address, "^(.*?),\\s*(\\d{4})\\s*(.+)$");
						if (i.Success)
						{
							di.PostalCode = i.Groups[2].Value.Trim();
							di.City = i.Groups[3].Value.Trim();
						}
						if (!string.IsNullOrWhiteSpace(di.DetailsLink))
						{
							pageItems.Add(di);
							mainForm.dgvResults.Rows.Add(new object[]
							{
								di.Category, di.BusinessName, di.Address, di.City, di.State, di.PostalCode, di.Country, di.Phone, di.Fax, di.Website,
								di.Email, di.MapLink, di.DetailsLink
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
					if (Program.AppSettings.ExtractEmails && pageItems.Count > 0)
					{
						LocalLinksScraper.GetData1(ref pageItems, WB, mainForm);
					}
					WB.LoadUrlAndWait(OldUrl);
					mainForm.tsProgress.Value = 0;
List<Element> NextButtons = WebScraper.GetElements(WB, "button", "l:inline-flex l:items-center l:justify-center l:transition-colors l:duration-200 l:cursor-pointer", false);
					if (NextButtons != null && NextButtons.Count > 0)
					{
						Thread.Sleep(1000);
						foreach (Element button in NextButtons)
						{
							if (button != null)
							{
								button.innerHTML.Contains("disabled");
								bool isDisabled = true;
								string cls = (button.className ?? "").ToString();
								if (cls.Contains("cursor-not-allowed") || cls.Contains("opacity-50"))
								{
									isDisabled = true;
								}
								if (!isDisabled)
								{
									string txt = (button.innerText ?? "").Trim();
									if (txt == "Weiter" || txt == "Next" || txt == "Avanti" || txt == "Suivant")
									{
										nextBtn = button;
										break;
									}
								}
							}
						}
						if (nextBtn != null)
						{
							WebScraper.InvokeMember(WB, nextBtn, "click");
							Thread.Sleep(2500);
						}
					}
					else
					{
						nextBtn = null;
					}
					if (nextBtn == null)
					{
						goto Block_12;
					}
				}
				else
				{
					Application.DoEvents();
				}
			}

			Block_12:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0001D6A4 File Offset: 0x0001B8A4
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
						LocalLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
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
						LocalLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
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
						LocalLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
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

		// Token: 0x06000191 RID: 401 RVA: 0x0001D840 File Offset: 0x0001BA40
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			PagesjaunesDataScraper[] Pool = new PagesjaunesDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PagesjaunesDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						LocalLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new PagesjaunesDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				Application.DoEvents();
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0001D944 File Offset: 0x0001BB44
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[7].Value = HTTPScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[9].Value = HTTPScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = HTTPScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x040000DE RID: 222
		private static readonly string JsExtractItems = "\r\n(function () {\r\n  const clean = s => (s || '').replace(/\\s+/g, ' ').trim();\r\n\r\n  // ? CORRETTO: l'ARTICLE contiene il link /d/\r\n  const articles = Array.from(document.querySelectorAll('article[data-id]'));\r\n\r\n  const items = articles.map(article => {\r\n    // link principale del dettaglio (quello /d/)\r\n    const link = article.querySelector('a[href*=\"/d/\"]');\r\n    const href = link ? new URL(link.getAttribute('href'), location.origin).href : '';\r\n\r\n    // titolo / address normalmente stanno dentro il link\r\n    const titleEl = article.querySelector('h2[data-testid=\"title\"], h2');\r\n    const title = clean(titleEl ? titleEl.textContent : '');\r\n\r\n    const addressEl = article.querySelector('address');\r\n    const address = clean(addressEl ? addressEl.textContent : '');\r\n\r\n    // categorie: su local.ch spesso è la section senza data-testid (es. class mE)\r\n    let category = '';\r\n    const categorySection =\r\n      article.querySelector('section:not([data-testid])') ||\r\n      article.querySelector('section.mE'); // fallback se cambiano attributi ma resta la classe\r\n    if (categorySection) {\r\n      category = clean(categorySection.textContent).replace(/\\s*•\\s*/g, ' • ');\r\n    }\r\n\r\n    // rating + recensioni:\r\n    // 1) caso vecchio: data-testid average-rating + counter-rating\r\n    let avgRating = '';\r\n    let reviews = '';\r\n    const avgEl = article.querySelector('[data-testid=\"average-rating\"]');\r\n    if (avgEl) {\r\n      const clone = avgEl.cloneNode(true);\r\n      const cnt = clone.querySelector('[data-testid=\"counter-rating\"]');\r\n      if (cnt) cnt.remove();\r\n      avgRating = clean(clone.textContent).replace(/[()]/g, '');\r\n      const cnt2 = avgEl.querySelector('[data-testid=\"counter-rating\"]');\r\n      if (cnt2) reviews = clean(cnt2.textContent).replace(/[()]/g, '');\r\n    } else {\r\n      // 2) fallback nuovo: blocco testo (es. 'Nessuna recensione ancora' oppure '4.6 (23)')\r\n      const ratingTextEl = article.querySelector('.mr, [aria-label*=\"recension\"]');\r\n      const rt = clean(ratingTextEl ? ratingTextEl.textContent : '');\r\n\r\n      // esempi possibili:\r\n      // 'Nessuna recensione ancora'\r\n      // '4.6 (23)'\r\n      // '4,6 (23)' (locale)\r\n      if (/nessuna recensione/i.test(rt)) {\r\n        avgRating = '';\r\n        reviews = '';\r\n      } else {\r\n        const m = rt.match(/([0-9]+(?:[\\\\.,][0-9]+)?)\\\\s*(?:\\\\((\\\\d+)\\\\))?/);\r\n        if (m) {\r\n          avgRating = (m[1] || '').replace(',', '.');\r\n          reviews = (m[2] || '');\r\n        }\r\n      }\r\n    }\r\n\r\n    return { title, href, address, category, avgRating, reviews };\r\n  })\r\n  // opzionale: tieni solo elementi con href/titolo (evita roba extra tipo ads)\r\n  .filter(x => x.href && x.title);\r\n\r\n  return JSON.stringify(items);\r\n})();\r\n";

		// Token: 0x02000083 RID: 131
		public class AutoClosingMessageBox
		{
			// Token: 0x06000243 RID: 579 RVA: 0x00026108 File Offset: 0x00024308
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x06000244 RID: 580 RVA: 0x00026168 File Offset: 0x00024368
			public static void Show(string text, string caption, int timeout)
			{
				new LocalLinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x06000245 RID: 581 RVA: 0x00026174 File Offset: 0x00024374
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = LocalLinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				if (mbWnd != IntPtr.Zero)
				{
					LocalLinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x06000246 RID: 582
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x06000247 RID: 583
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x04000228 RID: 552
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x04000229 RID: 553
			private string _caption;

			// Token: 0x0400022A RID: 554
			private const int WM_CLOSE = 16;
		}
	}
}

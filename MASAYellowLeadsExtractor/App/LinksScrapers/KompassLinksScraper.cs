using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using Newtonsoft.Json;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000029 RID: 41
	public class KompassLinksScraper
	{
		// Token: 0x06000137 RID: 311 RVA: 0x00015D04 File Offset: 0x00013F04
		private static bool WaitForResults(WebView wb, int timeoutMs = 8000)
		{
			Stopwatch sw = Stopwatch.StartNew();
			while (sw.ElapsedMilliseconds < (long)timeoutMs)
			{
				if (Program.IsStopped())
				{
					return false;
				}
				if (wb.IsReady)
				{
					List<Element> els = WebScraper.GetElements(wb, "div", "y-css-8x4us", true);
					if (els != null && els.Count > 0)
					{
						return true;
					}
				}
				Application.DoEvents();
				Thread.Sleep(50);
			}
			return false;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00015D64 File Offset: 0x00013F64
		private static bool WaitForSelector(WebView wb, string cssSelector, int timeoutMs = 8000, int pollMs = 100)
		{
			Stopwatch sw = Stopwatch.StartNew();
			while (sw.ElapsedMilliseconds < (long)timeoutMs)
			{
				if (Program.IsStopped())
				{
					return false;
				}
				try
				{
					object countObj = wb.EvalScript("document.querySelectorAll(" + KompassLinksScraper.JsQuote(cssSelector) + ").length");
					int count = 0;
					if (countObj != null)
					{
						int.TryParse(countObj.ToString(), out count);
					}
					if (count > 0)
					{
						return true;
					}
				}
				catch
				{
				}
				Application.DoEvents();
				Thread.Sleep(pollMs);
			}
			return false;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00015DE8 File Offset: 0x00013FE8
		private static string JsQuote(string s)
		{
			if (s == null)
			{
				return "''";
			}
			return "'" + s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "")
				.Replace("\n", "") + "'";
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00015E4C File Offset: 0x0001404C
		private static List<KompassLinksScraper.YelpItem> ExtractItemsFast(WebView wb)
		{
			string js = "\r\n(function(){\r\n  function t(el){ return el ? (el.innerText || '').replace(/\\s+/g,' ').trim() : ''; }\r\n\r\n  // Punto stabile: il blocco nome attività\r\n  var nameBlocks = document.querySelectorAll('[data-traffic-crawl-id=\"SearchResultBizName\"]');\r\n  var out = [];\r\n\r\n  for (var i=0; i<nameBlocks.length; i++){\r\n    var nb = nameBlocks[i];\r\n    var a = nb.querySelector('a[href]');\r\n    if(!a) continue;\r\n\r\n    var name = t(a);\r\n    var link = a.href || a.getAttribute('href') || '';\r\n\r\n    // Risali al contenitore principale del risultato (di solito un div abbastanza alto)\r\n    // Prova: primo ancestor che contiene address o categories\r\n    var card = nb;\r\n    for (var up=0; up<8 && card; up++){\r\n      if(card.querySelector && (card.querySelector('address') || card.querySelector('[data-testid=\"serp-ia-categories\"]')))\r\n        break;\r\n      card = card.parentElement;\r\n    }\r\n    if(!card) card = nb.parentElement || nb;\r\n\r\n    // Categorie (stabile con data-testid)\r\n    var cat = '';\r\n    var catWrap = card.querySelector('[data-testid=\"serp-ia-categories\"]');\r\n    if(catWrap){\r\n      // prendiamo la prima categoria visibile\r\n      var catBtn = catWrap.querySelector('button');\r\n      if(catBtn) cat = t(catBtn);\r\n      if(!cat){\r\n        var catSpan = catWrap.querySelector('span');\r\n        if(catSpan) cat = t(catSpan);\r\n      }\r\n    }\r\n\r\n    // Indirizzo: usa tag address (molto stabile)\r\n    var addr = '';\r\n    var addrSpan = card.querySelector('address span');\r\n    if(addrSpan) addr = t(addrSpan);\r\n\r\n    // Telefono: Yelp spesso non lo mostra in SERP; metto fallback su pattern tel:\r\n    var phone = '';\r\n    var tel = card.querySelector('a[href^=\"tel:\"]');\r\n    if(tel) phone = (tel.getAttribute('href') || '').replace('tel:','').trim();\r\n\r\n    out.push({ name:name, href:link, cat:cat, addr:addr, phone:phone });\r\n  }\r\n\r\n  return JSON.stringify(out);\r\n})();";
			object jsonObj = wb.EvalScript(js);
			return JsonConvert.DeserializeObject<List<KompassLinksScraper.YelpItem>>((jsonObj == null) ? "[]" : jsonObj.ToString()) ?? new List<KompassLinksScraper.YelpItem>();
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00015E88 File Offset: 0x00014088
		private static void AddRowsBatch(MainForm mainForm, List<DataItem> pageItems)
		{
			mainForm.dgvResults.SuspendLayout();
			try
			{
				foreach (DataItem d in pageItems)
				{
					mainForm.dgvResults.Rows.Add(new object[]
					{
						d.Category, d.BusinessName, d.Address, "", d.State, d.PostalCode, d.Country, d.Phone, d.Fax, d.Website,
						"", d.MapLink, d.DetailsLink
					});
				}
			}
			finally
			{
				mainForm.dgvResults.ResumeLayout();
			}
			mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00015FB4 File Offset: 0x000141B4
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			while (!Program.IsStopped())
			{
				if (!KompassLinksScraper.WaitForSelector(WB, "[data-traffic-crawl-id='SearchResultBizName'] a[href]", 8000, 100))
				{
					return;
				}
				List<KompassLinksScraper.YelpItem> list = KompassLinksScraper.ExtractItemsFast(WB);
				List<DataItem> pageItems = new List<DataItem>(list.Count);
				foreach (KompassLinksScraper.YelpItem yi in list)
				{
					DataItem di = new DataItem();
					di.BusinessName = yi.name;
					di.Category = (yi.cat ?? "").Replace(",", "").Trim();
					di.Address = yi.addr ?? "";
					di.Phone = yi.phone ?? "";
					string link = yi.href ?? "";
					if (!string.IsNullOrEmpty(link))
					{
						if (link.IndexOf("http", StringComparison.OrdinalIgnoreCase) < 0)
						{
							Uri baseUri = new Uri(WB.Url.ToString());
							di.DetailsLink = new Uri(baseUri, link).ToString();
						}
						else
						{
							di.DetailsLink = link;
							if (link.IndexOf("adredir", StringComparison.OrdinalIgnoreCase) >= 0)
							{
								List<string[]> addItems = HTTPScraper.ParseHTML(link, "https:(.*?)redirect_url=(.*?)");
								if (addItems.Count > 0)
								{
									di.DetailsLink = addItems[0][2];
								}
							}
						}
					}
					if (!string.IsNullOrEmpty(di.DetailsLink))
					{
						pageItems.Add(di);
					}
if (Program.IsStopped())
					{
						return;
					}
				}
				KompassLinksScraper.AddRowsBatch(mainForm, pageItems);
				KompassLinksScraper.GetData(ref pageItems, mainForm);
				mainForm.tsProgress.Value = 0;

				List<Element> nextButtons = WebScraper.GetElements(WB, "a", "next-link navigation-button", false);
				Element NextButton;
				if (nextButtons.Count > 0)
				{
					NextButton = nextButtons[0];
					WB.LoadUrlAndWait(NextButton["href"].ToString());
					if (!KompassLinksScraper.WaitForSelector(WB, "[data-traffic-crawl-id='SearchResultBizName'] a[href]", 8000, 100))
					{
						goto IL_0245;
					}
				}
				else
				{
					NextButton = null;
				}
				if (!Program.IsStopped() && NextButton != null)
				{
					continue;
				}
				IL_0245:
				mainForm.tsProgress.Value = 0;
				MessageBox.Show(Program.LanguagesManager.WorkIsDone);
				return;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00016240 File Offset: 0x00014440
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 4;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			KompassDataScraper[] Pool = new KompassDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new KompassDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						KompassLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new KompassDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				if (Program.IsStopped())
				{
					break;
				}
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00016354 File Offset: 0x00014554
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = HTTPScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[2].Value = HTTPScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = HTTPScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = HTTPScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = HTTPScraper.ClearValue(Item.PostalCode);
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

		// Token: 0x0600013F RID: 319 RVA: 0x00002A43 File Offset: 0x00000C43
		private static string ClearValue(string Value)
		{
			if (Value != null)
			{
				return Value.Replace("&#x27", "");
			}
			return "";
		}

		// Token: 0x02000081 RID: 129
		private class YelpItem
		{
			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000236 RID: 566 RVA: 0x00026099 File Offset: 0x00024299
			// (set) Token: 0x06000237 RID: 567 RVA: 0x000260A1 File Offset: 0x000242A1
			public string name { get; set; }

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000238 RID: 568 RVA: 0x000260AA File Offset: 0x000242AA
			// (set) Token: 0x06000239 RID: 569 RVA: 0x000260B2 File Offset: 0x000242B2
			public string href { get; set; }

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x0600023A RID: 570 RVA: 0x000260BB File Offset: 0x000242BB
			// (set) Token: 0x0600023B RID: 571 RVA: 0x000260C3 File Offset: 0x000242C3
			public string cat { get; set; }

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x0600023C RID: 572 RVA: 0x000260CC File Offset: 0x000242CC
			// (set) Token: 0x0600023D RID: 573 RVA: 0x000260D4 File Offset: 0x000242D4
			public string addr { get; set; }

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x0600023E RID: 574 RVA: 0x000260DD File Offset: 0x000242DD
			// (set) Token: 0x0600023F RID: 575 RVA: 0x000260E5 File Offset: 0x000242E5
			public string phone { get; set; }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x0200001B RID: 27
	public static class JustLinksScraper
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x0000EE6C File Offset: 0x0000D06C
		private static string JsQuote(string s)
		{
			if (s == null)
			{
				return "''";
			}
			return "'" + s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "")
				.Replace("\n", "") + "'";
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000EECE File Offset: 0x0000D0CE
		private static void EnsureLoadMoreJs(WebView WB)
		{
			WB.EvalScript("\r\n(function(){\r\n  if(window.__gg_click_last_loadmore) return;\r\n\r\n  function isVisible(el){\r\n    if(!el) return false;\r\n    var st = window.getComputedStyle(el);\r\n    if(st.display==='none' || st.visibility==='hidden' || st.opacity==='0') return false;\r\n    var r = el.getBoundingClientRect();\r\n    if(r.width<=0 || r.height<=0) return false;\r\n    return true;\r\n  }\r\n\r\n  function isDisabled(el){\r\n    if(!el) return true;\r\n    if(el.disabled) return true;\r\n    var aria = el.getAttribute('aria-disabled');\r\n    if(aria && aria.toLowerCase()==='true') return true;\r\n    return false;\r\n  }\r\n\r\n  window.__gg_click_last_loadmore = function(){\r\n    // prendi TUTTI i bottoni loadMoreBtn (anche se id cambia)\r\n    var btns = Array.prototype.slice.call(document.querySelectorAll('button.loadMoreBtn'));\r\n    if(!btns.length) return { ok:false, why:'no_buttons' };\r\n\r\n    // scegli l'ultimo VISIBILE e NON disabilitato\r\n    var b = null;\r\n    for(var i=btns.length-1; i>=0; i--){\r\n      var x = btns[i];\r\n      if(isVisible(x) && !isDisabled(x)) { b = x; break; }\r\n    }\r\n    if(!b) return { ok:false, why:'no_visible_enabled', total:btns.length };\r\n\r\n    try{ b.scrollIntoView({block:'center', inline:'center'}); } catch(e){}\r\n    try{ window.scrollBy(0, 120); } catch(e){}\r\n\r\n    // dispatch eventi mouse (più affidabile)\r\n    var evs = ['mouseover','mousedown','mouseup','click'];\r\n    for(var j=0;j<evs.length;j++){\r\n      try{\r\n        b.dispatchEvent(new MouseEvent(evs[j], {bubbles:true, cancelable:true, view:window}));\r\n      }catch(e){}\r\n    }\r\n\r\n    // fallback click diretto\r\n    try{ b.click(); }catch(e){}\r\n\r\n    return { ok:true, id:b.id||'', text:(b.innerText||'').trim(), total:btns.length };\r\n  };\r\n\r\n})();");
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000EEDC File Offset: 0x0000D0DC
		private static int GetCardsCount(WebView WB)
		{
			try
			{
				object o = WB.EvalScript("document.querySelectorAll('div.eachPopular').length");
				int i;
				if (o != null && int.TryParse(o.ToString(), out i))
				{
					return i;
				}
			}
			catch
			{
			}
			return 0;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000EF24 File Offset: 0x0000D124
		private static bool ClickLoadMoreAndWait(WebView WB, int prevCount, int timeoutMs = 15000)
		{
			JustLinksScraper.EnsureLoadMoreJs(WB);
			try
			{
				WB.EvalScript("window.scrollTo(0, document.body.scrollHeight);");
			}
			catch
			{
			}
			Thread.Sleep(250);
			Application.DoEvents();
			object r = null;
			try
			{
				r = WB.EvalScript("window.__gg_click_last_loadmore();");
			}
			catch
			{
			}
			if (r == null || r.ToString().Contains("ok:false"))
			{
				return false;
			}
			Stopwatch sw = Stopwatch.StartNew();
			while (sw.ElapsedMilliseconds < (long)timeoutMs)
			{
				if (Program.IsStopped())
				{
					return false;
				}
				int cur = 0;
				try
				{
					object o = WB.EvalScript("document.querySelectorAll('div.eachPopular').length");
					if (o != null)
					{
						int.TryParse(o.ToString(), out cur);
					}
				}
				catch
				{
				}
				if (cur > prevCount)
				{
					return true;
				}
				Thread.Sleep(250);
				Application.DoEvents();
			}
			return false;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000F000 File Offset: 0x0000D200
		private static DataItem ParseCard(WebView WB, Element card)
		{
			DataItem di = new DataItem();
			di.Country = "India";
			List<Element> titleA = WebScraper.QuerySelectorAll(WB, card, "a.eachPopularTitle[href]");
			if (titleA.Count > 0)
			{
				try
				{
					di.BusinessName = (titleA[0].innerText ?? "").Trim();
					string href = (titleA[0]["href"] ?? "").ToString();
					if (!string.IsNullOrEmpty(href))
					{
						if (href.StartsWith("http", StringComparison.OrdinalIgnoreCase))
						{
							di.DetailsLink = href;
						}
						else
						{
							di.DetailsLink = new Uri(new Uri(WB.Url), href).ToString();
						}
					}
				}
				catch
				{
				}
			}
			List<Element> phoneA = WebScraper.QuerySelectorAll(WB, card, "a.businessContact");
			if (phoneA.Count > 0)
			{
				try
				{
					di.Phone = (phoneA[0].innerText ?? "").Trim();
				}
				catch
				{
				}
			}
			List<Element> addr = WebScraper.QuerySelectorAll(WB, card, "address.businessArea");
			if (addr.Count > 0)
			{
				try
				{
					string t = (addr[0].innerText ?? "").Trim();
					Match i = Regex.Match(t, "^\\s*(.+?)\\s+(.+?)\\s*-\\s*(\\d{6})\\s*$");
					if (i.Success)
					{
						di.Address = i.Groups[1].Value.Trim();
						di.City = i.Groups[2].Value.Trim();
						di.PostalCode = i.Groups[3].Value.Trim();
					}
					else
					{
						di.Address = t;
					}
				}
				catch
				{
				}
			}
			List<Element> cat = WebScraper.QuerySelectorAll(WB, card, "ul.eachPopularTagsList");
			if (cat.Count > 0)
			{
				try
				{
					di.Category = (cat[0].innerText ?? "").Trim();
				}
				catch
				{
				}
			}
			foreach (Element a in WebScraper.QuerySelectorAll(WB, card, "div.eachPopularLink a[href]"))
			{
				try
				{
					string href2 = (a["href"] ?? "").ToString().Trim();
					if (!string.IsNullOrEmpty(href2))
					{
						if (href2.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
						{
							string email = href2.Substring("mailto:".Length).Trim();
							if (!string.IsNullOrEmpty(email))
							{
								di.Email = email;
							}
						}
						else
						{
							if (href2.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
							{
								href2 = "http://" + href2;
							}
							di.Website = href2;
						}
					}
				}
				catch
				{
				}
			}
			return di;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000F304 File Offset: 0x0000D504
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			List<DataItem> pageItems = new List<DataItem>();
			JustLinksScraper.WaitForBrowser(WB);
			HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			int noNewRounds = 0;
			while (!Program.IsStopped() && noNewRounds < 12)
			{
				JustLinksScraper.WaitForBrowser(WB);
				int addedThisRound = 0;
				foreach (Element card in WebScraper.QuerySelectorAll(WB, "div.eachPopular"))
				{
					if (Program.IsStopped())
					{
						return;
					}
					string key = "";
					List<Element> titleA = WebScraper.QuerySelectorAll(WB, card, "a.eachPopularTitle[href]");
					if (titleA.Count > 0)
					{
						try
						{
							key = (titleA[0]["href"] ?? "").ToString();
						}
						catch
						{
						}
					}
					if (string.IsNullOrWhiteSpace(key))
					{
						string nm = "";
						string ph = "";
						try
						{
							if (titleA.Count > 0)
							{
								nm = (titleA[0].innerText ?? "").Trim();
							}
							List<Element> phA = WebScraper.QuerySelectorAll(WB, card, "a.businessContact[href^='tel:'], a.businessContact");
							if (phA.Count > 0)
							{
								ph = (phA[0].innerText ?? "").Trim();
							}
						}
						catch
						{
						}
						key = (nm + "|" + ph).Trim(new char[] { '|' });
					}
					if (!string.IsNullOrWhiteSpace(key) && seen.Add(key))
					{
						DataItem di = JustLinksScraper.ParseCard(WB, card);
						if (!string.IsNullOrEmpty(di.DetailsLink))
						{
							pageItems.Add(di);
							addedThisRound++;
							mainForm.dgvResults.Rows.Add(new object[]
							{
								di.Category, di.BusinessName, di.Address, di.City, di.State, di.PostalCode, di.Country, di.Phone, di.Fax, di.Website,
								di.Email, di.MapLink, di.DetailsLink
							});
							mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
							mainForm.tssLabelListed.Invalidate();
						}
Application.DoEvents();
					}
				}

				int before = JustLinksScraper.GetCardsCount(WB);
				try
				{
					WB.EvalScript("window.scrollTo(0, document.body.scrollHeight);");
				}
				catch
				{
				}
				Thread.Sleep(250);
				Application.DoEvents();
				bool loaded = JustLinksScraper.ClickLoadMoreAndWait(WB, before, 15000);
				if (addedThisRound == 0 && !loaded)
				{
					noNewRounds++;
				}
				else
				{
					noNewRounds = 0;
				}
				for (int w = 0; w < 8; w++)
				{
					if (Program.IsStopped())
					{
						return;
					}
					Thread.Sleep(150);
					Application.DoEvents();
				}
				continue;
			}
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000F6B4 File Offset: 0x0000D8B4
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			JustDataScraper[] Pool = new JustDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new JustDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						JustLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new JustDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000EA RID: 234 RVA: 0x0000F7C8 File Offset: 0x0000D9C8
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000F83F File Offset: 0x0000DA3F
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1000)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000F868 File Offset: 0x0000DA68
		private static void ScrollItDown(WebView WB, int Iterations)
		{
			List<Element> NextButtons = WebScraper.GetElements(WB, "button", "loadMoreBtn", true);
			if (NextButtons.Count > 0)
			{
				WebScraper.InvokeMember(WB, NextButtons[0], "click");
				Thread.Sleep(50);
				Application.DoEvents();
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00002A43 File Offset: 0x00000C43
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

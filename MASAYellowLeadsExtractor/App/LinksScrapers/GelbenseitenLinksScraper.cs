using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000036 RID: 54
	public class GelbenseitenLinksScraper
	{
		// Token: 0x06000176 RID: 374 RVA: 0x0001BF20 File Offset: 0x0001A120
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			List<DataItem> PageItems = new List<DataItem>();
			GelbenseitenLinksScraper.WaitForBrowser(WB);
			int stableAttempts = 0;
			int clickCount = 0;
			int lastCount = GelbenseitenLinksScraper.GetArticleCountSafe(WB);
			Stopwatch scrollWatch = Stopwatch.StartNew();
			Stopwatch noProgressWatch = Stopwatch.StartNew();
			while (!Program.IsStopped())
			{
				Application.DoEvents();
				if (scrollWatch.Elapsed.TotalSeconds > 1200.0)
				{
					mainForm.tssLabelListed.Text = string.Format("Stopped by safety timeout. {0} results loaded", lastCount);
					mainForm.tssLabelListed.Invalidate();
					break;
				}
				int currentCount = GelbenseitenLinksScraper.GetArticleCountSafe(WB);
				if (currentCount > lastCount)
				{
					lastCount = currentCount;
					stableAttempts = 0;
					noProgressWatch.Restart();
				}
				mainForm.tssLabelListed.Text = string.Format("Loaded results: {0}", currentCount);
				mainForm.tssLabelListed.Invalidate();
				int buttonWaitMs = GelbenseitenLinksScraper.GetButtonAppearTimeoutMs(currentCount);
				int clickWaitMs = GelbenseitenLinksScraper.GetResultsLoadTimeoutMs(currentCount);
				mainForm.tssLabelListed.Text = string.Format("Scroll: {0} | button: 0/{1}s", currentCount, buttonWaitMs / 1000);
				mainForm.tssLabelListed.Invalidate();
				Application.DoEvents();
				GelbenseitenLinksScraper.ForceScrollDown(WB);
				Application.DoEvents();
				string clickStatus = GelbenseitenLinksScraper.TryClickLoadMoreWithWait(WB, buttonWaitMs, mainForm, currentCount);
				if (clickStatus != "CLICKED")
				{
					mainForm.tssLabelListed.Text = string.Format("Scroll stopped: {0}. {1} results loaded", clickStatus, currentCount);
					mainForm.tssLabelListed.Invalidate();
					break;
				}
				clickCount++;
				mainForm.tssLabelListed.Text = string.Format("Scroll: {0} | loading: 0/{1}s", currentCount, clickWaitMs / 1000);
				mainForm.tssLabelListed.Invalidate();
				Application.DoEvents();
				bool flag = GelbenseitenLinksScraper.WaitForMoreArticles(WB, currentCount, clickWaitMs, mainForm);
				int afterCount = GelbenseitenLinksScraper.GetArticleCountSafe(WB);
				if (flag || afterCount > currentCount)
				{
					lastCount = afterCount;
					stableAttempts = 0;
					noProgressWatch.Restart();
				}
				else
				{
					stableAttempts++;
					mainForm.tssLabelListed.Text = string.Format("No new results after click {0}. Retry {1}/{2}. Loaded: {3}", new object[] { clickCount, stableAttempts, 3, afterCount });
					mainForm.tssLabelListed.Invalidate();
					Application.DoEvents();
					if (stableAttempts >= 3)
					{
						mainForm.tssLabelListed.Text = string.Format("Scroll stopped: button clicked but no new results. {0} results loaded", afterCount);
						mainForm.tssLabelListed.Invalidate();
						break;
					}
					int noProgressLimitSeconds = GelbenseitenLinksScraper.GetNoProgressLimitSeconds(afterCount);
					if (noProgressWatch.Elapsed.TotalSeconds > (double)noProgressLimitSeconds)
					{
						mainForm.tssLabelListed.Text = string.Format("Scroll stopped: no progress for {0} seconds. {1} results loaded", noProgressLimitSeconds, afterCount);
						mainForm.tssLabelListed.Invalidate();
						break;
					}
				}
				Thread.Sleep(200);
			}
			int totalLoaded = GelbenseitenLinksScraper.GetArticleCountSafe(WB);
			mainForm.tssLabelListed.Text = string.Format("Scroll finished: {0} results loaded. Now extracting data...", totalLoaded);
			mainForm.tssLabelListed.Invalidate();
			Application.DoEvents();
			HashSet<string> processedDetailLinks = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			int parsedIndex = 0;
			while (!Program.IsStopped())
			{
				int hardLimit = 6000;
				if (mainForm.dgvResults.Rows.Count >= hardLimit)
				{
					break;
				}
				List<string> articleHtmlList = GelbenseitenLinksScraper.GetArticleHtmlBatch(WB, parsedIndex, 200);
				if (articleHtmlList.Count == 0)
				{
					break;
				}
				for (int i = 0; i < articleHtmlList.Count; i++)
				{
					if (Program.IsStopped())
					{
						goto IL_0537;
					}
					DataItem dataItem = GelbenseitenLinksScraper.ParseDataItemFromArticleHtml(articleHtmlList[i]);
					if (dataItem != null && !string.IsNullOrEmpty(dataItem.DetailsLink) && processedDetailLinks.Add(dataItem.DetailsLink))
					{
						if (Program.AppSettings.ExtractEmails)
						{
							mainForm.tssLabelListed.Text = string.Format("Extracting full data: {0} listed from {1} loaded articles", mainForm.dgvResults.Rows.Count + 1, totalLoaded);
							mainForm.tssLabelListed.Invalidate();
							Application.DoEvents();
							GelbenseitenLinksScraper.EnrichDataItemFromDetailsHttp(dataItem, mainForm);
						}
						PageItems.Add(dataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							dataItem.Category, dataItem.BusinessName, dataItem.Address, dataItem.City, "", dataItem.PostalCode, "Germany", dataItem.Phone, dataItem.Fax, dataItem.Website,
							dataItem.Email, "", dataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} complete items listed from {1} loaded articles", mainForm.dgvResults.Rows.Count, totalLoaded);
						mainForm.tssLabelListed.Invalidate();
						Application.DoEvents();
					}
				}
				parsedIndex += articleHtmlList.Count;
			}
			IL_0537:

			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0001C4A4 File Offset: 0x0001A6A4
		private static List<string> GetArticleHtmlBatch(WebView WB, int startIndex, int batchSize)
		{
			List<string> result = new List<string>();
			try
			{
				string script = string.Concat(new string[]
				{
					"\n(function(start, limit){\n    var items = document.querySelectorAll('article[class*=\"mod-Treffer\"]');\n    var arr = [];\n    var max = Math.min(items.length, start + limit);\n    for (var i = start; i < max; i++) {\n        arr.push(items[i].outerHTML || '');\n    }\n    return arr.join('\\n---ARTICLE_SPLIT---\\n');\n})(",
					startIndex.ToString(),
					", ",
					batchSize.ToString(),
					");"
				});
				object html = WB.EvalScript(script);
				if (html == null)
				{
					return result;
				}
				string all = html.ToString();
				if (string.IsNullOrEmpty(all))
				{
					return result;
				}
				result.AddRange(all.Split(new string[] { "\n---ARTICLE_SPLIT---\n" }, StringSplitOptions.RemoveEmptyEntries));
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0001C548 File Offset: 0x0001A748
		private static DataItem ParseDataItemFromArticleHtml(string html)
		{
			if (string.IsNullOrEmpty(html))
			{
				return null;
			}
			DataItem dataItem = new DataItem();
			dataItem.Country = "Germany";
			List<string[]> addressParts = HTTPScraper.ParseHTML(html, GelbenseitenLinksScraper.RxRealId);
			if (addressParts.Count > 0)
			{
				dataItem.DetailsLink = "https://www.gelbeseiten.de/gsbiz/" + addressParts[0][1];
			}
			if (string.IsNullOrEmpty(dataItem.DetailsLink))
			{
				return null;
			}
			dataItem.BusinessName = GelbenseitenLinksScraper.ExtractInnerText(html, "<h2[^>]*>([\\s\\S]*?)</h2>");
			dataItem.Address = GelbenseitenLinksScraper.ExtractInnerTextByClass(html, "mod-AdresseKompakt__adress-text");
			if (!string.IsNullOrEmpty(dataItem.Address) && dataItem.Address.IndexOf(',') >= 0)
			{
				dataItem.Address = dataItem.Address.Split(new char[] { ',' })[0].Trim();
			}
			string cityPostal = GelbenseitenLinksScraper.ExtractInnerTextByClass(html, "mod-AdresseKompakt__adress__ort");
			if (!string.IsNullOrEmpty(cityPostal))
			{
				string[] cityAndPostalCode = cityPostal.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				if (cityAndPostalCode.Length != 0)
				{
					dataItem.PostalCode = cityAndPostalCode[0];
					if (cityAndPostalCode.Length > 1)
					{
						dataItem.City = string.Join(" ", cityAndPostalCode.Skip<string>(1));
					}
				}
			}
			dataItem.Category = GelbenseitenLinksScraper.ExtractInnerTextByClass(html, "mod-Treffer--besteBranche");
			dataItem.Phone = GelbenseitenLinksScraper.ExtractInnerTextByClass(html, "mod-TelefonnummerKompakt__phoneNumber");
			List<string[]> mapParts = HTTPScraper.ParseHTML(html, GelbenseitenLinksScraper.RxMapSearchQuery);
			if (mapParts.Count > 0)
			{
				dataItem.MapLink = "https://www.google.com/maps/place/" + mapParts[0][1].Replace("\"", "");
			}
			return dataItem;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0001C6C8 File Offset: 0x0001A8C8
		private static string ExtractInnerTextByClass(string html, string classPart)
		{
			if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(classPart))
			{
				return "";
			}
			string pattern = "<[^>]+class=\"[^\"]*" + Regex.Escape(classPart) + "[^\"]*\"[^>]*>([\\s\\S]*?)</[^>]+>";
			return GelbenseitenLinksScraper.ExtractInnerText(html, pattern);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0001C708 File Offset: 0x0001A908
		private static string ExtractInnerText(string html, string pattern)
		{
			string text;
			try
			{
				Match i = Regex.Match(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
				if (!i.Success)
				{
					text = "";
				}
				else
				{
					text = Regex.Replace(WebUtility.HtmlDecode(Regex.Replace(Regex.Replace(Regex.Replace(i.Groups[1].Value, "<script[\\s\\S]*?</script>", "", RegexOptions.IgnoreCase), "<style[\\s\\S]*?</style>", "", RegexOptions.IgnoreCase), "<[^>]+>", " ")), "\\s+", " ").Trim();
				}
			}
			catch
			{
				text = "";
			}
			return text;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0001C7A8 File Offset: 0x0001A9A8
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			int i = 0;
			while (i < DataItems.Count)
			{
				Program.RequestDelay();
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(500);
				string HTML = WB.GetHtml();
				Thread.Sleep(500);
				List<string[]> emailMatches = HTTPScraper.ParseHTML(HTML, GelbenseitenLinksScraper.RxEmailButton);
				if (emailMatches.Count > 0 && emailMatches[0][1].IndexOf("@", StringComparison.Ordinal) > -1)
				{
					try
					{
						DataItems[i].Email = emailMatches[0][1].Replace("mailto:", "").Split(new char[] { '?' })[0];
						GelbenseitenLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
						goto IL_0150;
					}
					catch
					{
						goto IL_0150;
					}
					goto IL_00BB;
				}
				goto IL_00BB;
				IL_0150:
				List<string[]> websiteMatches = HTTPScraper.ParseHTML(HTML, GelbenseitenLinksScraper.RxWebsite);
				if (websiteMatches.Count > 0)
				{
					DataItems[i].Website = websiteMatches[0][1].Split(new char[] { '"' })[0];
					GelbenseitenLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				}
				if (Program.IsStopped())
				{
					return;
				}
				i++;
				continue;
				IL_00BB:
				try
				{
					List<string[]> emailFallback = HTTPScraper.ParseHTML(HTML, GelbenseitenLinksScraper.RxEmailFallback);
					if (emailFallback.Count > 0)
					{
						DataItems[i].Email = emailFallback[0][1].Replace("&quot;", "").Replace("&quot", "").Replace("%40", "@")
							.Replace("&amp", "")
							.Split(new char[] { ',' })[0];
						GelbenseitenLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
					}
				}
				catch
				{
				}
				goto IL_0150;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0001C98C File Offset: 0x0001AB8C
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			GelbenseitenDataScraper[] Pool = new GelbenseitenDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new GelbenseitenDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						GelbenseitenLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new GelbenseitenDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				Thread.Sleep(2000);
				Application.DoEvents();
				if (Program.IsStopped())
				{
					break;
				}
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[8].Value = GelbenseitenLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = GelbenseitenLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = GelbenseitenLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00002A43 File Offset: 0x00000C43
		private static string ClearValue(string Value)
		{
			if (Value != null)
			{
				return Value.Replace("&#x27", "");
			}
			return "";
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0001CBA8 File Offset: 0x0001ADA8
		private static int GetArticleCountSafe(WebView WB)
		{
			try
			{
				object result = WB.EvalScript("\n(function(){\n    return document.querySelectorAll('article[class*=\"mod-Treffer\"]').length;\n})();\n");
				int count;
				if (result != null && int.TryParse(result.ToString(), out count))
				{
					return count;
				}
			}
			catch
			{
			}
			return 0;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0001CBF0 File Offset: 0x0001ADF0
		private static int GetButtonAppearTimeoutMs(int currentCount)
		{
			if (currentCount < 1000)
			{
				return 2000;
			}
			if (currentCount < 2000)
			{
				return 4000;
			}
			if (currentCount < 3000)
			{
				return 5000;
			}
			return 10000;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0001CBF0 File Offset: 0x0001ADF0
		private static int GetResultsLoadTimeoutMs(int currentCount)
		{
			if (currentCount < 1000)
			{
				return 2000;
			}
			if (currentCount < 2000)
			{
				return 4000;
			}
			if (currentCount < 3000)
			{
				return 5000;
			}
			return 10000;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0001CC21 File Offset: 0x0001AE21
		private static int GetNoProgressLimitSeconds(int currentCount)
		{
			return 30;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0001CC28 File Offset: 0x0001AE28
		private static string TryClickLoadMoreWithWait(WebView WB, int timeoutMs, MainForm mainForm, int currentCount)
		{
			int waited = 0;
			string lastStatus = "NO_BUTTON";
			while (waited < timeoutMs)
			{
				if (Program.IsStopped())
				{
					return "STOPPED";
				}
				Application.DoEvents();
				lastStatus = GelbenseitenLinksScraper.TryClickLoadMore(WB);
				if (lastStatus == "CLICKED")
				{
					return "CLICKED";
				}
				if (mainForm != null)
				{
					mainForm.tssLabelListed.Text = string.Format("Scroll: {0} | button: {1}/{2}s | {3}", new object[]
					{
						currentCount,
						waited / 1000,
						timeoutMs / 1000,
						lastStatus
					});
					mainForm.tssLabelListed.Invalidate();
				}
				Thread.Sleep(250);
				waited += 250;
			}
			return lastStatus + "_TIMEOUT";
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0001CCE8 File Offset: 0x0001AEE8
		private static string TryClickLoadMore(WebView WB)
		{
			string text;
			try
			{
				object result = WB.EvalScript("\n(function(){\n    try {\n        var btn = document.querySelector('a.mod-LoadMore--button');\n        if (!btn) {\n            btn = document.querySelector('[class*=\"LoadMore\"] a, a[class*=\"LoadMore\"], button[class*=\"LoadMore\"], .mod-LoadMore--button');\n        }\n        if (!btn) return 'NO_BUTTON';\n\n        var style = window.getComputedStyle ? window.getComputedStyle(btn) : null;\n        if (style && (style.display === 'none' || style.visibility === 'hidden' || style.opacity === '0')) return 'HIDDEN';\n        if (btn.disabled || btn.getAttribute('aria-disabled') === 'true') return 'DISABLED';\n\n        var rect = btn.getBoundingClientRect ? btn.getBoundingClientRect() : null;\n        if (rect && (rect.width <= 0 || rect.height <= 0)) return 'HIDDEN';\n\n        // Click diretto dopo lo scroll visibile già eseguito dal codice C#.\n\n        try {\n            btn.dispatchEvent(new MouseEvent('mousedown', {bubbles:true, cancelable:true, view:window}));\n            btn.dispatchEvent(new MouseEvent('mouseup', {bubbles:true, cancelable:true, view:window}));\n            btn.dispatchEvent(new MouseEvent('click', {bubbles:true, cancelable:true, view:window}));\n        } catch(e1) {\n            try { btn.click(); } catch(e2) { return 'ERROR'; }\n        }\n\n        return 'CLICKED';\n    } catch(ex) {\n        return 'ERROR';\n    }\n})();");
				if (result == null)
				{
					text = "ERROR";
				}
				else
				{
					text = result.ToString();
				}
			}
			catch
			{
				text = "ERROR";
			}
			return text;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0001CD30 File Offset: 0x0001AF30
		private static bool HasLoadMoreButtonSafe(WebView WB)
		{
			bool flag;
			try
			{
				object result = WB.EvalScript("\n(function(){\n    var btn = document.querySelector('a.mod-LoadMore--button');\n    if (!btn) {\n        btn = document.querySelector('[class*=\"LoadMore\"] a, a[class*=\"LoadMore\"], button[class*=\"LoadMore\"], .mod-LoadMore--button');\n    }\n    if (!btn) return false;\n\n    var style = window.getComputedStyle ? window.getComputedStyle(btn) : null;\n    if (style && (style.display === 'none' || style.visibility === 'hidden')) return false;\n\n    if (btn.disabled) return false;\n    if (btn.getAttribute('aria-disabled') === 'true') return false;\n\n    var txt = (btn.innerText || btn.textContent || '').replace(/\\s+/g, ' ').trim();\n    if (txt === '0') return false;\n\n    return true;\n})();\n");
				flag = result != null && result.ToString().ToLower() == "true";
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0001CD80 File Offset: 0x0001AF80
		private static bool ClickLoadMoreSafe(WebView WB)
		{
			bool flag;
			try
			{
				object result = WB.EvalScript("\n(function(){\n    var btn = document.querySelector('a.mod-LoadMore--button');\n    if (!btn) {\n        btn = document.querySelector('[class*=\"LoadMore\"] a, a[class*=\"LoadMore\"], button[class*=\"LoadMore\"], .mod-LoadMore--button');\n    }\n    if (!btn) return false;\n\n    var style = window.getComputedStyle ? window.getComputedStyle(btn) : null;\n    if (style && (style.display === 'none' || style.visibility === 'hidden')) return false;\n\n    // Click diretto, senza scrollIntoView aggiuntivo qui.\n    try {\n        var ev = new MouseEvent('click', {bubbles:true, cancelable:true, view:window});\n        btn.dispatchEvent(ev);\n    } catch(e) {\n        try { btn.click(); } catch(e2) { return false; }\n    }\n\n    return true;\n})();\n");
				flag = result != null && result.ToString().ToLower() == "true";
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0001CDD0 File Offset: 0x0001AFD0
		private static void ForceScrollDown(WebView WB)
		{
			try
			{
				WB.EvalScript("\n(function(){\n    window.scrollTo(0, document.body.scrollHeight || document.documentElement.scrollHeight || 999999);\n})();\n");
			}
			catch
			{
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0001CE00 File Offset: 0x0001B000
		private static bool WaitForMoreArticles(WebView WB, int previousCount, int timeoutMs, MainForm mainForm)
		{
			int waited = 0;
			while (waited < timeoutMs)
			{
				Application.DoEvents();
				Thread.Sleep(250);
				waited += 250;
				int currentCount = GelbenseitenLinksScraper.GetArticleCountSafe(WB);
				if (mainForm != null)
				{
					mainForm.tssLabelListed.Text = string.Format("Scroll: {0} | loading: {1}/{2}s", currentCount, waited / 1000, timeoutMs / 1000);
					mainForm.tssLabelListed.Invalidate();
				}
				if (currentCount > previousCount)
				{
					return true;
				}
				if (Program.IsStopped())
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0001CE84 File Offset: 0x0001B084
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,20000)");
				Application.DoEvents();
				Thread.Sleep(100);
				WB.EvalScript("window.scrollTo(0,30000)");
				Application.DoEvents();
				Thread.Sleep(100);
				WB.EvalScript("window.scrollTo(0,50000)");
				Application.DoEvents();
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0001CEDC File Offset: 0x0001B0DC
		private static void EnrichAllDataItemsFromDetailsHttp(List<DataItem> pageItems, MainForm mainForm)
		{
			int total = ((pageItems == null) ? 0 : pageItems.Count);
			int websitesFound = 0;
			int emailsFound = 0;
			if (total == 0)
			{
				return;
			}
			int i = 0;
			while (i < total && !Program.IsStopped())
			{
				DataItem item = pageItems[i];
				string oldWebsite = item.Website;
				string oldEmail = item.Email;
				GelbenseitenLinksScraper.EnrichDataItemFromDetailsHttp(item, mainForm);
				if (string.IsNullOrEmpty(oldWebsite) && !string.IsNullOrEmpty(item.Website))
				{
					websitesFound++;
				}
				if (string.IsNullOrEmpty(oldEmail) && !string.IsNullOrEmpty(item.Email))
				{
					emailsFound++;
				}
				float progress = 100f * (float)(i + 1) / (float)total;
				if (progress < 100f)
				{
					mainForm.tsProgress.Value = (int)progress;
				}
				mainForm.tssLabelListed.Text = string.Format("Extracting details... {0}/{1} - websites: {2}, emails: {3}", new object[]
				{
					i + 1,
					total,
					websitesFound,
					emailsFound
				});
				mainForm.tssLabelListed.Invalidate();
				Application.DoEvents();
				i++;
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0001CFEC File Offset: 0x0001B1EC
		private static void EnrichDataItemFromDetailsHttp(DataItem item, MainForm mainForm)
		{
			try
			{
				Program.RequestDelay();
				ProxyServer proxy = null;
				if (mainForm.ProxyServers != null && mainForm.ProxyServers.Count > 0)
				{
					proxy = mainForm.ProxyServers[0];
				}
				string html = HTTPScraper.GetPage(item.DetailsLink, proxy);
				if (!string.IsNullOrEmpty(html))
				{
					List<string[]> emailMatches = HTTPScraper.ParseHTML(html, GelbenseitenLinksScraper.RxEmailButton);
					if (emailMatches.Count > 0 && emailMatches[0][1].IndexOf("@", StringComparison.Ordinal) > -1)
					{
						item.Email = emailMatches[0][1].Replace("mailto:", "").Split(new char[] { '?' })[0];
						GelbenseitenLinksScraper.UpdateTable(item, mainForm, 100f);
					}
					else
					{
						List<string[]> emailFallback = HTTPScraper.ParseHTML(html, GelbenseitenLinksScraper.RxEmailFallback);
						if (emailFallback.Count > 0)
						{
							item.Email = emailFallback[0][1].Replace("&quot;", "").Replace("&quot", "").Replace("%40", "@")
								.Replace("&amp", "")
								.Split(new char[] { ',' })[0];
							GelbenseitenLinksScraper.UpdateTable(item, mainForm, 100f);
						}
					}
					List<string[]> websiteMatches = HTTPScraper.ParseHTML(html, GelbenseitenLinksScraper.RxWebsite);
					if (websiteMatches.Count > 0)
					{
						string url = websiteMatches[0][1].Trim();
						if (!(url == "/") && !(url == "#"))
						{
							if (url.StartsWith("//"))
							{
								url = "https:" + url;
							}
							item.Website = url;
							GelbenseitenLinksScraper.UpdateTable(item, mainForm, 100f);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0001D1C0 File Offset: 0x0001B3C0
		private static void ScrollItDown(WebView WB, int Iterations)
		{
			GelbenseitenLinksScraper.TryClickLoadMore(WB);
			Application.DoEvents();
		}

		// Token: 0x040000D9 RID: 217
		private static readonly Regex RxRealId = new Regex("data-realid=\"(.*?)\"", RegexOptions.Compiled | RegexOptions.Singleline);

		// Token: 0x040000DA RID: 218
		private static readonly Regex RxMapSearchQuery = new Regex("\"searchquery\": (.*?)}", RegexOptions.Compiled | RegexOptions.Singleline);

		// Token: 0x040000DB RID: 219
		private static readonly Regex RxEmailButton = new Regex("data-wipe-realview=\"detailseite_e-mail-button\" data-link=\"(.*?)\"", RegexOptions.Compiled | RegexOptions.Singleline);

		// Token: 0x040000DC RID: 220
		private static readonly Regex RxEmailFallback = new Regex("email=(.*?);", RegexOptions.Compiled | RegexOptions.Singleline);

		// Token: 0x040000DD RID: 221
		private static readonly Regex RxWebsite = new Regex("mod-Kontaktdaten__list-item[^>]*contains-icon-big-homepage[\\s\\S]*?<a[^>]+href=\"([^\"]+)\"", RegexOptions.Compiled | RegexOptions.Singleline);
	}
}

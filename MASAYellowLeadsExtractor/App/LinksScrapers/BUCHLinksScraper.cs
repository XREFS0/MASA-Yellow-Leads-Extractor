using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASAYellowLeadsExtractor.DataScrapers;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000016 RID: 22
	public static class BUCHLinksScraper
	{
		// Token: 0x060000CD RID: 205 RVA: 0x0000BFB4 File Offset: 0x0000A1B4
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			int noNewRounds = 0;
			while (!Program.IsStopped() && noNewRounds < 10)
			{
				int addedThisRound = 0;
				List<DataItem> pageItems = new List<DataItem>();
				foreach (Element card in WebScraper.QuerySelectorAll(WB, "div.vcard"))
				{
					if (Program.IsStopped())
					{
						return;
					}
					string key = "";
					List<Element> aName = WebScraper.QuerySelectorAll(WB, card, "a.name[href]");
					if (aName.Count > 0)
					{
						try
						{
							key = (aName[0]["href"] ?? "").ToString();
						}
						catch
						{
						}
					}
					if (string.IsNullOrWhiteSpace(key))
					{
						string nm = "";
						string ad = "";
						try
						{
							List<Element> nmEl = WebScraper.QuerySelectorAll(WB, card, "[itemprop='name']");
							if (nmEl.Count > 0)
							{
								nm = (nmEl[0].innerText ?? "").Trim();
							}
							List<Element> adEl = WebScraper.QuerySelectorAll(WB, card, "[itemprop='streetAddress']");
							if (adEl.Count > 0)
							{
								ad = (adEl[0].innerText ?? "").Trim();
							}
						}
						catch
						{
						}
						key = (nm + "|" + ad).Trim(new char[] { '|' });
					}
					if (!string.IsNullOrWhiteSpace(key) && seen.Add(key))
					{
						DataItem di = BUCHLinksScraper.ParseCard(WB, card);
						if (Program.AppSettings.ExtractEmails && !string.IsNullOrEmpty(di.Website))
						{
							di.Email = EmailMiner.GetEmail(di.Website, new string[] { "kontakt", "impressum", "contact" });
						}
						if (!string.IsNullOrEmpty(di.DetailsLink) && !string.IsNullOrEmpty(di.Address))
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
				mainForm.tsProgress.Value = 0;

				if (addedThisRound == 0)
				{
					noNewRounds++;
				}
				else
				{
					noNewRounds = 0;
				}
				try
				{
					WB.EvalScript("window.scrollBy(0, Math.max(800, window.innerHeight));");
				}
				catch
				{
				}
				try
				{
					if (WebScraper.QuerySelectorAll(WB, "div.loadbutton button.btn").Count > 0)
					{
						WB.EvalScript("\r\n(function(){\r\n  var b = document.querySelector('div.loadbutton button.btn');\r\n  if(b) b.click();\r\n})();");
					}
				}
				catch
				{
				}
				for (int w = 0; w < 15; w++)
				{
					if (Program.IsStopped())
					{
						return;
					}
					Thread.Sleep(100);
					Application.DoEvents();
				}
				continue;
			}
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000C3E4 File Offset: 0x0000A5E4
		private static DataItem ParseCard(WebView WB, Element card)
		{
			DataItem di = new DataItem();
			List<Element> aName = WebScraper.QuerySelectorAll(WB, card, "a.name[href]");
			if (aName.Count > 0)
			{
				try
				{
					di.DetailsLink = (aName[0]["href"] ?? "").ToString();
					di.BusinessName = (aName[0].innerText ?? "").Trim();
					goto IL_009F;
				}
				catch
				{
					goto IL_009F;
				}
			}
			List<Element> nm = WebScraper.QuerySelectorAll(WB, card, "[itemprop='name']");
			if (nm.Count > 0)
			{
				di.BusinessName = (nm[0].innerText ?? "").Trim();
			}
			IL_009F:
			List<Element> street = WebScraper.QuerySelectorAll(WB, card, "[itemprop='streetAddress']");
			List<Element> pc = WebScraper.QuerySelectorAll(WB, card, "[itemprop='postalCode']");
			List<Element> city = WebScraper.QuerySelectorAll(WB, card, "[itemprop='addressLocality']");
			if (street.Count > 0 || pc.Count > 0 || city.Count > 0)
			{
				string sStreet = ((street.Count > 0) ? (street[0].innerText ?? "").Trim() : "");
				string sPc = ((pc.Count > 0) ? (pc[0].innerText ?? "").Trim() : "");
				string sCity = ((city.Count > 0) ? (city[0].innerText ?? "").Trim() : "");
				di.Address = string.Concat(new string[] { sStreet, ", ", sPc, " ", sCity }).Trim().Trim(new char[] { ',', ' ' });
				di.PostalCode = sPc;
				di.City = sCity;
				di.Country = "Deutchland";
			}
			else
			{
				List<Element> addrA = WebScraper.QuerySelectorAll(WB, card, "a.addr[title]");
				if (addrA.Count > 0)
				{
					string t = ((addrA[0]["title"] ?? "").ToString() ?? "").Trim();
					Match i = Regex.Match(t, "^\\s*(.+?)\\s*,\\s*(\\d{5})\\s+(.+?)\\s*$");
					if (i.Success)
					{
						string street2 = i.Groups[1].Value.Trim();
						di.PostalCode = i.Groups[2].Value.Trim();
						di.City = i.Groups[3].Value.Trim();
						di.Address = string.Concat(new string[] { street2, ", ", di.PostalCode, " ", di.City });
						di.Country = "Deutchland";
					}
					else
					{
						di.Address = t;
					}
				}
			}
			di.Phone = "";
			try
			{
				List<Element> tel = WebScraper.QuerySelectorAll(WB, card, "[itemprop='telephone']");
				if (tel.Count > 0)
				{
					di.Phone = (tel[0].innerText ?? "").Trim();
				}
				if (string.IsNullOrWhiteSpace(di.Phone))
				{
					List<Element> nr = WebScraper.QuerySelectorAll(WB, card, "div.nr");
					if (nr.Count > 0)
					{
						Match j = Regex.Match(nr[0].innerHTML ?? "", "phoneTo:\\s*([0-9\\+\\(\\)\\s\\-\\/]+)", RegexOptions.IgnoreCase);
						if (j.Success)
						{
							di.Phone = j.Groups[1].Value.Trim();
						}
						else
						{
							string t2 = (nr[0].innerText ?? "").Trim();
							t2 = t2.Replace("Tel.", "").Replace("Tel", "").Trim();
							t2 = Regex.Replace(t2, "\\s+", " ");
							di.Phone = t2;
						}
					}
				}
				if (string.IsNullOrWhiteSpace(di.Phone))
				{
					List<Element> slog = WebScraper.QuerySelectorAll(WB, card, "div.slogan");
					if (slog.Count > 0)
					{
						Match m2 = Regex.Match((slog[0].innerText ?? "").Trim(), "(\\+?\\d[\\d\\s\\-\\/]{6,})");
						if (m2.Success)
						{
							di.Phone = m2.Groups[1].Value.Trim();
						}
					}
				}
				if (!string.IsNullOrWhiteSpace(di.Phone))
				{
					di.Phone = Regex.Replace(di.Phone, "\\s+", " ").Trim();
				}
			}
			catch
			{
			}
			List<Element> web = WebScraper.QuerySelectorAll(WB, card, "div.url a[href]");
			if (web.Count > 0)
			{
				try
				{
					di.Website = (web[0]["href"] ?? "").ToString().Trim();
				}
				catch
				{
				}
			}
			List<Element> cat = WebScraper.QuerySelectorAll(WB, card, "div.category");
			if (cat.Count > 0)
			{
				string c = (cat[0].innerText.Split(new char[] { ',' })[0] ?? "").Trim();
				di.Category = c.Replace("Branche:", "").Trim();
			}
			return di;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000C984 File Offset: 0x0000AB84
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 3;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			BUCHDataScraper[] Pool = new BUCHDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new BUCHDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						BUCHLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new BUCHDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000CA90 File Offset: 0x0000AC90
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[10].Value = BUCHLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00002A43 File Offset: 0x00000C43
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

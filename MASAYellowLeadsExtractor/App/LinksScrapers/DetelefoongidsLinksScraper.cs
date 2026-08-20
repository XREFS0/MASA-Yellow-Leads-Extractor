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
	// Token: 0x0200002C RID: 44
	public class DetelefoongidsLinksScraper
	{
		// Token: 0x0600014D RID: 333 RVA: 0x00017348 File Offset: 0x00015548
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element nextButton = null;
			for (;;)
			{
				List<DataItem> pageItems = new List<DataItem>();
				foreach (Element hItem in WebScraper.QuerySelectorAll(WB, "div.result-item__info"))
				{
					DataItem di = new DataItem();
					List<Element> titleH2 = WebScraper.QuerySelectorAll(WB, hItem, "h2[itemprop='name']");
					if (titleH2.Count == 0)
					{
						titleH2 = WebScraper.QuerySelectorAll(WB, hItem, "h2");
					}
					if (titleH2.Count > 0)
					{
						try
						{
							di.BusinessName = (titleH2[0].innerText ?? "").Trim();
						}
						catch
						{
						}
					}
					List<Element> titleLink = WebScraper.QuerySelectorAll(WB, hItem, "a.inline-flex[href]");
					if (titleLink.Count > 0)
					{
						try
						{
							string href = (titleLink[0]["href"] ?? "").ToString();
							if (!string.IsNullOrEmpty(href))
							{
								if (href.StartsWith("http", StringComparison.OrdinalIgnoreCase))
								{
									di.DetailsLink = href;
								}
								else
								{
									Uri baseUri = new Uri(WB.Url);
									di.DetailsLink = new Uri(baseUri, href).ToString();
								}
							}
						}
						catch
						{
						}
					}
					List<Element> WPE = WebScraper.QuerySelectorAll(WB, hItem, "div.mt-auto");
					if (WPE.Count > 0)
					{
						List<string[]> Parts = HTTPScraper.ParseHTML(WPE[0].innerHTML, "data-js-value=\"+(.*?)\" data-js-event=\"call\"");
						if (Parts.Count > 0)
						{
							di.Phone = Parts[0][1];
						}
					}
					List<Element> addrLi = WebScraper.QuerySelectorAll(WB, hItem, "li[itemprop='address']");
					if (addrLi.Count > 0)
					{
						try
						{
							di.Address = (addrLi[0].innerText ?? "").Trim();
							List<string[]> parts = HTTPScraper.ParseHTML(di.Address, "(\\d{4})([A-Z]{2})\\s+(\\p{L}+(?:[\\s\\-]\\p{L}+)*)");
							if (parts.Count > 0)
							{
								di.PostalCode = parts[0][1] + parts[0][2];
								di.City = parts[0][3];
							}
							di.Country = "Holland";
						}
						catch
						{
						}
					}
					List<Element> cat = WebScraper.QuerySelectorAll(WB, hItem, "div.mb-2\\.5 span.leading-4");
					if (cat.Count == 0)
					{
						cat = WebScraper.QuerySelectorAll(WB, hItem, "span.leading-4");
					}
					if (cat.Count > 0)
					{
						try
						{
							di.Category = (cat[0].innerText ?? "").Replace(",", "").Trim();
						}
						catch
						{
						}
					}
					List<Element> actionsRoot = WebScraper.QuerySelectorAll(WB, hItem, ".profile-actions__result, .profile-actions");
					if (actionsRoot.Count > 0)
					{
						Element root = actionsRoot[0];
						List<Element> web = WebScraper.QuerySelectorAll(WB, root, "a[data-js-event='link__'][href]");
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
						List<string> emails = new List<string>();
						foreach (Element i in WebScraper.QuerySelectorAll(WB, actionsRoot[0], "[data-js-event='mail'], [data-js-event='email']"))
						{
							try
							{
								string v = "";
								try
								{
									v = (i["data-js-value"] ?? "").ToString().Trim();
								}
								catch
								{
									v = "";
								}
								if (!string.IsNullOrEmpty(v) && !v.Equals("undefined", StringComparison.OrdinalIgnoreCase))
								{
									if (v.Contains("@"))
									{
										emails.Add(v);
									}
								}
								else
								{
									string t = (i.innerText ?? "").Trim();
									if (!string.IsNullOrEmpty(t) && t.Contains("@"))
									{
										emails.Add(t);
									}
								}
							}
							catch
							{
							}
						}
						if (emails.Count == 0)
						{
							try
							{
								Match j = Regex.Match(actionsRoot[0].innerHTML ?? "", "[A-Z0-9._%+\\-]+@[A-Z0-9.\\-]+\\.[A-Z]{2,}", RegexOptions.IgnoreCase);
								if (j.Success)
								{
									emails.Add(j.Value);
								}
							}
							catch
							{
							}
						}
						if (emails.Count > 0)
						{
							di.Email = emails[0];
						}
					}
					if (!string.IsNullOrEmpty(di.DetailsLink))
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
				DetelefoongidsLinksScraper.GetData(ref pageItems, mainForm);
				mainForm.tsProgress.Value = 0;
nextButton = null;
				List<Element> nextCandidates = WebScraper.QuerySelectorAll(WB, "a[rel='next'], a.next, a[aria-label*='Next'], a[aria-label*='Volgende']");
				if (nextCandidates.Count == 0)
				{
					nextCandidates = WebScraper.QuerySelectorAll(WB, "a[class*='justify-center'][class*='h-8'][href]");
				}
				if (nextCandidates.Count > 0)
				{
					nextButton = nextCandidates[0];
					try
					{
						string href2 = (nextButton["href"] ?? "").ToString();
						if (!string.IsNullOrEmpty(href2))
						{
							if (!href2.StartsWith("http", StringComparison.OrdinalIgnoreCase))
							{
								href2 = new Uri(new Uri(WB.Url), href2).ToString();
							}
							WB.LoadUrlAndWait(href2);
							for (int k = 0; k < 10; k++)
							{
								Thread.Sleep(100);
								Application.DoEvents();
							}
						}
						else
						{
							nextButton = null;
						}
					}
					catch
					{
						nextButton = null;
					}
				}
				if (Program.IsStopped() || nextButton == null)
				{
					goto IL_0643;
				}
			}

			IL_0643:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00017AC0 File Offset: 0x00015CC0
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			if (Program.AppSettings.IsRandomDelay)
			{
				PoolSize = 1;
			}
			LocalDataScraper[] Pool = new LocalDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new LocalDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						DetelefoongidsLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						if (ScraperIndex < DataItems.Count)
						{
							Pool[j] = new LocalDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x0600014F RID: 335 RVA: 0x00017BCC File Offset: 0x00015DCC
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[8].Value = DetelefoongidsLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[10].Value = DetelefoongidsLinksScraper.ClearValue(Item.Email);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00002A43 File Offset: 0x00000C43
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

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASAYellowLeadsExtractor.LinksScrapers
{
	// Token: 0x02000033 RID: 51
	internal class TrovanumeriLinksScraper
	{
		// Token: 0x0600016B RID: 363 RVA: 0x0001A778 File Offset: 0x00018978
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<Element> Tables = WebScraper.GetElementsByTag(WB, "table");
				int IterNbr = 0;
				int MaxIterNbr = 20;
				while (Tables.Count == 0 && IterNbr < MaxIterNbr)
				{
					Application.DoEvents();
					Thread.Sleep(100);
					Tables = WebScraper.GetElementsByTag(WB, "table");
					IterNbr++;
				}
				if (IterNbr == MaxIterNbr)
				{
					break;
				}
				foreach (Element Table in Tables)
				{
					if (Table.innerText != null && Table.innerText.Length >= 500 && !(Table["width"].ToString() != "340"))
					{
						List<Element> TableRows = WebScraper.GetElements(WB, Table, "tr");
						for (int RowIndex = 0; RowIndex < TableRows.Count; RowIndex++)
						{
							if (TableRows[RowIndex].outerHTML.IndexOf("<a href=\"/?azione=dettagli") > -1)
							{
								DataItem DataItem = new DataItem();
								try
								{
									DataItem.BusinessName = TableRows[RowIndex].innerText.Replace("\r\n", "").Trim();
								}
								catch
								{
								}
								try
								{
									List<Element> Children = WebScraper.GetChildren(WB, TableRows[RowIndex]);
									List<Element> Items = WebScraper.GetElementsByTag(WB, Children[0], "a");
									if (Items.Count > 0)
									{
										DataItem.DetailsLink = Items[0]["href"].ToString();
										DataItem.MapLink = Items[0]["href"].ToString();
									}
								}
								catch
								{
								}
								DataItem.Category = "";
								DataItem.Address = "";
								if (TableRows[RowIndex + 4].innerText != null)
								{
									DataItem dataItem = DataItem;
									dataItem.Address += TableRows[RowIndex + 4].innerText.Replace("\r\n", "").Trim();
								}
								if (TableRows[RowIndex + 1].innerText != null)
								{
									if (DataItem.Address != "")
									{
										DataItem dataItem2 = DataItem;
										dataItem2.Address += ", ";
									}
									DataItem dataItem3 = DataItem;
									dataItem3.Address += TableRows[RowIndex + 1].innerText.Replace("\r\n", "").Trim();
								}
								if (DataItem.Address != "")
								{
									List<string[]> aIndex = HTTPScraper.ParseHTML(DataItem.Address, "(.*?)(\\d{4,5}) (.*) \\((.*?)\\)");
									if (aIndex.Count > 0)
									{
										try
										{
											DataItem.Address = aIndex[0][1].Remove(aIndex[0][1].Length - 2);
											DataItem.City = aIndex[0][3];
											DataItem.State = aIndex[0][4];
											DataItem.PostalCode = aIndex[0][2];
										}
										catch
										{
										}
									}
									DataItem.Country = "Italy";
								}
								if (TableRows[RowIndex + 2].innerText != null && TableRows[RowIndex + 2].innerText.IndexOf("Tel:") > -1)
								{
									DataItem.Phone = "+39" + TableRows[RowIndex + 2].innerText.Replace("Tel:", "").Replace("\r\n", "").Trim();
								}
								if (TableRows[RowIndex + 5].innerText != null && TableRows[RowIndex + 5].innerText.IndexOf("@") > -1)
								{
									DataItem.Email = TableRows[RowIndex + 5].innerText.Replace("\r\n", "").Trim();
								}
								if (DataItem.DetailsLink != null && DataItem.DetailsLink != "")
								{
									mainForm.dgvResults.Rows.Add(new object[]
									{
										DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
										DataItem.Email, DataItem.MapLink, DataItem.DetailsLink
									});
									mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
									mainForm.tssLabelListed.Invalidate();
									int Val = 100 * mainForm.dgvResults.Rows.Count / 35;
									if (Val < 100)
									{
										mainForm.tsProgress.Value = Val;
									}
								}
if (Program.IsStopped())
								{
									return;
								}
								Application.DoEvents();
							}
						}
					}
				}
Program.RequestDelay();
				List<Element> NextButtons = WebScraper.GetElementsByTag(WB, "table");
				if (NextButtons.Count > 0)
				{
					NextButton = null;
					foreach (Element btn in NextButtons)
					{
						try
						{
							string html = btn.outerHTML;
							if (html != null)
							{
								int avanti = html.IndexOf("Avanti");
								if (avanti != -1)
								{
									int lastHref = html.Substring(0, avanti).LastIndexOf("<a href=\"");
									if (lastHref != -1)
									{
										int start = lastHref + 9;
										int end = html.IndexOf("\"", start);
										if (end != -1)
										{
											string NewUrl = WebUtility.HtmlDecode(html.Substring(start, end - start));
											if (NewUrl.IndexOf("da=") > -1)
											{
												NextButton = btn;
												WB.LoadUrlAndWait("https://www.trovanumeri.com" + NewUrl);
												break;
											}
										}
									}
								}
							}
						}
						catch
						{
						}
					}
				}
				if (NextButton == null)
				{
					goto Block_8;
				}
			}
			return;
			Block_5:

			mainForm.tsProgress.Value = 0;
			return;
			Block_8:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0001AE90 File Offset: 0x00019090
		public static string GetLink(Element BlockTitle)
		{
			string Url = BlockTitle["href"].ToString();
			if (Url.IndexOf("#") > -1)
			{
				List<string[]> Items = HTTPScraper.ParseHTML(BlockTitle["data-pjlb"].ToString(), "\"url\":\"(.*?)\"");
				if (Items.Count > 0)
				{
					try
					{
						Url = BitConverter.ToString(Convert.FromBase64String(Items[0][1]));
						string[] array = Url.Split(new char[] { '-' });
						Url = "";
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							int decValue = Convert.ToInt32(array2[i], 16);
							Url += Convert.ToChar(decValue).ToString();
						}
					}
					catch
					{
						Url = "";
					}
				}
			}
			return Url;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0001AF5C File Offset: 0x0001915C
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				if (mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = TrovanumeriLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[2].Value = TrovanumeriLinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = TrovanumeriLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = TrovanumeriLinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = TrovanumeriLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = TrovanumeriLinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = TrovanumeriLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = TrovanumeriLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[10].Value = TrovanumeriLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = TrovanumeriLinksScraper.ClearValue(Item.MapLink);
					if (ProgressValue < 100f)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					return;
				}
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00002A43 File Offset: 0x00000C43
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

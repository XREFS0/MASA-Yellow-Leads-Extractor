using System;
using System.Collections.Generic;

namespace MASAYellowLeadsExtractor
{
	// Token: 0x02000008 RID: 8
	public static class PhoneMiner
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00003448 File Offset: 0x00001648
		public static string GetPhone(string Url, string[] ContactPageUrls)
		{
			string Phone = "";
			if (Url == null || Url == "")
			{
				return Phone;
			}
			Url = Url.Replace("https:", "http:");
			string ClearPage = HTTPScraper.ClearString(HTTPScraper.GetPage(Url, null));
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "([+]?\\d[ ]?[(]?\\d{3}[)]?[ ]?\\d{2,3}[- ]?\\d{2,3}[- ]?\\d{2,3})");
			if (Items.Count > 0 && (Items[0][1].IndexOf("+") > -1 || Items[0][1].IndexOf("-") > -1))
			{
				Phone = PhoneMiner.FindCorrectPhone(Items);
			}
			else
			{
				Items = HTTPScraper.ParseHTML(ClearPage, "href=(\"|'|)(.*?)(\"|'|)[>|\\s]");
				foreach (string[] PageUrl in Items)
				{
					foreach (string ContactPageUrl in ContactPageUrls)
					{
						if (PageUrl[2].IndexOf(ContactPageUrl, StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							string ContactsPageUrl = PageUrl[2];
							if (ContactsPageUrl.IndexOf("http") == -1)
							{
								if (ContactsPageUrl[0] == '/')
								{
									ContactsPageUrl = "http://" + Url.TrimEnd(new char[] { '/' }) + ContactsPageUrl;
								}
								else
								{
									ContactsPageUrl = "http://" + Url.TrimEnd(new char[] { '/' }).Replace("http://", "") + "/" + ContactsPageUrl;
								}
							}
							else
							{
								string WebsiteName = Url.Replace("http://", "").Replace("https://", "").Replace("www.", "");
								WebsiteName = WebsiteName.Split(new char[] { '/' })[0];
								if (ContactsPageUrl.IndexOf(WebsiteName) == -1)
								{
									ContactsPageUrl = "";
								}
							}
							if (ContactsPageUrl != "")
							{
								ClearPage = HTTPScraper.ClearString(HTTPScraper.GetPage(ContactsPageUrl, null));
								Items = HTTPScraper.ParseHTML(ClearPage, "([+]?\\d[ ]?[(]?\\d{3}[)]?[ ]?\\d{2,3}[- ]?\\d{2,3}[- ]?\\d{2,3})");
								if (Items.Count > 0 && (Items[0][1].IndexOf("+") > -1 || Items[0][1].IndexOf("-") > -1))
								{
									Phone = PhoneMiner.FindCorrectPhone(Items);
								}
							}
						}
					}
				}
			}
			return Phone;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000369C File Offset: 0x0000189C
		private static string FindCorrectPhone(List<string[]> Items)
		{
			foreach (string[] phone in Items)
			{
				if (phone[0].IndexOf("@mail.com", StringComparison.InvariantCultureIgnoreCase) == -1 && phone[0].IndexOf("example", StringComparison.InvariantCultureIgnoreCase) == -1 && phone[0].IndexOf(".png", StringComparison.InvariantCultureIgnoreCase) == -1)
				{
					return phone[0];
				}
			}
			return "";
		}
	}
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MASAYellowLeadsExtractor
{
	public static class EmailMiner
	{
		public static string GetEmail(string Url, string[] ContactPageUrls)
		{
			return EmailMiner.GetEmailAsync(Url, ContactPageUrls, TimeSpan.FromSeconds(5.0)).GetAwaiter().GetResult();
		}

		private static async Task<string> GetEmailAsync(string Url, string[] ContactPageUrls, TimeSpan globalTimeout)
		{
			if (string.IsNullOrWhiteSpace(Url))
			{
				return "";
			}
			Url = Url.Replace("http:", "https:");
			using (CancellationTokenSource cts = new CancellationTokenSource(globalTimeout))
			{
				string page = await EmailMiner.GetPageWithTimeout(Url, cts.Token).ConfigureAwait(false);
				if (page == null)
				{
					return "";
				}
				string clearPage = HTTPScraper.ClearString(page);
				List<string[]> items = HTTPScraper.ParseHTML(clearPage, "(mailto\\:|)([\\w\\.\\-]+)@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))");
				if (items.Count > 0)
				{
					return EmailMiner.FindCorrectEmail(items).Replace("mailto:", "");
				}
				items = HTTPScraper.ParseHTML(clearPage, "href=(\"|'|)(.*?)(\"|'|)[>|\\s]");
				foreach (string[] pageUrlArr in items)
				{
					if (cts.IsCancellationRequested)
					{
						return "";
					}
					foreach (string contactKey in ContactPageUrls)
					{
						if (cts.IsCancellationRequested)
						{
							return "";
						}
						if (pageUrlArr[2].IndexOf(contactKey, StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							string contactsPageUrl = pageUrlArr[2];
							if (contactsPageUrl.IndexOf("http", StringComparison.InvariantCultureIgnoreCase) == -1)
							{
								contactsPageUrl = ((contactsPageUrl.Length <= 0 || contactsPageUrl[0] != '/') ? ("http://" + Url.TrimEnd(new char[] { '/' }).Replace("http://", "") + "/" + contactsPageUrl) : ("http://" + Url.TrimEnd(new char[] { '/' }) + contactsPageUrl));
							}
							else if (contactsPageUrl.IndexOf(Url.Replace("http://", "").Replace("https://", "").Replace("www.", "")
								.Split(new char[] { '/' })[0], StringComparison.InvariantCultureIgnoreCase) == -1)
							{
								contactsPageUrl = "";
							}
							if (!string.IsNullOrEmpty(contactsPageUrl))
							{
								page = await EmailMiner.GetPageWithTimeout(contactsPageUrl, cts.Token).ConfigureAwait(false);
								if (page == null)
								{
									return "";
								}
								clearPage = HTTPScraper.ClearString(page);
								items = HTTPScraper.ParseHTML(clearPage, "(mailto\\:|)([\\w\\.\\-]+)@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))");
								if (items.Count > 0)
								{
									return EmailMiner.FindCorrectEmail(items).Replace("mailto:", "");
								}
							}
						}
					}
				}
				return "";
			}
		}

		private static async Task<string> GetPageWithTimeout(string url, CancellationToken ct)
		{
			try
			{
				Task<string> t = Task.Run<string>(() => HTTPScraper.GetPage(url, null), ct);
				Task completed = await Task.WhenAny(new Task[]
				{
					t,
					Task.Delay(-1, ct)
				}).ConfigureAwait(false);
				if (completed != t)
				{
					return null;
				}
				return await t.ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error retrieving page (" + url + "): " + ex.Message);
				return null;
			}
		}

		private static string FindCorrectEmail(List<string[]> Items)
		{
			foreach (string[] email in Items)
			{
				if (email[0].IndexOf("@mail.com", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf("example", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf(".jpg", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf(".wiz", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf(".wix", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf(".png", StringComparison.InvariantCultureIgnoreCase) == -1)
				{
					return email[0];
				}
			}
			return "";
		}
	}
}

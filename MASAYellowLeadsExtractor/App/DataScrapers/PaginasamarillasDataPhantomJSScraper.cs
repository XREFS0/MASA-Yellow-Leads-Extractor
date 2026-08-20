using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace MASAYellowLeadsExtractor.DataScrapers
{
	// Token: 0x0200005C RID: 92
	public class PaginasamarillasDataPhantomJSScraper
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x000249CC File Offset: 0x00022BCC
		public PaginasamarillasDataPhantomJSScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00024A38 File Offset: 0x00022C38
		public void ProcessPage()
		{
			HTTPScraper.GetPage(this.PageUrl, null);
			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true,
				FileName = "phantomjs.exe",
				StandardOutputEncoding = Encoding.GetEncoding(850),
				Arguments = string.Format("--output-encoding=cp850 --script-encoding=utf8 \"{0}\\{1}\" {2}", Directory.GetCurrentDirectory(), "phantomjs-script.js", this.PageUrl)
			};
			process.StartInfo = startInfo;
			process.Start();
			string PhantomJSResponse = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			List<string[]> SubItems = HTTPScraper.ParseHTML(PhantomJSResponse, "<a href=\"tel:(.*?)\" title=\"#\" class=\"phone");
			if (SubItems.Count > 0)
			{
				for (int i = 1; i < SubItems.Count; i++)
				{
					DataItem data = this.Data;
					data.Phone = data.Phone + SubItems[i][1] + ", ";
				}
			}
			if (this.Data.Phone != null && this.Data.Phone.Length > 2)
			{
				this.Data.Phone = this.Data.Phone.Substring(0, this.Data.Phone.Length - 2);
			}
			if (Program.AppSettings.ExtractEmails)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
			}
			this.IsDone = true;
		}

		// Token: 0x04000195 RID: 405
		private Thread MainThread;

		// Token: 0x04000196 RID: 406
		private string PageUrl;

		// Token: 0x04000197 RID: 407
		private Settings AppSettings;

		// Token: 0x04000198 RID: 408
		private List<ProxyServer> Proxies;

		// Token: 0x04000199 RID: 409
		public bool IsDone;

		// Token: 0x0400019A RID: 410
		public DataItem Data;
	}
}

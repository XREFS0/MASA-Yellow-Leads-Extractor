using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using EO.Base;
using EO.WebBrowser;
using EO.WebEngine;
using EO.WinForm;
using MASAYellowLeadsExtractor.LinksScrapers;

namespace MASAYellowLeadsExtractor
{
	// Token: 0x0200000D RID: 13
	public partial class MainForm : KryptonForm
	{
		// Token: 0x06000049 RID: 73 RVA: 0x000049B4 File Offset: 0x00002BB4
		public MainForm()
		{
			this.InitializeComponent();
			ApplyGoldDarkTheme();
			try
			{
				string icoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "YellowLeadsExtractor.ico");
				if (System.IO.File.Exists(icoPath))
					this.Icon = new Icon(icoPath);
			}
			catch { }
			base.WindowState = FormWindowState.Maximized;
			Program.AppSettings = Settings.Load(Program.SettingsFileName);
			Program.LanguagesManager.InitFields(Program.LanguagesFiles[Program.AppSettings.Language]);
			Program.LanguagesManager.InitControl(this, base.Controls);
			Program.LanguagesManager.InitMenu(this);
			Program.LanguagesManager.InitTableColumns(this.dgvResults);
			this.ProxyServers = null;
			this.dgvResults.Columns[9].DefaultCellStyle.ForeColor = Color.Blue;
			this.dgvResults.Columns[10].DefaultCellStyle.ForeColor = Color.Blue;
			this.dgvResults.Columns[11].DefaultCellStyle.ForeColor = Color.Blue;
			this.dgvResults.Columns[12].DefaultCellStyle.ForeColor = Color.Blue;
			EO.WebBrowser.Runtime.AddLicense("t8TbrmuntsXNn6zs5tYj76Lp6QTs83aZtcDer2iptMPgoVnt6QMe6KjlwbPdsluXs8+4iVmXpLHn8qLe8vIf9KvcwsQW6LHvuQXf9aHk7MAE7Ybm0QQj5aC0wc3a8qLe8vIf9Kvcwp61u2jj7fQQ7azcwp61dePt9BDtrNzCnrWfWZekzRfonNzyBBDInbW1xuywcqu9xOzUcau1w9yvg7Oz/RTinuX39vTjd4SOscufWbPw+g7kp+rp9um7aOPt9BDtrNzpz7iJWZeksefgpePzCOmMQ5ekscufWZekzQzjnZf4ChvkdpnJ4NnCoenz/hChWe3pAx7oqOXBs92zZ6emsdq9RoGkscufdabl/RfusLWRm8ufWZfAAB3jnunN/xHuWdvlBRC8W6iz");
			this.webView.Engine.Options.CustomUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36";
			this.webView.CertificateError += this.webview_CertificateError;
			string localPage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "masa-page.html");
			if (System.IO.File.Exists(localPage))
				this.webView.LoadUrl("file:///" + localPage.Replace("\\", "/"));
			else
				this.webView.LoadUrl("about:blank");
			this.webView.CertificateError += this.webview_CertificateError;
			EO.Base.Runtime.EnableEOWP = true;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00005034 File Offset: 0x00003234
		private void WebViewEngine_Started(object sender, EventArgs e)
		{
			if (this.webView.Engine.State == EngineState.Running)
			{
				this.SetCookies();
				return;
			}
			this.webView.Engine.Started += delegate(object sender1, EngineEventArgs args)
			{
				if (this.webView.Engine.State == EngineState.Running)
				{
					this.SetCookies();
				}
			};
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000506C File Offset: 0x0000326C
		private void SetCookies()
		{
			CookieManager cookieManager = this.webView.Engine.CookieManager;
			string url = "https://www.pagesjaunes.fr";
			Cookie cookie = new Cookie("clientId=1002312; event.origin:google.com; sessionState=true");
			cookieManager.SetCookie(url, cookie);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000050A2 File Offset: 0x000032A2
		private void webview_CertificateError(object sender, CertificateErrorEventArgs e)
		{
			e.Continue();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000516C File Offset: 0x0000336C
		private void NavigateSource()
		{
			this.tbWebbrowserUrl.Text = this.BaseScraperUrl;
			this.webView.CertificateError += this.webview_CertificateError;
			this.webView.LoadUrlAndWait(this.BaseScraperUrl);
			this.webView.CertificateError += this.webview_CertificateError;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000051C9 File Offset: 0x000033C9
		private void btnBrowserHome_Click(object sender, EventArgs e)
		{
			string localHome = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "masa-page.html");
			if (System.IO.File.Exists(localHome))
				this.webView.LoadUrl("file:///" + localHome.Replace("\\", "/"));
			else
				this.webView.LoadUrl("about:blank");
			this.webView.CertificateError += this.webview_CertificateError;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000051F3 File Offset: 0x000033F3
		private void btnWebbrowserBack_Click(object sender, EventArgs e)
		{
			this.webView.GoBack();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00005201 File Offset: 0x00003401
		private void btnWebbrowserForward_Click(object sender, EventArgs e)
		{
			this.webView.GoForward();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000520F File Offset: 0x0000340F
		private void btnWebbrowserRefresh_Click(object sender, EventArgs e)
		{
			this.webView.Reload();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000521D File Offset: 0x0000341D
		private void btnWebbrowserStop_Click(object sender, EventArgs e)
		{
			this.webView.StopLoad();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000522A File Offset: 0x0000342A
		private void bnWebrowserGo_Click(object sender, EventArgs e)
		{
			this.webView.LoadUrl(this.tbWebbrowserUrl.Text);
			this.webView.CertificateError += this.webview_CertificateError;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000525A File Offset: 0x0000345A
		private void wwwpaginegialleitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.paginegialle.it";
			this.NavigateSource();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000526D File Offset: 0x0000346D
		private void wwwpagesjaunesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.NavigateSource();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005275 File Offset: 0x00003475
		private void wwwpaginasamarillasesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.paginasamarillas.es";
			this.NavigateSource();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005288 File Offset: 0x00003488
		private void wwwgelbenseitendeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.gelbeseiten.de";
			this.NavigateSource();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000529B File Offset: 0x0000349B
		private void aziendevirgilioitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://aziende.virgilio.it";
			this.NavigateSource();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000052AE File Offset: 0x000034AE
		private void wwwyellowpagescomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://www.yellowpages.com/";
			this.NavigateSource();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000052C1 File Offset: 0x000034C1
		private void wwwlocalchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.local.ch";
			this.NavigateSource();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000052D4 File Offset: 0x000034D4
		private void wwwyellowpagescaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellowpages.ca";
			this.NavigateSource();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000052E7 File Offset: 0x000034E7
		private void wwwyellcomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yell.com";
			this.NavigateSource();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000052FA File Offset: 0x000034FA
		private void wwwgoldenpagesbeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.1307.be";
			this.NavigateSource();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000530D File Offset: 0x0000350D
		private void wwwheroldatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.herold.at";
			this.NavigateSource();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005320 File Offset: 0x00003520
		private void wwwyellowpagesplToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellowpages.pl";
			this.NavigateSource();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005333 File Offset: 0x00003533
		private void wwwtrovanumericomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.trovanumeri.com";
			this.NavigateSource();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005346 File Offset: 0x00003546
		private void wwwinfobelcomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.infobel.com";
			this.NavigateSource();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005359 File Offset: 0x00003559
		private void wwwpaginiauriiroToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.paginiaurii.ro";
			this.NavigateSource();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000536C File Offset: 0x0000356C
		private void wwwyellowpagesalbaniacomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellowpagesalbania.com";
			this.NavigateSource();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000537F File Offset: 0x0000357F
		private void wwwdetelefoongidsnlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.goudengids.nl";
			this.NavigateSource();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005392 File Offset: 0x00003592
		private void AUToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.guiamais.com.br";
			this.NavigateSource();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000053A5 File Offset: 0x000035A5
		private void AUSToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellowpages.com.au";
			this.NavigateSource();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000053B8 File Offset: 0x000035B8
		private void wwwypruToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellow-pages.ph/";
			this.NavigateSource();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000053CB File Offset: 0x000035CB
		private void KompassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yelp.com";
			this.NavigateSource();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000053DE File Offset: 0x000035DE
		private void YelpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.oeffnungszeitenbuch.de";
			this.NavigateSource();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000053F1 File Offset: 0x000035F1
		private void YelpITToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.oraridiapertura24.it";
			this.NavigateSource();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00005404 File Offset: 0x00003604
		private void YelpCZToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.tuttocitta.it";
			this.NavigateSource();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005417 File Offset: 0x00003617
		private void YelpESToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://yellow.co.nz";
			this.NavigateSource();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000542A File Offset: 0x0000362A
		private void YelpPTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.11880.com";
			this.NavigateSource();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000543D File Offset: 0x0000363D
		private void YelpSEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.gulesider.no/";
			this.NavigateSource();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005450 File Offset: 0x00003650
		private void YelpFRToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.horairesdouverture24.fr";
			this.NavigateSource();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005463 File Offset: 0x00003663
		private void PakiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.lookup.pk";
			this.NavigateSource();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005476 File Offset: 0x00003676
		private void JustToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://yellowpages.in/";
			this.NavigateSource();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005489 File Offset: 0x00003689
		private void COZAToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://yep.co.za";
			this.NavigateSource();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000549C File Offset: 0x0000369C
		private void UAEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellowpages-uae.com";
			this.NavigateSource();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000054AF File Offset: 0x000036AF
		private void BUCHToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.dastelefonbuch.de";
			this.NavigateSource();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000054C2 File Offset: 0x000036C2
		private void LATToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.paginasamarillas.com";
			this.NavigateSource();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000054D5 File Offset: 0x000036D5
		private void EGToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellowpages.com.eg/en/related-categories";
			this.NavigateSource();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000054E8 File Offset: 0x000036E8
		private void PaiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.pai.pt";
			this.NavigateSource();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000054FB File Offset: 0x000036FB
		private void XoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.xo.gr";
			this.NavigateSource();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000550E File Offset: 0x0000370E
		private void GTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://www.guiatel.es";
			this.NavigateSource();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00005521 File Offset: 0x00003721
		private void GoldenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://www.goldenpages.ie";
			this.NavigateSource();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005534 File Offset: 0x00003734
		private void CZToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://www.zlatestranky.cz";
			this.NavigateSource();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00005548 File Offset: 0x00003748
		private void webView_LoadCompleted(object sender, LoadCompletedEventArgs e)
		{
			string webBrowserUrl = "";
			try
			{
				webBrowserUrl = this.webView.Url.ToString();
			}
			catch
			{
				MessageBox.Show("Can't get URL from web browser!");
			}
			for (int i = 0; i < this.BaseScraperUrls.Length; i++)
			{
				if (webBrowserUrl.ToString().IndexOf(this.BaseScraperUrls_[i]) > -1)
				{
					this.BaseScraperUrl = this.BaseScraperUrls[i];
					return;
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000032E1 File Offset: 0x000014E1
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000055C4 File Offset: 0x000037C4
		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnExport_Click(null, null);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000055CE File Offset: 0x000037CE
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (MessageBox.Show(Program.LanguagesManager.ExitMessage, "MASA Yellow Leads Extractor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				e.Cancel = false;
				return;
			}
			e.Cancel = true;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000055FC File Offset: 0x000037FC
		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new SettingsForm().ShowDialog();
			Program.AppSettings = Settings.Load(Program.SettingsFileName);
			Program.LanguagesManager.InitFields(Program.LanguagesFiles[Program.AppSettings.Language]);
			Program.LanguagesManager.InitControl(this, base.Controls);
			Program.LanguagesManager.InitMenu(this);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000056A6 File Offset: 0x000038A6
		private void aboutOnlineYellowPagesScraperToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutForm().ShowDialog();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000056B4 File Offset: 0x000038B4
		private void btnGetData_Click(object sender, EventArgs e)
		{
			Program.StopDataCollection = false;
			if (Program.AppSettings.ConnectionType == 3)
			{
				ProxiesForm ProxiesForm = new ProxiesForm(Program.AppSettings.ProxySourcesList);
				ProxiesForm.ShowDialog();
				this.ProxyServers = ProxiesForm.ProxyServers;
				if (this.ProxyServers.Count == 0)
				{
					MessageBox.Show(Program.LanguagesManager.NoFreeProxiesMessage);
					return;
				}
			}
			this.GetLinks();
			if (this.dgvResults.Rows.Count == 0)
			{
				MessageBox.Show(Program.LanguagesManager.MakeSearchFirst);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000573D File Offset: 0x0000393D
		private void btnStop_Click(object sender, EventArgs e)
		{
			Program.StopDataCollection = true;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005745 File Offset: 0x00003945
		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			this.dgvResults.SelectAll();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005752 File Offset: 0x00003952
		private void btnClearSelection_Click(object sender, EventArgs e)
		{
			this.dgvResults.ClearSelection();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005760 File Offset: 0x00003960
		private void btnDeleteSelected_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(string.Format(Program.LanguagesManager.DeleteSomeRows, this.dgvResults.SelectedRows.Count), "Delete", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				foreach (object obj in this.dgvResults.SelectedRows)
				{
					DataGridViewRow item = (DataGridViewRow)obj;
					this.dgvResults.Rows.RemoveAt(item.Index);
				}
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005800 File Offset: 0x00003A00
		private void btnDeleteAll_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(string.Format(Program.LanguagesManager.DeleteAllRows, this.dgvResults.SelectedRows.Count), "Delete", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				this.dgvResults.Rows.Clear();
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005850 File Offset: 0x00003A50
		private void btnExport_Click(object sender, EventArgs e)
		{
			if (this.dgvResults.Rows.Count == 0)
			{
				MessageBox.Show(Program.LanguagesManager.NoDataToExport);
				return;
			}
			if (this.dgvResults.SelectedRows.Count == 0)
			{
				MessageBox.Show(Program.LanguagesManager.NoDataSelectedToExport);
				return;
			}
			Program.StopDataCollection = true;
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.ShowHelp = true;
			this.tssLabelStatus.Text = "Export is started...";
			Application.DoEvents();
			if (Program.AppSettings.ExportType == 0)
			{
				saveFileDialog.Filter = "CSV files|*.csv";
			}
			else if (Program.AppSettings.ExportType == 1)
			{
				saveFileDialog.Filter = "Text files|*.txt";
			}
			else if (Program.AppSettings.ExportType == 2)
			{
				saveFileDialog.Filter = "Excel files|*.xls";
			}
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.tssLabelStatus.Text = "Exporting data...";
				Application.DoEvents();
				if (Program.AppSettings.ExportType == 1)
				{
					ExportManager.SaveToText(Program.AppSettings, saveFileDialog.FileName, this.dgvResults);
				}
				else if (Program.AppSettings.ExportType == 0)
				{
					ExportManager.SaveToCSV(Program.AppSettings, saveFileDialog.FileName, this.dgvResults);
				}
				else if (Program.AppSettings.ExportType == 2)
				{
					ExportManager.SaveToXLS(Program.AppSettings, saveFileDialog.FileName, this.dgvResults);
				}
				this.tssLabelStatus.Text = "Done! Ready to work!";
				Application.DoEvents();
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000059C8 File Offset: 0x00003BC8
		private void GetLinks()
		{
			if (this.BaseScraperUrl.IndexOf("paginegialle") > -1)
			{
				PaginegialleLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("pagesjaunes") > -1)
			{
				PagesjaunesLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("paginasamarillas.es") > -1)
			{
				PaginasamarillasLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("gelbeseiten") > -1)
			{
				GelbenseitenLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("aziende.virgilio.it") > -1)
			{
				AziendeVirgilioLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yellowpages.com/") > -1)
			{
				YellowPagesLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("local") > -1 && this.BaseScraperUrl.IndexOf("cylex") == -1)
			{
				LocalLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yellowpages.ca") > -1)
			{
				YellowPagesCaLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yell.com") > -1)
			{
				YellLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("1307.be") > -1)
			{
				GoldenPagesLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("herold.at") > -1)
			{
				HeroldLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yellowpages.pl") > -1)
			{
				YellowPagesPlLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("trovanumeri.com") > -1)
			{
				TrovanumeriLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("infobel.com") > -1)
			{
				InfobelLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("paginiaurii.ro") > -1)
			{
				PaginiauriiROLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yellowpagesalbania.com") > -1)
			{
				YellowPagesAlbaniaLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("goudengids") > -1)
			{
				DetelefoongidsLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("guiamais.com.br") > -1)
			{
				AustraliaLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yellowpages.com.au") > -1)
			{
				AUSLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf(".ph") > -1)
			{
				YPRULinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yelp") > -1)
			{
				KompassLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("oeffnungszeitenbuch") > -1)
			{
				YelpLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("oraridiapertura24") > -1)
			{
				YelpITLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("tuttocitta.it") > -1)
			{
				YelpCZLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yellow.co.nz") > -1)
			{
				YelpESLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("11880.com") > -1)
			{
				YelpPTLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("degulesider.dk") > -1 || this.BaseScraperUrl.IndexOf("gulesider.no") > -1)
			{
				YelpSELinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("horairesdouverture24") > -1)
			{
				YelpFRLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("lookup.pk") > -1)
			{
				PakiLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yellowpages.in") > -1)
			{
				JustLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf(".za") > -1)
			{
				COZALinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yellowpages-uae.com") > -1)
			{
				UAELinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("dastelefonbuch.de") > -1)
			{
				BUCHLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("paginasamarillas.com") > -1 || this.BaseScraperUrl.IndexOf("amarillas.cl") > -1 || this.BaseScraperUrl.IndexOf("paginas-amarillas") > -1)
			{
				LATLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("zlatestranky") > -1)
			{
				CZLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("pai.pt") > -1)
			{
				PaiLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("xo.gr") > -1)
			{
				XoLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("yellowpages.com.eg") > -1)
			{
				EGLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("goldenpages.ie") > -1)
			{
				GoldenLinksScraper.GetLinks(this.webView, this);
			}
			if (this.BaseScraperUrl.IndexOf("cylex") > -1)
			{
				CylexLinksScraper.GetLinks(this.webView, this);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005EFC File Offset: 0x000040FC
		private void dgvResults_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex > -1 && e.ColumnIndex >= 9 && this.dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
			{
				try
				{
					string url = this.dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
					if (e.ColumnIndex == 10)
					{
						url = string.Format("mailto:{0}", url);
					}
					Process.Start(new ProcessStartInfo(url));
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005FB8 File Offset: 0x000041B8
		private void dgvResults_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex > 0 && e.ColumnIndex >= 9 && this.dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != "")
			{
				this.Cursor = Cursors.Hand;
				return;
			}
			this.Cursor = Cursors.Default;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000604E File Offset: 0x0000424E
		private void MainForm_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000604E File Offset: 0x0000424E
		private void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000604E File Offset: 0x0000424E
		private void tbWebbrowserUrl_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000604E File Offset: 0x0000424E
		private void yellcomUKToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0400002A RID: 42
		private bool engineStarted;

		// Token: 0x0400002B RID: 43
		private string BaseScraperUrl = "";

		// Token: 0x0400002C RID: 44
		public List<ProxyServer> ProxyServers;

		// Token: 0x0400002D RID: 45
		private string[] BaseScraperUrls = new string[]
		{
			"https://www.paginegialle.it", "https://www.pagesjaunes.fr", "https://www.paginasamarillas.es", "http://www.gelbeseiten.de", "https://aziende.virgilio.it", "http://www.yellowpages.com/", "https://www.local.ch", "https://www.yellowpages.ca", "http://www.yell.com", "https://www.1307.be/",
			"https://www.herold.at", "https://www.yellowpages.pl", "https://www.trovanumeri.com", "https://www.infobel.com", "https://www.infobel.com", "https://www.paginiaurii.ro", "https://www.yellowpagesalbania.com", "https://goudengids.nl", "https://www.guiamais.com.br", "https://www.yellowpages.com.au",
			"https://www.yellow-pages.ph", "https://www.yelp.com", "https://www.oeffnungszeitenbuch.de", "https://www.oraridiapertura24.it", "https://www.tuttocitta.it", "https://yellow.co.nz", "https://www.11880.com", "https://www.degulesider.dk", "https://www.horairesdouverture24.fr", "https://www.lookup.pk",
			"http://www.yellowpages.in", "http://www.yep.co.za/", "https://www.yellowpages-uae.com", "https://www.dastelefonbuch.de", "https://www.paginasamarillas.com", "http://www.guiatel.es/", "https://www.yellowpages.com.eg", "https://www.pai.pt", "https://www.xo.gr", "https://www.yelp.it",
			"https://www.yelp.de", "https://www.yelp.fr", "https://www.yelp.cz", "https://www.amarillas.cl", "https://www.goldenpages.ie", "https://www.zlatestranky.sk", "https://www.zlatestranky.cz", "https://www.gulesider.no", "https://www.paginas-amarillas.com", "https://www.yelp.se",
			"https://www.yelp.pt", "https://www.yelp.es", "https://www.cylex-locale.fr/", "https://www.cylex.net.za/", "https://www.cylex-australia.com/", "https://fr.cylex-belgie.be/", "https://www.cylex.com.br/", "https://web2.cylex.de/", "https://www.cylex-italia.it/", "https://www.cylex.nl/",
			"https://www.cylex.es/", "https://www.cylex-swiss.ch/", "https://www.cylex-uk.co.uk/", "https://www.cylex.cz/", "https://www.cylex.dk/", "https://www.cylex.hu/", "https://www.cylex.co.nz/", "https://www.cylex.no/", "https://www.cylex-polska.pl/", "https://www.cylex.se/",
			"https://www.cylex.us.com/"
		};

		// Token: 0x0400002E RID: 46
		private string[] BaseScraperUrls_ = new string[]
		{
			"paginegialle.it", "pagesjaunes.fr", "paginasamarillas.es", "gelbeseiten.de", "aziende.virgilio.it", "yellowpages.com/", "local.ch", "yellowpages.ca", "yell.com", "1307.be",
			"herold.at", "yellowpages.pl", "trovanumeri.com", "infobel.com", "us-info.com", "paginiaurii.ro", "yellowpagesalbania.com", "goudengids.nl", "guiamais.com.br", "yellowpages.com.au",
			"yellow-pages.ph", "yelp.com", "oeffnungszeitenbuch.de", "oraridiapertura24.it", "tuttocitta.it", "yellow.co.nz", "11880.com", "degulesider.dk", "horairesdouverture24.fr", "lookup.pk",
			"yellowpages.in", "yep.co.za", "yellowpages-uae.com", "dastelefonbuch.de", "paginasamarillas.com", "guiatel.es", "yellowpages.com.eg", "pai.pt", "xo.gr", "yelp.it",
			"yelp.de", "yelp.fr", "yelp.cz", "amarillas.cl", "goldenpages.ie", "zlatestranky.sk", "zlatestranky.cz", "gulesider.no", "paginas-amarillas.com", "yelp.se",
			"yelp.pt", "yelp.es", "cylex-locale.fr", "cylex.net.za", "cylex-australia.com", "cylex-belgie.be", "cylex.com.br", "cylex.de", "cylex-italia.it", "cylex.nl",
			"cylex.es", "cylex-swiss.ch", "cylex-uk.co.uk", "cylex.cz", "cylex.dk", "cylex.hu", "cylex.co.nz", "cylex.no", "cylex-polska.pl", "cylex.se",
			"cylex.us.com"
		};

		private void ApplyGoldDarkTheme()
		{
			Color bgColor = Color.FromArgb(13, 27, 42);
			Color panelColor = Color.FromArgb(20, 35, 55);
			Color accentColor = Color.FromArgb(255, 215, 0);
			Color textColor = Color.FromArgb(224, 224, 224);
			Color darkControl = Color.FromArgb(25, 42, 64);

			this.BackColor = bgColor;
			this.ForeColor = textColor;

			menuStrip.BackColor = panelColor;
			menuStrip.ForeColor = accentColor;

			statusStrip.BackColor = panelColor;
			statusStrip.ForeColor = textColor;

			panel1.BackColor = panelColor;
			splitContainer.BackColor = bgColor;
			splitContainer.Panel1.BackColor = panelColor;
			splitContainer.Panel2.BackColor = bgColor;

			splitContainer.SplitterDistance = 420;

			dgvResults.BackgroundColor = darkControl;
			dgvResults.GridColor = Color.FromArgb(40, 60, 80);
			dgvResults.DefaultCellStyle.BackColor = darkControl;
			dgvResults.DefaultCellStyle.ForeColor = textColor;
			dgvResults.DefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 60, 0);
			dgvResults.DefaultCellStyle.SelectionForeColor = accentColor;
			dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 60, 80);
			dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = accentColor;
			dgvResults.ColumnHeadersDefaultCellStyle.Font = new Font(dgvResults.Font, FontStyle.Bold);
			dgvResults.EnableHeadersVisualStyles = false;
			dgvResults.RowHeadersDefaultCellStyle.BackColor = darkControl;
			dgvResults.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
			dgvResults.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

			for (int i = 9; i <= 12; i++)
			{
				if (i < dgvResults.Columns.Count)
					dgvResults.Columns[i].DefaultCellStyle.ForeColor = Color.DodgerBlue;
			}

			foreach (Control c in panel1.Controls)
			{
				if (c is Button btn)
				{
					btn.BackColor = darkControl;
					btn.ForeColor = textColor;
					btn.FlatStyle = FlatStyle.Flat;
					btn.FlatAppearance.BorderColor = Color.FromArgb(60, 80, 100);
				}
				if (c is TextBox)
				{
					c.BackColor = darkControl;
					c.ForeColor = textColor;
				}
				if (c is Label)
				{
					c.ForeColor = accentColor;
				}
			}

			btnExport.BackColor = Color.FromArgb(180, 40, 0);
			btnExport.ForeColor = Color.White;
			btnExport.FlatStyle = FlatStyle.Flat;
			btnExport.FlatAppearance.BorderColor = Color.FromArgb(200, 60, 0);

			btnGetData.BackColor = Color.FromArgb(0, 100, 0);
			btnGetData.ForeColor = Color.White;
			btnGetData.FlatStyle = FlatStyle.Flat;
			btnGetData.FlatAppearance.BorderColor = Color.FromArgb(0, 140, 0);

			btnStop.BackColor = Color.FromArgb(180, 40, 0);
			btnStop.ForeColor = Color.White;
			btnStop.FlatStyle = FlatStyle.Flat;
			btnStop.FlatAppearance.BorderColor = Color.FromArgb(200, 60, 0);

			foreach (Control c in panel2.Controls)
			{
				if (c is Button btn2 && c != btnExport && c != btnGetData && c != btnStop)
				{
					btn2.BackColor = darkControl;
					btn2.ForeColor = textColor;
					btn2.FlatStyle = FlatStyle.Flat;
					btn2.FlatAppearance.BorderColor = Color.FromArgb(60, 80, 100);
				}
			}

			foreach (ToolStripMenuItem item in menuStrip.Items)
			{
				item.BackColor = panelColor;
				item.ForeColor = accentColor;
				foreach (ToolStripMenuItem sub in item.DropDownItems)
				{
					sub.BackColor = panelColor;
					sub.ForeColor = textColor;
				}
			}
		}
	}
}

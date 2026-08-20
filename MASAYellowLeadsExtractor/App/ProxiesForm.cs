using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MASAYellowLeadsExtractor
{
	// Token: 0x0200000F RID: 15
	public partial class ProxiesForm : Form
	{
		// Token: 0x0600009B RID: 155 RVA: 0x000081BA File Offset: 0x000063BA
		public ProxiesForm(string[] AppProxieSources)
		{
			this.InitializeComponent();
			this.ProxyServers = new List<ProxyServer>();
			this.AllProxyServers = new List<ProxyServer>();
			this.ProxieSources = AppProxieSources;
			Program.LanguagesManager.InitControl(this, base.Controls);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000081F8 File Offset: 0x000063F8
		private void ProxiesForm_Shown(object sender, EventArgs e)
		{
			this.label1.Invalidate();
			foreach (string ProxySource in this.ProxieSources)
			{
				try
				{
					string ProxiesPage = HTTPScraper.GetPage(ProxySource, null);
					if (ProxiesPage != null && ProxiesPage.Length > 0)
					{
						foreach (object obj in Regex.Matches(ProxiesPage, "\\b(\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}):(\\d{1,5})\\b"))
						{
							Match i = (Match)obj;
							string ProxyIP = i.Groups[1].Value;
							int ProxyPort = 0;
							try
							{
								ProxyPort = Convert.ToInt32(i.Groups[2].Value);
							}
							catch
							{
							}
							if (ProxyPort != 0)
							{
								this.AllProxyServers.Add(new ProxyServer
								{
									IP = ProxyIP,
									Port = ProxyPort
								});
							}
						}
						foreach (object obj2 in Regex.Matches(ProxiesPage, "<td>(\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3})<\\/td><td>(\\d{1,5})<\\/td>"))
						{
							Match j = (Match)obj2;
							string ProxyIP2 = j.Groups[1].Value;
							int ProxyPort2 = 0;
							try
							{
								ProxyPort2 = Convert.ToInt32(j.Groups[2].Value);
							}
							catch
							{
							}
							if (ProxyPort2 != 0)
							{
								this.AllProxyServers.Add(new ProxyServer
								{
									IP = ProxyIP2,
									Port = ProxyPort2
								});
							}
						}
						foreach (object obj3 in Regex.Matches(ProxiesPage, "\"PROXY_IP\":\"(.*?)\",\"PROXY_LAST_UPDATE\":\"(.*?)\",\"PROXY_PORT\":\"(.*?)\""))
						{
							Match k = (Match)obj3;
							string ProxyIP3 = k.Groups[1].Value;
							int ProxyPort3 = 0;
							try
							{
								ProxyPort3 = Convert.ToInt32(k.Groups[3].Value, 16);
							}
							catch
							{
							}
							if (ProxyPort3 != 0)
							{
								this.AllProxyServers.Add(new ProxyServer
								{
									IP = ProxyIP3,
									Port = ProxyPort3
								});
							}
						}
					}
				}
				catch
				{
				}
			}
			if (this.AllProxyServers.Count == 0)
			{
				base.Close();
				return;
			}
			for (int l = 0; l < this.AllProxyServers.Count; l++)
			{
				this.dgv.Rows.Add(new object[]
				{
					l + 1,
					this.AllProxyServers[l].IP,
					this.AllProxyServers[l].Port,
					""
				});
				this.AllProxyServers[l].CheckProxy();
			}
			this.dgv.Refresh();
			string[] CheckingLog = new string[this.AllProxyServers.Count];
			for (int m = 0; m < this.AllProxyServers.Count; m++)
			{
				CheckingLog[m] = string.Format("{0}:{1};", this.AllProxyServers[m].IP, this.AllProxyServers[m].Port);
			}
			int CheckProxyAttempts = 1;
			for (int n = 0; n < CheckProxyAttempts; n++)
			{
				int CheckedCount = 0;
				int Iterations = 0;
				while (CheckedCount < this.AllProxyServers.Count && Iterations < 15)
				{
					CheckedCount = 0;
					for (int n2 = 0; n2 < this.AllProxyServers.Count; n2++)
					{
						if (this.AllProxyServers[n2].Checked)
						{
							CheckedCount++;
							if (!this.AllProxyServers[n2].Processed)
							{
								if (this.AllProxyServers[n2].CanUse)
								{
									this.dgv.Rows[n2].Cells[3].Value = "OK";
									this.dgv.Rows[n2].Cells[3].Style.BackColor = Color.Lime;
									this.ProxyServers.Add(this.AllProxyServers[n2]);
									string[] array = CheckingLog;
									int num2 = n2;
									array[num2] += "1;";
								}
								else
								{
									this.dgv.Rows[n2].Cells[3].Value = "failed";
									this.dgv.Rows[n2].Cells[3].Style.BackColor = Color.Pink;
									string[] array2 = CheckingLog;
									int num3 = n2;
									array2[num3] += "0;";
								}
								this.AllProxyServers[n2].Processed = true;
							}
						}
					}
					Thread.Sleep(1000);
					Iterations++;
					this.lblInfo.Text = string.Format(Program.LanguagesManager.TotalProxiesMessage, this.AllProxyServers.Count, CheckedCount, this.ProxyServers.Count);
					this.lblInfo.Refresh();
					this.dgv.Refresh();
				}
				for (int n3 = 0; n3 < this.AllProxyServers.Count; n3++)
				{
					this.AllProxyServers[n3].CheckProxy();
				}
			}
			string LogData = "";
			for (int n4 = 0; n4 < this.AllProxyServers.Count; n4++)
			{
				LogData += string.Format("{0}{1}", CheckingLog[n4], Environment.NewLine);
			}
			File.WriteAllText("proxies_log.csv", LogData);
			base.Close();
		}

		// Token: 0x04000076 RID: 118
		public List<ProxyServer> ProxyServers;

		// Token: 0x04000077 RID: 119
		public List<ProxyServer> AllProxyServers;

		// Token: 0x04000078 RID: 120
		private string[] ProxieSources;
	}
}

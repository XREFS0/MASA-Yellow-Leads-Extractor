using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace MASAYellowLeadsExtractor
{
	// Token: 0x02000013 RID: 19
	public partial class SettingsForm : KryptonForm
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00009A38 File Offset: 0x00007C38
		public SettingsForm()
		{
			this.InitializeComponent();
			this.IsInitSettings = true;
			foreach (string ColumnName in this.ColumnNames)
			{
				this.cblShow.Items.Add(ColumnName);
				this.cblExport.Items.Add(ColumnName);
			}
			Program.LanguagesManager.InitControl(this, base.Controls);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00009B20 File Offset: 0x00007D20
		private void btnOk_Click(object sender, EventArgs e)
		{
			Settings AppSettings = new Settings();
			AppSettings.Language = this.cbLanguage.SelectedIndex;
			for (int i = 0; i < this.cblShow.Items.Count; i++)
			{
				AppSettings.ColumnsToShow[i] = this.cblShow.GetItemChecked(i);
				AppSettings.ColumnsToExport[i] = this.cblExport.GetItemChecked(i);
			}
			AppSettings.ExtractEmails = this.cbExtractEmails.Checked;
			AppSettings.AutoExport = this.rbAutoExport.Checked;
			AppSettings.AutoExportPath = this.tbExportPath.Text;
			AppSettings.ExportType = this.cbExportType.SelectedIndex;
			AppSettings.CSVDelimiter = this.cbCSVDelimiter.SelectedIndex;
			AppSettings.CSVEncoding = this.cbCSVEncoding.SelectedIndex;
			if (this.rbNoProxy.Checked)
			{
				AppSettings.ConnectionType = 0;
			}
			else if (this.rbUseSingleProxy.Checked)
			{
				AppSettings.ConnectionType = 1;
			}
			else if (this.rbRundomProxyList.Checked)
			{
				AppSettings.ConnectionType = 2;
			}
			else if (this.rbFreeProxiesList.Checked)
			{
				AppSettings.ConnectionType = 3;
			}
			else if (this.rbUseVPN.Checked)
			{
				AppSettings.ConnectionType = 4;
			}
			AppSettings.IsRandomDelay = this.cbRandomDelay.Checked;
			AppSettings.DelayFrom = this.tbDelayFrom.Value;
			AppSettings.DelayTo = this.tbDelayTo.Value;
			AppSettings.ProxyServer = this.tbProxyServerIP.Text;
			int.TryParse(this.tbProxyServerIP.Text, out AppSettings.ProxyPort);
			AppSettings.ProxyAuthentification = this.cbAuthentification.Checked;
			AppSettings.ProxyAuthLogin = this.tbProxyAuthUsername.Text;
			AppSettings.ProxyAuthPassword = this.tbProxyAuthPassword.Text;
			AppSettings.ProxyList = this.tbRandomProxyList.Text.Split(new char[] { '\r' });
			AppSettings.ProxySourcesList = this.tbFreeProxiesList.Text.Split(new char[] { '\r' });
			AppSettings.Save(Program.SettingsFileName);
			base.Close();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00009D3C File Offset: 0x00007F3C
		private void SettingsForm_Shown(object sender, EventArgs e)
		{
			Settings AppSettings = Settings.Load(Program.SettingsFileName);
			this.cbLanguage.SelectedIndex = AppSettings.Language;
			for (int i = 0; i < this.cblShow.Items.Count; i++)
			{
				if (AppSettings.ColumnsToShow[i])
				{
					this.cblShow.SetItemChecked(i, true);
				}
			}
			for (int j = 0; j < this.cblExport.Items.Count; j++)
			{
				if (AppSettings.ColumnsToExport[j])
				{
					this.cblExport.SetItemChecked(j, true);
				}
			}
			this.tbExportPath.Text = AppSettings.AutoExportPath;
			if (AppSettings.AutoExport)
			{
				this.rbAutoExport.Checked = true;
				this.tbExportPath.Enabled = true;
			}
			else
			{
				this.tbExportPath.Enabled = false;
				this.rbManualExport.Checked = true;
			}
			this.cbExtractEmails.Checked = AppSettings.ExtractEmails;
			this.cbExportType.SelectedIndex = AppSettings.ExportType;
			this.cbCSVDelimiter.SelectedIndex = AppSettings.CSVDelimiter;
			this.cbCSVEncoding.SelectedIndex = AppSettings.CSVEncoding;
			this.tbDelayFrom.Value = AppSettings.DelayFrom;
			this.tbDelayTo.Value = AppSettings.DelayTo;
			this.tbDelayFrom.Enabled = AppSettings.IsRandomDelay;
			this.tbDelayTo.Enabled = AppSettings.IsRandomDelay;
			this.cbRandomDelay.Checked = AppSettings.IsRandomDelay;
			this.tbProxyServerIP.Text = AppSettings.ProxyServer;
			this.tbProxyServerPort.Text = AppSettings.ProxyPort.ToString();
			if (AppSettings.ProxyList != null)
			{
				foreach (string p in AppSettings.ProxyList)
				{
					TextBox textBox = this.tbRandomProxyList;
					textBox.Text += string.Format("{0}{1}", p, Environment.NewLine);
				}
			}
			if (AppSettings.ProxySourcesList != null)
			{
				foreach (string p2 in AppSettings.ProxySourcesList)
				{
					TextBox textBox2 = this.tbFreeProxiesList;
					textBox2.Text += string.Format("{0}{1}", p2, Environment.NewLine);
				}
			}
			switch (AppSettings.ConnectionType)
			{
			case 0:
				this.rbNoProxy.Checked = true;
				break;
			case 1:
				this.rbUseSingleProxy.Checked = true;
				this.tbProxyServerIP.Enabled = true;
				this.tbProxyServerPort.Enabled = true;
				break;
			case 2:
				this.rbRundomProxyList.Checked = true;
				this.tbRandomProxyList.Enabled = true;
				break;
			case 3:
				this.rbFreeProxiesList.Checked = true;
				this.tbFreeProxiesList.Enabled = true;
				break;
			case 4:
				this.rbUseVPN.Checked = true;
				break;
			}
			this.cbAuthentification.Checked = AppSettings.ProxyAuthentification;
			this.tbProxyAuthUsername.Enabled = AppSettings.ProxyAuthentification;
			this.tbProxyAuthPassword.Enabled = AppSettings.ProxyAuthentification;
			this.IsInitSettings = false;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000032E1 File Offset: 0x000014E1
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000A044 File Offset: 0x00008244
		private void rbNoProxy_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyServerIP.Enabled = false;
			this.tbProxyServerPort.Enabled = false;
			this.tbProxyAuthPassword.Enabled = false;
			this.tbProxyAuthUsername.Enabled = false;
			this.cbAuthentification.Enabled = false;
			this.tbRandomProxyList.Enabled = false;
			this.tbFreeProxiesList.Enabled = false;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000A0A8 File Offset: 0x000082A8
		private void rbUseSingleProxy_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyServerIP.Enabled = true;
			this.tbProxyServerPort.Enabled = true;
			this.tbProxyAuthPassword.Enabled = false;
			this.tbProxyAuthUsername.Enabled = false;
			this.cbAuthentification.Enabled = true;
			this.tbRandomProxyList.Enabled = false;
			this.tbFreeProxiesList.Enabled = false;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000A10C File Offset: 0x0000830C
		private void rbRundomProxyList_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyServerIP.Enabled = false;
			this.tbProxyServerPort.Enabled = false;
			this.tbProxyAuthPassword.Enabled = false;
			this.tbProxyAuthUsername.Enabled = false;
			this.cbAuthentification.Enabled = false;
			this.tbRandomProxyList.Enabled = true;
			this.tbFreeProxiesList.Enabled = false;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000A170 File Offset: 0x00008370
		private void rbFreeProxiesList_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyServerIP.Enabled = false;
			this.tbProxyServerPort.Enabled = false;
			this.tbProxyAuthPassword.Enabled = false;
			this.tbProxyAuthUsername.Enabled = false;
			this.cbAuthentification.Enabled = false;
			this.tbRandomProxyList.Enabled = false;
			this.tbFreeProxiesList.Enabled = true;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000A1D1 File Offset: 0x000083D1
		private void rbUseVPN_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000A1F3 File Offset: 0x000083F3
		private void cbRandomDelay_CheckedChanged(object sender, EventArgs e)
		{
			this.tbDelayFrom.Enabled = this.cbRandomDelay.Checked;
			this.tbDelayTo.Enabled = this.cbRandomDelay.Checked;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000A224 File Offset: 0x00008424
		private void tbDelayFrom_ValueChanged(object sender, EventArgs e)
		{
			if (this.tbDelayTo.Value < this.tbDelayFrom.Value && this.tbDelayFrom.Value + 1 <= this.tbDelayTo.Maximum)
			{
				this.tbDelayTo.Value = this.tbDelayFrom.Value + 1;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000A27C File Offset: 0x0000847C
		private void tbDelayTo_ValueChanged(object sender, EventArgs e)
		{
			if (this.tbDelayTo.Value < this.tbDelayFrom.Value && this.tbDelayTo.Value - 1 >= this.tbDelayFrom.Minimum)
			{
				this.tbDelayFrom.Value = this.tbDelayTo.Value - 1;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000A2D3 File Offset: 0x000084D3
		private void cbAuthentification_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyAuthPassword.Enabled = this.cbAuthentification.Checked;
			this.tbProxyAuthUsername.Enabled = this.cbAuthentification.Checked;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000A301 File Offset: 0x00008501
		private void rbAutoExport_CheckedChanged(object sender, EventArgs e)
		{
			this.tbExportPath.Enabled = this.rbAutoExport.Checked;
			this.btnChooseExportFolder.Enabled = this.rbAutoExport.Checked;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000A330 File Offset: 0x00008530
		private void btnChooseExportFolder_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.SelectedPath = Application.StartupPath;
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				this.tbExportPath.Text = fbd.SelectedPath;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000A368 File Offset: 0x00008568
		private void cbExportType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cbExportType.SelectedIndex == 0)
			{
				this.cbCSVDelimiter.Enabled = true;
				this.cbCSVEncoding.Enabled = false;
				return;
			}
			if (this.cbExportType.SelectedIndex == 1)
			{
				this.cbCSVDelimiter.Enabled = false;
				this.cbCSVEncoding.Enabled = true;
				return;
			}
			this.cbCSVDelimiter.Enabled = false;
			this.cbCSVEncoding.Enabled = false;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000604E File Offset: 0x0000424E
		private void cblShow_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x040000A7 RID: 167
		private bool IsInitSettings;

		// Token: 0x040000A8 RID: 168
		private string[] ColumnNames = new string[]
		{
			"Category", "Business Name", "Address", "City", "State", "Zip Code", "Country", "Phone", "Fax", "Website",
			"Email", "Maplink", "Details Link"
		};
	}
}

namespace MASAYellowLeadsExtractor
{
	// Token: 0x02000013 RID: 19
	public partial class SettingsForm : global::ComponentFactory.Krypton.Toolkit.KryptonForm
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x0000A3DA File Offset: 0x000085DA
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000A3FC File Offset: 0x000085FC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::MASAYellowLeadsExtractor.SettingsForm));
			this.tabControl = new global::System.Windows.Forms.TabControl();
			this.tabPage1 = new global::System.Windows.Forms.TabPage();
			this.cbLanguage = new global::System.Windows.Forms.ComboBox();
			this.label10 = new global::System.Windows.Forms.Label();
			this.cbExtractEmails = new global::System.Windows.Forms.CheckBox();
			this.cbCSVEncoding = new global::System.Windows.Forms.ComboBox();
			this.label5 = new global::System.Windows.Forms.Label();
			this.cbCSVDelimiter = new global::System.Windows.Forms.ComboBox();
			this.label4 = new global::System.Windows.Forms.Label();
			this.cbExportType = new global::System.Windows.Forms.ComboBox();
			this.label3 = new global::System.Windows.Forms.Label();
			this.btnChooseExportFolder = new global::System.Windows.Forms.Button();
			this.tbExportPath = new global::System.Windows.Forms.TextBox();
			this.rbAutoExport = new global::System.Windows.Forms.RadioButton();
			this.rbManualExport = new global::System.Windows.Forms.RadioButton();
			this.cblExport = new global::System.Windows.Forms.CheckedListBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.cblShow = new global::System.Windows.Forms.CheckedListBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.tabPage2 = new global::System.Windows.Forms.TabPage();
			this.tbDelayTo = new global::System.Windows.Forms.TrackBar();
			this.tbDelayFrom = new global::System.Windows.Forms.TrackBar();
			this.label12 = new global::System.Windows.Forms.Label();
			this.label11 = new global::System.Windows.Forms.Label();
			this.cbRandomDelay = new global::System.Windows.Forms.CheckBox();
			this.rbUseVPN = new global::System.Windows.Forms.RadioButton();
			this.tbProxyAuthPassword = new global::System.Windows.Forms.TextBox();
			this.tbProxyAuthUsername = new global::System.Windows.Forms.TextBox();
			this.label9 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.tbFreeProxiesList = new global::System.Windows.Forms.TextBox();
			this.tbRandomProxyList = new global::System.Windows.Forms.TextBox();
			this.cbAuthentification = new global::System.Windows.Forms.CheckBox();
			this.tbProxyServerPort = new global::System.Windows.Forms.TextBox();
			this.label7 = new global::System.Windows.Forms.Label();
			this.tbProxyServerIP = new global::System.Windows.Forms.TextBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.rbFreeProxiesList = new global::System.Windows.Forms.RadioButton();
			this.rbRundomProxyList = new global::System.Windows.Forms.RadioButton();
			this.rbUseSingleProxy = new global::System.Windows.Forms.RadioButton();
			this.rbNoProxy = new global::System.Windows.Forms.RadioButton();
			this.btnOk = new global::System.Windows.Forms.Button();
			this.btnCancel = new global::System.Windows.Forms.Button();
			this.panel = new global::System.Windows.Forms.Panel();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.tbDelayTo).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.tbDelayFrom).BeginInit();
			this.panel.SuspendLayout();
			base.SuspendLayout();
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPage2);
			this.tabControl.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new global::System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new global::System.Drawing.Size(584, 315);
			this.tabControl.TabIndex = 0;
			this.tabPage1.Controls.Add(this.cbLanguage);
			this.tabPage1.Controls.Add(this.label10);
			this.tabPage1.Controls.Add(this.cbExtractEmails);
			this.tabPage1.Controls.Add(this.cbCSVEncoding);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.cbCSVDelimiter);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.cbExportType);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.btnChooseExportFolder);
			this.tabPage1.Controls.Add(this.tbExportPath);
			this.tabPage1.Controls.Add(this.rbAutoExport);
			this.tabPage1.Controls.Add(this.rbManualExport);
			this.tabPage1.Controls.Add(this.cblExport);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.cblShow);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new global::System.Drawing.Size(576, 289);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Data";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.cbLanguage.FormattingEnabled = true;
			this.cbLanguage.Items.AddRange(new object[] { "English", "Italian", "German", "French", "Spain" });
			this.cbLanguage.Location = new global::System.Drawing.Point(126, 11);
			this.cbLanguage.Name = "cbLanguage";
			this.cbLanguage.Size = new global::System.Drawing.Size(159, 21);
			this.cbLanguage.TabIndex = 16;
			this.label10.AutoSize = true;
			this.label10.Location = new global::System.Drawing.Point(24, 14);
			this.label10.Name = "label10";
			this.label10.Size = new global::System.Drawing.Size(96, 13);
			this.label10.TabIndex = 15;
			this.label10.Text = "Interface language";
			this.cbExtractEmails.AutoSize = true;
			this.cbExtractEmails.Checked = true;
			this.cbExtractEmails.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.cbExtractEmails.Location = new global::System.Drawing.Point(27, 158);
			this.cbExtractEmails.Name = "cbExtractEmails";
			this.cbExtractEmails.Size = new global::System.Drawing.Size(208, 17);
			this.cbExtractEmails.TabIndex = 14;
			this.cbExtractEmails.Text = "Extract email from website (slower app)";
			this.cbExtractEmails.UseVisualStyleBackColor = true;
			this.cbCSVEncoding.FormattingEnabled = true;
			this.cbCSVEncoding.Items.AddRange(new object[] { "ASCII", "UTF7", "UTF8" });
			this.cbCSVEncoding.Location = new global::System.Drawing.Point(428, 255);
			this.cbCSVEncoding.Name = "cbCSVEncoding";
			this.cbCSVEncoding.Size = new global::System.Drawing.Size(121, 21);
			this.cbCSVEncoding.TabIndex = 13;
			this.label5.AutoSize = true;
			this.label5.Location = new global::System.Drawing.Point(370, 258);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(52, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "Encoding";
			this.cbCSVDelimiter.FormattingEnabled = true;
			this.cbCSVDelimiter.Items.AddRange(new object[] { ", - comma (for USA)", "; - semicolon (for Europe)" });
			this.cbCSVDelimiter.Location = new global::System.Drawing.Point(180, 255);
			this.cbCSVDelimiter.Name = "cbCSVDelimiter";
			this.cbCSVDelimiter.Size = new global::System.Drawing.Size(186, 21);
			this.cbCSVDelimiter.TabIndex = 11;
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(24, 258);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(133, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "CSV file columns separator";
			this.cbExportType.FormattingEnabled = true;
			this.cbExportType.Items.AddRange(new object[] { "CSV (comma separated file)", "Tab separated text file", "Microsoft Excel File" });
			this.cbExportType.Location = new global::System.Drawing.Point(180, 224);
			this.cbExportType.Name = "cbExportType";
			this.cbExportType.Size = new global::System.Drawing.Size(186, 21);
			this.cbExportType.TabIndex = 9;
			this.cbExportType.SelectedIndexChanged += new global::System.EventHandler(this.cbExportType_SelectedIndexChanged);
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(24, 227);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(150, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "File format for data export data";
			this.btnChooseExportFolder.Enabled = false;
			this.btnChooseExportFolder.Location = new global::System.Drawing.Point(348, 155);
			this.btnChooseExportFolder.Name = "btnChooseExportFolder";
			this.btnChooseExportFolder.Size = new global::System.Drawing.Size(75, 23);
			this.btnChooseExportFolder.TabIndex = 7;
			this.btnChooseExportFolder.Text = "Choose";
			this.btnChooseExportFolder.UseVisualStyleBackColor = true;
			this.btnChooseExportFolder.Visible = false;
			this.btnChooseExportFolder.Click += new global::System.EventHandler(this.btnChooseExportFolder_Click);
			this.tbExportPath.Location = new global::System.Drawing.Point(438, 158);
			this.tbExportPath.Name = "tbExportPath";
			this.tbExportPath.Size = new global::System.Drawing.Size(441, 20);
			this.tbExportPath.TabIndex = 6;
			this.tbExportPath.Visible = false;
			this.rbAutoExport.AutoSize = true;
			this.rbAutoExport.Location = new global::System.Drawing.Point(314, 158);
			this.rbAutoExport.Name = "rbAutoExport";
			this.rbAutoExport.Size = new global::System.Drawing.Size(202, 17);
			this.rbAutoExport.TabIndex = 5;
			this.rbAutoExport.TabStop = true;
			this.rbAutoExport.Text = "Export results automatically to a folder";
			this.rbAutoExport.UseVisualStyleBackColor = true;
			this.rbAutoExport.Visible = false;
			this.rbAutoExport.CheckedChanged += new global::System.EventHandler(this.rbAutoExport_CheckedChanged);
			this.rbManualExport.AutoSize = true;
			this.rbManualExport.Location = new global::System.Drawing.Point(291, 158);
			this.rbManualExport.Name = "rbManualExport";
			this.rbManualExport.Size = new global::System.Drawing.Size(132, 17);
			this.rbManualExport.TabIndex = 4;
			this.rbManualExport.TabStop = true;
			this.rbManualExport.Text = "Export results manually";
			this.rbManualExport.UseVisualStyleBackColor = true;
			this.rbManualExport.Visible = false;
			this.cblExport.FormattingEnabled = true;
			this.cblExport.Location = new global::System.Drawing.Point(291, 73);
			this.cblExport.MultiColumn = true;
			this.cblExport.Name = "cblExport";
			this.cblExport.Size = new global::System.Drawing.Size(258, 79);
			this.cblExport.TabIndex = 3;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(288, 57);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(92, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Columns to Export";
			this.cblShow.FormattingEnabled = true;
			this.cblShow.Location = new global::System.Drawing.Point(27, 73);
			this.cblShow.MultiColumn = true;
			this.cblShow.Name = "cblShow";
			this.cblShow.Size = new global::System.Drawing.Size(258, 79);
			this.cblShow.TabIndex = 1;
			this.cblShow.SelectedIndexChanged += new global::System.EventHandler(this.cblShow_SelectedIndexChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(24, 57);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(104, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Show in results table";
			this.tabPage2.Controls.Add(this.tbDelayTo);
			this.tabPage2.Controls.Add(this.tbDelayFrom);
			this.tabPage2.Controls.Add(this.label12);
			this.tabPage2.Controls.Add(this.label11);
			this.tabPage2.Controls.Add(this.cbRandomDelay);
			this.tabPage2.Controls.Add(this.rbUseVPN);
			this.tabPage2.Controls.Add(this.tbProxyAuthPassword);
			this.tabPage2.Controls.Add(this.tbProxyAuthUsername);
			this.tabPage2.Controls.Add(this.label9);
			this.tabPage2.Controls.Add(this.label8);
			this.tabPage2.Controls.Add(this.tbFreeProxiesList);
			this.tabPage2.Controls.Add(this.tbRandomProxyList);
			this.tabPage2.Controls.Add(this.cbAuthentification);
			this.tabPage2.Controls.Add(this.tbProxyServerPort);
			this.tabPage2.Controls.Add(this.label7);
			this.tabPage2.Controls.Add(this.tbProxyServerIP);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.rbFreeProxiesList);
			this.tabPage2.Controls.Add(this.rbRundomProxyList);
			this.tabPage2.Controls.Add(this.rbUseSingleProxy);
			this.tabPage2.Controls.Add(this.rbNoProxy);
			this.tabPage2.Location = new global::System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new global::System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new global::System.Drawing.Size(576, 289);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Connection";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.tbDelayTo.AutoSize = false;
			this.tbDelayTo.Location = new global::System.Drawing.Point(483, 54);
			this.tbDelayTo.Name = "tbDelayTo";
			this.tbDelayTo.Size = new global::System.Drawing.Size(85, 18);
			this.tbDelayTo.TabIndex = 20;
			this.tbDelayTo.ValueChanged += new global::System.EventHandler(this.tbDelayTo_ValueChanged);
			this.tbDelayFrom.AutoSize = false;
			this.tbDelayFrom.Location = new global::System.Drawing.Point(352, 54);
			this.tbDelayFrom.Name = "tbDelayFrom";
			this.tbDelayFrom.Size = new global::System.Drawing.Size(85, 18);
			this.tbDelayFrom.TabIndex = 19;
			this.tbDelayFrom.ValueChanged += new global::System.EventHandler(this.tbDelayFrom_ValueChanged);
			this.label12.AutoSize = true;
			this.label12.Location = new global::System.Drawing.Point(452, 54);
			this.label12.Name = "label12";
			this.label12.Size = new global::System.Drawing.Size(16, 13);
			this.label12.TabIndex = 18;
			this.label12.Text = "to";
			this.label11.AutoSize = true;
			this.label11.Location = new global::System.Drawing.Point(288, 54);
			this.label11.Name = "label11";
			this.label11.Size = new global::System.Drawing.Size(57, 13);
			this.label11.TabIndex = 17;
			this.label11.Text = "Delay from";
			this.cbRandomDelay.AutoSize = true;
			this.cbRandomDelay.Location = new global::System.Drawing.Point(291, 34);
			this.cbRandomDelay.Name = "cbRandomDelay";
			this.cbRandomDelay.Size = new global::System.Drawing.Size(198, 17);
			this.cbRandomDelay.TabIndex = 16;
			this.cbRandomDelay.Text = "Use random delay between requests";
			this.cbRandomDelay.UseVisualStyleBackColor = true;
			this.cbRandomDelay.CheckedChanged += new global::System.EventHandler(this.cbRandomDelay_CheckedChanged);
			this.rbUseVPN.AutoSize = true;
			this.rbUseVPN.Location = new global::System.Drawing.Point(8, 10);
			this.rbUseVPN.Name = "rbUseVPN";
			this.rbUseVPN.Size = new global::System.Drawing.Size(140, 17);
			this.rbUseVPN.TabIndex = 15;
			this.rbUseVPN.TabStop = true;
			this.rbUseVPN.Text = "Use VPN to hide your IP";
			this.rbUseVPN.UseVisualStyleBackColor = true;
			this.rbUseVPN.CheckedChanged += new global::System.EventHandler(this.rbUseVPN_CheckedChanged);
			this.tbProxyAuthPassword.Enabled = false;
			this.tbProxyAuthPassword.Location = new global::System.Drawing.Point(352, 132);
			this.tbProxyAuthPassword.Name = "tbProxyAuthPassword";
			this.tbProxyAuthPassword.Size = new global::System.Drawing.Size(216, 20);
			this.tbProxyAuthPassword.TabIndex = 14;
			this.tbProxyAuthPassword.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Center;
			this.tbProxyAuthPassword.Visible = false;
			this.tbProxyAuthUsername.Enabled = false;
			this.tbProxyAuthUsername.Location = new global::System.Drawing.Point(352, 106);
			this.tbProxyAuthUsername.Name = "tbProxyAuthUsername";
			this.tbProxyAuthUsername.Size = new global::System.Drawing.Size(216, 20);
			this.tbProxyAuthUsername.TabIndex = 13;
			this.tbProxyAuthUsername.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Center;
			this.tbProxyAuthUsername.Visible = false;
			this.label9.AutoSize = true;
			this.label9.Location = new global::System.Drawing.Point(288, 135);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(56, 13);
			this.label9.TabIndex = 12;
			this.label9.Text = "Password:";
			this.label9.Visible = false;
			this.label8.AutoSize = true;
			this.label8.Location = new global::System.Drawing.Point(288, 109);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(58, 13);
			this.label8.TabIndex = 11;
			this.label8.Text = "Username:";
			this.label8.Visible = false;
			this.tbFreeProxiesList.Enabled = false;
			this.tbFreeProxiesList.Location = new global::System.Drawing.Point(291, 197);
			this.tbFreeProxiesList.Multiline = true;
			this.tbFreeProxiesList.Name = "tbFreeProxiesList";
			this.tbFreeProxiesList.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.tbFreeProxiesList.Size = new global::System.Drawing.Size(277, 84);
			this.tbFreeProxiesList.TabIndex = 10;
			this.tbRandomProxyList.Enabled = false;
			this.tbRandomProxyList.Location = new global::System.Drawing.Point(8, 197);
			this.tbRandomProxyList.Multiline = true;
			this.tbRandomProxyList.Name = "tbRandomProxyList";
			this.tbRandomProxyList.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.tbRandomProxyList.Size = new global::System.Drawing.Size(277, 84);
			this.tbRandomProxyList.TabIndex = 9;
			this.cbAuthentification.AutoSize = true;
			this.cbAuthentification.Location = new global::System.Drawing.Point(291, 88);
			this.cbAuthentification.Name = "cbAuthentification";
			this.cbAuthentification.Size = new global::System.Drawing.Size(199, 17);
			this.cbAuthentification.TabIndex = 8;
			this.cbAuthentification.Text = "Proxy server requires authentification";
			this.cbAuthentification.UseVisualStyleBackColor = true;
			this.cbAuthentification.Visible = false;
			this.cbAuthentification.CheckedChanged += new global::System.EventHandler(this.cbAuthentification_CheckedChanged);
			this.tbProxyServerPort.Enabled = false;
			this.tbProxyServerPort.Location = new global::System.Drawing.Point(240, 110);
			this.tbProxyServerPort.Name = "tbProxyServerPort";
			this.tbProxyServerPort.Size = new global::System.Drawing.Size(45, 20);
			this.tbProxyServerPort.TabIndex = 7;
			this.tbProxyServerPort.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Center;
			this.label7.AutoSize = true;
			this.label7.Location = new global::System.Drawing.Point(205, 113);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(28, 13);
			this.label7.TabIndex = 6;
			this.label7.Text = "port:";
			this.tbProxyServerIP.Enabled = false;
			this.tbProxyServerIP.Location = new global::System.Drawing.Point(92, 110);
			this.tbProxyServerIP.Name = "tbProxyServerIP";
			this.tbProxyServerIP.Size = new global::System.Drawing.Size(107, 20);
			this.tbProxyServerIP.TabIndex = 5;
			this.tbProxyServerIP.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Center;
			this.label6.AutoSize = true;
			this.label6.Location = new global::System.Drawing.Point(5, 113);
			this.label6.Name = "label6";
			this.label6.Size = new global::System.Drawing.Size(81, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "Proxy server IP:";
			this.rbFreeProxiesList.AutoSize = true;
			this.rbFreeProxiesList.Location = new global::System.Drawing.Point(291, 174);
			this.rbFreeProxiesList.Name = "rbFreeProxiesList";
			this.rbFreeProxiesList.Size = new global::System.Drawing.Size(247, 17);
			this.rbFreeProxiesList.TabIndex = 3;
			this.rbFreeProxiesList.TabStop = true;
			this.rbFreeProxiesList.Text = "Use free proxies lists (enter one source per line)";
			this.rbFreeProxiesList.UseVisualStyleBackColor = true;
			this.rbFreeProxiesList.CheckedChanged += new global::System.EventHandler(this.rbFreeProxiesList_CheckedChanged);
			this.rbRundomProxyList.AutoSize = true;
			this.rbRundomProxyList.Location = new global::System.Drawing.Point(8, 174);
			this.rbRundomProxyList.Name = "rbRundomProxyList";
			this.rbRundomProxyList.Size = new global::System.Drawing.Size(258, 17);
			this.rbRundomProxyList.TabIndex = 2;
			this.rbRundomProxyList.TabStop = true;
			this.rbRundomProxyList.Text = "Use random proxy from list (line format server:port)";
			this.rbRundomProxyList.UseVisualStyleBackColor = true;
			this.rbRundomProxyList.CheckedChanged += new global::System.EventHandler(this.rbRundomProxyList_CheckedChanged);
			this.rbUseSingleProxy.AutoSize = true;
			this.rbUseSingleProxy.Location = new global::System.Drawing.Point(8, 88);
			this.rbUseSingleProxy.Name = "rbUseSingleProxy";
			this.rbUseSingleProxy.Size = new global::System.Drawing.Size(102, 17);
			this.rbUseSingleProxy.TabIndex = 1;
			this.rbUseSingleProxy.TabStop = true;
			this.rbUseSingleProxy.Text = "Use single proxy";
			this.rbUseSingleProxy.UseVisualStyleBackColor = true;
			this.rbUseSingleProxy.CheckedChanged += new global::System.EventHandler(this.rbUseSingleProxy_CheckedChanged);
			this.rbNoProxy.AutoSize = true;
			this.rbNoProxy.Location = new global::System.Drawing.Point(8, 33);
			this.rbNoProxy.Name = "rbNoProxy";
			this.rbNoProxy.Size = new global::System.Drawing.Size(142, 17);
			this.rbNoProxy.TabIndex = 0;
			this.rbNoProxy.TabStop = true;
			this.rbNoProxy.Text = "Do not use proxy servers";
			this.rbNoProxy.UseVisualStyleBackColor = true;
			this.rbNoProxy.CheckedChanged += new global::System.EventHandler(this.rbNoProxy_CheckedChanged);
			this.btnOk.Location = new global::System.Drawing.Point(214, 12);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new global::System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			this.btnCancel.Location = new global::System.Drawing.Point(295, 12);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new global::System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new global::System.EventHandler(this.btnCancel_Click);
			this.panel.Controls.Add(this.btnCancel);
			this.panel.Controls.Add(this.btnOk);
			this.panel.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel.Location = new global::System.Drawing.Point(0, 315);
			this.panel.Name = "panel";
			this.panel.Size = new global::System.Drawing.Size(584, 46);
			this.panel.TabIndex = 3;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(584, 361);
			base.Controls.Add(this.tabControl);
			base.Controls.Add(this.panel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Icon = null;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SettingsForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Settings";
			base.Shown += new global::System.EventHandler(this.SettingsForm_Shown);
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.tbDelayTo).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.tbDelayFrom).EndInit();
			this.panel.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x040000A9 RID: 169
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040000AA RID: 170
		private global::System.Windows.Forms.TabControl tabControl;

		// Token: 0x040000AB RID: 171
		private global::System.Windows.Forms.TabPage tabPage1;

		// Token: 0x040000AC RID: 172
		private global::System.Windows.Forms.TabPage tabPage2;

		// Token: 0x040000AD RID: 173
		private global::System.Windows.Forms.Button btnOk;

		// Token: 0x040000AE RID: 174
		private global::System.Windows.Forms.Button btnCancel;

		// Token: 0x040000AF RID: 175
		private global::System.Windows.Forms.Panel panel;

		// Token: 0x040000B0 RID: 176
		private global::System.Windows.Forms.ComboBox cbCSVEncoding;

		// Token: 0x040000B1 RID: 177
		private global::System.Windows.Forms.Label label5;

		// Token: 0x040000B2 RID: 178
		private global::System.Windows.Forms.ComboBox cbCSVDelimiter;

		// Token: 0x040000B3 RID: 179
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040000B4 RID: 180
		private global::System.Windows.Forms.ComboBox cbExportType;

		// Token: 0x040000B5 RID: 181
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040000B6 RID: 182
		private global::System.Windows.Forms.Button btnChooseExportFolder;

		// Token: 0x040000B7 RID: 183
		private global::System.Windows.Forms.TextBox tbExportPath;

		// Token: 0x040000B8 RID: 184
		private global::System.Windows.Forms.RadioButton rbAutoExport;

		// Token: 0x040000B9 RID: 185
		private global::System.Windows.Forms.RadioButton rbManualExport;

		// Token: 0x040000BA RID: 186
		private global::System.Windows.Forms.CheckedListBox cblExport;

		// Token: 0x040000BB RID: 187
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040000BC RID: 188
		private global::System.Windows.Forms.CheckedListBox cblShow;

		// Token: 0x040000BD RID: 189
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000BE RID: 190
		private global::System.Windows.Forms.RadioButton rbFreeProxiesList;

		// Token: 0x040000BF RID: 191
		private global::System.Windows.Forms.RadioButton rbRundomProxyList;

		// Token: 0x040000C0 RID: 192
		private global::System.Windows.Forms.RadioButton rbUseSingleProxy;

		// Token: 0x040000C1 RID: 193
		private global::System.Windows.Forms.RadioButton rbNoProxy;

		// Token: 0x040000C2 RID: 194
		private global::System.Windows.Forms.TextBox tbProxyAuthPassword;

		// Token: 0x040000C3 RID: 195
		private global::System.Windows.Forms.TextBox tbProxyAuthUsername;

		// Token: 0x040000C4 RID: 196
		private global::System.Windows.Forms.Label label9;

		// Token: 0x040000C5 RID: 197
		private global::System.Windows.Forms.Label label8;

		// Token: 0x040000C6 RID: 198
		private global::System.Windows.Forms.TextBox tbFreeProxiesList;

		// Token: 0x040000C7 RID: 199
		private global::System.Windows.Forms.TextBox tbRandomProxyList;

		// Token: 0x040000C8 RID: 200
		private global::System.Windows.Forms.CheckBox cbAuthentification;

		// Token: 0x040000C9 RID: 201
		private global::System.Windows.Forms.TextBox tbProxyServerPort;

		// Token: 0x040000CA RID: 202
		private global::System.Windows.Forms.Label label7;

		// Token: 0x040000CB RID: 203
		private global::System.Windows.Forms.TextBox tbProxyServerIP;

		// Token: 0x040000CC RID: 204
		private global::System.Windows.Forms.Label label6;

		// Token: 0x040000CD RID: 205
		private global::System.Windows.Forms.CheckBox cbExtractEmails;

		// Token: 0x040000CE RID: 206
		private global::System.Windows.Forms.RadioButton rbUseVPN;

		// Token: 0x040000CF RID: 207
		private global::System.Windows.Forms.ComboBox cbLanguage;

		// Token: 0x040000D0 RID: 208
		private global::System.Windows.Forms.Label label10;

		// Token: 0x040000D1 RID: 209
		private global::System.Windows.Forms.TrackBar tbDelayTo;

		// Token: 0x040000D2 RID: 210
		private global::System.Windows.Forms.TrackBar tbDelayFrom;

		// Token: 0x040000D3 RID: 211
		private global::System.Windows.Forms.Label label12;

		// Token: 0x040000D4 RID: 212
		private global::System.Windows.Forms.Label label11;

		// Token: 0x040000D5 RID: 213
		private global::System.Windows.Forms.CheckBox cbRandomDelay;
	}
}

namespace MASAYellowLeadsExtractor
{
	// Token: 0x0200000D RID: 13
	public partial class MainForm : global::ComponentFactory.Krypton.Toolkit.KryptonForm
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00006050 File Offset: 0x00004250
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006070 File Offset: 0x00004270
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::MASAYellowLeadsExtractor.MainForm));
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new global::System.Windows.Forms.DataGridViewCellStyle();
			global::System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new global::System.Windows.Forms.DataGridViewCellStyle();
			this.statusStrip = new global::System.Windows.Forms.StatusStrip();
			this.tssLabelStatus = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.tssLabelListed = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.tssLabelExported = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.tssLabelProgress = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.tsProgress = new global::System.Windows.Forms.ToolStripProgressBar();
			this.menuStrip = new global::System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.websitesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.wwwpaginegialleitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.wwwgelbenseitendeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.wwwpagesjaunesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.paginasAmarillasesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.yellowPagescomUSAToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.yellcomUKToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.infobelcomToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.yELPcomToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.aboutOnlineYellowPagesScraperToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolTip = new global::System.Windows.Forms.ToolTip(this.components);
			this.btnBrowserHome = new global::System.Windows.Forms.Button();
			this.btnWebbrowserBack = new global::System.Windows.Forms.Button();
			this.btnWebbrowserForward = new global::System.Windows.Forms.Button();
			this.btnWebbrowserRefresh = new global::System.Windows.Forms.Button();
			this.btnWebbrowserStop = new global::System.Windows.Forms.Button();
			this.bnWebrowserGo = new global::System.Windows.Forms.Button();
			this.panel1 = new global::System.Windows.Forms.Panel();
			this.tbWebbrowserUrl = new global::System.Windows.Forms.TextBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnExport = new global::System.Windows.Forms.Button();
			this.btnStop = new global::System.Windows.Forms.Button();
			this.btnDeleteAll = new global::System.Windows.Forms.Button();
			this.btnDeleteSelected = new global::System.Windows.Forms.Button();
			this.btnClearSelection = new global::System.Windows.Forms.Button();
			this.btnSelectAll = new global::System.Windows.Forms.Button();
			this.btnGetData = new global::System.Windows.Forms.Button();
			this.splitContainer = new global::System.Windows.Forms.SplitContainer();
			this.webControl = new global::EO.WinForm.WebControl();
			this.webView = new global::EO.WebBrowser.WebView();
			this.panel2 = new global::System.Windows.Forms.Panel();
			this.dgvResults = new global::System.Windows.Forms.DataGridView();
			this.category = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.business_name = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.address = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.city = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.state = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.postal_code = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.country = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.phone = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fax = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.website = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.email = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.map_link = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.details_link = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.statusStrip.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.panel1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.panel2.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dgvResults).BeginInit();
			base.SuspendLayout();
			this.statusStrip.ImageScalingSize = new global::System.Drawing.Size(20, 20);
			this.statusStrip.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.tssLabelStatus, this.tssLabelListed, this.tssLabelExported, this.tssLabelProgress, this.tsProgress });
			this.statusStrip.Location = new global::System.Drawing.Point(0, 785);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new global::System.Drawing.Size(1184, 26);
			this.statusStrip.TabIndex = 0;
			this.statusStrip.Text = "statusStrip1";
			this.tssLabelStatus.AutoSize = false;
			this.tssLabelStatus.Name = "tssLabelStatus";
			this.tssLabelStatus.Size = new global::System.Drawing.Size(450, 21);
			this.tssLabelStatus.Text = "Ready to work!";
			this.tssLabelStatus.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.tssLabelListed.AutoSize = false;
			this.tssLabelListed.Name = "tssLabelListed";
			this.tssLabelListed.Size = new global::System.Drawing.Size(150, 21);
			this.tssLabelListed.Text = "0 items listed";
			this.tssLabelListed.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.tssLabelExported.AutoSize = false;
			this.tssLabelExported.Name = "tssLabelExported";
			this.tssLabelExported.Size = new global::System.Drawing.Size(80, 21);
			this.tssLabelExported.Text = "0 exported";
			this.tssLabelExported.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.tssLabelProgress.AutoSize = false;
			this.tssLabelProgress.Name = "tssLabelProgress";
			this.tssLabelProgress.Size = new global::System.Drawing.Size(60, 21);
			this.tssLabelProgress.Text = "Progress:";
			this.tssLabelProgress.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.tsProgress.AutoSize = false;
			this.tsProgress.Name = "tsProgress";
			this.tsProgress.Size = new global::System.Drawing.Size(200, 20);
			this.menuStrip.Font = new global::System.Drawing.Font("Segoe UI", 9f);
			this.menuStrip.ImageScalingSize = new global::System.Drawing.Size(20, 20);
			this.menuStrip.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.fileToolStripMenuItem, this.websitesToolStripMenuItem, this.settingsToolStripMenuItem });
			this.menuStrip.Location = new global::System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new global::System.Drawing.Size(1184, 24);
			this.menuStrip.TabIndex = 1;
			this.menuStrip.Text = "menuStrip1";
			this.fileToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.exportToolStripMenuItem, this.exitToolStripMenuItem });
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new global::System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			this.exportToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("exportToolStripMenuItem.Image");
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new global::System.Drawing.Size(120, 26);
			this.exportToolStripMenuItem.Text = "Export...";
			this.exportToolStripMenuItem.Click += new global::System.EventHandler(this.exportToolStripMenuItem_Click);
			this.exitToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("exitToolStripMenuItem.Image");
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new global::System.Drawing.Size(120, 26);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new global::System.EventHandler(this.exitToolStripMenuItem_Click);
			this.websitesToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.wwwpaginegialleitToolStripMenuItem, this.wwwgelbenseitendeToolStripMenuItem, this.wwwpagesjaunesToolStripMenuItem, this.paginasAmarillasesToolStripMenuItem, this.yellowPagescomUSAToolStripMenuItem, this.yellcomUKToolStripMenuItem, this.infobelcomToolStripMenuItem, this.yELPcomToolStripMenuItem });
			this.websitesToolStripMenuItem.Name = "websitesToolStripMenuItem";
			this.websitesToolStripMenuItem.Size = new global::System.Drawing.Size(66, 20);
			this.websitesToolStripMenuItem.Text = "Websites";
			this.wwwpaginegialleitToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("wwwpaginegialleitToolStripMenuItem.Image");
			this.wwwpaginegialleitToolStripMenuItem.Name = "wwwpaginegialleitToolStripMenuItem";
			this.wwwpaginegialleitToolStripMenuItem.Size = new global::System.Drawing.Size(214, 26);
			this.wwwpaginegialleitToolStripMenuItem.Text = "PagineGialle.it (Italy)";
			this.wwwpaginegialleitToolStripMenuItem.ToolTipText = "Click to choose website";
			this.wwwpaginegialleitToolStripMenuItem.Click += new global::System.EventHandler(this.wwwpaginegialleitToolStripMenuItem_Click);
			this.wwwgelbenseitendeToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("wwwgelbenseitendeToolStripMenuItem.Image");
			this.wwwgelbenseitendeToolStripMenuItem.Name = "wwwgelbenseitendeToolStripMenuItem";
			this.wwwgelbenseitendeToolStripMenuItem.Size = new global::System.Drawing.Size(214, 26);
			this.wwwgelbenseitendeToolStripMenuItem.Text = "Gelbeseiten.de (Germany)";
			this.wwwgelbenseitendeToolStripMenuItem.Click += new global::System.EventHandler(this.wwwgelbenseitendeToolStripMenuItem_Click);
			this.wwwpagesjaunesToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("wwwpagesjaunesToolStripMenuItem.Image");
			this.wwwpagesjaunesToolStripMenuItem.Name = "wwwpagesjaunesToolStripMenuItem";
			this.wwwpagesjaunesToolStripMenuItem.Size = new global::System.Drawing.Size(214, 26);
			this.wwwpagesjaunesToolStripMenuItem.Text = "PagesJaunes.fr (France)";
			this.wwwpagesjaunesToolStripMenuItem.Click += new global::System.EventHandler(this.wwwpagesjaunesToolStripMenuItem_Click);
			this.paginasAmarillasesToolStripMenuItem.Name = "paginasAmarillasesToolStripMenuItem";
			this.paginasAmarillasesToolStripMenuItem.Size = new global::System.Drawing.Size(214, 26);
			this.paginasAmarillasesToolStripMenuItem.Text = "PaginasAmarillas.es";
			this.paginasAmarillasesToolStripMenuItem.Click += new global::System.EventHandler(this.wwwpaginasamarillasesToolStripMenuItem_Click);
			this.yellowPagescomUSAToolStripMenuItem.Name = "yellowPagescomUSAToolStripMenuItem";
			this.yellowPagescomUSAToolStripMenuItem.Size = new global::System.Drawing.Size(214, 26);
			this.yellowPagescomUSAToolStripMenuItem.Text = "YellowPages.com (USA)";
			this.yellowPagescomUSAToolStripMenuItem.Click += new global::System.EventHandler(this.wwwyellowpagescomToolStripMenuItem_Click);
			this.yellcomUKToolStripMenuItem.Name = "yellcomUKToolStripMenuItem";
			this.yellcomUKToolStripMenuItem.Size = new global::System.Drawing.Size(214, 26);
			this.yellcomUKToolStripMenuItem.Text = "Yell.com (UK)";
			this.yellcomUKToolStripMenuItem.Click += new global::System.EventHandler(this.wwwyellcomToolStripMenuItem_Click);
			this.infobelcomToolStripMenuItem.Name = "infobelcomToolStripMenuItem";
			this.infobelcomToolStripMenuItem.Size = new global::System.Drawing.Size(214, 26);
			this.infobelcomToolStripMenuItem.Text = "Infobel.com";
			this.infobelcomToolStripMenuItem.Click += new global::System.EventHandler(this.wwwinfobelcomToolStripMenuItem_Click);
			this.yELPcomToolStripMenuItem.Name = "yELPcomToolStripMenuItem";
			this.yELPcomToolStripMenuItem.Size = new global::System.Drawing.Size(214, 26);
			this.yELPcomToolStripMenuItem.Text = "YELP.com";
			this.yELPcomToolStripMenuItem.Click += new global::System.EventHandler(this.KompassToolStripMenuItem_Click);
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new global::System.Drawing.Size(61, 20);
			this.settingsToolStripMenuItem.Text = "Settings";
			this.settingsToolStripMenuItem.Click += new global::System.EventHandler(this.settingsToolStripMenuItem_Click);
			this.helpToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[] { this.aboutOnlineYellowPagesScraperToolStripMenuItem });
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new global::System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			this.aboutOnlineYellowPagesScraperToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("aboutOnlineYellowPagesScraperToolStripMenuItem.Image");
			this.aboutOnlineYellowPagesScraperToolStripMenuItem.Name = "aboutOnlineYellowPagesScraperToolStripMenuItem";
			this.aboutOnlineYellowPagesScraperToolStripMenuItem.Size = new global::System.Drawing.Size(262, 26);
			this.aboutOnlineYellowPagesScraperToolStripMenuItem.Text = "About Online Yellow Pages Scraper";
			this.aboutOnlineYellowPagesScraperToolStripMenuItem.Visible = false;
			this.aboutOnlineYellowPagesScraperToolStripMenuItem.Click += new global::System.EventHandler(this.aboutOnlineYellowPagesScraperToolStripMenuItem_Click);
			this.btnBrowserHome.Image = (global::System.Drawing.Image)resources.GetObject("btnBrowserHome.Image");
			this.btnBrowserHome.Location = new global::System.Drawing.Point(12, 3);
			this.btnBrowserHome.Name = "btnBrowserHome";
			this.btnBrowserHome.Size = new global::System.Drawing.Size(23, 23);
			this.btnBrowserHome.TabIndex = 0;
			this.toolTip.SetToolTip(this.btnBrowserHome, "Home");
			this.btnBrowserHome.UseVisualStyleBackColor = true;
			this.btnBrowserHome.Click += new global::System.EventHandler(this.btnBrowserHome_Click);
			this.btnWebbrowserBack.Image = (global::System.Drawing.Image)resources.GetObject("btnWebbrowserBack.Image");
			this.btnWebbrowserBack.Location = new global::System.Drawing.Point(41, 3);
			this.btnWebbrowserBack.Name = "btnWebbrowserBack";
			this.btnWebbrowserBack.Size = new global::System.Drawing.Size(23, 23);
			this.btnWebbrowserBack.TabIndex = 1;
			this.toolTip.SetToolTip(this.btnWebbrowserBack, "Back");
			this.btnWebbrowserBack.UseVisualStyleBackColor = true;
			this.btnWebbrowserBack.Click += new global::System.EventHandler(this.btnWebbrowserBack_Click);
			this.btnWebbrowserForward.Image = (global::System.Drawing.Image)resources.GetObject("btnWebbrowserForward.Image");
			this.btnWebbrowserForward.Location = new global::System.Drawing.Point(70, 3);
			this.btnWebbrowserForward.Name = "btnWebbrowserForward";
			this.btnWebbrowserForward.Size = new global::System.Drawing.Size(23, 23);
			this.btnWebbrowserForward.TabIndex = 2;
			this.toolTip.SetToolTip(this.btnWebbrowserForward, "Forward");
			this.btnWebbrowserForward.UseVisualStyleBackColor = true;
			this.btnWebbrowserForward.Click += new global::System.EventHandler(this.btnWebbrowserForward_Click);
			this.btnWebbrowserRefresh.Image = (global::System.Drawing.Image)resources.GetObject("btnWebbrowserRefresh.Image");
			this.btnWebbrowserRefresh.Location = new global::System.Drawing.Point(99, 3);
			this.btnWebbrowserRefresh.Name = "btnWebbrowserRefresh";
			this.btnWebbrowserRefresh.Size = new global::System.Drawing.Size(23, 23);
			this.btnWebbrowserRefresh.TabIndex = 3;
			this.toolTip.SetToolTip(this.btnWebbrowserRefresh, "Refresh");
			this.btnWebbrowserRefresh.UseVisualStyleBackColor = true;
			this.btnWebbrowserRefresh.Click += new global::System.EventHandler(this.btnWebbrowserRefresh_Click);
			this.btnWebbrowserStop.Image = (global::System.Drawing.Image)resources.GetObject("btnWebbrowserStop.Image");
			this.btnWebbrowserStop.Location = new global::System.Drawing.Point(128, 3);
			this.btnWebbrowserStop.Name = "btnWebbrowserStop";
			this.btnWebbrowserStop.Size = new global::System.Drawing.Size(23, 23);
			this.btnWebbrowserStop.TabIndex = 4;
			this.toolTip.SetToolTip(this.btnWebbrowserStop, "Stop");
			this.btnWebbrowserStop.UseVisualStyleBackColor = true;
			this.btnWebbrowserStop.Click += new global::System.EventHandler(this.btnWebbrowserStop_Click);
			this.bnWebrowserGo.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this.bnWebrowserGo.Image = (global::System.Drawing.Image)resources.GetObject("bnWebrowserGo.Image");
			this.bnWebrowserGo.Location = new global::System.Drawing.Point(1149, 3);
			this.bnWebrowserGo.Name = "bnWebrowserGo";
			this.bnWebrowserGo.Size = new global::System.Drawing.Size(23, 23);
			this.bnWebrowserGo.TabIndex = 7;
			this.toolTip.SetToolTip(this.bnWebrowserGo, "Go!");
			this.bnWebrowserGo.UseVisualStyleBackColor = true;
			this.bnWebrowserGo.Click += new global::System.EventHandler(this.bnWebrowserGo_Click);
			this.panel1.Controls.Add(this.bnWebrowserGo);
			this.panel1.Controls.Add(this.tbWebbrowserUrl);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.btnWebbrowserStop);
			this.panel1.Controls.Add(this.btnWebbrowserRefresh);
			this.panel1.Controls.Add(this.btnWebbrowserForward);
			this.panel1.Controls.Add(this.btnWebbrowserBack);
			this.panel1.Controls.Add(this.btnBrowserHome);
			this.panel1.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new global::System.Drawing.Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new global::System.Drawing.Size(1184, 30);
			this.panel1.TabIndex = 2;
			this.tbWebbrowserUrl.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this.tbWebbrowserUrl.Location = new global::System.Drawing.Point(211, 5);
			this.tbWebbrowserUrl.Name = "tbWebbrowserUrl";
			this.tbWebbrowserUrl.Size = new global::System.Drawing.Size(932, 20);
			this.tbWebbrowserUrl.TabIndex = 6;
			this.tbWebbrowserUrl.TextChanged += new global::System.EventHandler(this.tbWebbrowserUrl_TextChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(157, 8);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(48, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Address:";
			this.btnExport.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this.btnExport.BackColor = global::System.Drawing.Color.Red;
			this.btnExport.ForeColor = global::System.Drawing.Color.White;
			this.btnExport.Location = new global::System.Drawing.Point(1097, 3);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new global::System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 14;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = false;
			this.btnExport.Click += new global::System.EventHandler(this.btnExport_Click);
			this.btnStop.Image = (global::System.Drawing.Image)resources.GetObject("btnStop.Image");
			this.btnStop.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.btnStop.Location = new global::System.Drawing.Point(99, 3);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new global::System.Drawing.Size(106, 23);
			this.btnStop.TabIndex = 13;
			this.btnStop.Text = "Stop collection";
			this.btnStop.TextAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new global::System.EventHandler(this.btnStop_Click);
			this.btnDeleteAll.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this.btnDeleteAll.Location = new global::System.Drawing.Point(1016, 3);
			this.btnDeleteAll.Name = "btnDeleteAll";
			this.btnDeleteAll.Size = new global::System.Drawing.Size(75, 23);
			this.btnDeleteAll.TabIndex = 12;
			this.btnDeleteAll.Text = "Delete all";
			this.btnDeleteAll.UseVisualStyleBackColor = true;
			this.btnDeleteAll.Click += new global::System.EventHandler(this.btnDeleteAll_Click);
			this.btnDeleteSelected.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this.btnDeleteSelected.Location = new global::System.Drawing.Point(914, 3);
			this.btnDeleteSelected.Name = "btnDeleteSelected";
			this.btnDeleteSelected.Size = new global::System.Drawing.Size(96, 23);
			this.btnDeleteSelected.TabIndex = 11;
			this.btnDeleteSelected.Text = "Delete selected";
			this.btnDeleteSelected.UseVisualStyleBackColor = true;
			this.btnDeleteSelected.Click += new global::System.EventHandler(this.btnDeleteSelected_Click);
			this.btnClearSelection.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this.btnClearSelection.Location = new global::System.Drawing.Point(812, 3);
			this.btnClearSelection.Name = "btnClearSelection";
			this.btnClearSelection.Size = new global::System.Drawing.Size(96, 23);
			this.btnClearSelection.TabIndex = 10;
			this.btnClearSelection.Text = "Clear selection";
			this.btnClearSelection.UseVisualStyleBackColor = true;
			this.btnClearSelection.Click += new global::System.EventHandler(this.btnClearSelection_Click);
			this.btnSelectAll.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this.btnSelectAll.Location = new global::System.Drawing.Point(731, 3);
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Size = new global::System.Drawing.Size(75, 23);
			this.btnSelectAll.TabIndex = 9;
			this.btnSelectAll.Text = "Select all";
			this.btnSelectAll.UseVisualStyleBackColor = true;
			this.btnSelectAll.Click += new global::System.EventHandler(this.btnSelectAll_Click);
			this.btnGetData.Image = (global::System.Drawing.Image)resources.GetObject("btnGetData.Image");
			this.btnGetData.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.btnGetData.Location = new global::System.Drawing.Point(12, 3);
			this.btnGetData.Name = "btnGetData";
			this.btnGetData.Size = new global::System.Drawing.Size(81, 23);
			this.btnGetData.TabIndex = 8;
			this.btnGetData.Text = "Get data";
			this.btnGetData.TextAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.btnGetData.UseVisualStyleBackColor = true;
			this.btnGetData.Click += new global::System.EventHandler(this.btnGetData_Click);
			this.splitContainer.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new global::System.Drawing.Point(0, 54);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.splitContainer.Panel1.Controls.Add(this.webControl);
			this.splitContainer.Panel1.Controls.Add(this.panel2);
			this.splitContainer.Panel2.Controls.Add(this.dgvResults);
			this.splitContainer.Size = new global::System.Drawing.Size(1184, 731);
			this.splitContainer.SplitterDistance = 425;
			this.splitContainer.TabIndex = 3;
			this.webControl.BackColor = global::System.Drawing.Color.White;
			this.webControl.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.webControl.Location = new global::System.Drawing.Point(0, 0);
			this.webControl.Name = "webControl";
			this.webControl.Size = new global::System.Drawing.Size(1184, 397);
			this.webControl.TabIndex = 2;
			this.webControl.Text = "webControl1";
			this.webControl.WebView = this.webView;
			this.webView.LoadCompleted += new global::EO.WebBrowser.LoadCompletedEventHandler(this.webView_LoadCompleted);
			this.panel2.Controls.Add(this.btnExport);
			this.panel2.Controls.Add(this.btnGetData);
			this.panel2.Controls.Add(this.btnStop);
			this.panel2.Controls.Add(this.btnSelectAll);
			this.panel2.Controls.Add(this.btnDeleteAll);
			this.panel2.Controls.Add(this.btnClearSelection);
			this.panel2.Controls.Add(this.btnDeleteSelected);
			this.panel2.Dock = global::System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new global::System.Drawing.Point(0, 397);
			this.panel2.Name = "panel2";
			this.panel2.Size = new global::System.Drawing.Size(1184, 28);
			this.panel2.TabIndex = 1;
			this.dgvResults.AllowUserToAddRows = false;
			this.dgvResults.AllowUserToDeleteRows = false;
			this.dgvResults.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvResults.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[]
			{
				this.category, this.business_name, this.address, this.city, this.state, this.postal_code, this.country, this.phone, this.fax, this.website,
				this.email, this.map_link, this.details_link
			});
			this.dgvResults.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.dgvResults.Location = new global::System.Drawing.Point(0, 0);
			this.dgvResults.Name = "dgvResults";
			this.dgvResults.ReadOnly = true;
			this.dgvResults.RowHeadersWidth = 11;
			this.dgvResults.SelectionMode = global::System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvResults.Size = new global::System.Drawing.Size(1184, 302);
			this.dgvResults.TabIndex = 0;
			this.dgvResults.CellClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResults_CellClick);
			this.dgvResults.CellContentClick += new global::System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResults_CellContentClick);
			this.dgvResults.CellMouseMove += new global::System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvResults_CellMouseMove);
			dataGridViewCellStyle8.BackColor = global::System.Drawing.Color.FromArgb(255, 255, 192);
			this.category.DefaultCellStyle = dataGridViewCellStyle8;
			this.category.HeaderText = "Category";
			this.category.Name = "category";
			this.category.ReadOnly = true;
			this.business_name.HeaderText = "Business name";
			this.business_name.Name = "business_name";
			this.business_name.ReadOnly = true;
			this.business_name.Width = 150;
			dataGridViewCellStyle9.BackColor = global::System.Drawing.Color.FromArgb(255, 255, 192);
			this.address.DefaultCellStyle = dataGridViewCellStyle9;
			this.address.HeaderText = "Address";
			this.address.Name = "address";
			this.address.ReadOnly = true;
			this.address.Width = 150;
			this.city.HeaderText = "City";
			this.city.Name = "city";
			this.city.ReadOnly = true;
			dataGridViewCellStyle10.BackColor = global::System.Drawing.Color.FromArgb(255, 255, 192);
			this.state.DefaultCellStyle = dataGridViewCellStyle10;
			this.state.HeaderText = "State";
			this.state.Name = "state";
			this.state.ReadOnly = true;
			this.state.Width = 50;
			this.postal_code.HeaderText = "Zip code";
			this.postal_code.Name = "postal_code";
			this.postal_code.ReadOnly = true;
			this.postal_code.Width = 80;
			dataGridViewCellStyle11.BackColor = global::System.Drawing.Color.FromArgb(255, 255, 192);
			this.country.DefaultCellStyle = dataGridViewCellStyle11;
			this.country.HeaderText = "Country";
			this.country.Name = "country";
			this.country.ReadOnly = true;
			this.phone.HeaderText = "Phone";
			this.phone.Name = "phone";
			this.phone.ReadOnly = true;
			this.phone.Width = 150;
			dataGridViewCellStyle12.BackColor = global::System.Drawing.Color.FromArgb(255, 255, 192);
			this.fax.DefaultCellStyle = dataGridViewCellStyle12;
			this.fax.HeaderText = "Fax";
			this.fax.Name = "fax";
			this.fax.ReadOnly = true;
			this.website.HeaderText = "Website";
			this.website.Name = "website";
			this.website.ReadOnly = true;
			this.website.Width = 150;
			dataGridViewCellStyle13.BackColor = global::System.Drawing.Color.FromArgb(255, 255, 192);
			this.email.DefaultCellStyle = dataGridViewCellStyle13;
			this.email.HeaderText = "Email";
			this.email.Name = "email";
			this.email.ReadOnly = true;
			this.email.Width = 150;
			this.map_link.HeaderText = "MapLink";
			this.map_link.Name = "map_link";
			this.map_link.ReadOnly = true;
			dataGridViewCellStyle14.BackColor = global::System.Drawing.Color.FromArgb(255, 255, 192);
			this.details_link.DefaultCellStyle = dataGridViewCellStyle14;
			this.details_link.HeaderText = "Details Link";
			this.details_link.Name = "details_link";
			this.details_link.ReadOnly = true;
			this.details_link.Width = 250;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(1184, 811);
			base.Controls.Add(this.splitContainer);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.statusStrip);
			base.Controls.Add(this.menuStrip);
			base.Icon = null;
			base.MainMenuStrip = this.menuStrip;
			base.Name = "MainForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MASA Yellow Leads Extractor";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			base.Load += new global::System.EventHandler(this.MainForm_Load);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.splitContainer).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dgvResults).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400002F RID: 47
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000030 RID: 48
		private global::System.Windows.Forms.StatusStrip statusStrip;

		// Token: 0x04000031 RID: 49
		public global::System.Windows.Forms.MenuStrip menuStrip;

		// Token: 0x04000032 RID: 50
		private global::System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;

		// Token: 0x04000033 RID: 51
		private global::System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;

		// Token: 0x04000034 RID: 52
		private global::System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

		// Token: 0x04000035 RID: 53
		private global::System.Windows.Forms.ToolStripMenuItem websitesToolStripMenuItem;

		// Token: 0x04000036 RID: 54
		private global::System.Windows.Forms.ToolStripMenuItem wwwpaginegialleitToolStripMenuItem;

		// Token: 0x04000037 RID: 55
		private global::System.Windows.Forms.ToolStripMenuItem wwwpagesjaunesToolStripMenuItem;

		// Token: 0x04000038 RID: 56
		private global::System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;

		// Token: 0x04000039 RID: 57
		private global::System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;

		// Token: 0x0400003C RID: 60
		private global::System.Windows.Forms.ToolStripMenuItem aboutOnlineYellowPagesScraperToolStripMenuItem;

		// Token: 0x0400003D RID: 61
		private global::System.Windows.Forms.ToolTip toolTip;

		// Token: 0x0400003E RID: 62
		private global::System.Windows.Forms.Panel panel1;

		// Token: 0x0400003F RID: 63
		private global::System.Windows.Forms.SplitContainer splitContainer;

		// Token: 0x04000040 RID: 64
		public global::System.Windows.Forms.DataGridView dgvResults;

		// Token: 0x04000041 RID: 65
		private global::System.Windows.Forms.Button bnWebrowserGo;

		// Token: 0x04000042 RID: 66
		private global::System.Windows.Forms.TextBox tbWebbrowserUrl;

		// Token: 0x04000043 RID: 67
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000044 RID: 68
		private global::System.Windows.Forms.Button btnWebbrowserStop;

		// Token: 0x04000045 RID: 69
		private global::System.Windows.Forms.Button btnWebbrowserRefresh;

		// Token: 0x04000046 RID: 70
		private global::System.Windows.Forms.Button btnWebbrowserForward;

		// Token: 0x04000047 RID: 71
		private global::System.Windows.Forms.Button btnWebbrowserBack;

		// Token: 0x04000048 RID: 72
		private global::System.Windows.Forms.Button btnBrowserHome;

		// Token: 0x04000049 RID: 73
		private global::System.Windows.Forms.ToolStripMenuItem wwwgelbenseitendeToolStripMenuItem;

		// Token: 0x0400004A RID: 74
		private global::System.Windows.Forms.Button btnExport;

		// Token: 0x0400004B RID: 75
		private global::System.Windows.Forms.Button btnStop;

		// Token: 0x0400004C RID: 76
		private global::System.Windows.Forms.Button btnDeleteAll;

		// Token: 0x0400004D RID: 77
		private global::System.Windows.Forms.Button btnDeleteSelected;

		// Token: 0x0400004E RID: 78
		private global::System.Windows.Forms.Button btnClearSelection;

		// Token: 0x0400004F RID: 79
		private global::System.Windows.Forms.Button btnSelectAll;

		// Token: 0x04000050 RID: 80
		private global::System.Windows.Forms.Button btnGetData;

		// Token: 0x04000051 RID: 81
		private global::System.Windows.Forms.ToolStripStatusLabel tssLabelStatus;

		// Token: 0x04000052 RID: 82
		public global::System.Windows.Forms.ToolStripStatusLabel tssLabelListed;

		// Token: 0x04000053 RID: 83
		private global::System.Windows.Forms.ToolStripStatusLabel tssLabelExported;

		// Token: 0x04000054 RID: 84
		private global::System.Windows.Forms.ToolStripStatusLabel tssLabelProgress;

		// Token: 0x04000055 RID: 85
		public global::System.Windows.Forms.ToolStripProgressBar tsProgress;

		// Token: 0x04000056 RID: 86
		private global::System.Windows.Forms.Panel panel2;

		// Token: 0x04000057 RID: 87
		private global::EO.WinForm.WebControl webControl;

		// Token: 0x04000059 RID: 89
		private global::EO.WebBrowser.WebView webView;

		// Token: 0x0400005A RID: 90
		private global::System.Windows.Forms.DataGridViewTextBoxColumn category;

		// Token: 0x0400005B RID: 91
		private global::System.Windows.Forms.DataGridViewTextBoxColumn business_name;

		// Token: 0x0400005C RID: 92
		private global::System.Windows.Forms.DataGridViewTextBoxColumn address;

		// Token: 0x0400005D RID: 93
		private global::System.Windows.Forms.DataGridViewTextBoxColumn city;

		// Token: 0x0400005E RID: 94
		private global::System.Windows.Forms.DataGridViewTextBoxColumn state;

		// Token: 0x0400005F RID: 95
		private global::System.Windows.Forms.DataGridViewTextBoxColumn postal_code;

		// Token: 0x04000060 RID: 96
		private global::System.Windows.Forms.DataGridViewTextBoxColumn country;

		// Token: 0x04000061 RID: 97
		private global::System.Windows.Forms.DataGridViewTextBoxColumn phone;

		// Token: 0x04000062 RID: 98
		private global::System.Windows.Forms.DataGridViewTextBoxColumn fax;

		// Token: 0x04000063 RID: 99
		private global::System.Windows.Forms.DataGridViewTextBoxColumn website;

		// Token: 0x04000064 RID: 100
		private global::System.Windows.Forms.DataGridViewTextBoxColumn email;

		// Token: 0x04000065 RID: 101
		private global::System.Windows.Forms.DataGridViewTextBoxColumn map_link;

		// Token: 0x04000066 RID: 102
		private global::System.Windows.Forms.DataGridViewTextBoxColumn details_link;

		// Token: 0x04000067 RID: 103
		private global::System.Windows.Forms.ToolStripMenuItem paginasAmarillasesToolStripMenuItem;

		// Token: 0x04000068 RID: 104
		private global::System.Windows.Forms.ToolStripMenuItem yellowPagescomUSAToolStripMenuItem;

		// Token: 0x04000069 RID: 105
		private global::System.Windows.Forms.ToolStripMenuItem yellcomUKToolStripMenuItem;

		// Token: 0x0400006A RID: 106
		private global::System.Windows.Forms.ToolStripMenuItem infobelcomToolStripMenuItem;

		// Token: 0x0400006B RID: 107
		private global::System.Windows.Forms.ToolStripMenuItem yELPcomToolStripMenuItem;
	}
}

namespace MASAYellowLeadsExtractor
{
	// Token: 0x0200000F RID: 15
	public partial class ProxiesForm : global::System.Windows.Forms.Form
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00008868 File Offset: 0x00006A68
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00008888 File Offset: 0x00006A88
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::MASAYellowLeadsExtractor.ProxiesForm));
			this.lblInfo = new global::System.Windows.Forms.Label();
			this.dgv = new global::System.Windows.Forms.DataGridView();
			this.nbr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.proxy = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.port = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.status = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label1 = new global::System.Windows.Forms.Label();
			((global::System.ComponentModel.ISupportInitialize)this.dgv).BeginInit();
			base.SuspendLayout();
			this.lblInfo.AutoSize = true;
			this.lblInfo.Location = new global::System.Drawing.Point(12, 439);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new global::System.Drawing.Size(100, 13);
			this.lblInfo.TabIndex = 5;
			this.lblInfo.Text = "Searching proxies...";
			this.dgv.AllowUserToAddRows = false;
			this.dgv.AllowUserToDeleteRows = false;
			this.dgv.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this.dgv.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.nbr, this.proxy, this.port, this.status });
			this.dgv.Location = new global::System.Drawing.Point(15, 25);
			this.dgv.Name = "dgv";
			this.dgv.ReadOnly = true;
			this.dgv.RowHeadersWidth = 11;
			this.dgv.Size = new global::System.Drawing.Size(257, 396);
			this.dgv.TabIndex = 4;
			this.nbr.HeaderText = "#";
			this.nbr.Name = "nbr";
			this.nbr.ReadOnly = true;
			this.nbr.Width = 30;
			this.proxy.HeaderText = "Proxy";
			this.proxy.Name = "proxy";
			this.proxy.ReadOnly = true;
			this.port.HeaderText = "Port";
			this.port.Name = "port";
			this.port.ReadOnly = true;
			this.port.Width = 50;
			this.status.HeaderText = "Status";
			this.status.Name = "status";
			this.status.ReadOnly = true;
			this.status.Width = 50;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(115, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Available proxy servers";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(284, 461);
			base.Controls.Add(this.lblInfo);
			base.Controls.Add(this.dgv);
			base.Controls.Add(this.label1);
			base.Icon = null;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ProxiesForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Searching proxies...";
			base.Shown += new global::System.EventHandler(this.ProxiesForm_Shown);
			((global::System.ComponentModel.ISupportInitialize)this.dgv).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000079 RID: 121
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400007A RID: 122
		private global::System.Windows.Forms.Label lblInfo;

		// Token: 0x0400007B RID: 123
		private global::System.Windows.Forms.DataGridView dgv;

		// Token: 0x0400007C RID: 124
		private global::System.Windows.Forms.DataGridViewTextBoxColumn nbr;

		// Token: 0x0400007D RID: 125
		private global::System.Windows.Forms.DataGridViewTextBoxColumn proxy;

		// Token: 0x0400007E RID: 126
		private global::System.Windows.Forms.DataGridViewTextBoxColumn port;

		// Token: 0x0400007F RID: 127
		private global::System.Windows.Forms.DataGridViewTextBoxColumn status;

		// Token: 0x04000080 RID: 128
		private global::System.Windows.Forms.Label label1;
	}
}

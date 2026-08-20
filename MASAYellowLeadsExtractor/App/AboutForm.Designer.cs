namespace MASAYellowLeadsExtractor
{
	// Token: 0x02000006 RID: 6
	public partial class AboutForm : global::System.Windows.Forms.Form
	{
		// Token: 0x0600002E RID: 46 RVA: 0x000032E9 File Offset: 0x000014E9
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003308 File Offset: 0x00001508
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::MASAYellowLeadsExtractor.AboutForm));
			this.btnOk = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.btnOk.Location = new global::System.Drawing.Point(205, 226);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new global::System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(484, 261);
			base.Controls.Add(this.btnOk);
			base.Icon = null;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About MASA Yellow Leads Extractor";
			base.ResumeLayout(false);
		}

		// Token: 0x0400000B RID: 11
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400000C RID: 12
		private global::System.Windows.Forms.Button btnOk;
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MASAYellowLeadsExtractor
{
	// Token: 0x02000006 RID: 6
	public partial class AboutForm : Form
	{
		// Token: 0x0600002C RID: 44 RVA: 0x000032D3 File Offset: 0x000014D3
		public AboutForm()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000032E1 File Offset: 0x000014E1
		private void btnOk_Click(object sender, EventArgs e)
		{
			base.Close();
		}
	}
}

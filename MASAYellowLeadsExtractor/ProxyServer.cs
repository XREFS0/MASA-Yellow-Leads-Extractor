using System;
using System.Threading;

// Token: 0x02000004 RID: 4
public class ProxyServer
{
	// Token: 0x06000013 RID: 19 RVA: 0x00002A5E File Offset: 0x00000C5E
	public ProxyServer()
	{
		this.Checked = false;
		this.Processed = false;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002A74 File Offset: 0x00000C74
	private void DoCheckProxy()
	{
		string SourcePageHTML = HTTPScraper.GetPage("http://www.equibase.com/profiles/Results.cfm?type=Horse&refno=8685211&registry=T&rbt=TB", this);
		this.CanUse = SourcePageHTML.IndexOf("Aldous Snow") > -1;
		this.Checked = true;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002AA8 File Offset: 0x00000CA8
	public void CheckProxy()
	{
		this.Checked = false;
		this.Processed = false;
		new Thread(new ThreadStart(this.DoCheckProxy)).Start();
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002AD0 File Offset: 0x00000CD0
	public void CheckProxyAndWait()
	{
		string SourcePageHTML = HTTPScraper.GetPage("http://www.google.com", this);
		this.CanUse = SourcePageHTML.IndexOf("Google") > -1;
	}

	// Token: 0x04000004 RID: 4
	public string IP;

	// Token: 0x04000005 RID: 5
	public int Port;

	// Token: 0x04000006 RID: 6
	public bool CanUse;

	// Token: 0x04000007 RID: 7
	public bool Checked;

	// Token: 0x04000008 RID: 8
	public bool Processed;
}

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace MASAYellowLeadsExtractor
{
	// Token: 0x02000012 RID: 18
	public class Settings
	{
		// Token: 0x060000AF RID: 175
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern uint VerLanguageName(uint wLang, [Out] char[] szLang, int nSize);

		// Token: 0x060000B0 RID: 176 RVA: 0x00009830 File Offset: 0x00007A30
		public Settings()
		{
			CultureInfo Culture = CultureInfo.CurrentCulture;
			if (Culture.Name.IndexOf("it") > -1)
			{
				this.Language = 1;
			}
			else if (Culture.Name.IndexOf("de") > -1)
			{
				this.Language = 2;
			}
			else if (Culture.Name.IndexOf("fr") > -1)
			{
				this.Language = 3;
			}
			else if (Culture.Name.IndexOf("es") > -1)
			{
				this.Language = 4;
			}
			else
			{
				this.Language = 0;
			}
			this.ColumnsToShow = new bool[13];
			this.ColumnsToExport = new bool[13];
			for (int i = 0; i < 13; i++)
			{
				this.ColumnsToShow[i] = true;
				this.ColumnsToExport[i] = true;
			}
			this.ExtractEmails = true;
			this.AutoExport = false;
			this.ProxySourcesList = new string[]
			{
				"http://gatherproxy.com/proxylist/country/?c=United%20States", "http://gatherproxy.com/proxylist/country/?c=Canada", "http://txt.proxyspy.net/proxy.txt", "http://dogdev.net/Proxy/US?port=8080", "", "", "", "", "", "",
				"", "", "", ""
			};
			this.CSVDelimiter = 1;
			this.CSVEncoding = 2;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00009998 File Offset: 0x00007B98
		public bool Save(string FName)
		{
			XmlSerializer writer = new XmlSerializer(typeof(Settings));
			bool flag;
			try
			{
				StreamWriter file = new StreamWriter(FName);
				writer.Serialize(file, this);
				file.Close();
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000099E4 File Offset: 0x00007BE4
		public static Settings Load(string FName)
		{
			XmlSerializer reader = new XmlSerializer(typeof(Settings));
			Settings settings2;
			try
			{
				StreamReader file = new StreamReader(FName);
				Settings settings = (Settings)reader.Deserialize(file);
				file.Close();
				settings2 = settings;
			}
			catch
			{
				settings2 = new Settings();
			}
			return settings2;
		}

		// Token: 0x04000093 RID: 147
		public int Language;

		// Token: 0x04000094 RID: 148
		public bool[] ColumnsToShow;

		// Token: 0x04000095 RID: 149
		public bool[] ColumnsToExport;

		// Token: 0x04000096 RID: 150
		public bool AutoExport;

		// Token: 0x04000097 RID: 151
		public string AutoExportPath;

		// Token: 0x04000098 RID: 152
		public bool ExtractEmails;

		// Token: 0x04000099 RID: 153
		public int ExportType;

		// Token: 0x0400009A RID: 154
		public int CSVDelimiter;

		// Token: 0x0400009B RID: 155
		public int CSVEncoding;

		// Token: 0x0400009C RID: 156
		public int ConnectionType;

		// Token: 0x0400009D RID: 157
		public string ProxyServer;

		// Token: 0x0400009E RID: 158
		public int ProxyPort;

		// Token: 0x0400009F RID: 159
		public bool ProxyAuthentification;

		// Token: 0x040000A0 RID: 160
		public string ProxyAuthLogin;

		// Token: 0x040000A1 RID: 161
		public string ProxyAuthPassword;

		// Token: 0x040000A2 RID: 162
		public string[] ProxyList;

		// Token: 0x040000A3 RID: 163
		public string[] ProxySourcesList;

		// Token: 0x040000A4 RID: 164
		public bool IsRandomDelay;

		// Token: 0x040000A5 RID: 165
		public int DelayFrom;

		// Token: 0x040000A6 RID: 166
		public int DelayTo;
	}
}

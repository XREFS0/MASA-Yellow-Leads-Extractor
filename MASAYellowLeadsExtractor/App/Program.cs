using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MASAYellowLeadsExtractor
{
	// Token: 0x0200000E RID: 14
	internal static class Program
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00008045 File Offset: 0x00006245
		public static void RequestDelay()
		{
			if (Program.AppSettings.IsRandomDelay)
			{
				Thread.Sleep((int)(1000.0 * (double)Program.Rnd.Next(Program.AppSettings.DelayFrom, Program.AppSettings.DelayTo)));
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00008082 File Offset: 0x00006282
		public static bool IsStopped()
		{
			if (Program.StopDataCollection)
			{
				MessageBox.Show(Program.LanguagesManager.StoppedByUser);
				return true;
			}
			return false;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000080A0 File Offset: 0x000062A0
		[STAThread]
		private static void Main()
		{
			Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MASA Yellow Leads Extractor");
			Program.SettingsFileName = string.Format("{0}\\MASA Yellow Leads Extractor\\settings.cfg", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
			Program.ExportFile = string.Format("{0}\\MASA Yellow Leads Extractor\\export.txt", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
			Program.Rnd = new Random(DateTime.Now.Millisecond);
			Program.LanguagesManager = new Languages();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		// Token: 0x0400006C RID: 108
		public static string SettingsFileName;

		// Token: 0x0400006D RID: 109
		public static string ExportFile;

		// Token: 0x04000070 RID: 112
		public static Random Rnd;

		// Token: 0x04000071 RID: 113
		public static Settings AppSettings;

		// Token: 0x04000073 RID: 115
		public static Languages LanguagesManager;

		// Token: 0x04000074 RID: 116
		public static string[] LanguagesFiles = new string[]
		{
			Application.StartupPath + "\\languages\\lang-en.txt",
			Application.StartupPath + "\\languages\\lang-it.txt",
			Application.StartupPath + "\\languages\\lang-ge.txt",
			Application.StartupPath + "\\languages\\lang-fr.txt",
			Application.StartupPath + "\\languages\\lang-sp.txt"
		};

		// Token: 0x04000075 RID: 117
		public static bool StopDataCollection = false;
	}
}

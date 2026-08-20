using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace MASAYellowLeadsExtractor.Properties
{
	// Token: 0x02000015 RID: 21
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.11.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000BF8C File Offset: 0x0000A18C
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040000D8 RID: 216
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}

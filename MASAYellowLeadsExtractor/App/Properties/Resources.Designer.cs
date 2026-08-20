using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MASAYellowLeadsExtractor.Properties
{
	// Token: 0x02000014 RID: 20
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x0000343D File Offset: 0x0000163D
		internal Resources()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000BF51 File Offset: 0x0000A151
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("MASAYellowLeadsExtractor.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x0000BF7D File Offset: 0x0000A17D
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x0000BF84 File Offset: 0x0000A184
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x040000D6 RID: 214
		private static ResourceManager resourceMan;

		// Token: 0x040000D7 RID: 215
		private static CultureInfo resourceCulture;
	}
}

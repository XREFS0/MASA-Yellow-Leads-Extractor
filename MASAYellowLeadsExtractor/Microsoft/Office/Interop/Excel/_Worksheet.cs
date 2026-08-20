using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	// Token: 0x02000074 RID: 116
	[CompilerGenerated]
	[Guid("000208D8-0000-0000-C000-000000000046")]
	[TypeIdentifier]
	[ComImport]
	public interface _Worksheet
	{
		// Token: 0x0600021D RID: 541
		void _VtblGap1_93();

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600021E RID: 542
		[DispId(197)]
		Range Range
		{
			[DispId(197)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x0600021F RID: 543
		void _VtblGap2_16();

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000220 RID: 544
		[DispId(412)]
		Range UsedRange
		{
			[DispId(412)]
			[LCIDConversion(0)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}

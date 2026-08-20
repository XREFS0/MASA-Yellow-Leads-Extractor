using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	// Token: 0x02000072 RID: 114
	[CompilerGenerated]
	[Guid("000208D5-0000-0000-C000-000000000046")]
	[TypeIdentifier]
	[ComImport]
	public interface _Application
	{
		// Token: 0x0600020E RID: 526
		void _VtblGap1_45();

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600020F RID: 527
		[DispId(572)]
		Workbooks Workbooks
		{
			[DispId(572)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x06000210 RID: 528
		void _VtblGap2_60();

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000211 RID: 529
		[DispId(0)]
		string _Default
		{
			[DispId(0)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}

		// Token: 0x06000212 RID: 530
		void _VtblGap3_116();

		// Token: 0x06000213 RID: 531
		[DispId(302)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Quit();

		// Token: 0x06000214 RID: 532
		void _VtblGap4_51();

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000215 RID: 533
		// (set) Token: 0x06000216 RID: 534
		[DispId(558)]
		bool Visible
		{
			[LCIDConversion(0)]
			[DispId(558)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			get;
			[LCIDConversion(0)]
			[DispId(558)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[param: In]
			set;
		}
	}
}

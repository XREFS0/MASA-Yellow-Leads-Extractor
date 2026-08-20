using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	// Token: 0x0200006E RID: 110
	[CompilerGenerated]
	[Guid("000208DB-0000-0000-C000-000000000046")]
	[DefaultMember("_Default")]
	[TypeIdentifier]
	[ComImport]
	public interface Workbooks : IEnumerable
	{
		// Token: 0x0600020A RID: 522
		void _VtblGap1_3();

		// Token: 0x0600020B RID: 523
		[DispId(181)]
		[LCIDConversion(1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		Workbook Add([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Template);

		// Token: 0x0600020C RID: 524
		void _VtblGap2_8();

		// Token: 0x0600020D RID: 525
		[DispId(1923)]
		[LCIDConversion(15)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		Workbook Open([MarshalAs(UnmanagedType.BStr)] [In] string Filename, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object UpdateLinks, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object ReadOnly, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Format, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Password, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object WriteResPassword, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object IgnoreReadOnlyRecommended, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Origin, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Delimiter, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Editable, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Notify, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Converter, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object AddToMru, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Local, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object CorruptLoad);
	}
}

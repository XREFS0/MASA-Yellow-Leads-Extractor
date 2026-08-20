using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	// Token: 0x02000073 RID: 115
	[CompilerGenerated]
	[Guid("000208DA-0000-0000-C000-000000000046")]
	[TypeIdentifier]
	[ComImport]
	public interface _Workbook
	{
		// Token: 0x06000217 RID: 535
		void _VtblGap1_20();

		// Token: 0x06000218 RID: 536
		[LCIDConversion(3)]
		[DispId(277)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Close([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object SaveChanges, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Filename, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object RouteWorkbook);

		// Token: 0x06000219 RID: 537
		void _VtblGap2_103();

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600021A RID: 538
		[DispId(494)]
		Sheets Worksheets
		{
			[DispId(494)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x0600021B RID: 539
		void _VtblGap3_40();

		// Token: 0x0600021C RID: 540
		[DispId(1925)]
		[LCIDConversion(12)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SaveAs([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Filename, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object FileFormat, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Password, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object WriteResPassword, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object ReadOnlyRecommended, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object CreateBackup, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object AccessMode, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object ConflictResolution, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object AddToMru, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object TextCodepage, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object TextVisualLayout, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Local);
	}
}

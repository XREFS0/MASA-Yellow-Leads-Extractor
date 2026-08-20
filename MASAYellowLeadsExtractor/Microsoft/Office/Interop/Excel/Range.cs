using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	// Token: 0x02000069 RID: 105
	[CompilerGenerated]
	[InterfaceType(2)]
	[DefaultMember("_Default")]
	[Guid("00020846-0000-0000-C000-000000000046")]
	[TypeIdentifier]
	[ComImport]
	public interface Range : IEnumerable
	{
		// Token: 0x06000203 RID: 515
		void _VtblGap1_164();

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000204 RID: 516
		[DispId(138)]
		object Text
		{
			[DispId(138)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Struct)]
			get;
		}

		// Token: 0x06000205 RID: 517
		void _VtblGap2_8();

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000206 RID: 518
		// (set) Token: 0x06000207 RID: 519
		[DispId(6)]
		object Value
		{
			[DispId(6)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Struct)]
			get;
			[DispId(6)]
			[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[param: MarshalAs(UnmanagedType.Struct)]
			[param: In]
			[param: Optional]
			set;
		}
	}
}

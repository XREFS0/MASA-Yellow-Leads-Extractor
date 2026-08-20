using System;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	[ComEventInterface(typeof(DocEvents), typeof(DocEvents))]
	[TypeIdentifier("00020813-0000-0000-c000-000000000046", "Microsoft.Office.Interop.Excel.DocEvents_Event")]
	[ComImport]
	[Guid("00020813-0000-0000-c000-000000000046")]
	public interface DocEvents_Event
	{
	}
}

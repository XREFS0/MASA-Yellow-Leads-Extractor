using System;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	[ComEventInterface(typeof(WorkbookEvents), typeof(WorkbookEvents))]
	[TypeIdentifier("00020813-0000-0000-c000-000000000046", "Microsoft.Office.Interop.Excel.WorkbookEvents_Event")]
	[ComImport]
	[Guid("00020813-0000-0000-c000-000000000046")]
	public interface WorkbookEvents_Event
	{
	}
}

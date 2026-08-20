using System;
using System.Reflection;
using System.Runtime.InteropServices;

public class ExcelDocument
{
	public ExcelDocument()
	{
		this.excelApp = Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
		dynamic app = this.excelApp;
		app.Visible = false;
	}

	public void Open(string FileName)
	{
		dynamic app = this.excelApp;
		this.excelWorkbook = app.Workbooks.Open(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		dynamic wb = this.excelWorkbook;
		this.excelWorksheet = wb.Worksheets[1];
	}

	public void Create()
	{
		dynamic app = this.excelApp;
		this.excelWorkbook = app.Workbooks.Add(Type.Missing);
		dynamic wb = this.excelWorkbook;
		this.excelWorksheet = wb.Worksheets[1];
	}

	public void Save(string FileName)
	{
		try
		{
			dynamic wb = this.excelWorkbook;
			wb.SaveAs(FileName, 56, Type.Missing, Type.Missing, Type.Missing, Type.Missing, 4163, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		}
		catch (Exception)
		{
		}
	}

	public string GetCellValue(int Row, int Col)
	{
		string Value = "error";
		try
		{
			string CellName = string.Format("{0}{1}", (char)(65 + Col), 1 + Row);
			dynamic ws = this.excelWorksheet;
			dynamic range = ws.Range[CellName, Type.Missing];
			Value = ((string)range.Text).Trim();
		}
		catch
		{
		}
		return Value;
	}

	public void SetCellValue(int Row, int Col, string Value)
	{
		string CellName = string.Format("{0}{1}", (char)(65 + Col), 1 + Row);
		dynamic ws = this.excelWorksheet;
		dynamic range = ws.Range[CellName, Type.Missing];
		if (Value.Length < 10)
		{
			Value = Value.Replace(",", ".");
		}
		range.Value = Value;
	}

	public object GetUsedRange()
	{
		dynamic ws = this.excelWorksheet;
		return ws.UsedRange;
	}

	public void SetWorksheet(int Index)
	{
		dynamic wb = this.excelWorkbook;
		this.excelWorksheet = wb.Worksheets[Index];
	}

	public void Close()
	{
		dynamic wb = this.excelWorkbook;
		wb.Close(true, Type.Missing, Type.Missing);
		dynamic app = this.excelApp;
		app.Quit();
		this.ReleaseObject(this.excelWorksheet);
		this.ReleaseObject(this.excelWorkbook);
		this.ReleaseObject(this.excelApp);
	}

	private void ReleaseObject(object obj)
	{
		try
		{
			Marshal.ReleaseComObject(obj);
			obj = null;
		}
		catch (Exception)
		{
			obj = null;
		}
		finally
		{
			GC.Collect();
		}
	}

	private object excelApp;
	private object excelWorkbook;
	private object excelWorksheet;
}

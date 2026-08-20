using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MASAYellowLeadsExtractor
{
	// Token: 0x0200000B RID: 11
	public static class ExportManager
	{
		// Token: 0x06000038 RID: 56 RVA: 0x000039BC File Offset: 0x00001BBC
		public static string BuildTxtLine(Settings AppSettings, DataGridView dgv, int RowIndex)
		{
			string Line = "";
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				string Value = "";
				try
				{
					Value = dgv.Rows[RowIndex].Cells[i].Value.ToString();
				}
				catch
				{
				}
				if (AppSettings.ColumnsToExport[i])
				{
					Line += string.Format("\"{0}\"{1}", Value, "\t");
				}
			}
			if (Line.Length > 0)
			{
				Line = Line.Substring(0, Line.Length - 1).Replace(Environment.NewLine, "") + Environment.NewLine;
			}
			return Line;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003A78 File Offset: 0x00001C78
		public static string BuildCSVLine(Settings AppSettings, DataGridView dgv, int RowIndex)
		{
			string Line = "";
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				string Value = "";
				try
				{
					Value = dgv.Rows[RowIndex].Cells[i].Value.ToString();
				}
				catch
				{
				}
				if (AppSettings.ColumnsToExport[i])
				{
					Line += string.Format("{0}{1}", Value, ExportManager.CSVDelimiters[AppSettings.CSVDelimiter]);
				}
			}
			if (Line.Length > 0)
			{
				Line = Line.Substring(0, Line.Length - 1).Replace(Environment.NewLine, "") + Environment.NewLine;
			}
			return Line;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003B3C File Offset: 0x00001D3C
		public static void SaveToText(Settings AppSettings, string FileName, DataGridView dgv)
		{
			Encoding FileEncoding = Encoding.UTF8;
			if (AppSettings.CSVEncoding == 0)
			{
				FileEncoding = Encoding.ASCII;
			}
			else if (AppSettings.CSVEncoding == 1)
			{
				FileEncoding = Encoding.UTF7;
			}
			else if (AppSettings.CSVEncoding == 2)
			{
				FileEncoding = Encoding.UTF8;
			}
			File.WriteAllText(FileName, "", FileEncoding);
			string Line = "";
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				if (AppSettings.ColumnsToExport[i])
				{
					Line += string.Format("{0}{1}", ExportManager.Columns[i], "\t");
				}
			}
			if (Line.Length > 0)
			{
				Line = Line.Substring(0, Line.Length - 1) + Environment.NewLine;
			}
			File.AppendAllText(FileName, Line);
			if (dgv.SelectedRows.Count > 0)
			{
				for (int j = 0; j < dgv.SelectedRows.Count; j++)
				{
					File.AppendAllText(FileName, ExportManager.BuildTxtLine(AppSettings, dgv, dgv.SelectedRows[dgv.SelectedRows.Count - 1 - j].Index), FileEncoding);
				}
				return;
			}
			for (int k = 0; k < dgv.Rows.Count; k++)
			{
				File.AppendAllText(FileName, ExportManager.BuildTxtLine(AppSettings, dgv, k), FileEncoding);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003C74 File Offset: 0x00001E74
		public static void SaveToCSV(Settings AppSettings, string FileName, DataGridView dgv)
		{
			Encoding FileEncoding = Encoding.UTF8;
			if (AppSettings.CSVEncoding == 0)
			{
				FileEncoding = Encoding.ASCII;
			}
			else if (AppSettings.CSVEncoding == 1)
			{
				FileEncoding = Encoding.UTF7;
			}
			else if (AppSettings.CSVEncoding == 2)
			{
				FileEncoding = Encoding.UTF8;
			}
			File.WriteAllText(FileName, "", FileEncoding);
			string Line = "";
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				if (AppSettings.ColumnsToExport[i])
				{
					Line += string.Format("{0}{1}", ExportManager.Columns[i], ExportManager.CSVDelimiters[AppSettings.CSVDelimiter]);
				}
			}
			if (Line.Length > 0)
			{
				Line = Line.Substring(0, Line.Length - 1) + Environment.NewLine;
			}
			File.AppendAllText(FileName, Line);
			if (dgv.SelectedRows.Count > 0)
			{
				for (int j = 0; j < dgv.SelectedRows.Count; j++)
				{
					File.AppendAllText(FileName, ExportManager.BuildCSVLine(AppSettings, dgv, dgv.SelectedRows[dgv.SelectedRows.Count - 1 - j].Index), FileEncoding);
				}
				return;
			}
			for (int k = 0; k < dgv.Rows.Count; k++)
			{
				File.AppendAllText(FileName, ExportManager.BuildCSVLine(AppSettings, dgv, k), FileEncoding);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public static void SaveToXLS(Settings AppSettings, string FileName, DataGridView dgv)
		{
			ExcelDocument doc = new ExcelDocument();
			doc.Create();
			int ColIndex = 0;
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				if (AppSettings.ColumnsToExport[i])
				{
					doc.SetCellValue(0, ColIndex, ExportManager.Columns[i]);
					ColIndex++;
				}
			}
			if (dgv.SelectedRows.Count > 0)
			{
				for (int j = 0; j < dgv.SelectedRows.Count; j++)
				{
					ColIndex = 0;
					for (int k = 0; k < dgv.Columns.Count; k++)
					{
						if (AppSettings.ColumnsToExport[k])
						{
							string Value = "";
							try
							{
								Value = dgv.Rows[dgv.SelectedRows[dgv.SelectedRows.Count - 1 - j].Index].Cells[k].Value.ToString();
							}
							catch
							{
							}
							doc.SetCellValue(j + 1, ColIndex, Value);
							ColIndex++;
						}
					}
				}
			}
			else
			{
				for (int l = 0; l < dgv.Rows.Count; l++)
				{
					ColIndex = 0;
					for (int m = 0; m < dgv.Columns.Count; m++)
					{
						if (AppSettings.ColumnsToExport[m])
						{
							string Value2 = "";
							try
							{
								Value2 = dgv.Rows[l].Cells[m].Value.ToString();
							}
							catch
							{
							}
							doc.SetCellValue(l + 1, ColIndex, Value2);
							ColIndex++;
						}
					}
				}
			}
			doc.Save(FileName);
			doc.Close();
		}

		// Token: 0x0400001A RID: 26
		private static string[] Columns = new string[]
		{
			"Category", "Business Name", "Address", "City", "State", "Zip Code", "Country", "Phone", "Fax", "Website",
			"Email", "Maplink", "Details Link"
		};

		// Token: 0x0400001B RID: 27
		private static string[] CSVDelimiters = new string[] { ",", ";" };
	}
}

using Avalonia.Controls;
using CsvHelper;
using MiniExcelLibs;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace CodeWF.AvaloniaControls.Extensions;

public static class DataGridExtensions
{
    public static bool ExportToCsv(this DataGrid dataGrid, string filePath, out string errorMsg,
        bool containHeader = true)
    {
        errorMsg = "";
        try
        {
            dataGrid.GetDataGridData(out errorMsg, out var dt);
            using var writer = new StreamWriter(filePath, false, Encoding.Default);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            if (containHeader)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    csv.WriteField(dc.ColumnName);
                }

                csv.NextRecord();
            }

            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    csv.WriteField(dr[dc]);
                }

                csv.NextRecord();
            }

            return true;
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;
            return false;
        }
    }

    public static bool ExportToExcel(this DataGrid dataGrid, string filePath, out string errorMsg,
        bool containHeader = true)
    {
        errorMsg = "";
        try
        {
            dataGrid.GetDataGridData(out errorMsg, out var dt);

            MiniExcel.SaveAs(filePath, dt);
            return true;
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;
            return false;
        }
    }

    public static bool GetDataGridData(this DataGrid dataGrid, out string? errorMsg, out DataTable dataTable)
    {
        errorMsg = default;

        dataTable = new DataTable();

        foreach (var column in dataGrid.Columns)
        {
            dataTable.Columns.Add(column.Header.ToString());
        }

        var itemsSource = dataGrid.ItemsSource;
        if (itemsSource != null)
        {
            foreach (var item in itemsSource)
            {
                var row = dataTable.NewRow();
                for (int colIndex = 0; colIndex < dataGrid.Columns.Count; colIndex++)
                {
                    var cellContent = dataGrid.Columns[colIndex].GetCellContent(item);
                    if (cellContent is TextBlock textBlock)
                    {
                        row[colIndex] = textBlock.Text ?? "";
                    }
                    else
                    {
                        row[colIndex] = cellContent?.ToString();
                    }
                }

                dataTable.Rows.Add(row);
            }
        }

        return true;
    }
}
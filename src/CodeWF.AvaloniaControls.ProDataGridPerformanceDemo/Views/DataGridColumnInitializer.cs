using Avalonia.Controls;
using Avalonia.Data;
using CodeWF.AvaloniaControls;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.Views;

internal static class DataGridColumnInitializer
{
    public static void EnsureProcessColumns(DataGrid dataGrid)
    {
        if (dataGrid.Columns.Count > 0)
        {
            return;
        }

        AddTextColumn(dataGrid, "编号", 90, "Id");
        AddTextColumn(dataGrid, "名称", 180, "Name");
        AddTextColumn(dataGrid, "启用", 90, "Enabled");
        AddTextColumn(dataGrid, "源节点", 90, "SourceNode");
        AddTextColumn(dataGrid, "主机", 180, "Host");
        AddTextColumn(dataGrid, "程序路径", 240, "ProgramPath");
        AddTextColumn(dataGrid, "工作路径", 220, "WorkPath");
        AddTextColumn(dataGrid, "参数", 160, "Params");
        AddTextColumn(dataGrid, "自动启动", 100, "AutoStart");
        AddTextColumn(dataGrid, "前置处理", 220, "PreProcess");
        AddTextColumn(dataGrid, "后置处理", 240, "PostProcess");
        AddTextColumn(dataGrid, "说明", 260, "Description");
    }

    public static void ApplyDefaultBehavior(DataGrid dataGrid)
    {
        dataGrid.ApplyPerformancePreset();
        dataGrid.AddSorting();
        dataGrid.EnableSmartTooltips(1, 4, 5, 6, 7, 9, 10, 11);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The performance demo intentionally uses reflection binding to build columns dynamically.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The performance demo intentionally uses reflection binding to build columns dynamically.")]
    private static void AddTextColumn(DataGrid dataGrid, string header, double width, string path)
    {
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = header,
            Width = new DataGridLength(width),
            SortMemberPath = path,
            Binding = new Binding(path)
        });
    }
}

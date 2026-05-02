using Avalonia.Controls;
using Avalonia.Data;
using CodeWF.AvaloniaControls;
using CodeWF.AvaloniaControls.DataGridLegacyDemo.Models;
using System.Collections.Generic;

namespace CodeWF.AvaloniaControls.DataGridLegacyDemo.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeScenario(ScenarioOneDataGrid, ScenarioOneSummary, 60000, 17);
        InitializeScenario(ScenarioTwoDataGrid, ScenarioTwoSummary, 120000, 29);
        InitializeScenario(ScenarioThreeDataGrid, ScenarioThreeSummary, 180000, 41);
    }

    private static void InitializeScenario(DataGrid dataGrid, TextBlock summaryBlock, int count, int seed)
    {
        var items = CreateRecords(count, seed);
        EnsureColumns(dataGrid);

        dataGrid.ItemsSource = items;
        dataGrid.RowHeight = 36;
        dataGrid.CanUserReorderColumns = false;
        dataGrid.CanUserResizeColumns = true;
        dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
        dataGrid.AddSorting();
        dataGrid.EnableSmartTooltips(1, 3, 4, 5, 6, 7);

        summaryBlock.Text = $"共 {count:N0} 行，建议快速来回切换页签，观察旧版 DataGrid 的重绘与切换停顿。";
    }

    private static void EnsureColumns(DataGrid dataGrid)
    {
        if (dataGrid.Columns.Count > 0)
        {
            return;
        }

        AddColumn(dataGrid, "编号", nameof(LegacyProcessRecord.Id), 90);
        AddColumn(dataGrid, "任务名称", nameof(LegacyProcessRecord.Name), 180);
        AddColumn(dataGrid, "产线", nameof(LegacyProcessRecord.Line), 120);
        AddColumn(dataGrid, "主机", nameof(LegacyProcessRecord.Host), 180);
        AddColumn(dataGrid, "程序路径", nameof(LegacyProcessRecord.ProgramPath), 260);
        AddColumn(dataGrid, "工作路径", nameof(LegacyProcessRecord.WorkPath), 240);
        AddColumn(dataGrid, "启动参数", nameof(LegacyProcessRecord.Parameters), 220);
        AddColumn(dataGrid, "说明", nameof(LegacyProcessRecord.Description), 260);
        AddColumn(dataGrid, "状态", nameof(LegacyProcessRecord.Status), 120);
    }

    private static void AddColumn(DataGrid dataGrid, string header, string path, double width)
    {
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = header,
            Width = new DataGridLength(width),
            SortMemberPath = path,
            Binding = new Binding(path)
        });
    }

    private static List<LegacyProcessRecord> CreateRecords(int count, int seed)
    {
        var items = new List<LegacyProcessRecord>(count);

        // 这里故意一次性准备大数据量，用来稳定复现页签切换时的 UI 压力。
        for (var i = 1; i <= count; i++)
        {
            var lineNo = (i + seed) % 6 + 1;
            items.Add(new LegacyProcessRecord
            {
                Id = i,
                Name = $"配方同步任务 {i}",
                Line = $"东区 {lineNo} 号线",
                Host = $"10.24.{seed % 20}.{i % 220 + 10}",
                ProgramPath = $@"D:\runtime\line-{lineNo}\worker-{i % 12}.exe",
                WorkPath = $@"D:\runtime\line-{lineNo}\workspace-{i % 18}",
                Parameters = i % 4 == 0 ? "--mode verify --delay 3" : "--mode sync --retry 2",
                Description = i % 5 == 0
                    ? "用于模拟工艺配方下发与回滚校验的大表格切换场景。"
                    : "用于观察旧版 DataGrid 在多页签之间切换时的滚动与重绘负载。",
                Status = i % 7 == 0 ? "待复核" : "已就绪"
            });
        }

        return items;
    }
}

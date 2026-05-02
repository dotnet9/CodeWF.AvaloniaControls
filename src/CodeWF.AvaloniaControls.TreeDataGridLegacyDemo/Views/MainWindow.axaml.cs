using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CodeWF.AvaloniaControls;
using CodeWF.AvaloniaControls.TreeDataGridLegacyDemo.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeWF.AvaloniaControls.TreeDataGridLegacyDemo.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeScenario(ScenarioOneTreeDataGrid, ScenarioOneSummary, 60000, 19);
        InitializeScenario(ScenarioTwoTreeDataGrid, ScenarioTwoSummary, 120000, 31);
        InitializeScenario(ScenarioThreeTreeDataGrid, ScenarioThreeSummary, 180000, 43);
    }

    private static void InitializeScenario(TreeDataGrid treeDataGrid, TextBlock summaryBlock, int count, int seed)
    {
        var source = CreateSource(count, seed);
        treeDataGrid.Source = source;
        treeDataGrid.AddSorting(source);
        treeDataGrid.AddSelectAll(source);

        summaryBlock.Text = $"共 {count:N0} 行，建议与 DataGrid 示例并排对照，快速切换页签观察 TreeDataGrid 的稳定性。";
    }

    private static FlatTreeDataGridSource<LegacyTreeRecord> CreateSource(int count, int seed)
    {
        var items = new ObservableCollection<LegacyTreeRecord>(CreateRecords(count, seed));

        return new FlatTreeDataGridSource<LegacyTreeRecord>(items)
        {
            Columns =
            {
                new TextColumn<LegacyTreeRecord, int>("编号", x => x.Id),
                new TextColumn<LegacyTreeRecord, string>("任务名称", x => x.Name),
                new TextColumn<LegacyTreeRecord, string>("产线", x => x.Line),
                new TextColumn<LegacyTreeRecord, string>("主机", x => x.Host),
                new TextColumn<LegacyTreeRecord, string>("模式", x => x.Mode),
                new TextColumn<LegacyTreeRecord, string>("责任人", x => x.Owner),
                new TextColumn<LegacyTreeRecord, string>("状态", x => x.Status),
            }
        };
    }

    private static List<LegacyTreeRecord> CreateRecords(int count, int seed)
    {
        var items = new List<LegacyTreeRecord>(count);

        // 这里与 DataGrid 对照示例保持相近规模，用来观察 TreeDataGrid 的切换差异。
        for (var i = 1; i <= count; i++)
        {
            var lineNo = (i + seed) % 6 + 1;
            items.Add(new LegacyTreeRecord
            {
                Id = i,
                Name = $"工艺节点 {i}",
                Line = $"东区 {lineNo} 号线",
                Host = $"10.30.{seed % 18}.{i % 220 + 10}",
                Mode = i % 4 == 0 ? "巡检" : "同步",
                Owner = i % 5 == 0 ? "夜班值守" : "调度中心",
                Status = i % 7 == 0 ? "待处理" : "运行中"
            });
        }

        return items;
    }
}

using System.Collections.Generic;
using CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.Models;

namespace CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.ViewModels;

public class LargeDataGridViewModel : ViewModelBase
{
    public LargeDataGridViewModel(int rowCount, int seed, string scenarioName)
    {
        Items = PerformanceDataFactory.CreateRows(rowCount, seed, scenarioName);
        Summary = $"共 {Items.Count:N0} 行，直接保活在页签中，用于观察切换、滚动和排序时是否出现明显卡顿。";
    }

    public IReadOnlyList<ProcessItem> Items { get; }

    public string Summary { get; }
}

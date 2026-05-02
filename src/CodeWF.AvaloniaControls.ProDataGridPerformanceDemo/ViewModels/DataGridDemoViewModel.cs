using System.Collections.Generic;
using CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.Models;

namespace CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.ViewModels;

public class DataGridDemoViewModel : ViewModelBase
{
    public DataGridDemoViewModel()
    {
        Items = PerformanceDataFactory.CreateRows(12000, 11, "基础交互");
        Count = Items.Count;
    }

    public IReadOnlyList<ProcessItem> Items { get; }

    public int Count { get; }
}

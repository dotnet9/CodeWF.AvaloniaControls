using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace TestDataGridDemo.Models;

public class DynamicItem : ReactiveObject
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ObservableCollection<DynamicColumnInfo> DynamicColumns
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public DateTime CreatedAt { get; set; }
}

public class DynamicColumnInfo : ReactiveObject
{
    public string Name { get; set; }
    public string DisplayName { get; set; }

    public double Value
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
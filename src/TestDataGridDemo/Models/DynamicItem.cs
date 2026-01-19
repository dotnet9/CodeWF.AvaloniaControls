using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tmds.DBus.Protocol;

namespace TestDataGridDemo.Models;

public class DynamicItem : ReactiveObject
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Dictionary<string, DynamicColumnInfo> DynamicColumns { get; } = new ();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
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
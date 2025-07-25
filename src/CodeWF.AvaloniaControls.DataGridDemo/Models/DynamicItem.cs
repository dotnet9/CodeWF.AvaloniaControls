using ReactiveUI;
using System.Collections.Generic;

namespace CodeWF.AvaloniaControls.DataGridDemo.Models;

public class DynamicItem : ReactiveObject
{
    public string? Name
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public Dictionary<string, string>? Values
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
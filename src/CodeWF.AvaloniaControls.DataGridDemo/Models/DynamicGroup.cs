using ReactiveUI;
using System.Collections.Generic;

namespace CodeWF.AvaloniaControls.DataGridDemo.Models;

public class DynamicGroup : ReactiveObject
{
    public string? Name
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public List<DynamicItem>? Items
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
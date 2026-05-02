using ReactiveUI;

namespace CodeWF.AvaloniaControls.ProDataGridShowcase.Models;

public class DynamicItem : ReactiveObject
{
    public string? Key
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string? Name
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string? Value
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
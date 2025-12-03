using ReactiveUI;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Models.Documents.Homes.Tools;

public class ItemBase:ReactiveObject
{
    public int Id { get; set; }
    public string? Name
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
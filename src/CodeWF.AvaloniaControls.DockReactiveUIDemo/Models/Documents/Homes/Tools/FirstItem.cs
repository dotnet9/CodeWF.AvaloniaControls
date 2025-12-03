using CodeWF.AvaloniaControls.Extensions;
using ReactiveUI;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Models.Documents.Homes.Tools;

public class FirstItem : ItemBase
{
    public RangeObservableCollection<SecondItem>? SecondItems
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}
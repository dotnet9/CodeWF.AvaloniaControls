using Dock.Model.Controls;
using Dock.Model.Core;
using ReactiveUI;

namespace CodeWF.AvaloniaControls.DockDemo.ViewModels;

public class DockDemoViewModel : ReactiveObject
{
    private readonly IFactory? _factory;
    private IRootDock? _layout;

    public IRootDock? Layout
    {
        get => _layout;
        set => this.RaiseAndSetIfChanged(ref _layout, value);
    }

    public DockDemoViewModel()
    {
    }
}

using ReactiveUI;

namespace CodeWF.AvaloniaControls.DockDemo.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    public string? Title { get; set; } = "Avalonia Dock Demo";

    public MainWindowViewModel()
    {
    }
}

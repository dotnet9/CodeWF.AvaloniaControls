using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Views.Documents;

public partial class HelpDocumentationView : UserControl
{
    private ContentControl? _hostProcessWin;

    public HelpDocumentationView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _hostProcessWin = this.FindControl<ContentControl>("HostProcessWin");
    }

    private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // 通过代码触发一次宿主加载，避免行为库命名空间差异导致的 XAML 兼容问题。
        if (DataContext is HelpDocumentationViewModel viewModel && _hostProcessWin is not null)
        {
            viewModel.RaiseLoadHostHandler(_hostProcessWin);
        }
    }
}

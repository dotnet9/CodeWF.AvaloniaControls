using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CodeWF.AvaloniaControls.DockDemo;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
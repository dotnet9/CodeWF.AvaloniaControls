using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Views.Pages;

public partial class DataGridDemoView : UserControl
{
    public DataGridDemoView()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Views.Documents;

public partial class LogRecordsView : UserControl
{
    public LogRecordsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
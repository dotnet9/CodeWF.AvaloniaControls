using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Views.Documents;

public partial class UserCenterView : UserControl
{
    public UserCenterView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
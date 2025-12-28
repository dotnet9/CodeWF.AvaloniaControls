using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CodeWF.AvaloniaControls.Demo.Pages;

public partial class TestNoneWindowDemo : UserControl
{
    public TestNoneWindowDemo()
    {
        InitializeComponent();
    }

    private void ShowNoneWindow_OnClick(object? sender, RoutedEventArgs e)
    {
        new TestWindow().Show();
    }
}
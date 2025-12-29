using Avalonia.Controls;
using Avalonia.Interactivity;
using CodeWF.AvaloniaControls.Demo.Views.TestNoneWindowDemos;

namespace CodeWF.AvaloniaControls.Demo.Pages;

public partial class TestNoneWindowDemo : UserControl
{
    public TestNoneWindowDemo()
    {
        InitializeComponent();
    }

    private void ShowNativeWindow_OnClick(object? sender, RoutedEventArgs e)
    {
        new NativeWindowDemo().Show();
    }
    private void ShowUrsaWindow_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowDefault().Show();
    }
    private void ShowUrsaWindowWithNone_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowWithNone().Show();
    }
    private void ShowUrsaWindowWithNoneAndMove_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowWithNoneAndMove().Show();
    }
}
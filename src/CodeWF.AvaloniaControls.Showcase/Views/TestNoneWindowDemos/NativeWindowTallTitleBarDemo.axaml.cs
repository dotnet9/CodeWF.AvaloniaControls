using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

public partial class NativeWindowTallTitleBarDemo : Window
{
    public NativeWindowTallTitleBarDemo()
    {
        InitializeComponent();
    }

    private void TitleBar_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Button)
        {
            return;
        }

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    private void Minimize_OnClick(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeRestore_OnClick(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}

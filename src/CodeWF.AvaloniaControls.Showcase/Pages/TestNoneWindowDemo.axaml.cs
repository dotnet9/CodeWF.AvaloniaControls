using Avalonia.Controls;
using Avalonia.Interactivity;
using CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

namespace CodeWF.AvaloniaControls.Showcase.Pages;

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

    private void ShowNativeWindowDraggable_OnClick(object? sender, RoutedEventArgs e)
    {
        new NativeWindowDraggableDemo().Show();
    }

    private void ShowNativeWindowDarkTitleBar_OnClick(object? sender, RoutedEventArgs e)
    {
        new NativeWindowDarkTitleBarDemo().Show();
    }

    private void ShowNativeWindowTallTitleBar_OnClick(object? sender, RoutedEventArgs e)
    {
        new NativeWindowTallTitleBarDemo().Show();
    }

    private void ShowNativeWindowShaped_OnClick(object? sender, RoutedEventArgs e)
    {
        new NativeWindowShapedDemo().Show();
    }

    private void ShowNativeWindowStar_OnClick(object? sender, RoutedEventArgs e)
    {
        new NativeWindowStarDemo().Show();
    }

    private void ShowNativeWindowBlob_OnClick(object? sender, RoutedEventArgs e)
    {
        new NativeWindowBlobDemo().Show();
    }

    private void ShowUrsaWindow_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowDefault().Show();
    }

    private void ShowUrsaWindowDarkTitleBar_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowDarkTitleBarDemo().Show();
    }

    private void ShowUrsaWindowAccent_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowAccentDemo().Show();
    }

    private void ShowUrsaWindowTallTitleBar_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowTallTitleBarDemo().Show();
    }

    private void ShowCodeWFWindowStandard_OnClick(object? sender, RoutedEventArgs e)
    {
        new CodeWFWindowStandardDemo().Show();
    }

    private void ShowCodeWFWindowDark_OnClick(object? sender, RoutedEventArgs e)
    {
        new CodeWFWindowDarkDemo().Show();
    }

    private void ShowCodeWFWindowAccent_OnClick(object? sender, RoutedEventArgs e)
    {
        new CodeWFWindowAccentDemo().Show();
    }

    private void ShowCodeWFWindowTall_OnClick(object? sender, RoutedEventArgs e)
    {
        new CodeWFWindowTallDemo().Show();
    }
}

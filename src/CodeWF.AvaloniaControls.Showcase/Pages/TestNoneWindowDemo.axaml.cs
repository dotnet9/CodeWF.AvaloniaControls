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

    private void ShowUrsaWindowDarkTitleBar_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowDarkTitleBarDemo().Show();
    }

    private void ShowUrsaWindowCompactTool_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowCompactToolDemo().Show();
    }

    private void ShowUrsaWindowTallTitleBar_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowTallTitleBarDemo().Show();
    }

    private void ShowUrsaWindowShaped_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowShapedDemo().Show();
    }

    private void ShowUrsaWindowStar_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowStarDemo().Show();
    }

    private void ShowUrsaWindowBlob_OnClick(object? sender, RoutedEventArgs e)
    {
        new UrsaWindowBlobDemo().Show();
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

    private void ShowCodeWFWindowCompactTool_OnClick(object? sender, RoutedEventArgs e)
    {
        new CodeWFWindowCompactToolDemo().Show();
    }

    private void ShowCodeWFWindowTall_OnClick(object? sender, RoutedEventArgs e)
    {
        new CodeWFWindowTallDemo().Show();
    }

    private void ShowCodeWFWindowShaped_OnClick(object? sender, RoutedEventArgs e)
    {
        new CodeWFWindowShapedDemo().Show();
    }

    private void ShowCodeWFWindowStar_OnClick(object? sender, RoutedEventArgs e)
    {
        new CodeWFWindowStarDemo().Show();
    }

    private void ShowCodeWFWindowBlob_OnClick(object? sender, RoutedEventArgs e)
    {
        new CodeWFWindowBlobDemo().Show();
    }
}

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CodeWF.AvaloniaControls.Controls;

namespace CodeWF.AvaloniaControlsDemo.Pages;

public partial class GuideDemo : UserControl
{
    public GuideDemo()
    {
        InitializeComponent();
    }

    private void BeginBasicGuide_OnClick(object? sender, RoutedEventArgs e)
    {
        BasicGuide.Show();
    }

    private void BeginNonMaskGuide_OnClick(object? sender, RoutedEventArgs e)
    {
        NonMaskGuide.Show();
    }

    private void BeginDynamicGuide_OnClick(object? sender, RoutedEventArgs e)
    {
        DynamicGuide.GoTo(0);
        GuideThemeMenu.IsSubMenuOpen = false;
        Dispatcher.UIThread.Post(() => DynamicGuide.Show(), DispatcherPriority.Loaded);
    }

    private void SkipBasicGuide_OnClick(object? sender, RoutedEventArgs e)
    {
        BasicGuide.Close();
    }

    private void DynamicGuide_OnStepOpening(object? sender, GuideStepEventArgs e)
    {
        SetDynamicGuideMenuOpen(e.Index is >= 1 and <= 3);
    }

    private void DynamicGuide_OnClosed(object? sender, EventArgs e)
    {
        SetDynamicGuideMenuOpen(false);
    }

    private void SetDynamicGuideMenuOpen(bool isOpen)
    {
        GuideThemeMenu.IsSubMenuOpen = isOpen;
        if (!isOpen) return;

        Dispatcher.UIThread.Post(() => GuideThemeMenu.IsSubMenuOpen = true, DispatcherPriority.Background);
    }
}
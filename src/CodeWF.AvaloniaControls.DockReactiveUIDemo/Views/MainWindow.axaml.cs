using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Core;
using CodeWF.AvaloniaControls.Extensions;
using System;
using Ursa.Controls;
using CodeWF.AvaloniaControls.Helpers;
using Avalonia.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Views;

public partial class MainWindow : UrsaWindow
{
    public MainWindow()
    {
        InitializeComponent();
        this.RegisterGlobalKeyDownHandler();
        this.EnableOSVersionAwareDecorations();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        e.Cancel = true;
        await MessageBox.ShowOverlayAsync("Are you sure you want to exit?", "Confirm Exit");
#if DEBUG
        Environment.Exit(0);
#endif
    }
    protected override void OnClosed(EventArgs e)
    {
        ProcessEmbedHost.CloseAll();
        base.OnClosed(e);
    }
}
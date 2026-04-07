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
        // 如果打开了第三方进程的窗口，使用遮罩提示框会被第三方窗口覆盖，导致用户无法看到提示框，因此这里直接使用普通的消息框。
        //await MessageBox.ShowOverlayAsync("Are you sure you want to exit?", "Confirm Exit");
        await MessageBox.ShowAsync("Are you sure you want to exit?", "Confirm Exit");
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
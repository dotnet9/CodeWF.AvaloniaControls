using System;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows;
using Ursa.Controls;
using CodeWF.AvaloniaControls.Extensions;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Views;

public partial class MainWindow : UrsaWindow
{
    public MainWindow()
    {
        InitializeComponent();
        this.RegisterGlobalKeyDownHandler();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnClosed(EventArgs e)
    {
        EmbedWindow.CloseAll();
        base.OnClosed(e);
    }
}
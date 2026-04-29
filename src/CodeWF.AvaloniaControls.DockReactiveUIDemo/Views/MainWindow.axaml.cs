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
    private bool _isCloseConfirmed;

    public MainWindow()
    {
        InitializeComponent();
        this.RegisterGlobalKeyDownHandler();
        this.EnableOSVersionAwareDecorations();

        PropertyChanged += async (s, e) => 
        {
            if(e.Property == WindowStateProperty && OperatingSystem.IsWindows())
            {
                if(WindowState == WindowState.Minimized)
                {
                    Hide();
                    ShowInTaskbar = false;
                }
                else
                {
                    Show();
                    Activate();
                    ShowInTaskbar = true;
                }
            }
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        if (_isCloseConfirmed)
        {
            base.OnClosing(e);
            return;
        }

        e.Cancel = true;
        if (!IsVisible)
        {
            Show();
            ShowInTaskbar = true;
            Activate();
        }

        var result = await MessageBox.ShowAsync(
            "Are you sure you want to exit?",
            "Confirm Exit",
            MessageBoxIcon.Question,
            MessageBoxButton.YesNo);
        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        _isCloseConfirmed = true;
        Close();
    }

    protected override void OnClosed(EventArgs e)
    {
        this.RemoveGlobalKeyDownHandler();
        ProcessEmbedHost.CloseAll();
        base.OnClosed(e);
    }
}

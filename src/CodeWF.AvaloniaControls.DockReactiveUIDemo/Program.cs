using Avalonia;
using Avalonia.ReactiveUI;
using System;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows.Windows;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo;

internal sealed class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        // DockSettings.UseFloatingDockAdorner = true;
        // DockSettings.EnableGlobalDocking = true;

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI()
            .LogToTrace();
}

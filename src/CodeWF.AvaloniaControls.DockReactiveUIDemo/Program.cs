using Avalonia;
using ReactiveUI.Avalonia;
using System;

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
using System;
using System.IO;
using Avalonia;
using CodeWF.Log.Core;
using ReactiveUI.Avalonia;

namespace CodeWF.AvaloniaControlsDemo;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Logger.Initialize(new LoggerOptions
        {
            EnableConsole = false,
            File = new FileLogOptions
            {
                DirectoryPath = Path.Combine(Environment.CurrentDirectory, "Log")
            }
        });

        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        finally
        {
            Logger.ShutdownAsync().GetAwaiter().GetResult();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI(_ => { });
    }
}

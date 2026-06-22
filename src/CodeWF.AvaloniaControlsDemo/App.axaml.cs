using System.Globalization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControlsDemo.Views;
using Lang.Avalonia;
using Lang.Avalonia.Json;

namespace CodeWF.AvaloniaControlsDemo;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        I18nManager.Instance.Register(new JsonLangPlugin(), new CultureInfo("zh-CN"), out _);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();

        base.OnFrameworkInitializationCompleted();
    }
}
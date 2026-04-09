using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo;

public partial class App : PrismApplication
{
    public static App? Instance { get; private set; }
    public static MainWindow? MainWin { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize(); // <-- Required
    }

    protected override AvaloniaObject CreateShell()
    {
        Instance = this;
#if DEBUG
        this.AttachDevTools();
#endif
        MainWin = Container.Resolve<MainWindow>();
        return MainWin;
    }

    private void OpenMainWindow_Clicked(object? sender, EventArgs e)
    {
        MainWin?.Show();
        MainWin?.Activate();
        MainWin?.WindowState = WindowState.Normal;
    }

    private void Exit_Clicked(object? sender, EventArgs e)
    {
        MainWin?.Close();
    }
}
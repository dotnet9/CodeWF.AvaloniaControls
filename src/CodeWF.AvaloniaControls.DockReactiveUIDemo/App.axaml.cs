using Avalonia;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Views;
using Prism.DryIoc;
using Prism.Ioc;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo;

public partial class App : PrismApplication
{
    public static MainWindow? MainWin { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize(); // <-- Required
    }

    protected override AvaloniaObject CreateShell()
    {
#if DEBUG
        this.AttachDevTools();
#endif
        MainWin = Container.Resolve<MainWindow>();
        return MainWin;
    }
}
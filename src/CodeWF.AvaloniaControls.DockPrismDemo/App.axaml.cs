using Avalonia;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.DockPrismDemo.Views;
using Prism.DryIoc;
using Prism.Ioc;

namespace CodeWF.AvaloniaControls.DockPrismDemo;

public partial class App : PrismApplication
{
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
        return Container.Resolve<MainWindow>();
    }
}
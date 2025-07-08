using Avalonia;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.DockDemo.Views;
using Prism.DryIoc;
using Prism.Ioc;

namespace CodeWF.AvaloniaControls.DockDemo;

public partial class App : PrismApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize(); // <-- Required
    }


    protected override AvaloniaObject CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }


    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }
}
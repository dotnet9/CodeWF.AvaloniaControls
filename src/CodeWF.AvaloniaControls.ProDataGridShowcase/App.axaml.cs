using Avalonia;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.ProDataGridShowcase.Views;
using Prism.DryIoc;
using Prism.Ioc;

namespace CodeWF.AvaloniaControls.ProDataGridShowcase;

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

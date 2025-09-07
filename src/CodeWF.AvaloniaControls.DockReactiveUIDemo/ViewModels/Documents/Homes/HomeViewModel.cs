using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;
using System.Windows.Input;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents.Homes;

public class HomeViewModel : Document
{
    public HomeViewModel()
    {
        Id = nameof(HomeViewModel);
        Title = "首页";
        CanClose = false;


        Instance = this;
        Factory = new HomeDockFactory();

        Layout = Factory?.CreateLayout();

        InitLayout();

        if (Layout is { } root)
        {
            root.Navigate.Execute("Home");
        }

        NewLayout = ReactiveCommand.Create(ResetLayout);

        DockFactory.Documents.Add(this);
    }
    public HomeDockFactory? Factory { get; }
    private IRootDock? _layout;

    public IRootDock? Layout
    {
        get => _layout;
        set => this.RaiseAndSetIfChanged(ref _layout, value);
    }

    public ICommand NewLayout { get; }

    public static HomeViewModel? Instance { get; private set; }


    public void InitLayout()
    {
        if (Layout is null)
        {
            return;
        }

        Factory?.InitLayout(Layout);
    }

    public void CloseLayout()
    {
        if (Layout is IDock dock)
        {
            if (dock.Close.CanExecute(null))
            {
                dock.Close.Execute(null);
            }
        }
    }

    public void ResetLayout()
    {
        if (Layout is not null)
        {
            if (Layout.Close.CanExecute(null))
            {
                Layout.Close.Execute(null);
            }
        }

        var layout = Factory?.CreateLayout();
        if (layout is not null)
        {
            Factory?.InitLayout(layout);
            Layout = layout;
        }
    }
    public override bool OnClose()
    {
        EventBus.EventBus.Default.Publish(new CloseDocumentCommand(Id));
        return base.OnClose();
    }
}
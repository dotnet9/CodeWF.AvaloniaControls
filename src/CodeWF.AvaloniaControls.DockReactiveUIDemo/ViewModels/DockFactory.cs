using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Models.Documents;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents.Homes;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels;

public class DockFactory : Factory
{
    private IRootDock? _rootDock;
    private IDocumentDock? _documentDock;
    public const string DocumentsKey = "Documents";


    public static List<Document> Documents { get; } = new();
    public static List<IDockWindow> DockWindows { get; }= new();

    public override IRootDock CreateLayout()
    {
        var document0 = new HomeViewModel();
        var document1 = new DataManagementViewModel();
        var document2 = new SystemSettingsViewModel();
        var document3 = new UserCenterViewModel();
        var document4 = new LogRecordsViewModel();
        var document5 = new HelpDocumentationViewModel();


        var documentDock = new DocumentDock
        {
            IsCollapsable = false,
            ActiveDockable = document0,
            VisibleDockables = CreateList<IDockable>(document0, document1, document2, document3, document4, document5),
            CanCreateDocument = false,
            // CanDrop = false,
            EnableWindowDrag = true,
        };

        var mainLayout = new ProportionalDock
        {
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>
            (
                documentDock
            )
        };

        var rootDock = CreateRootDock();

        rootDock.IsCollapsable = false;
        rootDock.ActiveDockable = mainLayout;
        rootDock.DefaultDockable = mainLayout;
        rootDock.VisibleDockables = CreateList<IDockable>(mainLayout);

        _documentDock = documentDock;
        _rootDock = rootDock;

        return rootDock;
    }

    public override IDockWindow? CreateWindowFrom(IDockable dockable)
    {
        var window = base.CreateWindowFrom(dockable);

        if (window != null)
        {
            window.Title = "Dock Avalonia ReactiveUI Demo";
        }
        DockWindows.Add(window);

        return window;
    }

    public override void OnWindowClosed(IDockWindow? window)
    {
        if (window?.Layout is IDock layout)
        {
            void CloseDock(IDock dock)
            {
                if (dock.VisibleDockables is null)
                {
                    return;
                }

                foreach (var item in dock.VisibleDockables.ToList())
                {
                    if (item is IDock subDock)
                    {
                        CloseDock(subDock);
                    }

                    CloseDockable(item);
                }
            }

            CloseDock(layout);
        }

        DockWindows.Remove(window);
        base.OnWindowClosed(window);
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object?>>
        {
            [nameof(HomeViewModel)] = () => new DemoDocument(),
            [nameof(DataManagementViewModel)] = () => new DemoDocument(),
            [nameof(HelpDocumentationViewModel)] = () => new DemoDocument(),
            [nameof(LogRecordsViewModel)] = () => new DemoDocument(),
            [nameof(SystemSettingsViewModel)] = () => new DemoDocument(),
            [nameof(UserCenterViewModel)] = () => new DemoDocument(),
        };

        DockableLocator = new Dictionary<string, Func<IDockable?>>()
        {
            [DocumentsKey] = () => _documentDock
        };

        HostWindowLocator = new Dictionary<string, Func<IHostWindow?>>
        {
            [nameof(IDockWindow)] = () => new HostWindow()
        };

        base.InitLayout(layout);
    }
}
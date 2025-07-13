using System;
using System.Collections.Generic;
using CodeWF.AvaloniaControls.DockPrismDemo.Models.Documents;
using CodeWF.AvaloniaControls.DockPrismDemo.ViewModels.Docks;
using CodeWF.AvaloniaControls.DockPrismDemo.ViewModels.Documents;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Prism;
using Dock.Model.Prism.Controls;

namespace CodeWF.AvaloniaControls.DockPrismDemo.ViewModels;

public class DockFactory : Factory
{
    private readonly object _context;
    private IRootDock? _rootDock;
    private IDocumentDock? _documentDock;
    public const string DocumentsKey = "Documents";

    public DockFactory(object context)
    {
        _context = context;
    }

    public override IDocumentDock CreateDocumentDock() => new CustomDocumentDock();

    public override IRootDock CreateLayout()
    {
        var document1 = new DataManagementViewModel()
            { Id = nameof(DataManagementViewModel), Title = "数据管理", CanClose = false };
        var document2 = new SystemSettingsViewModel()
            { Id = nameof(SystemSettingsViewModel), Title = "系统设置", CanClose = false };
        var document3 = new UserCenterViewModel()
            { Id = nameof(UserCenterViewModel), Title = "用户中心", CanClose = false };
        var document4 = new LogRecordsViewModel()
            { Id = nameof(LogRecordsViewModel), Title = "日志记录", CanClose = false };
        var document5 = new HelpDocumentationViewModel()
            { Id = nameof(HelpDocumentationViewModel), Title = "帮助文档", CanClose = false };


        var documentDock = new CustomDocumentDock
        {
            IsCollapsable = false,
            ActiveDockable = document1,
            VisibleDockables = CreateList<IDockable>(document1, document2, document3, document4, document5),
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
            window.Title = "Dock Avalonia Prism Demo";
        }

        return window;
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object?>>
        {
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
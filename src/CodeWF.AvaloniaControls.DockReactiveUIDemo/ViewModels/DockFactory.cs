﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Models.Documents;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Docks;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using DryIoc.ImTools;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels;

public class DockFactory : Factory
{
    private readonly object _context;
    private IRootDock? _rootDock;
    private IDocumentDock? _documentDock;
    public const string DocumentsKey = "Documents";
    public Dictionary<IDockable, IDockWindow> DocumentWindows { get; private set; } = new();

    public DockFactory(object context)
    {
        _context = context;
    }

    public override IDocumentDock CreateDocumentDock() => new CustomDocumentDock();

    public override IRootDock CreateLayout()
    {
        var document1 = new DataManagementViewModel();
        var document2 = new SystemSettingsViewModel();
        var document3 = new UserCenterViewModel();
        var document4 = new LogRecordsViewModel();
        var document5 = new HelpDocumentationViewModel();


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
            window.Title = "Dock Avalonia ReactiveUI Demo";
        }

        DocumentWindows[dockable] = window;
        return window;
    }

    public override void OnWindowClosed(IDockWindow? window)
    {
        IDockable? document = null;
        foreach (var item in DocumentWindows)
        {
            if (item.Value == window)
            {
                document = item.Key;
            }
        }

        if (document != null)
        {
            EventBus.EventBus.Default.Publish(new CloseDocumentCommand(document.Id));
            DocumentWindows.Remove(document);
        }

        base.OnWindowClosed(window);
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
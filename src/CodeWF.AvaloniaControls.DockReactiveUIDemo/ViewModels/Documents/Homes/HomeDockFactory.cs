using CodeWF.AvaloniaControls.DockReactiveUIDemo.Models.Tools;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents.Homes.Tools;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using System;
using System.Collections.Generic;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents.Homes;

public class HomeDockFactory : Factory
{
    private readonly object _context;
    private IRootDock? _rootDock;

    public HomeDockFactory(object context)
    {
        _context = context;
    }

    public override IRootDock CreateLayout()
    {
        var tool1 = new ParamRealTimeViewModel();
        var tool2 = new ProgInfoViewModel();
        var tool3 = new ManageStatusViewModel();
        var tool4 = new AlertLatestViewModel();
        var tool5 = new SysInfoViewModel();
        var tool6 = new LogViewModel();

        var leftDock = new ProportionalDock
        {
            Proportion = 0.75,
            Orientation = Orientation.Vertical,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ProportionalDock
                {
                    Proportion = 0.2,
                    ActiveDockable = null,
                    VisibleDockables = CreateList<IDockable>
                        (
                            new ToolDock
                            {

                                ActiveDockable = tool1,
                                VisibleDockables = CreateList<IDockable>(tool1),
                            }
                        )
                },
                new ProportionalDockSplitter(),
                new ProportionalDock
                {
                    Proportion = 0.8,
                    ActiveDockable = null,
                    VisibleDockables = CreateList<IDockable>
                    (
                        new ToolDock
                        {

                            ActiveDockable = tool2,
                            VisibleDockables = CreateList<IDockable>(tool2),
                        }
                    )
                }
            )
        };
        var rightDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Vertical,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ProportionalDock
                {
                    Proportion = 0.13,
                    ActiveDockable = null,
                    VisibleDockables = CreateList<IDockable>
                    (
                        new ToolDock
                        {

                            ActiveDockable = tool3,
                            VisibleDockables = CreateList<IDockable>(tool3),
                        }
                    )
                },
                new ProportionalDockSplitter(),
                new ProportionalDock
                {
                    Proportion = 0.27,
                    ActiveDockable = null,
                    VisibleDockables = CreateList<IDockable>
                    (
                        new ToolDock
                        {

                            ActiveDockable = tool4,
                            VisibleDockables = CreateList<IDockable>(tool4),
                        }
                    )
                },
                new ProportionalDockSplitter(),
                new ProportionalDock
                {
                    Proportion = 0.33,
                    ActiveDockable = null,
                    VisibleDockables = CreateList<IDockable>
                    (
                        new ToolDock
                        {

                            ActiveDockable = tool5,
                            VisibleDockables = CreateList<IDockable>(tool5),
                        }
                    )
                },
                new ProportionalDockSplitter(),
                new ProportionalDock
                {
                    Proportion = 0.27,
                    ActiveDockable = null,
                    VisibleDockables = CreateList<IDockable>
                    (
                        new ToolDock
                        {

                            ActiveDockable = tool6,
                            VisibleDockables = CreateList<IDockable>(tool6),
                        }
                    )
                }
            ),
        };

        var mainLayout = new ProportionalDock
        {
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>
            (
                leftDock,
                new ProportionalDockSplitter(),
                rightDock
            )
        };

        var rootDock = CreateRootDock();

        rootDock.IsCollapsable = false;
        rootDock.ActiveDockable = mainLayout;
        rootDock.DefaultDockable = mainLayout;
        rootDock.VisibleDockables = CreateList<IDockable>(mainLayout);

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

        return window;
    }


    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object?>>
        {
            [nameof(ParamRealTimeViewModel)] = () => new ParamRealTime(),
            [nameof(ProgInfoViewModel)] = () => new ProgInfo(),
            [nameof(ManageStatusViewModel)] = () => new ManageStatus(),
            [nameof(AlertLatestViewModel)] = () => new AlertLatest(),
            [nameof(SysInfoViewModel)] = () => new SysInfo(),
            [nameof(LogViewModel)] = () => new Log(),
        };


        HostWindowLocator = new Dictionary<string, Func<IHostWindow?>>
        {
            [nameof(IDockWindow)] = () => new HostWindow()
        };

        base.InitLayout(layout);
    }
}
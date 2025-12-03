using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents.Homes.Tools;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents.Homes;

public class HomeDockFactory : Factory
{
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
            Proportion = 0.7,
            Orientation = Orientation.Vertical,
            VisibleDockables = CreateList<IDockable>
            (
                new ProportionalDock
                {
                    Proportion = 0.2,
                    VisibleDockables = CreateList<IDockable>(new ToolDock
                        { VisibleDockables = CreateList<IDockable>(tool1), })
                },
                new ProportionalDockSplitter(),
                new ProportionalDock
                {
                    Proportion = 0.8,
                    VisibleDockables = CreateList<IDockable>(new ToolDock
                        { VisibleDockables = CreateList<IDockable>(tool2) })
                }
            )
        };
        var rightDock = new ProportionalDock
        {
            MinWidth = 200,
            // 移除MaxWidth限制，允许拖动调整大小
            Proportion = 0.3, // 设置合适的比例值
            Orientation = Orientation.Vertical,
            VisibleDockables = CreateList<IDockable>
            (
                new ProportionalDock
                {
                    Proportion = 0.13,
                    VisibleDockables = CreateList<IDockable>(new ToolDock
                        { VisibleDockables = CreateList<IDockable>(tool3) })
                },
                new ProportionalDockSplitter(),
                new ProportionalDock
                {
                    Proportion = 0.27,
                    VisibleDockables = CreateList<IDockable>(new ToolDock
                        { VisibleDockables = CreateList<IDockable>(tool4), })
                },
                new ProportionalDockSplitter(),
                new ProportionalDock
                {
                    Proportion = 0.33,
                    VisibleDockables = CreateList<IDockable>(
                        new ToolDock { VisibleDockables = CreateList<IDockable>(tool5), }
                    )
                },
                new ProportionalDockSplitter(),
                new ProportionalDock
                {
                    Proportion = 0.27,
                    VisibleDockables = CreateList<IDockable>(
                        new ToolDock { VisibleDockables = CreateList<IDockable>(tool6), }
                    )
                }
            ),
        };

        var mainLayout = new ProportionalDock
        {
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>(leftDock, new ProportionalDockSplitter(), rightDock)
        };

        var rootDock = CreateRootDock();

        rootDock.ActiveDockable = mainLayout;
        rootDock.VisibleDockables = CreateList<IDockable>(mainLayout);

        return rootDock;
    }
}
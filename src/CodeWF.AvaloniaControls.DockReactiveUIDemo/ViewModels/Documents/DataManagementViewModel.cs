using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class DataManagementViewModel : Document
{
    public DataManagementViewModel()
    {
        Id = nameof(DataManagementViewModel);
        Title = "数据管理";
        CanClose = false;
        CanFloat = false;
        CanDrag = false;
    }

    public override bool OnClose()
    {
        return base.OnClose();
    }
}
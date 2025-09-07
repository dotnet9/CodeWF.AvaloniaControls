using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class DataManagementViewModel : Document
{
    public DataManagementViewModel()
    {
        Id = nameof(DataManagementViewModel);
        Title = "数据管理";

        DockFactory.Documents.Add(this);
    }

    public override bool OnClose()
    {
        EventBus.EventBus.Default.Publish(new CloseDocumentCommand(Id));
        return base.OnClose();
    }
}
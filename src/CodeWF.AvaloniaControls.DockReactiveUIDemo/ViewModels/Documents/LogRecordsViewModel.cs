using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class LogRecordsViewModel : Document
{
    public LogRecordsViewModel()
    {
        Id = nameof(LogRecordsViewModel);
        Title = "日志记录";

        DockFactory.Documents.Add(this);
    }

    public override bool OnClose()
    {
        EventBus.EventBus.Default.Publish(new CloseDocumentCommand(Id));
        return base.OnClose();
    }
}
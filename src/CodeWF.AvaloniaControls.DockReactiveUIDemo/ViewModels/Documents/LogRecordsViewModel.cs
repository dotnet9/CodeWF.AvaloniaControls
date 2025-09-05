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
        return base.OnClose();
    }
}
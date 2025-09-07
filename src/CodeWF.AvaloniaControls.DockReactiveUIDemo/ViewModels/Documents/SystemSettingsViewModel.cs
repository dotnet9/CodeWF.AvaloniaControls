using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class SystemSettingsViewModel : Document
{
    public SystemSettingsViewModel()
    {
        Id = nameof(SystemSettingsViewModel);
        Title = "系统设置";

        DockFactory.Documents.Add(this);
    }
    public override bool OnClose()
    {
        EventBus.EventBus.Default.Publish(new CloseDocumentCommand(Id));
        return base.OnClose();
    }
}
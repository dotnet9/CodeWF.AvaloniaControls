using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class SystemSettingsViewModel : Document
{
    public SystemSettingsViewModel()
    {
        Id = nameof(SystemSettingsViewModel);
        Title = "系统设置";
        CanClose = false;
    }
}
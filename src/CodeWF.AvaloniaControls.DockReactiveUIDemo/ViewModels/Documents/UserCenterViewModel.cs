using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class UserCenterViewModel : Document
{
    public UserCenterViewModel()
    {
        Id = nameof(UserCenterViewModel);
        Title = "用户中心";
        CanClose = false;

        DockFactory.Documents.Add(this);
    }

    public override bool OnClose()
    {
        EventBus.EventBus.Default.Publish(new CloseDocumentCommand(Id));
        return base.OnClose();
    }
}
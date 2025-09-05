using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class UserCenterViewModel : Document
{
    public UserCenterViewModel()
    {
        Id = nameof(UserCenterViewModel);
        Title = "用户中心";

        DockFactory.Documents.Add(this);
    }

    public override bool OnClose()
    {
        return base.OnClose();
    }
}
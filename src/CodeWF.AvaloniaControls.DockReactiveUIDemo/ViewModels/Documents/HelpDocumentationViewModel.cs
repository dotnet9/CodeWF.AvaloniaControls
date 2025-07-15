using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class HelpDocumentationViewModel : Document
{
    public HelpDocumentationViewModel()
    {
        Id = nameof(HelpDocumentationViewModel);
        Title = "帮助文档";
        CanClose = true;
    }

    public override bool OnClose()
    {
        return base.OnClose();
    }
}
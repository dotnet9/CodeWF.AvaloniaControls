using CodeWF.AvaloniaControls.DockDemo.Commands;
using ReactiveUI;

namespace CodeWF.AvaloniaControls.DockDemo.ViewModels.Documents;

public class DataManagementViewModel : ReactiveObject
{
    public string Title { get; set; } = "Document Operations";

    public void CreateDocumentHandler()
    {
        EventBus.EventBus.Default.Publish(new CreateDocumentCommand());
    }
}

using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using Dock.Model.Avalonia.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class DataManagementViewModel : Document
{
    public void CreateDocumentHandler()
    {
        EventBus.EventBus.Default.Publish(new CreateDocumentCommand());
    }
}
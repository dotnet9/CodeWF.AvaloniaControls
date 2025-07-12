using CodeWF.AvaloniaControls.DockPrismDemo.Commands;
using Dock.Model.Avalonia.Controls;

namespace CodeWF.AvaloniaControls.DockPrismDemo.ViewModels.Documents;

public class DataManagementViewModel : Document
{
    public void CreateDocumentHandler()
    {
        EventBus.EventBus.Default.Publish(new CreateDocumentCommand());
    }
}
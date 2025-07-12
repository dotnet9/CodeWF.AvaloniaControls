using CodeWF.AvaloniaControls.DockMVVMDemo.Commands;
using Dock.Model.Avalonia.Controls;

namespace CodeWF.AvaloniaControls.DockMVVMDemo.ViewModels.Documents;

public class DataManagementViewModel : Document
{
    public void CreateDocumentHandler()
    {
        EventBus.EventBus.Default.Publish(new CreateDocumentCommand());
    }
}
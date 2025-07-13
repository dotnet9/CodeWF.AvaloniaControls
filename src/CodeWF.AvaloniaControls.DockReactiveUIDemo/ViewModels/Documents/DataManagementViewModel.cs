using Dock.Model.ReactiveUI.Controls;
using Dock.Model.Controls;
using System.Linq;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class DataManagementViewModel : Document
{
    public void CreateDocumentHandler()
    {
        var documents = MainWindowViewModel.Instance?.Factory?.GetDockable<IDocumentDock>(DockFactory.DocumentsKey);
        var existHelpDocument = documents?.VisibleDockables?.OfType<IDocument>()
            .FirstOrDefault(d => d.Id == nameof(HelpDocumentationViewModel));
        if (existHelpDocument != null)
        {
            documents.ActiveDockable = existHelpDocument;
            return;
        }

        var newHelpDocument = new HelpDocumentationViewModel() { Title = nameof(HelpDocumentationViewModel), Id = nameof(HelpDocumentationViewModel) };
        MainWindowViewModel.Instance?.Factory?.AddDockable(documents, newHelpDocument);
        MainWindowViewModel.Instance?.Factory?.SetActiveDockable(newHelpDocument);
        MainWindowViewModel.Instance?.Factory?.SetFocusedDockable(MainWindowViewModel.Instance.Layout!, newHelpDocument);
    }
}
using Avalonia.Controls;
using CodeWF.AvaloniaControls.DockDemo.Commands;
using CodeWF.AvaloniaControls.DockDemo.Views.Documents;
using CodeWF.EventBus;
using Dock.Model.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using DryIoc;
using Prism.Ioc;
using System.Linq;

namespace CodeWF.AvaloniaControls.DockDemo.Views;

public partial class DockMainView : UserControl
{
    private IFactory? _factory;
    private const string _testDocId = "SystemSettingsDocument";
    public DockMainView()
    {
        InitializeComponent();
        InitDock();
        EventBus.EventBus.Default.Subscribe(this);
    }

    private void InitDock()
    {
        _factory = DocumentsPane.Factory;
    }
    

    [EventHandler]
    private void ReceiveCreateDocumentCommand(CreateDocumentCommand command)
    {
        var existing = DocumentsPane.VisibleDockables
            ?.OfType<IDocument>()
            ?.FirstOrDefault(d => d.Id == _testDocId);
        if (existing != null)
        {
            DocumentsPane.ActiveDockable = existing;
            return;
        }
        var view = ContainerLocator.Container.Resolve<SystemSettingsDocumentView>();
        var document = new Document
        {
            Title = "œµÕ≥…Ë÷√",
            Name = _testDocId,
            Id = _testDocId,
            CanClose = false,
            Content = view
        };
        _factory?.AddDockable(DocumentsPane, document);
        _factory?.SetActiveDockable(document);
        _factory?.SetFocusedDockable(DocumentsPane, document);
    }

}


﻿using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Docks;

public class CustomDocumentDock : DocumentDock
{
    public CustomDocumentDock()
    {
        CreateDocument = ReactiveCommand.Create(CreateNewDocument);
    }

    private void CreateNewDocument()
    {
        if (!CanCreateDocument)
        {
            return;
        }

        var index = VisibleDockables?.Count + 1;
        var document = new SystemSettingsViewModel() { Id = $"Document{index}", Title = $"Document{index}" };

        Factory?.AddDockable(this, document);
        Factory?.SetActiveDockable(document);
        Factory?.SetFocusedDockable(this, document);
    }
}

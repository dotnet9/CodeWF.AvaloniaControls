using Avalonia.Controls;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows;
using Dock.Model.ReactiveUI.Controls;
using System;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class HelpDocumentationViewModel : Document
{
    private bool _isFirstLoad = false;
    private EmbedWindow? _embedWindow;
    public const string BeHostProcessPath = @"AppsToHost\CodeWF.AvaloniaControls.Demo\CodeWF.AvaloniaControls.Demo.exe";

    public HelpDocumentationViewModel()
    {
        Id = nameof(HelpDocumentationViewModel);
        Title = "帮助文档";

        DockFactory.Documents.Add(this);
    }

    public void RaiseLoadHostHandler(ContentControl control)
    {
        if (!_isFirstLoad)
        {
            return;
        }

        _isFirstLoad = false;
        _embedWindow =
            new EmbedWindow(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BeHostProcessPath));
        control.Content = _embedWindow;
    }

    public override bool OnClose()
    {
        _embedWindow?.Implementation?.CloseWindow();
        EventBus.EventBus.Default.Publish(new CloseDocumentCommand(Id));
        return base.OnClose();
    }
}
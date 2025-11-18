using Avalonia.Controls;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows;
using Dock.Model.ReactiveUI.Controls;
using System;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class HelpDocumentationViewModel : Document
{
    private bool _isFirstLoad = true;
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
        //var exe = "E:\\github\\avalonia\\CodeWF.AvaloniaControls\\src\\CodeWF.AvaloniaControls.Demo\\bin\\Debug\\net10.0-windows\\CodeWF.AvaloniaControls.Demo.exe";
        var exe = @"E:\github\avalonia\CodeWF.AvaloniaControls\src\FluentDemo\bin\Debug\net10.0\FluentDemo.exe";
        _embedWindow =
            new EmbedWindow(exe);
        control.Content = _embedWindow;
    }

    public override bool OnClose()
    {
        _embedWindow?.Implementation?.CloseWindow();
        EventBus.EventBus.Default.Publish(new CloseDocumentCommand(Id));
        return base.OnClose();
    }
}
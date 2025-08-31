using System;
using Avalonia.Controls;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows;
using Dock.Model.ReactiveUI.Controls;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class HelpDocumentationViewModel : Document
{
    private bool _isFirstLoad = true;
    public const string BeHostProcessPath = @"AppsToHost\CodeWF.AvaloniaControls.Demo\CodeWF.AvaloniaControls.Demo.exe";

    public HelpDocumentationViewModel()
    {
        Id = nameof(HelpDocumentationViewModel);
        Title = "帮助文档";
        CanClose = true;
    }

    public void RaiseLoadHostHandler(ContentControl control)
    {
        if (!_isFirstLoad)
        {
            return;
        }

        _isFirstLoad = false;
        var embeddedProcessWindow =
            new EmbedWindow(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BeHostProcessPath));
        control.Content = embeddedProcessWindow;
    }

    public override bool OnClose()
    {
        return base.OnClose();
    }
}
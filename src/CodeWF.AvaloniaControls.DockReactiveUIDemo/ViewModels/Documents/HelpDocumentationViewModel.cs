using System.IO;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Core;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;

public class HelpDocumentationViewModel : Document
{
    private bool _isFirstLoad = true;
    private ProcessEmbedHost? _embedWindow;

    public HelpDocumentationViewModel()
    {
        Id = nameof(HelpDocumentationViewModel);
        Title = "帮助文档";

        DockFactory.Documents.Add(this);
    }

    public string Tip
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public void RaiseLoadHostHandler(ContentControl control)
    {
        if (!_isFirstLoad)
        {
            return;
        }

        _isFirstLoad = false;
        var exe = string.Empty;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            exe = Path.Combine(Directory.GetCurrentDirectory(), "CodeWF.AvaloniaControls.DockReactiveUIDemo.exe");
        }
        else
        {
            exe = Path.Combine(Directory.GetCurrentDirectory(), "CodeWF.AvaloniaControls.DockReactiveUIDemo");
        }
        Tip = exe;
        if (!File.Exists(exe))
        {
            Tip += $"：文件不存在";
            return;
        }
        _embedWindow = new ProcessEmbedHost(exe, Path.GetDirectoryName(exe), string.Empty);
        control.Content = _embedWindow;
    }

    public override bool OnClose()
    {
        _embedWindow?.Embedder?.Close();
        EventBus.EventBus.Default.Publish(new CloseDocumentCommand(Id));
        return base.OnClose();
    }
}
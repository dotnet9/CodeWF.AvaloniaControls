using CodeWF.EventBus;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;

public class CloseDocumentCommand(string key) : Command
{
    public string? DocumentKey { get; set; } = key;
}
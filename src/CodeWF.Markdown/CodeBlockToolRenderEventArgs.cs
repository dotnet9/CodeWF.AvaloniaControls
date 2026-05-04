using Avalonia.Controls;

using Markdig.Syntax;

namespace CodeWF.Markdown;

public sealed class CodeBlockToolRenderEventArgs : EventArgs
{
    public CodeBlockToolRenderEventArgs(StackPanel headerPanel, StackPanel contentPanel, CodeBlock codeBlock)
    {
        HeaderPanel = headerPanel;
        ContentPanel = contentPanel;
        CodeBlock = codeBlock;
    }

    public StackPanel HeaderPanel { get; }

    public StackPanel ContentPanel { get; }

    public CodeBlock CodeBlock { get; }
}

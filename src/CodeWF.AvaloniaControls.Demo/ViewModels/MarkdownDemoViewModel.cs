using System.IO;

namespace CodeWF.AvaloniaControls.Demo.ViewModels;

public class MarkdownDemoViewModel
{
    public MarkdownDemoViewModel()
    {
        var file = "UpdateLog.MD";
        if (File.Exists(file))
        {
            Markdown = File.ReadAllText(Markdown);
        }
        else
        {
            Markdown = """
                       # Update Log
                       ## V0.0.7.0(2024-12-27)
                       Add markdown control
                       """;
        }
    }
    public string Markdown { get; set; }
}
namespace CodeWF.AvaloniaControlsDemo.Models;

public sealed record AutoCompleteBoxDemoSuggestion(
    string DisplayName,
    string Category,
    string Detail,
    int SourceIndex,
    bool IsPinned)
{
    public int DisplayIndex => SourceIndex + 1;
}

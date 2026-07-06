using System;

namespace CodeWF.AvaloniaControlsDemo.Models;

public sealed record AutoCompleteBoxDemoSuggestion(
    string DisplayName,
    string Category,
    string Detail,
    string Keywords,
    int SourceIndex,
    bool IsPinned)
{
    public int DisplayIndex => SourceIndex + 1;

    public bool Matches(string query)
    {
        return DisplayName.Contains(query, StringComparison.OrdinalIgnoreCase)
               || Category.Contains(query, StringComparison.OrdinalIgnoreCase)
               || Detail.Contains(query, StringComparison.OrdinalIgnoreCase)
               || Keywords.Contains(query, StringComparison.OrdinalIgnoreCase);
    }
}

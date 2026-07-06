using System;

namespace CodeWF.AvaloniaControlsDemo.Models;

public sealed record AutoCompleteBoxDemoOption(
    string DisplayName,
    string Category,
    string Detail,
    string Keywords,
    int SourceIndex)
{
    public bool Matches(string query)
    {
        return DisplayName.Contains(query, StringComparison.OrdinalIgnoreCase)
               || Category.Contains(query, StringComparison.OrdinalIgnoreCase)
               || Detail.Contains(query, StringComparison.OrdinalIgnoreCase)
               || Keywords.Contains(query, StringComparison.OrdinalIgnoreCase);
    }
}

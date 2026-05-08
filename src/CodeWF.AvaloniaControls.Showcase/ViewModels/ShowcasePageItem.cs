using System;
using System.ComponentModel;
using Avalonia.Controls;
using CodeWF.AvaloniaControls.Showcase.Services;

namespace CodeWF.AvaloniaControls.Showcase.ViewModels;

public sealed class ShowcasePageItem(string headerKey, string searchToken, Control content) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string HeaderKey { get; } = headerKey;

    public string SearchToken { get; } = searchToken;

    public Control Content { get; } = content;

    public string DisplayName => LocalizationService.Get(HeaderKey);

    public bool Matches(string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return true;
        }

        var text = searchText.Trim();
        return DisplayName.Contains(text, StringComparison.OrdinalIgnoreCase)
            || SearchToken.Contains(text, StringComparison.OrdinalIgnoreCase);
    }

    public void Refresh()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
    }
}

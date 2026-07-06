using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CodeWF.AvaloniaControlsDemo.Models;
using ReactiveUI;

namespace CodeWF.AvaloniaControlsDemo.ViewModels;

internal sealed class AutoCompleteBoxDemoViewModel : ReactiveObject
{
    private const decimal DefaultPinnedCount = 2;

    private readonly IReadOnlyList<AutoCompleteBoxDemoOption> _allOptions =
    [
        new("Dashboard", "Pinned", "Default entry that always stays at the top.", "dashboard overview home", 0),
        new("Command Palette", "Pinned", "Default entry that always stays at the top.", "command palette action", 1),
        new("DataGrid", "Data", "Dense data table with sorting and filtering.", "table grid data row", 2),
        new("Transfer", "Selection", "Move items between source and target lists.", "transfer selection list", 3),
        new("VComboBox", "Input", "Compact combo box for command surfaces.", "combo input selector warning", 4),
        new("TabControl", "Navigation", "Switch between related work areas.", "tabs navigation page", 5),
        new("Guide Tour", "Overlay", "Step-by-step guidance for complex screens.", "guide tour overlay help", 6),
        new("StatusBadge", "Status", "Inline severity and state indicator.", "badge status warning", 7),
        new("StatusCard", "Status", "Card-style service health indicator.", "card health service", 8),
        new("AnimatedImage", "Media", "Load local or network animated GIF resources.", "gif image animation", 9),
        new("CodeWFWindow", "Window", "Managed title bar and shell options.", "window titlebar shell", 10),
        new("MarkupExtensions", "XAML", "If and Switch markup extension examples.", "xaml markup converter", 11)
    ];

    private decimal _pinnedCount = DefaultPinnedCount;
    private AutoCompleteBoxDemoSuggestion? _selectedSuggestion;
    private string? _searchText;

    public AutoCompleteBoxDemoViewModel()
    {
        RebuildSuggestions();
    }

    public ObservableCollection<AutoCompleteBoxDemoSuggestion> Suggestions { get; } = [];

    public decimal MaxPinnedCount => _allOptions.Count;

    public decimal PinnedCount
    {
        get => _pinnedCount;
        set
        {
            var wholeValue = decimal.Round(value, 0, MidpointRounding.AwayFromZero);
            var clippedValue = Math.Clamp(wholeValue, 0, MaxPinnedCount);
            if (_pinnedCount == clippedValue) return;

            this.RaiseAndSetIfChanged(ref _pinnedCount, clippedValue);
            RebuildSuggestions();
            this.RaisePropertyChanged(nameof(FilterSummary));
        }
    }

    public string? SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;

            this.RaiseAndSetIfChanged(ref _searchText, value);
            RebuildSuggestions();
            this.RaisePropertyChanged(nameof(FilterSummary));
        }
    }

    public AutoCompleteBoxDemoSuggestion? SelectedSuggestion
    {
        get => _selectedSuggestion;
        set => this.RaiseAndSetIfChanged(ref _selectedSuggestion, value);
    }

    public string FilterSummary
    {
        get
        {
            var queryText = string.IsNullOrWhiteSpace(SearchText) ? "empty" : $"\"{SearchText.Trim()}\"";
            return $"{PinnedCount:0} pinned, {Suggestions.Count} shown, query: {queryText}";
        }
    }

    private void RebuildSuggestions()
    {
        var pinnedCount = (int)PinnedCount;
        var pinnedOptions = _allOptions.Take(pinnedCount);
        var filteredOptions = FilterNormalOptions(_allOptions.Skip(pinnedCount), SearchText);

        Suggestions.Clear();
        foreach (var option in pinnedOptions.Concat(filteredOptions))
        {
            Suggestions.Add(new AutoCompleteBoxDemoSuggestion(
                option.DisplayName,
                option.Category,
                option.Detail,
                option.SourceIndex,
                option.SourceIndex < pinnedCount));
        }

        EnsureSelectedSuggestionIsVisible();
    }

    private void EnsureSelectedSuggestionIsVisible()
    {
        if (SelectedSuggestion is null) return;
        if (Suggestions.Any(item => item.SourceIndex == SelectedSuggestion.SourceIndex)) return;

        SelectedSuggestion = null;
    }

    private static IEnumerable<AutoCompleteBoxDemoOption> FilterNormalOptions(
        IEnumerable<AutoCompleteBoxDemoOption> options,
        string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText)) return options;

        var query = searchText.Trim();
        return options.Where(option => option.Matches(query));
    }
}

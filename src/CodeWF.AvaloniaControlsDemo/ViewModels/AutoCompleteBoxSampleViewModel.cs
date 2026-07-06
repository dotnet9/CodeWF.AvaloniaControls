using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using CodeWF.AvaloniaControlsDemo.Models;
using ReactiveUI;

namespace CodeWF.AvaloniaControlsDemo.ViewModels;

internal sealed class AutoCompleteBoxSampleViewModel : ReactiveObject
{
    private const decimal DefaultPinnedCount = 2;

    private readonly IReadOnlyList<AutoCompleteBoxDemoOption> _options;

    public AutoCompleteBoxSampleViewModel(IReadOnlyList<AutoCompleteBoxDemoOption> options)
    {
        _options = options;
        ItemFilter = FilterSuggestion;

        RebuildSuggestions();
    }

    public IReadOnlyList<AutoCompleteBoxDemoSuggestion> Suggestions
    {
        get;
        private set => this.RaiseAndSetIfChanged(ref field, value);
    } = [];

    public decimal MaxPinnedCount => _options.Count;

    public decimal PinnedCount
    {
        get;
        set
        {
            var wholeValue = decimal.Round(value, 0, MidpointRounding.AwayFromZero);
            var clippedValue = Math.Clamp(wholeValue, 0, MaxPinnedCount);
            if (field == clippedValue) return;

            this.RaiseAndSetIfChanged(ref field, clippedValue);
            RebuildSuggestions();
        }
    } = DefaultPinnedCount;

    public AutoCompleteBoxDemoSuggestion? SelectedSuggestion
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public AutoCompleteFilterPredicate<object?> ItemFilter { get; }

    private bool FilterSuggestion(string? searchText, object? item)
    {
        if (item is not AutoCompleteBoxDemoSuggestion suggestion) return false;
        if (suggestion.IsPinned) return true;
        if (string.IsNullOrWhiteSpace(searchText)) return true;

        return suggestion.Matches(searchText.Trim());
    }

    private void RebuildSuggestions()
    {
        var pinnedCount = (int)PinnedCount;
        var selectedSourceIndex = SelectedSuggestion?.SourceIndex;
        var suggestions = _options
            .Select((option, index) => CreateSuggestion(option, index < pinnedCount))
            .ToList();

        Suggestions = suggestions;
        SyncSelectedSuggestion(suggestions, selectedSourceIndex);
    }

    private static AutoCompleteBoxDemoSuggestion CreateSuggestion(
        AutoCompleteBoxDemoOption option,
        bool isPinned)
    {
        return new AutoCompleteBoxDemoSuggestion(
            option.DisplayName,
            option.Category,
            option.Detail,
            option.Keywords,
            option.SourceIndex,
            isPinned);
    }

    private void SyncSelectedSuggestion(
        IReadOnlyList<AutoCompleteBoxDemoSuggestion> suggestions,
        int? selectedSourceIndex)
    {
        if (selectedSourceIndex is null) return;

        SelectedSuggestion = suggestions.FirstOrDefault(item => item.SourceIndex == selectedSourceIndex);
    }
}

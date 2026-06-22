using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CodeWF.AvaloniaControls.Extensions;
using CodeWF.AvaloniaControls.Models;
using CodeWF.AvaloniaControlsDemo.Services;
using Lang.Avalonia;
using ReactiveUI;
using PageLangs = Showcase.Pages.ColumnDisplayEditorDemo;

namespace CodeWF.AvaloniaControlsDemo.ViewModels;

public sealed class ColumnDisplayEditorDemoViewModel : ReactiveObject
{
    private string _previewText = string.Empty;
    private bool _reloadingText;

    public ColumnDisplayEditorDemoViewModel()
    {
        Items.CollectionChanged += Items_OnCollectionChanged;
        ReloadText();
        I18nManager.Instance.CultureChanged += (_, _) => ReloadText();
    }

    public RangeObservableCollection<ColumnDisplayItem> Items { get; } = new();

    public string PreviewText
    {
        get => _previewText;
        private set => this.RaiseAndSetIfChanged(ref _previewText, value);
    }

    private void Items_OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_reloadingText) return;

        if (e.OldItems is not null)
            foreach (ColumnDisplayItem item in e.OldItems)
                item.PropertyChanged -= Item_OnPropertyChanged;

        if (e.NewItems is not null)
            foreach (ColumnDisplayItem item in e.NewItems)
                item.PropertyChanged += Item_OnPropertyChanged;

        UpdatePreviewText();
    }

    private void Item_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        UpdatePreviewText();
    }

    private void ReloadText()
    {
        _reloadingText = true;

        try
        {
            var currentState = Items.ToDictionary(item => item.Key, item => item.Visible);
            var orderedKeys = Items.Select(item => item.Key).ToList();
            var definitions = GetColumnDefinitions().ToDictionary(item => item.Key);
            var sortedDefinitions = orderedKeys
                .Where(definitions.ContainsKey)
                .Select(key => definitions[key])
                .Concat(GetColumnDefinitions().Where(item => !orderedKeys.Contains(item.Key)))
                .ToList();

            foreach (var item in Items) item.PropertyChanged -= Item_OnPropertyChanged;

            Items.Clear();
            Items.AddRange(sortedDefinitions.Select(item =>
                new ColumnDisplayItem(
                    item.Key,
                    item.DisplayText,
                    currentState.TryGetValue(item.Key, out var visible) ? visible : item.Visible)));

            foreach (var item in Items) item.PropertyChanged += Item_OnPropertyChanged;
        }
        finally
        {
            _reloadingText = false;
        }

        UpdatePreviewText();
    }

    private void UpdatePreviewText()
    {
        var builder = new StringBuilder();
        builder.AppendLine("[");

        foreach (var (item, index) in Items.Select((item, index) => (item, index)))
        {
            builder.AppendLine("  {");
            builder.AppendLine($"""    "Key": "{item.Key}",""");
            builder.AppendLine($"""    "DisplayText": "{item.DisplayText}",""");
            builder.AppendLine($"""    "Visible": {item.Visible.ToString().ToLowerInvariant()}""");
            builder.Append(index == Items.Count - 1 ? "  }" : "  },");
            builder.AppendLine();
        }

        builder.Append("]");
        PreviewText = builder.ToString();
    }

    private static ColumnDisplayItem[] GetColumnDefinitions()
    {
        return
        [
            new ColumnDisplayItem("RecordId", LocalizationService.Get(PageLangs.ColumnRecordId)),
            new ColumnDisplayItem("Title", LocalizationService.Get(PageLangs.ColumnTitle)),
            new ColumnDisplayItem("Category", LocalizationService.Get(PageLangs.ColumnCategory)),
            new ColumnDisplayItem("Owner", LocalizationService.Get(PageLangs.ColumnOwner)),
            new ColumnDisplayItem("Priority", LocalizationService.Get(PageLangs.ColumnPriority)),
            new ColumnDisplayItem("Status", LocalizationService.Get(PageLangs.ColumnStatus)),
            new ColumnDisplayItem("CreatedAt", LocalizationService.Get(PageLangs.ColumnCreatedAt)),
            new ColumnDisplayItem("UpdatedAt", LocalizationService.Get(PageLangs.ColumnUpdatedAt)),
            new ColumnDisplayItem("DueDate", LocalizationService.Get(PageLangs.ColumnDueDate), false),
            new ColumnDisplayItem("Tags", LocalizationService.Get(PageLangs.ColumnTags)),
            new ColumnDisplayItem("Summary", LocalizationService.Get(PageLangs.ColumnSummary)),
            new ColumnDisplayItem("Notes", LocalizationService.Get(PageLangs.ColumnNotes), false)
        ];
    }
}
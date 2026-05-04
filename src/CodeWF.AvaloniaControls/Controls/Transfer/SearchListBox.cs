using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Threading;
using CodeWF.AvaloniaControls.Extensions;

namespace CodeWF.AvaloniaControls.Controls;

public class SearchListBox : TemplatedControl
{
    private readonly DispatcherTimer _searchTimer;
    private ListBox? _listBox;
    private TextBox? _searchBox;
    private RangeObservableCollection<string>? _observedItemsSource;
    private string? _searchKey;
    private int _searchCount;
    private int _totalCount;

    static SearchListBox()
    {
        ItemsSourceProperty.Changed.AddClassHandler<SearchListBox>((box, _) => box.OnItemsSourceChanged());
    }

    public SearchListBox()
    {
        _searchTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(400)
        };
        _searchTimer.Tick += (_, _) =>
        {
            _searchTimer.Stop();
            SearchData();
        };
    }

    public event Action<object?, TappedEventArgs>? ListItemDoubleTapped;

    public static readonly StyledProperty<RangeObservableCollection<string>?> ItemsSourceProperty =
        AvaloniaProperty.Register<SearchListBox, RangeObservableCollection<string>?>(nameof(ItemsSource));

    public RangeObservableCollection<string>? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DirectProperty<SearchListBox, int> TotalCountProperty =
        AvaloniaProperty.RegisterDirect<SearchListBox, int>(nameof(TotalCount), box => box.TotalCount);

    public int TotalCount
    {
        get => _totalCount;
        private set => SetAndRaise(TotalCountProperty, ref _totalCount, value);
    }

    public static readonly DirectProperty<SearchListBox, int> SearchCountProperty =
        AvaloniaProperty.RegisterDirect<SearchListBox, int>(nameof(SearchCount), box => box.SearchCount);

    public int SearchCount
    {
        get => _searchCount;
        private set => SetAndRaise(SearchCountProperty, ref _searchCount, value);
    }

    public RangeObservableCollection<string> BindingItemsSource { get; } = new();

    public List<string>? SelectedItems => _listBox?.SelectedItems?.Cast<string>().ToList();

    public void AddRange(IEnumerable<string>? items)
    {
        if (ItemsSource is null || items is null)
        {
            return;
        }

        var itemList = items.ToList();
        if (itemList.Count == 0)
        {
            return;
        }

        ItemsSource.AddRange(itemList);
        SearchData();
    }

    public void RemoveRange(IEnumerable<string>? items)
    {
        if (ItemsSource is null || items is null)
        {
            return;
        }

        var itemList = items.ToList();
        if (itemList.Count == 0)
        {
            return;
        }

        ItemsSource.RemoveRange(itemList);
        SearchData();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_searchBox is not null)
        {
            _searchBox.TextChanged -= SearchBox_OnTextChanged;
            _searchBox.KeyUp -= SearchBox_OnKeyUp;
        }

        if (_listBox is not null)
        {
            _listBox.DoubleTapped -= ListBox_OnDoubleTapped;
        }

        _searchBox = e.NameScope.Find<TextBox>("PART_SearchBox");
        _listBox = e.NameScope.Find<ListBox>("PART_ListBox");

        if (_searchBox is not null)
        {
            _searchBox.TextChanged += SearchBox_OnTextChanged;
            _searchBox.KeyUp += SearchBox_OnKeyUp;
            _searchKey = _searchBox.Text?.Trim().ToLowerInvariant();
        }

        if (_listBox is not null)
        {
            _listBox.DoubleTapped += ListBox_OnDoubleTapped;
        }

        SearchData();
    }

    private void OnItemsSourceChanged()
    {
        if (_observedItemsSource is not null)
        {
            _observedItemsSource.CollectionChanged -= ItemsSource_OnCollectionChanged;
        }

        _observedItemsSource = ItemsSource;

        if (_observedItemsSource is not null)
        {
            _observedItemsSource.CollectionChanged += ItemsSource_OnCollectionChanged;
        }

        SearchData();
    }

    private void ItemsSource_OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        SearchData();
    }

    private void SearchBox_OnKeyUp(object? sender, KeyEventArgs e)
    {
        UpdateSearchKey(sender);
    }

    private void SearchBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        UpdateSearchKey(sender);
    }

    private void UpdateSearchKey(object? sender)
    {
        if (sender is TextBox textBox)
        {
            _searchKey = textBox.Text?.Trim().ToLowerInvariant();
        }

        _searchTimer.Stop();
        _searchTimer.Start();
    }

    private void ListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        ListItemDoubleTapped?.Invoke(sender, e);
    }

    private void SearchData()
    {
        BindingItemsSource.Clear();

        if (ItemsSource is not null)
        {
            BindingItemsSource.AddRange(string.IsNullOrWhiteSpace(_searchKey)
                ? ItemsSource
                : ItemsSource.Where(item => item.ToLowerInvariant().Contains(_searchKey)));
        }

        TotalCount = ItemsSource?.Count ?? 0;
        SearchCount = BindingItemsSource.Count;
    }
}

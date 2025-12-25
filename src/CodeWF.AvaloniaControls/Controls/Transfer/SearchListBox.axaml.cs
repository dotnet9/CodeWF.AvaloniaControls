using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.Extensions;
using ReactiveUI;

namespace CodeWF.AvaloniaControls.Controls;

public partial class SearchListBox : UserControl, INotifyPropertyChanged
{
    private readonly ListBox? _listBox;
    private string? _searchKey;

    private Subject<string?> _searchSubject;

    static SearchListBox()
    {
        ItemsSourceProperty.Changed.AddClassHandler<SearchListBox, RangeObservableCollection<string>?>((box, args) =>
            box.OnItemSourceChanged(args));
    }

    public SearchListBox()
    {
        InitializeComponent();
        _listBox = this.FindControl<ListBox>("MyListBox");

        _searchSubject = new Subject<string?>();
        _searchSubject.Throttle(TimeSpan.FromMilliseconds(400))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => SearchData());
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnItemSourceChanged(AvaloniaPropertyChangedEventArgs<RangeObservableCollection<string>?> args)
    {
        SearchData();
    }

    private void ChangeSearchKey_OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (sender is TextBox txtBox) _searchKey = txtBox.Text?.Trim().ToLower();

        _searchSubject.OnNext(_searchKey);
        //SearchData();
    }

    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ChangeSearchKey_OnKeyUp(sender, null);
    }

    private void MyListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        ListItemDoubleTapped?.Invoke(sender, e);
    }

    private void SearchData()
    {
        if (_listBox == null || ItemsSource == null) return;

        BindingItemsSource.Clear();
        BindingItemsSource.AddRange(string.IsNullOrWhiteSpace(_searchKey)
            ? ItemsSource
            : ItemsSource.Where(item => item.ToLower().Contains(_searchKey)));
        ChangeCountInfo();
    }

    #region 公开属性

    public event Action<object?, TappedEventArgs>? ListItemDoubleTapped;

    public static readonly StyledProperty<RangeObservableCollection<string>?> ItemsSourceProperty =
        AvaloniaProperty.Register<Transfer, RangeObservableCollection<string>?>(
            nameof(ItemsSource));

    public RangeObservableCollection<string>? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public List<string>? SelectedItems => _listBox?.SelectedItems?.Cast<string>().ToList();

    #endregion

    #region 公开方法

    public void AddRange(List<string> items)
    {
        ItemsSource.AddRange(items);
        BindingItemsSource.AddRange(items);
        ChangeCountInfo();
    }

    public void RemoveRange(List<string> items)
    {
        ItemsSource.RemoveRange(items);
        BindingItemsSource.RemoveRange(items);

        ChangeCountInfo();
    }

    #endregion

    #region 内部使用

    public RangeObservableCollection<string> BindingItemsSource { get; } = new();

    private int _totalCount;

    public int TotalCount
    {
        get => _totalCount;

        set
        {
            if (_totalCount == value) return;
            _totalCount = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCount)));
        }
    }

    private int _searchCount;

    public int SearchCount
    {
        get => _searchCount;

        set
        {
            if (_searchCount == value) return;
            _searchCount = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchCount)));
        }
    }

    private void ChangeCountInfo()
    {
        TotalCount = ItemsSource?.Count ?? default;
        SearchCount = BindingItemsSource.Count;
    }

    #endregion
}
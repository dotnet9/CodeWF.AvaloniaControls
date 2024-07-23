using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.Extensions;
using System.ComponentModel;

namespace CodeWF.AvaloniaControls.Controls;

public partial class Transfer : UserControl, INotifyPropertyChanged
{
    private readonly SearchListBox? _leftSearchListBox;
    private readonly SearchListBox? _rightSearchListBox;

    public Transfer()
    {
        InitializeComponent();
        _leftSearchListBox = this.FindControl<SearchListBox>("LeftSearchListBox");
        _rightSearchListBox = this.FindControl<SearchListBox>("RightSearchListBox");
        _leftSearchListBox.ListItemDoubleTapped += (s, e) => MoveLeftToRight_OnClick(null, null);
        _rightSearchListBox.ListItemDoubleTapped += (s, e) => MoveRightToLeft_OnClick(null, null);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void MoveLeftToRight_OnClick(object? sender, RoutedEventArgs e)
    {
        var leftSelectedItems = _leftSearchListBox!.SelectedItems;
        _leftSearchListBox!.Remove(leftSelectedItems);
        _rightSearchListBox.Add(leftSelectedItems);
    }

    private void MoveRightToLeft_OnClick(object? sender, RoutedEventArgs e)
    {
        var rightSelectedItems = _rightSearchListBox!.SelectedItems;
        _rightSearchListBox.Remove(rightSelectedItems);
        _leftSearchListBox.Add(rightSelectedItems);
    }

    #region 对外提供属性

    public static readonly StyledProperty<string?> LeftHeaderProperty =
        AvaloniaProperty.Register<Transfer, string?>(
            nameof(LeftHeader), "源数据");

    public string? LeftHeader
    {
        get => GetValue(LeftHeaderProperty);
        set => SetValue(LeftHeaderProperty, value);
    }

    public static readonly StyledProperty<string?> RightHeaderProperty =
        AvaloniaProperty.Register<Transfer, string?>(
            nameof(RightHeader), "选择数据");

    public string? RightHeader
    {
        get => GetValue(RightHeaderProperty);
        set => SetValue(RightHeaderProperty, value);
    }

    public static readonly StyledProperty<RangeObservableCollection<string>?> LeftItemsSourceProperty =
        AvaloniaProperty.Register<Transfer, RangeObservableCollection<string>?>(
            nameof(LeftItemsSource));

    public RangeObservableCollection<string>? LeftItemsSource
    {
        get => GetValue(LeftItemsSourceProperty);
        set => SetValue(LeftItemsSourceProperty, value);
    }

    public static readonly StyledProperty<RangeObservableCollection<string>?> RightItemsSourceProperty =
        AvaloniaProperty.Register<Transfer, RangeObservableCollection<string>?>(
            nameof(RightItemsSource));

    public RangeObservableCollection<string>? RightItemsSource
    {
        get => GetValue(RightItemsSourceProperty);
        set => SetValue(RightItemsSourceProperty, value);
    }

    #endregion
}
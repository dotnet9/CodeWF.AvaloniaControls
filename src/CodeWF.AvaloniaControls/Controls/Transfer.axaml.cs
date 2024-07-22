using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.Extensions;

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
    }

    #region �ڲ�ʹ��

    public RangeObservableCollection<string> RightItemsSource { get; } = new();

    #endregion

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

    #region �����ṩ����

    public static readonly StyledProperty<string?> LeftHeaderProperty =
        AvaloniaProperty.Register<Transfer, string?>(
            nameof(LeftHeader), "Դ����");

    public string? LeftHeader
    {
        get => GetValue(LeftHeaderProperty);
        set => SetValue(LeftHeaderProperty, value);
    }

    public static readonly StyledProperty<string?> RightHeaderProperty =
        AvaloniaProperty.Register<Transfer, string?>(
            nameof(RightHeader), "ѡ������");

    public string? RightHeader
    {
        get => GetValue(RightHeaderProperty);
        set => SetValue(RightHeaderProperty, value);
    }

    public static readonly StyledProperty<RangeObservableCollection<string>?> ItemsSourceProperty =
        AvaloniaProperty.Register<Transfer, RangeObservableCollection<string>?>(
            nameof(ItemsSource));

    public RangeObservableCollection<string>? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public List<string> SelectedItems => RightItemsSource.ToList();

    #endregion
}
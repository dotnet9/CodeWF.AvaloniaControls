using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using CodeWF.AvaloniaControls.Extensions;

namespace CodeWF.AvaloniaControls.Controls;

public class Transfer : TemplatedControl
{
    private Button? _moveLeftToRightButton;
    private Button? _moveRightToLeftButton;
    private SearchListBox? _leftSearchListBox;
    private SearchListBox? _rightSearchListBox;

    public static readonly StyledProperty<string?> LeftHeaderProperty =
        AvaloniaProperty.Register<Transfer, string?>(nameof(LeftHeader), "Source");

    public string? LeftHeader
    {
        get => GetValue(LeftHeaderProperty);
        set => SetValue(LeftHeaderProperty, value);
    }

    public static readonly StyledProperty<string?> RightHeaderProperty =
        AvaloniaProperty.Register<Transfer, string?>(nameof(RightHeader), "Selected");

    public string? RightHeader
    {
        get => GetValue(RightHeaderProperty);
        set => SetValue(RightHeaderProperty, value);
    }

    public static readonly StyledProperty<RangeObservableCollection<string>?> LeftItemsSourceProperty =
        AvaloniaProperty.Register<Transfer, RangeObservableCollection<string>?>(nameof(LeftItemsSource));

    public RangeObservableCollection<string>? LeftItemsSource
    {
        get => GetValue(LeftItemsSourceProperty);
        set => SetValue(LeftItemsSourceProperty, value);
    }

    public static readonly StyledProperty<RangeObservableCollection<string>?> RightItemsSourceProperty =
        AvaloniaProperty.Register<Transfer, RangeObservableCollection<string>?>(nameof(RightItemsSource));

    public RangeObservableCollection<string>? RightItemsSource
    {
        get => GetValue(RightItemsSourceProperty);
        set => SetValue(RightItemsSourceProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachTemplateEvents();

        _leftSearchListBox = e.NameScope.Find<SearchListBox>("PART_LeftSearchListBox");
        _rightSearchListBox = e.NameScope.Find<SearchListBox>("PART_RightSearchListBox");
        _moveLeftToRightButton = e.NameScope.Find<Button>("PART_MoveLeftToRightButton");
        _moveRightToLeftButton = e.NameScope.Find<Button>("PART_MoveRightToLeftButton");

        if (_leftSearchListBox is not null)
        {
            _leftSearchListBox.ListItemDoubleTapped += LeftSearchListBox_OnListItemDoubleTapped;
        }

        if (_rightSearchListBox is not null)
        {
            _rightSearchListBox.ListItemDoubleTapped += RightSearchListBox_OnListItemDoubleTapped;
        }

        if (_moveLeftToRightButton is not null)
        {
            _moveLeftToRightButton.Click += MoveLeftToRightButton_OnClick;
        }

        if (_moveRightToLeftButton is not null)
        {
            _moveRightToLeftButton.Click += MoveRightToLeftButton_OnClick;
        }
    }

    public void MoveLeftToRight()
    {
        var leftSelectedItems = _leftSearchListBox?.SelectedItems;
        if (leftSelectedItems?.Count > 0)
        {
            _leftSearchListBox?.RemoveRange(leftSelectedItems);
            _rightSearchListBox?.AddRange(leftSelectedItems);
        }
    }

    public void MoveRightToLeft()
    {
        var rightSelectedItems = _rightSearchListBox?.SelectedItems;
        if (rightSelectedItems?.Count > 0)
        {
            _rightSearchListBox?.RemoveRange(rightSelectedItems);
            _leftSearchListBox?.AddRange(rightSelectedItems);
        }
    }

    private void DetachTemplateEvents()
    {
        if (_leftSearchListBox is not null)
        {
            _leftSearchListBox.ListItemDoubleTapped -= LeftSearchListBox_OnListItemDoubleTapped;
        }

        if (_rightSearchListBox is not null)
        {
            _rightSearchListBox.ListItemDoubleTapped -= RightSearchListBox_OnListItemDoubleTapped;
        }

        if (_moveLeftToRightButton is not null)
        {
            _moveLeftToRightButton.Click -= MoveLeftToRightButton_OnClick;
        }

        if (_moveRightToLeftButton is not null)
        {
            _moveRightToLeftButton.Click -= MoveRightToLeftButton_OnClick;
        }
    }

    private void LeftSearchListBox_OnListItemDoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        MoveLeftToRight();
    }

    private void RightSearchListBox_OnListItemDoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        MoveRightToLeft();
    }

    private void MoveLeftToRightButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MoveLeftToRight();
    }

    private void MoveRightToLeftButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MoveRightToLeft();
    }
}

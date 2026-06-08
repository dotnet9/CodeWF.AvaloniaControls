using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using CodeWF.AvaloniaControls.Extensions;

namespace CodeWF.AvaloniaControls.Controls;

public class Transfer : TemplatedControl
{
    #region 字段

    private Button? _moveLeftToRightButton;
    private Button? _moveRightToLeftButton;
    private SearchListBox? _leftSearchListBox;
    private SearchListBox? _rightSearchListBox;

    #endregion 字段

    #region 标题属性

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

    #endregion 标题属性

    #region 数据源属性

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

    #endregion 数据源属性

    #region 模板生命周期

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachTemplateEvents();
        ResolveTemplateParts(e);
        AttachTemplateEvents();
        UpdateButtonStates();
    }

    private void ResolveTemplateParts(TemplateAppliedEventArgs e)
    {
        _leftSearchListBox = e.NameScope.Find<SearchListBox>("PART_LeftSearchListBox");
        _rightSearchListBox = e.NameScope.Find<SearchListBox>("PART_RightSearchListBox");
        _moveLeftToRightButton = e.NameScope.Find<Button>("PART_MoveLeftToRightButton");
        _moveRightToLeftButton = e.NameScope.Find<Button>("PART_MoveRightToLeftButton");
    }

    private void AttachTemplateEvents()
    {
        if (_leftSearchListBox is not null)
        {
            _leftSearchListBox.ListItemDoubleTapped += LeftSearchListBox_OnListItemDoubleTapped;
            _leftSearchListBox.ListSelectionChanged += SearchListBox_OnListSelectionChanged;
        }

        if (_rightSearchListBox is not null)
        {
            _rightSearchListBox.ListItemDoubleTapped += RightSearchListBox_OnListItemDoubleTapped;
            _rightSearchListBox.ListSelectionChanged += SearchListBox_OnListSelectionChanged;
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

    private void DetachTemplateEvents()
    {
        if (_leftSearchListBox is not null)
        {
            _leftSearchListBox.ListItemDoubleTapped -= LeftSearchListBox_OnListItemDoubleTapped;
            _leftSearchListBox.ListSelectionChanged -= SearchListBox_OnListSelectionChanged;
        }

        if (_rightSearchListBox is not null)
        {
            _rightSearchListBox.ListItemDoubleTapped -= RightSearchListBox_OnListItemDoubleTapped;
            _rightSearchListBox.ListSelectionChanged -= SearchListBox_OnListSelectionChanged;
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

    #endregion 模板生命周期

    #region 移动操作

    public void MoveLeftToRight()
    {
        var leftSelectedItems = _leftSearchListBox?.SelectedItems;
        if (leftSelectedItems?.Count > 0)
        {
            _leftSearchListBox?.RemoveRange(leftSelectedItems);
            _rightSearchListBox?.AddRange(leftSelectedItems);
        }

        UpdateButtonStates();
    }

    public void MoveRightToLeft()
    {
        var rightSelectedItems = _rightSearchListBox?.SelectedItems;
        if (rightSelectedItems?.Count > 0)
        {
            _rightSearchListBox?.RemoveRange(rightSelectedItems);
            _leftSearchListBox?.AddRange(rightSelectedItems);
        }

        UpdateButtonStates();
    }

    #endregion 移动操作

    #region 事件处理

    private void LeftSearchListBox_OnListItemDoubleTapped(object? sender, TappedEventArgs e)
    {
        MoveLeftToRight();
    }

    private void RightSearchListBox_OnListItemDoubleTapped(object? sender, TappedEventArgs e)
    {
        MoveRightToLeft();
    }

    private void SearchListBox_OnListSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        UpdateButtonStates();
    }

    private void MoveLeftToRightButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MoveLeftToRight();
    }

    private void MoveRightToLeftButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MoveRightToLeft();
    }

    #endregion 事件处理

    #region 按钮状态

    /// <summary>
    /// 移动按钮只在对应侧存在选中项时启用。
    /// </summary>
    private void UpdateButtonStates()
    {
        if (_moveLeftToRightButton is not null)
        {
            _moveLeftToRightButton.IsEnabled = _leftSearchListBox?.SelectedItems?.Count > 0;
        }

        if (_moveRightToLeftButton is not null)
        {
            _moveRightToLeftButton.IsEnabled = _rightSearchListBox?.SelectedItems?.Count > 0;
        }
    }

    #endregion 按钮状态
}

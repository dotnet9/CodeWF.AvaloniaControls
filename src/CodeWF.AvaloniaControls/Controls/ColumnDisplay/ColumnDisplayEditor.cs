using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace CodeWF.AvaloniaControls.Controls;

public class ColumnDisplayEditor : TemplatedControl
{
    private ListBox? _listBox;
    private object? _draggingItem;
    private object? _dropTargetItem;
    private ListBoxItem? _dropTargetContainer;
    private bool _dropAfterTarget;
    private bool _isDragging;

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<ColumnDisplayEditor, IEnumerable?>(nameof(ItemsSource));

    public static readonly StyledProperty<string> VisibleHeaderTextProperty =
        AvaloniaProperty.Register<ColumnDisplayEditor, string>(nameof(VisibleHeaderText), "Visible");

    public static readonly StyledProperty<string> DisplayTextHeaderTextProperty =
        AvaloniaProperty.Register<ColumnDisplayEditor, string>(nameof(DisplayTextHeaderText), "Name");

    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public string VisibleHeaderText
    {
        get => GetValue(VisibleHeaderTextProperty);
        set => SetValue(VisibleHeaderTextProperty, value);
    }

    public string DisplayTextHeaderText
    {
        get => GetValue(DisplayTextHeaderTextProperty);
        set => SetValue(DisplayTextHeaderTextProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachTemplateEvents();

        _listBox = e.NameScope.Find<ListBox>("PART_ListBox");

        if (_listBox is not null)
        {
            _listBox.AddHandler(InputElement.PointerPressedEvent, ListBox_OnPointerPressed, RoutingStrategies.Tunnel);
            _listBox.AddHandler(InputElement.PointerMovedEvent, ListBox_OnPointerMoved, RoutingStrategies.Tunnel);
            _listBox.AddHandler(InputElement.PointerReleasedEvent, ListBox_OnPointerReleased, RoutingStrategies.Tunnel);
            _listBox.AddHandler(InputElement.PointerCaptureLostEvent, ListBox_OnPointerCaptureLost, RoutingStrategies.Tunnel);
        }
    }

    private void DetachTemplateEvents()
    {
        if (_listBox is null)
        {
            return;
        }

        _listBox.RemoveHandler(InputElement.PointerPressedEvent, ListBox_OnPointerPressed);
        _listBox.RemoveHandler(InputElement.PointerMovedEvent, ListBox_OnPointerMoved);
        _listBox.RemoveHandler(InputElement.PointerReleasedEvent, ListBox_OnPointerReleased);
        _listBox.RemoveHandler(InputElement.PointerCaptureLostEvent, ListBox_OnPointerCaptureLost);
    }

    private void ListBox_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_listBox is null || e.GetCurrentPoint(_listBox).Properties.IsLeftButtonPressed != true)
        {
            return;
        }

        if (e.Source is not Visual source || !IsDragHandle(source))
        {
            return;
        }

        var item = FindItemFromVisual(source);
        if (item is null)
        {
            return;
        }

        _draggingItem = item;
        _dropTargetItem = item;
        _dropAfterTarget = false;
        _isDragging = true;
        e.Pointer.Capture(_listBox);
        e.Handled = true;
    }

    private void ListBox_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isDragging || _draggingItem is null || _listBox is null)
        {
            return;
        }

        if (!e.GetCurrentPoint(_listBox).Properties.IsLeftButtonPressed)
        {
            EndDrag(e.Pointer);
            return;
        }

        var targetContainer = FindItemContainerFromVisual(_listBox.InputHitTest(e.GetPosition(_listBox)) as Visual);
        var targetItem = targetContainer?.DataContext;
        if (targetContainer is null || targetItem is null || ReferenceEquals(targetItem, _draggingItem))
        {
            return;
        }

        var position = e.GetPosition(targetContainer);
        _dropTargetItem = targetItem;
        _dropAfterTarget = position.Y > targetContainer.Bounds.Height / 2;
        UpdateDropPreview(targetContainer, _dropAfterTarget);
        e.Handled = true;
    }

    private void ListBox_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_isDragging && _draggingItem is not null && _dropTargetItem is not null)
        {
            MoveItem(_draggingItem, _dropTargetItem, _dropAfterTarget);
        }

        EndDrag(e.Pointer);
    }

    private void ListBox_OnPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        EndDrag(e.Pointer);
    }

    private void EndDrag(IPointer pointer)
    {
        _draggingItem = null;
        _dropTargetItem = null;
        ClearDropPreview();
        _dropAfterTarget = false;
        _isDragging = false;
        if (pointer.Captured == _listBox)
        {
            pointer.Capture(null);
        }
    }

    private void MoveItem(object sourceItem, object targetItem, bool insertAfterTarget)
    {
        if (ItemsSource is not IList list)
        {
            return;
        }

        var sourceIndex = list.IndexOf(sourceItem);
        var targetIndex = list.IndexOf(targetItem);
        if (sourceIndex < 0 || targetIndex < 0 || sourceIndex == targetIndex)
        {
            return;
        }

        var insertIndex = insertAfterTarget ? targetIndex + 1 : targetIndex;

        list.RemoveAt(sourceIndex);
        if (sourceIndex < insertIndex)
        {
            insertIndex--;
        }

        list.Insert(insertIndex, sourceItem);
    }

    private void UpdateDropPreview(ListBoxItem targetContainer, bool afterTarget)
    {
        if (!ReferenceEquals(_dropTargetContainer, targetContainer))
        {
            ClearDropPreview();
            _dropTargetContainer = targetContainer;
        }

        targetContainer.Classes.Set("drop-before", !afterTarget);
        targetContainer.Classes.Set("drop-after", afterTarget);
    }

    private void ClearDropPreview()
    {
        if (_dropTargetContainer is null)
        {
            return;
        }

        _dropTargetContainer.Classes.Set("drop-before", false);
        _dropTargetContainer.Classes.Set("drop-after", false);
        _dropTargetContainer = null;
    }

    private static bool IsDragHandle(Visual source)
    {
        for (Visual? visual = source; visual is not null; visual = visual.GetVisualParent())
        {
            if (visual is StyledElement { Name: "PART_DragHandle" })
            {
                return true;
            }
        }

        return false;
    }

    private static object? FindItemFromVisual(Visual? source)
    {
        return FindItemContainerFromVisual(source)?.DataContext;
    }

    private static ListBoxItem? FindItemContainerFromVisual(Visual? source)
    {
        for (Visual? visual = source; visual is not null; visual = visual.GetVisualParent())
        {
            if (visual is ListBoxItem item)
            {
                return item;
            }
        }

        return null;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace CodeWF.AvaloniaControls.Controls;

/// <summary>
/// 窗口化堆叠面板：子项数量超过阈值后，只把「视口 ± overscan」范围内的子项控件挂到可视树，
/// 其余子项保留已测量的尺寸占位，滚动离屏即释放控件，避免长列表一次性挂载全部控件。
/// <para>
/// 与 Avalonia 自带 <c>VirtualizingStackPanel</c> 的差别：本面板的子项索引由调用方维护，
/// 即使某项当前未物化，索引也不会错位（<see cref="InsertItem"/> / <see cref="RemoveAt"/> 按索引操作），
/// 适合「文档块」这类由业务侧按语义增删、又需要按视口裁剪的场景。
/// </para>
/// <para>
/// 关闭 <see cref="EnableVirtualization"/> 或子项数低于 <see cref="VirtualizationThreshold"/> 时，
/// 行为与普通垂直面板一致，可在异常场景一开关退回非虚拟化宿主。
/// </para>
/// </summary>
public class WindowedStackPanel : Panel
{
    private sealed class ItemEntry
    {
        public ItemEntry(Control control, bool pinned, bool pinnedByKind)
        {
            Control = control;
            Pinned = pinned;
            PinnedByKind = pinnedByKind;
        }

        public Control Control { get; }

        /// <summary>调用方指定的固定物化（如当前滚动锚点所在项）。</summary>
        public bool Pinned { get; }

        /// <summary>按子项类型固定物化（如代码块、表格、图片等大块），避免测量与滚动抖动。</summary>
        public bool PinnedByKind { get; }

        public bool ShouldAlwaysRealize => Pinned || PinnedByKind;

        public bool Realized { get; set; } = true;

        public bool Measured { get; set; }

        public double Size { get; set; }
    }

    private readonly List<ItemEntry> _entries = [];
    private ScrollViewer? _subscribedScrollHost;
    private double _lastWidth = double.NaN;
    private double[] _sizeBuffer = [];
    private bool[] _pinnedBuffer = [];

    public static readonly StyledProperty<bool> EnableVirtualizationProperty =
        AvaloniaProperty.Register<WindowedStackPanel, bool>(nameof(EnableVirtualization), true);

    /// <summary>启用虚拟化的最小子项数量；低于该值按普通面板处理，避免小列表付出额外开销。</summary>
    public static readonly StyledProperty<int> VirtualizationThresholdProperty =
        AvaloniaProperty.Register<WindowedStackPanel, int>(nameof(VirtualizationThreshold), 40);

    /// <summary>视口上下额外物化的屏数。</summary>
    public static readonly StyledProperty<double> OverscanViewportsProperty =
        AvaloniaProperty.Register<WindowedStackPanel, double>(nameof(OverscanViewports), 2d);

    /// <summary>滚动宿主；未显式指定时自动向上查找。</summary>
    public static readonly StyledProperty<ScrollViewer?> ScrollHostProperty =
        AvaloniaProperty.Register<WindowedStackPanel, ScrollViewer?>(nameof(ScrollHost));

    /// <summary>子项间距。</summary>
    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<WindowedStackPanel, double>(nameof(Spacing));

    /// <summary>尚未测量过的子项使用的估算尺寸（经验值，量测后回填）。</summary>
    public static readonly StyledProperty<double> EstimatedItemHeightProperty =
        AvaloniaProperty.Register<WindowedStackPanel, double>(nameof(EstimatedItemHeight), 24d);

    public bool EnableVirtualization
    {
        get => GetValue(EnableVirtualizationProperty);
        set => SetValue(EnableVirtualizationProperty, value);
    }

    public int VirtualizationThreshold
    {
        get => GetValue(VirtualizationThresholdProperty);
        set => SetValue(VirtualizationThresholdProperty, value);
    }

    public double OverscanViewports
    {
        get => GetValue(OverscanViewportsProperty);
        set => SetValue(OverscanViewportsProperty, value);
    }

    public ScrollViewer? ScrollHost
    {
        get => GetValue(ScrollHostProperty);
        set => SetValue(ScrollHostProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public double EstimatedItemHeight
    {
        get => GetValue(EstimatedItemHeightProperty);
        set => SetValue(EstimatedItemHeightProperty, value);
    }

    /// <summary>当前子项数量（含未物化项）。</summary>
    public int ItemCount => _entries.Count;

    /// <summary>当前是否实际处于虚拟化状态（开关打开且子项数达到阈值）。</summary>
    public bool IsVirtualizing => EnableVirtualization && _entries.Count >= Math.Max(1, VirtualizationThreshold);

    /// <summary>已物化（挂在可视树上）的子项数量，供诊断与测试使用。</summary>
    public int RealizedItemCount => Children.Count;

    /// <summary>待重构：请求物化但尚未完成布局测量的子项数量。</summary>

    static WindowedStackPanel()
    {
        AffectsMeasure<WindowedStackPanel>(
            EnableVirtualizationProperty,
            VirtualizationThresholdProperty,
            OverscanViewportsProperty,
            EstimatedItemHeightProperty,
            SpacingProperty);
        AffectsArrange<WindowedStackPanel>(SpacingProperty);
    }

    public void AddItem(Control control, bool pinned = false, bool pinnedByKind = false) =>
        InsertItem(_entries.Count, control, pinned, pinnedByKind);

    public void InsertItem(int index, Control control, bool pinned = false, bool pinnedByKind = false)
    {
        var entry = new ItemEntry(control, pinned, pinnedByKind);
        index = Math.Clamp(index, 0, _entries.Count);
        _entries.Insert(index, entry);
        Children.Insert(RealizedIndexBefore(index), control);
        entry.Realized = true;
        InvalidateMeasure();
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _entries.Count)
        {
            return;
        }

        var entry = _entries[index];
        if (entry.Realized)
        {
            Children.Remove(entry.Control);
            entry.Realized = false;
        }

        _entries.RemoveAt(index);
        InvalidateMeasure();
    }

    public void ClearItems()
    {
        _entries.Clear();
        Children.Clear();
        InvalidateMeasure();
    }

    /// <summary>取子项控件；未物化时返回 null。</summary>
    public Control? GetRealizedItem(int index) =>
        index >= 0 && index < _entries.Count && _entries[index].Realized ? _entries[index].Control : null;

    /// <summary>
    /// 请求物化指定子项（偏移映射等需要精确 Bounds 的场景），返回该子项是否存在。
    /// </summary>
    public bool RealizeItem(int index)
    {
        if (index < 0 || index >= _entries.Count)
        {
            return false;
        }

        var entry = _entries[index];
        if (entry.Realized)
        {
            return true;
        }

        entry.Realized = true;
        Children.Insert(RealizedIndexBefore(index), entry.Control);
        InvalidateMeasure();
        return true;
    }

    /// <summary>取子项在滚动方向上的顶边（未物化项用已测量/估算尺寸推算）。</summary>
    public bool TryGetItemTop(int index, out double top)
    {
        top = 0;
        if (index < 0 || index >= _entries.Count)
        {
            return false;
        }

        for (var i = 0; i < index; i++)
        {
            top += ResolveSize(_entries[i]) + Spacing;
        }

        return true;
    }

    /// <summary>子项尺寸：已量测优先，其次估算。</summary>
    public double GetItemHeight(int index) =>
        index >= 0 && index < _entries.Count ? ResolveSize(_entries[index]) : 0d;

    protected override Size MeasureOverride(Size availableSize)
    {
        var width = double.IsFinite(availableSize.Width) ? availableSize.Width : _lastWidth;
        if (!double.IsFinite(width))
        {
            width = 0;
        }

        var virtualizing = IsVirtualizing;
        var (windowTop, windowBottom) = ResolveVisibleWindow(virtualizing, availableSize);
        var realization = WindowedRealization.Resolve(
            FillSizeBuffer(), FillPinnedBuffer(), Spacing, windowTop, windowBottom);

        double y = 0;
        var maxWidth = 0d;
        for (var i = 0; i < _entries.Count; i++)
        {
            var entry = _entries[i];
            SetRealized(entry, realization[i]);

            if (entry.Realized)
            {
                entry.Control.Measure(new Size(width, double.PositiveInfinity));
                var desired = entry.Control.DesiredSize;
                entry.Size = desired.Height;
                entry.Measured = true;
                maxWidth = Math.Max(maxWidth, desired.Width);
            }

            y += ResolveSize(entry) + Spacing;
        }

        if (_entries.Count > 0)
        {
            y -= Spacing;
        }

        _lastWidth = width;
        return new Size(maxWidth, Math.Max(0, y));
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double y = 0;
        var childIndex = 0;
        foreach (var entry in _entries)
        {
            if (entry.Realized)
            {
                if (childIndex < Children.Count)
                {
                    var height = entry.Measured ? entry.Size : entry.Control.DesiredSize.Height;
                    Children[childIndex].Arrange(new Rect(0, y, finalSize.Width, Math.Max(0, height)));
                }

                childIndex++;
            }

            y += ResolveSize(entry) + Spacing;
        }

        return finalSize;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        SubscribeScrollHost();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        UnsubscribeScrollHost();
        base.OnDetachedFromVisualTree(e);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ScrollHostProperty)
        {
            SubscribeScrollHost();
        }
    }

    private void SubscribeScrollHost()
    {
        var host = ScrollHost ?? this.GetVisualAncestors().OfType<ScrollViewer>().FirstOrDefault();
        if (ReferenceEquals(host, _subscribedScrollHost))
        {
            return;
        }

        UnsubscribeScrollHost();
        _subscribedScrollHost = host;
        if (host is not null)
        {
            host.ScrollChanged += OnScrollHostChanged;
        }
    }

    private void UnsubscribeScrollHost()
    {
        if (_subscribedScrollHost is not null)
        {
            _subscribedScrollHost.ScrollChanged -= OnScrollHostChanged;
            _subscribedScrollHost = null;
        }
    }

    private void OnScrollHostChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (IsVirtualizing)
        {
            InvalidateMeasure();
        }
    }

    private double[] FillSizeBuffer()
    {
        if (_sizeBuffer.Length != _entries.Count)
        {
            _sizeBuffer = new double[_entries.Count];
        }

        for (var i = 0; i < _entries.Count; i++)
        {
            _sizeBuffer[i] = ResolveSize(_entries[i]);
        }

        return _sizeBuffer;
    }

    private bool[] FillPinnedBuffer()
    {
        if (_pinnedBuffer.Length != _entries.Count)
        {
            _pinnedBuffer = new bool[_entries.Count];
        }

        for (var i = 0; i < _entries.Count; i++)
        {
            _pinnedBuffer[i] = _entries[i].ShouldAlwaysRealize;
        }

        return _pinnedBuffer;
    }

    private (double Top, double Bottom) ResolveVisibleWindow(bool virtualizing, Size availableSize)
    {
        if (!virtualizing)
        {
            return (double.NegativeInfinity, double.PositiveInfinity);
        }

        var host = _subscribedScrollHost ?? ScrollHost;
        if (host is null)
        {
            return (double.NegativeInfinity, double.PositiveInfinity);
        }

        var viewportHeight = host.Viewport.Height;
        if (!double.IsFinite(viewportHeight) || viewportHeight <= 0)
        {
            viewportHeight = double.IsFinite(availableSize.Height) && availableSize.Height > 0
                ? availableSize.Height
                : 720;
        }

        return WindowedRealization.ResolveWindow(host.Offset.Y, viewportHeight, OverscanViewports);
    }

    private void SetRealized(ItemEntry entry, bool realized)
    {
        if (entry.Realized == realized)
        {
            return;
        }

        if (realized)
        {
            entry.Realized = true;
            Children.Insert(RealizedIndexBefore(_entries.IndexOf(entry)), entry.Control);
        }
        else
        {
            if (entry.Control.Bounds.Height > 0)
            {
                entry.Size = entry.Control.Bounds.Height;
                entry.Measured = true;
            }

            Children.Remove(entry.Control);
            entry.Realized = false;
        }
    }

    private int RealizedIndexBefore(int entryIndex)
    {
        var count = 0;
        for (var i = 0; i < entryIndex && i < _entries.Count; i++)
        {
            if (_entries[i].Realized)
            {
                count++;
            }
        }

        return count;
    }

    private double ResolveSize(ItemEntry entry) =>
        entry.Measured ? entry.Size : Math.Max(0, EstimatedItemHeight);
}

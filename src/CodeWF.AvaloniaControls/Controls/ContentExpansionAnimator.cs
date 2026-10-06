using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using CodeWF.AvaloniaControls.Animations;
using CodeWF.AvaloniaControls.Models;

namespace CodeWF.AvaloniaControls.Controls;

/// <summary>
/// 内容展开动画：单一进度（0 → 1）同时驱动布局尺寸与透明度，
/// 用于侧栏开合、折叠面板、大纲树展开等「占位尺寸变化」场景，替代宽高硬切。
/// <para>
/// <see cref="Duration"/> 为零或 <see cref="Animate"/> 为 false 时立即到达终态，
/// 因此全局「关闭界面动效」开关只需把时长资源归零即可生效，不影响布局与业务语义。
/// </para>
/// </summary>
public class ContentExpansionAnimator : Border
{
    private static readonly StyledProperty<double> ProgressProperty =
        AvaloniaProperty.Register<ContentExpansionAnimator, double>(nameof(Progress));

    public static readonly StyledProperty<bool> IsExpandedProperty =
        AvaloniaProperty.Register<ContentExpansionAnimator, bool>(nameof(IsExpanded), true);

    /// <summary>展开后的尺寸（水平方向为宽度，垂直方向为高度）。</summary>
    public static readonly StyledProperty<double> ExpandedSizeProperty =
        AvaloniaProperty.Register<ContentExpansionAnimator, double>(nameof(ExpandedSize), 320d);

    /// <summary>折叠后的尺寸（通常为 0；图标态侧栏可设为 48）。</summary>
    public static readonly StyledProperty<double> CollapsedSizeProperty =
        AvaloniaProperty.Register<ContentExpansionAnimator, double>(nameof(CollapsedSize));

    public static readonly StyledProperty<ExpansionOrientation> OrientationProperty =
        AvaloniaProperty.Register<ContentExpansionAnimator, ExpansionOrientation>(
            nameof(Orientation),
            ExpansionOrientation.Horizontal);

    /// <summary>动效时长；为零表示不做动画（全局关闭动效时由资源注入 0）。</summary>
    public static readonly StyledProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.Register<ContentExpansionAnimator, TimeSpan>(
            nameof(Duration),
            TimeSpan.FromMilliseconds(200));

    public static readonly StyledProperty<bool> AnimateProperty =
        AvaloniaProperty.Register<ContentExpansionAnimator, bool>(nameof(Animate), true);

    public ContentExpansionAnimator()
    {
        ClipToBounds = true;
    }

    public bool IsExpanded
    {
        get => GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public double ExpandedSize
    {
        get => GetValue(ExpandedSizeProperty);
        set => SetValue(ExpandedSizeProperty, value);
    }

    public double CollapsedSize
    {
        get => GetValue(CollapsedSizeProperty);
        set => SetValue(CollapsedSizeProperty, value);
    }

    public ExpansionOrientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public TimeSpan Duration
    {
        get => GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public bool Animate
    {
        get => GetValue(AnimateProperty);
        set => SetValue(AnimateProperty, value);
    }

    /// <summary>当前进度（0 = 折叠，1 = 展开），供测试与串联动效读取。</summary>
    public double Progress => GetValue(ProgressProperty);

    /// <summary>当前占位尺寸（由进度在折叠/展开尺寸之间插值得到）。</summary>
    public double CurrentSize => Interpolate(CollapsedSize, ExpandedSize, Math.Clamp(Progress, 0d, 1d));

    static ContentExpansionAnimator()
    {
        AffectsMeasure<ContentExpansionAnimator>(
            ProgressProperty,
            ExpandedSizeProperty,
            CollapsedSizeProperty,
            OrientationProperty);
        IsExpandedProperty.Changed.AddClassHandler<ContentExpansionAnimator>(
            (animator, _) => animator.OnIsExpandedChanged());
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var size = CurrentSize;
        if (Child is { } child)
        {
            var constraint = Orientation == ExpansionOrientation.Horizontal
                ? new Size(size, availableSize.Height)
                : new Size(availableSize.Width, size);
            child.Measure(constraint);
        }

        return Orientation == ExpansionOrientation.Horizontal
            ? new Size(size, double.IsFinite(availableSize.Height) ? availableSize.Height : 0)
            : new Size(double.IsFinite(availableSize.Width) ? availableSize.Width : 0, size);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var size = CurrentSize;
        if (Child is { } child)
        {
            var rect = Orientation == ExpansionOrientation.Horizontal
                ? new Rect(0, 0, size, finalSize.Height)
                : new Rect(0, 0, finalSize.Width, size);
            child.Arrange(rect);
        }

        Opacity = Math.Clamp(Progress, 0d, 1d);
        return Orientation == ExpansionOrientation.Horizontal
            ? new Size(size, finalSize.Height)
            : new Size(finalSize.Width, size);
    }

    private void OnIsExpandedChanged() => Run(IsExpanded ? 1d : 0d);

    private void Run(double target)
    {
        var from = Math.Clamp(Progress, 0d, 1d);
        if (!Animate || Duration <= TimeSpan.Zero || Math.Abs(from - target) < 0.001d)
        {
            SetCurrentValue(ProgressProperty, target);
            InvalidateMeasure();
            return;
        }

        if (target < from)
        {
            // 收起方向先把进度拉回 0，再由动画推到目标，保证每帧都有尺寸变化可测。
            SetCurrentValue(ProgressProperty, 0d);
        }

        var animation = AwaitableAnimation.CreateProgressAnimation(Duration, ProgressProperty);
        _ = AwaitableAnimation.RunAsync(this, animation)
            .ContinueWith(_ => InvalidateMeasure(), TaskScheduler.FromCurrentSynchronizationContext());
    }

    private static double Interpolate(double from, double to, double progress) => from + (to - from) * progress;
}

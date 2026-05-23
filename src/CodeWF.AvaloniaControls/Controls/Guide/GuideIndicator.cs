using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Controls;

public abstract class GuideIndicator : TemplatedControl
{
    public static readonly StyledProperty<int> StepCountProperty =
        AvaloniaProperty.Register<GuideIndicator, int>(nameof(StepCount));

    public static readonly StyledProperty<int> ActiveIndexProperty =
        AvaloniaProperty.Register<GuideIndicator, int>(nameof(ActiveIndex));

    public static readonly StyledProperty<GuideStyleType> StyleTypeProperty =
        AvaloniaProperty.Register<GuideIndicator, GuideStyleType>(nameof(StyleType));

    public int StepCount
    {
        get => GetValue(StepCountProperty);
        set => SetValue(StepCountProperty, value);
    }

    public int ActiveIndex
    {
        get => GetValue(ActiveIndexProperty);
        set => SetValue(ActiveIndexProperty, value);
    }

    public GuideStyleType StyleType
    {
        get => GetValue(StyleTypeProperty);
        set => SetValue(StyleTypeProperty, value);
    }
}

public class DefaultGuideIndicator : GuideIndicator
{
    public static readonly StyledProperty<double> IndicatorSizeProperty =
        AvaloniaProperty.Register<DefaultGuideIndicator, double>(nameof(IndicatorSize), 6);

    public static readonly StyledProperty<double> ItemSpacingProperty =
        AvaloniaProperty.Register<DefaultGuideIndicator, double>(nameof(ItemSpacing), 6);

    public static readonly StyledProperty<IBrush?> IndicatorBrushProperty =
        AvaloniaProperty.Register<DefaultGuideIndicator, IBrush?>(nameof(IndicatorBrush));

    public static readonly StyledProperty<IBrush?> ActiveIndicatorBrushProperty =
        AvaloniaProperty.Register<DefaultGuideIndicator, IBrush?>(nameof(ActiveIndicatorBrush));

    static DefaultGuideIndicator()
    {
        AffectsMeasure<DefaultGuideIndicator>(StepCountProperty, IndicatorSizeProperty, ItemSpacingProperty);
        AffectsRender<DefaultGuideIndicator>(StepCountProperty, ActiveIndexProperty, IndicatorBrushProperty, ActiveIndicatorBrushProperty);
    }

    public double IndicatorSize
    {
        get => GetValue(IndicatorSizeProperty);
        set => SetValue(IndicatorSizeProperty, value);
    }

    public double ItemSpacing
    {
        get => GetValue(ItemSpacingProperty);
        set => SetValue(ItemSpacingProperty, value);
    }

    public IBrush? IndicatorBrush
    {
        get => GetValue(IndicatorBrushProperty);
        set => SetValue(IndicatorBrushProperty, value);
    }

    public IBrush? ActiveIndicatorBrush
    {
        get => GetValue(ActiveIndicatorBrushProperty);
        set => SetValue(ActiveIndicatorBrushProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var count = System.Math.Max(0, StepCount);
        if (count == 0)
        {
            return default;
        }

        return new Size(count * IndicatorSize + (count - 1) * ItemSpacing, IndicatorSize);
    }

    public override void Render(DrawingContext context)
    {
        var count = System.Math.Max(0, StepCount);
        var size = IndicatorSize;
        var x = 0d;
        var y = (Bounds.Height - size) / 2;

        for (var i = 0; i < count; i++)
        {
            var brush = i == ActiveIndex ? ActiveIndicatorBrush : IndicatorBrush;
            context.DrawEllipse(brush, null, new Rect(x, y, size, size));
            x += size + ItemSpacing;
        }
    }
}

public class TextGuideIndicator : GuideIndicator
{
    public static readonly DirectProperty<TextGuideIndicator, string> IndicatorTextProperty =
        AvaloniaProperty.RegisterDirect<TextGuideIndicator, string>(
            nameof(IndicatorText),
            indicator => indicator.IndicatorText);

    private string _indicatorText = "0 / 0";

    static TextGuideIndicator()
    {
        AffectsMeasure<TextGuideIndicator>(IndicatorTextProperty);
    }

    public string IndicatorText
    {
        get => _indicatorText;
        private set => SetAndRaise(IndicatorTextProperty, ref _indicatorText, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == StepCountProperty || change.Property == ActiveIndexProperty)
        {
            IndicatorText = $"{ActiveIndex + 1} / {StepCount}";
        }
    }
}

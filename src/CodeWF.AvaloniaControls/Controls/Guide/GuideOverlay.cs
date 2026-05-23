using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Controls;

public class GuideOverlay : Control
{
    public static readonly StyledProperty<IBrush?> MaskBrushProperty =
        AvaloniaProperty.Register<GuideOverlay, IBrush?>(nameof(MaskBrush));

    public static readonly StyledProperty<Rect> TargetRegionProperty =
        AvaloniaProperty.Register<GuideOverlay, Rect>(nameof(TargetRegion));

    public static readonly StyledProperty<double> TargetRegionCornerRadiusProperty =
        AvaloniaProperty.Register<GuideOverlay, double>(nameof(TargetRegionCornerRadius));

    static GuideOverlay()
    {
        AffectsRender<GuideOverlay>(MaskBrushProperty, TargetRegionProperty, TargetRegionCornerRadiusProperty);
    }

    public IBrush? MaskBrush
    {
        get => GetValue(MaskBrushProperty);
        set => SetValue(MaskBrushProperty, value);
    }

    public Rect TargetRegion
    {
        get => GetValue(TargetRegionProperty);
        set => SetValue(TargetRegionProperty, value);
    }

    public double TargetRegionCornerRadius
    {
        get => GetValue(TargetRegionCornerRadiusProperty);
        set => SetValue(TargetRegionCornerRadiusProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        var brush = MaskBrush;
        if (brush is null)
        {
            return;
        }

        var bounds = new Rect(Bounds.Size);
        if (bounds.Width <= 0 || bounds.Height <= 0)
        {
            return;
        }

        var target = TargetRegion.Intersect(bounds);
        if (target.Width <= 0 || target.Height <= 0)
        {
            context.DrawRectangle(brush, null, bounds);
            return;
        }

        var radius = System.Math.Max(0, TargetRegionCornerRadius);
        var geometry = new GeometryGroup
        {
            FillRule = FillRule.EvenOdd
        };
        geometry.Children.Add(new RectangleGeometry(bounds));
        geometry.Children.Add(new RectangleGeometry(target, radius, radius));

        context.DrawGeometry(brush, null, geometry);
    }
}

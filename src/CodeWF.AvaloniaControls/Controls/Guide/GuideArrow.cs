using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Controls;

public sealed class GuideArrow : Control
{
    public static readonly StyledProperty<GuidePlacementMode> PlacementProperty =
        AvaloniaProperty.Register<GuideArrow, GuidePlacementMode>(nameof(Placement), GuidePlacementMode.Bottom);

    public static readonly StyledProperty<IBrush?> FillProperty =
        AvaloniaProperty.Register<GuideArrow, IBrush?>(nameof(Fill), Brushes.White);

    public static readonly StyledProperty<IBrush?> StrokeProperty =
        AvaloniaProperty.Register<GuideArrow, IBrush?>(nameof(Stroke), Brushes.LightGray);

    public static readonly StyledProperty<double> StrokeThicknessProperty =
        AvaloniaProperty.Register<GuideArrow, double>(nameof(StrokeThickness), 1);

    static GuideArrow()
    {
        AffectsRender<GuideArrow>(PlacementProperty, FillProperty, StrokeProperty, StrokeThicknessProperty);
    }

    public GuidePlacementMode Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public IBrush? Fill
    {
        get => GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public IBrush? Stroke
    {
        get => GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        var bounds = new Rect(Bounds.Size);
        if (bounds.Width <= 0 || bounds.Height <= 0)
        {
            return;
        }

        var points = GetTrianglePoints(Placement, bounds);
        var geometry = new StreamGeometry();
        using (var geometryContext = geometry.Open())
        {
            geometryContext.BeginFigure(points[0], true);
            geometryContext.LineTo(points[1]);
            geometryContext.LineTo(points[2]);
            geometryContext.EndFigure(true);
        }

        var stroke = Stroke;
        var thickness = System.Math.Max(0, StrokeThickness);
        var pen = stroke is null || thickness <= 0 ? null : new Pen(stroke, thickness);
        context.DrawGeometry(Fill, pen, geometry);
    }

    private static Point[] GetTrianglePoints(GuidePlacementMode placement, Rect bounds)
    {
        var width = bounds.Width;
        var height = bounds.Height;
        return placement switch
        {
            GuidePlacementMode.Top or GuidePlacementMode.TopLeft or GuidePlacementMode.TopRight =>
            [
                new Point(0, 0),
                new Point(width, 0),
                new Point(width / 2, height)
            ],
            GuidePlacementMode.Left or GuidePlacementMode.LeftTop or GuidePlacementMode.LeftBottom =>
            [
                new Point(0, 0),
                new Point(0, height),
                new Point(width, height / 2)
            ],
            GuidePlacementMode.Right or GuidePlacementMode.RightTop or GuidePlacementMode.RightBottom =>
            [
                new Point(width, 0),
                new Point(width, height),
                new Point(0, height / 2)
            ],
            _ =>
            [
                new Point(0, height),
                new Point(width, height),
                new Point(width / 2, 0)
            ]
        };
    }
}

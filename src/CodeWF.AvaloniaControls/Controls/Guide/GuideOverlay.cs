using System;
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

    public static readonly StyledProperty<IBrush?> TargetRegionBorderBrushProperty =
        AvaloniaProperty.Register<GuideOverlay, IBrush?>(nameof(TargetRegionBorderBrush), Brushes.DodgerBlue);

    public static readonly StyledProperty<double> TargetRegionBorderThicknessProperty =
        AvaloniaProperty.Register<GuideOverlay, double>(nameof(TargetRegionBorderThickness), 3);

    static GuideOverlay()
    {
        AffectsRender<GuideOverlay>(
            MaskBrushProperty,
            TargetRegionProperty,
            TargetRegionCornerRadiusProperty,
            TargetRegionBorderBrushProperty,
            TargetRegionBorderThicknessProperty);
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

    public IBrush? TargetRegionBorderBrush
    {
        get => GetValue(TargetRegionBorderBrushProperty);
        set => SetValue(TargetRegionBorderBrushProperty, value);
    }

    public double TargetRegionBorderThickness
    {
        get => GetValue(TargetRegionBorderThicknessProperty);
        set => SetValue(TargetRegionBorderThicknessProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        var brush = MaskBrush;
        if (brush is null) return;

        var bounds = new Rect(Bounds.Size);
        if (bounds.Width <= 0 || bounds.Height <= 0) return;

        var target = TargetRegion.Intersect(bounds);
        if (target.Width <= 0 || target.Height <= 0)
        {
            context.DrawRectangle(brush, null, bounds);
            return;
        }

        var radius = Math.Max(0, TargetRegionCornerRadius);
        var geometry = new GeometryGroup
        {
            FillRule = FillRule.EvenOdd
        };
        geometry.Children.Add(new RectangleGeometry(bounds));
        geometry.Children.Add(new RectangleGeometry(target, radius, radius));

        context.DrawGeometry(brush, null, geometry);
        DrawTargetBorder(context, target, radius);
    }

    private void DrawTargetBorder(DrawingContext context, Rect target, double radius)
    {
        var borderBrush = TargetRegionBorderBrush;
        var thickness = Math.Max(0, TargetRegionBorderThickness);
        if (borderBrush is null || thickness <= 0) return;

        var inset = thickness / 2;
        var borderRect = target.Deflate(new Thickness(inset));
        if (borderRect.Width <= 0 || borderRect.Height <= 0) return;

        context.DrawRectangle(
            null,
            new Pen(borderBrush, thickness),
            borderRect,
            Math.Max(0, radius - inset),
            Math.Max(0, radius - inset));
    }
}
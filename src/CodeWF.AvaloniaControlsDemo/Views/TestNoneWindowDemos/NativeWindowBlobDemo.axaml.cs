using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CodeWF.AvaloniaControlsDemo.Views.TestNoneWindowDemos;

public partial class NativeWindowBlobDemo : Window
{
    public NativeWindowBlobDemo()
    {
        InitializeComponent();
        Opened += (_, _) => WindowRegionHelper.ApplyPolygon(this, 6, CreateBlobRegionPoints());
    }

    private static Point[] CreateBlobRegionPoints()
    {
        const int segments = 14;
        var points = new List<Point>();
        var start = new Point(233, 34);

        points.Add(start);
        AddCubic(points, start, new Point(305, 18), new Point(390, 56), new Point(410, 132), segments);
        AddCubic(points, new Point(410, 132), new Point(434, 223), new Point(354, 318), new Point(252, 323), segments);
        AddCubic(points, new Point(252, 323), new Point(162, 327), new Point(60, 298), new Point(42, 213), segments);
        AddCubic(points, new Point(42, 213), new Point(25, 130), new Point(91, 52), new Point(164, 55), segments);
        AddCubic(points, new Point(164, 55), new Point(189, 56), new Point(207, 40), start, segments);

        return points.ToArray();
    }

    private static void AddCubic(List<Point> points, Point start, Point control1, Point control2, Point end,
        int segments)
    {
        for (var i = 1; i <= segments; i++) points.Add(Cubic(start, control1, control2, end, (double)i / segments));
    }

    private static Point Cubic(Point start, Point control1, Point control2, Point end, double t)
    {
        var u = 1 - t;
        var uu = u * u;
        var tt = t * t;
        var uuu = uu * u;
        var ttt = tt * t;

        return new Point(
            uuu * start.X + 3 * uu * t * control1.X + 3 * u * tt * control2.X + ttt * end.X,
            uuu * start.Y + 3 * uu * t * control1.Y + 3 * u * tt * control2.Y + ttt * end.Y);
    }

    private void Shape_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Button) return;

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) BeginMoveDrag(e);
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
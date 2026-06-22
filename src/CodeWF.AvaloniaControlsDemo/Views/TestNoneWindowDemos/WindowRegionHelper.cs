using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;

namespace CodeWF.AvaloniaControlsDemo.Views.TestNoneWindowDemos;

internal static class WindowRegionHelper
{
    public static void ApplyEllipse(Window window, double x, double y, double width, double height, double padding = 0)
    {
        if (!OperatingSystem.IsWindows()) return;

        var scaling = window.RenderScaling;
        var region = CreateEllipticRgn(
            Scale(x - padding, scaling),
            Scale(y - padding, scaling),
            Scale(x + width + padding, scaling),
            Scale(y + height + padding, scaling));

        ApplyRegion(window, region);
    }

    public static void ApplyPolygon(Window window, params Point[] points)
    {
        ApplyPolygon(window, 0, points);
    }

    public static void ApplyPolygon(Window window, double padding, params Point[] points)
    {
        if (!OperatingSystem.IsWindows() || points.Length < 3) return;

        points = InflateFromCenter(points, padding);
        var scaling = window.RenderScaling;
        var nativePoints = new NativePoint[points.Length];
        for (var i = 0; i < points.Length; i++)
            nativePoints[i] = new NativePoint(Scale(points[i].X, scaling), Scale(points[i].Y, scaling));

        ApplyRegion(window, CreatePolygonRgn(nativePoints, nativePoints.Length, 1));
    }

    private static Point[] InflateFromCenter(Point[] points, double padding)
    {
        if (padding <= 0) return points;

        double centerX = 0;
        double centerY = 0;
        foreach (var point in points)
        {
            centerX += point.X;
            centerY += point.Y;
        }

        var center = new Point(centerX / points.Length, centerY / points.Length);

        var inflated = new Point[points.Length];
        for (var i = 0; i < points.Length; i++)
        {
            var offset = points[i] - center;
            var length = Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y);
            if (length <= 0.001)
            {
                inflated[i] = points[i];
                continue;
            }

            inflated[i] = center + offset * ((length + padding) / length);
        }

        return inflated;
    }

    private static void ApplyRegion(Window window, IntPtr region)
    {
        if (region == IntPtr.Zero) return;

        var handle = window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
        if (handle == IntPtr.Zero || SetWindowRgn(handle, region, true) == 0) DeleteObject(region);
    }

    private static int Scale(double value, double scaling)
    {
        return (int)Math.Round(value * scaling, MidpointRounding.AwayFromZero);
    }

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr CreateEllipticRgn(int left, int top, int right, int bottom);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr CreatePolygonRgn(NativePoint[] points, int count, int fillMode);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern bool DeleteObject(IntPtr value);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowRgn(IntPtr hwnd, IntPtr region, bool redraw);

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct NativePoint
    {
        public NativePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public readonly int X;
        public readonly int Y;
    }
}
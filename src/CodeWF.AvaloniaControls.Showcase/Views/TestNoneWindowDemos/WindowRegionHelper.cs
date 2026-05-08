using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;

namespace CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

internal static class WindowRegionHelper
{
    public static void AttachRoundedCorners(Window window, double radius = 4)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var nativeRegionApplied = false;

        void Update()
        {
            if (window.WindowState == WindowState.Maximized
                || window.WindowState == WindowState.FullScreen
                || window.ClientSize.Width <= 0
                || window.ClientSize.Height <= 0
                || radius <= 0)
            {
                ClearRegionIfApplied(window, ref nativeRegionApplied);
                return;
            }

            var scaling = window.RenderScaling;
            var region = CreateRoundRectRgn(
                0,
                0,
                Math.Max(1, Scale(window.ClientSize.Width, scaling)),
                Math.Max(1, Scale(window.ClientSize.Height, scaling)),
                Math.Max(1, Scale(radius * 2, scaling)),
                Math.Max(1, Scale(radius * 2, scaling)));

            if (region == IntPtr.Zero)
            {
                return;
            }

            var handle = window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
            if (handle == IntPtr.Zero || SetWindowRgn(handle, region, true) == 0)
            {
                DeleteObject(region);
                return;
            }

            nativeRegionApplied = true;
        }

        window.Opened += (_, _) => Update();
        window.SizeChanged += (_, _) => Update();
        window.PropertyChanged += (_, change) =>
        {
            if (change.Property == Window.WindowStateProperty)
            {
                Update();
            }
        };
    }

    public static void ApplyEllipse(Window window, double x, double y, double width, double height)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var scaling = window.RenderScaling;
        var region = CreateEllipticRgn(
            Scale(x, scaling),
            Scale(y, scaling),
            Scale(x + width, scaling),
            Scale(y + height, scaling));

        ApplyRegion(window, region);
    }

    public static void ApplyPolygon(Window window, params Point[] points)
    {
        if (!OperatingSystem.IsWindows() || points.Length < 3)
        {
            return;
        }

        var scaling = window.RenderScaling;
        var nativePoints = new NativePoint[points.Length];
        for (var i = 0; i < points.Length; i++)
        {
            nativePoints[i] = new NativePoint(Scale(points[i].X, scaling), Scale(points[i].Y, scaling));
        }

        ApplyRegion(window, CreatePolygonRgn(nativePoints, nativePoints.Length, 1));
    }

    private static void ClearRegionIfApplied(Window window, ref bool nativeRegionApplied)
    {
        if (!nativeRegionApplied)
        {
            return;
        }

        var handle = window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
        if (handle != IntPtr.Zero && SetWindowRgn(handle, IntPtr.Zero, true) != 0)
        {
            nativeRegionApplied = false;
        }
    }

    private static void ApplyRegion(Window window, IntPtr region)
    {
        if (region == IntPtr.Zero)
        {
            return;
        }

        var handle = window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
        if (handle == IntPtr.Zero || SetWindowRgn(handle, region, true) == 0)
        {
            DeleteObject(region);
        }
    }

    private static int Scale(double value, double scaling)
    {
        return (int)Math.Round(value * scaling, MidpointRounding.AwayFromZero);
    }

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr CreateEllipticRgn(int left, int top, int right, int bottom);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom, int width, int height);

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

using Avalonia.Controls;
using System;

namespace CodeWF.AvaloniaControls.Helpers;

public static class OSHelper
{
    public static bool IsModernWindowSystem { get; } =
        !OperatingSystem.IsWindows() || OperatingSystem.IsWindowsVersionAtLeast(6, 2);

    public static ThreadStaticAttribute EnableOSVersionAwareDecorations<T>(this T window) where T : Window
    {
        window.SystemDecorations = IsModernWindowSystem ? SystemDecorations.Full : SystemDecorations.None;
        return new ThreadStaticAttribute();
    }
}
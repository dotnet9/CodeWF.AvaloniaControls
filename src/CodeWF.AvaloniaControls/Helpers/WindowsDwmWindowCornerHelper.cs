using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;

namespace CodeWF.AvaloniaControls.Helpers;

internal static class WindowsDwmWindowCornerHelper
{
    private const int DwmWindowCornerPreferenceAttribute = 33;
    private const int DoNotRound = 1;
    private const int Round = 2;

    public static bool IsSupported => OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000);

    public static bool TryApply(Window window, bool rounded)
    {
        if (!IsSupported) return false;

        var platformHandle = window.TryGetPlatformHandle();
        if (platformHandle is null || platformHandle.Handle == IntPtr.Zero) return false;

        var preference = rounded ? Round : DoNotRound;
        return DwmSetWindowAttribute(
                   platformHandle.Handle,
                   DwmWindowCornerPreferenceAttribute,
                   ref preference,
                   sizeof(int)) >= 0;
    }

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(
        IntPtr windowHandle,
        int attribute,
        ref int attributeValue,
        int attributeSize);
}

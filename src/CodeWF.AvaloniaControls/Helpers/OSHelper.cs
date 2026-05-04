using Avalonia.Controls;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CodeWF.AvaloniaControls.Helpers;

public static class OSHelper
{
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    public static bool IsModernWindowSystem { get; } =
        IsWindows && OperatingSystem.IsWindowsVersionAtLeast(6, 2);

    public static T EnableOSVersionAwareDecorations<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(this T window)
        where T : Window
    {
        SetEnumProperty(window, "WindowDecorations", IsModernWindowSystem ? "Full" : "None");

        if (!IsModernWindowSystem && window.CanResize)
        {
            SetBooleanProperty(window, "IsManagedResizerVisible", true);
        }

        return window;
    }

    private static void SetEnumProperty<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
        T target,
        string propertyName,
        string valueName)
    {
        var property = typeof(T).GetProperty(propertyName);
        if (property?.PropertyType.IsEnum != true || !property.CanWrite)
        {
            return;
        }

        property.SetValue(target, Enum.Parse(property.PropertyType, valueName));
    }

    private static void SetBooleanProperty<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
        T target,
        string propertyName,
        bool value)
    {
        var property = typeof(T).GetProperty(propertyName);
        if (property?.PropertyType != typeof(bool) || !property.CanWrite)
        {
            return;
        }

        property.SetValue(target, value);
    }
}

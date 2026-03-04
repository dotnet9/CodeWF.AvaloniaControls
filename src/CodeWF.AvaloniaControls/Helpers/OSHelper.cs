using Avalonia.Controls;
using System;
using System.Runtime.InteropServices;
using Ursa.Controls;

namespace CodeWF.AvaloniaControls.Helpers;

public static class OSHelper
{
    /// <summary>
    /// 判断当前操作系统是否为Windows
    /// </summary>
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// 判断当前操作系统是否为Linux
    /// </summary>
    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    /// <summary>
    /// 判断当前操作系统是否为macOS
    /// </summary>
    public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    /// <summary>
    /// 判断当前操作系统是否支持现代窗口装饰
    /// <value>
    ///     true: win8+
    ///     false: win7及更早版本、Linux、macOS
    /// </value>
    /// </summary>
    public static bool IsModernWindowSystem { get; } =
        IsWindows && OperatingSystem.IsWindowsVersionAtLeast(6, 2);

    /// <summary>
    /// 根据操作系统版本启用或禁用窗口装饰
    /// - Windows 8及更高版本启用窗口装饰
    /// - Windows 7及更早版本、Linux、macOS禁用窗口装饰
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="window"></param>
    /// <returns></returns>
    public static T EnableOSVersionAwareDecorations<T>(this T window) where T : UrsaWindow
    {
        window.SystemDecorations = IsModernWindowSystem ? SystemDecorations.Full : SystemDecorations.None;
        if(!IsModernWindowSystem && window.CanResize)
        {
            window.IsManagedResizerVisible = true;
        }
        return window;
    }
}
using Avalonia.Controls;
using System;
using Ursa.Controls;

namespace CodeWF.AvaloniaControls.Helpers;

public static class OSHelper
{
    /// <summary>
    /// 判断当前操作系统是否支持现代窗口装饰
    /// <value>
    ///     true: win8+ 或 非Windows系统
    ///     false: win7及更早版本
    /// </value>
    /// </summary>
    public static bool IsModernWindowSystem { get; } =
        !OperatingSystem.IsWindows() || OperatingSystem.IsWindowsVersionAtLeast(6, 2);

    /// <summary>
    /// 根据操作系统版本启用或禁用窗口装饰
    /// - Windows 8及更高版本或非Windows系统启用窗口装饰
    /// - Windows 7及更早版本禁用窗口装饰
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
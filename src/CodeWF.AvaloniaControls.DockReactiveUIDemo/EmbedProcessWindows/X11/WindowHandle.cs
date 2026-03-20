using Avalonia.Controls.Platform;
using Avalonia.Platform;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.X11;

/// <summary>
/// X11 窗口句柄包装类
/// </summary>
internal class WindowHandle : PlatformHandle, INativeControlHostDestroyableControlHandle
{
    private readonly IntPtr _x11Display;

    public WindowHandle(IntPtr handle, string? descriptor, IntPtr x11Display = default) : base(handle, descriptor)
    {
        _x11Display = x11Display == default ? NativeMethods.XOpenDisplay(IntPtr.Zero) : x11Display;
    }

    public void Destroy()
    {
        if (Handle != IntPtr.Zero && _x11Display != IntPtr.Zero)
        {
            NativeMethods.XDestroyWindow(_x11Display, Handle);
        }
    }
}
using Avalonia.Controls.Platform;
using Avalonia.Platform;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Platforms.Linux;

/// <summary>
/// Linux 窗口句柄实现
/// </summary>
internal class LinuxWindowHandle : PlatformHandle, INativeControlHostDestroyableControlHandle
{
    private readonly IntPtr _x11Display;

    public LinuxWindowHandle(IntPtr handle, string? descriptor, IntPtr x11Display = default) : base(handle, descriptor)
    {
        _x11Display = x11Display == default ? X11Api.XOpenDisplay(IntPtr.Zero) : x11Display;
    }

    public void Destroy()
    {
        if (Handle != IntPtr.Zero && _x11Display != IntPtr.Zero)
        {
            X11Api.XDestroyWindow(_x11Display, Handle);
        }
    }
}
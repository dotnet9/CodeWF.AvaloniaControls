using Avalonia.Controls.Platform;
using Avalonia.Platform;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Platforms.Windows;

/// <summary>
/// Windows 窗口句柄实现
/// </summary>
internal class WindowsWindowHandle : PlatformHandle, INativeControlHostDestroyableControlHandle
{
    public WindowsWindowHandle(IntPtr handle, string? descriptor) : base(handle, descriptor)
    {
    }

    public void Destroy()
    {
        if (Handle != IntPtr.Zero)
        {
            Win32Api.DestroyWindow(Handle);
        }
    }
}
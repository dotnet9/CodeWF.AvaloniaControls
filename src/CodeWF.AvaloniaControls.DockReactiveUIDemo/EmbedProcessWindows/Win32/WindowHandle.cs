using Avalonia.Controls.Platform;
using Avalonia.Platform;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Win32;

/// <summary>
/// Win32 窗口句柄包装类
/// </summary>
internal class WindowHandle : PlatformHandle, INativeControlHostDestroyableControlHandle
{
    public WindowHandle(IntPtr handle, string? descriptor) : base(handle, descriptor)
    {
    }

    public void Destroy()
    {
        _ = Win32Api.DestroyWindow(Handle);
    }
}
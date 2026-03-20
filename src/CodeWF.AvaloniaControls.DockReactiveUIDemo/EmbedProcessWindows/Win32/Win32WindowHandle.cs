using Avalonia.Controls.Platform;
using Avalonia.Platform;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Win32;

internal class Win32WindowHandle : PlatformHandle, INativeControlHostDestroyableControlHandle
{
    public Win32WindowHandle(IntPtr handle, string? descriptor) : base(handle, descriptor)
    {
    }

    public void Destroy()
    {
        _ = WinApi.DestroyWindow(Handle);
    }
}
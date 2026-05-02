using System;
using Avalonia.Controls.Platform;
using Avalonia.Platform;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Contracts;

/// <summary>
/// 原生窗口句柄接口
/// 封装不同平台的窗口句柄操作
/// </summary>
public interface INativeWindowHandle : IPlatformHandle, INativeControlHostDestroyableControlHandle
{
    /// <summary>
    /// 窗口句柄值
    /// </summary>
    new IntPtr Handle { get; }

    /// <summary>
    /// 句柄描述符
    /// </summary>
    string? Descriptor { get; }

    /// <summary>
    /// 销毁窗口句柄
    /// </summary>
    new void Destroy();
}

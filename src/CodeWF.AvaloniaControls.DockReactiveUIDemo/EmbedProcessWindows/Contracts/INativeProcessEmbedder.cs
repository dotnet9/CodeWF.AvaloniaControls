using System;
using Avalonia.Platform;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Contracts;

/// <summary>
/// 原生进程窗口嵌入器接口
/// </summary>
public interface INativeProcessEmbedder
{
    /// <summary>
    /// 创建并嵌入第三方进程窗口
    /// </summary>
    /// <param name="parent">父级平台句柄</param>
    /// <param name="createDefault">创建默认句柄的回退函数</param>
    /// <returns>嵌入后的窗口句柄</returns>
    IPlatformHandle CreateWindow(IPlatformHandle parent, Func<IPlatformHandle> createDefault);

    /// <summary>
    /// 关闭已嵌入的第三方进程
    /// </summary>
    void Close();

    /// <summary>
    /// 获取第三方进程的窗口句柄
    /// </summary>
    IntPtr ProcessWindowHandle { get; }
}
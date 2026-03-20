using Avalonia.Platform;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows;

/// <summary>
/// 本地进程窗体创建接口
/// </summary>
public interface INativeProcessWindowCreator
{
    /// <summary>
    /// 运行第三方进程，捕获主窗体贴入Avalonia控件
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="createDefault"></param>
    /// <returns></returns>
    IPlatformHandle CreateWindow(IPlatformHandle parent, Func<IPlatformHandle> createDefault);

    /// <summary>
    /// 关闭第三方进程
    /// </summary>
    void CloseWindow();
}
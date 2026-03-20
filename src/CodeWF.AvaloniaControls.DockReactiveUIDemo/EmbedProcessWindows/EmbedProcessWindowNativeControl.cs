using Avalonia.Controls;
using Avalonia.Platform;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Win32;
using CodeWF.Log.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows;

/// <summary>
/// 嵌入第三进程窗口的辅助控件，使用时放入Avalonia容器中即可
/// </summary>
public class EmbedProcessWindowNativeControl : NativeControlHost
{
    /// <summary>
    /// 是否已经启动进程并捕获该进程主窗口，避免重复创建
    /// </summary>
    private bool _isCreated;

    /// <summary>
    /// 第三方进程窗口辅助句柄
    /// </summary>
    private IPlatformHandle? _processWindowHandle;

    /// <summary>
    /// 保存创建的本地控件引用，在整个应用关闭时释放使用
    /// </summary>
    private static List<EmbedProcessWindowNativeControl> _allNativeControls = new();

    /// <summary>
    /// 创建进程交互控件
    /// </summary>
    /// <param name="processPath">进程路径</param>
    /// <param name="workDir">进程工作目录</param>
    /// <param name="arguments">进程命令行参数</param>
    /// <exception cref="NotImplementedException"></exception>
    public EmbedProcessWindowNativeControl(string processPath, string workDir, string? arguments)
    {
        // Windows平台
        if (OperatingSystem.IsWindows())
        {
            Creator = new Win32NativeProcessWindowCreator(processPath, workDir, arguments);
        }
        // TODO: Linux平台
        else if (OperatingSystem.IsLinux())
        {
            throw new NotImplementedException("Linux平台启动第三方进程待实现！");
        }
        else
        {
            throw new NotImplementedException("其他平台启动第三方进程待实现！");
        }

        // 记录当前控件，释放时需要
        _allNativeControls.Add(this);
    }

    // 注意：Creator属性和INativeProcessWindowCreator接口需已定义
    public INativeProcessWindowCreator Creator { get; }

    /// <summary>
    /// 启动第三方进程
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        if (_isCreated)
        {
            return _processWindowHandle;
        }
        _isCreated = true;

        _processWindowHandle = Creator?.CreateWindow(parent, () => base.CreateNativeControlCore(parent))
                               ?? base.CreateNativeControlCore(parent);
        return _processWindowHandle;
    }

    protected override void DestroyNativeControlCore(IPlatformHandle control)
    {
        base.DestroyNativeControlCore(control);
    }

    /// <summary>
    /// 释放所有控件（应用程序退出时建议调用）
    /// </summary>
    public static void CloseAll()
    {
        if (!_allNativeControls.Any())
        {
            return;
        }

        foreach (var control in _allNativeControls)
        {
            try
            {
                control.Creator?.CloseWindow();
            }
            catch (Exception ex)
            {
                var friendlyMsg = "关闭三方进程控件异常，请联系管理员！";
                Logger.Error($"关闭三方进程控件异常", ex, friendlyMsg);
            }
        }
    }
}
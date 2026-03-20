using Avalonia.Controls;
using Avalonia.Platform;
using CodeWF.Log.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows;

/// <summary>
/// 嵌入第三方进程窗口的辅助控件
/// 将外部进程的主窗口嵌入到当前 Avalonia 应用程序中
/// </summary>
public class EmbedProcessWindowNativeControl : NativeControlHost
{
    private bool _isCreated;
    private IPlatformHandle? _processWindowHandle;
    private static List<EmbedProcessWindowNativeControl> _allNativeControls = new();

    /// <summary>
    /// 创建进程交互控件
    /// </summary>
    /// <param name="processPath">进程路径</param>
    /// <param name="workDir">工作目录</param>
    /// <param name="arguments">命令行参数</param>
    public EmbedProcessWindowNativeControl(string processPath, string workDir, string? arguments)
    {
        if (OperatingSystem.IsWindows())
        {
            Creator = new Win32.NativeProcessWindowCreator(processPath, workDir, arguments);
        }
        else if (OperatingSystem.IsLinux())
        {
            Creator = new X11.NativeProcessWindowCreator(processPath, workDir, arguments);
        }
        else
        {
            throw new NotImplementedException("当前平台不支持启动第三方进程！");
        }

        _allNativeControls.Add(this);
    }

    public INativeProcessWindowCreator Creator { get; }

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        if (_isCreated)
        {
            return _processWindowHandle!;
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
    /// 关闭所有已创建的嵌入进程窗口
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
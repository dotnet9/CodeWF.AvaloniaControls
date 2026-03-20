using Avalonia.Controls;
using Avalonia.Platform;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Contracts;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Models;
using CodeWF.Log.Core;
using System;
using System.Collections.Generic;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Core;

/// <summary>
/// 原生进程窗口嵌入控件
/// 用于将外部进程的主窗口嵌入到当前 Avalonia 应用程序中
/// </summary>
public class ProcessEmbedHost : Avalonia.Controls.NativeControlHost
{
    private bool _isCreated;
    private IPlatformHandle? _processWindowHandle;
    private static readonly List<WeakReference<ProcessEmbedHost>> _instances = new();

    /// <summary>
    /// 进程嵌入器实例
    /// </summary>
    public INativeProcessEmbedder? Embedder { get; private set; }

    /// <summary>
    /// 创建进程交互控件
    /// </summary>
    /// <param name="options">进程嵌入配置</param>
    public ProcessEmbedHost(ProcessEmbedOptions options)
    {
        Embedder = ProcessEmbedderFactory.Create(options);
        _instances.Add(new WeakReference<ProcessEmbedHost>(this));
    }

    /// <summary>
    /// 创建进程交互控件
    /// </summary>
    /// <param name="processPath">进程路径</param>
    /// <param name="workingDirectory">工作目录</param>
    /// <param name="arguments">命令行参数</param>
    public ProcessEmbedHost(string processPath, string? workingDirectory = null, string? arguments = null)
    {
        var options = ProcessEmbedOptions.Create(processPath, workingDirectory, arguments);
        Embedder = ProcessEmbedderFactory.Create(options);
        _instances.Add(new WeakReference<ProcessEmbedHost>(this));
    }

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        if (_isCreated || Embedder == null)
        {
            return _processWindowHandle ?? base.CreateNativeControlCore(parent);
        }

        _isCreated = true;
        _processWindowHandle = Embedder.CreateWindow(parent, () => base.CreateNativeControlCore(parent));
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
        foreach (var weakRef in _instances)
        {
            if (weakRef.TryGetTarget(out var instance) && instance.Embedder != null)
            {
                try
                {
                    instance.Embedder.Close();
                }
                catch (Exception ex)
                {
                    Logger.Error("关闭三方进程控件异常", ex, "关闭三方进程控件异常，请联系管理员！");
                }
            }
        }

        _instances.Clear();
    }
}
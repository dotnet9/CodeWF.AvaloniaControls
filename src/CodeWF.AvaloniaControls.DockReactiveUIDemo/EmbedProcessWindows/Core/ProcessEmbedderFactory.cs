using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Contracts;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Models;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Platforms.Linux;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Platforms.Windows;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Core;

/// <summary>
/// 进程嵌入器工厂
/// 根据运行时平台自动创建对应的嵌入器实例
/// </summary>
public static class ProcessEmbedderFactory
{
    /// <summary>
    /// 创建平台对应的进程嵌入器
    /// </summary>
    /// <param name="options">进程嵌入配置</param>
    /// <returns>平台对应的嵌入器实例</returns>
    /// <exception cref="PlatformNotSupportedException">当前平台不支持时抛出</exception>
    public static INativeProcessEmbedder Create(ProcessEmbedOptions options)
    {
        if (OperatingSystem.IsWindows())
        {
            return new WindowsEmbedder(options);
        }

        if (OperatingSystem.IsLinux())
        {
            return new LinuxEmbedder(options);
        }

        throw new PlatformNotSupportedException("当前平台不支持嵌入第三方进程窗口");
    }

    /// <summary>
    /// 创建平台对应的进程嵌入器
    /// </summary>
    /// <param name="processPath">进程路径</param>
    /// <param name="workingDirectory">工作目录</param>
    /// <param name="arguments">命令行参数</param>
    /// <returns>平台对应的嵌入器实例</returns>
    public static INativeProcessEmbedder Create(string processPath, string? workingDirectory = null, string? arguments = null)
    {
        var options = ProcessEmbedOptions.Create(processPath, workingDirectory, arguments);
        return Create(options);
    }
}
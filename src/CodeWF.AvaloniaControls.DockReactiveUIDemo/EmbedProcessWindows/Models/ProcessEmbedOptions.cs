using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Models;

/// <summary>
/// 进程嵌入配置选项
/// </summary>
public class ProcessEmbedOptions
{
    /// <summary>
    /// 进程可执行文件路径
    /// </summary>
    public required string ProcessPath { get; init; }

    /// <summary>
    /// 工作目录
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <summary>
    /// 命令行参数
    /// </summary>
    public string? Arguments { get; init; }

    /// <summary>
    /// 等待进程窗口初始化的超时时间（毫秒）
    /// </summary>
    public int WindowReadyTimeoutMs { get; init; } = 5000;

    /// <summary>
    /// 查找窗口前的等待时间（毫秒）
    /// </summary>
    public int WindowSearchDelayMs { get; init; } = 50;

    /// <summary>
    /// 查找窗口的总超时时间（毫秒），默认 30 秒
    /// </summary>
    public int WindowSearchTimeoutMs { get; init; } = 30000;

    /// <summary>
    /// 创建配置
    /// </summary>
    public static ProcessEmbedOptions Create(string processPath, string? workDir = null, string? arguments = null)
    {
        if (string.IsNullOrWhiteSpace(processPath))
        {
            throw new ArgumentException("进程路径不能为空", nameof(processPath));
        }

        return new ProcessEmbedOptions
        {
            ProcessPath = processPath,
            WorkingDirectory = workDir,
            Arguments = arguments
        };
    }
}
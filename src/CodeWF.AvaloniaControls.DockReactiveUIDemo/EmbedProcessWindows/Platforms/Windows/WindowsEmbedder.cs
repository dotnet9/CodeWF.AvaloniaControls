using Avalonia.Platform;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Contracts;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Models;
using CodeWF.Log.Core;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Platforms.Windows;

/// <summary>
/// Windows 平台进程窗口嵌入器
/// </summary>
public class WindowsEmbedder : INativeProcessEmbedder
{
    private Process? _process;
    private WindowsWindowHandle? _windowHandle;
    private readonly ProcessEmbedOptions _options;

    public IntPtr ProcessWindowHandle { get; private set; }

    public WindowsEmbedder(ProcessEmbedOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IPlatformHandle CreateWindow(IPlatformHandle parent, Func<IPlatformHandle> createDefault)
    {
        try
        {
            _process = StartProcess();
            ProcessWindowHandle = GetMainWindowHandle();

            ModifyWindowStyle();
            ReParentWindow(parent);

            _windowHandle = new WindowsWindowHandle(ProcessWindowHandle, "ProcWinHandle");
            return _windowHandle;
        }
        catch (Exception ex)
        {
            Logger.Error($"Windows 平台嵌入进程异常({_options.ProcessPath})", ex, "启动第三方进程异常，请联系管理员！");
            Close();
            return createDefault();
        }
    }

    private Process StartProcess()
    {
        _process = Process.Start(new ProcessStartInfo
        {
            FileName = _options.ProcessPath,
            WorkingDirectory = _options.WorkingDirectory,
            Arguments = _options.Arguments
        });

        if (_process == null)
        {
            throw new InvalidOperationException("无法启动第三方进程");
        }

        if (!_process.WaitForInputIdle(_options.WindowReadyTimeoutMs))
        {
            throw new TimeoutException("第三方进程窗口初始化超时");
        }

        return _process;
    }

    private IntPtr GetMainWindowHandle()
    {
        var stopwatch = Stopwatch.StartNew();
        while (_process is { HasExited: false } && _process.MainWindowHandle == IntPtr.Zero)
        {
            if (stopwatch.ElapsedMilliseconds >= _options.WindowSearchTimeoutMs)
            {
                throw new TimeoutException("获取第三方进程窗口句柄超时");
            }

            Thread.Sleep(50);
            _process.Refresh();
        }

        if (_process is { HasExited: true })
        {
            throw new InvalidOperationException("第三方进程已退出，无法获取窗口句柄");
        }

        var handle = _process?.MainWindowHandle ?? IntPtr.Zero;
        if (handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("无法获取第三方进程窗口句柄");
        }

        return handle;
    }

    private void ModifyWindowStyle()
    {
        var handleRef = new HandleRef(null, ProcessWindowHandle);

        long style = Win32Api.GetWindowLongPtr(ProcessWindowHandle, Win32Constants.GWL_STYLE);
        style &= ~Win32Constants.WS_THICKFRAME;
        style &= ~Win32Constants.WS_MAXIMIZEBOX;
        style &= ~Win32Constants.WS_MINIMIZEBOX;
        style &= ~Win32Constants.WS_SYSMENU;
        style &= ~Win32Constants.WS_MINIMIZE;
        style &= ~Win32Constants.WS_MAXIMIZE;
        style &= ~Win32Constants.WS_POPUP;
        style &= ~Win32Constants.WS_CAPTION;
        style |= Win32Constants.WS_CHILD;
        style |= Win32Constants.WS_VISIBLE;
        Win32Api.SetWindowLongPtr(handleRef, Win32Constants.GWL_STYLE, (IntPtr)style);

        long exStyle = Win32Api.GetWindowLongPtr(ProcessWindowHandle, Win32Constants.GWL_EXSTYLE);
        exStyle &= ~Win32Constants.WS_EX_APPWINDOW;
        exStyle |= Win32Constants.WS_EX_TOOLWINDOW;
        Win32Api.SetWindowLongPtr(handleRef, Win32Constants.GWL_EXSTYLE, (IntPtr)exStyle);

        Win32Api.SetWindowPos(handleRef, IntPtr.Zero, 0, 0, 0, 0,
            Win32Constants.SWP_NOMOVE | Win32Constants.SWP_NOSIZE | Win32Constants.SWP_FRAMECHANGED);
    }

    private void ReParentWindow(IPlatformHandle parent)
    {
        Win32Api.SetParent(ProcessWindowHandle, parent.Handle);
    }

    public void Close()
    {
        try
        {
            if (_process is not { HasExited: false }) return;

            _process.CloseMainWindow();
            if (_process.WaitForExit(5000)) return;

            _process.Kill();
            _process = null;
        }
        catch (Exception ex)
        {
            Logger.Error($"关闭第三方进程异常({_options.ProcessPath})", ex, "关闭第三方进程异常，请联系管理员！");
        }
    }
}

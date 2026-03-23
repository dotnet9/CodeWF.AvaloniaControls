using Avalonia.Platform;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Contracts;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Models;
using CodeWF.Log.Core;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Platforms.Linux;

/// <summary>
/// Linux 平台进程窗口嵌入器
/// </summary>
public class LinuxEmbedder : INativeProcessEmbedder
{
    private Process? _process;
    private LinuxWindowHandle? _windowHandle;
    private readonly ProcessEmbedOptions _options;
    private IntPtr _x11Display;

    public IntPtr ProcessWindowHandle { get; private set; }

    public LinuxEmbedder(ProcessEmbedOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IPlatformHandle CreateWindow(IPlatformHandle parent, Func<IPlatformHandle> createDefault)
    {
        try
        {
            Logger.Info($"开始嵌入进程: {_options.ProcessPath}");

            _x11Display = X11Api.XOpenDisplay(IntPtr.Zero);
            if (_x11Display == IntPtr.Zero)
            {
                throw new InvalidOperationException("无法打开 X11 显示");
            }
            Logger.Info($"X11 显示打开成功: {_x11Display}");

            _process = StartProcess();
            Logger.Info($"进程启动成功，PID: {_process.Id}");

            ProcessWindowHandle = FindWindowByPID(_process.Id);
            Logger.Info($"找到窗口句柄: {ProcessWindowHandle}");

            if (ProcessWindowHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("找不到第三方进程窗口");
            }

            Logger.Info($"父窗口句柄: {parent.Handle}");

            // 修改窗口属性，确保窗口正确嵌入
            ModifyWindowAttributes();
            X11Api.XFlush(_x11Display);
            Thread.Sleep(50);

            // 先取消映射窗口，再重父化，最后重新映射
            int unmapResult = X11Api.XUnmapWindow(_x11Display, ProcessWindowHandle);
            Logger.Info($"XUnmapWindow 结果: {unmapResult}");
            X11Api.XFlush(_x11Display);
            Thread.Sleep(50);

            ReParentWindow(parent);
            X11Api.XFlush(_x11Display);
            Thread.Sleep(50);

            int mapResult = X11Api.XMapWindow(_x11Display, ProcessWindowHandle);
            Logger.Info($"XMapWindow 结果: {mapResult}");
            X11Api.XFlush(_x11Display);

            // 设置窗口位置和大小，使其铺满父容器
            ResizeWindowToParent(parent);
            X11Api.XFlush(_x11Display);

            _windowHandle = new LinuxWindowHandle(ProcessWindowHandle, "ProcWinHandle", _x11Display);
            Logger.Info($"创建窗口句柄完成");
            return _windowHandle;
        }
        catch (Exception ex)
        {
            Logger.Error($"Linux 平台嵌入进程异常({_options.ProcessPath})", ex, "启动第三方进程异常，请联系管理员！");
            return createDefault();
        }
    }

    private void ReParentWindow(IPlatformHandle parent)
    {
        try
        {
            int result = X11Api.XReparentWindow(_x11Display, ProcessWindowHandle, parent.Handle, 0, 0);
            Logger.Info($"XReparentWindow 结果: {result}");
        }
        catch (Exception ex)
        {
            Logger.Error("重父化窗口异常", ex);
            throw;
        }
    }

    private void ModifyWindowAttributes()
    {
        try
        {
            // 移除窗口的边框和标题栏
            var swa = new XSetWindowAttributes
            {
                override_redirect = 1 // 覆盖窗口管理器的重定向
            };

            // 设置窗口属性
            int result = X11Api.XChangeWindowAttributes(
                _x11Display,
                ProcessWindowHandle,
                X11Constants.CWOverrideRedirect, // 覆盖窗口管理器的重定向
                ref swa);

            Logger.Info($"修改窗口属性结果: {result}");
        }
        catch (Exception ex)
        {
            Logger.Error("修改窗口属性异常", ex);
        }
    }

    private void ResizeWindowToParent(IPlatformHandle parent)
    {
        try
        {
            // 获取父窗口的几何属性
            X11Api.XGetGeometry(_x11Display, parent.Handle, out _, out int x, out int y, out uint width, out uint height, out uint borderWidth, out uint depth);

            Logger.Info($"父窗口大小: {width}x{height}, 位置: ({x}, {y})");

            // 调整嵌入窗口的位置和大小
            int moveResult = X11Api.XMoveWindow(_x11Display, ProcessWindowHandle, 0, 0);
            Logger.Info($"XMoveWindow 结果: {moveResult}");

            int resizeResult = X11Api.XResizeWindow(_x11Display, ProcessWindowHandle, width, height);
            Logger.Info($"XResizeWindow 结果: {resizeResult}");
        }
        catch (Exception ex)
        {
            Logger.Error("调整窗口大小异常", ex);
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

        Thread.Sleep(_options.WindowSearchDelayMs);
        return _process;
    }

    private IntPtr FindWindowByPID(int pid)
    {
        var screen = X11Api.XDefaultScreen(_x11Display);
        var rootWindow = X11Api.XRootWindow(_x11Display, screen);

        // 多次尝试查找窗口，增加找到窗口的概率
        for (int i = 0; i < 10; i++)
        {
            Logger.Info($"尝试查找窗口 (尝试 {i + 1}/10)");
            var window = SearchWindowTree(rootWindow, pid);
            if (window != IntPtr.Zero)
            {
                Logger.Info($"找到窗口: {window}");
                return window;
            }
            Thread.Sleep(200);
        }
        Logger.Warn($"无法找到PID为 {pid} 的窗口");
        return IntPtr.Zero;
    }

    private IntPtr SearchWindowTree(IntPtr window, int targetPid)
    {
        IntPtr children = IntPtr.Zero;
        uint childCount = 0;

        try
        {
            X11Api.XQueryTree(_x11Display, window, out _, out _, out children, out childCount);

            if (children == IntPtr.Zero) return IntPtr.Zero;

            var childPtrs = new IntPtr[childCount];
            Marshal.Copy(children, childPtrs, 0, (int)childCount);

            foreach (var child in childPtrs)
            {
                if (child == IntPtr.Zero) continue;

                if (GetWindowPID(child) == targetPid)
                {
                    X11Api.XFree(children);
                    return child;
                }

                var found = SearchWindowTree(child, targetPid);
                if (found != IntPtr.Zero)
                {
                    X11Api.XFree(children);
                    return found;
                }
            }

            X11Api.XFree(children);
        }
        catch { }

        return IntPtr.Zero;
    }

    private int GetWindowPID(IntPtr window)
    {
        try
        {
            var atom = X11Api.XInternAtom(_x11Display, "_NET_WM_PID", true);
            if (atom == IntPtr.Zero) return -1;

            var prop = IntPtr.Zero;
            X11Api.XGetWindowProperty(_x11Display, window, atom, IntPtr.Zero, new IntPtr(1), false,
                (IntPtr)X11Constants.AnyPropertyType, out _, out _, out var nItems, out _, out prop);

            if (prop != IntPtr.Zero && nItems.ToInt64() > 0)
            {
                var pid = Marshal.ReadInt32(prop);
                X11Api.XFree(prop);
                return pid;
            }
        }
        catch { }

        return -1;
    }



    public void Close()
    {
        try
        {
            if (_x11Display != IntPtr.Zero)
            {
                if (ProcessWindowHandle != IntPtr.Zero)
                {
                    var rootReturn = X11Api.XDefaultRootWindow(_x11Display);
                    X11Api.XReparentWindow(_x11Display, ProcessWindowHandle, rootReturn, 0, 0);
                    X11Api.XFlush(_x11Display);
                }

                X11Api.XCloseDisplay(_x11Display);
                _x11Display = IntPtr.Zero;
            }

            if (_process is not { HasExited: true })
            {
                _process?.Kill();
            }

            _process?.Dispose();
            _process = null;
        }
        catch (Exception ex)
        {
            Logger.Error($"关闭第三方进程异常({_options.ProcessPath})", ex, "关闭第三方进程异常，请联系管理员！");
        }
    }
}
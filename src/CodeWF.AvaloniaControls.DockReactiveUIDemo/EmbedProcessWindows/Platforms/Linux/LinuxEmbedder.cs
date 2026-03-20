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
            _x11Display = X11Api.XOpenDisplay(IntPtr.Zero);
            if (_x11Display == IntPtr.Zero)
            {
                throw new InvalidOperationException("无法打开 X11 显示");
            }

            _process = StartProcess();
            ProcessWindowHandle = FindWindowByPID(_process.Id);

            ConfigureWindowEvents();
            ReParentWindow(parent);
            MapWindow();

            _windowHandle = new LinuxWindowHandle(ProcessWindowHandle, "ProcWinHandle", _x11Display);
            return _windowHandle;
        }
        catch (Exception ex)
        {
            Logger.Error($"Linux 平台嵌入进程异常({_options.ProcessPath})", ex, "启动第三方进程异常，请联系管理员！");
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

        Thread.Sleep(_options.WindowSearchDelayMs);
        return _process;
    }

    private IntPtr FindWindowByPID(int pid)
    {
        var screen = X11Api.XDefaultScreen(_x11Display);
        var rootWindow = X11Api.XRootWindow(_x11Display, screen);

        return SearchWindowTree(rootWindow, pid);
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

    private void ConfigureWindowEvents()
    {
        var swa = new XSetWindowAttributes
        {
            event_mask = (IntPtr)(
                X11Constants.ExposureMask |
                X11Constants.KeyPressMask |
                X11Constants.KeyReleaseMask |
                X11Constants.ButtonPressMask |
                X11Constants.ButtonReleaseMask |
                X11Constants.EnterWindowMask |
                X11Constants.LeaveWindowMask |
                X11Constants.PointerMotionMask)
        };

        X11Api.XChangeWindowAttributes(_x11Display, ProcessWindowHandle, X11Constants.CWEventMask, ref swa);
    }

    private void ReParentWindow(IPlatformHandle parent)
    {
        X11Api.XReparentWindow(_x11Display, ProcessWindowHandle, parent.Handle, 0, 0);
        X11Api.XFlush(_x11Display);
    }

    private void MapWindow()
    {
        X11Api.XMapWindow(_x11Display, ProcessWindowHandle);
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
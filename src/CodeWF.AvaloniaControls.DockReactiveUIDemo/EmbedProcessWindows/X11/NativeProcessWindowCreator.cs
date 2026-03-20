using Avalonia.Platform;
using CodeWF.Log.Core;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.X11;

/// <summary>
/// Linux 平台进程窗口嵌入实现
/// </summary>
public class NativeProcessWindowCreator : INativeProcessWindowCreator
{
    private Process? _currentProcess;
    private string _processPath;
    private string _workDir;
    private string? _arguments;
    private IntPtr _x11Display;
    private WindowHandle? _windowHandle;

    public IntPtr ProcessWindowHandle { get; private set; }

    public NativeProcessWindowCreator(string processPath, string workDir, string? arguments)
    {
        _processPath = processPath;
        _workDir = workDir;
        _arguments = arguments;
    }

    public IPlatformHandle CreateWindow(IPlatformHandle parent, Func<IPlatformHandle> createDefault)
    {
        try
        {
            _x11Display = NativeMethods.XOpenDisplay(IntPtr.Zero);
            if (_x11Display == IntPtr.Zero)
            {
                throw new InvalidOperationException("无法打开X11显示");
            }

            var screen = NativeMethods.XDefaultScreen(_x11Display);
            var rootWindow = NativeMethods.XRootWindow(_x11Display, screen);

            _currentProcess = Process.Start(new ProcessStartInfo
            {
                FileName = _processPath,
                WorkingDirectory = _workDir,
                Arguments = _arguments
            });

            if (_currentProcess == null)
            {
                throw new InvalidOperationException("无法启动第三方进程");
            }

            if (!_currentProcess.WaitForInputIdle(5000))
            {
                throw new TimeoutException("第三方进程窗口初始化超时");
            }

            Thread.Sleep(200);

            var window = FindWindowByPID(_currentProcess.Id, rootWindow);
            if (window == IntPtr.Zero)
            {
                throw new InvalidOperationException("无法获取第三方进程窗口句柄");
            }

            ProcessWindowHandle = window;

            XSetWindowAttributes swa = new XSetWindowAttributes();
            swa.event_mask = (IntPtr)(X11Constants.ExposureMask | X11Constants.KeyPressMask | X11Constants.KeyReleaseMask |
                                    X11Constants.ButtonPressMask | X11Constants.ButtonReleaseMask |
                                    X11Constants.EnterWindowMask | X11Constants.LeaveWindowMask |
                                    X11Constants.PointerMotionMask);

            NativeMethods.XChangeWindowAttributes(_x11Display, window, X11Constants.CWEventMask, ref swa);

            NativeMethods.XReparentWindow(_x11Display, window, parent.Handle, 0, 0);

            NativeMethods.XFlush(_x11Display);

            NativeMethods.XMapWindow(_x11Display, window);

            _windowHandle = new WindowHandle(window, "ProcWinHandle", _x11Display);
        }
        catch (Exception ex)
        {
            var friendlyMsg = "启动第三方进程异常，请联系管理员！";
            Logger.Error($"启动第三方进程异常({_processPath})", ex, friendlyMsg);
        }

        return _windowHandle ?? createDefault();
    }

    private IntPtr FindWindowByPID(int pid, IntPtr rootWindow)
    {
        var window = IntPtr.Zero;
        var rootReturn = IntPtr.Zero;
        var parentReturn = IntPtr.Zero;
        IntPtr children = IntPtr.Zero;
        uint childCount = 0;

        try
        {
            NativeMethods.XQueryTree(_x11Display, rootWindow, out rootReturn, out parentReturn, out children, out childCount);

            if (children != IntPtr.Zero)
            {
                var childPtrs = new IntPtr[childCount];
                Marshal.Copy(children, childPtrs, 0, (int)childCount);

                foreach (var child in childPtrs)
                {
                    if (child == IntPtr.Zero) continue;

                    var processId = GetWindowPID(child);
                    if (processId == pid)
                    {
                        window = child;
                        break;
                    }

                    window = FindWindowByPIDInChildren(child, pid);
                    if (window != IntPtr.Zero) break;
                }

                NativeMethods.XFree(children);
            }
        }
        catch { }

        return window;
    }

    private IntPtr FindWindowByPIDInChildren(IntPtr window, int pid)
    {
        var rootReturn = IntPtr.Zero;
        var parentReturn = IntPtr.Zero;
        IntPtr children = IntPtr.Zero;
        uint childCount = 0;

        try
        {
            NativeMethods.XQueryTree(_x11Display, window, out rootReturn, out parentReturn, out children, out childCount);

            if (children != IntPtr.Zero)
            {
                var childPtrs = new IntPtr[childCount];
                Marshal.Copy(children, childPtrs, 0, (int)childCount);

                foreach (var child in childPtrs)
                {
                    if (child == IntPtr.Zero) continue;

                    var processId = GetWindowPID(child);
                    if (processId == pid)
                    {
                        NativeMethods.XFree(children);
                        return child;
                    }

                    var found = FindWindowByPIDInChildren(child, pid);
                    if (found != IntPtr.Zero)
                    {
                        NativeMethods.XFree(children);
                        return found;
                    }
                }

                NativeMethods.XFree(children);
            }
        }
        catch { }

        return IntPtr.Zero;
    }

    private int GetWindowPID(IntPtr window)
    {
        try
        {
            var atom = NativeMethods.XInternAtom(_x11Display, "_NET_WM_PID", true);
            if (atom == IntPtr.Zero) return -1;

            IntPtr prop = IntPtr.Zero;
            NativeMethods.XGetWindowProperty(_x11Display, window, atom, IntPtr.Zero, new IntPtr(1), false,
                (IntPtr)X11Constants.AnyPropertyType, out var actualType, out var actualFormat,
                out var nItems, out var bytesAfter, out prop);

            if (prop != IntPtr.Zero && nItems.ToInt64() > 0)
            {
                var pid = Marshal.ReadInt32(prop);
                NativeMethods.XFree(prop);
                return pid;
            }
        }
        catch { }

        return -1;
    }

    public void CloseWindow()
    {
        try
        {
            if (_x11Display != IntPtr.Zero)
            {
                if (ProcessWindowHandle != IntPtr.Zero)
                {
                    var rootReturn = NativeMethods.XDefaultRootWindow(_x11Display);
                    NativeMethods.XReparentWindow(_x11Display, ProcessWindowHandle, rootReturn, 0, 0);
                    NativeMethods.XFlush(_x11Display);
                }

                NativeMethods.XCloseDisplay(_x11Display);
                _x11Display = IntPtr.Zero;
            }

            if (_currentProcess is not { HasExited: true })
            {
                _currentProcess?.Kill();
            }
            _currentProcess?.Dispose();
            _currentProcess = null;
        }
        catch (Exception ex)
        {
            var friendlyMsg = "关闭第三方进程异常，请联系管理员！";
            Logger.Error($"关闭第三方进程异常({_processPath})", ex, friendlyMsg);
        }
    }
}
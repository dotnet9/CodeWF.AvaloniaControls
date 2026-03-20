using Avalonia.Platform;
using CodeWF.Log.Core;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Win32;

/// <summary>
/// Windows 平台进程窗口嵌入实现
/// </summary>
public class NativeProcessWindowCreator : INativeProcessWindowCreator
{
    private Process? _currentProcess;
    private string _processPath;
    private string _workDir;
    private string? _arguments;

    /// <summary>
    /// 第三方进程主窗体句柄
    /// </summary>
    public IntPtr ProcessWindowHandle { get; private set; }

    public NativeProcessWindowCreator(string processPath, string workDir, string? arguments)
    {
        _processPath = processPath;
        _workDir = workDir;
        _arguments = arguments;
    }

    /// <summary>
    /// 启动第三方进程，并将进程主窗口嵌入控件
    /// </summary>
    public IPlatformHandle CreateWindow(IPlatformHandle parent, Func<IPlatformHandle> createDefault)
    {
        WindowHandle? windowHandle = null;
        try
        {
            // 1. 启动第三方进程并获取主窗口句柄
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

            // 等待窗口初始化（超时保护）
            if (!_currentProcess.WaitForInputIdle(5000))
            {
                throw new TimeoutException("第三方进程窗口初始化超时");
            }

            while (true)
            {
                Thread.Sleep(50);
                if (_currentProcess.MainWindowHandle != IntPtr.Zero)
                {
                    break;
                }
            }

            // 保存主窗体句柄引用，释放时需要
            ProcessWindowHandle = _currentProcess.MainWindowHandle;
            if (ProcessWindowHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("无法获取第三方进程窗口句柄");
            }

            // 2. 调整基础窗口样式（GWL_STYLE=-16）
            long style = Win32Api.GetWindowLongPtr(ProcessWindowHandle, Win32Constants.GWL_STYLE);

            // 修改主窗口样式，比如窗口边框、不在任务栏显示等
            style &= ~Win32Constants.WS_THICKFRAME; // 移除可调整大小的边框
            style &= ~Win32Constants.WS_MAXIMIZEBOX; // 移除最大化按钮
            style &= ~Win32Constants.WS_MINIMIZEBOX; // 移除最小化按钮
            style &= ~Win32Constants.WS_SYSMENU; // 移除系统菜单
            style &= ~Win32Constants.WS_MINIMIZE; // 移除初始最小化状态
            style &= ~Win32Constants.WS_MAXIMIZE; // 移除初始最大化状态
            style &= ~Win32Constants.WS_POPUP;
            style &= ~Win32Constants.WS_CAPTION;
            style |= Win32Constants.WS_CHILD; // 添加子窗口样式
            style |= Win32Constants.WS_VISIBLE;

            // 应用基础修改
            var handleRef = new HandleRef(null, ProcessWindowHandle);

            // 应用主窗口样式更新
            Win32Api.SetWindowLongPtr(handleRef, Win32Constants.GWL_STYLE, (IntPtr)style);

            // 3. 调整扩展窗口样式为工具窗口
            long exStyle = Win32Api.GetWindowLongPtr(ProcessWindowHandle, Win32Constants.GWL_EXSTYLE);
            exStyle &= ~Win32Constants.WS_EX_APPWINDOW; // 移除任务栏图标
            exStyle |= Win32Constants.WS_EX_TOOLWINDOW; // 添加工具窗口样式

            // 应用扩展样式修改
            Win32Api.SetWindowLongPtr(handleRef, Win32Constants.GWL_EXSTYLE, (IntPtr)exStyle);

            // 4. 刷新窗口样式（确保修改生效）
            Win32Api.SetWindowPos(handleRef, IntPtr.Zero, 0, 0, 0, 0,
                Win32Constants.SWP_NOMOVE | Win32Constants.SWP_NOSIZE | Win32Constants.SWP_FRAMECHANGED);

            // 5. 将主窗口贴在本地控件内
            Win32Api.SetParent(ProcessWindowHandle, parent.Handle);

            // 6. 返回包装后的窗口句柄
            windowHandle = new WindowHandle(ProcessWindowHandle, "ProcWinHandle");
        }
        catch (Exception ex)
        {
            var friendlyMsg = "启动第三方进程异常，请联系管理员！";
            Logger.Error($"启动第三方进程异常({_processPath})", ex, friendlyMsg);
        }

        return windowHandle;
    }

    public void CloseWindow()
    {
        try
        {
            if (_currentProcess is not { HasExited: false }) return;
            _currentProcess.CloseMainWindow();
            if (_currentProcess.WaitForExit(5000)) return;
            _currentProcess.Kill();
            _currentProcess = null;
        }
        catch (Exception ex)
        {
            var friendlyMsg = "关闭第三方进程异常，请联系管理员！";
            Logger.Error($"关闭第三方进程异常({_processPath})", ex, friendlyMsg);
        }
    }
}
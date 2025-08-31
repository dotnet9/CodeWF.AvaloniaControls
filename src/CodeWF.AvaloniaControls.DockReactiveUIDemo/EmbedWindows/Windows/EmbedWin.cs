using Avalonia.Platform;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows.Windows;

public class EmbedWin : INativeWindow
{
    private Process? _p;
    private string _processPath;

    public IntPtr ProcessWindowHandle { get; private set; }

    public EmbedWin(string processPath)
    {
        _processPath = processPath;
    }

    public IPlatformHandle CreateWindow(IPlatformHandle parent, Func<IPlatformHandle> createDefault)
    {
        // start the process
        _p = Process.Start(_processPath);
        _p.Exited += _p_Exited;

        // wait until p.MainWindowHandle is non-zero
        while (true)
        {
            Thread.Sleep(200);

            if (_p.MainWindowHandle != (IntPtr)0)
                break;
        }

        // set ProcessWindowHandle to the MainWindowHandle of the process
        ProcessWindowHandle = _p.MainWindowHandle;

        long style = WinApi.GetWindowLongPtr(ProcessWindowHandle, -16);

        // modify the style of the ChildWindow - remove the embedded window's frame and other attributes of
        // a stand alone window. Add child flag
        style &= ~0x00010000;
        style &= ~0x00800000;
        style &= ~0x80000000;
        style &= ~0x00400000;
        style &= ~0x00080000;
        style &= ~0x00020000;
        style &= ~0x00040000;
        style |= 0x40000000; // child

        HandleRef handleRef =
            new HandleRef(null, ProcessWindowHandle);

        // set the new style of the schild window
        WinApi.SetWindowLongPtr(handleRef, -16, (IntPtr)style);

        // set the parent of the ProcessWindowHandle to be the main window's handle
        WinApi.SetParent(ProcessWindowHandle, parent.Handle);
        // return the ProcessWindowHandle
        return new Win32WindowHandle(ProcessWindowHandle, "ProcWinHandle");
    }

    private void _p_Exited(object? sender, System.EventArgs e)
    {
    }

    public void CloseWindow()
    {
        _p?.CloseMainWindow();
        _p?.Close();
        _p = null;
    }
}
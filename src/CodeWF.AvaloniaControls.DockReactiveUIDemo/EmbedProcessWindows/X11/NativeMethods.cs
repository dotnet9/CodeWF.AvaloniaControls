using System;
using System.Runtime.InteropServices;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.X11;

/// <summary>
/// X11 API P/Invoke 声明
/// </summary>
public static class NativeMethods
{
    private const string LibX11 = "libX11.so.6";

    [DllImport(LibX11)]
    public static extern IntPtr XOpenDisplay(IntPtr display);

    [DllImport(LibX11)]
    public static extern int XCloseDisplay(IntPtr display);

    [DllImport(LibX11)]
    public static extern int XDefaultScreen(IntPtr display);

    [DllImport(LibX11)]
    public static extern IntPtr XRootWindow(IntPtr display, int screen);

    [DllImport(LibX11)]
    public static extern IntPtr XDefaultRootWindow(IntPtr display);

    [DllImport(LibX11)]
    public static extern int XFlush(IntPtr display);

    [DllImport(LibX11)]
    public static extern IntPtr XInternAtom(IntPtr display, string atomName, [MarshalAs(UnmanagedType.Bool)] bool onlyIfExists);

    [DllImport(LibX11)]
    public static extern int XGetWindowProperty(
        IntPtr display, IntPtr window, IntPtr property,
        IntPtr longOffset, IntPtr longLength, [MarshalAs(UnmanagedType.Bool)] bool delete,
        IntPtr reqType, out IntPtr actualType,
        out int actualFormat, out IntPtr nItems,
        out IntPtr bytesAfter, out IntPtr prop);

    [DllImport(LibX11)]
    public static extern int XFree(IntPtr ptr);

    [DllImport(LibX11)]
    public static extern int XQueryTree(
        IntPtr display, IntPtr window,
        out IntPtr rootReturn, out IntPtr parentReturn,
        out IntPtr childrenReturn, out uint nChildrenReturn);

    [DllImport(LibX11)]
    public static extern int XMapWindow(IntPtr display, IntPtr window);

    [DllImport(LibX11)]
    public static extern int XUnmapWindow(IntPtr display, IntPtr window);

    [DllImport(LibX11)]
    public static extern int XReparentWindow(IntPtr display, IntPtr window, IntPtr parent, int x, int y);

    [DllImport(LibX11)]
    public static extern int XDestroyWindow(IntPtr display, IntPtr window);

    [DllImport(LibX11, EntryPoint = "XChangeWindowAttributes")]
    public static extern int XChangeWindowAttributes(IntPtr display, IntPtr window, uint mask, ref XSetWindowAttributes attributes);

    [DllImport(LibX11)]
    public static extern int XMoveWindow(IntPtr display, IntPtr window, int x, int y);

    [DllImport(LibX11)]
    public static extern int XResizeWindow(IntPtr display, IntPtr window, uint width, uint height);

    [DllImport(LibX11)]
    public static extern int XSetWindowBackground(IntPtr display, IntPtr window, ulong background);

    [DllImport(LibX11)]
    public static extern IntPtr XCreateWindow(
        IntPtr display, IntPtr parent,
        int x, int y, uint width, uint height,
        uint borderWidth, int depth,
        uint windowClass, IntPtr visual,
        uint valuemask, ref XSetWindowAttributes attributes);

    [DllImport(LibX11)]
    public static extern IntPtr XCreateSimpleWindow(
        IntPtr display, IntPtr parent,
        int x, int y, uint width, uint height,
        int borderWidth, ulong border, ulong background);
}

/// <summary>
/// X11 窗口属性结构体
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct XSetWindowAttributes
{
    public IntPtr background_pixmap;
    public ulong background_pixel;
    public IntPtr border_pixmap;
    public ulong border_pixel;
    public int bit_gravity;
    public int win_gravity;
    public int backing_store;
    public ulong backing_planes;
    public ulong backing_pixel;
    public int save_under;
    public IntPtr event_mask;
    public IntPtr do_not_propagate_mask;
    public int override_redirect;
    public IntPtr colormap;
    public IntPtr cursor;
}

/// <summary>
/// X11 常量定义
/// </summary>
public static class X11Constants
{
    public const uint CWEventMask = 0x00000001;

    public const uint ExposureMask = 0x00000001;
    public const uint KeyPressMask = 0x00000001;
    public const uint KeyReleaseMask = 0x00000002;
    public const uint ButtonPressMask = 0x00000004;
    public const uint ButtonReleaseMask = 0x00000008;
    public const uint EnterWindowMask = 0x00000010;
    public const uint LeaveWindowMask = 0x00000020;
    public const uint PointerMotionMask = 0x00000040;
    public const uint PointerMotionHintMask = 0x00000080;
    public const uint Button1MotionMask = 0x00000100;
    public const uint Button2MotionMask = 0x00000200;
    public const uint Button3MotionMask = 0x00000400;
    public const uint Button4MotionMask = 0x00000800;
    public const uint Button5MotionMask = 0x00001000;
    public const uint KeymapStateMask = 0x00004000;
    public const uint ExposureMaskFull = 0x00000001;
    public const uint VisibilityChangeMask = 0x00000002;
    public const uint StructureNotifyMask = 0x00000004;
    public const uint ResizeRedirectMask = 0x00000008;
    public const uint SubstructureNotifyMask = 0x00000010;
    public const uint SubstructureRedirectMask = 0x00000020;
    public const uint FocusChangeMask = 0x00000040;
    public const uint PropertyChangeMask = 0x00000080;
    public const uint ColormapChangeMask = 0x00000100;
    public const uint OwnerGrabButtonMask = 0x00000200;

    public const int CopyFromParent = 0;
    public const int InputOutput = 1;
    public const int InputOnly = 2;

    public const uint StructureRedirectMask = SubstructureRedirectMask;
    public const uint AnyPropertyType = 0;
    public const uint None = 0;
}
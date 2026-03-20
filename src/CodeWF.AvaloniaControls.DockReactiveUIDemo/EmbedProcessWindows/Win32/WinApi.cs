using System;
using System.Runtime.InteropServices;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedProcessWindows.Win32;

public class Win32Constants
{
    // 窗口样式索引（GWL_xxx）
    public const int GWL_STYLE = -16;          // 基础窗口样式
    public const int GWL_EXSTYLE = -20;        // 扩展窗口样式

    // 基础窗口样式（WS_xxx）
    public const long WS_THICKFRAME = 0x00040000L;    // 可调整大小的边框
    public const long WS_MAXIMIZEBOX = 0x00010000L;   // 最大化按钮
    public const long WS_MINIMIZEBOX = 0x00020000L;   // 最小化按钮
    public const long WS_SYSMENU = 0x00080000L;       // 系统菜单（标题栏右键菜单）
    public const long WS_MINIMIZE = 0x00020000L;     // 初始最小化状态
    public const long WS_MAXIMIZE = 0x0010000L;       // 初始最大化状态
    public const long WS_CHILD = 0x40000000L;        // 子窗口样式
    public const long WS_POPUP = unchecked((int)0x80000000L); // 弹出窗口
    public const int WS_CAPTION = 0x00C00000;         // 标题栏
    public const int WS_VISIBLE = 0x10000000;         // 窗口可见

    // 扩展窗口样式（WS_EX_xxx）
    public const long WS_EX_APPWINDOW = 0x00040000L;  // 任务栏显示图标（与工具窗口冲突）
    public const long WS_EX_TOOLWINDOW = 0x00000080L; // 工具窗口样式

    // 窗口消息
    public const uint WM_SETTINGCHANGE = 0x001A;     // 刷新窗口样式的消息

    public const int SWP_NOMOVE = 0x0002;             // 不改变窗口位置（忽略X、Y参数）
    public const int SWP_NOSIZE = 0x0001;             // 不改变窗口大小（忽略cs、cy参数）
    public const int SWP_NOZORDER = 0x0004;           // 不改变窗口Z顺序（忽略hWndInsertAfter参数）
    public const int SWP_NOREDRAW = 0x0008;           // 不重绘窗口
    public const int SWP_NOACTIVATE = 0x0010;         // 不激活窗口
    public const int SWP_FRAMECHANGED = 0x0020;       // 强制发送 WM_NCCALCSIZE 消息，使窗口框架重绘
    public const int SWP_SHOWWINDOW = 0x0040;         // 显示窗口
    public const int SWP_HIDEWINDOW = 0x0080;         // 隐藏窗口
    public const int SWP_NOOWNERZORDER = 0x0200;      // 不改变所有者窗口的Z顺序
    public const int SWP_NOSENDCHANGING = 0x0400;      // 不发送 WM_WINDOWPOSCHANGING 消息

    // SetWindowPos函数的Z顺序插入位置
    public const int HWND_TOP = 0;                    // 将窗口置于Z顺序的顶部
    public const int HWND_BOTTOM = 1;                 // 将窗口置于Z顺序的底部
    public const int HWND_TOPMOST = -1;               // 将窗口置于所有非顶层窗口之上
    public const int HWND_NOTOPMOST = -2;             // 将窗口置于所有非顶层窗口之下
}

internal unsafe class WinApi
{
    public const int SW_HIDE = 0;
    public const int SW_SHOW = 5;

    public enum CommonControls : uint
    {
        ICC_LISTVIEW_CLASSES = 0x00000001,    // listview, header
        ICC_TREEVIEW_CLASSES = 0x00000002,    // treeview, tooltips
        ICC_BAR_CLASSES = 0x00000004,         // toolbar, statusbar, trackbar, tooltips
        ICC_TAB_CLASSES = 0x00000008,         // tab, tooltips
        ICC_UPDOWN_CLASS = 0x00000010,        // updown
        ICC_PROGRESS_CLASS = 0x00000020,      // progress
        ICC_HOTKEY_CLASS = 0x00000040,        // hotkey
        ICC_ANIMATE_CLASS = 0x00000080,       // animate
        ICC_WIN95_CLASSES = 0x000000FF,
        ICC_DATE_CLASSES = 0x00000100,        // month picker, date picker, time picker, updown
        ICC_USEREX_CLASSES = 0x00000200,      // comboex
        ICC_COOL_CLASSES = 0x00000400,        // rebar (coolbar) control
        ICC_INTERNET_CLASSES = 0x00000800,
        ICC_PAGESCROLLER_CLASS = 0x00001000,  // page scroller
        ICC_NATIVEFNTCTL_CLASS = 0x00002000,  // native font control
        ICC_STANDARD_CLASSES = 0x00004000,
        ICC_LINK_CLASS = 0x00008000
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INITCOMMONCONTROLSEX
    {
        public int dwSize;
        public uint dwICC;
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(
        HandleRef hWnd,              // 要操作的窗口句柄
        IntPtr hWndInsertAfter,      // 窗口Z序（设为IntPtr.Zero即可）
        int X,                       // 窗口X坐标（SWP_NOMOVE时忽略）
        int Y,                       // 窗口Y坐标（SWP_NOMOVE时忽略）
        int cx,                      // 窗口宽度（SWP_NOSIZE时忽略）
        int cy,                      // 窗口高度（SWP_NOSIZE时忽略）
        uint uFlags                  // 操作标志（组合使用）
    );

    [DllImport("Comctl32.dll")]
    public static extern void InitCommonControlsEx(ref INITCOMMONCONTROLSEX init);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool DestroyWindow(IntPtr hwnd);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "LoadLibraryW", ExactSpelling = true)]
    public static extern IntPtr LoadLibrary(string lib);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr CreateWindowEx(
        int dwExStyle,
        string lpClassName,
        string lpWindowName,
        uint dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam
    );

    [StructLayout(LayoutKind.Sequential)]
    public struct SETTEXTEX
    {
        public uint Flags;
        public uint Codepage;
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SendMessageW")]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, ref SETTEXTEX wParam, byte[] lParam);

    public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
    {
        if (IntPtr.Size == 8)
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        else
            return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
    }
}
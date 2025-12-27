using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace CodeWF.AvaloniaControls.Extensions;

/// <summary>
/// Window窗体键盘事件扩展类
/// 提供【全局ESC关闭窗体】【Enter执行自定义回调】核心能力，支持忽略多行TextBox回车拦截
/// 基于隧道模式实现，无视子控件焦点，窗体优先捕获按键事件
/// </summary>
public static class WindowKeyInputExtension
{
    /// <summary>
    /// 窗体与按键事件处理器的映射缓存
    /// 用于后续精准移除事件，防止内存泄漏与重复注销异常
    /// </summary>
    private static readonly Dictionary<Window, EventHandler<KeyEventArgs>> _windowKeyDownEventCache = new();

    /// <summary>
    /// 为窗体注册全局键盘按下事件（隧道模式）
    /// 内置：ESC键关闭窗体 | Enter键执行自定义回调 | 可配置是否忽略TextBox控件回车
    /// </summary>
    /// <param name="window">当前扩展的窗体实例</param>
    /// <param name="ignoreTextBoxEnter">是否忽略TextBox控件的Enter键触发（默认true）</param>
    /// <param name="enterCallback">Enter键触发的自定义回调方法（null则不响应Enter）</param>
    /// <exception cref="ArgumentNullException">窗体实例为null时抛出</exception>
    public static void RegisterGlobalKeyDownHandler(this Window window,
                                                    bool ignoreTextBoxEnter = true,
                                                    Action? enterCallback = null)
    {
        // 空值校验：杜绝传入null窗体导致的空引用异常
        if (window is null)
            throw new ArgumentNullException(nameof(window), "窗体实例不能为空，无法注册键盘事件");

        // 先移除旧事件：防止重复注册导致的事件多次触发问题
        if (_windowKeyDownEventCache.ContainsKey(window))
        {
            window.RemoveGlobalKeyDownHandler();
        }

        // 构建按键事件处理器核心逻辑
        EventHandler<KeyEventArgs> keyDownHandler = (sender, e) =>
        {
            HandleKeyDownEvent(window, e, ignoreTextBoxEnter, enterCallback);
        };

        // 缓存事件处理器 + 隧道模式注册事件（窗体优先捕获，无视子控件焦点）
        _windowKeyDownEventCache[window] = keyDownHandler;
        window.AddHandler(InputElement.KeyDownEvent, keyDownHandler, RoutingStrategies.Tunnel);
    }

    /// <summary>
    /// 移除窗体已注册的全局键盘按下事件
    /// 配套RegisterGlobalKeyDownHandler使用，防止内存泄漏
    /// </summary>
    /// <param name="window">当前扩展的窗体实例</param>
    /// <exception cref="ArgumentNullException">窗体实例为null时抛出</exception>
    public static void RemoveGlobalKeyDownHandler(this Window window)
    {
        // 空值校验
        if (window is null)
            throw new ArgumentNullException(nameof(window), "窗体实例不能为空，无法移除键盘事件");

        // 存在缓存的事件处理器，才执行移除操作
        if (_windowKeyDownEventCache.TryGetValue(window, out var keyDownHandler))
        {
            window.RemoveHandler(InputElement.KeyDownEvent, keyDownHandler);
            _windowKeyDownEventCache.Remove(window); // 移除缓存，释放资源
        }
    }

    /// <summary>
    /// 内部核心：按键事件处理逻辑解耦封装
    /// 分离事件注册与业务逻辑，提升代码可读性、可维护性
    /// </summary>
    /// <param name="window">目标窗体</param>
    /// <param name="e">键盘事件参数</param>
    /// <param name="ignoreTextBoxEnter">是否忽略TextBox回车</param>
    /// <param name="enterCallback">Enter自定义回调</param>
    private static void HandleKeyDownEvent(Window window, KeyEventArgs e, bool ignoreTextBoxEnter, Action? enterCallback)
    {
        // ========== 1. ESC键：无条件关闭窗体 ==========
        if (e.Key == Key.Escape)
        {
            window.Close();
            e.Handled = true; // 标记事件已处理，阻止向下传递给子控件
            return;
        }

        // ========== 2. Enter键：带条件执行自定义回调 ==========
        // 无回调/非Enter键，直接跳过
        if (enterCallback is null || e.Key != Key.Enter) return;

        // 判断是否需要忽略当前TextBox的Enter触发
        if (IsNeedIgnoreTextBoxEnter(e, ignoreTextBoxEnter)) return;

        // 执行自定义回调 + 标记事件已处理
        enterCallback.Invoke();
        e.Handled = true;
    }

    /// <summary>
    /// 内部辅助：判断是否需要忽略当前TextBox控件的Enter键触发
    /// 精准匹配【多行可换行TextBox】场景，放行其原生回车换行功能
    /// </summary>
    /// <param name="e">键盘事件参数</param>
    /// <param name="ignoreTextBoxEnter">外部配置的是否忽略开关</param>
    /// <returns>true=忽略Enter，false=执行Enter回调</returns>
    private static bool IsNeedIgnoreTextBoxEnter(KeyEventArgs e, bool ignoreTextBoxEnter)
    {
        // 开关关闭：不忽略任何TextBox的Enter，直接返回false
        if (!ignoreTextBoxEnter) return false;

        // 焦点不在TextBox上：无需忽略，返回false
        if (e.Source is not TextBox textBox) return false;

        // 满足【多行TextBox】条件：放行原生回车换行，返回true（忽略回调）
        return textBox.AcceptsReturn
               && textBox.TextWrapping == TextWrapping.Wrap;
    }
}
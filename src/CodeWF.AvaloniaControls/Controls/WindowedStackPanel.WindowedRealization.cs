using System;
using System.Collections.Generic;

namespace CodeWF.AvaloniaControls.Controls;

/// <summary>
/// 视口窗口物化判定（纯逻辑，无 UI 依赖）：给定每个子项的高度、是否固定物化、
/// 间距与视口窗口区间，计算哪些子项需要真实创建控件。
/// <para>
/// 抽成纯函数是为了让「窗口裁剪策略」可被独立单元测试，并被多种面板实现复用。
/// 窗口区间用 <see cref="double.NegativeInfinity"/> / <see cref="double.PositiveInfinity"/>
/// 表示「不裁剪」。
/// </para>
/// </summary>
public static class WindowedRealization
{
    /// <summary>
    /// 计算每个子项是否需要物化。区间判定为「与窗口重叠」，边界相切也算命中，
    /// 避免滚动到恰好边界时出现空白行。
    /// </summary>
    public static bool[] Resolve(
        IReadOnlyList<double> heights,
        IReadOnlyList<bool> pinned,
        double spacing,
        double windowTop,
        double windowBottom)
    {
        var result = new bool[heights.Count];
        var noWindow = double.IsNegativeInfinity(windowTop) || double.IsPositiveInfinity(windowBottom);
        var y = 0d;
        for (var i = 0; i < heights.Count; i++)
        {
            var height = Math.Max(0, heights[i]);
            result[i] = noWindow
                        || (i < pinned.Count && pinned[i])
                        || (y + height >= windowTop && y <= windowBottom);
            y += height + spacing;
        }

        return result;
    }

    /// <summary>视口窗口加上下 overscan 后的区间。</summary>
    public static (double Top, double Bottom) ResolveWindow(
        double offsetY,
        double viewportHeight,
        double overscanViewports)
    {
        var height = double.IsFinite(viewportHeight) && viewportHeight > 0 ? viewportHeight : 0d;
        if (height <= 0)
        {
            return (double.NegativeInfinity, double.PositiveInfinity);
        }

        var margin = Math.Max(0, overscanViewports) * height;
        return (offsetY - margin, offsetY + height + margin);
    }
}

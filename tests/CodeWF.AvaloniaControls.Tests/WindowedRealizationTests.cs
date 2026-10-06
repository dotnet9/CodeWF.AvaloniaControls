using CodeWF.AvaloniaControls.Controls;

namespace CodeWF.AvaloniaControls.Tests;

/// <summary>
/// 窗口化面板的裁剪策略（纯逻辑）：窗口内物化、窗口外释放、固定项始终物化、overscan 扩展区间。
/// </summary>
public sealed class WindowedRealizationTests
{
    private const double ItemHeight = 20;
    private const int ItemCount = 220;

    [Fact]
    public void Resolve_WhenNoWindow_RealizesEveryItem()
    {
        var result = WindowedRealization.Resolve(
            Heights(), Pinned(), 0, double.NegativeInfinity, double.PositiveInfinity);

        Assert.Equal(ItemCount, result.Count(realized => realized));
    }

    [Fact]
    public void Resolve_WhenWindowCoversViewport_RealizesOnlyVisibleItems()
    {
        var result = WindowedRealization.Resolve(Heights(), Pinned(), 0, 0, 200);

        Assert.True(result[0]);
        Assert.True(result[10]);
        Assert.False(result[11]);
        Assert.Equal(11, result.Count(realized => realized));
    }

    [Fact]
    public void Resolve_WhenItemIsPinnedOutOfWindow_KeepsItRealized()
    {
        var pinned = new bool[ItemCount];
        pinned[ItemCount - 1] = true;

        var result = WindowedRealization.Resolve(Heights(), pinned, 0, 0, 200);

        Assert.True(result[ItemCount - 1]);
        Assert.Equal(12, result.Count(realized => realized));
    }

    [Fact]
    public void Resolve_WithSpacing_AccountsForGap()
    {
        var heights = Enumerable.Repeat(ItemHeight, 10).ToArray();

        // 间距 4：第 6 项顶边 = 6 * 24 = 144，窗口 0..150 覆盖到第 6 项。
        var result = WindowedRealization.Resolve(heights, new bool[10], 4, 0, 150);

        Assert.True(result[6]);
        Assert.False(result[7]);
    }

    [Fact]
    public void ResolveWindow_WithViewportAndOverscan_ExtendsBothSides()
    {
        var (top, bottom) = WindowedRealization.ResolveWindow(1000, 200, 2);

        Assert.Equal(600, top, 3);
        Assert.Equal(1600, bottom, 3);
    }

    [Fact]
    public void ResolveWindow_WithInvalidViewport_DisablesClipping()
    {
        var (top, bottom) = WindowedRealization.ResolveWindow(100, 0, 2);

        Assert.True(double.IsNegativeInfinity(top));
        Assert.True(double.IsPositiveInfinity(bottom));
    }

    private static double[] Heights() => Enumerable.Repeat(ItemHeight, ItemCount).ToArray();

    private static bool[] Pinned() => new bool[ItemCount];
}

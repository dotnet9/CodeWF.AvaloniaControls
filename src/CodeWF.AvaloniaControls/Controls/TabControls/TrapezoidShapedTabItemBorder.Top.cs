using Avalonia;
using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Controls;

public partial class TrapezoidShapedTabItemBorder
{
    private static void DrawTopFirstTabItemBorder(StreamGeometryContext ctx, Rect adjustedRect, CornerRadius radius,
        Rect rect)
    {
        var x = adjustedRect.Left;
        var y = adjustedRect.Bottom;

        // 左下角开始
        ctx.BeginFigure(new Point(x, y), isFilled: true);

        // 左下角外圆
        if (radius.BottomLeft > 0)
        {
            x = rect.Left + radius.BottomLeft;
            y = adjustedRect.Bottom - radius.BottomLeft;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.BottomLeft, radius.BottomLeft),
                0,
                false,
                SweepDirection.CounterClockwise);
        }

        // 左边直线
        y = adjustedRect.Top + radius.TopLeft;
        ctx.LineTo(new Point(x, y));

        // 左上角内圆角
        if (radius.TopLeft > 0)
        {
            x += radius.TopLeft;
            y = adjustedRect.Top;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.TopLeft, radius.TopLeft),
                0,
                false,
                SweepDirection.Clockwise);
        }

        // 上边直线
        x = adjustedRect.Right - radius.TopRight * 2 - radius.BottomRight * 2;
        ctx.LineTo(new Point(x, y));

        // 右上角内圆角
        if (radius.TopRight > 0)
        {
            x += radius.TopRight;
            y += radius.TopRight * DiagonalFilletRatio;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.TopRight, radius.TopRight),
                0,
                false,
                SweepDirection.Clockwise);
        }

        // 右边斜线
        x = adjustedRect.Right - radius.BottomRight;
        y = adjustedRect.Bottom - radius.BottomRight;
        ctx.LineTo(new Point(x, y));

        // 右下角外圆
        if (radius.BottomRight > 0)
        {
            x = rect.Right;
            y = adjustedRect.Bottom;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.BottomRight, radius.BottomRight),
                0,
                false,
                SweepDirection.CounterClockwise);
        }
    }

    private static void DrawTopLastTabItemBorder(StreamGeometryContext ctx, Rect adjustedRect, CornerRadius radius,
        Rect rect)
    {
        var x = adjustedRect.Left;
        var y = adjustedRect.Bottom;

        // 开始路径，左下角
        ctx.BeginFigure(new Point(x, y), isFilled: true);

        // 左下角外圆角
        if (radius.BottomLeft > 0)
        {
            x = rect.Left + radius.BottomLeft;
            y = adjustedRect.Bottom - radius.BottomLeft;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.BottomLeft, radius.BottomLeft),
                0,
                false,
                SweepDirection.CounterClockwise);
        }

        // 左边斜线
        x = adjustedRect.Left + radius.BottomLeft * 2 + radius.TopLeft;
        y = adjustedRect.Top + radius.TopLeft * DiagonalFilletRatio;
        ctx.LineTo(new Point(x, y));

        // 左上角内圆角
        if (radius.TopLeft > 0)
        {
            x = adjustedRect.Left + radius.BottomLeft * 2 + radius.TopLeft * 2;
            y = adjustedRect.Top;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.TopLeft, radius.TopLeft),
                0,
                false,
                SweepDirection.Clockwise);
        }

        // 上边直线
        x = adjustedRect.Right - radius.TopRight - radius.BottomRight;
        y = adjustedRect.Top;
        ctx.LineTo(new Point(x, y));

        // 右上角内圆角
        if (radius.TopRight > 0)
        {
            x = adjustedRect.Right - radius.BottomRight;
            y = adjustedRect.Top + radius.TopRight;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.TopRight, radius.TopRight),
                0,
                false,
                SweepDirection.Clockwise);
        }

        // 右直线
        x = adjustedRect.Right - radius.BottomRight;
        y = adjustedRect.Bottom - radius.BottomRight;
        ctx.LineTo(new Point(x, y));

        // 右下角外圆角
        if (radius.BottomRight > 0)
        {
            x = rect.Right;
            y = adjustedRect.Bottom;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.BottomRight, radius.BottomRight),
                0,
                false,
                SweepDirection.CounterClockwise);
        }
    }


    private static void DrawTopOtherTabItemBorder(StreamGeometryContext ctx, Rect adjustedRect, CornerRadius radius,
        Rect rect)
    {
        var x = adjustedRect.Left;
        var y = adjustedRect.Bottom;

        // 开始路径，左下角
        ctx.BeginFigure(new Point(x, y), isFilled: true);

        // 左下角外圆角
        if (radius.BottomLeft > 0)
        {
            x = rect.Left + radius.BottomLeft;
            y = adjustedRect.Bottom - radius.BottomLeft;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.BottomLeft, radius.BottomLeft),
                0,
                false,
                SweepDirection.CounterClockwise);
        }

        // 左边斜线
        x = adjustedRect.Left + radius.BottomLeft * 2 + radius.TopLeft;
        y = adjustedRect.Top + radius.TopLeft * DiagonalFilletRatio;
        ctx.LineTo(new Point(x, y));

        // 左上角内圆角
        if (radius.TopLeft > 0)
        {
            x = adjustedRect.Left + radius.BottomLeft * 2 + radius.TopLeft * 2;
            y = adjustedRect.Top;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.TopLeft, radius.TopLeft),
                0,
                false,
                SweepDirection.Clockwise);
        }

        // 上边直线
        x = adjustedRect.Right - radius.TopRight * 2 - radius.BottomRight * 2;
        y = adjustedRect.Top;
        ctx.LineTo(new Point(x, y));

        // 右上角内圆角
        if (radius.TopRight > 0)
        {
            x += radius.TopRight;
            y += radius.TopRight * DiagonalFilletRatio;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.TopRight, radius.TopRight),
                0,
                false,
                SweepDirection.Clockwise);
        }

        // 右边斜线
        x = adjustedRect.Right - radius.BottomRight;
        y = adjustedRect.Bottom - radius.BottomRight;
        ctx.LineTo(new Point(x, y));

        // 右下角外圆
        if (radius.BottomRight > 0)
        {
            x = rect.Right;
            y = adjustedRect.Bottom;
            ctx.ArcTo(
                new Point(x, y),
                new Size(radius.BottomRight, radius.BottomRight),
                0,
                false,
                SweepDirection.CounterClockwise);
        }
    }
}
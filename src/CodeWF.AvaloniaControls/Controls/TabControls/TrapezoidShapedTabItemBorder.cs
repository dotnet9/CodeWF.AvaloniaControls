using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Controls.TabControls;

public partial class TrapezoidShapedTabItemBorder : Control
{
    public const double DiagonalFilletRatio = 0.8;

    public static readonly StyledProperty<IBrush> BorderBrushProperty =
        AvaloniaProperty.Register<TrapezoidShapedTabItemBorder, IBrush>(nameof(BorderBrush),
            new SolidColorBrush(Color.Parse("#05CCCCCC")));

    public static readonly StyledProperty<double> BorderThicknessProperty =
        AvaloniaProperty.Register<TrapezoidShapedTabItemBorder, double>(nameof(BorderThickness), 1);

    public static readonly StyledProperty<IBrush> BackgroundProperty =
        AvaloniaProperty.Register<TrapezoidShapedTabItemBorder, IBrush>(nameof(Background), Brushes.DarkGreen);

    public IBrush BorderBrush
    {
        get => GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public double BorderThickness
    {
        get => GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public IBrush Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        if (BorderThickness < 1)
        {
            return;
        }

        if (Parent?.Parent?.Parent is not TabControl tabControl ||
            Parent?.Parent is not TabItem currentTabItem)
        {
            return;
        }

        var index = tabControl.Items.IndexOf(currentTabItem);
        var isFirst = index == 0;
        var isLast = index == tabControl.Items.Count - 1;
        var radius = currentTabItem.CornerRadius;

        // 获取控件的尺寸
        var rect = new Rect(Bounds.Size);
        var borderThickness = BorderThickness;
        // 偏移路径使线条对齐像素网格
        var halfBorder = borderThickness / 2.0;
        var adjustedRect = rect.Deflate(halfBorder);

        // 设置边框路径
        var pathGeometry = new StreamGeometry();
        using (var ctx = pathGeometry.Open())
        {
            if (isFirst & !isLast)
            {
                if (tabControl.TabStripPlacement == Dock.Top)
                {
                    DrawTopFirstTabItemBorder(ctx, adjustedRect, radius, rect);
                }
            }
            else if (!isFirst && isLast)
            {
                if (tabControl.TabStripPlacement == Dock.Top)
                {
                    DrawTopLastTabItemBorder(ctx, adjustedRect, radius, rect);
                }
            }
            else
            {
                if (tabControl.TabStripPlacement == Dock.Top)
                {
                    DrawTopOtherTabItemBorder(ctx, adjustedRect, radius, rect);
                }
            }

            // 底边消失（不绘制）
            // 这里直接跳过底边路径，确保底边消失
            ctx.EndFigure(isClosed: true);
        }

        // 绘制边框
        context.DrawGeometry(Background, new Pen(BorderBrush, BorderThickness)
        {
            Thickness = BorderThickness,
            LineJoin = PenLineJoin.Round, // 圆角连接
            LineCap = PenLineCap.Round // 圆角端点
        }, pathGeometry);
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace CodeWF.AvaloniaControls.Controls;

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

        if (!TryGetTopTabContext(out var index, out var itemCount, out var radius))
        {
            return;
        }

        var isFirst = index == 0;
        var isLast = index == itemCount - 1;

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
            if (isFirst && !isLast)
            {
                DrawTopFirstTabItemBorder(ctx, adjustedRect, radius, rect);
            }
            else if (!isFirst && isLast)
            {
                DrawTopLastTabItemBorder(ctx, adjustedRect, radius, rect);
            }
            else
            {
                DrawTopOtherTabItemBorder(ctx, adjustedRect, radius, rect);
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

    private bool TryGetTopTabContext(out int index, out int itemCount, out CornerRadius radius)
    {
        var tabItem = this.FindAncestorOfType<TabItem>();
        if (tabItem is not null)
        {
            var tabControl = tabItem.FindAncestorOfType<TabControl>();
            if (tabControl is not null && tabControl.TabStripPlacement == Dock.Top)
            {
                index = tabControl.Items.IndexOf(tabItem);
                itemCount = tabControl.Items.Count;
                radius = tabItem.CornerRadius;
                return index >= 0;
            }
        }

        var tabStripItem = this.FindAncestorOfType<TabStripItem>();
        if (tabStripItem is not null)
        {
            var tabStrip = tabStripItem.FindAncestorOfType<TabStrip>();
            if (tabStrip is not null)
            {
                index = tabStrip.Items.IndexOf(tabStripItem);
                itemCount = tabStrip.Items.Count;
                radius = tabStripItem.CornerRadius;
                return index >= 0;
            }
        }

        index = -1;
        itemCount = 0;
        radius = default;
        return false;
    }
}

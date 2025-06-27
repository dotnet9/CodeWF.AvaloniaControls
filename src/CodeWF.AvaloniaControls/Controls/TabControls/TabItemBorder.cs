using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Controls.TabControls;

public class TabItemBorder : Control
{
    // 定义依赖属性用于设置圆角半径和边框颜色
    public static readonly StyledProperty<double> CornerRadiusProperty =
        AvaloniaProperty.Register<TabItemBorder, double>(nameof(CornerRadius), 2);

    public static readonly StyledProperty<IBrush> BorderBrushProperty =
        AvaloniaProperty.Register<TabItemBorder, IBrush>(nameof(BorderBrush), new SolidColorBrush(Color.Parse("#05CCCCCC")));

    public static readonly StyledProperty<double> BorderThicknessProperty =
        AvaloniaProperty.Register<TabItemBorder, double>(nameof(BorderThickness), 1);

    public static readonly StyledProperty<IBrush> BackgroundProperty =
        AvaloniaProperty.Register<TabItemBorder, IBrush>(nameof(Background), new LinearGradientBrush()
        {
            StartPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
            GradientStops =
            [
                new GradientStop(Color.Parse("#BAE7FF"), 0),
                new GradientStop(Color.Parse("#FFFFFF"), 1)
            ]
        });

    public static readonly StyledProperty<bool> IsFirstItemProperty =
        AvaloniaProperty.Register<TabItemBorder, bool>(nameof(IsFirstItem), false);

    public double CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

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

    public bool IsFirstItem
    {
        get => GetValue(IsFirstItemProperty);
        set => SetValue(IsFirstItemProperty, value);
    }

    public TabItemBorder()
    {
        //MessageCenter.Messenger.Register<string, string>(this, MessageToken.ThemeColorChanged, OnThemeColorChanged);
    }

    private void OnThemeColorChanged(object? sender, string e)
    {
        this.InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        if (BorderThickness < 1)
        {
            return;
        }

        var tabControl = Parent.Parent.Parent as TabControl;
        var currentTabItem = Parent.Parent as Level1TabItem;
        var isFirst = false;
        var isLast = false;
        if (tabControl != null && currentTabItem != null)
        {
            var index = tabControl.Items.IndexOf(currentTabItem);
            isFirst = index == 0;
            isLast = index == tabControl.Items.Count;
        }
        double radius = CornerRadius;

        // 获取控件的尺寸
        var rect = new Rect(Bounds.Size);
        double borderThickness = BorderThickness;
        // 偏移路径使线条对齐像素网格
        double halfBorder = borderThickness / 2.0;
        var adjustedRect = rect.Deflate(halfBorder);

        // 设置边框路径
        var pathGeometry = new StreamGeometry();
        using (var ctx = pathGeometry.Open())
        {
            // 开始路径，左下角
            ctx.BeginFigure(new Point(adjustedRect.Left, adjustedRect.Bottom), isFilled: true);

            // 左下角外圆角
            ctx.ArcTo(
                new Point(adjustedRect.Left + radius, adjustedRect.Bottom - radius),
                new Size(radius, radius),
                0,
                false,
                SweepDirection.CounterClockwise);

            // 左边直线
            ctx.LineTo(new Point(adjustedRect.Left + radius, adjustedRect.Top + radius));

            // 左上角内圆角
            ctx.ArcTo(
                new Point(rect.Left + radius + radius, adjustedRect.Top),
                new Size(radius, radius),
                0,
                false,
                SweepDirection.Clockwise);

            // 上边直线
            ctx.LineTo(new Point(adjustedRect.Right - radius - radius, adjustedRect.Top));

            // 右上角内圆角
            ctx.ArcTo(
                new Point(adjustedRect.Right - radius, adjustedRect.Top + radius),
                new Size(radius, radius),
                0,
                false,
                SweepDirection.Clockwise);

            // 右边直线
            ctx.LineTo(new Point(adjustedRect.Right - radius, adjustedRect.Bottom - radius));

            // 右下角外圆角
            ctx.ArcTo(
                new Point(adjustedRect.Right, adjustedRect.Bottom),
                new Size(radius, radius),
                0,
                false,
                SweepDirection.CounterClockwise);

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
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Controls;

public class LeftTop2RightBottomLine : Control
{
    public static readonly StyledProperty<IBrush> BorderBrushProperty =
        AvaloniaProperty.Register<LeftTop2RightBottomLine, IBrush>(nameof(BorderBrush),
            new SolidColorBrush(Color.Parse("#CCCCCC")));

    public static readonly StyledProperty<double> BorderThicknessProperty =
        AvaloniaProperty.Register<LeftTop2RightBottomLine, double>(nameof(BorderThickness), 1);

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


    public override void Render(DrawingContext context)
    {
        base.Render(context);
        if (BorderThickness < 1)
        {
            return;
        }

        var rect = new Rect(Bounds.Size);

        context.DrawLine(
            new Pen(BorderBrush, BorderThickness)
            {
                LineCap = PenLineCap.Round
            },
            new Point(rect.Left, rect.Top),
            new Point(rect.Right, rect.Bottom)
        );
    }
}
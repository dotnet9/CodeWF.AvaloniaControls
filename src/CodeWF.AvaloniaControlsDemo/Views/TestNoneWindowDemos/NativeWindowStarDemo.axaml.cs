using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CodeWF.AvaloniaControlsDemo.Views.TestNoneWindowDemos;

public partial class NativeWindowStarDemo : Window
{
    public NativeWindowStarDemo()
    {
        InitializeComponent();
        Opened += (_, _) => WindowRegionHelper.ApplyPolygon(
            this,
            6,
            new Point(215, 28),
            new Point(258, 156),
            new Point(392, 156),
            new Point(284, 235),
            new Point(326, 365),
            new Point(215, 286),
            new Point(104, 365),
            new Point(146, 235),
            new Point(38, 156),
            new Point(172, 156));
    }

    private void Shape_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Button) return;

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) BeginMoveDrag(e);
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
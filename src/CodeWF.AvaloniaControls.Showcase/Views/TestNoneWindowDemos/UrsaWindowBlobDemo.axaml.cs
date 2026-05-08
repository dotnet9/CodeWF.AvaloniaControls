using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia;
using Ursa.Controls;

namespace CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

public partial class UrsaWindowBlobDemo : UrsaWindow
{
    public UrsaWindowBlobDemo()
    {
        InitializeComponent();
        Opened += (_, _) => WindowRegionHelper.ApplyPolygon(
            this,
            new Point(233, 34),
            new Point(305, 18),
            new Point(390, 56),
            new Point(410, 132),
            new Point(434, 223),
            new Point(354, 318),
            new Point(252, 323),
            new Point(162, 327),
            new Point(60, 298),
            new Point(42, 213),
            new Point(25, 130),
            new Point(91, 52),
            new Point(164, 55));
    }

    private void Shape_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Button)
        {
            return;
        }

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}

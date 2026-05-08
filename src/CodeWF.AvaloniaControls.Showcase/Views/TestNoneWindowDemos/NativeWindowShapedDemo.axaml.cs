using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

public partial class NativeWindowShapedDemo : Window
{
    public NativeWindowShapedDemo()
    {
        InitializeComponent();
        Opened += (_, _) => WindowRegionHelper.ApplyEllipse(this, 22, 22, 316, 316);
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

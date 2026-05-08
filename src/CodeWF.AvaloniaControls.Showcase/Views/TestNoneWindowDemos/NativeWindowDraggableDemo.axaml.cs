using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

public partial class NativeWindowDraggableDemo : Window
{
    public NativeWindowDraggableDemo()
    {
        InitializeComponent();
    }

    private void TitleBar_OnPointerPressed(object? sender, PointerPressedEventArgs e)
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

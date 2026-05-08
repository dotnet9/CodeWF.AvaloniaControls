using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CodeWF.AvaloniaControls.Controls;

namespace CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

public partial class CodeWFWindowShapedDemo : CodeWFWindow
{
    public CodeWFWindowShapedDemo()
    {
        InitializeComponent();
    }

    private void Shell_OnPointerPressed(object? sender, PointerPressedEventArgs e)
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

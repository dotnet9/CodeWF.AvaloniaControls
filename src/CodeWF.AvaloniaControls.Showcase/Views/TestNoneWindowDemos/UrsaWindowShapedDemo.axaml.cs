using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Ursa.Controls;

namespace CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

public partial class UrsaWindowShapedDemo : UrsaWindow
{
    public UrsaWindowShapedDemo()
    {
        InitializeComponent();
        Opened += (_, _) => WindowRegionHelper.ApplyEllipse(this, 22, 22, 316, 316);
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

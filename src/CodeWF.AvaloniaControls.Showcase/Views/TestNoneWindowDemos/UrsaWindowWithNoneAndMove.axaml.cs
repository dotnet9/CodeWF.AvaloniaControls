using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Ursa.Controls;

namespace CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

public partial class UrsaWindowWithNoneAndMove : UrsaWindow
{
    public UrsaWindowWithNoneAndMove()
    {
        InitializeComponent();
        WindowRegionHelper.AttachRoundedCorners(this);
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

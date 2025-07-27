using Avalonia;
using Avalonia.Controls;
using CodeWF.AvaloniaControls.Models;

namespace CodeWF.AvaloniaControls.Controls;

public partial class StatusCard : UserControl
{
    public StatusCard()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<string?> TopTextProperty =
        AvaloniaProperty.Register<StatusLabel, string?>(nameof(TopText), nameof(TopText));

    public string? TopText
    {
        get => GetValue(TopTextProperty);
        set => SetValue(TopTextProperty, value);
    }

    public static readonly StyledProperty<string?> BottomTextProperty =
        AvaloniaProperty.Register<StatusLabel, string?>(nameof(BottomText), nameof(BottomText));

    public string? BottomText
    {
        get => GetValue(BottomTextProperty);
        set => SetValue(BottomTextProperty, value);
    }

    public static readonly StyledProperty<StatusLabelKind?> KindProperty =
        AvaloniaProperty.Register<StatusLabel, StatusLabelKind?>(nameof(Kind), StatusLabelKind.Normal);

    public StatusLabelKind? Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }
}
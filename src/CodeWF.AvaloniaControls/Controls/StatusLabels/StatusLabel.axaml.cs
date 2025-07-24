using Avalonia;
using Avalonia.Controls;
using CodeWF.AvaloniaControls.Models;

namespace CodeWF.AvaloniaControls.Controls;

public partial class StatusLabel : UserControl
{
    public StatusLabel()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<string?> LeftTextProperty =
        AvaloniaProperty.Register<StatusLabel, string?>(nameof(LeftText), nameof(LeftText));

    public string? LeftText
    {
        get => GetValue(LeftTextProperty);
        set => SetValue(LeftTextProperty, value);
    }

    public static readonly StyledProperty<string?> RightTextProperty =
        AvaloniaProperty.Register<StatusLabel, string?>(nameof(RightText), nameof(RightText));

    public string? RightText
    {
        get => GetValue(RightTextProperty);
        set => SetValue(RightTextProperty, value);
    }

    public static readonly StyledProperty<StatusLabelKind?> KindProperty =
        AvaloniaProperty.Register<StatusLabel, StatusLabelKind?>(nameof(Kind), StatusLabelKind.Normal);

    public StatusLabelKind? Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }
}
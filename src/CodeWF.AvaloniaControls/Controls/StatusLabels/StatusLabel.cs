using Avalonia;
using Avalonia.Controls.Primitives;
using CodeWF.AvaloniaControls.Models;

namespace CodeWF.AvaloniaControls.Controls;

public class StatusLabel : TemplatedControl
{
    public static readonly StyledProperty<string?> LeftTextProperty =
        AvaloniaProperty.Register<StatusLabel, string?>(nameof(LeftText));

    public string? LeftText
    {
        get => GetValue(LeftTextProperty);
        set => SetValue(LeftTextProperty, value);
    }

    public static readonly StyledProperty<string?> RightTextProperty =
        AvaloniaProperty.Register<StatusLabel, string?>(nameof(RightText));

    public string? RightText
    {
        get => GetValue(RightTextProperty);
        set => SetValue(RightTextProperty, value);
    }

    public static readonly StyledProperty<StatusLabelKind?> KindProperty =
        AvaloniaProperty.Register<StatusLabel, StatusLabelKind?>(nameof(Kind), StatusLabelKind.Debug);

    public StatusLabelKind? Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }
}

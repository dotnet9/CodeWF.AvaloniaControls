using Avalonia;
using Avalonia.Controls.Primitives;

namespace CodeWF.Controls;

public class StatusBadge : TemplatedControl
{
    public static readonly StyledProperty<string?> LeftTextProperty =
        AvaloniaProperty.Register<StatusBadge, string?>(nameof(LeftText));

    public string? LeftText
    {
        get => GetValue(LeftTextProperty);
        set => SetValue(LeftTextProperty, value);
    }

    public static readonly StyledProperty<string?> RightTextProperty =
        AvaloniaProperty.Register<StatusBadge, string?>(nameof(RightText));

    public string? RightText
    {
        get => GetValue(RightTextProperty);
        set => SetValue(RightTextProperty, value);
    }
}
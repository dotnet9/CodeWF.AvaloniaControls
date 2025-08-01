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

    public static readonly StyledProperty<int> FontSizeProperty =
        AvaloniaProperty.Register<StatusLabel, int>(nameof(FontSize), 12);

    public int FontSize
    {
        get => GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly StyledProperty<int> CornerRadiusProperty =
        AvaloniaProperty.Register<StatusLabel, int>(nameof(CornerRadius), 8);

    public int CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly StyledProperty<Thickness> PaddingProperty =
        AvaloniaProperty.Register<StatusLabel, Thickness>(nameof(Padding), new Thickness(4, 1));

    public Thickness Padding
    {
        get => GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public static readonly StyledProperty<StatusLabelKind?> KindProperty =
        AvaloniaProperty.Register<StatusLabel, StatusLabelKind?>(nameof(Kind), StatusLabelKind.Debug);

    public StatusLabelKind? Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }
}
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
        AvaloniaProperty.Register<StatusCard, string?>(nameof(TopText), nameof(TopText));

    public string? TopText
    {
        get => GetValue(TopTextProperty);
        set => SetValue(TopTextProperty, value);
    }

    public static readonly StyledProperty<int> TopTextFontSizeProperty =
        AvaloniaProperty.Register<StatusCard, int>(nameof(TopTextFontSize), 12);

    public int TopTextFontSize
    {
        get => GetValue(TopTextFontSizeProperty);
        set => SetValue(TopTextFontSizeProperty, value);
    }
    public static readonly StyledProperty<Thickness> TopTextMarginProperty =
        AvaloniaProperty.Register<StatusCard, Thickness>(nameof(TopTextMargin), new Thickness(8, 8, 8, 4));

    public Thickness TopTextMargin
    {
        get => GetValue(TopTextMarginProperty);
        set => SetValue(TopTextMarginProperty, value);
    }

    public static readonly StyledProperty<string?> BottomTextProperty =
        AvaloniaProperty.Register<StatusCard, string?>(nameof(BottomText), nameof(BottomText));

    public string? BottomText
    {
        get => GetValue(BottomTextProperty);
        set => SetValue(BottomTextProperty, value);
    }

    public static readonly StyledProperty<int> BottomTextFontSizeProperty =
        AvaloniaProperty.Register<StatusCard, int>(nameof(BottomTextFontSize), 16);

    public int BottomTextFontSize
    {
        get => GetValue(BottomTextFontSizeProperty);
        set => SetValue(BottomTextFontSizeProperty, value);
    }

    public static readonly StyledProperty<Thickness> BottomTextMarginProperty =
        AvaloniaProperty.Register<StatusCard, Thickness>(nameof(BottomTextMargin), new Thickness(8, 4, 8, 8));

    public Thickness BottomTextMargin
    {
        get => GetValue(BottomTextMarginProperty);
        set => SetValue(BottomTextMarginProperty, value);
    }

    public static readonly StyledProperty<StatusLabelKind?> KindProperty =
        AvaloniaProperty.Register<StatusCard, StatusLabelKind?>(nameof(Kind), StatusLabelKind.Normal);

    public StatusLabelKind? Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }
}
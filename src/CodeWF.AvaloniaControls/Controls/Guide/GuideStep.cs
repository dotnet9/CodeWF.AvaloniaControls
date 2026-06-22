using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Controls;

public class GuideStep : ContentControl, IGuideStepOption
{
    public static readonly StyledProperty<Control?> TargetProperty =
        AvaloniaProperty.Register<GuideStep, Control?>(nameof(Target));

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<GuideStep, string?>(nameof(Title));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<GuideStep, string?>(nameof(Description));

    public static readonly StyledProperty<object?> CoverProperty =
        AvaloniaProperty.Register<GuideStep, object?>(nameof(Cover));

    public static readonly StyledProperty<GuidePlacementMode?> PlacementProperty =
        AvaloniaProperty.Register<GuideStep, GuidePlacementMode?>(nameof(Placement));

    public static readonly StyledProperty<GuideStyleType?> StyleTypeProperty =
        AvaloniaProperty.Register<GuideStep, GuideStyleType?>(nameof(StyleType));

    public static readonly StyledProperty<bool?> IsShowMaskProperty =
        AvaloniaProperty.Register<GuideStep, bool?>(nameof(IsShowMask));

    public static readonly StyledProperty<bool?> IsArrowVisibleProperty =
        AvaloniaProperty.Register<GuideStep, bool?>(nameof(IsArrowVisible));

    public static readonly StyledProperty<bool?> IsPointAtCenterProperty =
        AvaloniaProperty.Register<GuideStep, bool?>(nameof(IsPointAtCenter));

    public static readonly StyledProperty<bool?> IsScrollIntoViewProperty =
        AvaloniaProperty.Register<GuideStep, bool?>(nameof(IsScrollIntoView));

    public static readonly StyledProperty<IBrush?> MaskColorProperty =
        AvaloniaProperty.Register<GuideStep, IBrush?>(nameof(MaskColor));

    public static readonly StyledProperty<double?> GapOffsetXProperty =
        AvaloniaProperty.Register<GuideStep, double?>(nameof(GapOffsetX));

    public static readonly StyledProperty<double?> GapOffsetYProperty =
        AvaloniaProperty.Register<GuideStep, double?>(nameof(GapOffsetY));

    public static readonly StyledProperty<double?> GapRadiusProperty =
        AvaloniaProperty.Register<GuideStep, double?>(nameof(GapRadius));

    public static readonly StyledProperty<ICommand?> OpeningCommandProperty =
        AvaloniaProperty.Register<GuideStep, ICommand?>(nameof(OpeningCommand));

    public static readonly StyledProperty<object?> OpeningCommandParameterProperty =
        AvaloniaProperty.Register<GuideStep, object?>(nameof(OpeningCommandParameter));

    public Control? Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public object? Cover
    {
        get => GetValue(CoverProperty);
        set => SetValue(CoverProperty, value);
    }

    public GuidePlacementMode? Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public GuideStyleType? StyleType
    {
        get => GetValue(StyleTypeProperty);
        set => SetValue(StyleTypeProperty, value);
    }

    public bool? IsShowMask
    {
        get => GetValue(IsShowMaskProperty);
        set => SetValue(IsShowMaskProperty, value);
    }

    public bool? IsArrowVisible
    {
        get => GetValue(IsArrowVisibleProperty);
        set => SetValue(IsArrowVisibleProperty, value);
    }

    public bool? IsPointAtCenter
    {
        get => GetValue(IsPointAtCenterProperty);
        set => SetValue(IsPointAtCenterProperty, value);
    }

    public bool? IsScrollIntoView
    {
        get => GetValue(IsScrollIntoViewProperty);
        set => SetValue(IsScrollIntoViewProperty, value);
    }

    public IBrush? MaskColor
    {
        get => GetValue(MaskColorProperty);
        set => SetValue(MaskColorProperty, value);
    }

    public double? GapOffsetX
    {
        get => GetValue(GapOffsetXProperty);
        set => SetValue(GapOffsetXProperty, value);
    }

    public double? GapOffsetY
    {
        get => GetValue(GapOffsetYProperty);
        set => SetValue(GapOffsetYProperty, value);
    }

    public double? GapRadius
    {
        get => GetValue(GapRadiusProperty);
        set => SetValue(GapRadiusProperty, value);
    }

    public ICommand? OpeningCommand
    {
        get => GetValue(OpeningCommandProperty);
        set => SetValue(OpeningCommandProperty, value);
    }

    public object? OpeningCommandParameter
    {
        get => GetValue(OpeningCommandParameterProperty);
        set => SetValue(OpeningCommandParameterProperty, value);
    }
}
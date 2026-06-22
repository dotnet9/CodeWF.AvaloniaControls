using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Controls;

public interface IGuideStepOption
{
    Control? Target { get; set; }

    string? Title { get; set; }

    string? Description { get; set; }

    object? Cover { get; set; }

    GuidePlacementMode? Placement { get; set; }

    GuideStyleType? StyleType { get; set; }

    bool? IsShowMask { get; set; }

    bool? IsArrowVisible { get; set; }

    bool? IsPointAtCenter { get; set; }

    bool? IsScrollIntoView { get; set; }

    IBrush? MaskColor { get; set; }

    double? GapOffsetX { get; set; }

    double? GapOffsetY { get; set; }

    double? GapRadius { get; set; }

    ICommand? OpeningCommand { get; set; }

    object? OpeningCommandParameter { get; set; }
}

public class GuideStepOption : IGuideStepOption
{
    public Control? Target { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public object? Cover { get; set; }

    public GuidePlacementMode? Placement { get; set; }

    public GuideStyleType? StyleType { get; set; }

    public bool? IsShowMask { get; set; }

    public bool? IsArrowVisible { get; set; }

    public bool? IsPointAtCenter { get; set; }

    public bool? IsScrollIntoView { get; set; }

    public IBrush? MaskColor { get; set; }

    public double? GapOffsetX { get; set; }

    public double? GapOffsetY { get; set; }

    public double? GapRadius { get; set; }

    public ICommand? OpeningCommand { get; set; }

    public object? OpeningCommandParameter { get; set; }
}
using System.Collections.Generic;

namespace CodeWF.AvaloniaControls.Models;

public static class StatusLabelKindBrushes
{
    public static Dictionary<StatusLabelKind, string> KindBorderBrushes = new Dictionary<StatusLabelKind, string>()
    {
        { StatusLabelKind.Debug, "#1890FF" },
        { StatusLabelKind.Info, "#52C41A" },
        { StatusLabelKind.Warn, "#FAAD14" },
        { StatusLabelKind.Error, "#FF4D4F" },
        { StatusLabelKind.Fatal, "#FF4D4F" }
    };

    public static Dictionary<StatusLabelKind, string> KindBackgrounds = new Dictionary<StatusLabelKind, string>()
    {
        { StatusLabelKind.Debug, "#E6F7FF" },
        { StatusLabelKind.Info, "#F6FFED" },
        { StatusLabelKind.Warn, "#FFF7E6" },
        { StatusLabelKind.Error, "#FFF1F0" },
        { StatusLabelKind.Fatal, "#19FF4D4F" }
    };

    public static Dictionary<StatusLabelKind, string> KindForCardForegrounds = new Dictionary<StatusLabelKind, string>()
    {
        { StatusLabelKind.Debug, "#262626" },
        { StatusLabelKind.Info, "#262626" },
        { StatusLabelKind.Warn, "#FAAD14" },
        { StatusLabelKind.Error, "#FF4D4F" },
        { StatusLabelKind.Fatal, "#FF4D4F" }
    };
}
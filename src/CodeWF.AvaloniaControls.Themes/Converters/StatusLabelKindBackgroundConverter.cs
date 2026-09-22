using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CodeWF.AvaloniaControls.Models;

namespace CodeWF.AvaloniaControls.Themes.Converters;

public class StatusLabelKindBackgroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var kind = value is StatusLabelKind statusKind ? statusKind : StatusLabelKind.Debug;
        if (!StatusLabelKindBrushes.KindBackgrounds.TryGetValue(kind, out var color))
            color = StatusLabelKindBrushes.KindBackgrounds[StatusLabelKind.Debug];
        return new SolidColorBrush(Color.Parse(color));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return AvaloniaProperty.UnsetValue;
    }
}

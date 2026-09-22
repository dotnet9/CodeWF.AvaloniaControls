using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using CodeWF.AvaloniaControls.Models;

namespace CodeWF.AvaloniaControls.Themes.Converters;

public class StatusCardKindForegroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var kind = value is StatusLabelKind statusKind ? statusKind : StatusLabelKind.Debug;
        if (!StatusLabelKindBrushes.KindForCardForegrounds.TryGetValue(kind, out var color))
            color = StatusLabelKindBrushes.KindForCardForegrounds[StatusLabelKind.Debug];
        return new SolidColorBrush(Color.Parse(color));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return AvaloniaProperty.UnsetValue;
    }
}

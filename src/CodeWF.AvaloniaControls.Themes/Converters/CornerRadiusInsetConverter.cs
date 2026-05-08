using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace CodeWF.AvaloniaControls.Themes.Converters;

public class CornerRadiusInsetConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not CornerRadius radius)
        {
            return new CornerRadius();
        }

        var inset = parameter switch
        {
            double doubleValue => doubleValue,
            string stringValue when double.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) => parsed,
            _ => 0
        };

        return new CornerRadius(
            Math.Max(0, radius.TopLeft - inset),
            Math.Max(0, radius.TopRight - inset),
            Math.Max(0, radius.BottomRight - inset),
            Math.Max(0, radius.BottomLeft - inset));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

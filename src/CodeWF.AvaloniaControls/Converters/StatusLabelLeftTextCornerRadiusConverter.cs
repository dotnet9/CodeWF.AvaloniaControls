using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace CodeWF.AvaloniaControls.Converters;

public class StatusLabelLeftTextCornerRadiusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int radius)
        {
            return new CornerRadius(radius, 0, 0, radius);
        }

        return new CornerRadius(8, 0, 0, 8);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
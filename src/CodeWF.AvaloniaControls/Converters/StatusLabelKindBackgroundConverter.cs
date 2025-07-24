using Avalonia.Data.Converters;
using Avalonia.Media;
using CodeWF.AvaloniaControls.Models;
using System;
using System.Globalization;

namespace CodeWF.AvaloniaControls.Converters;

public class StatusLabelKindBackgroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not StatusLabelKind kind)
        {
            return new SolidColorBrush(Color.Parse("#FAAD14"));
        }

        return kind switch
        {
            StatusLabelKind.Normal => new SolidColorBrush(Color.Parse("#73D13D")),
            StatusLabelKind.Alarm => new SolidColorBrush(Color.Parse("#FAAD14")),
            _ => new SolidColorBrush(Color.Parse("#FA4D4F")),
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
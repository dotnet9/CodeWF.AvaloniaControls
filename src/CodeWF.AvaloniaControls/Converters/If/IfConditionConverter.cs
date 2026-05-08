using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace CodeWF.AvaloniaControls.Converters;

public class IfConditionConverter : IValueConverter
{
    public object? True { get; set; }

    public object? False { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? True : False;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}

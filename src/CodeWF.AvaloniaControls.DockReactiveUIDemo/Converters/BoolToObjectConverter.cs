using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Converters;

// 布尔值转换器
public class BoolToObjectConverter<T> : IValueConverter
{
    public T? TrueValue { get; set; }
    public T? FalseValue { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool boolValue && boolValue ? TrueValue : FalseValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is T && Equals(value, TrueValue);
    }
}
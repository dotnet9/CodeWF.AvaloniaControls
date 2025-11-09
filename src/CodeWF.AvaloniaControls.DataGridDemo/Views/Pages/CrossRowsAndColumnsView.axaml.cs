using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace CodeWF.AvaloniaControls.DataGridDemo.Views.Pages;

public partial class CrossRowsAndColumnsView : UserControl
{
    public CrossRowsAndColumnsView()
    {
        InitializeComponent();
    }
}

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

public class BoolToThicknessConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is string param)
        {
            string[] parts = param.Split('|');
            if (parts.Length == 2)
            {
                string thicknessString = boolValue ? parts[0] : parts[1];
                string[] values = thicknessString.Split(',');
                if (values.Length >= 4 &&
                    double.TryParse(values[0], out double left) &&
                    double.TryParse(values[1], out double top) &&
                    double.TryParse(values[2], out double right) &&
                    double.TryParse(values[3], out double bottom))
                {
                    return new Avalonia.Thickness(left, top, right, bottom);
                }
            }
        }
        return new Avalonia.Thickness(1, 1, 1, 0); // 默认值
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
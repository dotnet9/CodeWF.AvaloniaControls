using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;

namespace CodeWF.AvaloniaControls.Converters;

public class String2ImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
        {
            return new Bitmap(AssetLoader.Open(new Uri(imagePath)));
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return AvaloniaProperty.UnsetValue;
    }
}
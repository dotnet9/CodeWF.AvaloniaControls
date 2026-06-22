using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace CodeWF.AvaloniaControls.Themes.Converters;

public class WindowIconToImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is WindowIcon icon)
        {
            using var stream = new MemoryStream();
            icon.Save(stream);
            stream.Position = 0;
            return new Bitmap(stream);
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return AvaloniaProperty.UnsetValue;
    }
}
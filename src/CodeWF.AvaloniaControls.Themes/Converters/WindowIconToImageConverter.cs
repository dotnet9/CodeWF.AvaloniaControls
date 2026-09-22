using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

using System.Runtime.CompilerServices;

namespace CodeWF.AvaloniaControls.Themes.Converters;

public class WindowIconToImageConverter : IValueConverter
{
    private readonly ConditionalWeakTable<WindowIcon, Bitmap> _bitmapCache = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is WindowIcon icon)
            return _bitmapCache.GetValue(icon, static windowIcon => CreateBitmap(windowIcon));

        return AvaloniaProperty.UnsetValue;
    }

    private static Bitmap CreateBitmap(WindowIcon icon)
    {
        using var stream = new MemoryStream();
        icon.Save(stream);
        stream.Position = 0;
        return new Bitmap(stream);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return AvaloniaProperty.UnsetValue;
    }
}

using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using System.Collections.Concurrent;

namespace CodeWF.AvaloniaControls.Themes.Converters;

public class String2ImageConverter : IValueConverter
{
    private readonly ConcurrentDictionary<string, WeakReference<Bitmap>> _bitmapCache = new(StringComparer.Ordinal);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
        {
            if (_bitmapCache.TryGetValue(imagePath, out var cached) && cached.TryGetTarget(out var bitmap))
                return bitmap;

            using var stream = AssetLoader.Open(new Uri(imagePath));
            var loadedBitmap = new Bitmap(stream);
            _bitmapCache[imagePath] = new WeakReference<Bitmap>(loadedBitmap);
            return loadedBitmap;
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return AvaloniaProperty.UnsetValue;
    }
}

using Avalonia;
using Avalonia.Data.Converters;
using System.Globalization;

namespace CodeWF.Themes.Converters;

public class StatusBadgeLeftTextBorderCornerRadiusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CornerRadius cornerRadius)
        {
            return new CornerRadius(cornerRadius.TopLeft, 0, 0, cornerRadius.BottomLeft);
        }

        return new CornerRadius(8, 0, 0, 8);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
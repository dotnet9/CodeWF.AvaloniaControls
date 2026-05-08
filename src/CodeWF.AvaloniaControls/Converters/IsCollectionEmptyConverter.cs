using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace CodeWF.AvaloniaControls.Converters;

public class IsCollectionEmptyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            null => true,
            ICollection collection => collection.Count == 0,
            IEnumerable enumerable => !enumerable.Cast<object?>().Any(),
            _ => false
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}

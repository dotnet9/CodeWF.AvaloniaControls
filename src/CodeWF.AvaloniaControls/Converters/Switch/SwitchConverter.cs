using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using CodeWF.AvaloniaControls.Markup;

namespace CodeWF.AvaloniaControls.Converters;

public class SwitchConverter(SwitchBinding switchBinding) : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return switchBinding.Evaluate(values, culture);
    }
}

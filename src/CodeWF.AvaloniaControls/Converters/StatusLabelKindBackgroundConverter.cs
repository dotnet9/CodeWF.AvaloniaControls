﻿using Avalonia.Data.Converters;
using Avalonia.Media;
using CodeWF.AvaloniaControls.Models;
using System;
using System.Globalization;
using System.Linq;

namespace CodeWF.AvaloniaControls.Converters;

public class StatusLabelKindBackgroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var color = value is not StatusLabelKind kind
            ? StatusLabelKindBrushes.KindBackgrounds.First().Value
            : StatusLabelKindBrushes.KindBackgrounds[kind];
        return new SolidColorBrush(Color.Parse(color));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
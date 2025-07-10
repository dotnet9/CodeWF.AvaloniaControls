using Avalonia;
using Avalonia.Data.Converters;
using Dock.Avalonia.Controls;
using System;
using System.Globalization;

namespace CodeWF.AvaloniaControls.Dock.Converters;

public class TrapezoidShapedDocumentTabStripItemPaddingConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DocumentTabStripItem { Parent: DocumentTabStrip tabControl } tabItem)
        {
            return new Thickness(0);
        }


        var padding = tabItem.Padding;
        return new Thickness(padding.Left * 2, padding.Top, padding.Right * 2, padding.Bottom);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Avalonia.Controls;
using CodeWF.AvaloniaControls.Controls.TabControls;

namespace CodeWF.AvaloniaControls.Converters;

public class TrapezoidShapedTabItemPaddingConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TrapezoidShapedTabItem { Parent: TabControl tabControl } tabItem)
        {
            return new Thickness(0);
        }
        

        var padding = tabItem.Padding;
        if (tabControl.TabStripPlacement is Dock.Top or Dock.Bottom)
        {
            return new Thickness(padding.Left * 2, padding.Top, padding.Right * 2, padding.Bottom);
        }
        else
        {
            return new Thickness(padding.Left, padding.Top * 2, padding.Right, padding.Bottom * 2);
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
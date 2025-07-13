using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Converters;
public class VisibilityToForegroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isVisible = (bool)value;
        return isVisible ? Brushes.Black : Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
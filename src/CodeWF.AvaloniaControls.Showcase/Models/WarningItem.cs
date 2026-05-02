using Avalonia.Media;

namespace CodeWF.AvaloniaControls.Showcase.Models;

public class WarningItem
{
    public string Name { get; set; } = string.Empty;
    public SolidColorBrush Color { get; set; } = new(Colors.Transparent);
}

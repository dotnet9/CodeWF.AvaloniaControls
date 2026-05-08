using Avalonia.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CodeWF.AvaloniaControls.Showcase.Models;

public class WarningItem : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private SolidColorBrush _color = new(Colors.Transparent);

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public SolidColorBrush Color
    {
        get => _color;
        set => SetField(ref _color, value);
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

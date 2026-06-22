using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media;

namespace CodeWF.AvaloniaControlsDemo.Models;

public class WarningItem : INotifyPropertyChanged
{
    private SolidColorBrush _color = new(Colors.Transparent);
    private string _name = string.Empty;

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

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value)) return;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
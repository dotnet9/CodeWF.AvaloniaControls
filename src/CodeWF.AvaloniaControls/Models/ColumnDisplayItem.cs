using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CodeWF.AvaloniaControls.Models;

public class ColumnDisplayItem : INotifyPropertyChanged
{
    private bool _visible = true;

    public ColumnDisplayItem()
    {
    }

    public ColumnDisplayItem(string key, string displayText, bool visible = true)
    {
        Key = key;
        DisplayText = displayText;
        Visible = visible;
    }

    public string Key { get; set; } = string.Empty;

    public string DisplayText { get; set; } = string.Empty;

    public bool Visible
    {
        get => _visible;
        set
        {
            if (_visible == value)
            {
                return;
            }

            _visible = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

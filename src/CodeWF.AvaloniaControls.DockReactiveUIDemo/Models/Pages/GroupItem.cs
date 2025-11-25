using System.Collections.ObjectModel;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Models.Pages;

public class GroupItem
{
    public int Id { get; set; }
    public bool IsKeyGroup { get; set; }
    public string? Cycle { get; set; }
    public ObservableCollection<ProcessItem> Items { get; set; } = new();
}
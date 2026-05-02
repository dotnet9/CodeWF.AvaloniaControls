using CodeWF.AvaloniaControls;
using CodeWF.AvaloniaControls.ProDataGridShowcase.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace CodeWF.AvaloniaControls.ProDataGridShowcase.ViewModels.Pages;

public class SortingViewModel : ReactiveObject
{
    public SortingViewModel()
    {
        var id = 1;
        for (var i = 0; i < 100; i++)
        {
            Students.Add(new Student(id++, "小明", "北京"));
            Students.Add(new Student(id++, "李华", "天津"));
            Students.Add(new Student(id++, "王五", "上海"));
        }
    }

    public ObservableCollection<Student> Students { get; } = new();

    public void RaiseAddSortingHandler(Avalonia.Controls.DataGrid dataGrid)
    {
        dataGrid.AddSorting();
    }
}

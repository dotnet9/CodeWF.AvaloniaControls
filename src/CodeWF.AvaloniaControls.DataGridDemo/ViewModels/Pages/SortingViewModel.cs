using CodeWF.AvaloniaControls.DataGridDemo.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace CodeWF.AvaloniaControls.DataGridDemo.ViewModels.Pages;

public class SortingViewModel : ReactiveObject
{
    public SortingViewModel()
    {
        var id = 1;
        for (var i = 0; i < 100; i++)
        {
            Students.Add(new Student(id++, "Xiao Ming", "Bei Jing"));
            Students.Add(new Student(id++, "Li Hua", "Tian Jing"));
            Students.Add(new Student(id++, "Wang Wu", "Shang Hai"));
        }
    }

    public ObservableCollection<Student> Students { get; } = new();

    public void RaiseAddSortingHandler(Avalonia.Controls.DataGrid dataGrid)
    {
        dataGrid.AddSorting();
    }
}
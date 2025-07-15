using CodeWF.AvaloniaControls.DataGridDemo.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using CodeWF.AvaloniaControls.DataGrid;

namespace CodeWF.AvaloniaControls.DataGridDemo.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    public MainWindowViewModel()
    {
        Students.Add(new Student(1, "Xiao Ming", "Bei Jing"));
        Students.Add(new Student(2, "Li Hua", "Tian Jing"));
        Students.Add(new Student(3, "Wang Wu", "Shang Hai"));
    }

    public ObservableCollection<Student> Students { get; } = new();

    public void RaiseAddSortingHandler(Avalonia.Controls.DataGrid dataGrid)
    {
        dataGrid.AddSorting();
    }
}
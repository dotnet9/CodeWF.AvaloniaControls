using System.Collections.Generic;
using System.Collections.ObjectModel;
using TestDataGridDemo.Models;

namespace TestDataGridDemo.ViewModels;

public class DataGridDemoViewModel : ViewModelBase
{
    public ObservableCollection<Person> People { get; }

    public DataGridDemoViewModel()
    {
        var people = new List<Person>
        {
            new Person("Neil", "Armstrong"),
            new Person("Buzz", "Lightyear"),
            new Person("James", "Kirk")
        };
        People = new ObservableCollection<Person>(people);
    }
}
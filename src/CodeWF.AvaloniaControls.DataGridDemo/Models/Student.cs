using ReactiveUI;

namespace CodeWF.AvaloniaControls.DataGridDemo.Models;

public class Student : ReactiveObject
{
    public Student(int id, string name, string address)
    {
        Id = id;
        Name = name;
        Address = address;
    }

    private int _id;

    public int Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    private string _name;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _address;

    public string Address
    {
        get => _address;
        set => this.RaiseAndSetIfChanged(ref _address, value);
    }
}
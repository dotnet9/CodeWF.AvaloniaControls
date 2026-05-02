using ReactiveUI;

namespace CodeWF.AvaloniaControls.ProDataGridShowcase.Models;

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

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _address = string.Empty;

    public string Address
    {
        get => _address;
        set => this.RaiseAndSetIfChanged(ref _address, value);
    }
}

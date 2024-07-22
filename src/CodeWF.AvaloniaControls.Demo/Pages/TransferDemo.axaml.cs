using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.Controls;
using CodeWF.AvaloniaControls.Extensions;

namespace CodeWF.AvaloniaControls.Demo.Pages;

public partial class TransferDemo : UserControl, INotifyPropertyChanged
{
    private string? _selectedInfo;
    private Transfer _transfer;

    public TransferDemo()
    {
        InitializeComponent();
    }

    public RangeObservableCollection<string> LeftItems { get; set; } = ["codewf.com", "dotnet9.com", "dotnetools.com", "dotnet.chat", "Ding", "Otter",];

    public RangeObservableCollection<string> RightItems { get; set; } = ["Husky", "Mr.17", "Cass"];

    public string? SelectedInfo
    {
        get => _selectedInfo;
        set
        {
            if (_selectedInfo == value) return;
            _selectedInfo = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedInfo)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _transfer = this.FindControl<Transfer>("MyTransfer")!;
    }

    private void ShowSelectedInfo_OnClick(object? sender, RoutedEventArgs e)
    {
        SelectedInfo = RightItems == null ? "нч" : string.Join(',', RightItems);
    }
}
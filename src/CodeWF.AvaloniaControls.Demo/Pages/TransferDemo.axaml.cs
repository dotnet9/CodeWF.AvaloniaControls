using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.Extensions;
using System.ComponentModel;

namespace CodeWF.AvaloniaControls.Demo.Pages;

public partial class TransferDemo : UserControl, INotifyPropertyChanged
{
    private string? _selectedInfo;

    public TransferDemo()
    {
        InitializeComponent();
        RightItems.CollectionChanged += (s, e) =>
        {
            UpdateSelectedCount();
        };
    }

    public RangeObservableCollection<string> LeftItems { get; set; } = ["codewf.com", "dotnet9.com", "dotnetools.com", "dotnet.chat", "Ding", "Otter",
    "到的是 Avalonia DataGrid 一个典型的","若内容高度超过可视区域","导致列宽之和的判定逻辑出","或通过自定义样式","rid 的布局容器预留垂直滚动条的",
        "制 DataGrid 的最小宽度覆盖列","宽度没有被纳入列宽的计算","致列宽之和的判定逻辑出错","Grid 外层套ScrollViewer"];

    public RangeObservableCollection<string> RightItems { get; set; } = ["Husky", "Mr.17", "Cass", "自定义 DataGrid 样式（更优", "想套外层ScrollViewer",
        "可以通过修改 DataGrid 的默认样式", "强制其布局时预留垂直滚动条的宽度", "确保水平滚动条正确计算", "DataGrid 会始终预留垂直滚动条的宽度", "无论数据行数多少，水平滚动条都会根据列宽"];

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

    public new event PropertyChangedEventHandler? PropertyChanged;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        UpdateSelectedCount();
    }

    private void UpdateSelectedCount()
    {
        SelectedInfo = RightItems.Count <= 0 ? "无" : string.Join(',', RightItems);
    }
}
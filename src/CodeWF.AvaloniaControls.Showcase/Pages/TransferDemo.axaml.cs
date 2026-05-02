using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.Extensions;
using System.ComponentModel;

namespace CodeWF.AvaloniaControls.Showcase.Pages;

public partial class TransferDemo : UserControl, INotifyPropertyChanged
{
    private string? _selectedInfo;

    public TransferDemo()
    {
        InitializeComponent();
        RightItems.CollectionChanged += (_, _) => UpdateSelectedCount();
    }

    // 这里使用中文示例数据，便于直接观察穿梭框在真实业务文案下的显示效果。
    public RangeObservableCollection<string> LeftItems { get; set; } =
    [
        "codewf.com",
        "dotnet9.com",
        "dotnetools.com",
        "dotnet.chat",
        "Ding",
        "Otter",
        "兼容 Avalonia DataGrid 最后一个免费版本",
        "补充高密度数据场景下的交互演示",
        "支持开源仓库中的中文界面说明",
        "统一普通示例的视觉样式",
        "按运行平台输出独立发布产物",
        "优化 DataGrid 三态排序扩展",
        "修复斜角页签边框绘制细节",
        "增强窗口外壳示例的可读性",
        "Grid 通过内置 ScrollViewer 承载滚动内容"
    ];

    public RangeObservableCollection<string> RightItems { get; set; } =
    [
        "Husky",
        "Mr.17",
        "Cass",
        "展示 Semi 主题下的穿梭框样式",
        "保留内置 ScrollViewer 行为",
        "统一普通示例的默认视觉风格",
        "强化发布脚本对多项目的支持",
        "确保水平滚动内容能够正确显示",
        "DataGrid 示例提供大数据切换压力场景",
        "整理窗口标题与按钮前景配色"
    ];

    public string? SelectedInfo
    {
        get => _selectedInfo;
        set
        {
            if (_selectedInfo == value)
            {
                return;
            }

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
        SelectedInfo = RightItems.Count <= 0 ? "无" : string.Join("、", RightItems);
    }
}

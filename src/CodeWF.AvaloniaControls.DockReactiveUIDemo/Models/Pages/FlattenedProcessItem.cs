namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.Models.Pages;

// 扁平化的数据项模型，用于优化渲染性能
// 不再继承ReactiveObject，因为我们不需要属性变更通知（所有属性都是初始化后不变的）
public class FlattenedProcessItem
{
    // 分组相关属性
    public int GroupId { get; init; }
    public bool IsKeyGroup { get; init; }
    public string? Cycle { get; init; }
    public bool IsFirstInGroup { get; init; }
    public bool IsLastInGroup { get; init; }
    public int GroupItemCount { get; init; }
    // 新增：用于确定是否在组的中间行显示组信息
    public bool IsMiddleOfGroup { get; init; }
    // 新增：用于设置行边框（第一行添加上边框，最后一行添加下边框）
    public Avalonia.Thickness RowBorderThickness { get; init; }

    // ProcessItem属性
    public int Id { get; init; }
    public string? Name { get; init; }
    public bool Enabled { get; set; } // 只有Enabled需要可变，因为它是CheckBox绑定的
    public int SourceNode { get; init; }
    public string? Host { get; init; }
    public string? ProgramPath { get; init; }
    public string? WorkPath { get; init; }
    public string? Params { get; init; }
    public bool AutoStart { get; init; }
    public string? PreProcess { get; init; }
    public string? PostProcess { get; init; }
    public string? Description { get; init; }
}
using CodeWF.AvaloniaControls.DataGridDemo.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeWF.AvaloniaControls.DataGridDemo.ViewModels.Pages;

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

public class CrossRowsAndColumnsViewModel : ReactiveObject
{
    public CrossRowsAndColumnsViewModel()
    {
        // 性能优化1：使用List<T>进行中间计算，然后一次性添加到ObservableCollection
        // ObservableCollection的添加操作会触发UI更新，避免频繁触发
        var allGroups = new List<GroupItem>();
        
        // 先生成所有组数据
        for (var i = 0; i < Random.Shared.Next(10, 30); i++)
        {
            var group = new GroupItem
            {
                Id = i+1,
                IsKeyGroup = i % Random.Shared.Next(1,4) == 0,
                Cycle = $"{Random.Shared.Next(2, 5)}/{Random.Shared.Next(3, 6)}"
            };
            allGroups.Add(group);

            // 预分配内部列表容量，减少动态扩容开销
            var processItems = new List<ProcessItem>(Random.Shared.Next(1, 10));
            
            for (var j = 0; j < processItems.Capacity; j++)
            {
                // 使用对象初始值设定项一次性初始化对象，避免多次属性赋值
                processItems.Add(new ProcessItem
                {
                    Id = j,
                    Name = $"Process {j}",
                    Enabled = j % Random.Shared.Next(3, 8) == 0,
                    SourceNode = j % Random.Shared.Next(3, 8),
                    Host = "127.0.0.1:89333",
                    ProgramPath = "../../test/bb.exe",
                    WorkPath = "../../test",
                    Params = j % Random.Shared.Next(3, 8) == 0 ? "---" : "-type 1",
                    AutoStart = j % Random.Shared.Next(3, 8) == 0,
                    PreProcess = j % Random.Shared.Next(3, 8) == 0 ? "---" : "make dir",
                    PostProcess = j % Random.Shared.Next(3, 8) == 0 ? "---" : "remove file",
                    Description = j % Random.Shared.Next(3, 8) == 0 ? "---" : "用于测试 ",
                });
            }
            
            // 一次性添加所有process items
            foreach (var item in processItems)
            {
                group.Items.Add(item);
            }
        }
        
        // 为兼容旧代码保留GroupItems
        GroupItems.AddRange(allGroups);
        
        // 性能优化2：预计算扁平化列表大小，减少动态扩容
        var totalItemsCount = allGroups.Sum(g => g.Items.Count);
        var flattenedItemsList = new List<FlattenedProcessItem>(totalItemsCount);
        
        // 创建扁平化的数据项
        foreach (var group in allGroups)
        {
            var itemCount = group.Items.Count;
            for (var i = 0; i < itemCount; i++)
            {
                var processItem = group.Items[i];
                flattenedItemsList.Add(new FlattenedProcessItem
                {
                    GroupId = group.Id,
                    IsKeyGroup = group.IsKeyGroup,
                    Cycle = group.Cycle,
                    IsFirstInGroup = i == 0,
                    IsLastInGroup = i == itemCount - 1,
                    GroupItemCount = itemCount,
                    // 计算是否为组的中间行：5行显示在第3行，4行显示在第2行
                    IsMiddleOfGroup = i == (itemCount / 2),
                    // 设置边框：第一行添加上边框，最后一行添加下边框
                    RowBorderThickness = new Avalonia.Thickness(
                        left: 0,
                        top: i == 0 ? 1 : 0,
                        right: 0,
                        bottom: i == itemCount - 1 ? 1 : 0
                    ),
                    Id = processItem.Id,
                    Name = processItem.Name,
                    Enabled = processItem.Enabled,
                    SourceNode = processItem.SourceNode,
                    Host = processItem.Host,
                    ProgramPath = processItem.ProgramPath,
                    WorkPath = processItem.WorkPath,
                    Params = processItem.Params,
                    AutoStart = processItem.AutoStart,
                    PreProcess = processItem.PreProcess,
                    PostProcess = processItem.PostProcess,
                    Description = processItem.Description
                });
            }
        }
        
        // 一次性添加所有扁平化项到ObservableCollection，只触发一次UI更新
        foreach (var item in flattenedItemsList)
        {
            FlattenedItems.Add(item);
        }
    }

    // 为兼容旧代码保留
    public ObservableCollection<GroupItem> GroupItems { get; } = [];
    
    // 新的扁平化数据集合，用于优化渲染
    public ObservableCollection<FlattenedProcessItem> FlattenedItems { get; } = [];
}
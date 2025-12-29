using CodeWF.AvaloniaControls.DockReactiveUIDemo.Models.Pages;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Pages;

public class DataGridDemoViewModel : ReactiveObject
{
    public DataGridDemoViewModel()
    {
        var datas = new List<ProcessItem>();

        // 预分配内部列表容量，减少动态扩容开销
        var count = Random.Shared.Next(15, 200);

        for (var j = 0; j < count; j++)
        {
            // 使用对象初始值设定项一次性初始化对象，避免多次属性赋值
            datas.Add(new ProcessItem
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

        Items.AddRange(datas);
        Count = Items.Count;
    }

    public ObservableCollection<ProcessItem> Items { get; } = [];

    public int Count { get; private set; }
}
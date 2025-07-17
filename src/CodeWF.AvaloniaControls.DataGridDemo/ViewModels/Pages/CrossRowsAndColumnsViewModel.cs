using CodeWF.AvaloniaControls.DataGridDemo.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace CodeWF.AvaloniaControls.DataGridDemo.ViewModels.Pages;

public class CrossRowsAndColumnsViewModel : ReactiveObject
{
    public CrossRowsAndColumnsViewModel()
    {
        for (var i = 0; i < Random.Shared.Next(10, 30); i++)
        {
            var group = new GroupItem()
            {
                Id = i+1,
                IsKeyGroup = i % Random.Shared.Next(1,4) == 0,
                Cycle = $"{Random.Shared.Next(2, 5)}/{Random.Shared.Next(3, 6)}"
            };
            GroupItems.Add(group);

            for (var j = 0; j < Random.Shared.Next(1, 10); j++)
            {
                group.Items.Add(new ProcessItem()
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
        }
    }

    public ObservableCollection<GroupItem> GroupItems { get; } = [];
}
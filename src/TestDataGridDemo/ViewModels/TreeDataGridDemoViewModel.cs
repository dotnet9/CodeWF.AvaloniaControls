using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using TestDataGridDemo.Models;
using Avalonia.Controls.Models.TreeDataGrid;

namespace TestDataGridDemo.ViewModels;

public class TreeDataGridDemoViewModel : ViewModelBase
{
    public TreeDataGridDemoViewModel()
    {
        var count = short.MaxValue / 2;

        for (var j = 0; j < count; j++)
        {
            Items.Add(new ProcessItem
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
        Count = Items.Count;
    }

    public ObservableCollection<ProcessItem> Items { get; } = [];

    public int Count { get; private set; }
    public FlatTreeDataGridSource<ProcessItem> ItemsSource
    {
        get
        {
            return field ??= new FlatTreeDataGridSource<ProcessItem>(Items)
            {
                Columns =
                {
                    new TextColumn<ProcessItem, int>("Id", x => x.Id),
                    new TextColumn<ProcessItem, string>("Name", x => x.Name),
                    new TextColumn<ProcessItem, bool>("Enabled", x => x.Enabled),
                    new TextColumn<ProcessItem, int>("SourceNode", x => x.SourceNode),
                    new TextColumn<ProcessItem, string>("Host", x => x.Host),
                    new TextColumn<ProcessItem, string>("ProgramPath", x => x.ProgramPath),
                    new TextColumn<ProcessItem, string>("WorkPath", x => x.WorkPath),
                    new TextColumn<ProcessItem, string>("Params", x => x.Params),
                    new TextColumn<ProcessItem, bool>("AutoStart", x => x.AutoStart),
                    new TextColumn<ProcessItem, string>("PreProcess", x => x.PreProcess),
                    new TextColumn<ProcessItem, string>("PostProcess", x => x.PostProcess),
                    new TextColumn<ProcessItem, string>("Description", x => x.Description),
                }
            };
        }
    }
}
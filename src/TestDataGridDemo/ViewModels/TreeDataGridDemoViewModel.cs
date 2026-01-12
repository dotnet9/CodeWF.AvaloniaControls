using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Media.TextFormatting;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TestDataGridDemo.Models;

namespace TestDataGridDemo.ViewModels;

public class TreeDataGridDemoViewModel : ViewModelBase
{
    public TreeDataGridDemoViewModel()
    {
        AddData();

        ItemsSource.RowSelection.SingleSelect = false;
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
                    new TextColumn<ProcessItem, int>(new IdHeader("ID"), x => x.Id),
                    new TextColumn<ProcessItem, string>(new NameHeader("名称"), x => x.Name),
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

    /// <summary>
    /// 选择内容
    /// </summary>
    public string? SelectedInfo
    { 
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private void AddData()
    {
        Task.Run(async () => 
        {
            while(true)
            {
                Dispatcher.UIThread.Post(() => {
                    var j = DateTime.Now.Millisecond;
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
                });
                await Task.Delay(1000);
            }
        });
    }

    public async Task RaiseGetSelectedItemsHandlerAsync()
    {
        if (ItemsSource.RowSelection?.SelectedItems.Any() == true)
        {
            SelectedInfo = string.Join(",", ItemsSource.RowSelection.SelectedItems.Select(p => p.Id));
        }
        else
        {
            SelectedInfo = "-";
        }
    }
}
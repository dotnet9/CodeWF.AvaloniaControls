using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TestDataGridDemo.Models;

namespace TestDataGridDemo.ViewModels;

public class TreeDataGridDynamicDemoViewModel : ViewModelBase
{
    private List<string> DynamicColumnNames { get; } = new();

    public TreeDataGridDynamicDemoViewModel()
    {
        CreateDynamicColumns();
        CreateDatas();
        UpdateDatas();
        ItemsSource?.RowSelection?.SingleSelect = false;
    }

    public ObservableCollection<DynamicItem> Items { get; } = [];

    public FlatTreeDataGridSource<DynamicItem>? ItemsSource
    {
        get
        {
            return field ??= new FlatTreeDataGridSource<DynamicItem>(Items)
            {
                Columns =
                {
                    new TextColumn<DynamicItem, int>("ID", x => x.Id),
                    new TextColumn<DynamicItem, string>("名称", x => x.Name),
                    new TextColumn<DynamicItem, string>("创建时间", x => $"{x.CreatedAt:yyyy-MM-dd HH:mm:ss}"),
                    new TextColumn<DynamicItem, string>("更新时间", x => $"{x.UpdatedAt:yyyy-MM-dd HH:mm:ss}"),
                }
            };
        }
    }

    private void CreateDynamicColumns()
    {
        for (var i = 0; i < Random.Shared.Next(2, 10); i++)
        {
            var columnName = $"Dynamic{i}";
            var displayName = $"动态列 {i}";
            DynamicColumnNames.Add(columnName);
            ItemsSource?.Columns.Add(
                new TextColumn<DynamicItem, double>(displayName,
                    item => item.DynamicColumns[columnName].Value)
            );
        }
    }

    private void CreateDatas()
    {
        for (var i = 0; i < Random.Shared.Next(5, 20); i++)
        {
            var rowData = new DynamicItem()
            {
                Id = i,
                Name = $"Item {i}",
                CreatedAt = DateTime.Now.AddSeconds(Random.Shared.Next(2, 200000))
            };
            for (var j = 0; j < DynamicColumnNames.Count; j++)
            {
                rowData.DynamicColumns[DynamicColumnNames[j]] = new DynamicColumnInfo()
                {
                    Name = DynamicColumnNames[j],
                    DisplayName = $"动态列 {j}",
                    Value = Random.Shared.NextDouble()
                };
                rowData.UpdatedAt = DateTime.Now;
            }

            Items.Add(rowData);
        }
    }

    private void UpdateDatas()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    foreach (var rowData in Items)
                    {

                        rowData.UpdatedAt = DateTime.Now;
                        // 1、添加或更新
                        foreach (var dynamicColumnName in DynamicColumnNames)
                        {
                            var value = Random.Shared.NextDouble();
                            if (rowData.DynamicColumns.ContainsKey(dynamicColumnName))
                            {
                                rowData.DynamicColumns[dynamicColumnName].Value = value;
                            }
                            else
                            {
                                rowData.DynamicColumns[dynamicColumnName] = new DynamicColumnInfo()
                                {
                                    Name = dynamicColumnName,
                                    Value = value
                                };
                            }
                        }
                    }
                });

                await Task.Delay(200);
            }
        });
    }
}
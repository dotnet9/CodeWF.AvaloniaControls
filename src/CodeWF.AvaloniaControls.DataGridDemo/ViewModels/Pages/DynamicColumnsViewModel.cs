using Avalonia.Controls;
using Avalonia.Data.Core;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using CodeWF.AvaloniaControls.DataGridDemo.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;

namespace CodeWF.AvaloniaControls.DataGridDemo.ViewModels.Pages;

public class DynamicColumnsViewModel : ReactiveObject, IDisposable
{
    private bool _isFirstLoadDataGrid = true;
    private Avalonia.Controls.DataGrid? _myDataGrid;
    private IDisposable? _updateTimerDisposable;

    public DynamicColumnsViewModel()
    {
        for (var i = 0; i < Random.Shared.Next(10, 30); i++)
        {
            var item = new DynamicItem()
            {
                Name = $"name{i}",
                Values = new()
            };
            DynamicItems.Add(item);

            for (var j = 0; j < 10; j++)
            {
                item.Values[$"p{j}"] = $"value{i}-{j}";
            }
        }

        _updateTimerDisposable = Observable.Interval(TimeSpan.FromSeconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => UpdateDynamicItemValues());
    }

    private void UpdateDynamicItemValues()
    {
        foreach (var item in DynamicItems)
        {
            foreach (var key in item.Values!.Keys.ToList())
            {
                if (int.TryParse(item.Values[key], out int currentValue))
                {
                    item.Values[key] = (currentValue + Random.Shared.Next(-5, 10)).ToString();
                }
                else
                {
                    item.Values[key] = $"value{Random.Shared.Next(1000, 9999)}";
                }
            }

            // 通知UI Values属性已更改
            item.RaisePropertyChanged(nameof(DynamicItem.Values));
        }
    }

    public void Dispose()
    {
        _updateTimerDisposable?.Dispose();
    }

    public ObservableCollection<DynamicItem> DynamicItems { get; } = [];

    public void RaiseDataGridLoadHandler(Avalonia.Controls.DataGrid dataGrid)
    {
        _myDataGrid = dataGrid;
        InitDynamicDataGrid();
    }

    private void InitDynamicDataGrid()
    {
        if (_myDataGrid == null || !_isFirstLoadDataGrid || !DynamicItems.Any() ||
            DynamicItems.First().Values?.Any() != true)
        {
            return;
        }

        _isFirstLoadDataGrid = true;

        var dynamicColumnNames = DynamicItems.First().Values!.Keys;
        var dynamicColumns = dynamicColumnNames.Select(columnName =>
        {
            var column = new DataGridTemplateColumn();
            column.IsReadOnly = false;
            column.HeaderTemplate = new FuncDataTemplate<DynamicItem>((_, _) => new TextBlock()
            {
                Classes = { "Header" },
                Text = columnName
            });
            column.CellTemplate = new FuncDataTemplate<DynamicItem>((_, _) => new TextBlock()
            {
                Classes = { "Content" },
                [!TextBlock.TextProperty] = new CompiledBindingExtension(new CompiledBindingPathBuilder()
                    .Property(new ClrPropertyInfo(nameof(DynamicItem.Values),
                            obj =>
                            {
                                ((DynamicItem)obj).Values!.TryGetValue(columnName, out var value);
                                return value;
                            },
                            (obj, value) =>
                            {
                                if (value is string newValue)
                                {
                                    var item = (DynamicItem)obj;
                                    item.Values[columnName] = newValue;
                                    item.RaisePropertyChanged(nameof(DynamicItem.Values));
                                }
                            },
                            typeof(string)),
                        PropertyInfoAccessorFactory.CreateInpcPropertyAccessor)
                    .Build())
            });

            return column;
        });
        foreach (var column in dynamicColumns)
        {
            _myDataGrid.Columns.Add(column);
        }
    }
}
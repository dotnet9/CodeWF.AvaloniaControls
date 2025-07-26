using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data.Core;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using CodeWF.AvaloniaControls.DataGridDemo.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

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
            var item = new DynamicGroup()
            {
                Name = $"name{i}",
                Items = new()
            };
            DynamicGroups.Add(item);

            for (var j = 0; j < 10; j++)
            {
                item.Items.Add(new DynamicItem()
                {
                    Key = $"p{j}",
                    Name = $"p{j}",
                    Value = $"value{i}-{j}"
                });
            }
        }

        _updateTimerDisposable = Observable.Interval(TimeSpan.FromSeconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => UpdateDynamicItemValues());
    }

    private void UpdateDynamicItemValues()
    {
        foreach (var group in DynamicGroups)
        {
            foreach (var item in group.Items!)
            {
                item.Value = $"value{Random.Shared.Next(1000, 9999)}";
            }

            // 通知UI Values属性已更改
            group.RaisePropertyChanged(nameof(DynamicGroup.Items));
        }
    }

    public void Dispose()
    {
        _updateTimerDisposable?.Dispose();
    }

    public ObservableCollection<DynamicGroup> DynamicGroups { get; } = [];

    public void RaiseDataGridLoadHandler(Avalonia.Controls.DataGrid dataGrid)
    {
        _myDataGrid = dataGrid;
        InitDynamicDataGrid();
    }

    private void InitDynamicDataGrid()
    {
        if (_myDataGrid == null || !_isFirstLoadDataGrid || !DynamicGroups.Any() ||
            DynamicGroups.First().Items?.Any() != true)
        {
            return;
        }

        _isFirstLoadDataGrid = true;

        var dynamicColumns = DynamicGroups.First().Items!.Select(item =>
        {
            var column = new DataGridTemplateColumn();
            column.IsReadOnly = false;
            column.HeaderTemplate = new FuncDataTemplate<DynamicGroup>((_, _) => new TextBlock()
            {
                Classes = { "Header" },
                Text = item.Name
            });
            column.CellTemplate = new FuncDataTemplate<DynamicGroup>((_, _) => new TextBlock()
            {
                Classes = { "Content" },
                [!TextBlock.TextProperty] = new CompiledBindingExtension(new CompiledBindingPathBuilder()
                    .Property(new ClrPropertyInfo(nameof(DynamicGroup.Items),
                            obj =>
                            {
                                var currentGroup = (DynamicGroup)obj;
                                var currentItem = currentGroup.Items!.FirstOrDefault(i => i.Key == item.Key);
                                return currentItem.Value;
                            },
                            (obj, value) =>
                            {
                                if (value is string newValue)
                                {
                                    var currentGroup = (DynamicGroup)obj;
                                    var currentItem = currentGroup.Items!.FirstOrDefault(i => i.Key == item.Key);
                                    currentItem.Value = newValue;
                                    item.RaisePropertyChanged(nameof(DynamicGroup.Items));
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
using Avalonia.Controls;
using Avalonia.Data.Core;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using CodeWF.AvaloniaControls.DataGridDemo.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeWF.AvaloniaControls.DataGridDemo.ViewModels.Pages;

public class DynamicColumnsViewModel : ReactiveObject
{
    private bool _isFirstLoadDataGrid = true;
    private Avalonia.Controls.DataGrid? _myDataGrid;

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
        var dynamicColumns = dynamicColumnNames.Select(columnName => new DataGridTextColumn()
        {
            Header = columnName,
            Binding = new CompiledBindingExtension(new CompiledBindingPathBuilder()
                .Property(new ClrPropertyInfo(columnName,
                        obj =>
                        {
                            ((DynamicItem)obj).Values!.TryGetValue(columnName, out var value);
                            return value;
                        },
                        (obj, value) =>
                        {
                            if (value is string newValue)
                            {
                                ((DynamicItem)obj).Values[columnName] = newValue;
                            }
                        },
                        typeof(string)),
                    PropertyInfoAccessorFactory.CreateInpcPropertyAccessor)
                .Build()),
            IsReadOnly = false
        });
        foreach (var column in dynamicColumns)
        {
            _myDataGrid.Columns.Add(column);
        }
    }
}
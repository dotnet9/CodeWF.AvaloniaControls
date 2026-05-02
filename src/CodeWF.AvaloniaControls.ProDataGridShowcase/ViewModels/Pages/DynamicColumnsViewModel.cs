using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Threading;
using CodeWF.AvaloniaControls.ProDataGridShowcase.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;

namespace CodeWF.AvaloniaControls.ProDataGridShowcase.ViewModels.Pages;

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
                Name = $"分组 {i + 1}",
                Items = new()
            };
            DynamicGroups.Add(item);

            for (var j = 0; j < 10; j++)
            {
                item.Items.Add(new DynamicItem()
                {
                    Key = $"p{j}",
                    Name = $"指标 {j + 1}",
                    Value = $"数值 {i + 1}-{j + 1}"
                });
            }
        }

        _updateTimerDisposable = Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => Dispatcher.UIThread.Post(UpdateDynamicItemValues));
    }

    private void UpdateDynamicItemValues()
    {
        foreach (var group in DynamicGroups)
        {
            foreach (var item in group.Items!)
            {
                item.Value = $"数值 {Random.Shared.Next(1000, 9999)}";
            }

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

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Demo page intentionally uses runtime-generated reflection bindings for dynamic columns.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Demo page intentionally uses runtime-generated reflection bindings for dynamic columns.")]
    private void InitDynamicDataGrid()
    {
        if (_myDataGrid == null || !_isFirstLoadDataGrid || !DynamicGroups.Any() ||
            DynamicGroups.First().Items?.Any() != true)
        {
            return;
        }

        _isFirstLoadDataGrid = false;

        var dynamicColumns = DynamicGroups.First().Items!.Select((item, index) =>
        {
            var column = new DataGridTemplateColumn
            {
                IsReadOnly = false,
                HeaderTemplate = new FuncDataTemplate<DynamicGroup>((_, _) => new TextBlock
                {
                    Classes = { "Header" },
                    Text = item.Name
                }),
                CellTemplate = new FuncDataTemplate<DynamicGroup>((_, _) => new TextBlock
                {
                    Classes = { "Content" },
                    [!TextBlock.TextProperty] = new Binding($"Items[{index}].Value")
                })
            };

            return column;
        });

        foreach (var column in dynamicColumns)
        {
            _myDataGrid.Columns.Add(column);
        }
    }
}

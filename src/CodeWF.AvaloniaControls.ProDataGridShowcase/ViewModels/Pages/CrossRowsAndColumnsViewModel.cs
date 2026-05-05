using Avalonia.Threading;
using CodeWF.AvaloniaControls.ProDataGridShowcase.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace CodeWF.AvaloniaControls.ProDataGridShowcase.ViewModels.Pages;

public class FlattenedProcessItem
{
    public int GroupId { get; init; }
    public bool IsKeyGroup { get; init; }
    public string? Cycle { get; init; }
    public bool IsFirstInGroup { get; init; }
    public bool IsLastInGroup { get; init; }
    public int GroupItemCount { get; init; }
    public bool IsMiddleOfGroup { get; init; }
    public Avalonia.Thickness RowBorderThickness { get; init; }

    public int Id { get; init; }
    public string? Name { get; init; }
    public bool Enabled { get; set; }
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

public class CrossRowsAndColumnsViewModel : ReactiveObject, IDisposable
{
    private const int MinGroupCount = 6;
    private const int MaxGroupCount = 50;
    private const int MinItemsPerGroup = 2;
    private const int MaxItemsPerGroup = 20;

    private readonly List<GroupItem> _groups = [];
    private readonly IDisposable _updateTimerDisposable;
    private ObservableCollection<FlattenedProcessItem> _flattenedItems = [];
    private int _nextGroupId = 1;
    private int _nextProcessId = 1;
    private int _updateStep;

    public CrossRowsAndColumnsViewModel()
    {
        for (var i = 0; i < Random.Shared.Next(20, 50); i++)
        {
            _groups.Add(CreateGroup(Random.Shared.Next(5, 20)));
        }

        RebuildFlattenedItems();

        _updateTimerDisposable = Observable.Interval(TimeSpan.FromSeconds(2))
            .Subscribe(_ => Dispatcher.UIThread.Post(UpdateGroups));
    }

    public ObservableCollection<FlattenedProcessItem> FlattenedItems
    {
        get => _flattenedItems;
        private set => this.RaiseAndSetIfChanged(ref _flattenedItems, value);
    }

    public void Dispose()
    {
        _updateTimerDisposable.Dispose();
    }

    private void UpdateGroups()
    {
        if (_groups.Count == 0)
        {
            _groups.Add(CreateGroup(Random.Shared.Next(5, 10)));
            RebuildFlattenedItems();
            return;
        }

        switch (_updateStep++ % 6)
        {
            case 0:
                AddGroup();
                break;
            case 1:
                AddProcessItemToRandomGroup();
                break;
            case 2:
                RemoveProcessItemFromRandomGroup();
                break;
            case 3:
                UpdateRandomGroupMetadata();
                break;
            case 4:
                RemoveGroup();
                break;
            default:
                UpdateRandomProcessItem();
                break;
        }

        RebuildFlattenedItems();
    }

    private void AddGroup()
    {
        if (_groups.Count >= MaxGroupCount)
        {
            RemoveGroup();
            return;
        }

        _groups.Insert(Random.Shared.Next(_groups.Count + 1), CreateGroup(Random.Shared.Next(3, 9)));
    }

    private void RemoveGroup()
    {
        if (_groups.Count <= MinGroupCount)
        {
            AddProcessItemToRandomGroup();
            return;
        }

        _groups.RemoveAt(Random.Shared.Next(_groups.Count));
    }

    private void AddProcessItemToRandomGroup()
    {
        var group = GetRandomGroup(g => g.Items.Count < MaxItemsPerGroup);
        if (group == null)
        {
            AddGroup();
            return;
        }

        group.Items.Insert(Random.Shared.Next(group.Items.Count + 1), CreateProcessItem());
    }

    private void RemoveProcessItemFromRandomGroup()
    {
        var group = GetRandomGroup(g => g.Items.Count > MinItemsPerGroup);
        if (group == null)
        {
            RemoveGroup();
            return;
        }

        group.Items.RemoveAt(Random.Shared.Next(group.Items.Count));
    }

    private void UpdateRandomGroupMetadata()
    {
        var group = GetRandomGroup(_ => true);
        if (group == null)
        {
            return;
        }

        group.IsKeyGroup = !group.IsKeyGroup;
        group.Cycle = $"{Random.Shared.Next(2, 5)}/{Random.Shared.Next(3, 6)}";
    }

    private void UpdateRandomProcessItem()
    {
        var group = GetRandomGroup(g => g.Items.Count > 0);
        if (group == null)
        {
            return;
        }

        var item = group.Items[Random.Shared.Next(group.Items.Count)];
        item.Enabled = !item.Enabled;
        item.AutoStart = !item.AutoStart;
        item.Params = Random.Shared.Next(2) == 0 ? "---" : $"-type {Random.Shared.Next(1, 4)}";
        item.Description = $"Updated {DateTime.Now:HH:mm:ss}";
    }

    private GroupItem CreateGroup(int itemCount)
    {
        var group = new GroupItem
        {
            Id = _nextGroupId++,
            IsKeyGroup = Random.Shared.Next(4) == 0,
            Cycle = $"{Random.Shared.Next(2, 5)}/{Random.Shared.Next(3, 6)}"
        };

        for (var i = 0; i < itemCount; i++)
        {
            group.Items.Add(CreateProcessItem());
        }

        return group;
    }

    private ProcessItem CreateProcessItem()
    {
        var id = _nextProcessId++;

        return new ProcessItem
        {
            Id = id,
            Name = $"Process {id}",
            Enabled = Random.Shared.Next(5) == 0,
            SourceNode = Random.Shared.Next(0, 8),
            Host = "127.0.0.1:89333",
            ProgramPath = "../../test/bb.exe",
            WorkPath = "../../test",
            Params = Random.Shared.Next(5) == 0 ? "---" : "-type 1",
            AutoStart = Random.Shared.Next(5) == 0,
            PreProcess = Random.Shared.Next(5) == 0 ? "---" : "make dir",
            PostProcess = Random.Shared.Next(5) == 0 ? "---" : "remove file",
            Description = Random.Shared.Next(5) == 0 ? "---" : "for dynamic row span test",
        };
    }

    private GroupItem? GetRandomGroup(Func<GroupItem, bool> predicate)
    {
        var groups = _groups.Where(predicate).ToList();
        return groups.Count == 0 ? null : groups[Random.Shared.Next(groups.Count)];
    }

    private void RebuildFlattenedItems()
    {
        var flattenedItemsList = new List<FlattenedProcessItem>(_groups.Sum(g => g.Items.Count));

        foreach (var group in _groups)
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
                    IsMiddleOfGroup = i == itemCount / 2,
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

        FlattenedItems = new ObservableCollection<FlattenedProcessItem>(flattenedItemsList);
    }
}

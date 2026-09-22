using CodeWF.AvaloniaControls.Extensions;

namespace CodeWF.AvaloniaControls.Tests;

public sealed class RangeObservableCollectionTests
{
    [Fact]
    public void AddRangeWithEmptyCollectionDoesNotRaiseReset()
    {
        var items = new RangeObservableCollection<int>();
        var notificationCount = 0;
        items.CollectionChanged += (_, _) => notificationCount++;

        items.AddRange(Array.Empty<int>());

        Assert.Empty(items);
        Assert.Equal(0, notificationCount);
    }

    [Fact]
    public void AddRangeCanCopyTheCollectionItself()
    {
        var items = new RangeObservableCollection<int> { 1, 2 };

        items.AddRange(items);

        Assert.Equal([1, 2, 1, 2], items);
    }

    [Fact]
    public void RemoveRangeRemovesRepeatedValuesByOccurrence()
    {
        var items = new RangeObservableCollection<string> { "same", "same", "other" };

        items.RemoveRange(["same", "same"]);

        Assert.Equal(["other"], items);
    }

    [Fact]
    public void RemoveRangeAllowsAnEmptyRangeAtTheEnd()
    {
        var items = new RangeObservableCollection<int> { 1, 2 };

        items.RemoveRange(items.Count, 0);

        Assert.Equal([1, 2], items);
    }
}

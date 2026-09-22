using System.Collections.ObjectModel;
using CodeWF.AvaloniaControls.Controls;

namespace CodeWF.AvaloniaControls.Tests;

public sealed class TransferSourceTests
{
    [Fact]
    public void SearchListBoxAcceptsAStandardObservableCollection()
    {
        var source = new ObservableCollection<TransferItem> { new("first") };
        var control = new SearchListBox { ItemsSource = source };

        control.AddRange(new[] { new TransferItem("second") });

        Assert.Equal(2, source.Count);
        Assert.Equal("second", source[1].Name);
    }

    private sealed record TransferItem(string Name)
    {
        public override string ToString() => Name;
    }
}

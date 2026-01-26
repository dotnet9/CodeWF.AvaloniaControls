using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CodeWF.AvaloniaControls;

public static class TreeDataGridExtension
{
    public static void AddSelectAll<T>(this TreeDataGrid treeDataGrid, FlatTreeDataGridSource<T> itemSource) where T : class
    {
        treeDataGrid.AddHandler(InputElement.KeyDownEvent, (sender, e) =>
        {
            if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.A)
            {
                itemSource.RowSelection?.Clear();
                itemSource.RowSelection?.BeginBatchUpdate();
                for(var i = 0; i < itemSource.Rows.Count; i++)
                {
                    itemSource.RowSelection?.Select(new IndexPath(i));
                }
                itemSource.RowSelection?.EndBatchUpdate();
                e.Handled = true;
            }
            else if (e.KeyModifiers == (KeyModifiers.Control | KeyModifiers.Shift) && e.Key == Key.A)
            {
                itemSource?.RowSelection?.Clear();
                e.Handled = true;
            }
        }, RoutingStrategies.Tunnel);
    }
}

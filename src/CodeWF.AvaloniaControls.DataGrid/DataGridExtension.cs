using Avalonia.Collections;
using System.ComponentModel;
using System.Linq;

namespace CodeWF.AvaloniaControls.DataGrid;

public static class DataGridExtension
{
    public static void AddSorting(this Avalonia.Controls.DataGrid dataGrid)
    {
        var view = new DataGridCollectionView(dataGrid.ItemsSource);
        dataGrid.Sorting += (s, e) =>
        {
            if (s is not Avalonia.Controls.DataGrid) return;

            var memberPath = e.Column.SortMemberPath;
            var sortDescription = view.SortDescriptions.FirstOrDefault(d => d.PropertyPath == memberPath);
            if (sortDescription is not null && sortDescription.Direction == ListSortDirection.Descending)
            {
                view.SortDescriptions.Clear();
                e.Handled = true;
            }

            dataGrid.ItemsSource = view;
            view.Refresh();
        };
    }
}
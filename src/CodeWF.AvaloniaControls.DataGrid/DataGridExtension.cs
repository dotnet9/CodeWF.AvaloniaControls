using Avalonia.Collections;
using Avalonia.Controls;
using System.ComponentModel;
using System.Linq;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace CodeWF.AvaloniaControls;

public static class DataGridExtension
{
    /// <summary>
    /// 添加排序，点击列有三种状态：未排序、升序、降序。
    /// </summary>
    /// <param name="dataGrid"></param>
    public static void AddSorting(this DataGrid dataGrid)
    {
        var view = new DataGridCollectionView(dataGrid.ItemsSource);
        dataGrid.Sorting += (s, e) =>
        {
            if (s is not DataGrid) return;

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

    /// <summary>
    /// 判断是否双击了DataGrid的行
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static bool IsDoubleClickRow(TappedEventArgs? e)
    {
        if (e is null) return true;
        if (e.Source is not Control control) return false;
        var row = control.FindAncestorOfType<DataGridRow>();
        return row is not null;
    }
}
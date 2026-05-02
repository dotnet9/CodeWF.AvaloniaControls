using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CodeWF.AvaloniaControls;

public static class TreeDataGridExtension
{
    /// <summary>
    /// 为 TreeDataGrid 添加三态排序。
    /// 点击同一列表头时，会在未排序、升序、降序之间循环切换。
    /// </summary>
    public static void AddSorting<T>(this TreeDataGrid treeDataGrid, FlatTreeDataGridSource<T> itemSource)
        where T : class
    {
        treeDataGrid.CanUserSortColumns = false;
        treeDataGrid.AddHandler(Button.ClickEvent, (_, e) =>
        {
            if (e.Source is not TreeDataGridColumnHeader header ||
                treeDataGrid.Source is not ITreeDataGridSource source ||
                source.Columns is not IList columns ||
                header.ColumnIndex < 0 ||
                header.ColumnIndex >= columns.Count ||
                columns[header.ColumnIndex] is not IColumn column)
            {
                return;
            }

            if (column.SortDirection == ListSortDirection.Descending)
            {
                // 第三次点击时清空排序，回到“未排序”状态。
                ClearSorting(treeDataGrid, source, columns);
                e.Handled = true;
                return;
            }

            var direction = column.SortDirection == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;

            source.SortBy(column, direction);
            e.Handled = true;
        }, RoutingStrategies.Bubble);
    }

    /// <summary>
    /// 为 TreeDataGrid 添加 Ctrl+A 全选，以及 Ctrl+Shift+A 取消全选。
    /// </summary>
    public static void AddSelectAll<T>(this TreeDataGrid treeDataGrid, FlatTreeDataGridSource<T> itemSource)
        where T : class
    {
        treeDataGrid.AddHandler(InputElement.KeyDownEvent, (_, e) =>
        {
            if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.A)
            {
                itemSource.RowSelection?.Clear();
                itemSource.RowSelection?.BeginBatchUpdate();

                for (var i = 0; i < itemSource.Rows.Count; i++)
                {
                    itemSource.RowSelection?.Select(new IndexPath(i));
                }

                itemSource.RowSelection?.EndBatchUpdate();
                e.Handled = true;
            }
            else if (e.KeyModifiers == (KeyModifiers.Control | KeyModifiers.Shift) && e.Key == Key.A)
            {
                itemSource.RowSelection?.Clear();
                e.Handled = true;
            }
        }, RoutingStrategies.Tunnel);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Legacy TreeDataGrid 11.1.1 requires reflection to clear private sorting fields for tri-state unsort behavior.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Legacy TreeDataGrid 11.1.1 requires reflection to clear private sorting fields for tri-state unsort behavior.")]
    private static void ClearSorting(TreeDataGrid treeDataGrid, ITreeDataGridSource source, IList columns)
    {
        foreach (var item in columns)
        {
            if (item is IColumn column)
            {
                column.SortDirection = null;
            }
        }

        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var rows = source.Rows;

        // 旧版免费 TreeDataGrid 没有公开的“清空排序”接口，这里通过反射恢复内部状态。
        rows.GetType().GetField("_sortedIndexes", flags)?.SetValue(rows, null);
        rows.GetType().GetField("_comparer", flags)?.SetValue(rows, null);

        // 重新挂回 Source，强制界面刷新排序结果。
        treeDataGrid.Source = null;
        treeDataGrid.Source = source;
    }
}

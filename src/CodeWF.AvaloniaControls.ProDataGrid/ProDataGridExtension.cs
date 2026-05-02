using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Reactive;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace CodeWF.AvaloniaControls;

public static class ProDataGridExtension
{
    /// <summary>
    /// 为 ProDataGrid 启用大数据量场景下更稳妥的默认参数。
    /// </summary>
    public static void ApplyPerformancePreset(this DataGrid dataGrid, bool isReadOnly = true)
    {
        // ProDataGrid 12 的虚拟化与滚动优化建立在逻辑滚动之上，
        // 这里保持启用，确保大数据量页签切换与鼠标滚轮行为一致。
        dataGrid.UseLogicalScrollable = true;
        dataGrid.IsReadOnly = isReadOnly;
        dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
        dataGrid.CanUserReorderColumns = false;
        dataGrid.CanUserResizeColumns = true;
        dataGrid.RowHeight = dataGrid.RowHeight <= 0 ? 36 : dataGrid.RowHeight;
    }

    /// <summary>
    /// 添加三态排序，点击列头时按“未排序、升序、降序”循环切换。
    /// </summary>
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Tri-state sorting relies on DataGridCollectionView reflection behavior provided by ProDataGrid.")]
    public static void AddSorting(this DataGrid dataGrid)
    {
        dataGrid.Sorting += (_, e) =>
        {
            if (dataGrid.ItemsSource is null || e.Column is null)
            {
                return;
            }

            var view = dataGrid.ItemsSource as DataGridCollectionView ?? new DataGridCollectionView(dataGrid.ItemsSource);
            var memberPath = e.Column.SortMemberPath;
            var sortDescription = view.SortDescriptions.FirstOrDefault(d => d.PropertyPath == memberPath);

            if (sortDescription is not null && sortDescription.Direction == ListSortDirection.Descending)
            {
                view.SortDescriptions.Clear();
                dataGrid.ItemsSource = view;
                view.Refresh();
                e.Handled = true;
                return;
            }

            dataGrid.ItemsSource = view;
            view.Refresh();
        };
    }

    /// <summary>
    /// 判断是否双击了表格行。
    /// </summary>
    public static bool IsDoubleClickRow(TappedEventArgs? e)
    {
        if (e is null)
        {
            return true;
        }

        if (e.Source is not Control control)
        {
            return false;
        }

        return control.FindAncestorOfType<DataGridRow>() is not null;
    }

    /// <summary>
    /// 为表格启用仅在文字被截断时才显示的智能提示。
    /// </summary>
    public static void EnableSmartTooltips(this DataGrid dataGrid, params int[] targetColumnIndexes)
    {
        dataGrid.LoadingRow += (_, e) => DispatcherTimer.RunOnce(
            () => ProcessDataGridRow(e.Row, targetColumnIndexes),
            TimeSpan.FromMilliseconds(300));
    }

    private static void ProcessDataGridRow(DataGridRow row, int[]? targetColumnIndexes)
    {
        var cells = new List<DataGridCell>();
        FindVisualChildren(row, cells);

        if (targetColumnIndexes?.Any() != true)
        {
            cells.ForEach(ProcessCell);
            return;
        }

        foreach (var columnIndex in targetColumnIndexes)
        {
            if (columnIndex < 0 || columnIndex >= cells.Count)
            {
                continue;
            }

            ProcessCell(cells[columnIndex]);
        }
    }

    private static void ProcessCell(DataGridCell cell)
    {
        var textBlocks = new List<TextBlock>();
        FindVisualChildren(cell, textBlocks);
        textBlocks.ForEach(SetupSmartTooltip);
    }

    private static void FindVisualChildren<T>(Visual visual, List<T> array)
        where T : Visual
    {
        foreach (var child in visual.GetVisualChildren())
        {
            if (child is T match)
            {
                array.Add(match);
            }

            FindVisualChildren(child, array);
        }
    }

    private static void SetupSmartTooltip(TextBlock textBlock)
    {
        if (textBlock.Tag is true)
        {
            return;
        }

        // 使用 Tag 做一次性标记，避免重复订阅同一个 TextBlock。
        textBlock.Tag = true;
        textBlock.TextTrimming = TextTrimming.CharacterEllipsis;

        UpdateToolTip(textBlock);
        textBlock.GetObservable(TextBlock.TextProperty)
            .Subscribe(new AnonymousObserver<string?>(_ => UpdateToolTip(textBlock)));
        textBlock.GetObservable(Visual.BoundsProperty)
            .Subscribe(new AnonymousObserver<Rect>(_ => UpdateToolTip(textBlock)));
    }

    private static void UpdateToolTip(TextBlock textBlock)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(textBlock.Text) || textBlock.Bounds.Width <= 0)
            {
                ToolTip.SetTip(textBlock, null);
                return;
            }

            var formattedText = new FormattedText(
                textBlock.Text!,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight),
                textBlock.FontSize,
                Brushes.Black);

            ToolTip.SetTip(textBlock, formattedText.Width > textBlock.Bounds.Width ? textBlock.Text : null);
        }
        catch
        {
            // 样例工具提示只做增强，不让异常影响主交互。
        }
    }
}

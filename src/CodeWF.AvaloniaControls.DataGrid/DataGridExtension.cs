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
using System.Globalization;
using System.Linq;

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

    /// <summary>
    /// 为 DataGrid 启用智能提示 ToolTip（只在内容显示不全时显示）
    /// </summary>
    /// <param name="dataGrid"></param>
    public static void EnableSmartTooltips(this DataGrid dataGrid)
    {
        dataGrid.LoadingRow += (sender, e) => DispatcherTimer.RunOnce(() => ProcessDataGridRow(e.Row), TimeSpan.FromMilliseconds(1000));
    }

    /// <summary>    
    /// 为 DataGrid 启用智能提示 ToolTip，可指定要处理的列索引
    /// </summary>
    /// <param name="dataGrid"></param>
    /// <param name="targetColumnIndexes"></param>
    public static void EnableSmartTooltips(this DataGrid dataGrid, params int[] targetColumnIndexes)
    {
        dataGrid.LoadingRow += (sender, e) => DispatcherTimer.RunOnce(() => ProcessDataGridRow(e.Row, targetColumnIndexes), TimeSpan.FromMilliseconds(1000));
    }

    private static void ProcessDataGridRow(DataGridRow row, int[]? targetColumnIndexes = default)
    {
        var cells = new List<DataGridCell>();
        FindVisualChildren(row, cells);

        if(targetColumnIndexes?.Any() != true)
        {
            cells.ForEach(ProcessCell);
            return;
        }

        for(var i = 0; i <targetColumnIndexes.Length;i++)
        {
            var cell = cells[i];
            ProcessCell(cell);
        }
    }

    private static void ProcessCell(DataGridCell cell)
    {
        var textBlocks = new List<TextBlock>();
        FindVisualChildren(cell, textBlocks);

        textBlocks.ForEach(SetupSmartTooltip);
    }

    private static void FindVisualChildren<T>(Visual visual, List<T> array)where T:Visual
    {
        foreach (var child in visual.GetVisualChildren())
        {
            if(child is T t)
            {
                array.Add(t);
            }

            if(child is Visual visualChild)
            {
                FindVisualChildren(visualChild, array);
            }
        }
    }

    private static void SetupSmartTooltip(TextBlock textBlock)
    {
        if (textBlock.Tag != null) return;

        textBlock.Tag = true;
        textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
        textBlock.FontFamily = FontFamily.Default;

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
            if(string.IsNullOrEmpty(textBlock.Text) || textBlock.Bounds.Width <= 0)
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
        catch(Exception ex)
        {

        }
    }
}
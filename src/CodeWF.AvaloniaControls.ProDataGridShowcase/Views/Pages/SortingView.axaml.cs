using Avalonia.Controls;
using Avalonia.Data;
using CodeWF.AvaloniaControls;
using CodeWF.AvaloniaControls.ProDataGridShowcase.Models;
using System.Diagnostics.CodeAnalysis;

namespace CodeWF.AvaloniaControls.ProDataGridShowcase.Views.Pages;

public partial class SortingView : UserControl
{
    private bool _isTriStateSortingEnabled;

    public SortingView()
    {
        InitializeComponent();
        InitializeColumns(DefaultStudentDataGrid);
        InitializeColumns(StudentDataGrid);
        DefaultStudentDataGrid.ApplyPerformancePreset();
        StudentDataGrid.ApplyPerformancePreset();
    }

    private static void InitializeColumns(DataGrid dataGrid)
    {
        if (dataGrid.Columns.Count > 0)
        {
            return;
        }

        AddColumn(dataGrid, "编号", nameof(Student.Id));
        AddColumn(dataGrid, "姓名", nameof(Student.Name));
        AddColumn(dataGrid, "地区", nameof(Student.Address));
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The sample builds DataGrid columns dynamically in code to demonstrate runtime column setup on Avalonia 12.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "The sample builds DataGrid columns dynamically in code to demonstrate runtime column setup on Avalonia 12.")]
    private static void AddColumn(DataGrid dataGrid, string header, string memberPath)
    {
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = header,
            SortMemberPath = memberPath,
            Binding = new Binding(memberPath),
        });
    }

    private void EnableTriStateSortingButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_isTriStateSortingEnabled)
        {
            return;
        }

        StudentDataGrid.AddSorting();
        _isTriStateSortingEnabled = true;

        EnableTriStateSortingButton.Content = "已启用三态排序";
        EnableTriStateSortingButton.IsEnabled = false;
    }
}

using Avalonia.Controls;
using CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.ViewModels;

namespace CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.Views;

public partial class DataGridDemo : UserControl
{
    public DataGridDemo()
    {
        DataContext = new DataGridDemoViewModel();
        InitializeComponent();
        DataGridColumnInitializer.EnsureProcessColumns(ProcessDataGrid);
        DataGridColumnInitializer.ApplyDefaultBehavior(ProcessDataGrid);
    }
}

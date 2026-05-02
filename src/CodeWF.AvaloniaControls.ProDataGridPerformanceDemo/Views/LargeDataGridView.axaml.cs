using Avalonia.Controls;
using CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.ViewModels;

namespace CodeWF.AvaloniaControls.ProDataGridPerformanceDemo.Views;

public partial class LargeDataGridView : UserControl
{
    private bool _initialized;

    public LargeDataGridView()
    {
        InitializeComponent();
        Loaded += (_, _) => EnsureInitialized();
    }

    public string HeaderTitle { get; set; } = "大数据量页签";

    public string HeaderDescription { get; set; } = "用于验证页签切换时的滚动与重绘表现。";

    public int RowCount { get; set; } = 120000;

    public int Seed { get; set; } = 1;

    private void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        var viewModel = new LargeDataGridViewModel(RowCount, Seed, HeaderTitle);
        DataContext = viewModel;

        HeaderTitleBlock.Text = HeaderTitle;
        HeaderDescriptionBlock.Text = HeaderDescription;
        SummaryBlock.Text = viewModel.Summary;

        ProcessDataGrid.ItemsSource = viewModel.Items;
        DataGridColumnInitializer.EnsureProcessColumns(ProcessDataGrid);
        DataGridColumnInitializer.ApplyDefaultBehavior(ProcessDataGrid);

        _initialized = true;
    }
}

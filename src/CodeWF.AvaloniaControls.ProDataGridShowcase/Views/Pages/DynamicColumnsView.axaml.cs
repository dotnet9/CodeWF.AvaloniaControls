using Avalonia.Controls;
using Avalonia.Interactivity;
using CodeWF.AvaloniaControls.ProDataGridShowcase.ViewModels.Pages;

namespace CodeWF.AvaloniaControls.ProDataGridShowcase.Views.Pages;

public partial class DynamicColumnsView : UserControl
{
    public DynamicColumnsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is DynamicColumnsViewModel viewModel)
        {
            // 这里直接在视图加载完成后触发一次动态列初始化，
            // 避免额外引入行为包，同时让 ProDataGrid 示例保持依赖简单。
            viewModel.RaiseDataGridLoadHandler(MyDataGrid);
        }
    }
}

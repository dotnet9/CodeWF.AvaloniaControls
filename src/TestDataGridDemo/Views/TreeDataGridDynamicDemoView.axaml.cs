using Avalonia.Controls;
using CodeWF.AvaloniaControls;
using TestDataGridDemo.ViewModels;

namespace TestDataGridDemo.Views;

public partial class TreeDataGridDynamicDemoView : UserControl
{
    public TreeDataGridDynamicDemoViewModel ViewModel    { get; set; }
    public TreeDataGridDynamicDemoView()
    {
        DataContext= ViewModel = new TreeDataGridDynamicDemoViewModel();
        InitializeComponent();
        MyTree.AddSelectAll(ViewModel.ItemsSource!);
    }
}
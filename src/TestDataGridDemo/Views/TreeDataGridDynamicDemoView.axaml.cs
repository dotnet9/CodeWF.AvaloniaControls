using Avalonia.Controls;
using TestDataGridDemo.ViewModels;

namespace TestDataGridDemo.Views;

public partial class TreeDataGridDynamicDemoView : UserControl
{
    public TreeDataGridDynamicDemoView()
    {
        DataContext = new TreeDataGridDynamicDemoViewModel();
        InitializeComponent();
    }
}
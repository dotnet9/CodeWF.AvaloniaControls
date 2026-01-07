using Avalonia.Controls;
using TestDataGridDemo.ViewModels;

namespace TestDataGridDemo.Views;

public partial class TreeDataGridDemoView : UserControl
{
    public TreeDataGridDemoView()
    {
        DataContext = new TreeDataGridDemoViewModel();
        InitializeComponent();
    }
}
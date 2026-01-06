using Avalonia.Controls;
using TestDataGridDemo.ViewModels;

namespace TestDataGridDemo.Views;

public partial class DataGridDemo : UserControl
{
    public DataGridDemo()
    {
        DataContext = new DataGridDemoViewModel();
        InitializeComponent();
    }
}
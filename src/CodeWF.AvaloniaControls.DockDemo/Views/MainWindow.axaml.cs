using Avalonia;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using Ursa.Controls;

namespace CodeWF.AvaloniaControls.DockDemo.Views;

public partial class MainWindow : UrsaWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        if (Debugger.IsAttached)
        {
            this.AttachDevTools();
        }
    }
}
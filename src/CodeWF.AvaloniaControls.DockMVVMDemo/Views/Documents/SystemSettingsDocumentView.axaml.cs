using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CodeWF.AvaloniaControls.DockMVVMDemo.Views.Documents;

public partial class SystemSettingsDocumentView : UserControl
{
    public SystemSettingsDocumentView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
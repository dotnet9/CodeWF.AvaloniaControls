using Avalonia.Controls.Templates;
using CodeWF.AvaloniaControls.Controls;

namespace CodeWF.AvaloniaControls.Showcase.Views.TestNoneWindowDemos;

public partial class CodeWFWindowStandardDemo : CodeWFWindow
{
    public CodeWFWindowStandardDemo()
    {
        InitializeComponent();
        ApplyTitleBarSlotTemplates();
    }

    private void ApplyTitleBarSlotTemplates()
    {
        LeftContentTemplate = (IDataTemplate)Resources["SampleLeftContentTemplate"]!;
        TitleBarContentTemplate = (IDataTemplate)Resources["SampleTitleBarContentTemplate"]!;
        RightContentTemplate = (IDataTemplate)Resources["SampleRightContentTemplate"]!;
    }
}

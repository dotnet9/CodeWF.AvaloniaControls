using Avalonia.Controls;
using Avalonia.Controls.Metadata;

namespace CodeWF.AvaloniaControls.Controls.TabControls;

[PseudoClasses(":previewSelected", ":firstSelected")]
public class TrapezoidShapedTabItem : TabItem
{
    public void SetSelectionPseudoClasses(bool isPreviewSelected, bool isFirstSelected)
    {
        PseudoClasses.Set(":previewSelected", isPreviewSelected);
        PseudoClasses.Set(":firstSelected", isFirstSelected);
    }
}
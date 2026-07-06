using System.Collections.Generic;
using CodeWF.AvaloniaControlsDemo.Models;
using ReactiveUI;

namespace CodeWF.AvaloniaControlsDemo.ViewModels;

internal sealed class AutoCompleteBoxDemoViewModel : ReactiveObject
{
    public AutoCompleteBoxSampleViewModel DetailedSample { get; } = new(CreateOptions());

    public AutoCompleteBoxSampleViewModel SimpleSample { get; } = new(CreateOptions());

    private static IReadOnlyList<AutoCompleteBoxDemoOption> CreateOptions()
    {
        return
        [
            new("Dashboard", "Pinned", "Default entry that always stays at the top.", "dashboard overview home", 0),
            new("Command Palette", "Pinned", "Default entry that always stays at the top.", "command palette action", 1),
            new("DataGrid", "Data", "Dense data table with sorting and filtering.", "table grid data row", 2),
            new("Transfer", "Selection", "Move items between source and target lists.", "transfer selection list", 3),
            new("VComboBox", "Input", "Compact combo box for command surfaces.", "combo input selector warning", 4),
            new("TabControl", "Navigation", "Switch between related work areas.", "tabs navigation page", 5),
            new("Guide Tour", "Overlay", "Step-by-step guidance for complex screens.", "guide tour overlay help", 6),
            new("StatusBadge", "Status", "Inline severity and state indicator.", "badge status warning", 7),
            new("StatusCard", "Status", "Card-style service health indicator.", "card health service", 8),
            new("AnimatedImage", "Media", "Load local or network animated GIF resources.", "gif image animation", 9),
            new("CodeWFWindow", "Window", "Managed title bar and shell options.", "window titlebar shell", 10),
            new("MarkupExtensions", "XAML", "If and Switch markup extension examples.", "xaml markup converter", 11)
        ];
    }
}

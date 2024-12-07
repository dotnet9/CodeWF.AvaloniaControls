using Avalonia.Media;
using CodeWF.AvaloniaControls.Demo.Models;
using System.Collections.Generic;

namespace CodeWF.AvaloniaControls.Demo.ViewModels
{
    internal class VComboBoxDemoViewModel
    {
        public List<WarningItem> WarningItems { get; } = new List<WarningItem>()
        {
            new() { Color = new SolidColorBrush(Colors.Black), Name = "显示所有" },
            new() { Color = new SolidColorBrush(Colors.Red), Name = "筛选告警" },
            new() { Color = new SolidColorBrush(Colors.Green), Name = "筛选正常" }
        };
    }
}
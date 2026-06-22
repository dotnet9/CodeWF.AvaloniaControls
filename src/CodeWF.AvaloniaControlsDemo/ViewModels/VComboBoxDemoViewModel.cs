using System.Collections.ObjectModel;
using Avalonia.Media;
using CodeWF.AvaloniaControlsDemo.Models;
using CodeWF.AvaloniaControlsDemo.Services;
using Lang.Avalonia;
using PageLangs = Showcase.Pages.VComboBoxDemo;

namespace CodeWF.AvaloniaControlsDemo.ViewModels;

internal class VComboBoxDemoViewModel
{
    public VComboBoxDemoViewModel()
    {
        WarningItems =
        [
            new WarningItem { Color = new SolidColorBrush(Color.Parse("#64748B")) },
            new WarningItem { Color = new SolidColorBrush(Colors.Red) },
            new WarningItem { Color = new SolidColorBrush(Colors.Green) }
        ];

        ReloadText();
        I18nManager.Instance.CultureChanged += (_, _) => ReloadText();
    }

    public ObservableCollection<WarningItem> WarningItems { get; }

    private void ReloadText()
    {
        WarningItems[0].Name = LocalizationService.Get(PageLangs.WarningAll);
        WarningItems[1].Name = LocalizationService.Get(PageLangs.WarningAlert);
        WarningItems[2].Name = LocalizationService.Get(PageLangs.WarningNormal);
    }
}
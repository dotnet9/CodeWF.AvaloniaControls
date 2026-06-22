using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Controls;
using CodeWF.AvaloniaControls.Controls;
using CodeWF.AvaloniaControls.Extensions;
using CodeWF.AvaloniaControlsDemo.Services;
using Lang.Avalonia;
using PageLangs = Showcase.Pages.TransferDemo;

namespace CodeWF.AvaloniaControlsDemo.Pages;

public partial class TransferDemo : UserControl, INotifyPropertyChanged
{
    private readonly Transfer _myTransfer;
    private string? _selectedInfo;

    public TransferDemo()
    {
        InitializeComponent();
        _myTransfer = this.FindControl<Transfer>("MyTransfer")
                      ?? throw new InvalidOperationException("Transfer control 'MyTransfer' was not found.");

        RightItems.CollectionChanged += (_, _) => UpdateSelectedCount();
        I18nManager.Instance.CultureChanged += (_, _) => ReloadLocalizedData();
        ReloadLocalizedData();
    }

    public RangeObservableCollection<string> LeftItems { get; set; } = [];

    public RangeObservableCollection<string> RightItems { get; set; } = [];

    public string? SelectedInfo
    {
        get => _selectedInfo;
        set
        {
            if (_selectedInfo == value) return;

            _selectedInfo = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedInfo)));
        }
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    private void ReloadLocalizedData()
    {
        _myTransfer.LeftHeader = LocalizationService.Get(PageLangs.LeftHeader);
        _myTransfer.RightHeader = LocalizationService.Get(PageLangs.RightHeader);

        LeftItems.Clear();
        LeftItems.AddRange(
        [
            "codewf.com",
            "dotnet9.com",
            "dotnetools.com",
            "dotnet.chat",
            "Ding",
            "Otter",
            LocalizationService.Get(PageLangs.Item01),
            LocalizationService.Get(PageLangs.Item02),
            LocalizationService.Get(PageLangs.Item03),
            LocalizationService.Get(PageLangs.Item04),
            LocalizationService.Get(PageLangs.Item05),
            LocalizationService.Get(PageLangs.Item06),
            LocalizationService.Get(PageLangs.Item07),
            LocalizationService.Get(PageLangs.Item08),
            LocalizationService.Get(PageLangs.Item09)
        ]);

        RightItems.Clear();
        RightItems.AddRange(
        [
            "Husky",
            "Mr.17",
            "Cass",
            LocalizationService.Get(PageLangs.Item10),
            LocalizationService.Get(PageLangs.Item11),
            LocalizationService.Get(PageLangs.Item04),
            LocalizationService.Get(PageLangs.Item12),
            LocalizationService.Get(PageLangs.Item13),
            LocalizationService.Get(PageLangs.Item14),
            LocalizationService.Get(PageLangs.Item15)
        ]);

        UpdateSelectedCount();
    }

    private void UpdateSelectedCount()
    {
        SelectedInfo = RightItems.Count <= 0
            ? LocalizationService.Get(PageLangs.EmptySelection)
            : string.Join(GetListSeparator(), RightItems);
    }

    private static string GetListSeparator()
    {
        var culture = I18nManager.Instance.Culture ?? CultureInfo.CurrentUICulture;
        return $"{culture.TextInfo.ListSeparator} ";
    }
}
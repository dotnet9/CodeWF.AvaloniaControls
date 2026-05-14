using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Styling;
using CodeWF.AvaloniaControlsDemo.Pages;
using Lang.Avalonia;
using ReactiveUI;

using MainWindowLangs = global::Showcase.Main.MainWindow;

namespace CodeWF.AvaloniaControlsDemo.ViewModels;

public sealed class MainWindowViewModel : ReactiveObject
{
    private readonly List<ShowcasePageItem> _pages;
    private string? _searchText;
    private LocalizationLanguage? _selectedLanguage;
    private ShowcasePageItem? _selectedPage;
    private bool _isDarkTheme;

    public MainWindowViewModel()
    {
        Languages = CreateLanguages(I18nManager.Instance.GetLanguages()?.Select(language => language.CultureName));
        _selectedLanguage = Languages.FirstOrDefault(l => l.CultureName == I18nManager.Instance.Culture?.Name)
            ?? Languages.FirstOrDefault();

        _pages =
        [
            new(MainWindowLangs.TabOverview, "Overview", new OverviewDemo()),
            new(MainWindowLangs.TabTransfer, "Transfer", new TransferDemo()),
            new(MainWindowLangs.TabVComboBox, "VComboBox", new VComboBoxDemo()),
            new(MainWindowLangs.TabTabControl, "TabControl", new TabControlDemo()),
            new(MainWindowLangs.TabStatusBadge, "StatusBadge", new StatusBadgeDemo()),
            new(MainWindowLangs.TabStatusCard, "StatusCard", new StatusCardDemo()),
            new(MainWindowLangs.TabMarkup, "MarkupExtensions If Switch Converter", new MarkupExtensionsDemo()),
            new(MainWindowLangs.TabAnimatedImage, "AnimatedImage GIF", new AnimatedImageDemo()),
            new(MainWindowLangs.TabWindowDemos, "Window UrsaWindow CodeWFWindow", new TestNoneWindowDemo())
        ];

        _selectedPage = FilteredPages.FirstOrDefault();
        I18nManager.Instance.CultureChanged += OnCultureChanged;
    }

    public IReadOnlyList<LocalizationLanguage> Languages { get; }

    public IReadOnlyList<ShowcasePageItem> FilteredPages =>
        _pages.Where(page => page.Matches(SearchText)).ToList();

    public bool HasNoMatches => FilteredPages.Count == 0;

    public bool HasMatches => !HasNoMatches;

    public string? SearchText
    {
        get => _searchText;
        set
        {
            this.RaiseAndSetIfChanged(ref _searchText, value);
            this.RaisePropertyChanged(nameof(FilteredPages));
            this.RaisePropertyChanged(nameof(HasNoMatches));
            this.RaisePropertyChanged(nameof(HasMatches));
            EnsureSelectedPageIsVisible();
        }
    }

    public ShowcasePageItem? SelectedPage
    {
        get => _selectedPage;
        set => this.RaiseAndSetIfChanged(ref _selectedPage, value);
    }

    public LocalizationLanguage? SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedLanguage, value);
            if (value is null)
            {
                return;
            }

            I18nManager.Instance.Culture = new CultureInfo(value.CultureName);
            this.RaisePropertyChanged(nameof(CurrentCultureName));
            this.RaisePropertyChanged(nameof(SelectedLanguageDescription));
        }
    }

    public string CurrentCultureName => I18nManager.Instance.Culture?.Name ?? CultureInfo.CurrentUICulture.Name;

    public string SelectedLanguageDescription =>
        SelectedLanguage == null ? string.Empty : $"{SelectedLanguage.CultureName} · {SelectedLanguage.DetailText}";

    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set
        {
            this.RaiseAndSetIfChanged(ref _isDarkTheme, value);
            Application.Current!.RequestedThemeVariant = value ? ThemeVariant.Dark : ThemeVariant.Light;
        }
    }

    private void OnCultureChanged(object? sender, EventArgs e)
    {
        foreach (var page in _pages)
        {
            page.Refresh();
        }

        this.RaisePropertyChanged(nameof(FilteredPages));
        this.RaisePropertyChanged(nameof(HasNoMatches));
        this.RaisePropertyChanged(nameof(HasMatches));
        this.RaisePropertyChanged(nameof(CurrentCultureName));
        this.RaisePropertyChanged(nameof(SelectedLanguageDescription));
        EnsureSelectedPageIsVisible();
    }

    private void EnsureSelectedPageIsVisible()
    {
        var filteredPages = FilteredPages;
        if (SelectedPage is not null && filteredPages.Contains(SelectedPage))
        {
            return;
        }

        SelectedPage = filteredPages.FirstOrDefault();
    }

    private static List<LocalizationLanguage> CreateLanguages(IEnumerable<string>? cultureNames)
    {
        return (cultureNames ?? [])
            .Where(cultureName => !string.IsNullOrWhiteSpace(cultureName))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(CreateLanguage)
            .OrderBy(GetSortOrder)
            .ThenBy(language => language.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static LocalizationLanguage CreateLanguage(string cultureName)
    {
        try
        {
            var culture = CultureInfo.GetCultureInfo(cultureName);
            return new LocalizationLanguage
            {
                CultureName = culture.Name,
                Language = culture.EnglishName,
                Description = culture.NativeName
            };
        }
        catch (CultureNotFoundException)
        {
            return new LocalizationLanguage
            {
                CultureName = cultureName,
                Language = cultureName,
                Description = cultureName
            };
        }
    }

    private static int GetSortOrder(LocalizationLanguage language) => language.CultureName switch
    {
        "zh-CN" => 0,
        "zh-Hant" => 1,
        "en-US" => 2,
        "ja-JP" => 3,
        _ => 4
    };
}

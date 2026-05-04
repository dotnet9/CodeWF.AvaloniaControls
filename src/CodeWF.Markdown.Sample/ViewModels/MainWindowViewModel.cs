using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Styling;

using CodeWF.Markdown.Themes;

using CommunityToolkit.Mvvm.ComponentModel;

namespace CodeWF.Markdown.Sample.ViewModels;

public sealed class MainWindowViewModel : ObservableObject
{
    private const string SampleTypographyThemeKey = "SampleInkGreen";

    private MarkdownSampleFile? _selectedFile;
    private MarkdownTypographyTheme? _selectedTypographyTheme;
    private ThemeVariantOption? _selectedThemeVariant;
    private string _markdown = string.Empty;

    public MainWindowViewModel()
    {
        MarkdownBasePath = ResolveMarkdownBasePath();
        ThemeVariants =
        [
            new("浅色", ThemeVariant.Light),
            new("深色", ThemeVariant.Dark),
        ];
        TypographyThemes = new ObservableCollection<MarkdownTypographyTheme>(
            MarkdownTypographyThemes.All.Append(new MarkdownTypographyTheme("示例追加：青墨", SampleTypographyThemeKey)));
        MarkdownFiles = new ObservableCollection<MarkdownSampleFile>(LoadMarkdownFiles());

        SelectedThemeVariant = ThemeVariants[0];
        SelectedTypographyTheme = TypographyThemes.FirstOrDefault(theme => theme.Key == MarkdownTypographyThemes.OrangeHeart)
                                  ?? TypographyThemes.FirstOrDefault();
        SelectedFile = MarkdownFiles.FirstOrDefault();
    }

    public ObservableCollection<ThemeVariantOption> ThemeVariants { get; }

    public ObservableCollection<MarkdownTypographyTheme> TypographyThemes { get; }

    public ObservableCollection<MarkdownSampleFile> MarkdownFiles { get; }

    public string MarkdownBasePath { get; }

    public string Markdown
    {
        get => _markdown;
        set => SetProperty(ref _markdown, value ?? string.Empty);
    }

    public MarkdownSampleFile? SelectedFile
    {
        get => _selectedFile;
        set
        {
            if (SetProperty(ref _selectedFile, value))
            {
                LoadMarkdown();
            }
        }
    }

    public MarkdownTypographyTheme? SelectedTypographyTheme
    {
        get => _selectedTypographyTheme;
        set
        {
            if (SetProperty(ref _selectedTypographyTheme, value))
            {
                OnPropertyChanged(nameof(SelectedTypographyThemeKey));
            }
        }
    }

    public string SelectedTypographyThemeKey => SelectedTypographyTheme?.Key ?? MarkdownTypographyThemes.Simple;

    public ThemeVariantOption? SelectedThemeVariant
    {
        get => _selectedThemeVariant;
        set
        {
            if (SetProperty(ref _selectedThemeVariant, value) && value is not null && Application.Current is { } app)
            {
                app.RequestedThemeVariant = value.ThemeVariant;
            }
        }
    }

    private void LoadMarkdown()
    {
        if (SelectedFile is null || !File.Exists(SelectedFile.Path))
        {
            Markdown = "# CodeWF.Markdown\n\n示例 Markdown 文件未找到。";
            return;
        }

        Markdown = File.ReadAllText(SelectedFile.Path);
    }

    private IReadOnlyList<MarkdownSampleFile> LoadMarkdownFiles()
    {
        if (!Directory.Exists(MarkdownBasePath))
        {
            return [];
        }

        return Directory.GetFiles(MarkdownBasePath, "*.md")
            .OrderBy(path => path)
            .Select(path => new MarkdownSampleFile(Path.GetFileName(path), path))
            .ToList();
    }

    private static string ResolveMarkdownBasePath()
    {
        var outputPath = Path.Combine(AppContext.BaseDirectory, "MarkdownSamples");
        if (Directory.Exists(outputPath))
        {
            return outputPath;
        }

        // 设计器或源码运行时，回退到项目目录下的样例文件。
        var sourcePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "MarkdownSamples"));
        return Directory.Exists(sourcePath) ? sourcePath : outputPath;
    }
}

public sealed record ThemeVariantOption(string Name, ThemeVariant ThemeVariant);

public sealed record MarkdownSampleFile(string Name, string Path);

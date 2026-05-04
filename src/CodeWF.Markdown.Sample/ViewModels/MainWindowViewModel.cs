using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Threading;

using CodeWF.Markdown.Sample.Themes;
using CodeWF.Markdown.Themes;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CodeWF.Markdown.Sample.ViewModels;

public sealed class MainWindowViewModel : ObservableObject
{
    private const string SampleTypographyThemeKey = "SampleInkGreen";
    private const string IncrementalStressHeading = "## 自动增量压力";

    private readonly DispatcherTimer _incrementalStressTimer;
    private MarkdownSampleFile? _selectedFile;
    private MarkdownTypographyTheme? _selectedTypographyTheme;
    private ThemeVariantOption? _selectedThemeVariant;
    private string _markdown = string.Empty;
    private bool _isIncrementalStressRunning;
    private int _incrementalStressTick;

    public MainWindowViewModel()
    {
        _incrementalStressTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(220)
        };
        _incrementalStressTimer.Tick += (_, _) => ApplyIncrementalStressTick();
        ToggleIncrementalStressCommand = new RelayCommand(ToggleIncrementalStress);

        MarkdownBasePath = ResolveMarkdownBasePath();
        ThemeVariants =
        [
            new("浅色", ThemeVariant.Light),
            new("深色", ThemeVariant.Dark),
        ];
        TypographyThemes = new ObservableCollection<MarkdownTypographyTheme>(
            MarkdownTypographyThemes.All.Append(new MarkdownTypographyTheme("示例：青墨绿", SampleTypographyThemeKey)));
        MarkdownFiles = new ObservableCollection<MarkdownSampleFile>(LoadMarkdownFiles());

        SelectedThemeVariant = ThemeVariants[0];
        SelectedTypographyTheme = TypographyThemes.FirstOrDefault(theme => theme.Key == MarkdownTypographyThemes.OrangeHeart)
                                  ?? TypographyThemes.FirstOrDefault();
        SelectedFile = MarkdownFiles.FirstOrDefault();
    }

    public ObservableCollection<ThemeVariantOption> ThemeVariants { get; }

    public ObservableCollection<MarkdownTypographyTheme> TypographyThemes { get; }

    public ObservableCollection<MarkdownSampleFile> MarkdownFiles { get; }

    public RelayCommand ToggleIncrementalStressCommand { get; }

    public string MarkdownBasePath { get; }

    public bool IsIncrementalStressRunning
    {
        get => _isIncrementalStressRunning;
        private set
        {
            if (SetProperty(ref _isIncrementalStressRunning, value))
            {
                OnPropertyChanged(nameof(IncrementalStressButtonText));
            }
        }
    }

    public string IncrementalStressButtonText => IsIncrementalStressRunning ? "停止增量压力" : "开始增量压力";

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
                StopIncrementalStress();
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
                ApplySelectedTypographyTheme();
            }
        }
    }

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

    private void ApplySelectedTypographyTheme()
    {
        if (Application.Current is not { } app)
        {
            return;
        }

        if (SelectedTypographyTheme?.Key == SampleTypographyThemeKey)
        {
            MarkdownThemes.OverrideTypographyResources(app, LoadSampleTypographyResources());
            return;
        }

        MarkdownThemes.OverrideTypographyResources(app, SelectedTypographyTheme?.Key);
    }

    private void ToggleIncrementalStress()
    {
        if (IsIncrementalStressRunning)
        {
            StopIncrementalStress();
            return;
        }

        IsIncrementalStressRunning = true;
        ApplyIncrementalStressTick();
        _incrementalStressTimer.Start();
    }

    private void StopIncrementalStress()
    {
        if (!IsIncrementalStressRunning)
        {
            return;
        }

        _incrementalStressTimer.Stop();
        IsIncrementalStressRunning = false;
    }

    private void ApplyIncrementalStressTick()
    {
        _incrementalStressTick++;
        Markdown = UpsertIncrementalStressSection(Markdown, _incrementalStressTick);
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

    private static string UpsertIncrementalStressSection(string markdown, int tick)
    {
        var section = BuildIncrementalStressSection(tick);
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return section;
        }

        var start = markdown.IndexOf(IncrementalStressHeading, StringComparison.Ordinal);
        if (start < 0)
        {
            var firstLineEnd = markdown.IndexOf('\n');
            var insertIndex = FindNextHeading(markdown, firstLineEnd < 0 ? markdown.Length : firstLineEnd + 1);
            return insertIndex < markdown.Length
                ? CombineMarkdown(markdown[..insertIndex], section, markdown[insertIndex..])
                : CombineMarkdown(markdown, section, string.Empty);
        }

        var end = FindNextHeading(markdown, start + IncrementalStressHeading.Length);
        return CombineMarkdown(markdown[..start], section, markdown[end..]);
    }

    private static string BuildIncrementalStressSection(int tick)
    {
        var phase = (tick % 4) switch
        {
            0 => "append",
            1 => "replace",
            2 => "table",
            _ => "code"
        };
        var token = new string((char)('A' + tick % 26), 12 + tick % 18);

        return $$"""
               {{IncrementalStressHeading}}

               当前轮次：{{tick}}，阶段：{{phase}}，自动输入片段：{{token}}。

               - 动态列表项：第 {{tick}} 次增量刷新
               - 长文本换行：CodeWFMarkdownIncrementalStress{{tick:0000}}-{{token}}-viewer-live-render-update

               ```csharp
               var tick = {{tick}};
               var phase = "{{phase}}";
               Console.WriteLine($"incremental {tick} / {phase}");
               ```

               | 项 | 值 |
               | --- | --- |
               | tick | {{tick}} |
               | phase | {{phase}} |
               """;
    }

    private static int FindNextHeading(string markdown, int startIndex)
    {
        var index = Math.Clamp(startIndex, 0, markdown.Length);
        if (index > 0)
        {
            var nextLine = markdown.IndexOf('\n', index - 1);
            if (nextLine < 0)
            {
                return markdown.Length;
            }

            index = nextLine + 1;
        }

        while (index < markdown.Length)
        {
            var lineStart = index;
            var lineEnd = markdown.IndexOf('\n', lineStart);
            if (lineEnd < 0)
            {
                lineEnd = markdown.Length;
            }

            var line = markdown[lineStart..lineEnd].TrimStart();
            if (line.StartsWith("# ", StringComparison.Ordinal) || line.StartsWith("## ", StringComparison.Ordinal))
            {
                return lineStart;
            }

            index = lineEnd + 1;
        }

        return markdown.Length;
    }

    private static string CombineMarkdown(string before, string section, string after)
    {
        var builder = new List<string>
        {
            before.TrimEnd(),
            section.Trim(),
            after.TrimStart()
        };

        return string.Join(Environment.NewLine + Environment.NewLine, builder.Where(part => part.Length > 0));
    }

    private static ResourceDictionary LoadSampleTypographyResources()
    {
        return new SampleTypographyThemeResources();
    }

    private static string ResolveMarkdownBasePath()
    {
        var outputPath = Path.Combine(AppContext.BaseDirectory, "MarkdownSamples");
        if (Directory.Exists(outputPath))
        {
            return outputPath;
        }

        var sourcePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "MarkdownSamples"));
        return Directory.Exists(sourcePath) ? sourcePath : outputPath;
    }
}

public sealed record ThemeVariantOption(string Name, ThemeVariant ThemeVariant);

public sealed record MarkdownSampleFile(string Name, string Path);

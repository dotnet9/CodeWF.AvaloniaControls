using System.Globalization;

using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Immutable;

using CodeWF.Markdown;

using TextMateSharp.Grammars;
using TextMateSharp.Registry;
using TextMateSharp.Themes;

using AvaloniaFontStyle = Avalonia.Media.FontStyle;
using TextMateFontStyle = TextMateSharp.Themes.FontStyle;

namespace CodeWF.Markdown.Rendering;

internal static class CodeHighlighter
{
    private static readonly Dictionary<(string Language, ThemeName Theme), (IGrammar? Grammar, Theme Theme)> Cache = new();

    public static Control Render(
        string code,
        string language,
        bool isDark,
        FontFamily fontFamily,
        double fontSize,
        double lineHeight)
    {
        var themeName = isDark ? ThemeName.DarkPlus : ThemeName.LightPlus;
        var (grammar, theme) = GetOrCreateGrammar(NormalizeLanguage(language), themeName);

        var textBlock = new SelectableTextBlock
        {
            Inlines = new InlineCollection(),
            TextWrapping = TextWrapping.NoWrap,
            FontFamily = fontFamily,
            FontSize = fontSize,
            LineHeight = lineHeight
        };
        textBlock.Classes.Add(MarkdownStyleKeys.CodeBlockText);
        TextOptions.SetBaselinePixelAlignment(textBlock, BaselinePixelAlignment.Aligned);
        textBlock.ContextMenu = CreateCopyContextMenu(textBlock);

        if (grammar is null)
        {
            textBlock.Inlines.Add(new Run(code) { Foreground = isDark ? Brushes.White : Brushes.Black });
            return Wrap(textBlock);
        }

        IStateStack? ruleStack = null;
        foreach (var line in code.Replace("\r\n", "\n").Split('\n'))
        {
            var result = grammar.TokenizeLine(line, ruleStack, TimeSpan.FromSeconds(2));
            ruleStack = result.RuleStack;

            foreach (var token in result.Tokens)
            {
                var start = Math.Min(token.StartIndex, line.Length);
                var end = Math.Min(token.EndIndex, line.Length);
                if (end <= start)
                {
                    continue;
                }

                var run = CreateRun(line[start..end], token.Scopes, theme, isDark);
                textBlock.Inlines.Add(run);
            }

            textBlock.Inlines.Add(new LineBreak());
        }

        return Wrap(textBlock);
    }

    private static Control Wrap(Control content)
    {
        var scrollViewer = new ScrollViewer
        {
            Content = content,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
        };
        scrollViewer.Classes.Add(MarkdownStyleKeys.CodeBlockScrollViewer);
        return scrollViewer;
    }

    private static ContextMenu CreateCopyContextMenu(SelectableTextBlock textBlock)
    {
        var copySelectionItem = new MenuItem { Header = "复制选中文本" };
        copySelectionItem.Click += (_, _) => textBlock.Copy();
        return new ContextMenu { ItemsSource = new[] { copySelectionItem } };
    }

    private static (IGrammar? Grammar, Theme Theme) GetOrCreateGrammar(string language, ThemeName themeName)
    {
        var key = (language, themeName);
        if (Cache.TryGetValue(key, out var cached))
        {
            return cached;
        }

        var options = new RegistryOptions(themeName);
        var registry = new Registry(options);
        var theme = registry.GetTheme();
        var scope = options.GetScopeByLanguageId(language);
        var grammar = string.IsNullOrWhiteSpace(scope) ? null : registry.LoadGrammar(scope);
        grammar ??= registry.LoadGrammar(options.GetScopeByLanguageId("log"));

        var value = (grammar, theme);
        Cache[key] = value;
        return value;
    }

    private static Run CreateRun(string text, IEnumerable<string> scopes, Theme theme, bool isDark)
    {
        var foregroundId = -1;
        var backgroundId = -1;
        var fontStyle = TextMateFontStyle.NotSet;

        foreach (var rule in theme.Match(scopes.ToList()))
        {
            if (foregroundId == -1 && rule.foreground > 0)
            {
                foregroundId = rule.foreground;
            }

            if (backgroundId == -1 && rule.background > 0)
            {
                backgroundId = rule.background;
            }

            if (rule.fontStyle > 0)
            {
                fontStyle |= (TextMateFontStyle)rule.fontStyle;
            }
        }

        var run = new Run
        {
            Text = text,
            Foreground = foregroundId == -1
                ? (isDark ? Brushes.White : Brushes.Black)
                : new ImmutableSolidColorBrush(ParseColor(theme.GetColor(foregroundId))),
            Background = backgroundId == -1
                ? null
                : new ImmutableSolidColorBrush(ParseColor(theme.GetColor(backgroundId)))
        };

        if ((fontStyle & TextMateFontStyle.Bold) != 0)
        {
            run.FontWeight = FontWeight.Bold;
        }

        if ((fontStyle & TextMateFontStyle.Italic) != 0)
        {
            run.FontStyle = AvaloniaFontStyle.Italic;
        }

        if ((fontStyle & TextMateFontStyle.Underline) != 0)
        {
            run.TextDecorations =
            [
                new TextDecoration
                {
                    Location = TextDecorationLocation.Underline,
                    StrokeThicknessUnit = TextDecorationUnit.Pixel
                }
            ];
        }

        return run;
    }

    private static string NormalizeLanguage(string? language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return "text";
        }

        return language.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant() switch
        {
            "csharp" => "c#",
            "cs" => "c#",
            "js" => "javascript",
            "ts" => "typescript",
            "ps" => "powershell",
            "pwsh" => "powershell",
            var value => value
        };
    }

    private static Color ParseColor(string hex)
    {
        var value = hex.TrimStart('#');
        if (value.Length == 8)
        {
            return Color.FromArgb(
                byte.Parse(value[..2], NumberStyles.HexNumber),
                byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber),
                byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber),
                byte.Parse(value.Substring(6, 2), NumberStyles.HexNumber));
        }

        return Color.FromRgb(
            byte.Parse(value[..2], NumberStyles.HexNumber),
            byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber),
            byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber));
    }
}

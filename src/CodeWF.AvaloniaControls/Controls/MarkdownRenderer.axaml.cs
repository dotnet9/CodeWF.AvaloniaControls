using Avalonia;
using Avalonia.Controls;
using CodeWF.AvaloniaControls.Extensions;
using Markdig;
using Markdig.Syntax;

namespace CodeWF.AvaloniaControls.Controls;

public partial class MarkdownRenderer : UserControl
{
    #region Properties

    public static readonly StyledProperty<string> MarkdownProperty =
        AvaloniaProperty.Register<MarkdownRenderer, string>(nameof(Markdown));

    public string Markdown
    {
        get => GetValue(MarkdownProperty);
        set => SetValue(MarkdownProperty, value);
    }

    public static readonly StyledProperty<RangeObservableCollection<object>?> ItemsSourceProperty =
        AvaloniaProperty.Register<MarkdownRenderer, RangeObservableCollection<object>?>(
            nameof(ItemsSource));

    public RangeObservableCollection<object>? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    #endregion

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property.Name == nameof(Markdown))
        {
            ParseMarkdownToBlocks(Markdown);
        }
    }

    private void ParseMarkdownToBlocks(string markdownText)
    {
        ItemsSource ??= new RangeObservableCollection<object>();
        ItemsSource.Clear();
        var pipeline = new MarkdownPipelineBuilder().Build();
        var document = Markdig.Markdown.Parse(markdownText, pipeline);
        foreach (var block in document)
        {
            if (block is HeadingBlock heading)
            {
                switch (heading.Level)
                {
                    case 1:
                        ItemsSource.Add(new H1Block { Text = heading.Inline?.FirstChild?.ToString() });
                        break;
                    case 2:
                        ItemsSource.Add(new H2Block { Text = heading.Inline?.FirstChild?.ToString() });
                        break;
                    case 3:
                        ItemsSource.Add(new H3Block { Text = heading.Inline?.FirstChild?.ToString() });
                        break;
                }
            }
            else if (block is ParagraphBlock paragraph)
            {
                ItemsSource.Add(new MdParagraphBlock { Text = paragraph.Inline?.FirstChild?.ToString() });
            }
        }
    }
}

public interface IBlock
{
}

public class H1Block : IBlock
{
    public string? Text { get; set; }
}

public class H2Block : IBlock
{
    public string? Text { get; set; }
}

public class H3Block : IBlock
{
    public string? Text { get; set; }
}

public class ListItemBlock : IBlock
{
    public string? Text { get; set; }
}

public class MdParagraphBlock : IBlock
{
    public string? Text { get; set; }
}
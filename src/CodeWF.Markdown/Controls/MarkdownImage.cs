using System.Net.Http;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace CodeWF.Markdown.Controls;

[TemplatePart(ContentHostPartName, typeof(ContentControl), IsRequired = true)]
public class MarkdownImage : TemplatedControl
{
    private const string ContentHostPartName = "PART_ContentHost";

    private static readonly HttpClient HttpClient = new();

    private ContentControl? _contentHost;
    private Bitmap? _bitmap;
    private byte[]? _imageBytes;
    private string? _fileName;
    private long _loadVersion;

    public static readonly StyledProperty<string?> SourceProperty =
        AvaloniaProperty.Register<MarkdownImage, string?>(nameof(Source));

    public static readonly StyledProperty<string?> AltTextProperty =
        AvaloniaProperty.Register<MarkdownImage, string?>(nameof(AltText));

    public static readonly StyledProperty<string?> BasePathProperty =
        AvaloniaProperty.Register<MarkdownImage, string?>(nameof(BasePath));

    public string? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public string? AltText
    {
        get => GetValue(AltTextProperty);
        set => SetValue(AltTextProperty, value);
    }

    public string? BasePath
    {
        get => GetValue(BasePathProperty);
        set => SetValue(BasePathProperty, value);
    }

    static MarkdownImage()
    {
        SourceProperty.Changed.AddClassHandler<MarkdownImage>((image, _) => image.QueueLoad());
        BasePathProperty.Changed.AddClassHandler<MarkdownImage>((image, _) => image.QueueLoad());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _contentHost = e.NameScope.Find<ContentControl>(ContentHostPartName);
        QueueLoad();
    }

    private void QueueLoad()
    {
        var source = Source;
        if (string.IsNullOrWhiteSpace(source))
        {
            _bitmap = null;
            _imageBytes = null;
            _fileName = null;
            SetContent(null);
            return;
        }

        var version = Interlocked.Increment(ref _loadVersion);
        _ = LoadAsync(source, version);
    }

    private async Task LoadAsync(string source, long version)
    {
        try
        {
            if (source.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            {
                await ShowFallbackAsync(source, version, "SVG 图片需要应用侧接入 SVG 渲染控件");
                return;
            }

            var bytes = await LoadBytesAsync(source);
            using var stream = new MemoryStream(bytes);
            var bitmap = new Bitmap(stream);
            var fileName = ResolveFileName(source);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (version != _loadVersion)
                {
                    return;
                }

                // 预览窗体和另存为需要复用当前图片，避免再次下载远程资源。
                _bitmap = bitmap;
                _imageBytes = bytes;
                _fileName = fileName;

                var image = new Image
                {
                    Source = bitmap,
                    Stretch = Stretch.Uniform,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Cursor = new Cursor(StandardCursorType.Hand)
                };
                image.Classes.Add(MarkdownStyleKeys.ImageContent);
                image.PointerReleased += OnImagePointerReleased;
                SetContent(image);
            });
        }
        catch
        {
            await ShowFallbackAsync(source, version, "图片加载失败");
        }
    }

    private async Task<byte[]> LoadBytesAsync(string source)
    {
        if (IsDataUri(source))
        {
            var index = source.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
            return Convert.FromBase64String(source[(index + "base64,".Length)..]);
        }

        if (Uri.TryCreate(source, UriKind.Absolute, out var uri))
        {
            if (uri.Scheme is "http" or "https")
            {
                return await HttpClient.GetByteArrayAsync(uri);
            }

            if (uri.IsFile)
            {
                return await File.ReadAllBytesAsync(uri.LocalPath);
            }
        }

        var localPath = ResolveLocalPath(source);
        return await File.ReadAllBytesAsync(localPath);
    }

    private string ResolveLocalPath(string source)
    {
        if (Path.IsPathRooted(source))
        {
            return source;
        }

        var basePath = string.IsNullOrWhiteSpace(BasePath) ? AppContext.BaseDirectory : BasePath!;
        return Path.GetFullPath(Path.Combine(basePath, source.Replace('/', Path.DirectorySeparatorChar)));
    }

    private static bool IsDataUri(string source)
    {
        return source.StartsWith("data:image", StringComparison.OrdinalIgnoreCase)
               && source.Contains("base64,", StringComparison.OrdinalIgnoreCase);
    }

    private void OnImagePointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton == MouseButton.Left)
        {
            OpenPreviewWindow();
            e.Handled = true;
        }
    }

    private void OpenPreviewWindow()
    {
        if (_bitmap is null || _imageBytes is null)
        {
            return;
        }

        var window = new MarkdownImagePreviewWindow(_bitmap, _imageBytes, _fileName ?? "markdown-image.png", AltText);
        if (TopLevel.GetTopLevel(this) is Window owner)
        {
            window.Show(owner);
        }
        else
        {
            window.Show();
        }
    }

    private static string ResolveFileName(string source)
    {
        var fileName = "markdown-image";
        if (IsDataUri(source))
        {
            fileName += ResolveDataUriExtension(source);
        }
        else if (Uri.TryCreate(source, UriKind.Absolute, out var uri))
        {
            fileName = Path.GetFileName(uri.IsFile ? uri.LocalPath : uri.LocalPath);
        }
        else
        {
            fileName = Path.GetFileName(source);
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = "markdown-image.png";
        }
        else if (string.IsNullOrWhiteSpace(Path.GetExtension(fileName)))
        {
            fileName += ".png";
        }

        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalidChar, '_');
        }

        return fileName;
    }

    private static string ResolveDataUriExtension(string source)
    {
        var separatorIndex = source.IndexOf(';');
        if (separatorIndex <= "data:image/".Length)
        {
            return ".png";
        }

        return source["data:image/".Length..separatorIndex].ToLowerInvariant() switch
        {
            "jpeg" => ".jpg",
            "jpg" => ".jpg",
            "webp" => ".webp",
            "bmp" => ".bmp",
            "gif" => ".gif",
            _ => ".png"
        };
    }

    private async Task ShowFallbackAsync(string source, long version, string message)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (version != _loadVersion)
            {
                return;
            }

            _bitmap = null;
            _imageBytes = null;
            _fileName = null;

            var fallback = new Border
            {
                Child = new TextBlock
                {
                    Text = $"{message}: {AltText ?? source}",
                    TextWrapping = TextWrapping.Wrap
                }
            };
            fallback.Classes.Add(MarkdownStyleKeys.ImageFallback);
            if (fallback.Child is TextBlock textBlock)
            {
                textBlock.Classes.Add(MarkdownStyleKeys.ImageFallbackText);
            }

            SetContent(fallback);
        });
    }

    private void SetContent(Control? content)
    {
        if (_contentHost is not null)
        {
            _contentHost.Content = content;
            _contentHost.InvalidateMeasure();
        }

        InvalidateMeasure();
        InvalidateArrange();
    }
}

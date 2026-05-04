using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace CodeWF.Markdown.Controls;

internal sealed class MarkdownImagePreviewWindow : Window
{
    private const double MinZoom = 0.1;
    private const double MaxZoom = 8.0;
    private const double ZoomStep = 1.2;

    private readonly byte[] _imageBytes;
    private readonly string _fileName;
    private readonly Image _image;
    private readonly ScrollViewer _scrollViewer;
    private readonly TextBlock _zoomText;
    private readonly double _originalWidth;
    private readonly double _originalHeight;
    private double _zoom = 1.0;

    public MarkdownImagePreviewWindow(Bitmap bitmap, byte[] imageBytes, string fileName, string? title)
    {
        _imageBytes = imageBytes;
        _fileName = string.IsNullOrWhiteSpace(fileName) ? "markdown-image.png" : fileName;
        _originalWidth = Math.Max(1, bitmap.PixelSize.Width);
        _originalHeight = Math.Max(1, bitmap.PixelSize.Height);

        Title = string.IsNullOrWhiteSpace(title) ? "图片预览" : title;
        Classes.Add(MarkdownStyleKeys.ImagePreviewWindow);
        WindowStartupLocation = WindowStartupLocation.CenterOwner;

        _zoomText = new TextBlock
        {
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        _zoomText.Classes.Add(MarkdownStyleKeys.ImagePreviewZoomText);

        _image = new Image
        {
            Source = bitmap,
            Stretch = Stretch.Fill,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        var imageHost = new Border
        {
            Child = _image
        };
        imageHost.Classes.Add(MarkdownStyleKeys.ImagePreviewContent);

        _scrollViewer = new ScrollViewer
        {
            Content = imageHost,
            HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
            VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto
        };

        Content = CreateLayout();
        PointerWheelChanged += OnPointerWheelChanged;
        UpdateImageSize();
    }

    private Control CreateLayout()
    {
        var root = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            }
        };

        var toolbar = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        toolbar.Classes.Add(MarkdownStyleKeys.ImagePreviewToolbar);

        toolbar.Children.Add(CreateButton("缩小", () => SetZoom(_zoom / ZoomStep)));
        toolbar.Children.Add(CreateButton("放大", () => SetZoom(_zoom * ZoomStep)));
        toolbar.Children.Add(CreateButton("原始大小", () => SetZoom(1.0)));
        toolbar.Children.Add(CreateButton("适应窗口", FitToWindow));
        toolbar.Children.Add(CreateButton("另存为", async () => await SaveAsAsync()));
        toolbar.Children.Add(_zoomText);

        Grid.SetRow(toolbar, 0);
        Grid.SetRow(_scrollViewer, 1);
        root.Children.Add(toolbar);
        root.Children.Add(_scrollViewer);
        return root;
    }

    private static Button CreateButton(string text, Action action)
    {
        var button = new Button
        {
            Content = text,
            VerticalAlignment = VerticalAlignment.Center
        };
        button.Classes.Add(MarkdownStyleKeys.ImagePreviewButton);
        button.Click += (_, _) => action();
        return button;
    }

    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if ((e.KeyModifiers & KeyModifiers.Control) == 0)
        {
            return;
        }

        SetZoom(e.Delta.Y > 0 ? _zoom * ZoomStep : _zoom / ZoomStep);
        e.Handled = true;
    }

    private void FitToWindow()
    {
        var padding = (_scrollViewer.Content as Border)?.Padding ?? default;
        var availableWidth = Math.Max(1, _scrollViewer.Bounds.Width - padding.Left - padding.Right);
        var availableHeight = Math.Max(1, _scrollViewer.Bounds.Height - padding.Top - padding.Bottom);
        SetZoom(Math.Min(availableWidth / _originalWidth, availableHeight / _originalHeight));
    }

    private void SetZoom(double zoom)
    {
        _zoom = Math.Clamp(zoom, MinZoom, MaxZoom);
        UpdateImageSize();
    }

    private void UpdateImageSize()
    {
        _image.Width = _originalWidth * _zoom;
        _image.Height = _originalHeight * _zoom;
        _zoomText.Text = $"{_zoom:P0}";
    }

    private async Task SaveAsAsync()
    {
        if (!StorageProvider.CanSave)
        {
            return;
        }

        var extension = Path.GetExtension(_fileName).TrimStart('.');
        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = "png";
        }

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            SuggestedFileName = _fileName,
            DefaultExtension = extension,
            ShowOverwritePrompt = true,
            FileTypeChoices =
            [
                new FilePickerFileType("图片")
                {
                    Patterns = ["*.png", "*.jpg", "*.jpeg", "*.webp", "*.bmp", "*.gif"],
                    MimeTypes = ["image/png", "image/jpeg", "image/webp", "image/bmp", "image/gif"]
                }
            ]
        });

        if (file is null)
        {
            return;
        }

        // 另存为写出原始字节，避免重新编码导致格式或质量变化。
        await using var stream = await file.OpenWriteAsync();
        await stream.WriteAsync(_imageBytes);
    }
}

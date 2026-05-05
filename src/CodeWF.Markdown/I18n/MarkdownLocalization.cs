using System.Globalization;

using Lang.Avalonia;

namespace CodeWF.Markdown.I18n;

internal static class MarkdownLocalization
{
    internal const string Copy = "CodeWF.Markdown.Copy";
    internal const string CopyRenderedText = "CodeWF.Markdown.CopyRenderedText";
    internal const string CopySelectedText = "CodeWF.Markdown.CopySelectedText";
    internal const string ImageFileNotFound = "CodeWF.Markdown.ImageFileNotFound";
    internal const string ImageLoadFailed = "CodeWF.Markdown.ImageLoadFailed";
    internal const string ImagePreviewTitle = "CodeWF.Markdown.ImagePreviewTitle";
    internal const string ImagePreviewZoomOut = "CodeWF.Markdown.ImagePreviewZoomOut";
    internal const string ImagePreviewZoomIn = "CodeWF.Markdown.ImagePreviewZoomIn";
    internal const string ImagePreviewActualSize = "CodeWF.Markdown.ImagePreviewActualSize";
    internal const string ImagePreviewFit = "CodeWF.Markdown.ImagePreviewFit";
    internal const string ImagePreviewRotateLeft = "CodeWF.Markdown.ImagePreviewRotateLeft";
    internal const string ImagePreviewRotateRight = "CodeWF.Markdown.ImagePreviewRotateRight";
    internal const string ImagePreviewSaveAs = "CodeWF.Markdown.ImagePreviewSaveAs";
    internal const string ImagePreviewImagesFileType = "CodeWF.Markdown.ImagePreviewImagesFileType";
    internal const string ImagePreviewZoomStatus = "CodeWF.Markdown.ImagePreviewZoomStatus";

    private static readonly Dictionary<string, string> Fallbacks = new()
    {
        [Copy] = "Copy",
        [CopyRenderedText] = "Copy rendered text",
        [CopySelectedText] = "Copy selected text",
        [ImageFileNotFound] = "File not found: {0}",
        [ImageLoadFailed] = "Image failed to load: {0}",
        [ImagePreviewTitle] = "Image Preview",
        [ImagePreviewZoomOut] = "Zoom -",
        [ImagePreviewZoomIn] = "Zoom +",
        [ImagePreviewActualSize] = "1:1",
        [ImagePreviewFit] = "Fit",
        [ImagePreviewRotateLeft] = "Rotate L",
        [ImagePreviewRotateRight] = "Rotate R",
        [ImagePreviewSaveAs] = "Save As",
        [ImagePreviewImagesFileType] = "Images",
        [ImagePreviewZoomStatus] = "{0:P0} / {1} deg"
    };

    internal static string Get(string key)
    {
        var value = I18nManager.Instance.GetResource(key);
        if (!string.IsNullOrWhiteSpace(value) && !string.Equals(value, key, StringComparison.Ordinal))
        {
            return value;
        }

        return Fallbacks.TryGetValue(key, out var fallback) ? fallback : key;
    }

    internal static string Format(string key, params object?[] args)
    {
        return string.Format(I18nManager.Instance.Culture ?? CultureInfo.CurrentUICulture, Get(key), args);
    }
}

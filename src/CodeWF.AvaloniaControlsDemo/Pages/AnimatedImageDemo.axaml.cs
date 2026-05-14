using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AnimatedImage.Avalonia;
using Avalonia;
using Avalonia.Controls;
using CodeWF.AvaloniaControlsDemo.Services;
using Lang.Avalonia;
using PageLangs = Showcase.Pages.AnimatedImageDemo;

namespace CodeWF.AvaloniaControlsDemo.Pages;

public partial class AnimatedImageDemo : UserControl
{
    private const string LocalGifUri = "avares://CodeWF.AvaloniaControlsDemo/Assets/nice.gif";
    private const string NetworkGifUri = "https://img1.dotnet9.com/2026/05/nice.gif";

    private static readonly HttpClient GifHttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    private NetworkStatusKind _networkStatusKind = NetworkStatusKind.None;
    private CancellationTokenSource? _networkGifLoadCts;
    private MemoryStream? _networkGifStream;

    public AnimatedImageDemo()
    {
        InitializeComponent();
        I18nManager.Instance.CultureChanged += (_, _) => UpdateNetworkStatusText();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (_networkGifLoadCts is not null)
        {
            return;
        }

        _networkGifLoadCts = new CancellationTokenSource();
        _ = LoadNetworkGifAsync(_networkGifLoadCts.Token);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        _networkGifLoadCts?.Cancel();
        _networkGifLoadCts?.Dispose();
        _networkGifLoadCts = null;

        _networkGifStream?.Dispose();
        _networkGifStream = null;

        base.OnDetachedFromVisualTree(e);
    }

    private async Task LoadNetworkGifAsync(CancellationToken cancellationToken)
    {
        try
        {
            _networkStatusKind = NetworkStatusKind.Loading;
            NetworkStatusPanel.IsVisible = true;
            UpdateNetworkStatusText();

            var bytes = await GifHttpClient.GetByteArrayAsync(NetworkGifUri, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            var stream = new MemoryStream(bytes);
            _networkGifStream?.Dispose();
            _networkGifStream = stream;

            ImageBehavior.SetAnimatedSource(NetworkImage, new AnimatedImageSourceStream(stream));
            _networkStatusKind = NetworkStatusKind.None;
            NetworkStatusPanel.IsVisible = false;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch
        {
            UseLocalGifFallback();
        }
    }

    private void UseLocalGifFallback()
    {
        _networkGifStream?.Dispose();
        _networkGifStream = null;

        ImageBehavior.SetAnimatedSource(NetworkImage, new AnimatedImageSourceUri(new Uri(LocalGifUri)));
        _networkStatusKind = NetworkStatusKind.Fallback;
        UpdateNetworkStatusText();
        NetworkStatusPanel.IsVisible = true;
    }

    private void UpdateNetworkStatusText()
    {
        NetworkStatusText.Text = _networkStatusKind switch
        {
            NetworkStatusKind.Loading => LocalizationService.Get(PageLangs.LoadingNetworkGif),
            NetworkStatusKind.Fallback => LocalizationService.Get(PageLangs.NetworkFallback),
            _ => string.Empty
        };
    }

    private enum NetworkStatusKind
    {
        None,
        Loading,
        Fallback
    }
}

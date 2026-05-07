using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AnimatedImage.Avalonia;
using Avalonia;
using Avalonia.Controls;

namespace CodeWF.AvaloniaControls.Showcase.Pages;

public partial class AnimatedImageDemo : UserControl
{
    private const string LocalGifUri = "avares://CodeWF.AvaloniaControls.Showcase/Assets/nice.gif";
    private const string NetworkGifUri = "https://img1.dotnet9.com/2026/05/nice.gif";

    private static readonly HttpClient GifHttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    private CancellationTokenSource? _networkGifLoadCts;
    private MemoryStream? _networkGifStream;

    public AnimatedImageDemo()
    {
        InitializeComponent();
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
            NetworkStatusPanel.IsVisible = true;
            NetworkStatusText.Text = "正在加载网络 GIF...";

            var bytes = await GifHttpClient.GetByteArrayAsync(NetworkGifUri, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            var stream = new MemoryStream(bytes);
            _networkGifStream?.Dispose();
            _networkGifStream = stream;

            ImageBehavior.SetAnimatedSource(NetworkImage, new AnimatedImageSourceStream(stream));
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
        NetworkStatusText.Text = "网络 GIF 加载失败，已显示本地 GIF。";
        NetworkStatusPanel.IsVisible = true;
    }
}

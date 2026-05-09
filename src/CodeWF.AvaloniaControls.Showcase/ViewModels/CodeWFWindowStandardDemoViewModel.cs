using Avalonia;
using ReactiveUI;

namespace CodeWF.AvaloniaControls.Showcase.ViewModels;

public sealed class CodeWFWindowStandardDemoViewModel : ReactiveObject
{
    private bool _isFullScreenButtonVisible;
    private bool _isMinimizeButtonVisible = true;
    private bool _isRestoreButtonVisible = true;
    private bool _isCloseButtonVisible = true;
    private bool _isTitleBarVisible = true;
    private bool _isManagedResizerVisible = true;
    private bool _canResize = true;
    private bool _canMaximize = true;
    private bool _showTitleBarContent;
    private bool _applyTitleBarMargin;

    public bool IsFullScreenButtonVisible
    {
        get => _isFullScreenButtonVisible;
        set => this.RaiseAndSetIfChanged(ref _isFullScreenButtonVisible, value);
    }

    public bool IsMinimizeButtonVisible
    {
        get => _isMinimizeButtonVisible;
        set => this.RaiseAndSetIfChanged(ref _isMinimizeButtonVisible, value);
    }

    public bool IsRestoreButtonVisible
    {
        get => _isRestoreButtonVisible;
        set => this.RaiseAndSetIfChanged(ref _isRestoreButtonVisible, value);
    }

    public bool IsCloseButtonVisible
    {
        get => _isCloseButtonVisible;
        set => this.RaiseAndSetIfChanged(ref _isCloseButtonVisible, value);
    }

    public bool IsTitleBarVisible
    {
        get => _isTitleBarVisible;
        set => this.RaiseAndSetIfChanged(ref _isTitleBarVisible, value);
    }

    public bool IsManagedResizerVisible
    {
        get => _isManagedResizerVisible;
        set => this.RaiseAndSetIfChanged(ref _isManagedResizerVisible, value);
    }

    public bool CanResize
    {
        get => _canResize;
        set => this.RaiseAndSetIfChanged(ref _canResize, value);
    }

    public bool CanMaximize
    {
        get => _canMaximize;
        set => this.RaiseAndSetIfChanged(ref _canMaximize, value);
    }

    public bool ShowTitleBarContent
    {
        get => _showTitleBarContent;
        set
        {
            this.RaiseAndSetIfChanged(ref _showTitleBarContent, value);
            this.RaisePropertyChanged(nameof(TitleBarContent));
        }
    }

    public bool ApplyTitleBarMargin
    {
        get => _applyTitleBarMargin;
        set
        {
            this.RaiseAndSetIfChanged(ref _applyTitleBarMargin, value);
            this.RaisePropertyChanged(nameof(TitleBarMargin));
        }
    }

    public string? TitleBarContent => ShowTitleBarContent ? "TitleBarContent" : null;

    public Thickness TitleBarMargin => ApplyTitleBarMargin ? new Thickness(10, 8, 10, 0) : default;
}

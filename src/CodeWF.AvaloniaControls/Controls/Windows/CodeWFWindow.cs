using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace CodeWF.AvaloniaControls.Controls;

public class CodeWFWindow : Window
{
    public static readonly StyledProperty<bool> IsFullScreenButtonVisibleProperty =
        AvaloniaProperty.Register<CodeWFWindow, bool>(nameof(IsFullScreenButtonVisible));

    public static readonly StyledProperty<bool> IsMinimizeButtonVisibleProperty =
        AvaloniaProperty.Register<CodeWFWindow, bool>(nameof(IsMinimizeButtonVisible), true);

    public static readonly StyledProperty<bool> IsRestoreButtonVisibleProperty =
        AvaloniaProperty.Register<CodeWFWindow, bool>(nameof(IsRestoreButtonVisible), true);

    public static readonly StyledProperty<bool> IsCloseButtonVisibleProperty =
        AvaloniaProperty.Register<CodeWFWindow, bool>(nameof(IsCloseButtonVisible), true);

    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty =
        AvaloniaProperty.Register<CodeWFWindow, bool>(nameof(IsTitleBarVisible), true);

    public static readonly StyledProperty<bool> IsManagedResizerVisibleProperty =
        AvaloniaProperty.Register<CodeWFWindow, bool>(nameof(IsManagedResizerVisible), true);

    public static readonly StyledProperty<bool> ContentExtendsIntoTitleBarProperty =
        AvaloniaProperty.Register<CodeWFWindow, bool>(nameof(ContentExtendsIntoTitleBar));

    public static readonly StyledProperty<object?> LeftContentProperty =
        AvaloniaProperty.Register<CodeWFWindow, object?>(nameof(LeftContent));

    public static readonly StyledProperty<IDataTemplate?> LeftContentTemplateProperty =
        AvaloniaProperty.Register<CodeWFWindow, IDataTemplate?>(nameof(LeftContentTemplate));

    public static readonly StyledProperty<object?> RightContentProperty =
        AvaloniaProperty.Register<CodeWFWindow, object?>(nameof(RightContent));

    public static readonly StyledProperty<IDataTemplate?> RightContentTemplateProperty =
        AvaloniaProperty.Register<CodeWFWindow, IDataTemplate?>(nameof(RightContentTemplate));

    public static readonly StyledProperty<object?> TitleBarContentProperty =
        AvaloniaProperty.Register<CodeWFWindow, object?>(nameof(TitleBarContent));

    public static readonly StyledProperty<IDataTemplate?> TitleBarContentTemplateProperty =
        AvaloniaProperty.Register<CodeWFWindow, IDataTemplate?>(nameof(TitleBarContentTemplate));

    public static readonly StyledProperty<Thickness> TitleBarMarginProperty =
        AvaloniaProperty.Register<CodeWFWindow, Thickness>(nameof(TitleBarMargin));

    public static readonly StyledProperty<double> TitleBarHeightProperty =
        AvaloniaProperty.Register<CodeWFWindow, double>(nameof(TitleBarHeight), 32);

    public static readonly StyledProperty<IBrush?> TitleBarBackgroundProperty =
        AvaloniaProperty.Register<CodeWFWindow, IBrush?>(nameof(TitleBarBackground));

    public static readonly StyledProperty<IBrush?> TitleBarForegroundProperty =
        AvaloniaProperty.Register<CodeWFWindow, IBrush?>(nameof(TitleBarForeground));

    public static readonly StyledProperty<IBrush?> TitleBarBorderBrushProperty =
        AvaloniaProperty.Register<CodeWFWindow, IBrush?>(nameof(TitleBarBorderBrush));

    public static readonly StyledProperty<Thickness> TitleBarBorderThicknessProperty =
        AvaloniaProperty.Register<CodeWFWindow, Thickness>(nameof(TitleBarBorderThickness), new Thickness(0, 0, 0, 1));

    public static readonly StyledProperty<CornerRadius> WindowCornerRadiusProperty =
        AvaloniaProperty.Register<CodeWFWindow, CornerRadius>(nameof(WindowCornerRadius), new CornerRadius(4));

    private readonly Dictionary<Control, WindowEdge> _resizeGrips = new();
    private Button? _closeButton;
    private Button? _fullScreenButton;
    private Button? _maximizeRestoreButton;
    private Button? _minimizeButton;
    private InputElement? _titleBar;

    public CodeWFWindow()
    {
        WindowDecorations = WindowDecorations.None;
        ExtendClientAreaToDecorationsHint = false;
        TransparencyLevelHint = [WindowTransparencyLevel.Transparent];
        TransparencyBackgroundFallback = Brushes.Transparent;
    }

    public bool IsFullScreenButtonVisible
    {
        get => GetValue(IsFullScreenButtonVisibleProperty);
        set => SetValue(IsFullScreenButtonVisibleProperty, value);
    }

    public bool IsMinimizeButtonVisible
    {
        get => GetValue(IsMinimizeButtonVisibleProperty);
        set => SetValue(IsMinimizeButtonVisibleProperty, value);
    }

    public bool IsRestoreButtonVisible
    {
        get => GetValue(IsRestoreButtonVisibleProperty);
        set => SetValue(IsRestoreButtonVisibleProperty, value);
    }

    public bool IsCloseButtonVisible
    {
        get => GetValue(IsCloseButtonVisibleProperty);
        set => SetValue(IsCloseButtonVisibleProperty, value);
    }

    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }

    public bool IsManagedResizerVisible
    {
        get => GetValue(IsManagedResizerVisibleProperty);
        set => SetValue(IsManagedResizerVisibleProperty, value);
    }

    public bool ContentExtendsIntoTitleBar
    {
        get => GetValue(ContentExtendsIntoTitleBarProperty);
        set => SetValue(ContentExtendsIntoTitleBarProperty, value);
    }

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public IDataTemplate? LeftContentTemplate
    {
        get => GetValue(LeftContentTemplateProperty);
        set => SetValue(LeftContentTemplateProperty, value);
    }

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public IDataTemplate? RightContentTemplate
    {
        get => GetValue(RightContentTemplateProperty);
        set => SetValue(RightContentTemplateProperty, value);
    }

    public object? TitleBarContent
    {
        get => GetValue(TitleBarContentProperty);
        set => SetValue(TitleBarContentProperty, value);
    }

    public IDataTemplate? TitleBarContentTemplate
    {
        get => GetValue(TitleBarContentTemplateProperty);
        set => SetValue(TitleBarContentTemplateProperty, value);
    }

    public Thickness TitleBarMargin
    {
        get => GetValue(TitleBarMarginProperty);
        set => SetValue(TitleBarMarginProperty, value);
    }

    public double TitleBarHeight
    {
        get => GetValue(TitleBarHeightProperty);
        set => SetValue(TitleBarHeightProperty, value);
    }

    public IBrush? TitleBarBackground
    {
        get => GetValue(TitleBarBackgroundProperty);
        set => SetValue(TitleBarBackgroundProperty, value);
    }

    public IBrush? TitleBarForeground
    {
        get => GetValue(TitleBarForegroundProperty);
        set => SetValue(TitleBarForegroundProperty, value);
    }

    public IBrush? TitleBarBorderBrush
    {
        get => GetValue(TitleBarBorderBrushProperty);
        set => SetValue(TitleBarBorderBrushProperty, value);
    }

    public Thickness TitleBarBorderThickness
    {
        get => GetValue(TitleBarBorderThicknessProperty);
        set => SetValue(TitleBarBorderThicknessProperty, value);
    }

    public CornerRadius WindowCornerRadius
    {
        get => GetValue(WindowCornerRadiusProperty);
        set => SetValue(WindowCornerRadiusProperty, value);
    }

    protected override Type StyleKeyOverride => typeof(CodeWFWindow);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachTemplateEvents();

        _titleBar = e.NameScope.Find<InputElement>("PART_TitleBar");
        _fullScreenButton = e.NameScope.Find<Button>("PART_FullScreenButton");
        _minimizeButton = e.NameScope.Find<Button>("PART_MinimizeButton");
        _maximizeRestoreButton = e.NameScope.Find<Button>("PART_MaximizeRestoreButton");
        _closeButton = e.NameScope.Find<Button>("PART_CloseButton");

        if (_titleBar is not null) _titleBar.PointerPressed += TitleBar_OnPointerPressed;

        if (_fullScreenButton is not null) _fullScreenButton.Click += FullScreenButton_OnClick;

        if (_minimizeButton is not null) _minimizeButton.Click += MinimizeButton_OnClick;

        if (_maximizeRestoreButton is not null) _maximizeRestoreButton.Click += MaximizeRestoreButton_OnClick;

        if (_closeButton is not null) _closeButton.Click += CloseButton_OnClick;

        AttachResizeGrip(e, "PART_ResizeTopLeft", WindowEdge.NorthWest);
        AttachResizeGrip(e, "PART_ResizeTop", WindowEdge.North);
        AttachResizeGrip(e, "PART_ResizeTopRight", WindowEdge.NorthEast);
        AttachResizeGrip(e, "PART_ResizeLeft", WindowEdge.West);
        AttachResizeGrip(e, "PART_ResizeRight", WindowEdge.East);
        AttachResizeGrip(e, "PART_ResizeBottomLeft", WindowEdge.SouthWest);
        AttachResizeGrip(e, "PART_ResizeBottom", WindowEdge.South);
        AttachResizeGrip(e, "PART_ResizeBottomRight", WindowEdge.SouthEast);

        UpdateWindowStatePseudoClasses();
        UpdateContentLayoutPseudoClasses();
        UpdateButtonStates();
        UpdateResizeGripStates();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == WindowStateProperty) UpdateWindowStatePseudoClasses();

        if (change.Property == ContentExtendsIntoTitleBarProperty) UpdateContentLayoutPseudoClasses();

        if (change.Property == WindowStateProperty
            || change.Property == CanResizeProperty
            || change.Property == CanMaximizeProperty)
        {
            UpdateButtonStates();
            UpdateResizeGripStates();
        }

        if (change.Property == IsManagedResizerVisibleProperty) UpdateResizeGripStates();
    }

    private void AttachResizeGrip(TemplateAppliedEventArgs e, string name, WindowEdge edge)
    {
        var grip = e.NameScope.Find<Control>(name);
        if (grip is null) return;

        _resizeGrips[grip] = edge;
        grip.PointerPressed += ResizeGrip_OnPointerPressed;
    }

    private void DetachTemplateEvents()
    {
        if (_titleBar is not null) _titleBar.PointerPressed -= TitleBar_OnPointerPressed;

        if (_fullScreenButton is not null) _fullScreenButton.Click -= FullScreenButton_OnClick;

        if (_minimizeButton is not null) _minimizeButton.Click -= MinimizeButton_OnClick;

        if (_maximizeRestoreButton is not null) _maximizeRestoreButton.Click -= MaximizeRestoreButton_OnClick;

        if (_closeButton is not null) _closeButton.Click -= CloseButton_OnClick;

        foreach (var grip in _resizeGrips.Keys) grip.PointerPressed -= ResizeGrip_OnPointerPressed;

        _resizeGrips.Clear();
    }

    private void TitleBar_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed || IsFromFocusableControl(e.Source)) return;

        if (e.ClickCount == 2 && CanResize && CanMaximize && WindowState != WindowState.FullScreen)
        {
            ToggleMaximizeRestore();
            e.Handled = true;
            return;
        }

        BeginMoveDrag(e);
        e.Handled = true;
    }

    private void ResizeGrip_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!IsManagedResizerVisible
            || !CanResize
            || WindowState == WindowState.Maximized
            || WindowState == WindowState.FullScreen
            || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed
            || sender is not Control grip
            || !_resizeGrips.TryGetValue(grip, out var edge))
            return;

        BeginResizeDrag(edge, e);
        e.Handled = true;
    }

    private void FullScreenButton_OnClick(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.FullScreen
            ? WindowState.Normal
            : WindowState.FullScreen;
    }

    private void MinimizeButton_OnClick(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeRestoreButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (CanResize && CanMaximize && WindowState != WindowState.FullScreen) ToggleMaximizeRestore();
    }

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ToggleMaximizeRestore()
    {
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void UpdateWindowStatePseudoClasses()
    {
        PseudoClasses.Set(":maximized", WindowState == WindowState.Maximized);
        PseudoClasses.Set(":normal", WindowState == WindowState.Normal);
        PseudoClasses.Set(":fullscreen", WindowState == WindowState.FullScreen);
    }

    private void UpdateContentLayoutPseudoClasses()
    {
        PseudoClasses.Set(":content-extends-into-title-bar", ContentExtendsIntoTitleBar);
    }

    private void UpdateButtonStates()
    {
        if (_maximizeRestoreButton is not null)
            _maximizeRestoreButton.IsEnabled = CanResize && CanMaximize && WindowState != WindowState.FullScreen;
    }

    private void UpdateResizeGripStates()
    {
        var showResizeGrips = IsManagedResizerVisible
                              && CanResize
                              && WindowState != WindowState.Maximized
                              && WindowState != WindowState.FullScreen;

        foreach (var grip in _resizeGrips.Keys) grip.IsVisible = showResizeGrips;
    }

    private bool IsFromFocusableControl(object? source)
    {
        if (source is not Visual sourceVisual) return false;

        var visual = sourceVisual;
        while (visual is not null && visual != _titleBar)
        {
            if (visual is Button || visual is ToggleButton || visual is Control { Focusable: true }) return true;

            visual = visual.GetVisualParent();
        }

        return false;
    }
}
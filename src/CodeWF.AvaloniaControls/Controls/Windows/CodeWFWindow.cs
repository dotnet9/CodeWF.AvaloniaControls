using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace CodeWF.AvaloniaControls.Controls;

public class CodeWFWindow : Window
{
    private readonly Dictionary<Control, WindowEdge> _resizeGrips = new();
    private InputElement? _titleBar;
    private Button? _minimizeButton;
    private Button? _maximizeRestoreButton;
    private Button? _closeButton;

    public static readonly StyledProperty<object?> LeftContentProperty =
        AvaloniaProperty.Register<CodeWFWindow, object?>(nameof(LeftContent));

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> LeftContentTemplateProperty =
        AvaloniaProperty.Register<CodeWFWindow, IDataTemplate?>(nameof(LeftContentTemplate));

    public IDataTemplate? LeftContentTemplate
    {
        get => GetValue(LeftContentTemplateProperty);
        set => SetValue(LeftContentTemplateProperty, value);
    }

    public static readonly StyledProperty<object?> RightContentProperty =
        AvaloniaProperty.Register<CodeWFWindow, object?>(nameof(RightContent));

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> RightContentTemplateProperty =
        AvaloniaProperty.Register<CodeWFWindow, IDataTemplate?>(nameof(RightContentTemplate));

    public IDataTemplate? RightContentTemplate
    {
        get => GetValue(RightContentTemplateProperty);
        set => SetValue(RightContentTemplateProperty, value);
    }

    public static readonly StyledProperty<double> TitleBarHeightProperty =
        AvaloniaProperty.Register<CodeWFWindow, double>(nameof(TitleBarHeight), 60);

    public double TitleBarHeight
    {
        get => GetValue(TitleBarHeightProperty);
        set => SetValue(TitleBarHeightProperty, value);
    }

    public static readonly StyledProperty<IBrush?> TitleBarBackgroundProperty =
        AvaloniaProperty.Register<CodeWFWindow, IBrush?>(nameof(TitleBarBackground));

    public IBrush? TitleBarBackground
    {
        get => GetValue(TitleBarBackgroundProperty);
        set => SetValue(TitleBarBackgroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> TitleBarForegroundProperty =
        AvaloniaProperty.Register<CodeWFWindow, IBrush?>(nameof(TitleBarForeground));

    public IBrush? TitleBarForeground
    {
        get => GetValue(TitleBarForegroundProperty);
        set => SetValue(TitleBarForegroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> TitleBarBorderBrushProperty =
        AvaloniaProperty.Register<CodeWFWindow, IBrush?>(nameof(TitleBarBorderBrush));

    public IBrush? TitleBarBorderBrush
    {
        get => GetValue(TitleBarBorderBrushProperty);
        set => SetValue(TitleBarBorderBrushProperty, value);
    }

    public static readonly StyledProperty<CornerRadius> WindowCornerRadiusProperty =
        AvaloniaProperty.Register<CodeWFWindow, CornerRadius>(nameof(WindowCornerRadius), new CornerRadius(4));

    public CornerRadius WindowCornerRadius
    {
        get => GetValue(WindowCornerRadiusProperty);
        set => SetValue(WindowCornerRadiusProperty, value);
    }

    protected override Type StyleKeyOverride => typeof(CodeWFWindow);

    public CodeWFWindow()
    {
        WindowDecorations = WindowDecorations.None;
        ExtendClientAreaToDecorationsHint = false;
        TransparencyLevelHint = [WindowTransparencyLevel.Transparent];
        TransparencyBackgroundFallback = Brushes.Transparent;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachTemplateEvents();

        _titleBar = e.NameScope.Find<InputElement>("PART_TitleBar");
        _minimizeButton = e.NameScope.Find<Button>("PART_MinimizeButton");
        _maximizeRestoreButton = e.NameScope.Find<Button>("PART_MaximizeRestoreButton");
        _closeButton = e.NameScope.Find<Button>("PART_CloseButton");

        if (_titleBar is not null)
        {
            _titleBar.PointerPressed += TitleBar_OnPointerPressed;
        }

        if (_minimizeButton is not null)
        {
            _minimizeButton.Click += MinimizeButton_OnClick;
        }

        if (_maximizeRestoreButton is not null)
        {
            _maximizeRestoreButton.Click += MaximizeRestoreButton_OnClick;
        }

        if (_closeButton is not null)
        {
            _closeButton.Click += CloseButton_OnClick;
        }

        AttachResizeGrip(e, "PART_ResizeTopLeft", WindowEdge.NorthWest);
        AttachResizeGrip(e, "PART_ResizeTop", WindowEdge.North);
        AttachResizeGrip(e, "PART_ResizeTopRight", WindowEdge.NorthEast);
        AttachResizeGrip(e, "PART_ResizeLeft", WindowEdge.West);
        AttachResizeGrip(e, "PART_ResizeRight", WindowEdge.East);
        AttachResizeGrip(e, "PART_ResizeBottomLeft", WindowEdge.SouthWest);
        AttachResizeGrip(e, "PART_ResizeBottom", WindowEdge.South);
        AttachResizeGrip(e, "PART_ResizeBottomRight", WindowEdge.SouthEast);

        UpdateWindowStatePseudoClasses();
        UpdateButtonStates();
        UpdateResizeGripStates();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == WindowStateProperty)
        {
            UpdateWindowStatePseudoClasses();
        }

        if (change.Property == WindowStateProperty || change.Property == CanResizeProperty)
        {
            UpdateButtonStates();
            UpdateResizeGripStates();
        }
    }

    private void AttachResizeGrip(TemplateAppliedEventArgs e, string name, WindowEdge edge)
    {
        var grip = e.NameScope.Find<Control>(name);
        if (grip is null)
        {
            return;
        }

        _resizeGrips[grip] = edge;
        grip.PointerPressed += ResizeGrip_OnPointerPressed;
    }

    private void DetachTemplateEvents()
    {
        if (_titleBar is not null)
        {
            _titleBar.PointerPressed -= TitleBar_OnPointerPressed;
        }

        if (_minimizeButton is not null)
        {
            _minimizeButton.Click -= MinimizeButton_OnClick;
        }

        if (_maximizeRestoreButton is not null)
        {
            _maximizeRestoreButton.Click -= MaximizeRestoreButton_OnClick;
        }

        if (_closeButton is not null)
        {
            _closeButton.Click -= CloseButton_OnClick;
        }

        foreach (var grip in _resizeGrips.Keys)
        {
            grip.PointerPressed -= ResizeGrip_OnPointerPressed;
        }

        _resizeGrips.Clear();
    }

    private void TitleBar_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed || IsFromFocusableControl(e.Source))
        {
            return;
        }

        if (e.ClickCount == 2 && CanResize)
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
        if (!CanResize
            || WindowState == WindowState.Maximized
            || WindowState == WindowState.FullScreen
            || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed
            || sender is not Control grip
            || !_resizeGrips.TryGetValue(grip, out var edge))
        {
            return;
        }

        BeginResizeDrag(edge, e);
        e.Handled = true;
    }

    private void MinimizeButton_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeRestoreButton_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (CanResize)
        {
            ToggleMaximizeRestore();
        }
    }

    private void CloseButton_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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

    private void UpdateButtonStates()
    {
        if (_maximizeRestoreButton is not null)
        {
            _maximizeRestoreButton.IsEnabled = CanResize;
        }
    }

    private void UpdateResizeGripStates()
    {
        var showResizeGrips = CanResize
            && WindowState != WindowState.Maximized
            && WindowState != WindowState.FullScreen;

        foreach (var grip in _resizeGrips.Keys)
        {
            grip.IsVisible = showResizeGrips;
        }
    }

    private bool IsFromFocusableControl(object? source)
    {
        if (source is not Visual sourceVisual)
        {
            return false;
        }

        Visual? visual = sourceVisual;
        while (visual is not null && visual != _titleBar)
        {
            if (visual is Button || visual is ToggleButton || visual is Control { Focusable: true })
            {
                return true;
            }

            visual = visual.GetVisualParent();
        }

        return false;
    }
}

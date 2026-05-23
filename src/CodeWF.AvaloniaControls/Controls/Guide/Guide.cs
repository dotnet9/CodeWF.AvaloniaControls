using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace CodeWF.AvaloniaControls.Controls;

public class Guide : TemplatedControl
{
    private const int MaxTargetResolveAttempts = 8;

    private readonly DispatcherTimer _targetResolveTimer;
    private readonly List<IGuideStepOption> _activeSteps = [];
    private Popup? _popup;
    private Popup? _maskPopup;
    private Popup? _targetMaskPopup;
    private GuideOverlay? _overlay;
    private GuideOverlay? _targetOverlay;
    private Border? _arrow;
    private Control? _cardRoot;
    private ItemsControl? _customActionsHost;
    private Button? _previousButton;
    private Button? _nextButton;
    private Button? _finishButton;
    private Button? _closeButton;
    private Control? _observedTarget;
    private TopLevel? _observedTopLevel;
    private INotifyCollectionChanged? _observedStepsSource;
    private IInputElement? _restoreFocusElement;
    private bool _isReallyOpen;
    private bool _ignoreOpenChange;
    private bool _layoutRefreshQueued;
    private bool _suppressNavigationClick;
    private int _pendingStepOpenedIndex = -1;
    private int _targetResolveAttempt;

    public static readonly StyledProperty<IEnumerable<IGuideStepOption>?> StepsSourceProperty =
        AvaloniaProperty.Register<Guide, IEnumerable<IGuideStepOption>?>(nameof(StepsSource));

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<Guide, bool>(nameof(IsOpen));

    public static readonly StyledProperty<GuidePlacementMode> PlacementProperty =
        AvaloniaProperty.Register<Guide, GuidePlacementMode>(nameof(Placement), GuidePlacementMode.Bottom);

    public static readonly StyledProperty<GuideStyleType> StyleTypeProperty =
        AvaloniaProperty.Register<Guide, GuideStyleType>(nameof(StyleType));

    public static readonly StyledProperty<bool> IsShowMaskProperty =
        AvaloniaProperty.Register<Guide, bool>(nameof(IsShowMask), true);

    public static readonly StyledProperty<bool> IsArrowVisibleProperty =
        AvaloniaProperty.Register<Guide, bool>(nameof(IsArrowVisible), true);

    public static readonly StyledProperty<bool> IsPointAtCenterProperty =
        AvaloniaProperty.Register<Guide, bool>(nameof(IsPointAtCenter));

    public static readonly StyledProperty<bool> IsScrollIntoViewProperty =
        AvaloniaProperty.Register<Guide, bool>(nameof(IsScrollIntoView), true);

    public static readonly StyledProperty<IBrush?> MaskColorProperty =
        AvaloniaProperty.Register<Guide, IBrush?>(nameof(MaskColor), new SolidColorBrush(Color.FromArgb(122, 0, 0, 0)));

    public static readonly StyledProperty<double> GapOffsetXProperty =
        AvaloniaProperty.Register<Guide, double>(nameof(GapOffsetX), 8);

    public static readonly StyledProperty<double> GapOffsetYProperty =
        AvaloniaProperty.Register<Guide, double>(nameof(GapOffsetY), 8);

    public static readonly StyledProperty<double> GapRadiusProperty =
        AvaloniaProperty.Register<Guide, double>(nameof(GapRadius), 6);

    public static readonly StyledProperty<double> PopupOffsetProperty =
        AvaloniaProperty.Register<Guide, double>(nameof(PopupOffset), 12);

    public static readonly StyledProperty<TimeSpan> TargetResolveDelayProperty =
        AvaloniaProperty.Register<Guide, TimeSpan>(nameof(TargetResolveDelay), TimeSpan.FromMilliseconds(80));

    public static readonly StyledProperty<GuideMissingTargetBehavior> MissingTargetBehaviorProperty =
        AvaloniaProperty.Register<Guide, GuideMissingTargetBehavior>(nameof(MissingTargetBehavior));

    public static readonly StyledProperty<string?> PreviousTextProperty =
        AvaloniaProperty.Register<Guide, string?>(nameof(PreviousText), "上一步");

    public static readonly StyledProperty<string?> NextTextProperty =
        AvaloniaProperty.Register<Guide, string?>(nameof(NextText), "下一步");

    public static readonly StyledProperty<string?> FinishTextProperty =
        AvaloniaProperty.Register<Guide, string?>(nameof(FinishText), "完成");

    public static readonly StyledProperty<string?> CloseTextProperty =
        AvaloniaProperty.Register<Guide, string?>(nameof(CloseText), "关闭");

    public static readonly StyledProperty<GuideIndicator?> IndicatorProperty =
        AvaloniaProperty.Register<Guide, GuideIndicator?>(nameof(Indicator));

    public static readonly DirectProperty<Guide, int> CurrentIndexProperty =
        AvaloniaProperty.RegisterDirect<Guide, int>(
            nameof(CurrentIndex),
            guide => guide.CurrentIndex,
            (guide, value) => guide.CurrentIndex = value,
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public static readonly DirectProperty<Guide, int> StepCountProperty =
        AvaloniaProperty.RegisterDirect<Guide, int>(nameof(StepCount), guide => guide.StepCount);

    public static readonly DirectProperty<Guide, string?> CurrentTitleProperty =
        AvaloniaProperty.RegisterDirect<Guide, string?>(nameof(CurrentTitle), guide => guide.CurrentTitle);

    public static readonly DirectProperty<Guide, string?> CurrentDescriptionProperty =
        AvaloniaProperty.RegisterDirect<Guide, string?>(nameof(CurrentDescription), guide => guide.CurrentDescription);

    public static readonly DirectProperty<Guide, object?> CurrentCoverProperty =
        AvaloniaProperty.RegisterDirect<Guide, object?>(nameof(CurrentCover), guide => guide.CurrentCover);

    public static readonly DirectProperty<Guide, bool> HasCurrentTitleProperty =
        AvaloniaProperty.RegisterDirect<Guide, bool>(nameof(HasCurrentTitle), guide => guide.HasCurrentTitle);

    public static readonly DirectProperty<Guide, bool> HasCurrentDescriptionProperty =
        AvaloniaProperty.RegisterDirect<Guide, bool>(nameof(HasCurrentDescription), guide => guide.HasCurrentDescription);

    public static readonly DirectProperty<Guide, bool> HasCurrentCoverProperty =
        AvaloniaProperty.RegisterDirect<Guide, bool>(nameof(HasCurrentCover), guide => guide.HasCurrentCover);

    public static readonly DirectProperty<Guide, bool> CanGoPreviousProperty =
        AvaloniaProperty.RegisterDirect<Guide, bool>(nameof(CanGoPrevious), guide => guide.CanGoPrevious);

    public static readonly DirectProperty<Guide, bool> CanGoNextProperty =
        AvaloniaProperty.RegisterDirect<Guide, bool>(nameof(CanGoNext), guide => guide.CanGoNext);

    public static readonly DirectProperty<Guide, bool> IsLastStepProperty =
        AvaloniaProperty.RegisterDirect<Guide, bool>(nameof(IsLastStep), guide => guide.IsLastStep);

    public static readonly DirectProperty<Guide, bool> TargetRegionVisibleProperty =
        AvaloniaProperty.RegisterDirect<Guide, bool>(nameof(TargetRegionVisible), guide => guide.TargetRegionVisible);

    public static readonly DirectProperty<Guide, Rect> TargetRegionProperty =
        AvaloniaProperty.RegisterDirect<Guide, Rect>(nameof(TargetRegion), guide => guide.TargetRegion);

    public static readonly DirectProperty<Guide, IBrush?> CurrentMaskColorProperty =
        AvaloniaProperty.RegisterDirect<Guide, IBrush?>(nameof(CurrentMaskColor), guide => guide.CurrentMaskColor);

    public static readonly DirectProperty<Guide, bool> CurrentIsShowMaskProperty =
        AvaloniaProperty.RegisterDirect<Guide, bool>(nameof(CurrentIsShowMask), guide => guide.CurrentIsShowMask);

    public static readonly DirectProperty<Guide, bool> CurrentIsArrowVisibleProperty =
        AvaloniaProperty.RegisterDirect<Guide, bool>(nameof(CurrentIsArrowVisible), guide => guide.CurrentIsArrowVisible);

    public static readonly DirectProperty<Guide, double> CurrentGapRadiusProperty =
        AvaloniaProperty.RegisterDirect<Guide, double>(nameof(CurrentGapRadius), guide => guide.CurrentGapRadius);

    public static readonly DirectProperty<Guide, GuideStyleType> CurrentStyleTypeProperty =
        AvaloniaProperty.RegisterDirect<Guide, GuideStyleType>(nameof(CurrentStyleType), guide => guide.CurrentStyleType);

    private int _currentIndex = -1;
    private int _stepCount;
    private string? _currentTitle;
    private string? _currentDescription;
    private object? _currentCover;
    private bool _hasCurrentTitle;
    private bool _hasCurrentDescription;
    private bool _hasCurrentCover;
    private bool _canGoPrevious;
    private bool _canGoNext;
    private bool _isLastStep;
    private bool _targetRegionVisible;
    private Rect _targetRegion;
    private IBrush? _currentMaskColor;
    private bool _currentIsShowMask;
    private bool _currentIsArrowVisible;
    private double _currentGapRadius;
    private GuideStyleType _currentStyleType;

    static Guide()
    {
        StepsSourceProperty.Changed.AddClassHandler<Guide>((guide, _) => guide.ConfigureStepsSource());
    }

    public Guide()
    {
        Steps.CollectionChanged += (_, _) => RefreshStepCollection();
        CustomActions.CollectionChanged += (_, _) => SyncCustomActionsHost();

        _targetResolveTimer = new DispatcherTimer();
        _targetResolveTimer.Tick += (_, _) =>
        {
            _targetResolveTimer.Stop();
            ApplyCurrentStep();
        };
    }

    public event EventHandler<GuideStepEventArgs>? StepOpening;

    public event EventHandler<GuideStepEventArgs>? StepOpened;

    public event EventHandler? Completed;

    public event EventHandler? Closed;

    public IEnumerable<IGuideStepOption>? StepsSource
    {
        get => GetValue(StepsSourceProperty);
        set => SetValue(StepsSourceProperty, value);
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public GuidePlacementMode Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public GuideStyleType StyleType
    {
        get => GetValue(StyleTypeProperty);
        set => SetValue(StyleTypeProperty, value);
    }

    public bool IsShowMask
    {
        get => GetValue(IsShowMaskProperty);
        set => SetValue(IsShowMaskProperty, value);
    }

    public bool IsArrowVisible
    {
        get => GetValue(IsArrowVisibleProperty);
        set => SetValue(IsArrowVisibleProperty, value);
    }

    public bool IsPointAtCenter
    {
        get => GetValue(IsPointAtCenterProperty);
        set => SetValue(IsPointAtCenterProperty, value);
    }

    public bool IsScrollIntoView
    {
        get => GetValue(IsScrollIntoViewProperty);
        set => SetValue(IsScrollIntoViewProperty, value);
    }

    public IBrush? MaskColor
    {
        get => GetValue(MaskColorProperty);
        set => SetValue(MaskColorProperty, value);
    }

    public double GapOffsetX
    {
        get => GetValue(GapOffsetXProperty);
        set => SetValue(GapOffsetXProperty, value);
    }

    public double GapOffsetY
    {
        get => GetValue(GapOffsetYProperty);
        set => SetValue(GapOffsetYProperty, value);
    }

    public double GapRadius
    {
        get => GetValue(GapRadiusProperty);
        set => SetValue(GapRadiusProperty, value);
    }

    public double PopupOffset
    {
        get => GetValue(PopupOffsetProperty);
        set => SetValue(PopupOffsetProperty, value);
    }

    public TimeSpan TargetResolveDelay
    {
        get => GetValue(TargetResolveDelayProperty);
        set => SetValue(TargetResolveDelayProperty, value);
    }

    public GuideMissingTargetBehavior MissingTargetBehavior
    {
        get => GetValue(MissingTargetBehaviorProperty);
        set => SetValue(MissingTargetBehaviorProperty, value);
    }

    public string? PreviousText
    {
        get => GetValue(PreviousTextProperty);
        set => SetValue(PreviousTextProperty, value);
    }

    public string? NextText
    {
        get => GetValue(NextTextProperty);
        set => SetValue(NextTextProperty, value);
    }

    public string? FinishText
    {
        get => GetValue(FinishTextProperty);
        set => SetValue(FinishTextProperty, value);
    }

    public string? CloseText
    {
        get => GetValue(CloseTextProperty);
        set => SetValue(CloseTextProperty, value);
    }

    public GuideIndicator? Indicator
    {
        get => GetValue(IndicatorProperty);
        set => SetValue(IndicatorProperty, value);
    }

    public int CurrentIndex
    {
        get => _currentIndex;
        set => SetAndRaise(CurrentIndexProperty, ref _currentIndex, value);
    }

    public int StepCount
    {
        get => _stepCount;
        private set => SetAndRaise(StepCountProperty, ref _stepCount, value);
    }

    public string? CurrentTitle
    {
        get => _currentTitle;
        private set => SetAndRaise(CurrentTitleProperty, ref _currentTitle, value);
    }

    public string? CurrentDescription
    {
        get => _currentDescription;
        private set => SetAndRaise(CurrentDescriptionProperty, ref _currentDescription, value);
    }

    public object? CurrentCover
    {
        get => _currentCover;
        private set => SetAndRaise(CurrentCoverProperty, ref _currentCover, value);
    }

    public bool HasCurrentTitle
    {
        get => _hasCurrentTitle;
        private set => SetAndRaise(HasCurrentTitleProperty, ref _hasCurrentTitle, value);
    }

    public bool HasCurrentDescription
    {
        get => _hasCurrentDescription;
        private set => SetAndRaise(HasCurrentDescriptionProperty, ref _hasCurrentDescription, value);
    }

    public bool HasCurrentCover
    {
        get => _hasCurrentCover;
        private set => SetAndRaise(HasCurrentCoverProperty, ref _hasCurrentCover, value);
    }

    public bool CanGoPrevious
    {
        get => _canGoPrevious;
        private set => SetAndRaise(CanGoPreviousProperty, ref _canGoPrevious, value);
    }

    public bool CanGoNext
    {
        get => _canGoNext;
        private set => SetAndRaise(CanGoNextProperty, ref _canGoNext, value);
    }

    public bool IsLastStep
    {
        get => _isLastStep;
        private set => SetAndRaise(IsLastStepProperty, ref _isLastStep, value);
    }

    public bool TargetRegionVisible
    {
        get => _targetRegionVisible;
        private set => SetAndRaise(TargetRegionVisibleProperty, ref _targetRegionVisible, value);
    }

    public Rect TargetRegion
    {
        get => _targetRegion;
        private set => SetAndRaise(TargetRegionProperty, ref _targetRegion, value);
    }

    public IBrush? CurrentMaskColor
    {
        get => _currentMaskColor;
        private set => SetAndRaise(CurrentMaskColorProperty, ref _currentMaskColor, value);
    }

    public bool CurrentIsShowMask
    {
        get => _currentIsShowMask;
        private set => SetAndRaise(CurrentIsShowMaskProperty, ref _currentIsShowMask, value);
    }

    public bool CurrentIsArrowVisible
    {
        get => _currentIsArrowVisible;
        private set => SetAndRaise(CurrentIsArrowVisibleProperty, ref _currentIsArrowVisible, value);
    }

    public double CurrentGapRadius
    {
        get => _currentGapRadius;
        private set => SetAndRaise(CurrentGapRadiusProperty, ref _currentGapRadius, value);
    }

    public GuideStyleType CurrentStyleType
    {
        get => _currentStyleType;
        private set => SetAndRaise(CurrentStyleTypeProperty, ref _currentStyleType, value);
    }

    [Content]
    public AvaloniaList<GuideStep> Steps { get; } = [];

    public AvaloniaList<Control> CustomActions { get; } = [];

    public void Show()
    {
        SetCurrentValue(IsOpenProperty, true);
    }

    public void Close()
    {
        HideGuideCore(true);
    }

    public void ResetState()
    {
        SetCurrentValue(CurrentIndexProperty, StepCount > 0 ? 0 : -1);
        SetCurrentValue(IsOpenProperty, false);
    }

    public void GoTo(int index)
    {
        if (index < 0 || index >= StepCount)
        {
            return;
        }

        SetCurrentValue(CurrentIndexProperty, index);
    }

    public void GoPrevious()
    {
        if (CurrentIndex > 0)
        {
            SetCurrentValue(CurrentIndexProperty, CurrentIndex - 1);
        }
    }

    public void GoNext()
    {
        if (CurrentIndex < StepCount - 1)
        {
            SetCurrentValue(CurrentIndexProperty, CurrentIndex + 1);
            return;
        }

        Completed?.Invoke(this, EventArgs.Empty);
        HideGuideCore(false);
    }

    public void Refresh()
    {
        if (_isReallyOpen)
        {
            ApplyCurrentStep();
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Indicator is null)
        {
            SetCurrentValue(IndicatorProperty, new DefaultGuideIndicator());
        }

        RefreshStepCollection();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        DetachTemplateEvents();

        base.OnApplyTemplate(e);

        _popup = e.NameScope.Find<Popup>("PART_Popup");
        _maskPopup = e.NameScope.Find<Popup>("PART_MaskPopup");
        _targetMaskPopup = e.NameScope.Find<Popup>("PART_TargetMaskPopup");
        _overlay = e.NameScope.Find<GuideOverlay>("PART_Overlay");
        _targetOverlay = e.NameScope.Find<GuideOverlay>("PART_TargetOverlay");
        _arrow = e.NameScope.Find<Border>("PART_Arrow");
        _cardRoot = e.NameScope.Find<Control>("PART_CardRoot");
        _customActionsHost = e.NameScope.Find<ItemsControl>("PART_CustomActionsHost");

        AttachTemplateEvents(e.NameScope);
        SyncCustomActionsHost();
        SyncIndicator();

        if (IsOpen)
        {
            ShowGuideCore();
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (IsOpen)
        {
            ShowGuideCore();
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _targetResolveTimer.Stop();
        DetachTemplateEvents();
        DetachObservedTarget();
        DetachObservedTopLevel();
        DetachStepsSource();
        ClosePopups();
        _isReallyOpen = false;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsOpenProperty && !_ignoreOpenChange)
        {
            if (change.GetNewValue<bool>())
            {
                ShowGuideCore();
            }
            else
            {
                HideGuideCore(true);
            }
        }
        else if (change.Property == CurrentIndexProperty)
        {
            UpdateNavigationState();
            SyncIndicator();

            if (_isReallyOpen)
            {
                ActivateCurrentStep();
            }
            else
            {
                UpdateCurrentStepContent(GetCurrentStep());
            }
        }
        else if (change.Property == IndicatorProperty)
        {
            SyncIndicator();
        }
        else if (_isReallyOpen && (
                     change.Property == PlacementProperty ||
                     change.Property == StyleTypeProperty ||
                     change.Property == IsShowMaskProperty ||
                     change.Property == IsArrowVisibleProperty ||
                     change.Property == IsPointAtCenterProperty ||
                     change.Property == IsScrollIntoViewProperty ||
                     change.Property == MaskColorProperty ||
                     change.Property == GapOffsetXProperty ||
                     change.Property == GapOffsetYProperty ||
                     change.Property == GapRadiusProperty ||
                     change.Property == PopupOffsetProperty))
        {
            ApplyCurrentStep();
        }
    }

    private void ShowGuideCore()
    {
        if (_isReallyOpen || _popup is null || StepCount == 0)
        {
            if (StepCount == 0)
            {
                SetIsOpenSilently(false);
            }
            return;
        }

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
        {
            return;
        }

        _restoreFocusElement = topLevel.FocusManager?.GetFocusedElement();
        AttachObservedTopLevel(topLevel);

        if (CurrentIndex < 0 || CurrentIndex >= StepCount)
        {
            SetCurrentValue(CurrentIndexProperty, 0);
        }

        _isReallyOpen = true;
        SetIsOpenSilently(true);
        ActivateCurrentStep();
    }

    private void HideGuideCore(bool raiseClosed)
    {
        if (!_isReallyOpen && !IsOpen)
        {
            return;
        }

        _targetResolveTimer.Stop();
        ClosePopups();
        DetachObservedTarget();
        DetachObservedTopLevel();
        _isReallyOpen = false;
        SetIsOpenSilently(false);

        if (_restoreFocusElement is not null)
        {
            var restoreFocus = _restoreFocusElement;
            Dispatcher.UIThread.Post(() => restoreFocus.Focus(NavigationMethod.Unspecified));
            _restoreFocusElement = null;
        }

        if (raiseClosed)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ActivateCurrentStep()
    {
        var step = GetCurrentStep();
        if (step is null)
        {
            HideGuideCore(true);
            return;
        }

        BeginStepTransition();

        var args = new GuideStepEventArgs(CurrentIndex, step);
        ExecuteOpeningCommand(step);
        StepOpening?.Invoke(this, args);

        if (args.Cancel)
        {
            HideGuideCore(true);
            return;
        }

        _pendingStepOpenedIndex = CurrentIndex;
        ScheduleApplyCurrentStep();
    }

    private void BeginStepTransition()
    {
        _targetResolveAttempt = 0;

        if (_popup is not null)
        {
            _popup.IsOpen = false;
        }

        CloseOverlayPopup(_maskPopup);
        CloseOverlayPopup(_targetMaskPopup);

        if (_arrow is not null)
        {
            _arrow.IsVisible = false;
        }

        TargetRegion = default;
        TargetRegionVisible = false;
    }

    private void ScheduleApplyCurrentStep()
    {
        _targetResolveTimer.Stop();
        _targetResolveTimer.Interval = TargetResolveDelay <= TimeSpan.Zero
            ? TimeSpan.FromMilliseconds(1)
            : TargetResolveDelay;
        _targetResolveTimer.Start();
    }

    private void ApplyCurrentStep()
    {
        if (!_isReallyOpen || _popup is null)
        {
            return;
        }

        var step = GetCurrentStep();
        if (step is null)
        {
            HideGuideCore(true);
            return;
        }

        UpdateCurrentStepContent(step);

        var target = ResolveTarget(step, out var missingTarget);
        if (missingTarget && DeferMissingTarget(step))
        {
            return;
        }

        if (missingTarget && HandleMissingTarget())
        {
            return;
        }

        _targetResolveAttempt = 0;
        ConfigureCurrentStepValues(step, target);
        ConfigureOverlay(target, step);
        ConfigurePopup(target);
        AttachObservedTarget(target);
        SyncIndicator();

        if (_pendingStepOpenedIndex == CurrentIndex)
        {
            _pendingStepOpenedIndex = -1;
            StepOpened?.Invoke(this, new GuideStepEventArgs(CurrentIndex, step));
        }
    }

    private bool HandleMissingTarget()
    {
        if (MissingTargetBehavior == GuideMissingTargetBehavior.Close)
        {
            HideGuideCore(true);
            return true;
        }

        if (MissingTargetBehavior == GuideMissingTargetBehavior.Skip)
        {
            GoNext();
            return true;
        }

        return false;
    }

    private bool DeferMissingTarget(IGuideStepOption step)
    {
        if (step.Target is null ||
            MissingTargetBehavior != GuideMissingTargetBehavior.Center ||
            _targetResolveAttempt >= MaxTargetResolveAttempts)
        {
            return false;
        }

        _targetResolveAttempt++;
        ScheduleApplyCurrentStep();
        return true;
    }

    private Control? ResolveTarget(IGuideStepOption step, out bool missingTarget)
    {
        missingTarget = false;
        var target = step.Target;

        if (target is null)
        {
            return null;
        }

        if (!target.IsVisible || !target.IsAttachedToVisualTree() || target.Bounds.Width <= 0 || target.Bounds.Height <= 0)
        {
            missingTarget = true;
            return null;
        }

        if (step.IsScrollIntoView ?? IsScrollIntoView)
        {
            target.BringIntoView();
        }

        return target;
    }

    private void ConfigureCurrentStepValues(IGuideStepOption step, Control? target)
    {
        CurrentStyleType = step.StyleType ?? StyleType;
        CurrentIsShowMask = step.IsShowMask ?? IsShowMask;
        CurrentIsArrowVisible = target is not null && (step.IsArrowVisible ?? IsArrowVisible);
        CurrentMaskColor = step.MaskColor ?? MaskColor;
        CurrentGapRadius = Math.Max(0, step.GapRadius ?? GapRadius);
        TargetRegion = target is not null
            ? CalculateTargetRegion(target, step, TopLevel.GetTopLevel(this))
            : default;
        TargetRegionVisible = target is not null;
        PseudoClasses.Set(":primary", CurrentStyleType == GuideStyleType.Primary);
    }

    private void ConfigureOverlay(Control? target, IGuideStepOption step)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (_maskPopup is null || _overlay is null || topLevel is null)
        {
            return;
        }

        var targetTopLevel = target is null ? null : TopLevel.GetTopLevel(target);
        var targetUsesForeignTopLevel = target is not null &&
                                        targetTopLevel is not null &&
                                        !ReferenceEquals(targetTopLevel, topLevel);
        var primaryTargetRegion = TargetRegion;

        if (!CurrentIsShowMask)
        {
            CloseOverlayPopup(_maskPopup);
            CloseOverlayPopup(_targetMaskPopup);
            TargetRegion = default;
            TargetRegionVisible = false;
            return;
        }

        ConfigureOverlayPopup(_maskPopup, _overlay, topLevel, primaryTargetRegion, true);

        if (targetUsesForeignTopLevel && targetTopLevel is not null)
        {
            ConfigureOverlayPopup(
                _targetMaskPopup,
                _targetOverlay,
                targetTopLevel,
                CalculateTargetRegion(target, step, targetTopLevel),
                false);
        }
        else
        {
            CloseOverlayPopup(_targetMaskPopup);
        }
    }

    private void ConfigureOverlayPopup(
        Popup? popup,
        GuideOverlay? overlay,
        TopLevel topLevel,
        Rect targetRegion,
        bool isHitTestVisible)
    {
        if (popup is null || overlay is null)
        {
            return;
        }

        overlay.Width = topLevel.ClientSize.Width;
        overlay.Height = topLevel.ClientSize.Height;
        overlay.MaskBrush = CurrentMaskColor;
        overlay.TargetRegion = targetRegion;
        overlay.TargetRegionCornerRadius = CurrentGapRadius;
        overlay.IsHitTestVisible = isHitTestVisible;
        popup.IsHitTestVisible = isHitTestVisible;
        popup.PlacementTarget = topLevel;
        popup.Placement = PlacementMode.Center;
        popup.HorizontalOffset = 0;
        popup.VerticalOffset = 0;
        OpenOverlayPopup(popup);
    }

    private static void CloseOverlayPopup(Popup? popup)
    {
        if (popup is not null)
        {
            popup.IsOpen = false;
        }
    }

    private static void OpenOverlayPopup(Popup popup)
    {
        try
        {
            popup.IsOpen = true;
        }
        catch (InvalidOperationException)
        {
            popup.IsOpen = false;
        }
    }

    private void ConfigurePopup(Control? target)
    {
        if (_popup is null)
        {
            return;
        }

        var placement = GetCurrentPlacement(target);
        var topLevel = TopLevel.GetTopLevel(this);

        _popup.PlacementTarget = target ?? topLevel;
        ConfigurePopupPlacement(placement);
        ConfigurePopupOffset(placement);
        ConfigureArrow(placement, target);
        _popup.IsOpen = true;

        if (_cardRoot is not null && ShouldFocusCard(target))
        {
            _cardRoot.Focus(NavigationMethod.Unspecified);
        }
    }

    private bool ShouldFocusCard(Control? target)
    {
        if (target is null)
        {
            return true;
        }

        var topLevel = TopLevel.GetTopLevel(this);
        var targetTopLevel = TopLevel.GetTopLevel(target);
        return topLevel is null || targetTopLevel is null || ReferenceEquals(topLevel, targetTopLevel);
    }

    private GuidePlacementMode GetCurrentPlacement(Control? target)
    {
        if (target is null)
        {
            return GuidePlacementMode.Center;
        }

        return GetCurrentStep()?.Placement ?? Placement;
    }

    private void ConfigurePopupPlacement(GuidePlacementMode placement)
    {
        if (_popup is null)
        {
            return;
        }

        if (placement == GuidePlacementMode.Center)
        {
            _popup.Placement = PlacementMode.Center;
            _popup.PlacementConstraintAdjustment = PopupPositionerConstraintAdjustment.None;
            return;
        }

        _popup.Placement = PlacementMode.AnchorAndGravity;
        _popup.PlacementAnchor = ToPopupAnchor(placement);
        _popup.PlacementGravity = ToPopupGravity(placement);
        _popup.PlacementConstraintAdjustment =
            PopupPositionerConstraintAdjustment.SlideX |
            PopupPositionerConstraintAdjustment.SlideY;
    }

    private void ConfigurePopupOffset(GuidePlacementMode placement)
    {
        if (_popup is null)
        {
            return;
        }

        var offset = Math.Max(0, PopupOffset);
        _popup.HorizontalOffset = placement switch
        {
            GuidePlacementMode.Left or GuidePlacementMode.LeftTop or GuidePlacementMode.LeftBottom => -offset,
            GuidePlacementMode.Right or GuidePlacementMode.RightTop or GuidePlacementMode.RightBottom => offset,
            _ => 0
        };
        _popup.VerticalOffset = placement switch
        {
            GuidePlacementMode.Top or GuidePlacementMode.TopLeft or GuidePlacementMode.TopRight => -offset,
            GuidePlacementMode.Bottom or GuidePlacementMode.BottomLeft or GuidePlacementMode.BottomRight => offset,
            _ => 0
        };
    }

    private void ConfigureArrow(GuidePlacementMode placement, Control? target)
    {
        if (_arrow is null)
        {
            return;
        }

        if (_cardRoot is not null)
        {
            _cardRoot.Margin = default;
        }

        _arrow.IsVisible = target is not null && CurrentIsArrowVisible && placement != GuidePlacementMode.Center;
        if (!_arrow.IsVisible)
        {
            return;
        }

        if (_cardRoot is not null)
        {
            _cardRoot.Margin = placement switch
            {
                GuidePlacementMode.Top or GuidePlacementMode.TopLeft or GuidePlacementMode.TopRight => new Thickness(0, 0, 0, 6),
                GuidePlacementMode.Bottom or GuidePlacementMode.BottomLeft or GuidePlacementMode.BottomRight => new Thickness(0, 6, 0, 0),
                GuidePlacementMode.Left or GuidePlacementMode.LeftTop or GuidePlacementMode.LeftBottom => new Thickness(0, 0, 6, 0),
                GuidePlacementMode.Right or GuidePlacementMode.RightTop or GuidePlacementMode.RightBottom => new Thickness(6, 0, 0, 0),
                _ => default
            };
        }

        _arrow.Margin = placement switch
        {
            GuidePlacementMode.Top or GuidePlacementMode.TopLeft or GuidePlacementMode.TopRight => new Thickness(0, 0, 0, -6),
            GuidePlacementMode.Bottom or GuidePlacementMode.BottomLeft or GuidePlacementMode.BottomRight => new Thickness(0, -6, 0, 0),
            GuidePlacementMode.Left or GuidePlacementMode.LeftTop or GuidePlacementMode.LeftBottom => new Thickness(0, 0, -6, 0),
            GuidePlacementMode.Right or GuidePlacementMode.RightTop or GuidePlacementMode.RightBottom => new Thickness(-6, 0, 0, 0),
            _ => default
        };

        _arrow.VerticalAlignment = placement switch
        {
            GuidePlacementMode.LeftTop or GuidePlacementMode.RightTop => VerticalAlignment.Top,
            GuidePlacementMode.LeftBottom or GuidePlacementMode.RightBottom => VerticalAlignment.Bottom,
            GuidePlacementMode.Top or GuidePlacementMode.TopLeft or GuidePlacementMode.TopRight => VerticalAlignment.Bottom,
            GuidePlacementMode.Bottom or GuidePlacementMode.BottomLeft or GuidePlacementMode.BottomRight => VerticalAlignment.Top,
            _ => VerticalAlignment.Center
        };

        _arrow.HorizontalAlignment = placement switch
        {
            GuidePlacementMode.TopLeft or GuidePlacementMode.BottomLeft => HorizontalAlignment.Left,
            GuidePlacementMode.TopRight or GuidePlacementMode.BottomRight => HorizontalAlignment.Right,
            GuidePlacementMode.Left or GuidePlacementMode.LeftTop or GuidePlacementMode.LeftBottom => HorizontalAlignment.Right,
            GuidePlacementMode.Right or GuidePlacementMode.RightTop or GuidePlacementMode.RightBottom => HorizontalAlignment.Left,
            _ => HorizontalAlignment.Center
        };
    }

    private Rect CalculateTargetRegion(Control? target, IGuideStepOption step, TopLevel? relativeTopLevel)
    {
        if (target is null)
        {
            return default;
        }

        if (relativeTopLevel is null)
        {
            return default;
        }

        var targetTopLeft = target.PointToScreen(new Point(0, 0));
        var origin = relativeTopLevel.PointToClient(targetTopLeft);
        var rect = new Rect(origin, target.Bounds.Size);
        var gapX = Math.Max(0, step.GapOffsetX ?? GapOffsetX);
        var gapY = Math.Max(0, step.GapOffsetY ?? GapOffsetY);
        return rect.Inflate(new Thickness(gapX, gapY));
    }

    private void UpdateCurrentStepContent(IGuideStepOption? step)
    {
        CurrentTitle = step?.Title;
        CurrentDescription = step?.Description;
        CurrentCover = step?.Cover;
        HasCurrentTitle = !string.IsNullOrWhiteSpace(CurrentTitle);
        HasCurrentDescription = !string.IsNullOrWhiteSpace(CurrentDescription);
        HasCurrentCover = CurrentCover is not null;
    }

    private void ExecuteOpeningCommand(IGuideStepOption step)
    {
        var command = step.OpeningCommand;
        if (command is null)
        {
            return;
        }

        var parameter = step.OpeningCommandParameter ?? step;
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }

    private void ConfigureStepsSource()
    {
        DetachStepsSource();

        if (StepsSource is INotifyCollectionChanged notifyCollectionChanged)
        {
            _observedStepsSource = notifyCollectionChanged;
            notifyCollectionChanged.CollectionChanged += StepsSource_OnCollectionChanged;
        }

        RefreshStepCollection();
    }

    private void StepsSource_OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        RefreshStepCollection();
    }

    private void RefreshStepCollection()
    {
        _activeSteps.Clear();
        if (StepsSource is not null)
        {
            _activeSteps.AddRange(StepsSource);
        }
        else
        {
            _activeSteps.AddRange(Steps);
        }

        StepCount = _activeSteps.Count;
        if (StepCount == 0)
        {
            SetCurrentValue(CurrentIndexProperty, -1);
            UpdateCurrentStepContent(null);
        }
        else if (CurrentIndex < 0 || CurrentIndex >= StepCount)
        {
            SetCurrentValue(CurrentIndexProperty, 0);
        }

        UpdateNavigationState();
        SyncIndicator();
    }

    private IGuideStepOption? GetCurrentStep()
    {
        return CurrentIndex >= 0 && CurrentIndex < _activeSteps.Count
            ? _activeSteps[CurrentIndex]
            : null;
    }

    private void UpdateNavigationState()
    {
        CanGoPrevious = StepCount > 1 && CurrentIndex > 0;
        CanGoNext = StepCount > 1 && CurrentIndex >= 0 && CurrentIndex < StepCount - 1;
        IsLastStep = StepCount > 0 && CurrentIndex == StepCount - 1;
    }

    private void SyncIndicator()
    {
        SyncNavigationActionStyles();

        if (Indicator is null)
        {
            return;
        }

        Indicator.StepCount = StepCount;
        Indicator.ActiveIndex = CurrentIndex;
        Indicator.StyleType = CurrentStyleType;
    }

    private void SyncNavigationActionStyles()
    {
        var isPrimary = CurrentStyleType == GuideStyleType.Primary;
        _nextButton?.Classes.Set("accent", isPrimary);
        _finishButton?.Classes.Set("accent", isPrimary);
    }

    private void SyncCustomActionsHost()
    {
        if (_customActionsHost is not null)
        {
            _customActionsHost.ItemsSource = CustomActions;
        }
    }

    private void AttachTemplateEvents(INameScope nameScope)
    {
        _previousButton = nameScope.Find<Button>("PART_PreviousButton");
        _nextButton = nameScope.Find<Button>("PART_NextButton");
        _finishButton = nameScope.Find<Button>("PART_FinishButton");
        _closeButton = nameScope.Find<Button>("PART_CloseButton");

        if (_previousButton is not null)
        {
            _previousButton.Click += PreviousButton_OnClick;
            _previousButton.AddHandler(
                InputElement.PointerPressedEvent,
                PreviousButton_OnPointerPressed,
                RoutingStrategies.Bubble,
                true);
        }

        if (_nextButton is not null)
        {
            _nextButton.Click += NextButton_OnClick;
            _nextButton.AddHandler(
                InputElement.PointerPressedEvent,
                NextButton_OnPointerPressed,
                RoutingStrategies.Bubble,
                true);
        }

        if (_finishButton is not null)
        {
            _finishButton.Click += FinishButton_OnClick;
            _finishButton.AddHandler(
                InputElement.PointerPressedEvent,
                FinishButton_OnPointerPressed,
                RoutingStrategies.Bubble,
                true);
        }

        if (_closeButton is not null)
        {
            _closeButton.Click += CloseButton_OnClick;
        }

        if (_cardRoot is not null)
        {
            _cardRoot.KeyDown += CardRoot_OnKeyDown;
        }

        SyncNavigationActionStyles();
    }

    private void DetachTemplateEvents()
    {
        if (_previousButton is not null)
        {
            _previousButton.Click -= PreviousButton_OnClick;
            _previousButton.RemoveHandler(InputElement.PointerPressedEvent, PreviousButton_OnPointerPressed);
            _previousButton = null;
        }

        if (_nextButton is not null)
        {
            _nextButton.Click -= NextButton_OnClick;
            _nextButton.RemoveHandler(InputElement.PointerPressedEvent, NextButton_OnPointerPressed);
            _nextButton = null;
        }

        if (_finishButton is not null)
        {
            _finishButton.Click -= FinishButton_OnClick;
            _finishButton.RemoveHandler(InputElement.PointerPressedEvent, FinishButton_OnPointerPressed);
            _finishButton = null;
        }

        if (_closeButton is not null)
        {
            _closeButton.Click -= CloseButton_OnClick;
            _closeButton = null;
        }

        if (_cardRoot is not null)
        {
            _cardRoot.KeyDown -= CardRoot_OnKeyDown;
        }
    }

    private void PreviousButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (TryConsumeNavigationClick())
        {
            return;
        }

        GoPrevious();
    }

    private void PreviousButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        TryHandleNavigationPointerPressed(sender, e, GoPrevious);
    }

    private void NextButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (TryConsumeNavigationClick())
        {
            return;
        }

        GoNext();
    }

    private void NextButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        TryHandleNavigationPointerPressed(sender, e, GoNext);
    }

    private void FinishButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (TryConsumeNavigationClick())
        {
            return;
        }

        GoNext();
    }

    private void FinishButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        TryHandleNavigationPointerPressed(sender, e, GoNext);
    }

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private bool TryConsumeNavigationClick()
    {
        return _suppressNavigationClick;
    }

    private bool TryHandleNavigationPointerPressed(object? sender, PointerPressedEventArgs e, Action navigate)
    {
        // Menu/Flyout popups can light-dismiss before Button.Click reaches the guide button.
        // Handle pointer navigation early so dynamic popup-backed guide steps remain clickable.
        if (!IsPrimaryButtonPressed(sender, e))
        {
            return false;
        }

        e.Handled = true;
        _suppressNavigationClick = true;
        Dispatcher.UIThread.Post(() =>
        {
            try
            {
                navigate();
            }
            finally
            {
                _suppressNavigationClick = false;
            }
        }, DispatcherPriority.Background);
        return true;
    }

    private static bool IsPrimaryButtonPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Control control)
        {
            return false;
        }

        return e.GetCurrentPoint(control).Properties.IsLeftButtonPressed;
    }

    private static PopupAnchor ToPopupAnchor(GuidePlacementMode placement)
    {
        return placement switch
        {
            GuidePlacementMode.LeftTop or GuidePlacementMode.TopLeft => PopupAnchor.TopLeft,
            GuidePlacementMode.LeftBottom or GuidePlacementMode.BottomLeft => PopupAnchor.BottomLeft,
            GuidePlacementMode.RightTop or GuidePlacementMode.TopRight => PopupAnchor.TopRight,
            GuidePlacementMode.RightBottom or GuidePlacementMode.BottomRight => PopupAnchor.BottomRight,
            GuidePlacementMode.Left => PopupAnchor.Left,
            GuidePlacementMode.Right => PopupAnchor.Right,
            GuidePlacementMode.Top => PopupAnchor.Top,
            GuidePlacementMode.Bottom => PopupAnchor.Bottom,
            _ => PopupAnchor.None
        };
    }

    private static PopupGravity ToPopupGravity(GuidePlacementMode placement)
    {
        return placement switch
        {
            GuidePlacementMode.TopLeft => PopupGravity.TopRight,
            GuidePlacementMode.TopRight => PopupGravity.TopLeft,
            GuidePlacementMode.BottomLeft => PopupGravity.BottomRight,
            GuidePlacementMode.BottomRight => PopupGravity.BottomLeft,
            GuidePlacementMode.LeftTop => PopupGravity.BottomLeft,
            GuidePlacementMode.LeftBottom => PopupGravity.TopLeft,
            GuidePlacementMode.RightTop => PopupGravity.BottomRight,
            GuidePlacementMode.RightBottom => PopupGravity.TopRight,
            GuidePlacementMode.Left => PopupGravity.Left,
            GuidePlacementMode.Right => PopupGravity.Right,
            GuidePlacementMode.Top => PopupGravity.Top,
            GuidePlacementMode.Bottom => PopupGravity.Bottom,
            _ => PopupGravity.None
        };
    }

    private void CardRoot_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Close();
            e.Handled = true;
        }
        else if (e.Key == Key.Left)
        {
            GoPrevious();
            e.Handled = true;
        }
        else if (e.Key == Key.Right || e.Key == Key.Enter)
        {
            GoNext();
            e.Handled = true;
        }
    }

    private void AttachObservedTarget(Control? target)
    {
        if (ReferenceEquals(_observedTarget, target))
        {
            return;
        }

        DetachObservedTarget();
        _observedTarget = target;

        if (_observedTarget is not null)
        {
            _observedTarget.LayoutUpdated += ObservedTarget_OnLayoutUpdated;
            _observedTarget.DetachedFromVisualTree += ObservedTarget_OnDetachedFromVisualTree;
        }
    }

    private void DetachObservedTarget()
    {
        if (_observedTarget is not null)
        {
            _observedTarget.LayoutUpdated -= ObservedTarget_OnLayoutUpdated;
            _observedTarget.DetachedFromVisualTree -= ObservedTarget_OnDetachedFromVisualTree;
            _observedTarget = null;
        }
    }

    private void ObservedTarget_OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (_isReallyOpen && !_layoutRefreshQueued)
        {
            _layoutRefreshQueued = true;
            Dispatcher.UIThread.Post(() =>
            {
                _layoutRefreshQueued = false;
                Refresh();
            }, DispatcherPriority.Background);
        }
    }

    private void ObservedTarget_OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (_isReallyOpen)
        {
            ScheduleApplyCurrentStep();
        }
    }

    private void AttachObservedTopLevel(TopLevel topLevel)
    {
        if (ReferenceEquals(_observedTopLevel, topLevel))
        {
            return;
        }

        DetachObservedTopLevel();
        _observedTopLevel = topLevel;
        _observedTopLevel.PropertyChanged += ObservedTopLevel_OnPropertyChanged;
        _observedTopLevel.Closed += ObservedTopLevel_OnClosed;
    }

    private void DetachObservedTopLevel()
    {
        if (_observedTopLevel is not null)
        {
            _observedTopLevel.PropertyChanged -= ObservedTopLevel_OnPropertyChanged;
            _observedTopLevel.Closed -= ObservedTopLevel_OnClosed;
            _observedTopLevel = null;
        }
    }

    private void ObservedTopLevel_OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (_isReallyOpen && e.Property == TopLevel.ClientSizeProperty)
        {
            Refresh();
        }
    }

    private void ObservedTopLevel_OnClosed(object? sender, EventArgs e)
    {
        HideGuideCore(false);
    }

    private void DetachStepsSource()
    {
        if (_observedStepsSource is not null)
        {
            _observedStepsSource.CollectionChanged -= StepsSource_OnCollectionChanged;
            _observedStepsSource = null;
        }
    }

    private void ClosePopups()
    {
        if (_popup is not null)
        {
            _popup.IsOpen = false;
        }

        if (_maskPopup is not null)
        {
            _maskPopup.IsOpen = false;
        }

        if (_targetMaskPopup is not null)
        {
            _targetMaskPopup.IsOpen = false;
        }
    }

    private void SetIsOpenSilently(bool isOpen)
    {
        _ignoreOpenChange = true;
        SetCurrentValue(IsOpenProperty, isOpen);
        _ignoreOpenChange = false;
    }

}

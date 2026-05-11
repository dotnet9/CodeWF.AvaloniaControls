using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace CodeWF.AvaloniaControls.Controls;

[PseudoClasses(
    ":has-left-content",
    ":has-center-content",
    ":has-right-content",
    ":has-icon")]
public class CodeWFTitleBar : TemplatedControl
{
    public static readonly StyledProperty<object?> LeftContentProperty =
        AvaloniaProperty.Register<CodeWFTitleBar, object?>(nameof(LeftContent));

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> LeftContentTemplateProperty =
        AvaloniaProperty.Register<CodeWFTitleBar, IDataTemplate?>(nameof(LeftContentTemplate));

    public IDataTemplate? LeftContentTemplate
    {
        get => GetValue(LeftContentTemplateProperty);
        set => SetValue(LeftContentTemplateProperty, value);
    }

    public static readonly StyledProperty<object?> CenterContentProperty =
        AvaloniaProperty.Register<CodeWFTitleBar, object?>(nameof(CenterContent));

    public object? CenterContent
    {
        get => GetValue(CenterContentProperty);
        set => SetValue(CenterContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> CenterContentTemplateProperty =
        AvaloniaProperty.Register<CodeWFTitleBar, IDataTemplate?>(nameof(CenterContentTemplate));

    public IDataTemplate? CenterContentTemplate
    {
        get => GetValue(CenterContentTemplateProperty);
        set => SetValue(CenterContentTemplateProperty, value);
    }

    public static readonly StyledProperty<object?> RightContentProperty =
        AvaloniaProperty.Register<CodeWFTitleBar, object?>(nameof(RightContent));

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> RightContentTemplateProperty =
        AvaloniaProperty.Register<CodeWFTitleBar, IDataTemplate?>(nameof(RightContentTemplate));

    public IDataTemplate? RightContentTemplate
    {
        get => GetValue(RightContentTemplateProperty);
        set => SetValue(RightContentTemplateProperty, value);
    }

    public static readonly StyledProperty<WindowIcon?> IconProperty =
        AvaloniaProperty.Register<CodeWFTitleBar, WindowIcon?>(nameof(Icon));

    public WindowIcon? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<CodeWFTitleBar, string?>(nameof(Title));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    protected override Type StyleKeyOverride => typeof(CodeWFTitleBar);

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == LeftContentProperty
            || change.Property == CenterContentProperty
            || change.Property == RightContentProperty
            || change.Property == IconProperty)
        {
            UpdatePseudoClasses();
        }
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(":has-left-content", LeftContent is not null);
        PseudoClasses.Set(":has-center-content", CenterContent is not null);
        PseudoClasses.Set(":has-right-content", RightContent is not null);
        PseudoClasses.Set(":has-icon", Icon is not null);
    }
}

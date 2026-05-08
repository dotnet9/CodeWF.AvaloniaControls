using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaEdit.Highlighting;
using CodeWF.AvaloniaControls.Showcase.Models;
using CodeWF.AvaloniaControls.Showcase.Services;
using Lang.Avalonia;
using PageLangs = Showcase.Pages.MarkupExtensionsDemo;

namespace CodeWF.AvaloniaControls.Showcase.Pages;

public partial class MarkupExtensionsDemo : UserControl, INotifyPropertyChanged
{
    private bool _isPipelineRunning = true;
    private PipelineHealth _health = PipelineHealth.Healthy;
    private ObservableCollection<string> _queuedDeployments = CreateDefaultQueue();

    public MarkupExtensionsDemo()
    {
        DataContext = this;
        InitializeComponent();
        ConfigureCodeEditors();
        I18nManager.Instance.CultureChanged += (_, _) => ReloadLocalizedText();
    }

    public bool IsPipelineRunning
    {
        get => _isPipelineRunning;
        set => SetField(ref _isPipelineRunning, value);
    }

    public PipelineHealth Health
    {
        get => _health;
        set => SetField(ref _health, value);
    }

    public ObservableCollection<string> QueuedDeployments
    {
        get => _queuedDeployments;
        set => SetField(ref _queuedDeployments, value);
    }

    public string PipelineStatusText => IsPipelineRunning
        ? LocalizationService.Get(PageLangs.PipelineRunning)
        : LocalizationService.Get(PageLangs.PipelinePaused);

    public string PipelineToggleText => IsPipelineRunning
        ? LocalizationService.Get(PageLangs.PausePipeline)
        : LocalizationService.Get(PageLangs.StartPipeline);

    public string QueueStatusText => QueuedDeployments.Count <= 0
        ? LocalizationService.Get(PageLangs.QueueEmpty)
        : LocalizationService.Get(PageLangs.QueueNotEmpty);

    public string IfSnippet => "{codewf:If {Binding IsPipelineRunning}, PausePipeline, StartPipeline}";

    public string SwitchSnippet => "{codewf:Switch {Binding Health}, Cases={StaticResource HealthTitleCases}, Default=UnknownStatus}";

    public new event PropertyChangedEventHandler? PropertyChanged;

    private static ObservableCollection<string> CreateDefaultQueue()
    {
        return
        [
            LocalizationService.Get(PageLangs.QueueItemApi),
            LocalizationService.Get(PageLangs.QueueItemDesktop),
            LocalizationService.Get(PageLangs.QueueItemDocs),
            LocalizationService.Get(PageLangs.QueueItemSync)
        ];
    }

    private void TogglePipelineStatus(object? sender, RoutedEventArgs e)
    {
        IsPipelineRunning = !IsPipelineRunning;
    }

    private void NextHealth(object? sender, RoutedEventArgs e)
    {
        var values = Enum.GetValues<PipelineHealth>();
        var index = (Array.IndexOf(values, Health) + 1) % values.Length;
        Health = values[index];
    }

    private void ClearQueue(object? sender, RoutedEventArgs e)
    {
        QueuedDeployments = [];
        RaiseQueueStateChanged();
    }

    private void RefillQueue(object? sender, RoutedEventArgs e)
    {
        QueuedDeployments = CreateDefaultQueue();
        RaiseQueueStateChanged();
    }

    private void ConfigureCodeEditors()
    {
        var xamlHighlighting = HighlightingManager.Instance.GetDefinition("XML");

        SwitchSnippetEditor.SyntaxHighlighting = xamlHighlighting;
        SwitchSnippetEditor.Text = SwitchSnippet;

        IfSnippetEditor.SyntaxHighlighting = xamlHighlighting;
        IfSnippetEditor.Text = IfSnippet;
    }

    private void ReloadLocalizedText()
    {
        if (QueuedDeployments.Count > 0)
        {
            QueuedDeployments = CreateDefaultQueue();
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PipelineStatusText)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PipelineToggleText)));
        RaiseQueueStateChanged();
    }

    private void RaisePipelineStateChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PipelineStatusText)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PipelineToggleText)));
    }

    private void RaiseQueueStateChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QueueStatusText)));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value))
        {
            return false;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        if (propertyName == nameof(IsPipelineRunning))
        {
            RaisePipelineStateChanged();
        }

        if (propertyName == nameof(QueuedDeployments))
        {
            RaiseQueueStateChanged();
        }

        return true;
    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaEdit.Highlighting;
using CodeWF.AvaloniaControls.Showcase.Models;

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

    public string IfSnippet => "{codewf:If {Binding IsPipelineRunning}, 暂停流水线, 启动流水线}";

    public string SwitchSnippet => "{codewf:Switch {Binding Health}, Cases={StaticResource HealthTitleCases}, Default=未知状态}";

    public new event PropertyChangedEventHandler? PropertyChanged;

    private static ObservableCollection<string> CreateDefaultQueue()
    {
        return
        [
            "API 网关",
            "桌面端安装包",
            "文档站点",
            "同步任务"
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
    }

    private void RefillQueue(object? sender, RoutedEventArgs e)
    {
        QueuedDeployments = CreateDefaultQueue();
    }

    private void ConfigureCodeEditors()
    {
        var xamlHighlighting = HighlightingManager.Instance.GetDefinition("XML");

        SwitchSnippetEditor.SyntaxHighlighting = xamlHighlighting;
        SwitchSnippetEditor.Text = SwitchSnippet;

        IfSnippetEditor.SyntaxHighlighting = xamlHighlighting;
        IfSnippetEditor.Text = IfSnippet;
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value))
        {
            return false;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}

using System;
using System.Linq;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;
using CodeWF.EventBus;
using Dock.Model.Controls;
using Dock.Model.ReactiveUI.Controls;
using DryIoc.ImTools;
using ReactiveUI;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels;

public class TitleBarRightContentViewModel : ReactiveObject
{
    public TitleBarRightContentViewModel()
    {
        EventBus.EventBus.Default.Subscribe(this);
    }

    #region 模块可见状态

    private bool _isDataManagementVisible = true;

    public bool IsDataManagementVisible
    {
        get => _isDataManagementVisible;
        set => this.RaiseAndSetIfChanged(ref _isDataManagementVisible, value);
    }

    private bool _isHelpDocumentationVisible = true;

    public bool IsHelpDocumentationVisible
    {
        get => _isHelpDocumentationVisible;
        set => this.RaiseAndSetIfChanged(ref _isHelpDocumentationVisible, value);
    }

    private bool _isLogRecordsVisible = true;

    public bool IsLogRecordsVisible
    {
        get => _isLogRecordsVisible;
        set => this.RaiseAndSetIfChanged(ref _isLogRecordsVisible, value);
    }

    private bool _isSystemSettingsVisible = true;

    public bool IsSystemSettingsVisible
    {
        get => _isSystemSettingsVisible;
        set => this.RaiseAndSetIfChanged(ref _isSystemSettingsVisible, value);
    }

    private bool _isUserCenterVisible = true;

    public bool IsUserCenterVisible
    {
        get => _isUserCenterVisible;
        set => this.RaiseAndSetIfChanged(ref _isUserCenterVisible, value);
    }

    #endregion 模块可见状态

    public void RaiseOpenOrLocateDocumentHandler(string documentKey)
    {
        switch (documentKey)
        {
            case nameof(DataManagementViewModel):
                OpenOrLocateDocument(documentKey, () => new DataManagementViewModel());
                IsDataManagementVisible = true;
                break;
            case nameof(HelpDocumentationViewModel):
                OpenOrLocateDocument(documentKey, () => new HelpDocumentationViewModel());
                IsHelpDocumentationVisible = true;
                break;
            case nameof(LogRecordsViewModel):
                OpenOrLocateDocument(documentKey, () => new LogRecordsViewModel());
                IsLogRecordsVisible = true;
                break;
            case nameof(SystemSettingsViewModel):
                OpenOrLocateDocument(documentKey, () => new SystemSettingsViewModel());
                IsSystemSettingsVisible = true;
                break;
            case nameof(UserCenterViewModel):
                OpenOrLocateDocument(documentKey, () => new UserCenterViewModel());
                IsUserCenterVisible = true;
                break;
        }
    }

    /// <summary>
    /// 弹出的独立窗口触发关闭
    /// </summary>
    /// <param name="command"></param>
    [EventHandler]
    private void ReceiveCloseDocumentCommand(CloseDocumentCommand command)
    {
        switch (command.DocumentKey)
        {
            case nameof(DataManagementViewModel):
                IsDataManagementVisible = false;
                break;
            case nameof(HelpDocumentationViewModel):
                IsHelpDocumentationVisible = false;
                break;
            case nameof(LogRecordsViewModel):
                IsLogRecordsVisible = false;
                break;
            case nameof(SystemSettingsViewModel):
                IsSystemSettingsVisible = false;
                break;
            case nameof(UserCenterViewModel):
                IsUserCenterVisible = false;
                break;
        }
    }

    private void OpenOrLocateDocument(string documentKey, Func<Document> createDocAction)
    {
        var documents = MainWindowViewModel.Instance?.Factory?.GetDockable<IDocumentDock>(DockFactory.DocumentsKey);
        var existDocument = documents?.VisibleDockables?.OfType<IDocument>()
            .FirstOrDefault(d => d.Id == documentKey);

        if (existDocument != null)
        {
            documents?.ActiveDockable = existDocument;
            return;
        }

        if (existDocument == null && MainWindowViewModel.Instance?.Factory?.DocumentWindows
                .Keys.OfType<IDocument>().FindFirst(d => d.Id == documentKey) != null)
        {
            return;
        }

        existDocument = createDocAction.Invoke();
        MainWindowViewModel.Instance?.Factory?.AddDockable(documents, existDocument);
        MainWindowViewModel.Instance?.Factory?.SetActiveDockable(existDocument);
        MainWindowViewModel.Instance?.Factory?.SetFocusedDockable(MainWindowViewModel.Instance.Layout!,
            existDocument);
    }
}
using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;
using CodeWF.EventBus;
using Dock.Model.Controls;
using Dock.Model.Core;
using ReactiveUI;
using System;
using System.Linq;

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
                OpenOrLocateDocument(documentKey);
                IsDataManagementVisible = true;
                break;
            case nameof(HelpDocumentationViewModel):
                OpenOrLocateDocument(documentKey);
                IsHelpDocumentationVisible = true;
                break;
            case nameof(LogRecordsViewModel):
                OpenOrLocateDocument(documentKey);
                IsLogRecordsVisible = true;
                break;
            case nameof(SystemSettingsViewModel):
                OpenOrLocateDocument(documentKey);
                IsSystemSettingsVisible = true;
                break; 
            case nameof(UserCenterViewModel):
                OpenOrLocateDocument(documentKey);
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

    private void OpenOrLocateDocument(string documentKey)
    {
        var documents = MainWindowViewModel.Instance?.Factory?.GetDockable<IDocumentDock>(DockFactory.DocumentsKey);

        // 1、先从主窗口Tab找
        var existDocument = documents?.VisibleDockables?.OfType<IDocument>()
            .FirstOrDefault(d => d.Id == documentKey);

        // 找到设置为焦点Item
        if (existDocument != null)
        {
            documents?.ActiveDockable = existDocument;
            return;
        }

        // 2、从打开的HostWindow里找
        if (IsExistInDockWindows(documentKey))
        {
            return;
        }
        
        // 3、从所有文档中查找，Dock没有完全释放，不需要重新实例化
        existDocument = DockFactory.Documents.FirstOrDefault(d => d.Id == documentKey);

        MainWindowViewModel.Instance?.Factory?.AddDockable(documents, existDocument);
        MainWindowViewModel.Instance?.Factory?.SetActiveDockable(existDocument);
        MainWindowViewModel.Instance?.Factory?.SetFocusedDockable(MainWindowViewModel.Instance.Layout!,
            existDocument);
    }

    private bool IsExistInDockWindows(string documentKey)
    {
        foreach (IDockWindow hostWindow in DockFactory.DockWindows)
        {
            if (hostWindow.Layout is not IDock layout)
            {
                continue;
            }

            if (IsExist(layout, documentKey))
            {
                hostWindow.SetActive();
                return true;
            }
        }

        return false;
    }

    private bool IsExist(IDock dock, string documentKey)
    {
        if (dock.VisibleDockables is null)
        {
            return false;
        }

        foreach (var item in dock.VisibleDockables)
        {
            if (item is IDock subDock)
            {
                if (IsExist(subDock, documentKey))
                {
                    return true;
                }
            }

            if (item is IDocument document && document.Id == documentKey)
            {
                return true;
            }
        }

        return false;
    }
}
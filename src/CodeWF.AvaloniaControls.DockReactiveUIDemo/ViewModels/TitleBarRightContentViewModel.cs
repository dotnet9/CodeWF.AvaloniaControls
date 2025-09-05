using CodeWF.AvaloniaControls.DockReactiveUIDemo.Commands;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.ViewModels.Documents;
using CodeWF.EventBus;
using Dock.Model.Controls;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using Dock.Model.Core;

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

        // 1、先从主Tab找
        var existDocument = documents?.VisibleDockables?.OfType<IDocument>()
            .FirstOrDefault(d => d.Id == documentKey);

        // 找到，直接聚焦
        if (existDocument != null)
        {
            documents?.ActiveDockable = existDocument;
            return;
        }

        // 2、从独立窗口找
        if (MainWindowViewModel.Instance?.Factory is DockFactory dockFactory)
        {
            // 收集所有唯一的窗口
            var uniqueWindows = new HashSet<IDockWindow>(dockFactory.DocumentWindows.Values);
            
            // 遍历所有唯一窗口
            foreach (var window in uniqueWindows)
            {
                // 检查窗口布局是否包含我们要找的文档
                if (window.Layout is IDock windowLayout && windowLayout.VisibleDockables != null)
                {
                    // 从窗口布局的VisibleDockables中查找ID匹配的文档
                    var docInWindow = windowLayout.VisibleDockables.OfType<IDocument>()
                        .FirstOrDefault(d => d.Id == documentKey);
                    
                    if (docInWindow != null)
                    {
                        existDocument = docInWindow;
                        
                        // 从窗口布局中移除匹配的文档
                        windowLayout.VisibleDockables.Remove(existDocument);
                        
                        // 只有当窗口中没有其他文档时才关闭窗口
                        if (!windowLayout.VisibleDockables.Any())
                        {
                            window.Exit();
                        }
                        break;
                    }
                }
            }
        }
        
        // 3、如果从窗口布局中没有找到，再尝试从DocumentWindows字典键中查找（后备方案）
        if (existDocument == null && MainWindowViewModel.Instance?.Factory is DockFactory dockFactoryFallback)
        {
            foreach (var kvp in dockFactoryFallback.DocumentWindows)
            {
                if (kvp.Key is IDocument fallbackDoc && fallbackDoc.Id == documentKey)
                {
                    existDocument = fallbackDoc;
                    // 这里无法确定窗口布局情况，为避免误关多文档窗口，不自动关闭窗口
                    // 只设置existDocument，后续会将其添加到主窗口
                    break;
                }
            }
        }
        
        // 如果在独立窗口找到文档，添加到主窗口并设置动画效果
        if (existDocument != null)
        {
            // 添加文档到主窗口，这将触发DockableAdded事件并应用内置的移动动画
            MainWindowViewModel.Instance?.Factory?.AddDockable(documents, existDocument);
            
            // 设置为活动和聚焦状态，完成移动效果
            documents?.ActiveDockable = existDocument;
            MainWindowViewModel.Instance?.Factory?.SetActiveDockable(existDocument);
            MainWindowViewModel.Instance?.Factory?.SetFocusedDockable(MainWindowViewModel.Instance.Layout!, existDocument);
            return;
        }

        // 3、最后从所有文档中找
        existDocument = DockFactory.Documents
            .FirstOrDefault(d => d.Id == documentKey);
            

        // 4、如果都没有找到，直接创建
        if (existDocument == null)
        {
            existDocument = createDocAction.Invoke();
        }

        MainWindowViewModel.Instance?.Factory?.AddDockable(documents, existDocument);
        MainWindowViewModel.Instance?.Factory?.SetActiveDockable(existDocument);
        MainWindowViewModel.Instance?.Factory?.SetFocusedDockable(MainWindowViewModel.Instance.Layout!,
            existDocument);
    }
}
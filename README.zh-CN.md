# CodeWF.AvaloniaControls

| 名称 | NuGet | 下载量 |
|------|-------|--------|
| CodeWF.AvaloniaControls | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) |
| CodeWF.AvaloniaControls.Themes | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.Themes.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Themes/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.Themes.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Themes/) |
| CodeWF.AvaloniaControls.DataGrid | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.DataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.DataGrid/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.DataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.DataGrid/) |
| CodeWF.AvaloniaControls.Dock | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.Dock.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Dock/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.Dock.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Dock/) |
| CodeWF.AvaloniaControls.ProDataGrid | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.ProDataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.ProDataGrid/) |
| CodeWF.Markdown | [![NuGet](https://img.shields.io/nuget/v/CodeWF.Markdown.svg)](https://www.nuget.org/packages/CodeWF.Markdown/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.Markdown.svg)](https://www.nuget.org/packages/CodeWF.Markdown/) |
| CodeWF.Markdown.Themes | [![NuGet](https://img.shields.io/nuget/v/CodeWF.Markdown.Themes.svg)](https://www.nuget.org/packages/CodeWF.Markdown.Themes/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.Markdown.Themes.svg)](https://www.nuget.org/packages/CodeWF.Markdown.Themes/) |

这是一个基于 .NET 11 与 Avalonia 12 的开源控件仓库，包含可复用类库、兼容旧版免费表格链路的辅助封装，以及可直接运行的示例工程。

[English](README.md) | 简体中文

## 安装

```powershell
Install-Package CodeWF.AvaloniaControls
```

## 仓库结构

- `src/`：所有工程的物理目录，包含 NuGet 类库与可运行示例
- `docs/`：截图、GIF 与仓库文档资源
- `artifacts/`：打包输出与临时构建产物
- `publish/`：一键发布示例后生成的发布目录
- `CodeWF.AvaloniaControls.slnx`：按包线和示例用途做逻辑分组的解决方案视图

## 包线说明

### Avalonia 12 主线

- `CodeWF.AvaloniaControls`：通用自定义控件
- `CodeWF.AvaloniaControls.Dock`：适配 Semi 主题的 Dock 扩展控件
- `CodeWF.AvaloniaControls.ProDataGrid`：基于 `ProDataGrid` 的开源高性能表格辅助包
- `CodeWF.AvaloniaControls.Themes`：`CodeWF.AvaloniaControls` 的控件模板与主题资源包
- `CodeWF.Markdown`：Markdown 解析、代码高亮、图片预览与增量渲染控件
- `CodeWF.Markdown.Themes`：MarkdownViewer 的默认控件模板和多套排版主题资源

### 旧版免费 DataGrid / TreeDataGrid 链路

- `CodeWF.AvaloniaControls.DataGrid` 保持在最后一个免费开源官方 `Avalonia.Controls.DataGrid` / `Avalonia.Controls.TreeDataGrid` 版本线上
- 这条兼容链路使用工程内显式版本，不在中央包管理器中增加复杂条件分支

## Markdown 控件用法

安装核心控件和主题包：

```powershell
Install-Package CodeWF.Markdown
Install-Package CodeWF.Markdown.Themes
```

在 `App.axaml` 引入默认样式和排版主题：

```xml
<Application
    xmlns="https://github.com/avaloniaui"
    xmlns:markdown="https://codewf.com">
    <Application.Styles>
        <FluentTheme />
        <markdown:MarkdownThemes TypographyTheme="Simple" />
    </Application.Styles>
</Application>
```

在页面中使用 `MarkdownViewer`。控件只接收 `Markdown` 属性：

```xml
<UserControl
    xmlns="https://github.com/avaloniaui"
    xmlns:md="https://codewf.com">
    <ScrollViewer
        HorizontalScrollBarVisibility="Disabled"
        VerticalScrollBarVisibility="Auto">
        <md:MarkdownViewer Markdown="{Binding Markdown}" />
    </ScrollViewer>
</UserControl>
```

运行时切换排版主题：

```csharp
MarkdownThemes.OverrideTypographyResources(
    Application.Current!,
    MarkdownTypographyThemes.BlueGlow);
```

如果需要给代码块追加业务按钮，可以处理 `CodeBlockToolRender`：

```xml
<md:MarkdownViewer
    Markdown="{Binding Markdown}"
    CodeBlockToolRender="OnCodeBlockToolRender" />
```

```csharp
private void OnCodeBlockToolRender(object? sender, CodeBlockToolRenderEventArgs e)
{
    e.HeaderPanel.Children.Add(new Button { Content = "运行" });
}
```

`MarkdownViewer` 支持标题、段落、列表、任务列表、引用、表格、行内代码、链接、图片和带语言标识的代码块。代码块内置复制按钮，图片点击后会打开预览窗口。相对图片路径默认按应用进程目录解析；如果 Markdown 文件来自其他目录，建议在传入控件前把图片地址整理为绝对路径或 `file://` 地址。

## 示例工程

- `CodeWF.AvaloniaControls.Showcase`：通用控件展示馆
- `CodeWF.AvaloniaControls.ProDataGridShowcase`：Avalonia 12 下的 ProDataGrid 功能示例
- `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo`：Avalonia 12 下的大数据量、Tab 切换、Document 切换性能示例
- `CodeWF.Markdown.Sample`：Markdown 编辑、实时预览、排版主题切换和增量渲染压力示例
- `CodeWF.AvaloniaControls.DataGridLegacyDemo`：旧版免费 DataGrid 示例
- `CodeWF.AvaloniaControls.TreeDataGridLegacyDemo`：旧版免费 TreeDataGrid 示例
- `CodeWF.AvaloniaControls.DockDemo`、`CodeWF.AvaloniaControls.DockPrismDemo`、`CodeWF.AvaloniaControls.DockReactiveUIDemo`：Dock 集成示例
- `CodeWF.AvaloniaControls.FluentStarterDemo`：轻量启动窗口示例

## 公共配置

- `Directory.Packages.props`：Avalonia 12 主线与 `ProDataGrid` 等共享依赖的中央包管理
- `Directory.Build.props`：仓库级公共元数据
- `Directory.Build.targets`：类库打包时的公共处理逻辑，例如统一补充 README、更新日志与通用打包默认值
- `Publish.Common.pubxml`：共享发布参数
- `src/*/Properties/PublishProfiles/Publish.Project.pubxml`：每个工程的发布补充配置，例如裁剪保留描述文件

## 脚本

- `pack.bat`：一键还原、构建并打包所有可发布类库到 `artifacts/packages`
- `publish_all.bat`：一键发布所有示例工程到 `publish/`
- `publishbase.bat`：根目录发布脚本共用的发布辅助脚本

## 更新日志

- 仓库级变更记录在根目录 `CHANGELOG.md`
- 每个工程目录下也都保留独立 `CHANGELOG.md`，用于记录各自类库或示例的变更历史

## 开源约束

- 仓库明确避免使用商业版 NuGet 包
- `Prism.DryIoc.Avalonia` 固定使用 `8.1.97.11073`，因为 `9.x` 已转为商业版
- `Semi.Avalonia.DataGrid` 与 `Semi.Avalonia.TreeDataGrid` 使用最后一个免费开源主题包版本
- `CodeWF.AvaloniaControls.ProDataGrid` 基于 MIT 协议的 [ProDataGrid](https://www.nuget.org/packages/ProDataGrid/)

## 演示

### Transfer

![](docs/Images/Transfer.gif)

### ComboBox

![](docs/Images/ComboBox.gif)

### TabControl

![](docs/Images/TabControl.gif)

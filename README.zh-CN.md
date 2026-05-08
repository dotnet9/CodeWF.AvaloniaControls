# CodeWF.AvaloniaControls

| 名称 | NuGet | 下载量 |
|------|-------|--------|
| CodeWF.AvaloniaControls | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) |
| CodeWF.AvaloniaControls.Themes | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.Themes.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Themes/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.Themes.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Themes/) |

这是一个基于 .NET 11 与 Avalonia 12 的开源控件仓库，包含可复用类库以及可直接运行的示例工程。

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
- `CodeWF.AvaloniaControls.Themes`：`CodeWF.AvaloniaControls` 的控件模板与主题资源包

Markdown 相关包已迁移到独立仓库：[CodeWF.Markdown](https://github.com/dotnet9/CodeWF.Markdown)。

旧版免费 `DataGrid` / `TreeDataGrid` 包与示例已迁移到独立仓库：[CodeWF.AvaloniaControls.DataGrid](https://github.com/dotnet9/CodeWF.AvaloniaControls.DataGrid)。

Dock 包与示例已迁移到独立仓库：[CodeWF.AvaloniaControls.Dock](https://github.com/dotnet9/CodeWF.AvaloniaControls.Dock)。

ProDataGrid 包与示例已迁移到独立仓库：[CodeWF.AvaloniaControls.ProDataGrid](https://github.com/dotnet9/CodeWF.AvaloniaControls.ProDataGrid)。

## 示例工程

- `CodeWF.AvaloniaControls.Showcase`：通用控件展示馆
- `CodeWF.AvaloniaControls.FluentStarterDemo`：轻量启动窗口示例

## 公共配置

- `Directory.Packages.props`：Avalonia 12 主线共享依赖的中央包管理
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

## 演示

### Transfer

![](docs/Images/Transfer.gif)

### ComboBox

![](docs/Images/ComboBox.gif)

### TabControl

![](docs/Images/TabControl.gif)

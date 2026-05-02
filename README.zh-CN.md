# CodeWF.AvaloniaControls

| 名称 | NuGet | 下载量 |
|------|-------|--------|
| CodeWF.AvaloniaControls | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) |
| CodeWF.AvaloniaControls.DataGrid | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.DataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.DataGrid/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.DataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.DataGrid/) |
| CodeWF.AvaloniaControls.Dock | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.Dock.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Dock/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.Dock.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Dock/) |
| CodeWF.Core | [![NuGet](https://img.shields.io/nuget/v/CodeWF.Core.svg)](https://www.nuget.org/packages/CodeWF.Core/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.Core.svg)](https://www.nuget.org/packages/CodeWF.Core/) |
| CodeWF.Themes | [![NuGet](https://img.shields.io/nuget/v/CodeWF.Themes.svg)](https://www.nuget.org/packages/CodeWF.Themes/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.Themes.svg)](https://www.nuget.org/packages/CodeWF.Themes/) |

一个基于 .NET 11 与 Avalonia 12 的 Avalonia 控件库和示例仓库。

[English](README.md) | 简体中文

## 安装

```powershell
Install-Package CodeWF.AvaloniaControls
```

## 仓库结构

- `src/`：所有项目的物理目录，包含可打包类库和可运行示例
- `docs/`：截图、GIF 与文档资源
- `artifacts/`：构建与打包输出目录
- `CodeWF.AvaloniaControls.slnx`：按 `datagrid`、`docking`、`showcase` 等逻辑分组组织项目

## 包管理策略

- 主线 Avalonia 12 项目统一使用 `Directory.Packages.props` 做中央包管理
- 只有必须固定在特殊依赖链路上的项目，才在自己的 `.csproj` 中显式写版本
- 解决方案格式使用 `.slnx`
- 所有项目物理目录统一保留在 `src/`

## DataGrid 与 TreeDataGrid 说明

### Avalonia 12 主线

- 主库与普通示例保持在 Avalonia 12 主线
- `CodeWF.AvaloniaControls.ProDataGridShowcase` 用于展示 Avalonia 12 下的 ProDataGrid 功能场景
- `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo` 用于展示大数据量、页签切换和文档切换性能场景
- Dock 普通示例不再依赖 `DataGrid` 或 `TreeDataGrid`，避免影响 Avalonia 12 主线体验

### 免费开源旧版表格链路

- `CodeWF.AvaloniaControls.DataGrid` 包继续兼容最后一个免费开源的官方 `Avalonia.Controls.DataGrid` 与 `Avalonia.Controls.TreeDataGrid`
- 该包在项目内部显式固定 Avalonia 11 兼容版本，不再走中央包管理
- 新增两个独立专项示例：
  - `CodeWF.AvaloniaControls.DataGridLegacyDemo`
  - `CodeWF.AvaloniaControls.TreeDataGridLegacyDemo`
- 这两个示例专门用于演示“大数据量 + TabControl 切换”场景，方便直观看出旧版 `DataGrid` 与 `TreeDataGrid` 的切换差异

### 开源高性能表格链路

- `CodeWF.AvaloniaControls.ProDataGrid` 基于 MIT 协议的 [ProDataGrid](https://www.nuget.org/packages/ProDataGrid/)
- 该链路面向 Avalonia 12，提供三态排序、智能提示、大数据量预设等扩展能力

## 开源约束

- 本仓库不使用商业版 NuGet 包
- `Prism.DryIoc.Avalonia` 固定使用 `8.1.97.11073`，因为 `9.x` 已转为商业版
- `Semi.Avalonia.DataGrid` 与 `Semi.Avalonia.TreeDataGrid` 使用最后一个免费开源主题包版本

## 一键打包

在仓库根目录运行：

```bat
pack.bat
```

脚本会自动执行还原、构建与打包，最终 NuGet 包输出到 `artifacts/packages`。

## 示例项目

- `CodeWF.AvaloniaControls.Showcase`：通用控件展示
- `CodeWF.AvaloniaControls.ProDataGridShowcase`：Avalonia 12 ProDataGrid 功能示例
- `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo`：Avalonia 12 ProDataGrid 性能示例
- `CodeWF.AvaloniaControls.DataGridLegacyDemo`：旧版免费 DataGrid 切换压力示例
- `CodeWF.AvaloniaControls.TreeDataGridLegacyDemo`：旧版免费 TreeDataGrid 切换压力示例
- `CodeWF.AvaloniaControls.DockDemo` / `CodeWF.AvaloniaControls.DockPrismDemo` / `CodeWF.AvaloniaControls.DockReactiveUIDemo`：Dock 集成示例
- `CodeWF.AvaloniaControls.FluentStarterDemo`：Fluent 起步示例
- `CodeWF.Themes.StatusBadgeDemo`：状态徽章主题示例

## 演示

### Transfer

![](docs/Images/Transfer.gif)

### ComboBox

![](docs/Images/ComboBox.gif)

### TabControl

![](docs/Images/TabControl.gif)

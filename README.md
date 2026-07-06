# CodeWF.AvaloniaControls

| 名称 | NuGet | 下载量 |
|------|-------|--------|
| CodeWF.AvaloniaControls | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) |
| CodeWF.AvaloniaControls.Themes | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.Themes.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Themes/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.Themes.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Themes/) |

这是一个基于 .NET 11 与 Avalonia 12 的开源控件仓库，包含可复用类库以及可直接运行的示例工程。

## 仓库规范

- 当前版本：`12.0.5.3`，版本号统一维护在根目录 `Directory.Build.props` 的 `<Version>` 节点。
- NuGet 包项目统一支持 `net8.0;net10.0`；Demo、App、测试与内部应用项目统一使用 `net11.0` / `net11.0-windows`。
- 根目录 `logo.svg`、`logo.png`、`logo.ico` 是唯一图标源，子工程只通过 MSBuild `Link` 引用，不维护图标副本。
- 运行时帮助、Markdown 示例、内置备忘录、设计说明等业务文档按功能保留；仓库级入口文档使用根目录 `README.md` 和 `UpdateLog.md`。

## 安装

```powershell
Install-Package CodeWF.AvaloniaControls
Install-Package CodeWF.AvaloniaControls.Themes
```

在 Avalonia 应用中引入主题资源：

```xml
<Application
    xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:codewf="https://codewf.com">
    <Application.Styles>
        <codewf:ACSemiTheme />
    </Application.Styles>
</Application>
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

## 控件概览

- `Guide`：用于桌面应用的新手引导、功能漫游和局部提示。
- `Transfer` / `SearchListBox`：左右穿梭框与可搜索列表。
- `StatusBadge` / `StatusCard`：语义化状态标签与状态概览卡片。
- `CodeWFWindow` / `CodeWFTitleBar`：自定义窗口外壳和标题栏。
- TabControl 样式、标记扩展、转换器和小型绘图辅助控件。

## Guide 引导控件

`Guide` 面向 Avalonia 桌面应用的新手引导、功能漫游和局部提示场景。它可以按步骤高亮不同控件，绘制带透明目标洞的遮罩，把引导卡片放在目标附近，并在布局或窗口尺寸变化后刷新高亮位置。

当前覆盖的场景：

- 多步骤导航：上一步、下一步、完成、关闭。
- 每一步绑定不同目标控件，也支持无目标居中说明。
- 按步骤配置卡片方向、遮罩、箭头、高亮间距、圆角和样式类型。
- 设置 `IsShowMask="False"` 可作为非模态提示使用。
- 支持封面内容、自定义操作按钮、圆点指示器和文本进度指示器。
- 目标晚一点出现时可以延迟解析。
- 目标位于 `Menu`、`Popup`、`Flyout` 等弹层里时仍可定位。
- 通过 `StepOpening` 或打开命令，在解析步骤前展开菜单、切换页签或准备业务状态。

基础用法：

```xml
<Grid>
    <StackPanel Orientation="Horizontal" Spacing="10">
        <Button x:Name="UploadButton" Content="上传文件" />
        <Button x:Name="SaveButton" Content="保存变更" />
        <Button x:Name="MoreButton" Content="更多操作" />
    </StackPanel>

    <codewf:Guide x:Name="BasicGuide" Placement="Bottom" PopupOffset="14">
        <codewf:GuideStep
            Target="{Binding ElementName=UploadButton}"
            Title="上传文件"
            Description="把本地文件加入处理队列。" />
        <codewf:GuideStep
            Target="{Binding ElementName=SaveButton}"
            Placement="Right"
            Title="保存变更"
            Description="保存当前工作区。" />
        <codewf:GuideStep
            Target="{Binding ElementName=MoreButton}"
            Placement="Top"
            Title="更多操作"
            Description="继续展开导出、复制或批处理。" />
    </codewf:Guide>
</Grid>
```

```csharp
BasicGuide.GoTo(0);
BasicGuide.Show();
```

菜单项这类动态目标需要在解析子项前打开父菜单，并给弹层布局留一点延迟：

```xml
<codewf:Guide
    x:Name="DynamicGuide"
    TargetResolveDelay="00:00:00.220"
    StepOpening="DynamicGuide_OnStepOpening">
    <codewf:GuideStep
        Target="{Binding ElementName=GuideThemeMenu}"
        Title="主题色菜单" />
    <codewf:GuideStep
        Target="{Binding ElementName=GuideThemeBlueItem}"
        Placement="RightBottom"
        Title="蓝色主题" />
</codewf:Guide>
```

```csharp
private void DynamicGuide_OnStepOpening(object? sender, GuideStepEventArgs e)
{
    GuideThemeMenu.IsSubMenuOpen = e.Index is >= 1 and <= 3;
    Dispatcher.UIThread.Post(
        () => GuideThemeMenu.IsSubMenuOpen = true,
        DispatcherPriority.Background);
}
```

## 示例工程

- `CodeWF.AvaloniaControlsDemo`：可运行控件展示工程，包含 Transfer、VComboBox、TabControl、Guide、StatusBadge、StatusCard、自定义窗口和 AnimatedImage 示例。

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

- 变更记录统一维护在根目录 `UpdateLog.md`，不再拆分工程级更新日志。

## 开源约束

- 仓库明确避免使用商业版 NuGet 包

## 第三方开源组件审计

检查时间：2026-05-20。检查范围包括 NuGet 元数据、恢复后的 `project.assets.json`、NuGet.org 信息以及上游源码/许可证链接。优先接受 MIT / Apache-2.0 / BSD。

本次整改：

- 将 `Semi.Avalonia.AvaloniaEdit` 替换为开源 `Avalonia.AvaloniaEdit`。
- 移除 `AvaloniaUI.DiagnosticsSupport`，因为该 NuGet 包未公开明确的开源许可证和源码仓库。

| 包 | 协议 | 源码/项目地址 | 结论 |
| --- | --- | --- | --- |
| `Avalonia` / `Avalonia.Desktop` / `Avalonia.Fonts.Inter` / `Avalonia.Themes.Fluent` | MIT | https://github.com/AvaloniaUI/Avalonia | 通过 |
| `Avalonia.AvaloniaEdit` | MIT | https://github.com/AvaloniaUI/AvaloniaEdit | 通过 |
| `AnimatedImage.Avalonia` | Apache-2.0 | https://github.com/whistyun/AnimatedImage | 通过 |
| `CodeWF.LogViewer.Avalonia` | MIT | https://github.com/dotnet9/CodeWF.LogViewer | 自研开源包 |
| `CommunityToolkit.Mvvm` | MIT | https://github.com/CommunityToolkit/dotnet | 通过 |
| `Irihi.Ursa.Themes.Semi` | MIT | https://github.com/irihitech/Ursa.Avalonia | 通过 |
| `Lang.Avalonia.Json` | MIT | https://github.com/dotnet9/Lang.Avalonia | 自研开源包 |
| `ReactiveUI.Avalonia` | MIT | https://github.com/reactiveui/reactiveui | 通过 |
| `Semi.Avalonia` | MIT | https://github.com/irihitech/Semi.Avalonia | 通过，仅使用开源主体包 |
| `VC-LTL` | EPL-2.0 | https://github.com/Chuyu-Team/VC-LTL5 | 源码开放，按“非优先但可追溯”规则通过 |
| `YY-Thunks` | MIT | https://github.com/Chuyu-Team/YY-Thunks | 通过 |

传递依赖检查结论：Avalonia / SkiaSharp / ANGLE 链路均有公开源码，许可证为 MIT 或 BSD-style。有效项目文件中未再发现 `Semi.Avalonia.AvaloniaEdit`、`Semi.Avalonia.Dock`、`Semi.Avalonia.ProDataGrid` 或 `AvaloniaUI.DiagnosticsSupport`。

## 演示

### Guide

![](https://img1.dotnet9.com/2026/05/codewf-guide-demo-basic.gif)

![](https://img1.dotnet9.com/2026/05/codewf-guide-demo-nonmask.gif)

### Transfer

![](docs/Images/Transfer.gif)

### ComboBox

![](docs/Images/ComboBox.gif)

### TabControl

![](docs/Images/TabControl.gif)
## 包版本维护约定

XML 文件统一使用两个空格缩进。`Directory.Packages.props` 统一承载 NuGet 中央包管理开关和包版本变量，包括 `AvaloniaVersion` 等共享版本属性；`Directory.Build.props` 仅保留项目构建、编译选项和 NuGet 元数据。仓库如引用 `VC-LTL`、`YY-Thunks`，这两个兼容旧版操作系统的特殊包必须使用最新预览版。

# 更新日志

## 12.0.4.11 (2026-06-08)

- 🔨[优化]-补齐根目录 logo.svg、logo.png、logo.ico 三件套，子工程通过 MSBuild Link 引用根 logo，避免维护多份图标副本。
- 🔨[优化]-统一目标框架：NuGet 包项目支持 `net8.0;net10.0`，Demo、App、测试与内部应用项目升级到 `net11.0` / `net11.0-windows`。
- 🔨[优化]-保留运行时帮助、Markdown 示例、内置备忘录和业务设计文档，仅收敛仓库级重复文档入口。

## 12.0.4.10 (2026-06-08)

- 统一版本号维护入口，只在仓库根目录 `Directory.Build.props` 中定义 `<Version>`。
- 清理英文/双语文档入口，后续仅维护简体中文文档。
- 完善 NuGet 发布配置，补充 Source Link、符号包和标签格式规范。


## 12.0.3.8 - 2026-05-24

- 将控件库与主题库版本提升到 12.0.3.8。
- 新增专用 `GuideArrow` 控件和可配置的目标区域描边渲染，使 Guide 高亮目标更容易识别。
- 扩展中英文 README，补充主题注册、控件概览、Guide 用法、动态菜单引导说明和 Guide 演示动图。
- 更新 Guide 需求文档，补充当前实现状态、源码位置和后续跟进项。

## 12.0.3.7 - 2026-05-24

- 将控件库与主题库版本提升到 12.0.3.7。
- 优化 Guide 箭头对齐逻辑，使边缘对齐模式下箭头能指向目标中心区域。
- 修正 Guide 在切换到 anchor/gravity 定位后的弹出方向分组。

## 12.0.3.6 - 2026-05-23

- 🔴[修复]-修复 Guide 在 `RightTop`、`RightBottom`、`LeftTop`、`TopLeft` 等边缘对齐模式下的弹出位置，按具体模式映射 Avalonia 的 anchor/gravity 组合。
- 🔨[优化]-保留现有箭头对齐规则，配合弹出重力修正后箭头会重新指向目标区域。

## 12.0.3.5 - 2026-05-23

- 将控件库与主题库版本提升到 12.0.3.5。
- 将 Guide 卡片定位切换为 Avalonia anchor/gravity 方式，并让菜单、Flyout 等动态目标中的导航按钮先于 popup light-dismiss 响应。
- 为 Guide 强调操作按钮补充明暗主题下的悬浮和按下状态资源。

## 12.0.3.4 - 2026-05-23

- 将控件库与主题库版本提升到 12.0.3.4。
- 新增 `Guide` 新手引导控件，支持 XAML 步骤、步骤数据源、有目标/无目标步骤、遮罩、位置模式、指示器、自定义操作和目标延迟解析。
- 新增 Guide 展示页面，覆盖基础引导、非模态提示、自定义高亮区域和动态 `MenuItem` 引导示例。

## 12.0.3.3 - 2026-05-20

- 将控件库与主题库版本提升到 12.0.3.3。
- 示例工程将 `Semi.Avalonia.AvaloniaEdit` 替换为源码开放的 `Avalonia.AvaloniaEdit`。
- 从示例工程移除 `AvaloniaUI.DiagnosticsSupport`，因为该包未公开明确的开源许可证和源码仓库。
- 在根目录中英文 README 中补充第三方开源组件审计说明。

## 12.0.3.2 - 2026-05-15

- 将控件库与主题库版本提升到 12.0.3.2。
- 修正 `CodeWFWindow` 在自定义标题栏高度下的控制按钮布局，使其始终贴近右上角。

## 12.0.3.1 - 2026-05-14

- 将控件库与主题库版本提升到 12.0.3.1。
- 更新中央包版本，包含 Avalonia 12.0.3 和 YY-Thunks 1.2.2-Beta.2。
- 调整默认 `CodeWFWindow` 标题栏布局，使控制按钮保持在右上控制区，并让 `RightContent` 紧贴在控制按钮左侧。
- 将默认 `CodeWFWindow` 标题栏改为透明，移除默认标题栏分隔线，并移除标题栏内容区为控制按钮预留的右侧内边距。
- 收紧默认 `CodeWFTitleBar` 左侧内容间距，并将 `CodeWFWindow` 窗体边框绘制改为显式的主题化覆盖边框。

## 12.0.2.12 - 2026-05-12

- 将控件库与主题库版本提升到 12.0.2.12，用于发布主题资源组织方式调整。
- 将 CodeWF 主题资源按控件拆分到 shared、light、dark 主题字典中，目录结构与 Semi 的资源组织方式保持一致。
- 将 `CodeWFWindow`、`CodeWFTitleBar`、`SearchListBox`、`StatusBadge`、`StatusCard` 和异型 TabItem 默认值改为使用 CodeWF 自有主题 Key，便于黑白主题适配。
- 将 `StatusCard` 改为通过状态伪类和主题资源设置状态背景与前景色，不再依赖固定色值转换器。
- 优化 Showcase 中 `VComboBox`、`TabControl`、`StatusBadge`、`StatusCard`、`Transfer` 和窗体示例，使示例更聚焦并更接近专业业务界面。
- 为 Showcase 新增英文、日文、简体中文和繁体中文的补充文案，并完成黑白主题下的页面截图验证。
- 精简重复窗体示例入口，使 Window 页面更聚焦于原生基线、Ursa 参考窗体和 CodeWFWindow 示例。

## 12.0.2.11 - 2026-05-12

- 将默认 `CodeWFWindow` 与 `CodeWFTitleBar` 主题值对齐 Semi/Ursa 的窗口资源，例如 `WindowDefaultBackground`、`WindowDefaultForeground`、`TitleBarBackground` 和 `CaptionButtonForeground`。
- 将托管窗体控制按钮的尺寸、内边距、圆角、前景、悬浮、按下、禁用和关闭按钮状态改为使用 Semi 资源 Key。
- 将 `CodeWFWindow` 默认标题栏高度改为 32，使托管标题栏与 Semi 的常规标题栏从同一基线开始，同时仍支持每个窗体单独覆盖。
- 为托管窗体控制按钮补充 Avalonia 窗口装饰元素角色，使模板结构更接近 Semi 和 Ursa 的窗口模板。

## 12.0.2.10 - 2026-05-11

- 将控件库与主题库版本提升到 12.0.2.10，用于发布 `CodeWFWindow` 标题栏组合能力更新。
- 新增独立的 `CodeWFTitleBar` 标题栏控件，提供 `LeftContent`、`CenterContent` 和 `RightContent` 插槽；未设置自定义左侧内容时默认显示 `Icon + Title`。
- 重构默认 `CodeWFWindow` 模板，使标题栏覆盖在内容层之上，跨标题栏背景和跨标题栏控件可以更自然地组合。
- 为 `CodeWFWindow` 新增 `ContentExtendsIntoTitleBar`，支持内容有意绘制到托管标题栏下方的布局。
- 完善标准 `CodeWFWindow` 展示示例，加入默认标题、自定义左侧内容、中间内容、右侧内容、标题栏边距和内容延伸的实时开关。
- 新增服务台 `CodeWFWindow` 展示示例，使用共享顶部蒙版图片，并通过 `CodeWF.LogViewer.Avalonia` 展示日志。
- 更新中央包版本，包含 `CodeWF.LogViewer.Avalonia`、`Lang.Avalonia.Json` 和 `YY-Thunks`。

## 12.0.2.9 - 2026-05-09

- 将控件库与主题库版本提升到 12.0.2.9，用于发布 `CodeWFWindow` 模板修复。
- 为默认 `CodeWFWindow` 模板补回并命名 `VisualLayerManager`，确保 `OverlayPopups=true` 时 `ComboBox`、下拉层和其他弹出控件可以找到窗口弹层宿主。
- 将默认 `CodeWFWindow` 前景色接入 `SemiGrey9`，避免深色主题表面中的普通内容文字继承黑色而不可读。
- 为 `CodeWFWindow` 新增与 Ursa 兼容的标题栏属性，支持控制按钮可见性、标题栏可见性、托管缩放热区、标题栏内容和标题栏边距。
- 调整 `CodeWFWindow` 最大化按钮与标题栏双击行为，使其遵守 `CanMaximize`，同时保留 Win7/Linux 下的托管标题栏实现。
- 为标准 `CodeWFWindow` 展示示例新增交互控制项，可实时切换新增标题栏属性并观察效果。
- 提升默认 `CodeWFWindow` 控制按钮悬浮与按下背景对比度，使最小化和最大化按钮反馈更明显。
- 将标准 `CodeWFWindow` 展示示例的标题栏开关改为 ViewModel 绑定，不再使用直接点击事件。
- 修复展示馆标题栏语言选择框的闭合态布局，使语言名称与区域码共用同一文本基线，同时保持下拉选项布局不变。
- 将各库和示例应用的更新日志条目统一汇总到根目录中英文更新日志文件。

## 12.0.2.4 - 2026-05-08

- 将 `CodeWF.AvaloniaControls.ProDataGrid` 及其 ProDataGrid 示例应用迁移到独立的 `CodeWF.AvaloniaControls.ProDataGrid` 仓库。
- 从 `CodeWF.AvaloniaControls` 解决方案、根目录打包脚本、示例发布脚本和中央包版本列表中移除 ProDataGrid 工程。
- 将 `CodeWF.AvaloniaControls.Dock` 及其 Dock 示例应用迁移到独立的 `CodeWF.AvaloniaControls.Dock` 仓库。
- 从 `CodeWF.AvaloniaControls` 解决方案、根目录打包脚本、示例发布脚本和中央包版本列表中移除 Dock 工程。
- 将 `CodeWF.Markdown`、`CodeWF.Markdown.Themes`、`CodeWF.Markdown.Lite`、`CodeWF.Markdown.Lite.Themes` 及其示例应用迁移到独立的 `CodeWF.Markdown` 仓库。
- 从 `CodeWF.AvaloniaControls` 解决方案、根目录打包脚本和示例发布脚本中移除 Markdown 工程。
- 将旧版 `CodeWF.AvaloniaControls.DataGrid` 包及其 DataGrid/TreeDataGrid 示例迁移到独立的 `CodeWF.AvaloniaControls.DataGrid` 仓库。
- 更新仓库文档，使当前仓库只描述 Avalonia 控件库和展示馆示例线。
- 新增 `CodeWFWindow`，作为托管自定义窗体基类，提供标题栏拖动、控制按钮和缩放热区，以统一 Win7 与 Linux 下的展示馆窗体行为。
- 将 `CodeWFWindow` 圆角渲染改为使用透明托管窗体外壳，不再依赖原生窗口区域。
- 调整默认 `CodeWFWindow` 模板，加入外层边框、内缩内容面板、统一的右/下边框和更平滑的小圆角。
- 新增通用圆角内缩转换器，用于嵌套圆角的 `CodeWFWindow` 表面。
- 将展示馆主窗体从 `UrsaWindow` 迁移到 `CodeWFWindow`，让内容区由基础窗体模板排在标题栏下方，不再需要手动偏移。
- 新增明暗主题切换、控件名称页签搜索，以及基于 JSON 的动态本地化。
- 重整窗体示例：`UrsaWindow` 和 `CodeWFWindow` 只演示常规窗体风格，无边框、可拖动、自定义标题栏和异形窗体示例改为使用原生 `Window`。
- 将标准 `UrsaWindow` 示例调整为与常规 `CodeWFWindow` 示例一致的风格，并删除重复的紧凑工具标题栏示例。
- 新增原生 `Window` 示例，覆盖无边框、可拖动、深色标题栏、高标题栏、圆形、星形和自由形状窗体。
- 新增 `UrsaWindow` 强调色标题栏示例，使 Ursa 与 CodeWF 的常规示例集合保持一致。
- 将标准 `CodeWFWindow` 示例改为接近 GitHub Desktop 的窄标题栏风格，控制按钮严格靠右，并添加标准桌面菜单。
- 移除 `CodeWFWindow` 示例标题栏控制按钮的鼠标悬浮提示文字。
- 优化标题栏菜单选中、展开、悬浮和按下状态，确保下拉菜单文字可读。
- 改进 `CodeWFWindow` 控制按钮，确保最小化图标可见，并让关闭按钮悬浮/按下状态使用符合预期的红色背景。
- 修正展示馆和起步示例中的 Avalonia Alpha 色值顺序，使半透明徽章和窗体控制按钮按预期对比度显示。

## 12.0.2.3 - 2026-05-06

- 新增 `CodeWF.Markdown.Lite`、`CodeWF.Markdown.Lite.Themes` 和 `CodeWF.Markdown.Lite.Sample`，作为轻量级 Markdown 包与示例应用。
- 从 Lite Markdown 控件线中移除 SVG 渲染、多语言资源、CSharpMath 和 TextMate 依赖；代码块与数学内容改为轻量文本回退显示。
- 为 `CodeWF.Markdown.Lite.Sample` 增加 Win7 AOT 兼容配置，包括 `YY-Thunks`、`VC-LTL`、Windows 6.1 目标元数据和 Win7 兼容应用清单。
- 将 `CodeWF.Markdown.Lite.Sample` 加入 Markdown 发布脚本和解决方案。
- 在 `CodeWF.AvaloniaControls.Showcase` 中新增 `AnimatedImage.Avalonia` GIF 示例，覆盖本地与网络图片源。

## 12.0.2.1 - 2026-05-05

- 将 `CodeWF.Markdown` 和 `CodeWF.Markdown.Themes` 提升到 12.0.2.1。
- 移除 `MarkdownViewer.Value` 兼容 API，使 `MarkdownViewer` 只通过 `Markdown` 接收 Markdown 文本。
- 新增 Markdown SVG 图片渲染，以及本地图片缺失时的文本回退。
- 扩展 README 文档，补充 Markdown 包安装、`MarkdownViewer` XAML 用法、排版主题注册、运行时主题切换和图片路径处理。
- 为 Markdown 查看器、代码块工具渲染事件参数、主题入口点和排版主题键新增 XML 文档。
- 新增独立的 `publish_Markdown.bat` 发布脚本，并将其加入解决方案文件。
- 移除 ProDataGrid 交叉行列展示视图模型中未使用的 `GroupItems` 兼容集合。

## 12.0.2 - 2026-05-05

- 将包版本拆分为五组独立维护的版本：controls/themes、DataGrid、Dock、ProDataGrid 和 Markdown/themes。
- 将 Markdown 包加入根目录打包脚本，使 `CodeWF.Markdown` 和 `CodeWF.Markdown.Themes` 与其他类库包一起输出。
- 在 Markdown 示例编辑器与预览面板之间新增 `GridSplitter`，用于实时布局测试。
- 改进 Markdown 渲染，覆盖底部内边距、长链接换行、Markdig 任务/引用节点、增量压力测试和代码块选择可读性。
- 移除 `MarkdownViewer.BasePath` 文件路径 API，使查看器表面保持由 Markdown 文本驱动。
- 增强 Markdown 示例，新增中文增量替换、插入和追加压力场景。
- 将 Markdown 示例加入一键发布脚本，提供 Windows/Linux 发布配置和 `net10.0-windows` 目标。
- 将 `CodeWF.AvaloniaControls` 拆分为核心控件包和新的 `CodeWF.AvaloniaControls.Themes` 模板/资源包。
- 将 `StatusBadge` 从已废弃的 `CodeWF.Core` / `CodeWF.Themes` 包线迁移到 `CodeWF.AvaloniaControls`。
- 从解决方案和脚本中移除已废弃的 `CodeWF.Core`、`CodeWF.Themes` 和 `CodeWF.Themes.StatusBadgeDemo` 工程。
- 将 `StatusBadge` 示例加入 `CodeWF.AvaloniaControls.Showcase`。

## 12.0.2 - 2026-05-02

- 仓库切换为中央 NuGet 包管理，使用 `Directory.Packages.props`。
- 解决方案迁移到 `.slnx`，SDK 选择交给本机或 CI 环境，不再维护 `global.json`。
- 仓库基线升级到 .NET 11 和 Avalonia 12 包生态。
- 将仓库重组为更适合开源的目录结构：所有工程物理上位于 `src/` 下，示例应用在 `.slnx` 中按逻辑分组。
- 新增 `CodeWF.AvaloniaControls.ProDataGrid` 包，以及 `CodeWF.AvaloniaControls.ProDataGridShowcase` 和 `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo` 示例工程。
- 将示例应用重命名为更清晰的开源名称，例如 `CodeWF.AvaloniaControls.Showcase`、`CodeWF.AvaloniaControls.ProDataGridShowcase`、`CodeWF.AvaloniaControls.FluentStarterDemo` 和 `CodeWF.Themes.StatusBadgeDemo`。
- 新增独立的 `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo` 示例，用于 Avalonia 12 大表格页签与文档切换验证。
- 新增根目录 `pack.bat` 脚本，用于一键还原、构建和 NuGet 打包。
- 新增 `publish_all.bat` 和 `publishbase.bat`，用于一键发布示例。
- 在仓库各工程中新增独立 `UpdateLog.md` 文件。
- 通过 `Directory.Build.props` 和 `Directory.Build.targets` 统一公共打包元数据，以及根 README/UpdateLog 注入。

## 11.3.14.1 - 2026-04-29

- 修复 DataGrid 工具提示列索引处理，避免无效列/索引访问。
- 修复 `Transfer` 的 `SearchListBox` 内部 `owner` 属性处理。
- 修复 `RangeObservableCollection.Clear` 重复触发重置通知的问题。
- 修复梯形页签项边框在顶部位置的路径渲染问题。
- 改进 `DockReactiveUIDemo` 进程嵌入流程中的清理与超时处理。

## 11.2.1.9 - 2025-07-15

- 新增 `CodeWF.AvaloniaControls.DataGrid`。

## 11.3.0.12 - 2025-07-14

- 新增 `CodeWF.AvaloniaControls.Dock`。

## 0.1.0.0 - 2025-06-28

- 新增最初的 `CodeWF.AvaloniaControls` TabControl 扩展包。
## 2026-06-08 仓库规范整理

- 统一文档维护入口：每个仓库只保留根目录 `README.md` 和根目录 `UpdateLog.md`，清理重复日志、英文文档和语言切换入口。
- 统一版本维护入口：包版本只在仓库根目录 `Directory.Build.props` 的 `<Version>` 节点维护，移除散落的程序集版本配置。
- 不再维护 `global.json`，SDK 选择交给本机或 CI 环境；NuGet 包和应用的目标框架在项目文件中明确声明。
- 统一 NuGet 包文档入口：包 README 统一引用仓库根 `README.md`，更新日志统一引用仓库根 `UpdateLog.md`。
